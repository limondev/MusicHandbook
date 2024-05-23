using SpotifyExplode;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MusicHandbook
{
    public partial class Table : DataGridView
    {
        public Table()
        {
            InitializeComponent();

            Columns.Add("Type", "Type");
            Columns.Add("Name", "Name");
            Columns.Add("Artist", "Artist");
            Columns.Add("URL", "URL");
            Columns.Add("YouTube URL", "YouTube URL");
            Columns.Add("Score", "Score");

            CellBeginEdit += DataGridView1_CellBeginEdit;
            CellContentClick += DataGridView1_CellContentClick;
            CellMouseEnter += DataGridView_CellMouseEnter;
            CellMouseLeave += DataGridView_CellMouseLeave;
            CellFormatting += DataGridView1_CellFormatting;
            CellEndEdit += DataGridView1_CellEndEdit;
        }



        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (Columns[e.ColumnIndex].Name != "Score")
            {
                e.Cancel = true;
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == Columns["URL"].Index && e.RowIndex != -1) || (e.ColumnIndex == Columns["YouTube URL"].Index && e.RowIndex != -1))
            {
                var url = Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
        private void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == Columns["URL"].Index && e.RowIndex != -1) || (e.ColumnIndex == Columns["YouTube URL"].Index && e.RowIndex != -1))
            {
                Cursor = Cursors.Hand;
            }
        }

        private void DataGridView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == Columns["URL"].Index && e.RowIndex != -1) || (e.ColumnIndex == Columns["YouTube URL"].Index && e.RowIndex != -1))
            {
                e.CellStyle.ForeColor = Color.Blue;
                e.CellStyle.Font = new Font(Font, FontStyle.Underline);
            }
        }
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (Columns[e.ColumnIndex].Name == "Score")
            {
                var cellValue = Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                if (int.TryParse(cellValue, out int score))
                {
                    if (score < 1 || score > 10)
                    {
                        MessageBox.Show("Please enter a score between 1 and 10", "Invalid Score", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid integer score", "Invalid Score", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
                }
            }
        }
    }
}
