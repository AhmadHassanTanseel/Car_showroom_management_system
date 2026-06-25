using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class ViewSales : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        public ViewSales()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ViewSales_Load(object sender, EventArgs e)
        {
            // ADD COLUMNS
            if (dataGridView1.Columns.Count == 0)
            {
                //dataGridView1.Columns.Add("saleid", "Sale ID");
                //dataGridView1.Columns.Add("model", "Car Model");
                //dataGridView1.Columns.Add("buyer", "Buyer Name");
                //dataGridView1.Columns.Add("price", "Price");
                //dataGridView1.Columns.Add("date", "Date");
                //dataGridView1.Columns.Add("payment", "Payment Method");

                //// SAMPLE DATA
                //dataGridView1.Rows.Add("1", "Civic", "Asima", "5000000", "14-03-2026", "Cash");
                //dataGridView1.Rows.Add("2", "Corolla", "Ashaa", "4500000", "15-03-2026", "Card");
                //dataGridView1.Rows.Add("3", "Sportage", "Sara", "6500000", "16-03-2026", "Bank Transfer");


            }

            // GRID BACKGROUND
            dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            dataGridView1.BorderStyle = BorderStyle.None;

            // GRID LINES
            dataGridView1.GridColor = Color.White;

            // COLUMN SIZE
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // HIDE ROW HEADER
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
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.RowTemplate.Height = 35;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();

                    // Aapki provide ki hui query
                    string sql = @"
                SELECT 
                    s.sales_id,
                    c.model AS Model,
                    c.brand AS Brand,
                    c.price AS Price, 
                    u.name AS Buyer, 
                    s.sale_date AS Date, 
                    s.payment_method AS Payment_Method
                FROM sales s
                JOIN cars c ON s.registration_id = c.registration_id
                JOIN users u ON s.buyer_id = u.user_id
                ORDER BY s.sale_date DESC";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                    {
                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                        DataTable dt = new DataTable();

                        // Data fetch kar ke DataTable mein bharna
                        da.Fill(dt);

                        // Grid ko refresh karna
                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = null; // Purana data clear karein
                            dataGridView1.DataSource = dt;   // Naya data load karein
                            da.Fill(dt);
                            //dataGridView1.DataSource = dt;

                            // Har column ke header ko Upper Case karne ke liye loop
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                col.HeaderText = col.HeaderText.ToUpper();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Database mein koi sales record majood nahi hai.");
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
