using Npgsql;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class carbooking : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        public List<string> reg_lis = new List<string>();
        public int id;
        public carbooking()
        {
            InitializeComponent();
        }

        private void carbooking_Load(object sender, EventArgs e)
        {
            loadcars();
            //dateTimePicker1.CalendarMonthBackground = Color.FromArgb(154, 160, 166);
            //dateTimePicker1.CalendarTitleBackColor = Color.FromArgb(154, 160, 166);
            //dateTimePicker1.CalendarTitleForeColor = Color.Black;
            //dateTimePicker1.CalendarForeColor = Color.Black;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void loadcars()
        {
                NpgsqlConnection conn = new NpgsqlConnection(con_string);
                conn.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("Select registration_id from cars where is_Active = True AND availability = 'yes'", conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reg_lis.Add(reader.GetString(0));
                }
                reader.Close();
                conn.Close();
                
                foreach(string ids in  reg_lis)
                {
                    comboBox1.Items.Add(ids);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Inputs validation
            if (string.IsNullOrEmpty(txt_email.Text) || string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(cb_method.Text))
            {
                MessageBox.Show("Please fill all fields (Email, Car, and Payment Method).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string buyer_mail = txt_email.Text;
            string payment_method = cb_method.Text;
            DateTime date_time = DateTime.Now;
            string car_id = comboBox1.Text;
            string status = "booked";
            decimal price = 0;

            // 'using' block ensures connection is closed even if an error occurs
            using (NpgsqlConnection con = new NpgsqlConnection(con_string))
            {
                try
                {
                    con.Open();

                    // --- 1. Get User ID from Email ---
                    using (NpgsqlCommand cmd0 = new NpgsqlCommand("SELECT user_id FROM users WHERE email = @mail", con))
                    {
                        cmd0.Parameters.AddWithValue("@mail", buyer_mail);
                        object result = cmd0.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            id = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("User email not found in database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // --- 2. Get Car Price ---
                    using (NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT price FROM cars WHERE registration_id = @reg_id", con))
                    {
                        cmd1.Parameters.AddWithValue("@reg_id", car_id);
                        object result1 = cmd1.ExecuteScalar();

                        if (result1 != null && result1 != DBNull.Value)
                        {
                            price = Convert.ToDecimal(result1);
                        }
                        else
                        {
                            MessageBox.Show("Could not retrieve car price.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // --- 3. Insert into Bookings & Sales (Transaction Logic) ---
                    // Hum aik hi Connection use kar rahe hain

                    // A. Insert into Bookings
                    string queryBooking = "INSERT INTO bookings (registration_id, buyer_id, booking_date, payment_method, status) " +
                                          "VALUES (@reg, @buy_id, @date, @pay, @status)";

                    using (NpgsqlCommand cmdBooking = new NpgsqlCommand(queryBooking, con))
                    {
                        cmdBooking.Parameters.AddWithValue("@reg", car_id);
                        cmdBooking.Parameters.AddWithValue("@buy_id", id);
                        cmdBooking.Parameters.AddWithValue("@date", date_time);
                        cmdBooking.Parameters.AddWithValue("@pay", payment_method);
                        cmdBooking.Parameters.AddWithValue("@status", status);
                        cmdBooking.ExecuteNonQuery();
                    }

                    // B. Insert into Sales
                    string querySales = "INSERT INTO sales (registration_id, buyer_id, price, sale_date, payment_method) " +
                                        "VALUES (@reg, @buy_id, @prc, @date, @pay)";

                    using (NpgsqlCommand cmdSales = new NpgsqlCommand(querySales, con))
                    {
                        cmdSales.Parameters.AddWithValue("@reg", car_id);
                        cmdSales.Parameters.AddWithValue("@buy_id", id);
                        cmdSales.Parameters.AddWithValue("@prc", price);
                        cmdSales.Parameters.AddWithValue("@date", date_time);
                        cmdSales.Parameters.AddWithValue("@pay", payment_method);
                        cmdSales.ExecuteNonQuery();
                    }

                    // If everything is successful
                    //MessageBox.Show("? Booking and Sale Processed Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optional: Form clear karne ke liye function call karein
                    // ClearFields(); 
                }
                catch (NpgsqlException ex)
                {
                    // Database specific errors (Constraint violations, connection issues)
                    MessageBox.Show("Database Error: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // General errors
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string queryUpdateCar = "UPDATE cars SET availability = 'no' WHERE registration_id = @reg";
                using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(queryUpdateCar, con))
                {
                    cmdUpdate.Parameters.AddWithValue("@reg", car_id);
                    cmdUpdate.ExecuteNonQuery();
                }

                // --- QUERY 2: Update the Bookings Table ---
                string queryUpdateBooking = "UPDATE bookings SET status = 'sold' WHERE registration_id = @reg";
                using (NpgsqlCommand cmdBooking = new NpgsqlCommand(queryUpdateBooking, con))
                {
                    cmdBooking.Parameters.AddWithValue("@reg", car_id);
                    cmdBooking.ExecuteNonQuery();
                }

                // Success Message (Iske baad Grid refresh function call karna hai)
                MessageBox.Show("? Booking and Sale Processed Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

