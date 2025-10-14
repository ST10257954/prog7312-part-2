using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp
{
    public partial class EventsForm : Form
    {
        private readonly SortedDictionary<DateTime, List<Event>> eventsByDate = new();

        public EventsForm()
        {
            InitializeComponent();
            Load += EventsForm_Load;
        }

        private void EventsForm_Load(object sender, EventArgs e)
        {
            // --- Demo events ---
            AddEvent("Heritage Day Celebration", new DateTime(2025, 9, 24), "Cultural", "Parade and food market.");
            AddEvent("Heritage Business Expo", new DateTime(2025, 10, 25), "Business", "Support local businesses.");
            AddEvent("Job Fair", new DateTime(2025, 10, 28), "Employment", "Meet local companies hiring youth.");
            AddEvent("Community Health Day", new DateTime(2025, 10, 30), "Health", "Free screening & nutrition tips.");
            AddEvent("Community Clean-Up", new DateTime(2025, 11, 5), "Environment", "Join us to clean local parks.");
            AddEvent("Youth Coding Bootcamp", new DateTime(2025, 11, 15), "Education", "Intro to coding for beginners.");
            AddEvent("Recycling Awareness Week", new DateTime(2025, 11, 20), "Environment", "Learn to recycle smartly.");
            AddEvent("Sports Day at the Stadium", new DateTime(2025, 11, 22), "Sports", "Family-friendly sports day.");
            AddEvent("Holiday Food Drive", new DateTime(2025, 12, 5), "Charity", "Donate food & support families.");
            AddEvent("Art in the Park", new DateTime(2025, 12, 10), "Arts", "Outdoor art exhibition & workshops.");

            DisplayEvents();
            btnShowAll.Visible = false;
        }

        private void AddEvent(string title, DateTime date, string category, string description)
        {
            if (!eventsByDate.ContainsKey(date))
                eventsByDate[date] = new List<Event>();
            eventsByDate[date].Add(new Event(title, date, category, description));
        }

        private void DisplayEvents(List<Event>? list = null)
        {
            dgvEvents.Rows.Clear();
            var display = list ?? eventsByDate.SelectMany(kv => kv.Value);

            foreach (var ev in display)
                dgvEvents.Rows.Add(ev.Title, ev.Date.ToShortDateString(), ev.Category, ev.Description);

            lblRecommendations.Text = list == null ? "Showing all events." : "Filtered results displayed below.";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword)) return;

            // --- Search for matches ---
            List<Event> results = eventsByDate
                .SelectMany(x => x.Value)
                .Where(ev => ev.Title.ToLower().Contains(keyword)
                          || ev.Category.ToLower().Contains(keyword)
                          || ev.Description.ToLower().Contains(keyword))
                .ToList();

            DisplayEvents(results);
            GenerateRecommendations(keyword, results);
            btnShowAll.Visible = true;
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            DisplayEvents();
            lblRecommendations.Text = "Showing all events.";
            btnShowAll.Visible = false;
        }

        private void GenerateRecommendations(string keyword, List<Event> currentResults)
        {
            var allEvents = eventsByDate.SelectMany(kv => kv.Value).ToList();

            // --- Step 1: Determine dominant category among search results ---
            string? mainCategory = currentResults
                .GroupBy(e => e.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            // --- Step 2: Find similar events ---
            List<Event> related = new();

            if (!string.IsNullOrEmpty(mainCategory))
            {
                // Priority 1: Events from the same category but not already shown
                related = allEvents
                    .Where(e => !currentResults.Contains(e)
                             && e.Category.Equals(mainCategory, StringComparison.OrdinalIgnoreCase))
                    .Take(3)
                    .ToList();
            }

            // Priority 2: If still less than 3, add keyword-based matches
            if (related.Count < 3)
            {
                var keywordMatches = allEvents
                    .Where(e => !currentResults.Contains(e)
                             && (e.Title.ToLower().Contains(keyword)
                              || e.Description.ToLower().Contains(keyword)
                              || e.Category.ToLower().Contains(keyword)))
                    .Except(related)
                    .Take(3 - related.Count)
                    .ToList();
                related.AddRange(keywordMatches);
            }

            // --- Step 3: Display nicely ---
            if (related.Count == 0)
            {
                lblRecommendations.Text = "You might also like:\nNo other related events found.";
                return;
            }

            lblRecommendations.Text = "You might also like:\n" +
                string.Join("\n", related.Select(r => $"- {r.Title} ({r.Category}) on {r.Date:d}"));
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var main = new MainMenuForm();
            main.ShowDialog();
            this.Close();
        }
    }
}
