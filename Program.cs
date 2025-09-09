using System;

namespace MunicipalServicesApp
{
    internal static class Program
    {
        // Windows Forms entry point  (microsoft, 2025).
        [STAThread] // single-threaded apartment required for WinForms/clipboard/dialogs
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenuForm()); // launch the main menu
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