using System;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Npgsql;
using MailKit.Net.Smtp;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace AutoNova_Car_Showroom
{
    public partial class UserSignUp : ResponsiveFormBase
    {
        public int otp;
        public string name;
        public string email;
        public string password;
        public string confirm_pass;

        public string con_string =
            "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";

        private void textPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textConfirmpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public UserSignUp()
        {
            InitializeComponent();

            // =========================
            // PLACEHOLDERS
            // =========================

            textName.Text = "Enter Your Name";
            textName.ForeColor = Color.FromArgb(154, 160, 166);

            textEmail.Text = "Enter Your Email";
            textEmail.ForeColor = Color.FromArgb(154, 160, 166);

            textPassword.Text = "Enter Your Password";
            textPassword.ForeColor = Color.FromArgb(154, 160, 166);
            textPassword.UseSystemPasswordChar = false;

            textConfirmpassword.Text = "Confirm Password";
            textConfirmpassword.ForeColor = Color.FromArgb(154, 160, 166);
            textConfirmpassword.UseSystemPasswordChar = false;

            textOTP.Text = "Enter OTP";
            textOTP.ForeColor = Color.FromArgb(154, 160, 166);

            // =========================
            // CONNECT EVENTS
            // =========================

            textName.Enter += textName_Enter;
            textName.Leave += textName_Leave;

            textEmail.Enter += textEmail_Enter;
            textEmail.Leave += textEmail_Leave;

            textPassword.Enter += textPassword_Enter;
            textPassword.Leave += textPassword_Leave;

            textConfirmpassword.Enter += textConfirmpassword_Enter;
            textConfirmpassword.Leave += textConfirmpassword_Leave;

            textOTP.Enter += textOTP_Enter;
            textOTP.Leave += textOTP_Leave;
        }

        private void UserSignUp_Load(object sender, EventArgs e)
        {

        }

        // ======================
        // NAME
        // ======================

        private void textName_Enter(object sender, EventArgs e)
        {
            if (textName.Text == "Enter Your Name")
            {
                textName.Text = "";
                textName.ForeColor = Color.White;
            }
        }

        private void textName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textName.Text))
            {
                textName.Text = "Enter Your Name";
                textName.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // ======================
        // EMAIL
        // ======================

        private void textEmail_Enter(object sender, EventArgs e)
        {
            if (textEmail.Text == "Enter Your Email")
            {
                textEmail.Text = "";
                textEmail.ForeColor = Color.White;
            }
        }

        private void textEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textEmail.Text))
            {
                textEmail.Text = "Enter Your Email";
                textEmail.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // ======================
        // PASSWORD
        // ======================

        private void textPassword_Enter(object sender, EventArgs e)
        {
            if (textPassword.Text == "Enter Your Password")
            {
                textPassword.Text = "";
                textPassword.ForeColor = Color.White;
                textPassword.UseSystemPasswordChar = true;
            }
        }

        private void textPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textPassword.Text))
            {
                textPassword.UseSystemPasswordChar = false;
                textPassword.Text = "Enter Your Password";
                textPassword.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // ======================
        // CONFIRM PASSWORD
        // ======================

        private void textConfirmpassword_Enter(object sender, EventArgs e)
        {
            if (textConfirmpassword.Text == "Confirm Password")
            {
                textConfirmpassword.Text = "";
                textConfirmpassword.ForeColor = Color.White;
                textConfirmpassword.UseSystemPasswordChar = true;
            }
        }

        private void textConfirmpassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textConfirmpassword.Text))
            {
                textConfirmpassword.UseSystemPasswordChar = false;
                textConfirmpassword.Text = "Confirm Password";
                textConfirmpassword.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        // ======================
        // OTP
        // ======================

        private void textOTP_Enter(object sender, EventArgs e)
        {
            if (textOTP.Text == "Enter OTP")
            {
                textOTP.Text = "";
                textOTP.ForeColor = Color.White;
            }
        }

        private void textOTP_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textOTP.Text))
            {
                textOTP.Text = "Enter OTP";
                textOTP.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        // ======================
        // SIGN IN BUTTON
        // ======================

        private void button1_Click(object sender, EventArgs e)
        {
            Userlogin obj = new Userlogin();
            obj.Show();
            this.Hide();
        }

        // ======================
        // SEND OTP BUTTON
        // ======================

        private void button3_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            otp = random.Next(100000, 999999);

            name = textName.Text.Trim();
            email = textEmail.Text.Trim();
            password = textPassword.Text.Trim();
            confirm_pass = textConfirmpassword.Text.Trim();

            try
            {
                // VALIDATION

                if (name == "" ||
                    email == "" ||
                    password == "" ||
                    confirm_pass == "" ||

                    name == "Enter Your Name" ||
                    email == "Enter Your Email" ||
                    password == "Enter Your Password" ||
                    confirm_pass == "Confirm Password")
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                if (password != confirm_pass)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                // EMAIL SETUP

                var message = new MimeMessage();

                message.From.Add(
                    new MailboxAddress(
                        "AutoNova",
                        "sheikhijaz137@gmail.com"));

                message.To.Add(
                    new MailboxAddress("", email));

                message.Subject = "OTP Verification";

                message.Body = new TextPart("plain")
                {
                    Text = $"Hello {name}, your OTP is: {otp}"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(
                        "smtp.gmail.com",
                        587,
                        MailKit.Security.SecureSocketOptions.StartTls);

                    client.Authenticate(
                        "hassantanseelahmad@gmail.com",
                        "upxkjhzfgvsvvlcx");

                    client.Send(message);

                    client.Disconnect(true);
                }

                MessageBox.Show("OTP Sent Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Something Went Wrong\n" + ex.Message);
            }
        }

        // ======================
        // SIGN UP BUTTON
        // ======================

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textOTP.Text == "Enter OTP" ||
                    textOTP.Text == "")
                {
                    MessageBox.Show("Please enter OTP.");
                    return;
                }

                int in_otp = int.Parse(textOTP.Text);

                if (in_otp == otp)
                {
                    using (NpgsqlConnection con =
                        new NpgsqlConnection(con_string))
                    {
                        con.Open();

                        // CHECK EMAIL EXISTS

                        NpgsqlCommand checkCmd =
                            new NpgsqlCommand(
                                "SELECT COUNT(*) FROM users WHERE email=@uemail",
                                con);

                        checkCmd.Parameters.AddWithValue(
                            "@uemail",
                            email);

                        int count =
                            Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Email already exists.");
                            return;
                        }

                        // INSERT USER

                        NpgsqlCommand cmd =
                            new NpgsqlCommand(
                                "INSERT INTO users(name,email,password,otp) VALUES(@uname,@uemail,@upass,@uotp)",
                                con);

                        cmd.Parameters.AddWithValue("@uname", name);
                        cmd.Parameters.AddWithValue("@uemail", email);
                        cmd.Parameters.AddWithValue("@upass", confirm_pass);
                        cmd.Parameters.AddWithValue("@uotp", otp);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Signup Successful!");

                    // =========================
                    // RESET PLACEHOLDERS
                    // =========================

                    textName.Text = "Enter Your Name";
                    textName.ForeColor =
                        Color.FromArgb(154, 160, 166);

                    textEmail.Text = "Enter Your Email";
                    textEmail.ForeColor =
                        Color.FromArgb(154, 160, 166);

                    textPassword.UseSystemPasswordChar = false;
                    textPassword.Text = "Enter Your Password";
                    textPassword.ForeColor =
                        Color.FromArgb(154, 160, 166);

                    textConfirmpassword.UseSystemPasswordChar = false;
                    textConfirmpassword.Text = "Confirm Password";
                    textConfirmpassword.ForeColor =
                        Color.FromArgb(154, 160, 166);

                    textOTP.Text = "Enter OTP";
                    textOTP.ForeColor =
                        Color.FromArgb(154, 160, 166);

                    // OPEN LOGIN FORM

                    Userlogin obj = new Userlogin();
                    obj.Show();

                    // HIDE CURRENT FORM

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid OTP");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Something went wrong\n" + ex.Message);
            }
        }
    }
}