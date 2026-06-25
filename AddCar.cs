using Npgsql;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class AddCar : ResponsiveFormBase
    {
        public string con_string = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=csms1;";
        private byte[] selectedCarImageBytes = null;

        public AddCar()
        {
            InitializeComponent();
        }

        private void lbllName_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            AppExceptionHandler.SafeExecute(() =>
            {
                if (!InputValidator.TryGetRequiredText(txt_id, "registration ID", out string reg_id) ||
                    !InputValidator.TryGetRequiredText(txt_model, "model", out string model) ||
                    !InputValidator.TryGetRequiredText(txt_brand, "brand", out string brand) ||
                    !InputValidator.TryGetRequiredText(txt_color, "color", out string color) ||
                    !InputValidator.TryGetRequiredText(txt_enginet, "engine type", out string engine_type) ||
                    !InputValidator.TryGetRequiredText(txt_avail, "availability", out string availability) ||
                    !InputValidator.TryParseInt(txt_year, "Year", out int year, 1900, DateTime.Now.Year + 1) ||
                    !InputValidator.TryParseDecimal(txt_price, "Price", out decimal price, 1m))
                {
                    return;
                }

                reg_id = reg_id.ToLower().Replace(" ", "_");
                model = model.ToLower().Replace(" ", "_");
                brand = brand.ToLower().Replace(" ", "_");
                color = color.ToLower().Replace(" ", "_");
                engine_type = engine_type.ToLower();
                availability = availability.ToLower();

                using (NpgsqlConnection con = new NpgsqlConnection(con_string))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(@"insert into cars(registration_id , color , model , year , brand , engine_type , price , availability, car_image) 
values(@reg,@color,@model,@year,@brand,@engine,@price,@available,@image)", con))
                    {
                        cmd.Parameters.AddWithValue("@reg", reg_id);
                        cmd.Parameters.AddWithValue("@color", color);
                        cmd.Parameters.AddWithValue("@model", model);
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@brand", brand);
                        cmd.Parameters.AddWithValue("@engine", engine_type);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@available", availability);
                        cmd.Parameters.AddWithValue("@image",
                            selectedCarImageBytes != null && selectedCarImageBytes.Length > 0
                                ? (object)selectedCarImageBytes
                                : DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }

                AppExceptionHandler.ShowInfo("Car added successfully.");
            }, "Could not add car.");
        }

        private void AddCar_Load(object sender, EventArgs e)
        {
            CarImageHelper.EnsureCarImageColumn(con_string);
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
    }
}
