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
    public partial class LoginChoice : ResponsiveFormBase
    {
        public LoginChoice()
        {
            InitializeComponent();
        }

        private void LoginChoice_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Adminlogin admin = new Adminlogin();
            admin.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Employeelogin emp = new Employeelogin();
            emp.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Userlogin user = new Userlogin();
            user.Show();
            this.Hide();
        }
    }
}
