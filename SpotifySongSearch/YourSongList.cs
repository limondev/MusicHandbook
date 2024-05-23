using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;
using SpotifyExplode.Tracks;
using SpotifyExplode;

namespace MusicHandbook
{
    public partial class YourSongList : Form
    {
        public List<ScoredSong> Songs { get; private set; }
        private readonly SpotifyClient _spotify;

        private SongRepository songRepository = new SongRepository();  
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



        private void LoadSongsToDataGridView()
        {
            Songs = songRepository.Load();

            table1.Rows.Clear();

            foreach (var track in Songs)
            {
                table1.Rows.Add("Track", track.Title, string.Join(", ", track.Artists.Select(a => a.Name)), track.Url, track.YouTubeUrl, track.Score);
            }
        }

  

        private void YourSongList_Load(object sender, EventArgs e)
        {
            LoadSongsToDataGridView();
        }


        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && table1.Columns[e.ColumnIndex].Name == "Delete song")
            {
                string songTitle = table1.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                var songToRemove = Songs.FirstOrDefault(song => song.Title == songTitle);
                if (songToRemove != null)
                {
                    songRepository.Delete(songToRemove);
                    LoadSongsToDataGridView();
                }
            }
        }
    }
}



