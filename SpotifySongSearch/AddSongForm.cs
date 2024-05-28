using SpotifyExplode.Artists;
using System;
using System.Windows.Forms;

namespace MusicHandbook
{
    public partial class AddSongForm : Form
    {
        private readonly SongsClass _songsClass;
        private readonly YourSongList _yourSongList;
        private readonly SongRepository _songRepository;

        public AddSongForm(SongsClass songsClass, YourSongList yourSongList)
        {
            InitializeComponent();
            _songsClass = songsClass;
            _yourSongList = yourSongList;
            _songRepository = new SongRepository();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text.Trim();
            string artist = textBox2.Text.Trim();
            int score;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(artist) || !int.TryParse(textBox5.Text, out score) || score > 10 || score < 0)
            {
                MessageBox.Show("Please enter valid song details.");
                return;
            }

            var song = new ScoredSong
            {
                Title = title,
                Artists = new List<Artist> { new Artist { Name = artist } },
                TrackId = null,
                YouTubeUrl = null,
                Score = score
            };

            _songRepository.Save(song);
            _yourSongList.LoadSongsToDataGridView();

            MessageBox.Show("Song added successfully!");
            Close();


        }

     

    }
}
