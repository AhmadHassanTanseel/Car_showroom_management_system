using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Npgsql;

namespace AutoNova_Car_Showroom
{
    public partial class UpdateCar : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        private byte[] selectedCarImageBytes = null;
        public List<string> list = new List<string>();
        string reg = "";
        string color = "";
        string model = "";
        int year = 0;
        string brand = "";
        string engine = "";
        decimal price = 0m;
        string isAvailable = "";
        string reg_id = "";
        public UpdateCar()
        {
            InitializeComponent();


        }

        private void UpdateCar_Load(object sender, EventArgs e)
        {
            CarImageHelper.EnsureCarImageColumn(con_string);
            load_reg();
            foreach (var item in list)
            {
                comboBox_reg.Items.Add(item);
            }

            //    // ADD COLUMNS
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

            //    // GRID STYLE
            //    dataGridView1.BackgroundColor = Color.FromArgb(154, 160, 166);
            //    dataGridView1.BorderStyle = BorderStyle.None;
            //    dataGridView1.GridColor = Color.White;

            //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //    dataGridView1.RowHeadersVisible = false;

            //    dataGridView1.EnableHeadersVisualStyles = false;
            //    dataGridView1.ColumnHeadersHeight = 40;

            //    // HEADER STYLE
            //    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            //    dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            //    dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            //    dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //    // ROW STYLE
            //    dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(154, 160, 166);
            //    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            //    dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //    dataGridView1.RowTemplate.Height = 35;

            //    // SELECTION STYLE
            //    dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Black;
            //    dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        public void load_reg()
        {
            NpgsqlConnection con = new NpgsqlConnection(con_string);
            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("Select registration_id from cars where availability = 'yes'", con);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                AppExceptionHandler.ShowError(ex, "Could not load registration IDs.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                reg_id = comboBox_reg.Text.Trim();
                if (string.IsNullOrWhiteSpace(reg_id))
                {
                    AppExceptionHandler.ShowWarning("Please select a registration ID.");
                    return;
                }

                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();
                    string sql = "SELECT * FROM cars WHERE registration_id = @reg";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@reg", reg_id);
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                AppExceptionHandler.ShowWarning("Car record not found.");
                                return;
                            }

                            reg = dr["registration_id"].ToString();
                            color = dr["color"].ToString();
                            model = dr["model"].ToString();
                            if (!InputValidator.TryParseIntFromDb(dr["year"], "year", out year) ||
                                !InputValidator.TryParseDecimalFromDb(dr["price"], "price", out price))
                            {
                                return;
                            }

                            brand = dr["brand"].ToString();
                            engine = dr["engine_type"].ToString();
                            isAvailable = dr["availability"].ToString();
                        }
                    }
                }

                txt_model.Text = model;
                txt_year.Text = year.ToString();
                txt_price.Text = price.ToString();
                txt_brand.Text = brand;
                txt_color.Text = color;
                txt_enginet.Text = engine;
                txt_avail.Text = isAvailable;
                selectedCarImageBytes = null;
                if (pictureBoxCarImage.Image != null)
                {
                    pictureBoxCarImage.Image.Dispose();
                }
                pictureBoxCarImage.Image = CarImageHelper.LoadCarImage(con_string, reg_id);
                AppExceptionHandler.ShowInfo("Car data loaded.");
            }, "Could not load car data.");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                reg_id = comboBox_reg.Text.Trim();
                if (string.IsNullOrWhiteSpace(reg_id))
                {
                    AppExceptionHandler.ShowWarning("Please select a registration ID.");
                    return;
                }

                if (!InputValidator.TryGetRequiredText(txt_model, "model", out model) ||
                    !InputValidator.TryGetRequiredText(txt_brand, "brand", out brand) ||
                    !InputValidator.TryGetRequiredText(txt_color, "color", out color) ||
                    !InputValidator.TryGetRequiredText(txt_enginet, "engine type", out engine) ||
                    !InputValidator.TryGetRequiredText(txt_avail, "availability", out isAvailable) ||
                    !InputValidator.TryParseInt(txt_year, "Year", out year, 1900, DateTime.Now.Year + 1) ||
                    !InputValidator.TryParseDecimal(txt_price, "Price", out price, 1m))
                {
                    return;
                }

                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();
                    string sql = @"UPDATE cars 
                           SET color = @color, 
                               model = @model, 
                               year = @year, 
                               brand = @brand, 
                               engine_type = @engine, 
                               price = @price, 
                               availability = @available";

                    if (selectedCarImageBytes != null && selectedCarImageBytes.Length > 0)
                    {
                        sql += ", car_image = @image";
                    }

                    sql += " WHERE registration_id = @reg";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@color", color);
                        cmd.Parameters.AddWithValue("@model", model);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@brand", brand);
                        cmd.Parameters.AddWithValue("@engine", engine);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@available", isAvailable);
                        if (selectedCarImageBytes != null && selectedCarImageBytes.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@image", selectedCarImageBytes);
                        }
                        cmd.Parameters.AddWithValue("@reg", reg_id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            AppExceptionHandler.ShowInfo("Record updated successfully.");
                        }
                        else
                        {
                            AppExceptionHandler.ShowWarning("Update failed. Record not found.");
                        }
                    }
                }
            }, "Could not update car.");
        }

        private void btnSelectCarImage_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    openFileDialog.Title = "Select Car Picture";

                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    selectedCarImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    if (pictureBoxCarImage.Image != null)
                    {
                        pictureBoxCarImage.Image.Dispose();
                    }
                    pictureBoxCarImage.Image = CarImageHelper.ImageFromBytes(selectedCarImageBytes);
                }
            }, "Could not load car picture.");
        }

        private void txt_year_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
