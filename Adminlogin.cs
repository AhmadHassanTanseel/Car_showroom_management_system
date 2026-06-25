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
    public partial class Adminlogin : ResponsiveFormBase
    {
        public Adminlogin()
        {
            InitializeComponent();

            textname.Text = "Enter Your Name";
            textname.ForeColor = Color.FromArgb(154, 160, 166);

            textpassword.Text = "Enter Your Password";
            textpassword.ForeColor = Color.FromArgb(154, 160, 166);

            textname.Enter += textname_Enter;
            textname.Leave += textname_Leave;

            textpassword.Enter += textpassword_Enter;
            textpassword.Leave += textpassword_Leave;
        }

        private void textname_Enter(object sender, EventArgs e)
        {
            if (textname.Text == "Enter Your Name")
            {
                textname.Text = "";
                textname.ForeColor = Color.White;
            }
        }

        private void textname_Leave(object sender, EventArgs e)
        {
            if (textname.Text == "")
            {
                textname.Text = "Enter Your Name";
                textname.ForeColor = Color.FromArgb(154, 160, 166);
            }
        }

        private void textpassword_Enter(object sender, EventArgs e)
        {
            if (textpassword.Text == "Enter Your Password")
            {
                textpassword.Text = "";
                textpassword.ForeColor = Color.White;
                textpassword.UseSystemPasswordChar = true;
            }
        }

        private void textpassword_Leave(object sender, EventArgs e)
        {
            if (textpassword.Text == "")
            {
                textpassword.Text = "Enter Your Password";
                textpassword.ForeColor = Color.FromArgb(154, 160, 166);
                textpassword.UseSystemPasswordChar = false;
            }
        }

        private void textname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textname.Text;
            string password = textpassword.Text;
            if (name == "nova" && password == "nova5")
            {
                AdminDashboard obj = new AdminDashboard();
                obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Credentials");
            }
        }
            

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoginChoice obj = new LoginChoice();
            obj.Show();
            this.Hide();
        }

        private void Adminlogin_Load(object sender, EventArgs e)
        {

        }
    }
}