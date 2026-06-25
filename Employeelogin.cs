using Npgsql;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class Employeelogin : ResponsiveFormBase
    {
        public string con_string =
            "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";

        public Employeelogin()
        {
            InitializeComponent();

            // =========================
            // PLACEHOLDERS
            // =========================

            // EMAIL
            textmail.Text = "Enter Your Email";
            textmail.ForeColor = Color.FromArgb(154, 160, 166);

            // PASSWORD
            textpassword.Text = "Enter Your Password";
            textpassword.ForeColor = Color.FromArgb(154, 160, 166);
            textpassword.UseSystemPasswordChar = false;

            // ROLE
            textrole.Text = "Enter Your Role";
            textrole.ForeColor = Color.FromArgb(154, 160, 166);

            // =========================
            // CONNECT EVENTS
            // =========================

            textmail.Enter += textname_Enter;
            textmail.Leave += textname_Leave;

            textpassword.Enter += textpassword_Enter;
            textpassword.Leave += textpassword_Leave;

            textrole.Enter += textrole_Enter;
            textrole.Leave += textrole_Leave;
        }

        private void Employeelogin_Load(object sender, EventArgs e)
        {

        }

        // =========================
        // EMAIL ENTER
        // =========================

        private void textname_Enter(object sender, EventArgs e)
        {
            if (textmail.Text == "Enter Your Email")
            {
                textmail.Text = "";
                textmail.ForeColor = Color.White;
            }
        }

        // =========================
        // EMAIL LEAVE
        // =========================

        private void textname_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textmail.Text))
            {
                textmail.Text = "Enter Your Email";
                textmail.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // =========================
        // PASSWORD ENTER
        // =========================

        private void textpassword_Enter(object sender, EventArgs e)
        {
            if (textpassword.Text == "Enter Your Password")
            {
                textpassword.Text = "";
                textpassword.ForeColor = Color.White;
                textpassword.UseSystemPasswordChar = true;
            }
        }

        // =========================
        // PASSWORD LEAVE
        // =========================

        private void textpassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textpassword.Text))
            {
                textpassword.UseSystemPasswordChar = false;

                textpassword.Text = "Enter Your Password";

                textpassword.ForeColor =
                    Color.FromArgb(154, 160, 166);
            }
        }

        // =========================
        // ROLE ENTER
        // =========================

        private void textrole_Enter(object sender, EventArgs e)
        {
            if (textrole.Text == "Enter Your Role")
            {
                textrole.Text = "";
                textrole.ForeColor = Color.White;
            }
        }

        // =========================
        // ROLE LEAVE
        // =========================

        private void textrole_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textrole.Text))
            {
                textrole.Text = "Enter Your Role";

                textrole.ForeColor =
                    Color.FromArgb(154, 160, 166);
            }
        }

        // =========================
        // BACK BUTTON
        // =========================

        private void button3_Click(object sender, EventArgs e)
        {
            LoginChoice obj = new LoginChoice();

            obj.Show();

            this.Hide();
        }

        // =========================
        // LOGIN BUTTON
        // =========================

        private void button2_Click(object sender, EventArgs e)
        {
            // GET VALUES

            string inputEmail =
                textmail.Text.Trim().ToLower();

            string inputPass =
                textpassword.Text.Trim();

            string inputRole =
                textrole.Text.Trim();

            // VALIDATION

            if (inputEmail == "" ||
                inputPass == "" ||
                inputRole == "" ||

                inputEmail == "Enter Your Email" ||
                inputPass == "Enter Your Password" ||
                inputRole == "Enter Your Role")
            {
                MessageBox.Show(
                    "Please fill all fields.");

                return;
            }

            try
            {
                using (NpgsqlConnection con =
                    new NpgsqlConnection(con_string))
                {
                    con.Open();

                    string sql =
                        @"SELECT COUNT(*) FROM employees
                          WHERE email = @email
                          AND password = @pass
                          AND role = @role
                          AND is_active = true";

                    using (NpgsqlCommand cmd =
                        new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue(
                            "@email",
                            inputEmail);

                        cmd.Parameters.AddWithValue(
                            "@pass",
                            inputPass);

                        cmd.Parameters.AddWithValue(
                            "@role",
                            inputRole);

                        int userCount =
                            Convert.ToInt32(
                                cmd.ExecuteScalar());

                        if (userCount == 1)
                        {
                            MessageBox.Show(
                                "Login Successful!");

                            // RESET PLACEHOLDERS

                            textmail.Text =
                                "Enter Your Email";

                            textmail.ForeColor =
                                Color.FromArgb(154, 160, 166);

                            textpassword.UseSystemPasswordChar =
                                false;

                            textpassword.Text =
                                "Enter Your Password";

                            textpassword.ForeColor =
                                Color.FromArgb(154, 160, 166);

                            textrole.Text =
                                "Enter Your Role";

                            textrole.ForeColor =
                                Color.FromArgb(154, 160, 166);

                            // OPEN DASHBOARD

                            EmployeeDashboard obj =
                                new EmployeeDashboard();

                            obj.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Login Failed.\nPlease check your credentials.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login Error:\n" + ex.Message);
            }
        }

        // =========================
        // EMPTY EVENTS
        // =========================

        private void textname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textrole_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {

        }

        private void textrole_TextChanged_1(
            object sender,
            EventArgs e)
        {

        }

        private void textname_Click(object sender, EventArgs e)
        {

        }

        private void Employeelogin_Load_1(
            object sender,
            EventArgs e)
        {

        }
    }
}