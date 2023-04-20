using System;
using System.Drawing;
using System.Net.Http;
using System.Diagnostics;
using System.Windows.Forms;

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
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "powershell_log.txt");
            string scriptFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");
            string scriptPath = Path.Combine(scriptFolderPath, "TerminateInstance.ps1");

            try
            {

                LogToFile($"Running PowerShell script: {scriptPath}", logFilePath);
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\PowerShell\7\pwsh.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.OutputDataReceived += (s, e) => LogToFile($"[Output] {e.Data}", logFilePath);
                    process.ErrorDataReceived += (s, e) => LogToFile($"[Error] {e.Data}", logFilePath);

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    // Wrap the process execution in a Task and use await
                    await Task.Run(() => process.WaitForExit());
                    process.WaitForExit();
                }

                LogToFile("PowerShell script completed.", logFilePath);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred: {ex.Message}";
                LogToFile(errorMessage, logFilePath);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
