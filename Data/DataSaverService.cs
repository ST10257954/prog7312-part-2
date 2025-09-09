using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Data
{
    public static class DataSaverService
    {
        public static bool DataSaverEnabled { get; set; } = true;
        public static bool OfflineMode { get; set; } = false;

        // Offline queue  (microsoft, 2025).
        public static readonly List<Issue> Pending = new();

        // Rough size estimate for UI feedback
        public static long EstimateBytes(Issue issue, bool afterCompression)
        {
            long bytes = 0;

            // text
            bytes += Encoding.UTF8.GetByteCount(issue.Location ?? "");
            bytes += Encoding.UTF8.GetByteCount(issue.Description ?? "");
            bytes += Encoding.UTF8.GetByteCount((issue.Category).ToString());

            // attachments
            foreach (var p in issue.AttachmentPaths ?? Enumerable.Empty<string>())
            {
                try
                {
                    var fi = new FileInfo(p);
                    if (!fi.Exists) continue;

                    if (afterCompression && IsImage(fi.FullName))
                        bytes += (long)Math.Max(5_000, fi.Length * 0.15);   // assume ~85% reduction
                    else
                        bytes += fi.Length;
                }
                catch { /* ignore */ }
            }

            return bytes;
        }

        public static bool IsImage(string path)
        {
            var ext = Path.GetExtension(path)?.ToLowerInvariant();
            return ext is ".jpg" or ".jpeg" or ".png" or ".bmp";
        }

        // Downscale + JPEG re-encode  (Microsoft, 2025)
        public static string CompressImageFile(string inputPath, string outputDir, int maxWidth = 1024, long quality = 55L)
        {
            Directory.CreateDirectory(outputDir);

            using var img = Image.FromFile(inputPath);
            int w = img.Width, h = img.Height;

            if (w > maxWidth)
            {
                h = (int)Math.Round(h * (maxWidth / (double)w));
                w = maxWidth;
            }

            using var resized = new Bitmap(img, new Size(w, h));

            var jpgEncoder = ImageCodecInfo.GetImageDecoders()
                                           .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
            var enc = System.Drawing.Imaging.Encoder.Quality;
            var encParams = new EncoderParameters(1);
            encParams.Param[0] = new EncoderParameter(enc, new long[] { quality });

            var outPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + "_cmp.jpg");

            if (jpgEncoder != null)
                resized.Save(outPath, jpgEncoder, encParams);
            else
                resized.Save(outPath, ImageFormat.Jpeg); // fallback

            return outPath;
        }

        // Compact, SMS-friendly body  (Microsoft, 2025)
        public static string BuildCompactSms(Issue issue)
        {
            string desc = (issue.Description ?? "").Replace("\r", " ").Replace("\n", " ");
            if (desc.Length > 80) desc = desc[..80] + "…";
            var s = $"MS {issue.TicketNumber} | {issue.Category} | {issue.Location} | {desc}";
            if (s.Length > 160) s = s[..160];
            return s;
        }

        public static void QueueForLater(Issue issue) => Pending.Add(issue);

        public static void ExportPendingToJson(string path)
        {
            var json = JsonSerializer.Serialize(Pending, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static void ExportPendingToCsv(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Ticket,Category,Location,Description,Attachments");

            foreach (var i in Pending)
            {
                string esc(string? v) => (v ?? "").Replace("\"", "\"\"");
                var atts = string.Join(" | ", i.AttachmentPaths ?? new List<string>());
                sb.AppendLine($"\"{esc(i.TicketNumber)}\",\"{i.Category}\",\"{esc(i.Location)}\",\"{esc(i.Description)}\",\"{esc(atts)}\"");
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}
/*References
Microsoft, 2025. Best Practices for the TableLayoutPanel Control. [Online]
Available at: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/best-practices-for-the-tablelayoutpanel-control
[Accessed 07 September 2025].
microsoft, 2025.Tutorial: Create a Windows Forms app in Visual Studio with C#. [Online] 
Available at: https://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022
[Accessed 05 September 2025]. */

