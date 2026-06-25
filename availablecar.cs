using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace AutoNova_Car_Showroom
{
    public partial class availablecar : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        public availablecar()
        {
            InitializeComponent();
        }

        private void availablecar_Load(object sender, EventArgs e)
        {
            load_cars();

            // GRID BACKGROUND
            dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            dataGridView1.BorderStyle = BorderStyle.None;

            // GRID LINES
            dataGridView1.GridColor = Color.White;

            // COLUMN SIZE
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // HIDE ROW HEADER
            dataGridView1.RowHeadersVisible = false;

            // HEADER STYLE
            dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            dataGridView1.BorderStyle = BorderStyle.None;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 40;

            // HEADER STYLE
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ROW STYLE
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            dataGridView1.RowTemplate.Height = 35;

            // TEXT ALIGN CENTER
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "availability")
            {
                if (e.Value != null && e.Value.ToString() == "Available")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void load_cars()
        {
            NpgsqlConnection con = new NpgsqlConnection(con_string);
            try
            {
                con.Open();
                // Use "AS" to give columns a user-friendly display name
                string query = @"SELECT 
                    registration_id AS ""Reg#"", 
                    color AS ""Color"", 
                    model AS ""Model"", 
                    year AS ""Year"", 
                    brand AS ""Brand"", 
                    engine_type AS ""Engine"", 
                    price AS ""Price"", 
                    availability AS ""Status"" 
                 FROM cars
                            where is_active = TRUE";

                NpgsqlDataAdapter sda = new NpgsqlDataAdapter(query, con);

                // Now when you fill your DataTable, the column headers will be the Aliases
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occurred " + ex.Message);
            }
        }
    }
}