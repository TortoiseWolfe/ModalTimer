using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Amazon.Util;

namespace ModalTimer
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer? countdownTimer;
        private int remainingTime = 300;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label_Message.Text = "Do you want to terminate the instance?";
            Label_CountDown.Text = $"{remainingTime} seconds";

            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "error_log.txt");
            Label_LogFilePath.Text = $"Log file will be created at: {logFilePath}";

            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
        }


        private void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            remainingTime--;
            Label_CountDown.Text = $"{remainingTime} seconds";

            if (remainingTime <= 0)
            {
                countdownTimer?.Stop();
                TerminateInstance();
                this.Close();
            }
        }

        private async void TerminateInstance()
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                string instanceId = await httpClient.GetStringAsync("http://169.254.169.254/latest/meta-data/instance-id");

                var ec2Client = new AmazonEC2Client();
                var terminateRequest = new TerminateInstancesRequest
                {
                    InstanceIds = new List<string> { instanceId }
                };

                var response = await ec2Client.TerminateInstancesAsync(terminateRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    string successMessage = "Instance terminated successfully.";
                    LogToFile(successMessage);
                    MessageBox.Show(successMessage, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string errorMessage = "Failed to terminate the instance.";
                    LogToFile(errorMessage);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred: {ex.Message}";
                LogToFile(errorMessage);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogToFile(string message)
        {
            try
            {
                string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "error_log.txt");
                using StreamWriter writer = new StreamWriter(logFilePath, true);
                writer.WriteLine($"{DateTime.Now}: {message}");
                MessageBox.Show($"Log file updated at: {logFilePath}", "Log File Location", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write to log file: {ex.Message}", "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Btn_Yes_Click(object sender, EventArgs e)
        {
            countdownTimer?.Stop();
            TerminateInstance();
            this.Close();
        }

        private void Btn_No_Click(object sender, EventArgs e)
        {
            countdownTimer?.Stop();
            this.Close();
        }
    }
}
