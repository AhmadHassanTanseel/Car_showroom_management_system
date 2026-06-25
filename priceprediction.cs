using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AutoNova_Car_Showroom
{
    public partial class priceprediction : ResponsiveFormBase 
    {
        // ? Single shared HttpClient with timeout
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        // ? Path to the folder containing api.py ? adjust if needed
        private const string API_FOLDER = @"D:\#AutoNovaCarShowroom\#AutoNovaCarShowroom";
        private const string API_URL = "http://127.0.0.1:8000";

        public priceprediction()
        {
            InitializeComponent();
        }

        // ------------------------------------------
        // Close button
        // ------------------------------------------
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ------------------------------------------
        // Switch to AI Car Advisor panel
        // ------------------------------------------
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AICarAdvisor obj = new AICarAdvisor();

            obj.TopLevel = false;
            obj.FormBorderStyle = FormBorderStyle.None;
            obj.Dock = DockStyle.Fill;

            pridictionpanel.Controls.Clear();
            pridictionpanel.Controls.Add(obj);

            obj.Show();
        
        }

        // ------------------------------------------
        // Predict button click
        // ------------------------------------------
        private async void button1_Click(object sender, EventArgs e)
        {
            await PredictPriceAsync();
        }

        // ------------------------------------------
        // Main prediction logic
        // ------------------------------------------
        private async Task PredictPriceAsync()
        {
            // Disable button to prevent double-clicks
            button1.Enabled = false;

            try
            {
                // -- Step 1: Check if server is already running --------------
                bool serverRunning = await IsServerRunning();

                // -- Step 2: Start server if not running ---------------------
                if (!serverRunning)
                {
                    MessageBox.Show(
                        "FastAPI server is not running.\nStarting it now ? please wait up to 15 seconds...",
                        "Starting Server",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    bool started = StartServer();
                    if (!started)
                    {
                        MessageBox.Show(
                            "Could not launch the server process.\n\nCheck that the folder path is correct:\n" + API_FOLDER,
                            "Launch Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }

                    // Wait until server responds (retry every second, max 15s)
                    bool ready = await WaitForServer(maxWaitSeconds: 15);
                    if (!ready)
                    {
                        MessageBox.Show(
                            "Server did not respond within 15 seconds.\n\n" +
                            "Make sure:\n" +
                            "  1. Python is installed and in PATH\n" +
                            "  2. uvicorn is installed:  pip install uvicorn fastapi\n" +
                            "  3. api.py exists in:\n     " + API_FOLDER,
                            "Server Timeout",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }
                }

                // -- Step 3: Validate all inputs -----------------------------
                if (!ValidateInputs(out string validationError))
                {
                    MessageBox.Show(validationError, "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // -- Step 4: Parse numeric fields ----------------------------
                int.TryParse(textBox9.Text.Trim(), out int modelYear);
                int.TryParse(textBox6.Text.Trim(), out int mileage);
                int.TryParse(textBox5.Text.Trim(), out int engineCapacity);

                // -- Step 5: Build request payload ---------------------------
                var carData = new
                {
                    brand = textBox12.Text.Trim(),
                    model_name = textBox11.Text.Trim(),
                    variant = textBox10.Text.Trim(),
                    model = modelYear,
                    mileage = mileage,
                    city = textBox_city.Text.Trim(),
                    fuel_type = textBox_fuel.Text.Trim(),
                    transmission = textBox_trans.Text.Trim(),
                    registered = textBox_reg.Text.Trim(),
                    assembly = textBox_assembly.Text.Trim(),
                    engine_capacity = engineCapacity,
                    car_type = textBox_cr.Text.Trim()
                };

                // -- Step 6: Send POST request --------------------------------
                string json = JsonSerializer.Serialize(carData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                try
                {
                    response = await client.PostAsync(API_URL + "/predict", content);
                }
                catch (HttpRequestException httpEx)
                {
                    MessageBox.Show(
                        "Could not reach the server.\n\nDetails: " + httpEx.Message,
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                catch (TaskCanceledException)
                {
                    MessageBox.Show(
                        "Request timed out after 30 seconds.\nThe server may be overloaded.",
                        "Timeout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // -- Step 7: Parse and display result ------------------------
                string resultJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Server returned an error (" + (int)response.StatusCode + "):\n" + resultJson,
                        "API Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                try
                {
                    var doc = JsonDocument.Parse(resultJson);

                    if (doc.RootElement.TryGetProperty("predicted_price", out var price))
                    {
                        // ? Use GetDouble() ? price may be decimal, GetInt32() would crash
                        string formatted = price.GetDouble().ToString("N0");
                        MessageBox.Show(
                            "Predicted Price:  Rs " + formatted,
                            "Prediction Result",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Unexpected response from server:\n" + resultJson,
                            "Parse Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
                catch (JsonException jsonEx)
                {
                    MessageBox.Show(
                        "Could not parse server response.\n\nRaw response:\n" + resultJson +
                        "\n\nDetails: " + jsonEx.Message,
                        "JSON Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                // Catch-all ? should never normally reach here
                MessageBox.Show(
                    "Unexpected error:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Always re-enable the button
                button1.Enabled = true;
            }
        }

        // ------------------------------------------
        // Check if FastAPI server is reachable
        // ------------------------------------------
        private async Task<bool> IsServerRunning()
        {
            try
            {
                var response = await client.GetAsync(API_URL + "/");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // ------------------------------------------
        // Launch api.py ? uvicorn starts inside it
        // ------------------------------------------
        private bool StartServer()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = "api.py",
                    UseShellExecute = false,   // ? must be false to hide window
                    CreateNoWindow = true,    // ? no terminal appears on screen
                    WorkingDirectory = "D:\\#AutoNovaCarShowroom\\#AutoNovaCarShowroom"
                };

                Process.Start(psi);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start server process:\n" + ex.Message);
                return false;
            }
        }

        // ------------------------------------------
        // Poll every 1 second until server is up
        // ------------------------------------------
        private async Task<bool> WaitForServer(int maxWaitSeconds)
        {
            for (int i = 0; i < maxWaitSeconds; i++)
            {
                await Task.Delay(1000);
                if (await IsServerRunning())
                    return true;
            }
            return false;
        }

        // ------------------------------------------
        // Validate every field before sending
        // ------------------------------------------
        private bool ValidateInputs(out string error)
        {
            // Text fields
            if (string.IsNullOrWhiteSpace(textBox12.Text)) { error = "Brand is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox11.Text)) { error = "Model name is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox10.Text)) { error = "Variant is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_city.Text)) { error = "City is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_fuel.Text)) { error = "Fuel type is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_trans.Text)) { error = "Transmission is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_reg.Text)) { error = "Registered city is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_assembly.Text)) { error = "Assembly is required."; return false; }
            if (string.IsNullOrWhiteSpace(textBox_cr.Text)) { error = "Car type is required."; return false; }

            // Numeric fields
            if (!int.TryParse(textBox9.Text, out int yr) || yr < 1900 || yr > DateTime.Now.Year)
            {
                error = "Enter a valid model year (e.g. 2020).";
                return false;
            }
            if (!int.TryParse(textBox6.Text, out int ml) || ml < 0)
            {
                error = "Mileage must be a positive number.";
                return false;
            }
            if (!int.TryParse(textBox5.Text, out int eng) || eng <= 0)
            {
                error = "Engine capacity must be a positive number.";
                return false;
            }

            error = null;
            return true;
        }

        private void pridictionpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}