using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Data;

namespace MunicipalServicesApp
{
    public partial class ReportIssueForm : Form
    {
        // Theme
        private static readonly Color Accent = Color.FromArgb(46, 125, 50);
        private static readonly Color AccentDark = Color.FromArgb(27, 94, 32);
        private static readonly Color Surface = Color.White;
        private static readonly Color SurfaceAlt = Color.FromArgb(246, 248, 250);
        private static readonly Color BorderColor = Color.FromArgb(220, 223, 226);
        private static readonly Color Muted = Color.FromArgb(108, 117, 125);

        // Controls
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox rtbDescription;
        private Button btnAddAttachment, btnSubmit, btnCopySms, btnExport, btnBack;
        private ListBox lstAttachments;
        private CheckBox chkDataSaver, chkOffline;
        private Label lblEstimate, lblEngagement;
        private ProgressBar progress;
        private StatusStrip status;
        private ToolStripStatusLabel statusMode, statusDataSaver;

        public ReportIssueForm()
        {
            BuildUi();
        }

        // minimal stub: this form is built fully in code
        private void InitializeComponent() { }


        // ---------- UI ----------
        private void BuildUi()
        {
            // Window setup
            Text = "Report an Issue";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1060, 640);
            BackColor = SurfaceAlt;
            Font = new Font("Segoe UI", 10.5f);
            MinimumSize = new Size(980, 600);

            // Root: Header | Content | Footer | Status
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(20, 16, 20, 16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            Controls.Add(root);

            // Header bar
            var header = new Panel { BackColor = AccentDark, Dock = DockStyle.Fill };
            var headerLbl = new Label
            {
                Text = "Municipal Services — Report an Issue",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 18, 0)
            };
            header.Controls.Add(headerLbl);
            root.Controls.Add(header, 0, 0);

            // Content = top card (details + engagement) + bottom card (attachments)  (Microsoft, 2025)
            var content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            content.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            root.Controls.Add(content, 0, 1);

            // Card 1: Details (left) + Modes/Engagement (right)  (Microsoft, 2025)
            var cardTop = MakeCard();
            content.Controls.Add(cardTop, 0, 0);

            var topGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            topGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            topGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            cardTop.Controls.Add(topGrid);

            // LEFT: Issue details
            var details = MakeCard(innerPadding: 16);
            topGrid.Controls.Add(details, 0, 0);

            var detailsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(6)
            };
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 8));
            detailsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            details.Controls.Add(detailsGrid);

            detailsGrid.Controls.Add(MakeSectionTitle("Issue details"), 0, 0);
            detailsGrid.SetColumnSpan(detailsGrid.GetControlFromPosition(0, 0), 2);

            detailsGrid.Controls.Add(MakeLabel("Address:"), 0, 1);
            txtLocation = new TextBox { Dock = DockStyle.Fill };
            txtLocation.TextChanged += (_, __) => { UpdateEngagement(); UpdateEstimate(); };
            detailsGrid.Controls.Add(txtLocation, 1, 1);

            detailsGrid.Controls.Add(MakeLabel("Category:"), 0, 2);
            cmbCategory = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.DataSource = Enum.GetValues(typeof(IssueCategory));
            cmbCategory.SelectedIndexChanged += (_, __) => { UpdateEngagement(); UpdateEstimate(); };
            detailsGrid.Controls.Add(cmbCategory, 1, 2);

            detailsGrid.Controls.Add(MakeLabel("Description:"), 0, 3);
            rtbDescription = new RichTextBox { Dock = DockStyle.Fill };
            rtbDescription.TextChanged += (_, __) => { UpdateEngagement(); UpdateEstimate(); };
            detailsGrid.Controls.Add(rtbDescription, 1, 3);

            // RIGHT: Modes & engagement (implements the chosen strategy)  (Microsoft, 2025)
            var modes = MakeCard(innerPadding: 16); 
            topGrid.Controls.Add(modes, 1, 0);

            var modesGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Padding = new Padding(6)
            };
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            modesGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            modes.Controls.Add(modesGrid);

            modesGrid.Controls.Add(MakeSectionTitle("Modes & engagement"), 0, 0);

            // Data-saver + Offline toggles (multichannel/low-data engagement)
            var toggleRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            chkDataSaver = new CheckBox { Text = "Data saver", Checked = true, AutoSize = true };
            chkOffline = new CheckBox { Text = "Work offline", AutoSize = true };
            chkDataSaver.CheckedChanged += (_, __) => { ToggleForDataSaver(); UpdateEstimate(); UpdateStatusBar(); };
            chkOffline.CheckedChanged += (_, __) => { UpdateStatusBar(); };
            toggleRow.Controls.Add(chkDataSaver);
            toggleRow.Controls.Add(new Label { Text = "   " }); // small spacer  (Microsoft, 2025)
            toggleRow.Controls.Add(chkOffline);
            modesGrid.Controls.Add(toggleRow, 0, 1);

            // Upload size hint for user awareness
            lblEstimate = new Label { Text = "Estimated upload size: ~1 KB", Dock = DockStyle.Fill, ForeColor = Muted };
            modesGrid.Controls.Add(lblEstimate, 0, 2);

            // Simple step-by-step text + progress bar as engagement indicator
            lblEngagement = new Label
            {
                Text = "Step 1: Add your location to help teams find the issue.",
                Dock = DockStyle.Fill
            };
            modesGrid.Controls.Add(lblEngagement, 0, 4);

            progress = new ProgressBar
            {
                Dock = DockStyle.Fill,
                Minimum = 0,
                Maximum = 100,
                Style = ProgressBarStyle.Continuous
            };
            modesGrid.Controls.Add(progress, 0, 5);

            // Card 2: Attachments (disabled when data saver is ON)
            var cardBottom = MakeCard();
            content.Controls.Add(cardBottom, 0, 1);

            var attachGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(16)
            };
            attachGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            attachGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            attachGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            attachGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            attachGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            cardBottom.Controls.Add(attachGrid);

            attachGrid.Controls.Add(MakeSectionTitle("Attachments"), 0, 0);
            attachGrid.SetColumnSpan(attachGrid.GetControlFromPosition(0, 0), 2);

            btnAddAttachment = new Button
            {
                Text = "Add attachment(s)",
                Dock = DockStyle.Fill,
                BackColor = Surface,
                FlatStyle = FlatStyle.Flat
            };
            btnAddAttachment.FlatAppearance.BorderColor = BorderColor;
            btnAddAttachment.FlatAppearance.BorderSize = 1;
            btnAddAttachment.Click += BtnAddAttachment_Click;
            attachGrid.Controls.Add(btnAddAttachment, 0, 1);

            lstAttachments = new ListBox { Dock = DockStyle.Fill };
            attachGrid.Controls.Add(lstAttachments, 1, 1);
            attachGrid.SetRowSpan(lstAttachments, 2);

            // Footer actions: Submit | Copy SMS | Export | Back
            var footer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 0, 0)
            };
            root.Controls.Add(footer, 0, 2);

            btnBack = Ghost("Back to Main Menu");
            btnBack.Click += (_, __) => Close();

            btnExport = Ghost("Export Pending (JSON/CSV)");
            btnExport.Click += BtnExport_Click;

            // Copies a compact template (multichannel: SMS/WhatsApp)  (Microsoft, 2025)
            btnCopySms = Ghost("Copy SMS/WhatsApp text");
            btnCopySms.Click += BtnCopySms_Click;

            btnSubmit = Primary("Submit");
            btnSubmit.Click += BtnSubmit_Click;

            footer.Controls.AddRange(new Control[] { btnBack, btnExport, btnCopySms, btnSubmit });

            AcceptButton = btnSubmit;
            CancelButton = btnBack;

            // Status bar reflects current mode
            status = new StatusStrip { SizingGrip = false, BackColor = Surface };
            statusMode = new ToolStripStatusLabel("Online");
            statusDataSaver = new ToolStripStatusLabel("Data saver: On") { ForeColor = Muted };
            status.Items.Add(statusMode);
            status.Items.Add(new ToolStripStatusLabel("   |   ") { ForeColor = BorderColor });
            status.Items.Add(statusDataSaver);
            root.Controls.Add(status, 0, 3);

            // Initial states
            ToggleForDataSaver();
            UpdateEstimate();
            UpdateEngagement();
            UpdateStatusBar();
        }

        // ---------- UI helpers ----------
        private Panel MakeCard(int innerPadding = 0)
        {
            return new Panel
            {
                BackColor = Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Padding = new Padding(innerPadding)
            };
        }

        private static Label MakeSectionTitle(string text) =>
            new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                TextAlign = ContentAlignment.MiddleLeft
            };

        private static Label MakeLabel(string text) =>
            new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(60, 72, 82)
            };

        private Button Primary(string text)
        {
            var b = new Button
            {
                Text = text,
                AutoSize = true,
                BackColor = Accent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                Margin = new Padding(8, 0, 0, 0)
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 104, 43);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(34, 90, 38);
            return b;
        }

        private Button Ghost(string text)
        {
            var b = new Button
            {
                Text = text,
                AutoSize = true,
                BackColor = Surface,
                ForeColor = Color.FromArgb(52, 58, 64),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                Margin = new Padding(8, 0, 0, 0)
            };
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = BorderColor;
            return b;
        }

        // ---------- Behaviour ----------
        private void ToggleForDataSaver()
        {
            // Data saver = text-first, so disable attachments  (Microsoft, 2025)
            btnAddAttachment.Enabled = !chkDataSaver.Checked;

            // Keep descriptions small in data saver mode
            if (chkDataSaver.Checked && rtbDescription.TextLength > 280)
                rtbDescription.Text = rtbDescription.Text[..280];
        }

        private void UpdateStatusBar()
        {
            statusMode.Text = chkOffline.Checked ? "Offline (queued)" : "Online";
            statusDataSaver.Text = $"Data saver: {(chkDataSaver.Checked ? "On" : "Off")}";
        }

        private void UpdateEngagement()
        {
            // Simple progress based on core fields filled  (Microsoft, 2025)
            int p = 0;
            if (!string.IsNullOrWhiteSpace(txtLocation.Text)) p += 25;
            if (cmbCategory.SelectedItem != null) p += 25;
            if (!string.IsNullOrWhiteSpace(rtbDescription.Text)) p += 25;
            if (lstAttachments.Items.Count > 0) p += 25;

            progress.Value = p;
            lblEngagement.Text =
                p < 25 ? "Step 1: Add your location to help teams find the issue." :
                p < 50 ? "Great! Now choose a category (water, roads, etc.)." :
                p < 75 ? "Almost there. Please describe the issue." :
                p < 100 ? "Optional: add photos/documents to speed up resolution." :
                          "Perfect! Submit to get your ticket number.";
        }

        private void UpdateEstimate()
        {
            // Size estimate reflects data-saver (compression assumption)
            var mock = new Issue
            {
                Location = txtLocation.Text.Trim(),
                Description = rtbDescription.Text.Trim(),
                Category = cmbCategory.SelectedItem is IssueCategory ic ? ic : IssueCategory.Other,
                AttachmentPaths = lstAttachments.Items.Cast<string>().ToList()
            };
            var bytes = DataSaverService.EstimateBytes(mock, afterCompression: chkDataSaver.Checked);
            lblEstimate.Text = $"Estimated upload size: ~{Math.Max(1, bytes / 1024)} KB";
        }

        // ---------- Events ----------
        private void BtnAddAttachment_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Select attachment(s)",
                Multiselect = true,
                Filter = "Images/Documents|*.jpg;*.jpeg;*.png;*.pdf;*.docx;*.xlsx|All files|*.*"
            };
            if (ofd.ShowDialog(this) != DialogResult.OK) return;

            foreach (var p in ofd.FileNames)
                lstAttachments.Items.Add(p);

            UpdateEngagement();
            UpdateEstimate();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            // Core validation
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show("Please enter the location.", "Missing information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Missing information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show("Please describe the issue.", "Missing information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Build new issue
            var issue = new Issue
            {
                TicketNumber = GenerateTicketNumber(),
                Location = txtLocation.Text.Trim(),
                Category = (IssueCategory)cmbCategory.SelectedItem,
                Description = rtbDescription.Text.Trim(),
                Channel = chkOffline.Checked ? "Offline-Queued" : (chkDataSaver.Checked ? "LowData" : "DesktopApp"),
                CreatedAt = DateTime.Now
            };

            // Attachment handling (compress images; copy other files)
            var storeRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachments");
            Directory.CreateDirectory(storeRoot);

            if (!chkDataSaver.Checked)
            {
                foreach (var p in lstAttachments.Items.Cast<string>())
                {
                    try
                    {
                        string finalPath;
                        if (DataSaverService.IsImage(p))
                        {
                            finalPath = DataSaverService.CompressImageFile(p, storeRoot, 1024, 65L);
                        }
                        else
                        {
                            var dest = Path.Combine(storeRoot, Path.GetFileName(p));
                            if (!File.Exists(dest)) File.Copy(p, dest);
                            finalPath = dest;
                        }
                        issue.AttachmentPaths.Add(finalPath);
                    }
                    catch
                    {
                        // If anything fails, keep original path as a fallback
                        issue.AttachmentPaths.Add(p);
                    }
                }
            }

            // Either queue offline or store in-memory (Part 1 requirement)  (Microsoft, 2025)
            if (chkOffline.Checked)
            {
                DataSaverService.QueueForLater(issue);
                MessageBox.Show($"Saved offline.\nTicket: {issue.TicketNumber}",
                    "Queued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                IssueRepository.Issues.Add(issue);
                MessageBox.Show(
                    $"Thank you! Your report was submitted.\n\nTicket: {issue.TicketNumber}\nCategory: {issue.Category}\nLocation: {issue.Location}",
                    "Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearForm();
            UpdateEstimate();
            UpdateStatusBar();
        }

        private void BtnCopySms_Click(object sender, EventArgs e)
        {
            // Compact, channel-agnostic message  (Microsoft, 2025)
            var text = $"REPORT | Loc: {txtLocation.Text}; Cat: {(cmbCategory.SelectedItem ?? IssueCategory.Other)}; " +
                       $"Desc: {Trim(rtbDescription.Text, 120)}";
            Clipboard.SetText(text);
            MessageBox.Show("Compact text copied. Paste it into SMS or WhatsApp.",
                "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Export Pending Reports",
                Filter = "JSON file|*.json|CSV file|*.csv",
                FileName = $"pending_{DateTime.Now:yyyyMMdd_HHmm}"
            };
            if (sfd.ShowDialog(this) != DialogResult.OK) return;

            if (Path.GetExtension(sfd.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                DataSaverService.ExportPendingToCsv(sfd.FileName);
            else
                DataSaverService.ExportPendingToJson(sfd.FileName);

            MessageBox.Show("Pending reports exported.", "Export",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string Trim(string s, int max) =>
            string.IsNullOrWhiteSpace(s) ? "" : (s.Length <= max ? s : s.Substring(0, max) + "…");

        private static string GenerateTicketNumber()
        {
            var id = Guid.NewGuid().ToString("N").ToUpper().Substring(0, 8);
            return $"MS-{id}";
        }

        private void ClearForm()
        {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = -1;
            rtbDescription.Clear();
            lstAttachments.Items.Clear();
            UpdateEngagement();
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