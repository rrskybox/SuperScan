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
            this.SuspendLayout();
            // 
            // SuspectListbox
            // 
            this.SuspectListbox.FormattingEnabled = true;
            this.SuspectListbox.Location = new System.Drawing.Point(12, 12);
            this.SuspectListbox.Name = "SuspectListbox";
            this.SuspectListbox.Size = new System.Drawing.Size(260, 381);
            this.SuspectListbox.TabIndex = 0;
            this.SuspectListbox.SelectedIndexChanged += new System.EventHandler(this.SuspectListbox_SelectedIndexChanged);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.LightGreen;
            this.CloseButton.Location = new System.Drawing.Point(99, 409);
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
            // SuspectReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(284, 444);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SuspectListbox);
            this.Name = "SuspectReportForm";
            this.Text = "SuperScan Suspects";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox SuspectListbox;
        private System.Windows.Forms.Button CloseButton;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
    }
}