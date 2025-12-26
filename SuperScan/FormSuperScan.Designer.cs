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
            this.label8 = new System.Windows.Forms.Label();
            this.FollowUpExposureTime = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.CLSReductionBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RefreshTargetsCheckBox = new System.Windows.Forms.CheckBox();
            this.DomeCheckBox = new System.Windows.Forms.CheckBox();
            this.ImageReductionBox = new System.Windows.Forms.ComboBox();
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
            this.label9 = new System.Windows.Forms.Label();
            this.RefocusTriggerBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinAltitudeSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterNumberSetting)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FollowUpExposureTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinGalaxySetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDTemperatureSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefocusTriggerBox)).BeginInit();
            this.SuspendLayout();
            // 
            // StartScanButton
            // 
            this.StartScanButton.BackColor = System.Drawing.Color.LightGreen;
            this.StartScanButton.Location = new System.Drawing.Point(13, 748);
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
            this.CloseButton.Location = new System.Drawing.Point(197, 779);
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
            this.MinAltitudeSetting.Location = new System.Drawing.Point(185, 86);
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
            this.MinAltitudeLabel.Location = new System.Drawing.Point(10, 88);
            this.MinAltitudeLabel.Name = "MinAltitudeLabel";
            this.MinAltitudeLabel.Size = new System.Drawing.Size(169, 13);
            this.MinAltitudeLabel.TabIndex = 4;
            this.MinAltitudeLabel.Text = "Minimum Target Altitude (Degrees)";
            this.MinAltitudeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ExposureTimeLabel
            // 
            this.ExposureTimeLabel.AutoSize = true;
            this.ExposureTimeLabel.Location = new System.Drawing.Point(10, 46);
            this.ExposureTimeLabel.Name = "ExposureTimeLabel";
            this.ExposureTimeLabel.Size = new System.Drawing.Size(134, 13);
            this.ExposureTimeLabel.TabIndex = 5;
            this.ExposureTimeLabel.Text = "Image Exposure (Seconds)";
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
            this.label1.Location = new System.Drawing.Point(10, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Filter Number  (Zero-based)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FilterNumberSetting
            // 
            this.FilterNumberSetting.Location = new System.Drawing.Point(185, 107);
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
            this.groupBox2.Controls.Add(this.RefocusTriggerBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.FollowUpExposureTime);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.CLSReductionBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.RefreshTargetsCheckBox);
            this.groupBox2.Controls.Add(this.DomeCheckBox);
            this.groupBox2.Controls.Add(this.ImageReductionBox);
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
            this.groupBox2.Size = new System.Drawing.Size(260, 346);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(152, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Follow Up Exposure (Seconds)";
            // 
            // FollowUpExposureTime
            // 
            this.FollowUpExposureTime.Location = new System.Drawing.Point(185, 65);
            this.FollowUpExposureTime.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.FollowUpExposureTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FollowUpExposureTime.Name = "FollowUpExposureTime";
            this.FollowUpExposureTime.Size = new System.Drawing.Size(61, 20);
            this.FollowUpExposureTime.TabIndex = 28;
            this.FollowUpExposureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FollowUpExposureTime.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.FollowUpExposureTime.ValueChanged += new System.EventHandler(this.FollowUpExposureTime_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "CLS Reduction";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CLSReductionBox
            // 
            this.CLSReductionBox.FormattingEnabled = true;
            this.CLSReductionBox.Items.AddRange(new object[] {
            "None",
            "Auto"});
            this.CLSReductionBox.Location = new System.Drawing.Point(185, 171);
            this.CLSReductionBox.Name = "CLSReductionBox";
            this.CLSReductionBox.Size = new System.Drawing.Size(62, 21);
            this.CLSReductionBox.TabIndex = 26;
            this.CLSReductionBox.SelectedIndexChanged += new System.EventHandler(this.CLSReductionBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Image Reduction";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RefreshTargetsCheckBox
            // 
            this.RefreshTargetsCheckBox.AutoSize = true;
            this.RefreshTargetsCheckBox.Location = new System.Drawing.Point(13, 308);
            this.RefreshTargetsCheckBox.Name = "RefreshTargetsCheckBox";
            this.RefreshTargetsCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RefreshTargetsCheckBox.Size = new System.Drawing.Size(102, 17);
            this.RefreshTargetsCheckBox.TabIndex = 21;
            this.RefreshTargetsCheckBox.Text = "Refresh Targets";
            this.RefreshTargetsCheckBox.UseVisualStyleBackColor = true;
            this.RefreshTargetsCheckBox.CheckedChanged += new System.EventHandler(this.RefreshTargetsCheckBox_CheckedChanged);
            // 
            // DomeCheckBox
            // 
            this.DomeCheckBox.AutoSize = true;
            this.DomeCheckBox.Location = new System.Drawing.Point(170, 308);
            this.DomeCheckBox.Name = "DomeCheckBox";
            this.DomeCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DomeCheckBox.Size = new System.Drawing.Size(76, 17);
            this.DomeCheckBox.TabIndex = 20;
            this.DomeCheckBox.Text = "Has Dome";
            this.DomeCheckBox.UseVisualStyleBackColor = true;
            this.DomeCheckBox.CheckedChanged += new System.EventHandler(this.DomeCheckBox_CheckedChanged);
            // 
            // ImageReductionBox
            // 
            this.ImageReductionBox.FormattingEnabled = true;
            this.ImageReductionBox.Items.AddRange(new object[] {
            "None",
            "Auto",
            "Full"});
            this.ImageReductionBox.Location = new System.Drawing.Point(185, 149);
            this.ImageReductionBox.Name = "ImageReductionBox";
            this.ImageReductionBox.Size = new System.Drawing.Size(62, 21);
            this.ImageReductionBox.TabIndex = 24;
            this.ImageReductionBox.SelectedIndexChanged += new System.EventHandler(this.CalibrationListBox_SelectedIndexChanged);
            // 
            // WatchWeatherCheckBox
            // 
            this.WatchWeatherCheckBox.AutoSize = true;
            this.WatchWeatherCheckBox.Location = new System.Drawing.Point(13, 287);
            this.WatchWeatherCheckBox.Name = "WatchWeatherCheckBox";
            this.WatchWeatherCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.WatchWeatherCheckBox.Size = new System.Drawing.Size(102, 17);
            this.WatchWeatherCheckBox.TabIndex = 19;
            this.WatchWeatherCheckBox.Text = "Watch Weather";
            this.WatchWeatherCheckBox.UseVisualStyleBackColor = true;
            this.WatchWeatherCheckBox.CheckedChanged += new System.EventHandler(this.WatchWeatherCheckBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Minimum Galaxy Size (arcmin)";
            // 
            // MinGalaxySetting
            // 
            this.MinGalaxySetting.DecimalPlaces = 1;
            this.MinGalaxySetting.ForeColor = System.Drawing.SystemColors.WindowText;
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
            this.label3.Location = new System.Drawing.Point(10, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Camera Temperature";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CCDTemperatureSetting
            // 
            this.CCDTemperatureSetting.Location = new System.Drawing.Point(185, 128);
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
            this.AutoFocusCheckBox.Location = new System.Drawing.Point(170, 287);
            this.AutoFocusCheckBox.Name = "AutoFocusCheckBox";
            this.AutoFocusCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AutoFocusCheckBox.Size = new System.Drawing.Size(77, 17);
            this.AutoFocusCheckBox.TabIndex = 14;
            this.AutoFocusCheckBox.Text = "AutoFocus";
            this.AutoFocusCheckBox.UseVisualStyleBackColor = true;
            this.AutoFocusCheckBox.CheckedChanged += new System.EventHandler(this.AutoFocusCheckBox_CheckedChanged);
            // 
            // OnTopCheckBox
            // 
            this.OnTopCheckBox.AutoSize = true;
            this.OnTopCheckBox.Location = new System.Drawing.Point(13, 264);
            this.OnTopCheckBox.Name = "OnTopCheckBox";
            this.OnTopCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.OnTopCheckBox.Size = new System.Drawing.Size(98, 17);
            this.OnTopCheckBox.TabIndex = 13;
            this.OnTopCheckBox.Text = "Always On Top";
            this.OnTopCheckBox.UseVisualStyleBackColor = true;
            this.OnTopCheckBox.CheckedChanged += new System.EventHandler(this.OnTopCheckBox_CheckedChanged);
            // 
            // AutoRunCheckBox
            // 
            this.AutoRunCheckBox.AutoSize = true;
            this.AutoRunCheckBox.Location = new System.Drawing.Point(170, 264);
            this.AutoRunCheckBox.Name = "AutoRunCheckBox";
            this.AutoRunCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AutoRunCheckBox.Size = new System.Drawing.Size(68, 17);
            this.AutoRunCheckBox.TabIndex = 12;
            this.AutoRunCheckBox.Text = "AutoRun";
            this.AutoRunCheckBox.UseVisualStyleBackColor = true;
            this.AutoRunCheckBox.CheckedChanged += new System.EventHandler(this.AutoStartCheckBox_CheckedChanged);
            // 
            // PostponeDetectionCheckBox
            // 
            this.PostponeDetectionCheckBox.AutoSize = true;
            this.PostponeDetectionCheckBox.Location = new System.Drawing.Point(13, 237);
            this.PostponeDetectionCheckBox.Name = "PostponeDetectionCheckBox";
            this.PostponeDetectionCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PostponeDetectionCheckBox.Size = new System.Drawing.Size(120, 17);
            this.PostponeDetectionCheckBox.TabIndex = 11;
            this.PostponeDetectionCheckBox.Text = "Postpone Detection";
            this.PostponeDetectionCheckBox.UseVisualStyleBackColor = true;
            this.PostponeDetectionCheckBox.CheckedChanged += new System.EventHandler(this.PostponeDetectionCheckBox_CheckedChanged);
            // 
            // ReScanButton
            // 
            this.ReScanButton.BackColor = System.Drawing.Color.LightGreen;
            this.ReScanButton.Location = new System.Drawing.Point(106, 748);
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
            this.LogBox.Location = new System.Drawing.Point(12, 464);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(260, 249);
            this.LogBox.TabIndex = 18;
            // 
            // SuspectsButton
            // 
            this.SuspectsButton.BackColor = System.Drawing.Color.LightGreen;
            this.SuspectsButton.Location = new System.Drawing.Point(106, 779);
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
            this.AbortButton.Location = new System.Drawing.Point(197, 719);
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
            this.CullButton.Location = new System.Drawing.Point(197, 748);
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
            this.SuspectCountLabel.Location = new System.Drawing.Point(99, 719);
            this.SuspectCountLabel.Name = "SuspectCountLabel";
            this.SuspectCountLabel.Size = new System.Drawing.Size(29, 13);
            this.SuspectCountLabel.TabIndex = 23;
            this.SuspectCountLabel.Text = "TBD";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 719);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Suspects";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Refocus Trigger (deg)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RefocusTriggerBox
            // 
            this.RefocusTriggerBox.DecimalPlaces = 1;
            this.RefocusTriggerBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.RefocusTriggerBox.Location = new System.Drawing.Point(185, 193);
            this.RefocusTriggerBox.Name = "RefocusTriggerBox";
            this.RefocusTriggerBox.Size = new System.Drawing.Size(61, 20);
            this.RefocusTriggerBox.TabIndex = 32;
            this.RefocusTriggerBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RefocusTriggerBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.RefocusTriggerBox.ValueChanged += new System.EventHandler(this.RefocusTriggerBox_ValueChanged);
            // 
            // FormSuperScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(284, 814);
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
            this.Text = "SuperScan Loading...";
            ((System.ComponentModel.ISupportInitialize)(this.ExposureTimeSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinAltitudeSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterNumberSetting)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FollowUpExposureTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinGalaxySetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CCDTemperatureSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RefocusTriggerBox)).EndInit();
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
        private System.Windows.Forms.ComboBox ImageReductionBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CLSReductionBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown FollowUpExposureTime;
        private System.Windows.Forms.NumericUpDown RefocusTriggerBox;
        private System.Windows.Forms.Label label9;
    }
}

