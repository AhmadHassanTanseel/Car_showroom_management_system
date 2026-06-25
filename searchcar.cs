using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace AutoNova_Car_Showroom
{
    public partial class searchcar : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        List<string> brand = new List<string>();
        List<string> model_obj = new List<string>();
        public searchcar()
        {
            InitializeComponent();

            // Form Load
            this.Load += searchcars_Load;

            // Attach Events in Code
            textmodel.Enter += textmodel_Enter;
            textmodel.Leave += textmodel_Leave;

            textbrand.Enter += textbrand_Enter;
            textbrand.Leave += textbrand_Leave;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            
        }

        private void textmodel_TextChanged(object sender, EventArgs e)
        {

        }
        private void textbrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void textprice_TextChanged(object sender, EventArgs e)
        {

        }

        private void searchcar_Load(object sender, EventArgs e)
        {
            //    if (dataGridView1.Columns.Count == 0)
            //    {
            //        dataGridView1.Columns.Add("carid", "Car ID");
            //        dataGridView1.Columns.Add("model", "Model");
            //        dataGridView1.Columns.Add("brand", "Brand / Name");
            //        dataGridView1.Columns.Add("price", "Price");
            //        dataGridView1.Columns.Add("color", "Color");
            //        dataGridView1.Columns.Add("year", "Year");
            //        dataGridView1.Columns.Add("engine", "Engine Type");
            //        dataGridView1.Columns.Add("availability", "Availability");
            //    }

            dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            dataGridView1.BorderStyle = BorderStyle.None;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 40;

            // HEADER STYLE
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // ROW STYLE
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

            dataGridView1.RowTemplate.Height = 35;

            CarImageHelper.EnsureCarImageColumn(con_string);
        }





        private void searchcars_Load(object sender, EventArgs e)
        {
            textmodel.Text = "Model";
            textmodel.ForeColor = Color.Gray;

            textbrand.Text = "Brand";
            textbrand.ForeColor = Color.Gray;


  
        }

        // MODEL
        private void textmodel_Enter(object sender, EventArgs e)
        {
            if (textmodel.Text == "Model")
            {
                textmodel.Text = "";
                textmodel.ForeColor = Color.White;
            }
        }

        private void textmodel_Leave(object sender, EventArgs e)
        {
            if (textmodel.Text == "")
            {
                textmodel.Text = "Model";
                textmodel.ForeColor = Color.Gray;
            }
        }

        // BRAND
        private void textbrand_Enter(object sender, EventArgs e)
        {
            if (textbrand.Text == "Brand")
            {
                textbrand.Text = "";
                textbrand.ForeColor = Color.White;
            }
        }

        private void textbrand_Leave(object sender, EventArgs e)
        {
            if (textbrand.Text == "")
            {
                textbrand.Text = "Brand";
                textbrand.ForeColor = Color.Gray;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                string brandInput = InputValidator.IsPlaceholder(textbrand.Text, "Brand", " Enter Brand..")
                    ? string.Empty
                    : textbrand.Text.ToLower().Replace(" ", "_");
                string modelInput = InputValidator.IsPlaceholder(textmodel.Text, "Model", " Enter Model..")
                    ? string.Empty
                    : textmodel.Text.ToLower().Replace(" ", "_");

                if (string.IsNullOrEmpty(brandInput) && string.IsNullOrEmpty(modelInput))
                {
                    AppExceptionHandler.ShowWarning("Please enter a brand or model name.");
                    return;
                }

                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();
                    string query = @"SELECT 
                                    registration_id,
                                    brand AS ""Brand"",
                                    model AS ""Model"", 
                                    color AS ""Color"", 
                                    engine_type AS ""Engine"", 
                                    price AS ""Price"", 
                                    availability AS ""Status"" 
                                 FROM cars
                                 WHERE is_active = TRUE 
                                 AND (LOWER(brand) = @brand OR LOWER(model) = @model)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@brand", brandInput);
                        cmd.Parameters.AddWithValue("@model", modelInput);

                        NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        dataGridView1.DataSource = dt;
                        HideRegistrationIdColumn();
                        ShowCarImagePreview(dt);

                        if (dt.Rows.Count == 0)
                        {
                            AppExceptionHandler.ShowWarning("Car not found.");
                        }
                    }
                }
            }, "Search failed.");
        }

        private void ShowCarImagePreview(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                pictureBoxCar.Image = null;
                return;
            }

            string registrationId = dt.Rows[0]["registration_id"]?.ToString() ?? string.Empty;
            LoadCarImageFromDb(registrationId);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
            {
                return;
            }

            string registrationId = dataGridView1.CurrentRow.Cells["registration_id"]?.Value?.ToString() ?? string.Empty;
            LoadCarImageFromDb(registrationId);
        }

        private void HideRegistrationIdColumn()
        {
            if (dataGridView1.Columns.Contains("registration_id"))
            {
                dataGridView1.Columns["registration_id"].Visible = false;
            }
        }

        private void LoadCarImageFromDb(string registrationId)
        {
            try
            {
                if (pictureBoxCar.Image != null)
                {
                    Image oldImage = pictureBoxCar.Image;
                    pictureBoxCar.Image = null;
                    oldImage.Dispose();
                }

                pictureBoxCar.Image = CarImageHelper.LoadCarImage(con_string, registrationId);
            }
            catch (Exception ex)
            {
                AppExceptionHandler.ShowError(ex, "Could not load car picture.");
            }
        }

        private void textbrand_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}