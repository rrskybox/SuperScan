namespace SuperScan
{
    partial class SuspectReportForm
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
            this.SuspectListbox = new System.Windows.Forms.ListBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.BlinkButton = new System.Windows.Forms.Button();
            this.LocationTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SuspectListbox
            // 
            this.SuspectListbox.FormattingEnabled = true;
            this.SuspectListbox.Location = new System.Drawing.Point(12, 12);
            this.SuspectListbox.Name = "SuspectListbox";
            this.SuspectListbox.Size = new System.Drawing.Size(393, 56);
            this.SuspectListbox.TabIndex = 0;
            this.SuspectListbox.SelectedIndexChanged += new System.EventHandler(this.SuspectListbox_SelectedIndexChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.LightGreen;
            this.CloseButton.Location = new System.Drawing.Point(333, 515);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // ImagePictureBox
            // 
            this.ImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImagePictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.ImagePictureBox.Location = new System.Drawing.Point(15, 180);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(393, 329);
            this.ImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImagePictureBox.TabIndex = 2;
            this.ImagePictureBox.TabStop = false;
            // 
            // BlinkButton
            // 
            this.BlinkButton.Location = new System.Drawing.Point(15, 515);
            this.BlinkButton.Name = "BlinkButton";
            this.BlinkButton.Size = new System.Drawing.Size(75, 23);
            this.BlinkButton.TabIndex = 3;
            this.BlinkButton.Text = "Blink";
            this.BlinkButton.UseVisualStyleBackColor = true;
            this.BlinkButton.Click += new System.EventHandler(this.BlinkButton_Click);
            // 
            // LocationTextBox
            // 
            this.LocationTextBox.Location = new System.Drawing.Point(82, 74);
            this.LocationTextBox.Name = "LocationTextBox";
            this.LocationTextBox.Size = new System.Drawing.Size(323, 20);
            this.LocationTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Suspect at: ";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(168, 515);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 6;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Location = new System.Drawing.Point(82, 103);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(323, 71);
            this.NotesTextBox.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Notes:";
            // 
            // SuspectReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(418, 547);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NotesTextBox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LocationTextBox);
            this.Controls.Add(this.BlinkButton);
            this.Controls.Add(this.ImagePictureBox);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SuspectListbox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "SuspectReportForm";
            this.Text = "SuperScan Suspects";
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SuspectListbox;
        private System.Windows.Forms.Button CloseButton;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.PictureBox ImagePictureBox;
        private System.Windows.Forms.Button BlinkButton;
        private System.Windows.Forms.TextBox LocationTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.Label label2;
    }
}