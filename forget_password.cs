using MailKit.Net.Smtp;
using MimeKit;
using Npgsql;
using System;
using System.Drawing;
using System.Windows.Forms;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace AutoNova_Car_Showroom
{
    public partial class forget_password : ResponsiveFormBase
    {
        public bool found = false;
        public int otp;
        public string email;

        public string con_string =
            "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";

        public forget_password()
        {
            InitializeComponent();

            // =========================
            // PLACEHOLDERS
            // =========================

            textEmail.Text = "Enter Your Email";
            textEmail.ForeColor = Color.FromArgb(154, 160, 166);

            textPassword.Text = "Enter Your Password";
            textPassword.ForeColor = Color.FromArgb(154, 160, 166);
            textPassword.UseSystemPasswordChar = false;

            textOTP.Text = "Enter OTP";
            textOTP.ForeColor = Color.FromArgb(154, 160, 166);

            // =========================
            // EVENTS
            // =========================

            textEmail.Enter += textEmail_Enter;
            textEmail.Leave += textEmail_Leave;

            textPassword.Enter += textPassword_Enter;
            textPassword.Leave += textPassword_Leave;

            textOTP.Enter += textOTP_Enter;
            textOTP.Leave += textOTP_Leave;
        }

        private void forget_password_Load(object sender, EventArgs e)
        {

        }

        // =========================
        // SEND OTP BUTTON
        // =========================

        private void button3_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                if (!InputValidator.TryGetEmail(textEmail, "email", out email))
                {
                    return;
                }

                found = false;

                using (NpgsqlConnection conn = new NpgsqlConnection(con_string))
                {
                    conn.Open();

                    using (NpgsqlCommand cmd =
                           new NpgsqlCommand("SELECT email FROM users", conn))

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) == email)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                if (found)
                {
                    Random random = new Random();
                    otp = random.Next(100000, 999999);

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
                        Text = $"Hello,\n\nYour OTP is: {otp}\n\n- AutoNova Team"
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(
                            "smtp.gmail.com",
                            587,
                            MailKit.Security.SecureSocketOptions.StartTls);

                        client.Authenticate(
                            "sheikhijaz137@gmail.com",
                            "huufvptcuhnhtlxr");

                        client.Send(message);

                        client.Disconnect(true);
                    }

                    AppExceptionHandler.ShowInfo("OTP sent successfully.");
                }
                else
                {
                    AppExceptionHandler.ShowWarning("Email not found.");
                }

            }, "Could not send OTP.");
        }

        // =========================
        // SAVE PASSWORD BUTTON
        // =========================

        private void button2_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                if (!found)
                {
                    AppExceptionHandler.ShowWarning(
                        "Please request an OTP first.");
                    return;
                }

                if (!InputValidator.TryParseOtp(textOTP, out int otp_in))
                {
                    return;
                }

                if (!InputValidator.TryGetRequiredText(
                        textPassword,
                        "new password",
                        out string pass))
                {
                    return;
                }

                if (otp_in != otp)
                {
                    AppExceptionHandler.ShowWarning("Wrong OTP.");
                    return;
                }

                using (NpgsqlConnection conn =
                       new NpgsqlConnection(con_string))

                using (NpgsqlCommand cmd =
                       new NpgsqlCommand(
                           "UPDATE users SET password = @new_pass WHERE email = @email",
                           conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@new_pass", pass);
                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.ExecuteNonQuery();
                }

                AppExceptionHandler.ShowInfo(
                    "Password updated successfully.");

                // =========================
                // CLEAR TEXTBOXES
                // =========================

                textEmail.Text = "Enter Your Email";
                textEmail.ForeColor = Color.FromArgb(154, 160, 166);

                textPassword.Text = "Enter Your Password";
                textPassword.ForeColor = Color.FromArgb(154, 160, 166);
                textPassword.UseSystemPasswordChar = false;

                textOTP.Text = "Enter OTP";
                textOTP.ForeColor = Color.FromArgb(154, 160, 166);

                // =========================
                // OPEN LOGIN FORM
                // =========================

                Userlogin login = new Userlogin();

                login.Show();

                this.Hide();

            }, "Could not update password.");
        }

        // =========================
        // BACK BUTTON
        // =========================

        private void button1_Click(object sender, EventArgs e)
        {
            Userlogin login = new Userlogin();

            login.Show();

            this.Hide();
        }

        // =========================
        // EMPTY EVENTS
        // =========================

        private void textOTP_TextChanged(object sender, EventArgs e)
        {

        }

        // =========================
        // EMAIL PLACEHOLDER
        // =========================

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

        // =========================
        // PASSWORD PLACEHOLDER
        // =========================

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

        // =========================
        // OTP PLACEHOLDER
        // =========================

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
    }
}