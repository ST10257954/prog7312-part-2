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
        // ---- Data Structures ----
        private readonly SortedDictionary<DateTime, List<Event>> eventsByDate = new();
        private readonly HashSet<string> eventCategories = new();
        private readonly Queue<string> recentSearches = new();
        private readonly PriorityQueue<Event, int> recommendations = new();
        private readonly Stack<Event> viewedEvents = new(); // track recently viewed

        public EventsForm()
        {
            InitializeComponent();
            this.Load += EventsForm_Load;
        }

        private void EventsForm_Load(object sender, EventArgs e)
        {
            // theme setup
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10);

            // preload events
            AddEvent("Community Clean-Up", new DateTime(2025, 11, 5),
                "Environment", "Join us to clean local parks.");
            AddEvent("Heritage Day Celebration", new DateTime(2025, 9, 24),
                "Cultural", "Parade and food market.");
            AddEvent("Job Fair", new DateTime(2025, 10, 28),
                "Employment", "Meet local companies hiring youth.");
            AddEvent("Art in the Park", new DateTime(2025, 12, 10),
                "Arts", "Outdoor art exhibition and workshops.");

            DisplayEvents();
        }

        private void AddEvent(string title, DateTime date, string category, string description)
        {
            if (!eventsByDate.ContainsKey(date))
                eventsByDate[date] = new List<Event>();

            Event ev = new(title, date, category, description);
            eventsByDate[date].Add(ev);
            eventCategories.Add(category);
        }

        private void DisplayEvents(List<Event>? list = null)
        {
            dgvEvents.Rows.Clear();
            var display = list ?? eventsByDate.SelectMany(kv => kv.Value);

            foreach (var ev in display)
            {
                dgvEvents.Rows.Add(ev.Title, ev.Date.ToShortDateString(), ev.Category, ev.Description);
            }
            lblSummary.Text = $"{dgvEvents.Rows.Count} events displayed.";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword)) return;

            recentSearches.Enqueue(keyword);
            if (recentSearches.Count > 5) recentSearches.Dequeue();

            List<Event> results = new();

            foreach (var pair in eventsByDate)
            {
                foreach (var ev in pair.Value)
                {
                    if (ev.Title.ToLower().Contains(keyword) ||
                        ev.Category.ToLower().Contains(keyword) ||
                        ev.Description.ToLower().Contains(keyword))
                    {
                        results.Add(ev);
                    }
                }
            }

            DisplayEvents(results);
            GenerateRecommendations(keyword);
        }

        private void GenerateRecommendations(string keyword)
        {
            recommendations.Clear();
            foreach (var pair in eventsByDate)
            {
                foreach (var ev in pair.Value)
                {
                    int score = 0;
                    if (ev.Category.ToLower().Contains(keyword)) score += 3;
                    if (ev.Title.ToLower().Contains(keyword)) score += 2;
                    if (ev.Description.ToLower().Contains(keyword)) score += 1;
                    if (score > 0) recommendations.Enqueue(ev, -score);
                }
            }

            if (recommendations.Count == 0)
            {
                lblRecommendations.Text = "No recommendations found.";
                return;
            }

            lblRecommendations.Text = "You might also like:\n";
            int count = 0;
            while (recommendations.Count > 0 && count < 3)
            {
                Event rec = recommendations.Dequeue();
                lblRecommendations.Text += $"- {rec.Title} ({rec.Category}) on {rec.Date:d}\n";
                count++;
            }
        }

        private void dgvEvents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var ev = new Event(
                    dgvEvents.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    DateTime.Parse(dgvEvents.Rows[e.RowIndex].Cells[1].Value.ToString()),
                    dgvEvents.Rows[e.RowIndex].Cells[2].Value.ToString(),
                    dgvEvents.Rows[e.RowIndex].Cells[3].Value.ToString());
                viewedEvents.Push(ev);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
