using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;
using System.Security.Principal;

namespace AutoNova_Car_Showroom
{
    public partial class HandleEmployes : ResponsiveFormBase
    {
        string empEmail, empName, empPass, empRole;
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        public HandleEmployes()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // 1. Capture and Clean Input
            // Using .ToLower() and .Trim() ensures we match the DB record exactly
            string targetEmail = textemail.Text.Trim().ToLower();
            string newName = textname.Text.Trim().ToLower();
            string newRole = textrole.Text;
            string newPass = textpassword.Text;

            // 2. Validation: Don't allow an update if the email field is empty
            if (string.IsNullOrEmpty(targetEmail))
            {
                MessageBox.Show("Please enter the Email of the record you wish to update.");
                return;
            }

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();

                    // 3. The SQL Command
                    // We only update if the email matches AND the account is still active
                    string sql = @"UPDATE employees 
                           SET name = @name, 
                               role = @role, 
                               password = @pass 
                           WHERE email = @email AND is_active = true";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                    {
                        // Mapping parameters
                        cmd.Parameters.AddWithValue("@name", newName);
                        cmd.Parameters.AddWithValue("@role", newRole);
                        cmd.Parameters.AddWithValue("@pass", newPass);
                        cmd.Parameters.AddWithValue("@email", targetEmail);

                        // 4. Execution and Feedback
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Success! The profile for {targetEmail} has been updated.");
                        }
                        else
                        {
                            // If 0 rows affected, either the email doesn't exist or it's 'is_active = false'
                            MessageBox.Show("Update failed. Email not found or account is deactivated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during update: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string targetEmail = textemail.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(targetEmail))
            {
                MessageBox.Show("Please enter the Email of the employee you wish to deactivate.");
                return;
            }

            // 2. Safety First: Ask for Confirmation
            DialogResult confirm = MessageBox.Show($"Are you sure you want to deactivate the account for {targetEmail}?",
                                                   "Confirm Deactivation",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                    {
                        con.Open();

                        // 3. The SQL Command (Soft Delete)
                        string sql = "UPDATE employees SET is_active = false WHERE email = @email";

                        using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@email", targetEmail);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Employee account deactivated successfully.");
                                // Optional: Clear the form after deletion
                                textname.Clear();
                                textemail.Clear();
                            }
                            else
                            {
                                MessageBox.Show("No active account found with that email.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void HandleEmployes_Load(object sender, EventArgs e)
        {
            // ADD COLUMNS
            NpgsqlConnection con = new NpgsqlConnection(con_string);
            try
            {
                con.Open();

                // 1. Updated query for the employees table
                // Using double quotes for aliases with spaces
                string query = @"SELECT 
                        employee_id AS ""ID"", 
                        name AS ""Name"", 
                        email AS ""Email"",
                        password AS ""Pin"",
                        role AS ""Role"", 
                        created_at AS ""Date""
                     FROM employees
                     WHERE is_active = true 
                     ORDER BY employee_id ASC";

                NpgsqlDataAdapter sda = new NpgsqlDataAdapter(query, con);

                // 2. Fill the data table
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // 3. Bind to the DataGridView
                dataGridView1.DataSource = dt;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Occurred: " + ex.Message);
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

            // HEADER STYLE
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 40;

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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1. Data Cleaning & Assignment
            empName = textname.Text.Trim().ToLower(); // Standardizing to lowercase
            empEmail = textemail.Text.Trim().ToLower();
            empPass = textpassword.Text; // In a real app, hash this before saving!
            empRole = textrole.Text;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();

                    // --- ADD / INSERT COMMAND ---
                    string sqlInsert = @"INSERT INTO employees (name, email, password, role) 
                                 VALUES (@name, @email, @pass, @role)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sqlInsert, con))
                    {
                        cmd.Parameters.AddWithValue("@name", empName);
                        cmd.Parameters.AddWithValue("@email", empEmail);
                        cmd.Parameters.AddWithValue("@pass", empPass);
                        cmd.Parameters.AddWithValue("@role", empRole);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Employee " + empName + " added successfully!");
                    }
                }
            }
            catch (PostgresException ex) when (ex.SqlState == "23505") // Unique Violation (Email)
            {
                MessageBox.Show("Error: The email '" + empEmail + "' is already in use.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message);
            }
        }
    }
}
