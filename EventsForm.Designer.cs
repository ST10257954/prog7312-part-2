namespace MunicipalServicesApp
{
    partial class EventsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel cardPanel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.Label lblRecommendations;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            header = new Panel();
            lblHeader = new Label();
            cardPanel = new Panel();
            txtSearch = new TextBox();
            btnSearch = new Button();
            dgvEvents = new DataGridView();
            lblRecommendations = new Label();
            lblSummary = new Label();
            btnBack = new Button();

            // ---- Form ----
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 250);
            ClientSize = new System.Drawing.Size(900, 640);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Local Events & Announcements";

            // ---- Header ----
            header.Dock = DockStyle.Top;
            header.Height = 70;
            header.BackColor = System.Drawing.Color.FromArgb(27, 94, 32);

            lblHeader.Text = "Local Events & Announcements";
            lblHeader.ForeColor = System.Drawing.Color.White;
            lblHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblHeader.Dock = DockStyle.Fill;
            header.Controls.Add(lblHeader);
            Controls.Add(header);

            // ---- Card Panel ----
            cardPanel.BackColor = System.Drawing.Color.White;
            cardPanel.Size = new Size(820, 500);
            cardPanel.Location = new Point(40, 100);
            cardPanel.BorderStyle = BorderStyle.None;
            cardPanel.Padding = new Padding(25);
            cardPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cardPanel.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, cardPanel.Width, cardPanel.Height, 20, 20));
            Controls.Add(cardPanel);

            // ---- Search Box ----
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.PlaceholderText = "Search by keyword, category, or date...";
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Size = new Size(500, 32);
            txtSearch.Location = new Point(25, 25);

            btnSearch.Text = "Search";
            btnSearch.Font = new Font("Segoe UI Semibold", 11);
            btnSearch.BackColor = System.Drawing.Color.FromArgb(46, 125, 50);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Location = new Point(540, 25);
            btnSearch.Size = new Size(100, 32);
            btnSearch.Click += btnSearch_Click;

            // ---- DataGridView ----
            dgvEvents.Location = new Point(25, 75);
            dgvEvents.Size = new Size(760, 300);
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.ReadOnly = true;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.BackgroundColor = Color.White;
            dgvEvents.GridColor = Color.FromArgb(220, 220, 220);
            dgvEvents.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvEvents.DefaultCellStyle.ForeColor = Color.Black;
            dgvEvents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);
            dgvEvents.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgvEvents.Columns.Add("Title", "Title");
            dgvEvents.Columns.Add("Date", "Date");
            dgvEvents.Columns.Add("Category", "Category");
            dgvEvents.Columns.Add("Description", "Description");
            dgvEvents.CellClick += dgvEvents_CellClick;

            // ---- Recommendations ----
            lblRecommendations.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblRecommendations.ForeColor = Color.FromArgb(60, 60, 60);
            lblRecommendations.BackColor = Color.FromArgb(247, 247, 247);
            lblRecommendations.Location = new Point(25, 390);
            lblRecommendations.Size = new Size(760, 60);
            lblRecommendations.Text = "Recommendations will appear here...";

            // ---- Summary ----
            lblSummary.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblSummary.ForeColor = Color.Gray;
            lblSummary.Location = new Point(25, 460);
            lblSummary.Size = new Size(400, 25);

            // ---- Back Button ----
            btnBack.Text = "← Back";
            btnBack.Font = new Font("Segoe UI Semibold", 10);
            btnBack.BackColor = Color.FromArgb(27, 94, 32);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Location = new Point(680, 455);
            btnBack.Size = new Size(100, 32);
            btnBack.Click += btnBack_Click;

            // Add controls to card
            cardPanel.Controls.Add(txtSearch);
            cardPanel.Controls.Add(btnSearch);
            cardPanel.Controls.Add(dgvEvents);
            cardPanel.Controls.Add(lblRecommendations);
            cardPanel.Controls.Add(lblSummary);
            cardPanel.Controls.Add(btnBack);

            ResumeLayout(false);
        }

        // Needed for rounded panel edges
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);
    }
}
