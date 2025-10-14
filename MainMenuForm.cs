using System;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            using (var form = new ReportIssueForm())
                form.ShowDialog();
        }

        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
            using (var form = new EventsForm())
                form.ShowDialog();
        }
    }
}
