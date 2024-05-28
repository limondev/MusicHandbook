using System;
using System.Linq;
using System.Windows.Forms;

namespace MusicHandbook
{
    public partial class YourSongList : Form
    {
        private readonly SongsClass _songsClass;

        public YourSongList()
        {
            InitializeComponent();
            InitializeDataGridView();
            _songsClass = new SongsClass();
            table1.CellClick += DataGridView_CellClick;
            table1.AutoGenerateColumns = false;
            textBox2.TextChanged += textBox2_TextChanged;
        }

        private void InitializeDataGridView()
        {
            DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn
            {
                Name = "Delete song",
                HeaderText = "Delete song",
                Text = "Delete song",
                UseColumnTextForButtonValue = true
            };
            table1.Columns.Add(buttonColumn1);
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

        public void LoadSongsToDataGridView(string filter = "")
        {
            var songs = _songsClass.LoadSongs(filter);
            table1.Rows.Clear();
            foreach (var track in songs)
            {
                table1.Rows.Add("Track", track.Title, string.Join(", ", track.Artists.Select(a => a.Name)), track.TrackId != null ? track.Url : "", track.YouTubeUrl, track.Score);
            }
            SetLastButtonText();
        }

        private void YourSongList_Load(object sender, EventArgs e)
        {
            LoadSongsToDataGridView();
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && table1.Columns[e.ColumnIndex].Name == "Delete song")
            {
                if (e.RowIndex == table1.Rows.Count - 1)
                {
                    _songsClass.ClearAllSongs();
                    LoadSongsToDataGridView();
                }
                else
                {
                    string songTitle = table1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    var songToRemove = _songsClass.Songs.FirstOrDefault(song => song.Title == songTitle);
                    if (songToRemove != null)
                    {
                        _songsClass.DeleteSong(songToRemove);
                        LoadSongsToDataGridView();
                    }
                }
            }
        }

        private void SetLastButtonText()
        {
            if (table1.Rows.Count > 0)
            {
                var lastRowIndex = table1.Rows.Count - 1;
                var lastButtonCell = table1.Rows[lastRowIndex].Cells["Delete song"] as DataGridViewButtonCell;
                if (lastButtonCell != null)
                {
                    lastButtonCell.Value = "Delete All";
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox2.Text;
            LoadSongsToDataGridView(filter);
        }


    }
}