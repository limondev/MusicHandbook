using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpotifyExplode.Tracks;

namespace MusicHandbook
{
    public partial class SearchForm : Form
    {
        private readonly SongsClass _songsClass;
        private List<Track> _tracks;
        private readonly YourSongList _yourSongList;

        public SearchForm()
        {
            InitializeComponent();
            _yourSongList = new YourSongList(); 
            _songsClass = new SongsClass();
            dataGridView1.AutoGenerateColumns = false;
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn
            {
                Name = "Add song",
                HeaderText = "Add song",
                Text = "Add song",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(buttonColumn);
            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
      
        }

        private async void Search()
        {
            string query = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("You can't search if the search bar is empty");
                return;
            }

            dataGridView1.Rows.Clear();
            _tracks = await _songsClass.SearchTracksAsync(query);

            foreach (var track in _tracks)
            {
                var youtubeLink = await _songsClass.GetYouTubeLinkAsync(track);
                dataGridView1.Rows.Add("Track", track.Title, string.Join(", ", track.Artists.Select(a => a.Name)), track.Url, youtubeLink, "");
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Add song"].Index && e.RowIndex != -1)
            {
                _songsClass.AddSongToRepository(dataGridView1.Rows[e.RowIndex], e.RowIndex);
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

        private void Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var yoursonglist = new YourSongList();
            yoursonglist.Show();
            Visible = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Exit();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var addSongForm = new AddSongForm(_songsClass, _yourSongList);
            addSongForm.ShowDialog();
        }
    }
}