using System;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class AdminDashboard : ResponsiveFormBase
    {
        // ACTIVE OPENED FORM
        private Form activeForm = null;

        public AdminDashboard()
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

                // SAVE ACTIVE FORM

                activeForm = childForm;

                // REMOVE CHILD TOP BAR

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

                panelmain1.Controls.Clear();

                // ADD FORM

                panelmain1.Controls.Add(childForm);

                panelmain1.Tag = childForm;

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
            OpenChildForm(new availablecar());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AICarAdvisor());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewSales());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewInventry());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenChildForm(new HandleEmployes());
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

        private void AdminDashboard_Load(
            object sender,
            EventArgs e)
        {

        }

        private void panelmain1_Paint(
            object sender,
            PaintEventArgs e)
        {

        }
    }
}