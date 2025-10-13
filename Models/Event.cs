using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp.Models
{
    public class Event
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public Event(string title, DateTime date, string category, string description)
        {
            Title = title;
            Date = date;
            Category = category;
            Description = description;
        }

        public override string ToString() => $"{Title} ({Category}) on {Date:d}";
    }
}
