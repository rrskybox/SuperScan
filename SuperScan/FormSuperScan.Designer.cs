namespace SuperScan
{
    partial class FormSuperScan
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
            this.StartScanButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ExposureTimeSetting = new System.Windows.Forms.NumericUpDown();
            this.MinAltitudeSetting = new System.Windows.Forms.NumericUpDown();
            this.MinAltitudeLabel = new System.Windows.Forms.Label();
            this.ExposureTimeLabel = new System.Windows.Forms.Label();
            this.GalaxyCountLabel = new System.Windows.Forms.Label();
            this.CurrentGalaxyLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FilterNumberSetting = new System.Windows.Forms.NumericUpDown();
            this.GalaxyCount = new System.Windows.Forms.Label();
            this.CurrentGalaxyName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentGalaxySizeArcmin = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RefreshTargetsCheckBox = new System.Windows.Forms.CheckBox();
            this.DomeCheckBox = new System.Windows.Forms.CheckBox();
            this.WatchWeatherCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MinGalaxySetting = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.CCDTemperatureSetting = new System.Windows.Forms.NumericUpDown();
            this.AutoFocusCheckBox = new System.Windows.Forms.CheckBox();
            this.OnTopCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoRunCheckBox = new System.Windows.Forms.CheckBox();
            this.PostponeDetectionCheckBox = new System.Windows.Forms.CheckBox();
            this.ReScanButton = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.SuspectsButton = new System.Windows.Forms.Button();
            this.AbortButton = new System.Windows.Forms.Button();
            this.CullButton = new System.Windows.Forms.Button();
            this.SuspectCountLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinAltitudeSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterNumberSetting)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinGalaxySetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDTemperatureSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // StartScanButton
            // 
            this.StartScanButton.BackColor = System.Drawing.Color.LightGreen;
            this.StartScanButton.Location = new System.Drawing.Point(12, 650);
            this.StartScanButton.Name = "StartScanButton";
            this.StartScanButton.Size = new System.Drawing.Size(75, 54);
            this.StartScanButton.TabIndex = 0;
            this.StartScanButton.Text = "Scan\r\nand\r\nDectect";
            this.StartScanButton.UseVisualStyleBackColor = false;
            this.StartScanButton.Click += new System.EventHandler(this.StartScanButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.LightGreen;
            this.CloseButton.Location = new System.Drawing.Point(196, 681);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ExposureTimeSetting
            // 
            this.ExposureTimeSetting.Location = new System.Drawing.Point(185, 44);
            this.ExposureTimeSetting.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.ExposureTimeSetting.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ExposureTimeSetting.Name = "ExposureTimeSetting";
            this.ExposureTimeSetting.Size = new System.Drawing.Size(61, 20);
            this.ExposureTimeSetting.TabIndex = 2;
            this.ExposureTimeSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ExposureTimeSetting.Value = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.ExposureTimeSetting.ValueChanged += new System.EventHandler(this.ExposureTimeSetting_ValueChanged);
            // 
            // MinAltitudeSetting
            // 
            this.MinAltitudeSetting.Location = new System.Drawing.Point(185, 66);
            this.MinAltitudeSetting.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.MinAltitudeSetting.Name = "MinAltitudeSetting";
            this.MinAltitudeSetting.Size = new System.Drawing.Size(61, 20);
            this.MinAltitudeSetting.TabIndex = 3;
            this.MinAltitudeSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MinAltitudeSetting.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.MinAltitudeSetting.ValueChanged += new System.EventHandler(this.MinAltitudeSetting_ValueChanged);
            // 
            // MinAltitudeLabel
            // 
            this.MinAltitudeLabel.AutoSize = true;
            this.MinAltitudeLabel.Location = new System.Drawing.Point(9, 68);
            this.MinAltitudeLabel.Name = "MinAltitudeLabel";
            this.MinAltitudeLabel.Size = new System.Drawing.Size(169, 13);
            this.MinAltitudeLabel.TabIndex = 4;
            this.MinAltitudeLabel.Text = "Minimum Target Altitude (Degrees)";
            this.MinAltitudeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExposureTimeLabel
            // 
            this.ExposureTimeLabel.AutoSize = true;
            this.ExposureTimeLabel.Location = new System.Drawing.Point(9, 46);
            this.ExposureTimeLabel.Name = "ExposureTimeLabel";
            this.ExposureTimeLabel.Size = new System.Drawing.Size(170, 13);
            this.ExposureTimeLabel.TabIndex = 5;
            this.ExposureTimeLabel.Text = "Image Exposure Length (Seconds)";
            // 
            // GalaxyCountLabel
            // 
            this.GalaxyCountLabel.AutoSize = true;
            this.GalaxyCountLabel.Location = new System.Drawing.Point(17, 24);
            this.GalaxyCountLabel.Name = "GalaxyCountLabel";
            this.GalaxyCountLabel.Size = new System.Drawing.Size(91, 13);
            this.GalaxyCountLabel.TabIndex = 6;
            this.GalaxyCountLabel.Text = "Galaxies to Image";
            // 
            // CurrentGalaxyLabel
            // 
            this.CurrentGalaxyLabel.AutoSize = true;
            this.CurrentGalaxyLabel.Location = new System.Drawing.Point(17, 48);
            this.CurrentGalaxyLabel.Name = "CurrentGalaxyLabel";
            this.CurrentGalaxyLabel.Size = new System.Drawing.Size(76, 13);
            this.CurrentGalaxyLabel.TabIndex = 7;
            this.CurrentGalaxyLabel.Text = "Current Galaxy";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Filter Number  (Zero-based)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FilterNumberSetting
            // 
            this.FilterNumberSetting.Location = new System.Drawing.Point(185, 88);
            this.FilterNumberSetting.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.FilterNumberSetting.Name = "FilterNumberSetting";
            this.FilterNumberSetting.Size = new System.Drawing.Size(61, 20);
            this.FilterNumberSetting.TabIndex = 9;
            this.FilterNumberSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FilterNumberSetting.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.FilterNumberSetting.ValueChanged += new System.EventHandler(this.FilterNumberSetting_ValueChanged);
            // 
            // GalaxyCount
            // 
            this.GalaxyCount.AutoSize = true;
            this.GalaxyCount.Location = new System.Drawing.Point(190, 24);
            this.GalaxyCount.Name = "GalaxyCount";
            this.GalaxyCount.Size = new System.Drawing.Size(29, 13);
            this.GalaxyCount.TabIndex = 10;
            this.GalaxyCount.Text = "TBD";
            // 
            // CurrentGalaxyName
            // 
            this.CurrentGalaxyName.AutoSize = true;
            this.CurrentGalaxyName.Location = new System.Drawing.Point(190, 48);
            this.CurrentGalaxyName.Name = "CurrentGalaxyName";
            this.CurrentGalaxyName.Size = new System.Drawing.Size(29, 13);
            this.CurrentGalaxyName.TabIndex = 11;
            this.CurrentGalaxyName.Text = "TBD";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Current Galaxy Size (Arcmin)";
            // 
            // CurrentGalaxySizeArcmin
            // 
            this.CurrentGalaxySizeArcmin.AutoSize = true;
            this.CurrentGalaxySizeArcmin.Location = new System.Drawing.Point(190, 72);
            this.CurrentGalaxySizeArcmin.Name = "CurrentGalaxySizeArcmin";
            this.CurrentGalaxySizeArcmin.Size = new System.Drawing.Size(29, 13);
            this.CurrentGalaxySizeArcmin.TabIndex = 14;
            this.CurrentGalaxySizeArcmin.Text = "TBD";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox1.Controls.Add(this.CurrentGalaxySizeArcmin);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.GalaxyCountLabel);
            this.groupBox1.Controls.Add(this.CurrentGalaxyName);
            this.groupBox1.Controls.Add(this.CurrentGalaxyLabel);
            this.groupBox1.Controls.Add(this.GalaxyCount);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 94);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox2.Controls.Add(this.RefreshTargetsCheckBox);
            this.groupBox2.Controls.Add(this.DomeCheckBox);
            this.groupBox2.Controls.Add(this.WatchWeatherCheckBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.MinGalaxySetting);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CCDTemperatureSetting);
            this.groupBox2.Controls.Add(this.AutoFocusCheckBox);
            this.groupBox2.Controls.Add(this.OnTopCheckBox);
            this.groupBox2.Controls.Add(this.AutoRunCheckBox);
            this.groupBox2.Controls.Add(this.PostponeDetectionCheckBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.FilterNumberSetting);
            this.groupBox2.Controls.Add(this.ExposureTimeLabel);
            this.groupBox2.Controls.Add(this.MinAltitudeLabel);
            this.groupBox2.Controls.Add(this.ExposureTimeSetting);
            this.groupBox2.Controls.Add(this.MinAltitudeSetting);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 243);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration";
            // 
            // RefreshTargetsCheckBox
            // 
            this.RefreshTargetsCheckBox.AutoSize = true;
            this.RefreshTargetsCheckBox.Checked = true;
            this.RefreshTargetsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RefreshTargetsCheckBox.Location = new System.Drawing.Point(31, 212);
            this.RefreshTargetsCheckBox.Name = "RefreshTargetsCheckBox";
            this.RefreshTargetsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RefreshTargetsCheckBox.Size = new System.Drawing.Size(102, 17);
            this.RefreshTargetsCheckBox.TabIndex = 21;
            this.RefreshTargetsCheckBox.Text = "Refresh Targets";
            this.RefreshTargetsCheckBox.UseVisualStyleBackColor = true;
            this.RefreshTargetsCheckBox.CheckedChanged += new System.EventHandler(this.RefreshTargetsCheckBox_CheckedChanged);
            // 
            // DomeCheckBox
            // 
            this.DomeCheckBox.AutoSize = true;
            this.DomeCheckBox.Location = new System.Drawing.Point(173, 189);
            this.DomeCheckBox.Name = "DomeCheckBox";
            this.DomeCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DomeCheckBox.Size = new System.Drawing.Size(54, 17);
            this.DomeCheckBox.TabIndex = 20;
            this.DomeCheckBox.Text = "Dome";
            this.DomeCheckBox.UseVisualStyleBackColor = true;
            this.DomeCheckBox.CheckedChanged += new System.EventHandler(this.DomeCheckBox_CheckedChanged);
            // 
            // WatchWeatherCheckBox
            // 
            this.WatchWeatherCheckBox.AutoSize = true;
            this.WatchWeatherCheckBox.Location = new System.Drawing.Point(29, 189);
            this.WatchWeatherCheckBox.Name = "WatchWeatherCheckBox";
            this.WatchWeatherCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.WatchWeatherCheckBox.Size = new System.Drawing.Size(102, 17);
            this.WatchWeatherCheckBox.TabIndex = 19;
            this.WatchWeatherCheckBox.Text = "Watch Weather";
            this.WatchWeatherCheckBox.UseVisualStyleBackColor = true;
            this.WatchWeatherCheckBox.CheckedChanged += new System.EventHandler(this.WatchWeatherCheckBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(9, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Minimum Galaxy Size (arc sec)";
            // 
            // MinGalaxySetting
            // 
            this.MinGalaxySetting.Enabled = false;
            this.MinGalaxySetting.ForeColor = System.Drawing.SystemColors.GrayText;
            this.MinGalaxySetting.Location = new System.Drawing.Point(185, 23);
            this.MinGalaxySetting.Name = "MinGalaxySetting";
            this.MinGalaxySetting.Size = new System.Drawing.Size(61, 20);
            this.MinGalaxySetting.TabIndex = 17;
            this.MinGalaxySetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MinGalaxySetting.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.MinGalaxySetting.ValueChanged += new System.EventHandler(this.MinGalaxySetting_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Camera Temperature";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CCDTemperatureSetting
            // 
            this.CCDTemperatureSetting.Location = new System.Drawing.Point(185, 111);
            this.CCDTemperatureSetting.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.CCDTemperatureSetting.Name = "CCDTemperatureSetting";
            this.CCDTemperatureSetting.Size = new System.Drawing.Size(61, 20);
            this.CCDTemperatureSetting.TabIndex = 16;
            this.CCDTemperatureSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CCDTemperatureSetting.Value = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.CCDTemperatureSetting.ValueChanged += new System.EventHandler(this.CCDTemperatureSetting_ValueChanged);
            // 
            // AutoFocusCheckBox
            // 
            this.AutoFocusCheckBox.AutoSize = true;
            this.AutoFocusCheckBox.Location = new System.Drawing.Point(150, 166);
            this.AutoFocusCheckBox.Name = "AutoFocusCheckBox";
            this.AutoFocusCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoFocusCheckBox.Size = new System.Drawing.Size(77, 17);
            this.AutoFocusCheckBox.TabIndex = 14;
            this.AutoFocusCheckBox.Text = "AutoFocus";
            this.AutoFocusCheckBox.UseVisualStyleBackColor = true;
            this.AutoFocusCheckBox.CheckedChanged += new System.EventHandler(this.AutoFocusCheckBox_CheckedChanged);
            // 
            // OnTopCheckBox
            // 
            this.OnTopCheckBox.AutoSize = true;
            this.OnTopCheckBox.Location = new System.Drawing.Point(33, 166);
            this.OnTopCheckBox.Name = "OnTopCheckBox";
            this.OnTopCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.OnTopCheckBox.Size = new System.Drawing.Size(98, 17);
            this.OnTopCheckBox.TabIndex = 13;
            this.OnTopCheckBox.Text = "Always On Top";
            this.OnTopCheckBox.UseVisualStyleBackColor = true;
            this.OnTopCheckBox.CheckedChanged += new System.EventHandler(this.OnTopCheckBox_CheckedChanged);
            // 
            // AutoRunCheckBox
            // 
            this.AutoRunCheckBox.AutoSize = true;
            this.AutoRunCheckBox.Location = new System.Drawing.Point(159, 143);
            this.AutoRunCheckBox.Name = "AutoRunCheckBox";
            this.AutoRunCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.AutoRunCheckBox.Size = new System.Drawing.Size(68, 17);
            this.AutoRunCheckBox.TabIndex = 12;
            this.AutoRunCheckBox.Text = "AutoRun";
            this.AutoRunCheckBox.UseVisualStyleBackColor = true;
            this.AutoRunCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartCheckBox_CheckedChanged);
            // 
            // PostponeDetectionCheckBox
            // 
            this.PostponeDetectionCheckBox.AutoSize = true;
            this.PostponeDetectionCheckBox.Location = new System.Drawing.Point(11, 143);
            this.PostponeDetectionCheckBox.Name = "PostponeDetectionCheckBox";
            this.PostponeDetectionCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PostponeDetectionCheckBox.Size = new System.Drawing.Size(120, 17);
            this.PostponeDetectionCheckBox.TabIndex = 11;
            this.PostponeDetectionCheckBox.Text = "Postpone Detection";
            this.PostponeDetectionCheckBox.UseVisualStyleBackColor = true;
            this.PostponeDetectionCheckBox.CheckedChanged += new System.EventHandler(this.PostponeDetectionCheckBox_CheckedChanged);
            // 
            // ReScanButton
            // 
            this.ReScanButton.BackColor = System.Drawing.Color.LightGreen;
            this.ReScanButton.Location = new System.Drawing.Point(105, 650);
            this.ReScanButton.Name = "ReScanButton";
            this.ReScanButton.Size = new System.Drawing.Size(75, 23);
            this.ReScanButton.TabIndex = 17;
            this.ReScanButton.Text = "Detect";
            this.ReScanButton.UseVisualStyleBackColor = false;
            this.ReScanButton.Click += new System.EventHandler(this.ReScanButton_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.LightSeaGreen;
            this.LogBox.ForeColor = System.Drawing.Color.White;
            this.LogBox.Location = new System.Drawing.Point(12, 361);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(260, 245);
            this.LogBox.TabIndex = 18;
            // 
            // SuspectsButton
            // 
            this.SuspectsButton.BackColor = System.Drawing.Color.LightGreen;
            this.SuspectsButton.Location = new System.Drawing.Point(105, 681);
            this.SuspectsButton.Name = "SuspectsButton";
            this.SuspectsButton.Size = new System.Drawing.Size(75, 23);
            this.SuspectsButton.TabIndex = 19;
            this.SuspectsButton.Text = "Suspects";
            this.SuspectsButton.UseVisualStyleBackColor = false;
            this.SuspectsButton.Click += new System.EventHandler(this.SuspectsButton_Click);
            // 
            // AbortButton
            // 
            this.AbortButton.BackColor = System.Drawing.Color.LightGreen;
            this.AbortButton.Location = new System.Drawing.Point(196, 621);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(75, 23);
            this.AbortButton.TabIndex = 20;
            this.AbortButton.Text = "Abort";
            this.AbortButton.UseVisualStyleBackColor = false;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // CullButton
            // 
            this.CullButton.BackColor = System.Drawing.Color.LightGreen;
            this.CullButton.Location = new System.Drawing.Point(196, 650);
            this.CullButton.Name = "CullButton";
            this.CullButton.Size = new System.Drawing.Size(75, 23);
            this.CullButton.TabIndex = 21;
            this.CullButton.Text = "Cull";
            this.CullButton.UseVisualStyleBackColor = false;
            this.CullButton.Click += new System.EventHandler(this.CullButton_Click);
            // 
            // SuspectCountLabel
            // 
            this.SuspectCountLabel.AutoSize = true;
            this.SuspectCountLabel.ForeColor = System.Drawing.Color.White;
            this.SuspectCountLabel.Location = new System.Drawing.Point(98, 621);
            this.SuspectCountLabel.Name = "SuspectCountLabel";
            this.SuspectCountLabel.Size = new System.Drawing.Size(29, 13);
            this.SuspectCountLabel.TabIndex = 23;
            this.SuspectCountLabel.Text = "TBD";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 621);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Suspects";
            // 
            // FormSuperScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(284, 716);
            this.Controls.Add(this.SuspectCountLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CullButton);
            this.Controls.Add(this.AbortButton);
            this.Controls.Add(this.SuspectsButton);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.ReScanButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.StartScanButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "FormSuperScan";
            this.Text = "SuperScan V 2.5";
            this.Load += new System.EventHandler(this.SuperScanForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinAltitudeSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterNumberSetting)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinGalaxySetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDTemperatureSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartScanButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.NumericUpDown ExposureTimeSetting;
        private System.Windows.Forms.NumericUpDown MinAltitudeSetting;
        private System.Windows.Forms.Label MinAltitudeLabel;
        private System.Windows.Forms.Label ExposureTimeLabel;
        private System.Windows.Forms.Label GalaxyCountLabel;
        private System.Windows.Forms.Label CurrentGalaxyLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown FilterNumberSetting;
        private System.Windows.Forms.Label GalaxyCount;
        private System.Windows.Forms.Label CurrentGalaxyName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CurrentGalaxySizeArcmin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ReScanButton;
        private System.Windows.Forms.CheckBox PostponeDetectionCheckBox;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.CheckBox AutoRunCheckBox;
        private System.Windows.Forms.CheckBox OnTopCheckBox;
        private System.Windows.Forms.Button SuspectsButton;
        private System.Windows.Forms.CheckBox AutoFocusCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown CCDTemperatureSetting;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown MinGalaxySetting;
        private System.Windows.Forms.CheckBox WatchWeatherCheckBox;
        private System.Windows.Forms.Button CullButton;
        private System.Windows.Forms.CheckBox DomeCheckBox;
        private System.Windows.Forms.Label SuspectCountLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox RefreshTargetsCheckBox;
    }
}

