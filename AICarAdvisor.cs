using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public partial class AICarAdvisor : ResponsiveFormBase
    {
        // ? HTTP CLIENT
        private static readonly HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        // ?? PUT YOUR NEW API KEY HERE
        private const string GROQ_API_KEY = "Your_Grok_APi";

        private const string GROQ_API_URL = "https://api.groq.com/openai/v1/chat/completions";
        private const string GROQ_MODEL = "llama-3.1-8b-instant";

        public AICarAdvisor()
        {
            InitializeComponent();
        }

        // ? REQUEST CLASSES (OUTSIDE METHODS, INSIDE CLASS)
        public class GroqMessage
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        public class GroqRequest
        {
            public string model { get; set; }
            public List<GroqMessage> messages { get; set; }
        }

        // ? BUTTON1 CLICK ? SHOW AI RESPONSE
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRequirements.Text))
            {
                MessageBox.Show("Please enter your car requirements.",
                                "Empty Input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                txtRequirements.Focus();
                return;
            }

            btnGetAdvice.Enabled = false;
            btnGetAdvice.Text = "Please wait...";
            txtSuggestions.Text = "AI is thinking...";

            try
            {
                string result = await CallGroqAPI(txtRequirements.Text.Trim());
                txtSuggestions.Text = result;
            }
            catch (HttpRequestException)
            {
                txtSuggestions.Text = "No internet connection.";
            }
            catch (TaskCanceledException)
            {
                txtSuggestions.Text = "Request timed out.";
            }
            catch (Exception ex)
            {
                txtSuggestions.Text = "Error:\n" + ex.Message;
            }
            finally
            {
                btnGetAdvice.Enabled = true;
                btnGetAdvice.Text = "Get Suggestions";
            }
        }

        // ? API CALL METHOD
        private async Task<string> CallGroqAPI(string requirements)
        {
            string prompt = $"Requirements: {requirements}. " +
                            $"Suggest 2-3 cars available in Pakistan and explain why they are good options.And Remember to tell user cars according to his budgets and currency should be rupees";

            var requestBody = new GroqRequest
            {
                model = GROQ_MODEL,
                messages = new List<GroqMessage>
                {
                    new GroqMessage
                    {
                        role = "system",
                        content = "You are an expert car buying consultant."
                    },
                    new GroqMessage
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            string json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GROQ_API_KEY}");

            var response = await client.PostAsync(GROQ_API_URL, content);
            string resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return $"API Error ({(int)response.StatusCode}):\n{resultJson}";

            using (JsonDocument doc = JsonDocument.Parse(resultJson))
            {
                if (doc.RootElement.TryGetProperty("choices", out JsonElement choices))
                {
                    return choices[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString() ?? "No response from AI.";
                }
                else if (doc.RootElement.TryGetProperty("error", out JsonElement error))
                {
                    return "Groq Error: " +
                        (error.TryGetProperty("message", out var msg)
                        ? msg.GetString()
                        : resultJson);
                }
            }

            return "Unexpected response:\n" + resultJson;
        }

        // ? EMPTY UI EVENTS (SAFE)
        private void AICarAdvisor_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void textName_TextChanged(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            priceprediction obj = new priceprediction();

            obj.TopLevel = false;
            obj.FormBorderStyle = FormBorderStyle.None;
            obj.Dock = DockStyle.Fill;

            ConsulationPanel.Controls.Clear();
            ConsulationPanel.Controls.Add(obj);

            obj.Show();
        }

        private void ConsulationPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}