using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class ViewInventry : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";

        public ViewInventry()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ViewInventry_Load(object sender, EventArgs e)
        {
            // ADD COLUMNS
            //if (dataGridView1.Columns.Count == 0)
            //{
            //    dataGridView1.Columns.Add("carid", "Car ID");
            //    dataGridView1.Columns.Add("model", "Model");
            //    dataGridView1.Columns.Add("brand", "Brand / Name");
            //    dataGridView1.Columns.Add("price", "Price");
            //    dataGridView1.Columns.Add("color", "Color");
            //    dataGridView1.Columns.Add("year", "Year");
            //    dataGridView1.Columns.Add("engine", "Engine Type");
            //    dataGridView1.Columns.Add("availability", "Availability");

                // SAMPLE DATA
                //dataGridView1.Rows.Add("1", "Civic", "Honda", "5000000", "Black", "2022", "1800cc", "Available");
                //dataGridView1.Rows.Add("2", "Corolla", "Toyota", "4500000", "White", "2021", "1600cc", "Available");
                //dataGridView1.Rows.Add("3", "Sportage", "KIA", "6500000", "Red", "2023", "2000cc", "Not Available");
            

            // GRID STYLE
            dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            dataGridView1.BorderStyle = BorderStyle.None;

            dataGridView1.GridColor = Color.White;

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

            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        
    }

       
            private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "availability")
            {
                if (e.Value != null)
                {
                    string value = e.Value.ToString();

                    if (value == "Available")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                    else if (value == "Not Available")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
