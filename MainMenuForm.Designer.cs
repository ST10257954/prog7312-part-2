using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    partial class MainMenuForm
    {
        private IContainer components = null;

        // Standard dispose for designer-created components  (microsoft, 2025).
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        // Minimal designer initialization (layout is built in code-behind)  (Microsoft, 2025)
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 640);
            Name = "MainMenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Municipal Services";
            Load += MainMenuForm_Load;
            ResumeLayout(false);
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