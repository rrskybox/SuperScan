namespace SuperScan
{
    partial class FormEnterGalaxyNumber
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
            this.NGCnumberTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NGCOKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NGCnumberTextBox
            // 
            this.NGCnumberTextBox.Location = new System.Drawing.Point(48, 12);
            this.NGCnumberTextBox.Name = "NGCnumberTextBox";
            this.NGCnumberTextBox.Size = new System.Drawing.Size(71, 20);
            this.NGCnumberTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "NGC";
            // 
            // NGCOKButton
            // 
            this.NGCOKButton.Location = new System.Drawing.Point(30, 38);
            this.NGCOKButton.Name = "NGCOKButton";
            this.NGCOKButton.Size = new System.Drawing.Size(75, 23);
            this.NGCOKButton.TabIndex = 2;
            this.NGCOKButton.Text = "OK";
            this.NGCOKButton.UseVisualStyleBackColor = true;
            this.NGCOKButton.Click += new System.EventHandler(this.NGCOKButton_Click);
            // 
            // FormEnterGalaxyNumber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(131, 72);
            this.ControlBox = false;
            this.Controls.Add(this.NGCOKButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NGCnumberTextBox);
            this.Name = "FormEnterGalaxyNumber";
            this.Text = "Galaxy Number";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button NGCOKButton;
        public System.Windows.Forms.TextBox NGCnumberTextBox;
    }
}