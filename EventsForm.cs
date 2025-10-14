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
        // === Core structures ===
        private readonly SortedDictionary<DateTime, List<Event>> eventsByDate = new();
        private readonly Queue<string> searchHistory = new();      // remembers last 5 search keywords
        private readonly Stack<Event> recentlyViewed = new();       // tracks recently viewed events
        private readonly HashSet<string> uniqueCategories = new();  // ensures unique event categories

        public EventsForm()
        {
            InitializeComponent();
            Load += EventsForm_Load;
        }

        /// <summary>
        /// Loads demo events and sets up the DataGridView click handler.
        /// </summary>
        private void EventsForm_Load(object sender, EventArgs e)
        {
            // --- Demo data ---
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

            // handle cell clicks for recently viewed events
            dgvEvents.CellClick += DgvEvents_CellClick;

            DisplayEvents();
            btnShowAll.Visible = false;
        }

        /// <summary>
        /// Adds a new event and stores it by date and category.
        /// </summary>
        private void AddEvent(string title, DateTime date, string category, string description)
        {
            if (!eventsByDate.ContainsKey(date))
                eventsByDate[date] = new List<Event>();
            eventsByDate[date].Add(new Event(title, date, category, description));

            uniqueCategories.Add(category); // maintain unique set
        }

        /// <summary>
        /// Displays either all events or a filtered list.
        /// </summary>
        private void DisplayEvents(List<Event>? list = null)
        {
            dgvEvents.Rows.Clear();
            var display = list ?? eventsByDate.SelectMany(kv => kv.Value);

            foreach (var ev in display)
                dgvEvents.Rows.Add(ev.Title, ev.Date.ToShortDateString(), ev.Category, ev.Description);

            lblRecommendations.Text = list == null ? "Showing all events." : "Filtered results displayed below.";
        }

        /// <summary>
        /// Handles Search button clicks, updating UI and data structures.
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Please enter a search term.", "Input Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // keep only last 5 searches
            if (searchHistory.Count >= 5)
                searchHistory.Dequeue();
            searchHistory.Enqueue(keyword);

            // perform search
            var results = eventsByDate
                .SelectMany(kv => kv.Value)
                .Where(ev => ev.Title.ToLower().Contains(keyword)
                          || ev.Category.ToLower().Contains(keyword)
                          || ev.Description.ToLower().Contains(keyword))
                .ToList();

            if (results.Count == 0)
            {
                MessageBox.Show("No matching events found.", "Search",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblRecommendations.Text = "";
                return;
            }

            // push found items onto recently viewed stack
            foreach (var ev in results)
                recentlyViewed.Push(ev);

            DisplayEvents(results);
            GenerateRecommendations(keyword, results);
            btnShowAll.Visible = true;

            MessageBox.Show($"{results.Count} event(s) found.", "Search Results",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Restores full event list and clears search box.
        /// </summary>
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            DisplayEvents();
            lblRecommendations.Text = "Showing all events.";
            btnShowAll.Visible = false;
        }

        /// <summary>
        /// Generates smart recommendations based on category, keyword, and trending searches.
        /// </summary>
        private void GenerateRecommendations(string keyword, List<Event> currentResults)
        {
            var allEvents = eventsByDate.SelectMany(kv => kv.Value).ToList();

            // Step 1: determine dominant category among search results
            string? mainCategory = currentResults
                .GroupBy(e => e.Category)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            // Step 2: find related events
            List<Event> related = new();

            if (!string.IsNullOrEmpty(mainCategory))
            {
                related = allEvents
                    .Where(e => !currentResults.Contains(e)
                             && e.Category.Equals(mainCategory, StringComparison.OrdinalIgnoreCase))
                    .Take(3)
                    .ToList();
            }

            // Step 3: include keyword-based matches if fewer than 3
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

            // Step 4: use trending search history if still less than 3
            var frequentTerm = searchHistory.GroupBy(s => s)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(frequentTerm) && related.Count < 3)
            {
                var trending = allEvents
                    .Where(e => !currentResults.Contains(e)
                             && (e.Title.ToLower().Contains(frequentTerm)
                              || e.Category.ToLower().Contains(frequentTerm)))
                    .Except(related)
                    .Take(3 - related.Count);
                related.AddRange(trending);
            }

            // Step 5: display
            if (related.Count == 0)
            {
                lblRecommendations.Text = "You might also like:\nNo other related events found.";
                return;
            }

            lblRecommendations.Text = "You might also like:\n" +
                string.Join("\n", related.Select(r => $"- {r.Title} ({r.Category}) on {r.Date:d}"));
        }

        /// <summary>
        /// Tracks when a user clicks an event row (adds to Stack).
        /// </summary>
        private void DgvEvents_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvEvents.Rows.Count)
            {
                string title = dgvEvents.Rows[e.RowIndex].Cells["Title"].Value?.ToString();
                var ev = eventsByDate.Values.SelectMany(v => v)
                    .FirstOrDefault(x => x.Title == title);
                if (ev != null)
                {
                    recentlyViewed.Push(ev);
                    MessageBox.Show($"Viewed: {ev.Title}", "Event Viewed",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Returns to Main Menu.
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            var main = new MainMenuForm();
            main.ShowDialog();
            this.Close();
        }
    }
}
