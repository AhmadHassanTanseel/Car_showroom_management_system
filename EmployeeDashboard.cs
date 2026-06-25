using System;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class EmployeeDashboard : ResponsiveFormBase
    {
        // Active opened form
        private Form activeForm = null;

        public EmployeeDashboard()
        {
            InitializeComponent();

            // MAIN DASHBOARD SETTINGS

            EnableWindowChrome = true;

            this.StartPosition =
                FormStartPosition.CenterScreen;

            this.WindowState =
                FormWindowState.Normal;

            this.MaximizeBox = true;

            this.MinimizeBox = true;
        }

        // Method for opening forms inside panel
        private void OpenChildForm(Form childForm)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                // OLD FORM CLOSE
                if (activeForm != null)
                {
                    activeForm.Close();
                }

                activeForm = childForm;

                // REMOVE TOP BAR OF CHILD FORM
                if (childForm is ResponsiveFormBase responsiveForm)
                {
                    responsiveForm.EnableWindowChrome = false;
                }

                // IMPORTANT SETTINGS
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;

                // PANEL SETTINGS
                panelmain2.Controls.Clear();
                panelmain2.Controls.Add(childForm);

                panelmain2.Tag = childForm;

                childForm.BringToFront();
                childForm.Show();

            }, "Could not open page.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AddCar());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new UpdateCar());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DeleteCar());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewInventry());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AICarAdvisor());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoginChoice obj = new LoginChoice();

            obj.Show();

            this.Hide();
        }

        private void EmployeeDashboard_Load(object sender, EventArgs e)
        {

        }

        private void panelmain2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}