namespace SuperScan
{
    partial class FormChooseGalaxyTarget
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
            this.NGCOKButton = new System.Windows.Forms.Button();
            this.TargetNameList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // NGCOKButton
            // 
            this.NGCOKButton.Location = new System.Drawing.Point(59, 161);
            this.NGCOKButton.Name = "NGCOKButton";
            this.NGCOKButton.Size = new System.Drawing.Size(75, 23);
            this.NGCOKButton.TabIndex = 2;
            this.NGCOKButton.Text = "OK";
            this.NGCOKButton.UseVisualStyleBackColor = true;
            this.NGCOKButton.Click += new System.EventHandler(this.NGCOKButton_Click);
            // 
            // TargetNameList
            // 
            this.TargetNameList.FormattingEnabled = true;
            this.TargetNameList.Location = new System.Drawing.Point(12, 7);
            this.TargetNameList.Name = "TargetNameList";
            this.TargetNameList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.TargetNameList.Size = new System.Drawing.Size(174, 147);
            this.TargetNameList.Sorted = true;
            this.TargetNameList.TabIndex = 3;
            // 
            // FormChooseGalaxyTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(192, 189);
            this.ControlBox = false;
            this.Controls.Add(this.TargetNameList);
            this.Controls.Add(this.NGCOKButton);
            this.Name = "FormChooseGalaxyTarget";
            this.Text = "Select Target Galaxies";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button NGCOKButton;
        public System.Windows.Forms.ListBox TargetNameList;
    }
}