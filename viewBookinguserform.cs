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
    public partial class viewBookinguserform : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        public viewBookinguserform()
        {
            InitializeComponent();
        }

        private void viewbooking_Load(object sender, EventArgs e)
        {
            this.Load += viewbooking_Load;

            // Attach Events in Code
            textview.Enter += textview_Enter;
            textview.Leave += textview_Leave;

            if (dataGridView1.Columns.Count == 0)
            {
                //dataGridView1.Columns.Add("bookingid", "Booking ID");
                //dataGridView1.Columns.Add("model", "Car Model");
                //dataGridView1.Columns.Add("buyername", "Buyer Name");
                //dataGridView1.Columns.Add("bookingdate", "Booking Date");
                //dataGridView1.Columns.Add("paymentmethod", "Payment Method");
                //dataGridView1.Columns.Add("status", "Status");


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

              
            }
        
    }
        private void textview_Enter(object sender, EventArgs e)
        {
            if (textview.Text == "  Enter Booking ID / Buyer Name")
            {
                textview.Text = "";
                textview.ForeColor = Color.White;
            }
        }

        private void textview_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textview.Text))
            {
                textview.Text = "  Enter Booking ID / Buyer Name";
                textview.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }






        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
            {
                if (e.Value == null)
                    return;

                string value = e.Value.ToString();

                if (value == "Booked")
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (value == "Cancelled")
                {
                    e.CellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string buyer_mail = textview.Text;
            int id;

            using (NpgsqlConnection conn = new NpgsqlConnection(con_string))
            {
                try
                {
                    conn.Open();

                    // 1. Get User ID from Email
                    using (NpgsqlCommand cmd0 = new NpgsqlCommand("SELECT user_id FROM users WHERE email = @mail", conn))
                    {
                        cmd0.Parameters.AddWithValue("@mail", buyer_mail);
                        object result = cmd0.ExecuteScalar();

                        if (result != null)
                        {
                            id = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("User not found!");
                            return;
                        }
                    }

                    // 2. Fetch Bookings based on retrieved id
                    // Hum yahan DataTable use karenge taake DataGridView mein show ho sakay
                    string viewQuery = "SELECT * FROM bookings WHERE buyer_id = @buy_id";

                    using (NpgsqlCommand cmdView = new NpgsqlCommand(viewQuery, conn))
                    {
                        cmdView.Parameters.AddWithValue("@buy_id", id);

                        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmdView);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // 3. Display in DataGridView
                        // Farz karein aapke grid ka naam 'dataGridView1' hai
                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dt;
                        }
                        else
                        {
                            dataGridView1.DataSource = null; // Purana data clear karein
                            MessageBox.Show("No bookings found for this user.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
