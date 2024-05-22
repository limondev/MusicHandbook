using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using SpotifyExplode;
using SpotifyExplode.Search;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public partial class SearchForm : Form
    {
        private readonly SpotifyClient _spotify;
        private YourSongList _yourSongList;
        private List<Track> _tracks = [];
        public SearchForm()
        {
            InitializeComponent();
            _spotify = new SpotifyClient();
            _yourSongList = new YourSongList();

            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "Add song";
            buttonColumn.HeaderText = "Add song";
            buttonColumn.Text = "Add song";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;

        }

        private async void Search()
        {
            string query = textBox1.Text.Trim();
            dataGridView1.Rows.Clear();
            _tracks.Clear();
            foreach (var result in await _spotify.Search.GetResultsAsync(query))
            {
                switch (result)
                {
                    case TrackSearchResult track:
                        dataGridView1.Rows.Add("Track", track.Title, string.Join(", ", track.Artists.Select(a => a.Name)), track.Url, "");
                        _tracks.Add(track);
                        break;
                    case AlbumSearchResult album:
                        dataGridView1.Rows.Add("Album", album.Name, string.Join(", ", album.Artists.Select(a => a.Name)), album.Url, "");
                        break;
                }
            }

        }

        private void SaveSongsToJson(ScoredSong track)
        {
            List<ScoredSong> songs;
            if (File.Exists("songs.json"))
            {
                var json = File.ReadAllText("songs.json");
                songs = JsonConvert.DeserializeObject<List<ScoredSong>>(json);
            }
            else
            {
                songs = new List<ScoredSong>();
            }
            songs.Add(track);
            var newjson = JsonConvert.SerializeObject(songs);
            File.WriteAllText("songs.json", newjson);
        }


        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Add song"].Index && e.RowIndex != -1)
            {
                var song = new ScoredSong() { 
                    Album = _tracks[e.RowIndex].Album,
                    Title = _tracks[e.RowIndex].Title,
                    Artists = _tracks[e.RowIndex].Artists,
                    Id = _tracks[e.RowIndex].Id,
                    Score = int.Parse((string)dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Score"].Index].Value),

            };
             
                SaveSongsToJson(song);
                MessageBox.Show("Song added to your list!", "Song Added");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "URL" && e.RowIndex >= 0)
            {
                e.CellStyle.ForeColor = Color.Blue;
                e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Underline);
            }
        }

        private void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "URL" && e.RowIndex >= 0)
            {
                dataGridView1.Cursor = Cursors.Hand;
            }
        }

        private void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Cursor = Cursors.Default;
        }

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (key == Keys.Enter)
            {
                Search();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var yoursonglist = new YourSongList();
            yoursonglist.Show();
            Visible = false;
        }
    }
}
