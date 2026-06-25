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
    public partial class ViewBookingsadminform : ResponsiveFormBase
    {
        public ViewBookingsadminform()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void ViewBookings_Load(object sender, EventArgs e)
        {
            // ADD COLUMNS
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("bookingid", "Booking ID");
                dataGridView1.Columns.Add("customername", "Customer Name");
                dataGridView1.Columns.Add("model", "Car Model");
                dataGridView1.Columns.Add("bookingdate", "Booking Date");
                dataGridView1.Columns.Add("paymentmethod", "Payment Method");
                dataGridView1.Columns.Add("status", "Status");

                // GRID STYLE
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

                // SAMPLE DATA
                dataGridView1.Rows.Add("1", "Asima", "Fortuner", "14-03-2026", "Cash", "Booked");
                dataGridView1.Rows.Add("2", "Ashaa", "Civic", "15-03-2026", "Card", "Cancelled");
                dataGridView1.Rows.Add("3", "Sara", "Corolla", "16-03-2026", "Cash", "Booked");
            

        }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
           
        
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "status")
                {
                    if (e.Value != null)
                    {
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
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
