using System;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class UserDashboard : ResponsiveFormBase
    {
        // ACTIVE OPENED FORM

        private Form activeForm = null;

        public UserDashboard()
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

        // =====================================================
        // OPEN FORM INSIDE PANEL
        // =====================================================

        private void OpenChildForm(Form childForm)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                // CLOSE OLD FORM

                if (activeForm != null)
                {
                    activeForm.Close();
                }

                // SAVE NEW FORM

                activeForm = childForm;

                // REMOVE CHILD FORM TOP BAR

                if (childForm is ResponsiveFormBase responsiveForm)
                {
                    responsiveForm.EnableWindowChrome = false;
                }

                // CHILD FORM SETTINGS

                childForm.TopLevel = false;

                childForm.FormBorderStyle =
                    FormBorderStyle.None;

                childForm.Dock =
                    DockStyle.Fill;

                // CLEAR PANEL

                panelmain.Controls.Clear();

                // ADD FORM

                panelmain.Controls.Add(childForm);

                panelmain.Tag = childForm;

                // SHOW FORM

                childForm.BringToFront();

                childForm.Show();

            },
            "Could not open page.");
        }

        // =====================================================
        // BUTTONS
        // =====================================================

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new availablecar());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new searchcar());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenChildForm(new carbooking());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenChildForm(new viewBookinguserform());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AICarAdvisor());
        }

        // =====================================================
        // LOGOUT
        // =====================================================

        private void button6_Click(object sender, EventArgs e)
        {
            LoginChoice obj = new LoginChoice();

            obj.Show();

            this.Hide();
        }

        private void UserDashboard_Load(
            object sender,
            EventArgs e)
        {

        }

        private void panelmain_Paint(
            object sender,
            PaintEventArgs e)
        {

        }
    }
}