using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class MainMenuForm : Form
    {
        // ---- Simple theme palette (shared across the window) ----
        private static readonly Color Accent = Color.FromArgb(46, 125, 50);   // primary green
        private static readonly Color AccentDark = Color.FromArgb(27, 94, 32);    // header green
        private static readonly Color Surface = Color.White;                   // card/backgrounds
        private static readonly Color SurfaceAlt = Color.FromArgb(246, 248, 250); // page background
        private static readonly Color BorderColor = Color.FromArgb(220, 223, 226); // subtle borders
        private static readonly Color Muted = Color.FromArgb(108, 117, 125); // muted text

        public MainMenuForm()
        {
            InitializeComponent();   // keeps the designer partial class happy
            BuildMainMenuUi();       // build the whole screen in code
        }

        private void BuildMainMenuUi()
        {
            // ---- Window chrome ----
            Text = "Municipal Services";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 640);
            BackColor = SurfaceAlt;
            Font = new Font("Segoe UI", 10.5f);

            // Root: Header | Content | Spacer
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(20, 16, 20, 16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 4));
            Controls.Add(root);

            // Header
            var header = new Panel { BackColor = AccentDark, Dock = DockStyle.Fill };
            var headerLbl = new Label
            {
                Text = "Municipal Services",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 0, 18, 0)
            };
            header.Controls.Add(headerLbl);
            root.Controls.Add(header, 0, 0);

            // Center container (keeps the card centered)
            var center = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 18, 0, 0) };
            root.Controls.Add(center, 0, 1);

            // Card
            var card = new Panel
            {
                BackColor = Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Size = new Size(640, 360),
                Anchor = AnchorStyles.None
            };

            void placeCard()
            {
                card.Left = Math.Max(0, (center.ClientSize.Width - card.Width) / 2);
                card.Top = Math.Max(0, (center.ClientSize.Height - card.Height) / 2);
            }
            center.Resize += (_, __) => placeCard();
            center.Controls.Add(card);
            placeCard();

            // Card grid: small title + a 3-row panel that the buttons fill
            var cardGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(24)
            };
            cardGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));  // title
            cardGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 10));  // tiny gap
            cardGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // rows area
            card.Controls.Add(cardGrid);

            var title = new Label
            {
                Text = "Choose an action",
                Dock = DockStyle.Fill,
                ForeColor = Muted,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            cardGrid.Controls.Add(title, 0, 0);

            // 3-row panel: each row is 33% height; buttons dock-fill each row.
            var rows = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            rows.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333f));
            rows.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333f));
            rows.RowStyles.Add(new RowStyle(SizeType.Percent, 33.333f));
            cardGrid.Controls.Add(rows, 0, 2);

            // Buttons (Dock = Fill; Margin provides equal spacing)
            Button MakePrimary(string text)
            {
                var b = new Button
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 4, 0, 4),
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    BackColor = Accent,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 104, 43);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(34, 90, 38);
                return b;
            }

            Button MakeGhost(string text)
            {
                var b = new Button
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0, 4, 0, 4),
                    Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
                    BackColor = Surface,
                    ForeColor = Color.FromArgb(108, 117, 125),
                    FlatStyle = FlatStyle.Flat,
                    Enabled = false,
                    TabStop = false
                };
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = BorderColor;
                return b;
            }

            var btnReport = MakePrimary("▶  Report Issues");

            var btnEvents = MakePrimary("▶  Local Events & Announcements");
            btnEvents.Click += btnEvents_Click;

            var btnStatus = MakeGhost("Service Request Status (coming soon)");

            rows.Controls.Add(btnReport, 0, 0);
            rows.Controls.Add(btnEvents, 0, 1);
            rows.Controls.Add(btnStatus, 0, 2);

            // Open Report Issues
            btnReport.Click += (_, __) =>
            {
                using var dlg = new ReportIssueForm();
                dlg.ShowDialog(this);
            };



            AcceptButton = btnReport;
        }
        private void btnEvents_Click(object sender, EventArgs e)
        {
            EventsForm eventsForm = new EventsForm();
            eventsForm.ShowDialog();
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {

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
