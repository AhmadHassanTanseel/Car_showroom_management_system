using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace AutoNova_Car_Showroom
{
    public partial class Userlogin : ResponsiveFormBase
    {
        public string con_string =
            "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";

        public Userlogin()
        {
            InitializeComponent();

            // PLACEHOLDER SETUP
            textname.Text = "Enter Your Email";
            textname.ForeColor = Color.FromArgb(154, 160, 166);

            textpassword.Text = "Enter Your Password";
            textpassword.ForeColor = Color.FromArgb(154, 160, 166);
            textpassword.UseSystemPasswordChar = false;

            // CONNECT EVENTS
            textname.Enter += textname_Enter;
            textname.Leave += textname_Leave;

            textpassword.Enter += textpassword_Enter;
            textpassword.Leave += textpassword_Leave;
        }

        private void Userlogin_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Name_Click(object sender, EventArgs e)
        {

        }

        private void texname_TextChanged(object sender, EventArgs e)
        {

        }

        // =============================
        // EMAIL TEXTBOX ENTER
        // =============================

        private void textname_Enter(object sender, EventArgs e)
        {
            if (textname.Text == "Enter Your Email")
            {
                textname.Text = "";
                textname.ForeColor = Color.White;
            }
        }

        // =============================
        // EMAIL TEXTBOX LEAVE
        // =============================

        private void textname_Leave(object sender, EventArgs e)
        {
            if (textname.Text == "")
            {
                textname.Text = "Enter Your Email";
                textname.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // =============================
        // PASSWORD ENTER
        // =============================

        private void textpassword_Enter(object sender, EventArgs e)
        {
            if (textpassword.Text == "Enter Your Password")
            {
                textpassword.Text = "";
                textpassword.ForeColor = Color.White;
                textpassword.UseSystemPasswordChar = true;
            }
        }

        // =============================
        // PASSWORD LEAVE
        // =============================

        private void textpassword_Leave(object sender, EventArgs e)
        {
            if (textpassword.Text == "")
            {
                textpassword.UseSystemPasswordChar = false;
                textpassword.Text = "Enter Your Password";
                textpassword.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // =============================
        // LOGIN BUTTON
        // =============================

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string email = textname.Text.Trim();
                string password = textpassword.Text.Trim();

                // VALIDATION
                if (email == "" ||
                    password == "" ||
                    email == "Enter Your Email" ||
                    password == "Enter Your Password")
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                bool found = false;

                using (NpgsqlConnection conn =
                    new NpgsqlConnection(con_string))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd =
                        new NpgsqlCommand(
                            "SELECT email,password FROM users",
                            conn))
                    using (NpgsqlDataReader reader =
                        cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ret_mail = reader.GetString(0);
                            string ret_pass = reader.GetString(1);

                            if (ret_mail == email &&
                                ret_pass == password)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (found)
                {
                    MessageBox.Show("Login Successful!");

                    // CLEAR TEXTBOXES
                    textname.Text = "Enter Your Email";
                    textname.ForeColor = Color.FromArgb(154, 160, 166);

                    textpassword.UseSystemPasswordChar = false;
                    textpassword.Text = "Enter Your Password";
                    textpassword.ForeColor = Color.FromArgb(154, 160, 166);

                    // OPEN DASHBOARD
                    UserDashboard dashboard =
                        new UserDashboard();

                    dashboard.Show();

                    // HIDE CURRENT FORM
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Wrong Email or Password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login Failed\n" + ex.Message);
            }
        }

        // =============================
        // BACK BUTTON
        // =============================

        private void button3_Click(object sender, EventArgs e)
        {
            LoginChoice obj = new LoginChoice();

            obj.Show();

            this.Hide();
        }

        // =============================
        // SIGNUP BUTTON
        // =============================

        private void button1_Click(object sender, EventArgs e)
        {
            UserSignUp obj = new UserSignUp();

            obj.Show();

            this.Hide();
        }

        // =============================
        // FORGET PASSWORD
        // =============================

        private void linkLabel1_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            forget_password pass_obj =
                new forget_password();

            pass_obj.Show();

            this.Hide();
        }
    }
}