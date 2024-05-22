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
            Columns.Add("Score", "Score");

            CellBeginEdit += DataGridView1_CellBeginEdit;
            CellContentClick += DataGridView1_CellContentClick;
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
            if (e.ColumnIndex == Columns["URL"].Index && e.RowIndex != -1)
            {
                var url = Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
    }
}
