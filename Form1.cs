using System;
using System.Drawing;
using System.Windows.Forms;

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

        private void TerminateInstance()
        {
            // Add your logic to terminate the instance here
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
