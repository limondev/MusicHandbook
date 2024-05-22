using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public partial class YourSongList : Form
    {
        public List<ScoredSong> Songs { get; private set; }

        public YourSongList()
        {
            InitializeComponent();
            Songs = new List<ScoredSong>();

            DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
            buttonColumn1.Name = "Delete song";
            buttonColumn1.HeaderText = "Delete song";
            buttonColumn1.Text = "Delete song";
            buttonColumn1.UseColumnTextForButtonValue = true;
            table1.Columns.Add(buttonColumn1);

            table1.CellClick += DataGridView_CellClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainmenu = new MainMenu();
            mainmenu.Show();
            Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var searchform = new SearchForm();
            searchform.Show();
            Visible = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }

        private void LoadSongsFromFile()
        {
            if (File.Exists("songs.json"))
            {
                string json = File.ReadAllText("songs.json");
                Songs = JsonConvert.DeserializeObject<List<ScoredSong>>(json);
                LoadSongsToDataGridView();
            }
        }

        private void LoadSongsToDataGridView()
        {
            table1.Rows.Clear();

            foreach (var track in Songs)
            {
                table1.Rows.Add("Track", track.Title, string.Join(", ", track.Artists.Select(a => a.Name)), track.Url, track.Score);
            }
        }

        private void SaveSongsToFile()
        {
            string json = JsonConvert.SerializeObject(Songs, Formatting.Indented);
            File.WriteAllText("songs.json", json);
        }

        private void YourSongList_Load(object sender, EventArgs e)
        {
            LoadSongsFromFile();
        }

        private void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (table1.Columns[e.ColumnIndex].Name == "URL" && e.RowIndex >= 0)
            {
                table1.Cursor = Cursors.Hand;
            }
        }

        private void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            table1.Cursor = Cursors.Default;
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && table1.Columns[e.ColumnIndex].Name == "Delete song")
            {
                string songTitle = table1.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                var songToRemove = Songs.FirstOrDefault(song => song.Title == songTitle);
                if (songToRemove != null)
                {
                    Songs.Remove(songToRemove);

                    SaveSongsToFile();

                    LoadSongsToDataGridView();
                }
            }
        }
    }
}



