using System;
using System.Collections.Generic;
using MunicipalServicesApp.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp.Data
{
    public static class IssueRepository
    {
        // In-memory store for now (meets “use appropriate data structures”) (microsoft, 2025).
        public static readonly List<Issue> Issues = new List<Issue>();
    }
}
/*References
microsoft, 2025.Tutorial: Create a Windows Forms app in Visual Studio with C#. [Online] 
Available at: https://learn.microsoft.com/en-us/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022
[Accessed 05 September 2025]. */