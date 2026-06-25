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
    public partial class SplashScreen : ResponsiveFormBase
    {
        public SplashScreen()
        {
            InitializeComponent();
            EnableWindowChrome = false;
        }

        private void SplashScreen_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            LoginChoice login = new LoginChoice();
            login.Show();

            this.Hide();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
