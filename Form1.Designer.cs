namespace ModalTimer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label_Message = new Label();
            Btn_Yes = new Button();
            Btn_No = new Button();
            Label_CountDown = new Label();
            Label_LogFilePath = new Label();
            SuspendLayout();
            // 
            // Label_Message
            // 
            Label_Message.AutoSize = true;
            Label_Message.BackColor = SystemColors.ActiveCaption;
            Label_Message.Font = new Font("Segoe UI", 19F, FontStyle.Regular, GraphicsUnit.Point);
            Label_Message.Location = new Point(12, 60);
            Label_Message.Name = "Label_Message";
            Label_Message.Size = new Size(187, 36);
            Label_Message.TabIndex = 0;
            Label_Message.Text = "Label_Message";
            // 
            // Btn_Yes
            // 
            Btn_Yes.BackColor = Color.YellowGreen;
            Btn_Yes.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point);
            Btn_Yes.Location = new Point(12, 12);
            Btn_Yes.Name = "Btn_Yes";
            Btn_Yes.Size = new Size(75, 45);
            Btn_Yes.TabIndex = 1;
            Btn_Yes.Text = "Yes";
            Btn_Yes.UseVisualStyleBackColor = false;
            Btn_Yes.Click += Btn_Yes_Click;
            // 
            // Btn_No
            // 
            Btn_No.BackColor = Color.PaleVioletRed;
            Btn_No.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point);
            Btn_No.Location = new Point(93, 12);
            Btn_No.Name = "Btn_No";
            Btn_No.Size = new Size(75, 45);
            Btn_No.TabIndex = 2;
            Btn_No.Text = "No";
            Btn_No.UseVisualStyleBackColor = false;
            Btn_No.Click += Btn_No_Click;
            // 
            // Label_CountDown
            // 
            Label_CountDown.AutoSize = true;
            Label_CountDown.BackColor = SystemColors.ActiveCaption;
            Label_CountDown.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point);
            Label_CountDown.Location = new Point(174, 21);
            Label_CountDown.Name = "Label_CountDown";
            Label_CountDown.Size = new Size(88, 36);
            Label_CountDown.TabIndex = 3;
            Label_CountDown.Text = "label1";
            // 
            // Label_LogFilePath
            // 
            Label_LogFilePath.AutoSize = true;
            Label_LogFilePath.Location = new Point(174, 9);
            Label_LogFilePath.Name = "Label_LogFilePath";
            Label_LogFilePath.Size = new Size(38, 15);
            Label_LogFilePath.TabIndex = 4;
            Label_LogFilePath.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(482, 109);
            Controls.Add(Label_LogFilePath);
            Controls.Add(Label_CountDown);
            Controls.Add(Btn_No);
            Controls.Add(Btn_Yes);
            Controls.Add(Label_Message);
            Name = "Form1";
            Text = "Terminate EC2 Intance";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label Label_Message;
        private Button Btn_Yes;
        private Button Btn_No;
        private Label Label_CountDown;
        private Label Label_LogFilePath;
    }
}