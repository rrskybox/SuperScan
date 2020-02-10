namespace SuperScan
{
    partial class AutoRunForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartingDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartUpFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ShutDownFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.StartUpFilePathBox = new System.Windows.Forms.TextBox();
            this.StartUpBrowseButton = new System.Windows.Forms.Button();
            this.ShutDownBrowseButton = new System.Windows.Forms.Button();
            this.ShutDownFilePathBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.StageSystemFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.StageSystemBrowseButton = new System.Windows.Forms.Button();
            this.StageSystemFilePathBox = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.StagingDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ShutdownDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartingDateTimePicker
            // 
            this.StartingDateTimePicker.Checked = false;
            this.StartingDateTimePicker.CustomFormat = "MMM dd HH:mm";
            this.StartingDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartingDateTimePicker.Location = new System.Drawing.Point(10, 21);
            this.StartingDateTimePicker.Name = "StartingDateTimePicker";
            this.StartingDateTimePicker.ShowCheckBox = true;
            this.StartingDateTimePicker.ShowUpDown = true;
            this.StartingDateTimePicker.Size = new System.Drawing.Size(117, 20);
            this.StartingDateTimePicker.TabIndex = 0;
            // 
            // StartUpFilePathBox
            // 
            this.StartUpFilePathBox.Location = new System.Drawing.Point(8, 19);
            this.StartUpFilePathBox.Name = "StartUpFilePathBox";
            this.StartUpFilePathBox.Size = new System.Drawing.Size(473, 20);
            this.StartUpFilePathBox.TabIndex = 1;
            // 
            // StartUpBrowseButton
            // 
            this.StartUpBrowseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StartUpBrowseButton.Location = new System.Drawing.Point(487, 16);
            this.StartUpBrowseButton.Name = "StartUpBrowseButton";
            this.StartUpBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.StartUpBrowseButton.TabIndex = 2;
            this.StartUpBrowseButton.Text = "Browse";
            this.StartUpBrowseButton.UseVisualStyleBackColor = true;
            this.StartUpBrowseButton.Click += new System.EventHandler(this.StartUpBrowseButton_Click);
            // 
            // ShutDownBrowseButton
            // 
            this.ShutDownBrowseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ShutDownBrowseButton.Location = new System.Drawing.Point(483, 16);
            this.ShutDownBrowseButton.Name = "ShutDownBrowseButton";
            this.ShutDownBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.ShutDownBrowseButton.TabIndex = 4;
            this.ShutDownBrowseButton.Text = "Browse";
            this.ShutDownBrowseButton.UseVisualStyleBackColor = true;
            this.ShutDownBrowseButton.Click += new System.EventHandler(this.ShutDownBrowseButton_Click);
            // 
            // ShutDownFilePathBox
            // 
            this.ShutDownFilePathBox.Location = new System.Drawing.Point(6, 19);
            this.ShutDownFilePathBox.Name = "ShutDownFilePathBox";
            this.ShutDownFilePathBox.Size = new System.Drawing.Size(471, 20);
            this.ShutDownFilePathBox.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox1.Controls.Add(this.StartUpBrowseButton);
            this.groupBox1.Controls.Add(this.StartUpFilePathBox);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(173, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 47);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Execute on Scan Start Up";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox2.Controls.Add(this.ShutDownBrowseButton);
            this.groupBox2.Controls.Add(this.ShutDownFilePathBox);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(173, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 47);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Execute on Shut Down";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox3.Controls.Add(this.StartingDateTimePicker);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 63);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(133, 47);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Starting Time";
            // 
            // OKButton
            // 
            this.OKButton.BackColor = System.Drawing.Color.LightGreen;
            this.OKButton.Location = new System.Drawing.Point(359, 171);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = false;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox4.Controls.Add(this.StageSystemBrowseButton);
            this.groupBox4.Controls.Add(this.StageSystemFilePathBox);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(173, 10);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(568, 47);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Execute At System Staging";
            // 
            // StageSystemBrowseButton
            // 
            this.StageSystemBrowseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StageSystemBrowseButton.Location = new System.Drawing.Point(483, 16);
            this.StageSystemBrowseButton.Name = "StageSystemBrowseButton";
            this.StageSystemBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.StageSystemBrowseButton.TabIndex = 4;
            this.StageSystemBrowseButton.Text = "Browse";
            this.StageSystemBrowseButton.UseVisualStyleBackColor = true;
            this.StageSystemBrowseButton.Click += new System.EventHandler(this.StageSystemBrowseButton_Click);
            // 
            // StageSystemFilePathBox
            // 
            this.StageSystemFilePathBox.AccessibleDescription = "";
            this.StageSystemFilePathBox.Location = new System.Drawing.Point(6, 19);
            this.StageSystemFilePathBox.Name = "StageSystemFilePathBox";
            this.StageSystemFilePathBox.Size = new System.Drawing.Size(471, 20);
            this.StageSystemFilePathBox.TabIndex = 3;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox5.Controls.Add(this.StagingDateTimePicker);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(12, 10);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(133, 47);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Staging Time";
            // 
            // StagingDateTimePicker
            // 
            this.StagingDateTimePicker.Checked = false;
            this.StagingDateTimePicker.CustomFormat = "MMM dd HH:mm";
            this.StagingDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StagingDateTimePicker.Location = new System.Drawing.Point(10, 21);
            this.StagingDateTimePicker.Name = "StagingDateTimePicker";
            this.StagingDateTimePicker.ShowCheckBox = true;
            this.StagingDateTimePicker.ShowUpDown = true;
            this.StagingDateTimePicker.Size = new System.Drawing.Size(117, 20);
            this.StagingDateTimePicker.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox6.Controls.Add(this.ShutdownDateTimePicker);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(12, 116);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(133, 47);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ending Time";
            // 
            // ShutdownDateTimePicker
            // 
            this.ShutdownDateTimePicker.Checked = false;
            this.ShutdownDateTimePicker.CustomFormat = "MMM dd HH:mm";
            this.ShutdownDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ShutdownDateTimePicker.Location = new System.Drawing.Point(10, 21);
            this.ShutdownDateTimePicker.Name = "ShutdownDateTimePicker";
            this.ShutdownDateTimePicker.ShowCheckBox = true;
            this.ShutdownDateTimePicker.ShowUpDown = true;
            this.ShutdownDateTimePicker.Size = new System.Drawing.Size(117, 20);
            this.ShutdownDateTimePicker.TabIndex = 0;
            // 
            // AutoRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(753, 206);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "AutoRunForm";
            this.Text = "AutoRun Configuration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker StartingDateTimePicker;
        private System.Windows.Forms.OpenFileDialog StartUpFileDialog;
        private System.Windows.Forms.OpenFileDialog ShutDownFileDialog;
        private System.Windows.Forms.TextBox StartUpFilePathBox;
        private System.Windows.Forms.Button StartUpBrowseButton;
        private System.Windows.Forms.Button ShutDownBrowseButton;
        private System.Windows.Forms.TextBox ShutDownFilePathBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.OpenFileDialog StageSystemFileDialog;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button StageSystemBrowseButton;
        private System.Windows.Forms.TextBox StageSystemFilePathBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DateTimePicker StagingDateTimePicker;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DateTimePicker ShutdownDateTimePicker;
    }
}