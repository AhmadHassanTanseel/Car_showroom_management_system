using Npgsql;
using System;
using System.Drawing;
using System.IO;

namespace AutoNova_Car_Showroom
{
    public static class CarImageHelper
    {
        public static void EnsureCarImageColumn(string connectionString)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(
                        "ALTER TABLE cars ADD COLUMN IF NOT EXISTS car_image BYTEA", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
            }
        }

        public static Image ImageFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream(imageBytes))
            using (Image temp = Image.FromStream(ms))
            {
                return new Bitmap(temp);
            }
        }

        public static Image LoadCarImage(string connectionString, string registrationId)
        {
            if (string.IsNullOrWhiteSpace(registrationId))
            {
                return null;
            }

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                con.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT car_image FROM cars WHERE registration_id = @reg", con))
                {
                    cmd.Parameters.AddWithValue("@reg", registrationId);
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }

                    return ImageFromBytes((byte[])result);
                }
            }
        }
    }
}
