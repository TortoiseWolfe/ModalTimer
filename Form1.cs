using System;
using System.Drawing;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Util;
using Amazon.Runtime;

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
        private async void CountdownTimer_Tick(object? sender, EventArgs e)
        {
            remainingTime--;
            Label_CountDown.Text = $"{remainingTime} seconds";

            if (remainingTime <= 0)
            {
                countdownTimer?.Stop();
                await TerminateInstance();
                this.Close();
            }
        }
        private async Task TerminateInstance()
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "error_log.txt");

            try
            {
                LogToFile("Starting TerminateEC2Instance.", logFilePath);

                var ec2Client = new AmazonEC2Client(RegionEndpoint.USEast1); // Replace with your desired region

                var instanceId = await GetInstanceIdAsync();
                LogToFile($"Instance ID: {instanceId}", logFilePath);

                // Check instance name
                var describeRequest = new DescribeInstancesRequest { InstanceIds = new List<string> { instanceId } };
                var describeResponse = await ec2Client.DescribeInstancesAsync(describeRequest);
                LogToFile("Received DescribeInstancesAsync response.", logFilePath);

                string instanceName = "";
                foreach (var reservation in describeResponse.Reservations)
                {
                    foreach (var instance in reservation.Instances)
                    {
                        if (instance.InstanceId == instanceId)
                        {
                            foreach (var tag in instance.Tags)
                            {
                                if (tag.Key == "Name")
                                {
                                    instanceName = tag.Value;
                                    break;
                                }
                            }
                        }
                    }
                }

                LogToFile($"Instance Name: {instanceName}", logFilePath);

                // Terminate instance if the name matches your criteria
                if (instanceName == "your-instance-name-here") // Replace with the desired instance name to check
                {
                    var request = new TerminateInstancesRequest
                    {
                        InstanceIds = new List<string> { instanceId }
                    };

                    var response = await ec2Client.TerminateInstancesAsync(request);
                    LogToFile($"TerminateInstancesAsync response: {response.HttpStatusCode}", logFilePath);

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        LogToFile("Instance termination request sent successfully.", logFilePath);
                        foreach (InstanceStateChange instanceStateChange in response.TerminatingInstances)
                        {
                            LogToFile($"Instance {instanceStateChange.InstanceId} changed state from {instanceStateChange.PreviousState.Name} to {instanceStateChange.CurrentState.Name}", logFilePath);
                        }
                    }
                    else
                    {
                        LogToFile($"Failed to terminate instance. HTTP status code: {response.HttpStatusCode}", logFilePath);
                    }
                }
                else
                {
                    LogToFile("Instance name does not match the criteria. Termination request not sent.", logFilePath);
                }
            }
            catch (AmazonServiceException ex)
            {
                LogToFile($"An Amazon service error occurred: {ex.Message}", logFilePath);
            }
            catch (AmazonClientException ex)
            {
                LogToFile($"An Amazon client error occurred: {ex.Message}", logFilePath);
            }
            catch (Exception ex)
            {
                LogToFile($"An error occurred: {ex.Message}", logFilePath);
            }
        }
        private void LogToFile(string message, string logFilePath = "")
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "error_log.txt");
            }

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(logFilePath, "myListener"));
            Trace.WriteLine($"{DateTime.Now}: {message}");
            Trace.Flush();
        }
        private async Task<string> GetInstanceIdAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://169.254.169.254/latest/meta-data/instance-id");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        private async void Btn_Yes_Click(object sender, EventArgs e)
        {
            countdownTimer?.Stop();
            await TerminateInstance();
            this.Close();
        }
        private void Btn_No_Click(object sender, EventArgs e)
        {
            countdownTimer?.Stop();
            this.Close();
        }
    }
}
