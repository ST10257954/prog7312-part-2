using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    partial class MainMenuForm
    {
        private Panel pnlCard;
        private Label lblHeader;
        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnStatus;
        private Label lblFooter;

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlCard = new Panel();
            this.lblHeader = new Label();
            this.btnReportIssues = new Button();
            this.btnLocalEvents = new Button();
            this.btnStatus = new Button();
            this.lblFooter = new Label();
            this.SuspendLayout();

            // === FORM ===
            this.ClientSize = new Size(720, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Municipal Services Portal";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            this.Paint += MainMenuForm_Paint;
            this.BackColor = Color.White;

            // === HEADER (Emerald Banner) ===
            this.lblHeader.Text = "Municipal Services Portal";
            this.lblHeader.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            this.lblHeader.ForeColor = Color.White;
            this.lblHeader.Size = new Size(480, 70);
            this.lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            this.lblHeader.BackColor = Color.Transparent;
            this.lblHeader.Location = new Point((720 - 480) / 2, 90);
            this.lblHeader.Paint += lblHeader_Paint;

            // === CARD ===
            this.pnlCard.Size = new Size(480, 250);
            this.pnlCard.Location = new Point((720 - 480) / 2, 170);
            this.pnlCard.Paint += pnlCard_Paint;
            this.pnlCard.BackColor = Color.Transparent;

            Font btnFont = new Font("Segoe UI", 12F);
            Color baseColor = Color.White;
            Color hoverColor = Color.FromArgb(240, 245, 240);
            Size btnSize = new Size(440, 55);

            // Report Issues
            this.btnReportIssues.FlatStyle = FlatStyle.Flat;
            this.btnReportIssues.FlatAppearance.BorderSize = 0;
            this.btnReportIssues.Font = btnFont;
            this.btnReportIssues.ForeColor = Color.FromArgb(40, 40, 40);
            this.btnReportIssues.BackColor = baseColor;
            this.btnReportIssues.Size = btnSize;
            this.btnReportIssues.Location = new Point(20, 25);
            this.btnReportIssues.Text = "📋  Report Issues";
            this.btnReportIssues.TextAlign = ContentAlignment.MiddleLeft;
            this.btnReportIssues.Padding = new Padding(25, 0, 0, 0);
            this.btnReportIssues.MouseEnter += (s, e) => this.btnReportIssues.BackColor = hoverColor;
            this.btnReportIssues.MouseLeave += (s, e) => this.btnReportIssues.BackColor = baseColor;
            this.btnReportIssues.Click += btnReportIssues_Click;

            // Local Events
            this.btnLocalEvents.FlatStyle = FlatStyle.Flat;
            this.btnLocalEvents.FlatAppearance.BorderSize = 0;
            this.btnLocalEvents.Font = btnFont;
            this.btnLocalEvents.ForeColor = Color.FromArgb(40, 40, 40);
            this.btnLocalEvents.BackColor = baseColor;
            this.btnLocalEvents.Size = btnSize;
            this.btnLocalEvents.Location = new Point(20, 95);
            this.btnLocalEvents.Text = "📅  Local Events  Announcements";
            this.btnLocalEvents.TextAlign = ContentAlignment.MiddleLeft;
            this.btnLocalEvents.Padding = new Padding(25, 0, 0, 0);
            this.btnLocalEvents.MouseEnter += (s, e) => this.btnLocalEvents.BackColor = hoverColor;
            this.btnLocalEvents.MouseLeave += (s, e) => this.btnLocalEvents.BackColor = baseColor;
            this.btnLocalEvents.Click += btnLocalEvents_Click;

            // Status (Disabled)
            this.btnStatus.FlatStyle = FlatStyle.Flat;
            this.btnStatus.FlatAppearance.BorderSize = 0;
            this.btnStatus.Font = btnFont;
            this.btnStatus.ForeColor = Color.Gray;
            this.btnStatus.BackColor = baseColor;
            this.btnStatus.Enabled = false;
            this.btnStatus.Size = btnSize;
            this.btnStatus.Location = new Point(20, 165);
            this.btnStatus.Text = "🕓  Service Request Status (coming soon)";
            this.btnStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.btnStatus.Padding = new Padding(25, 0, 0, 0);

            // === FOOTER ===
            this.lblFooter.Font = new Font("Segoe UI", 9F);
            this.lblFooter.ForeColor = Color.FromArgb(120, 120, 120);
            this.lblFooter.Text = "© 2025 City of Tshwane | Powered by Municipal Portal";
            this.lblFooter.Dock = DockStyle.Bottom;
            this.lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            this.lblFooter.Height = 40;

            // Add buttons
            this.pnlCard.Controls.Add(this.btnReportIssues);
            this.pnlCard.Controls.Add(this.btnLocalEvents);
            this.pnlCard.Controls.Add(this.btnStatus);

            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pnlCard);
            this.Controls.Add(this.lblFooter);

            this.ResumeLayout(false);
        }

        // Header Paint (Rounded Emerald Banner)
        private void lblHeader_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, lblHeader.Width, lblHeader.Height);
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 20;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom);
                path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
                path.CloseFigure();

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    Color.FromArgb(25, 90, 35),
                    Color.FromArgb(60, 130, 60),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }

            TextRenderer.DrawText(e.Graphics, lblHeader.Text, lblHeader.Font,
                rect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // Card shadow + white fill
        private void pnlCard_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(rect.X - 10, rect.Y - 10, rect.Width + 20, rect.Height + 30);
                using (PathGradientBrush pgb = new PathGradientBrush(gp))
                {
                    pgb.CenterColor = Color.FromArgb(60, 0, 0, 0);
                    pgb.SurroundColors = new Color[] { Color.Transparent };
                    e.Graphics.FillPath(pgb, gp);
                }
            }

            int radius = 18;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                using (SolidBrush brush = new SolidBrush(Color.White))
                    e.Graphics.FillPath(brush, path);
            }
        }

        // Background gradient
        private void MainMenuForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(245, 247, 240),
                Color.FromArgb(228, 237, 227),
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
