namespace MunicipalServicesApp
{
    partial class EventsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.Label lblRecommendations;
        private System.Windows.Forms.Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            header = new Panel();
            lblHeader = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnShowAll = new Button();
            dgvEvents = new DataGridView();
            lblRecommendations = new Label();
            btnBack = new Button();

            // --- Form ---
            this.Text = "Local Events & Announcements";
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(920, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // --- Header ---
            header.Dock = DockStyle.Top;
            header.Height = 70;
            header.BackColor = Color.FromArgb(27, 94, 32);
            lblHeader.Text = "Municipal Services — Local Events & Announcements";
            lblHeader.ForeColor = Color.White;
            lblHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblHeader.Dock = DockStyle.Fill;
            header.Controls.Add(lblHeader);

            // --- Search Box ---
            txtSearch.Location = new Point(40, 90);
            txtSearch.Size = new Size(480, 32);
            txtSearch.Font = new Font("Segoe UI", 11);
            txtSearch.PlaceholderText = "Search by keyword, category, or date...";

            // --- Search Button ---
            btnSearch.Text = "Search";
            btnSearch.Font = new Font("Segoe UI Semibold", 11);
            btnSearch.BackColor = Color.FromArgb(46, 125, 50);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Size = new Size(100, 32);
            btnSearch.Location = new Point(530, 90);
            btnSearch.Click += btnSearch_Click;

            // --- Show All Button ---
            btnShowAll.Text = "Show All Events";
            btnShowAll.Font = new Font("Segoe UI Semibold", 11);
            btnShowAll.BackColor = Color.FromArgb(27, 94, 32);
            btnShowAll.ForeColor = Color.White;
            btnShowAll.FlatStyle = FlatStyle.Flat;
            btnShowAll.Size = new Size(150, 32);
            btnShowAll.Location = new Point(640, 90);
            btnShowAll.Visible = false;
            btnShowAll.Click += btnShowAll_Click;

            // --- DataGridView ---
            dgvEvents.Location = new Point(40, 140);
            dgvEvents.Size = new Size(800, 350);
            dgvEvents.AllowUserToAddRows = false;
            dgvEvents.ReadOnly = true;
            dgvEvents.RowHeadersVisible = false;
            dgvEvents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEvents.Columns.Add("Title", "Title");
            dgvEvents.Columns.Add("Date", "Date");
            dgvEvents.Columns.Add("Category", "Category");
            dgvEvents.Columns.Add("Description", "Description");

            // --- Recommendations ---
            lblRecommendations.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblRecommendations.ForeColor = Color.FromArgb(60, 60, 60);
            lblRecommendations.Location = new Point(40, 510);
            lblRecommendations.Size = new Size(700, 80);
            lblRecommendations.Text = "Recommendations will appear here...";

            // --- Back to Main Menu ---
            btnBack.Text = "Back to Main Menu";
            btnBack.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnBack.BackColor = Color.FromArgb(46, 125, 50);
            btnBack.ForeColor = Color.White;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Size = new Size(160, 32);
            btnBack.Location = new Point(720, 580);
            btnBack.Click += btnBack_Click;

            Controls.AddRange(new Control[]
            {
                header, txtSearch, btnSearch, btnShowAll, dgvEvents, lblRecommendations, btnBack
            });
        }
    }
}
