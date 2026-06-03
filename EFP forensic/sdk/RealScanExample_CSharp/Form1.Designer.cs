using System.Windows.Forms;

namespace RealScanUICSharp
{
    partial class Form1
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
            this.InitSDK = new System.Windows.Forms.Button();
            this.MsgPanel = new System.Windows.Forms.TextBox();
            this.PreviewWindow = new System.Windows.Forms.PictureBox();
            this.groupDeviceInfo = new System.Windows.Forms.GroupBox();
            this.brnViewSoftwareLicense = new System.Windows.Forms.Button();
            this.DeviceID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SDKInfo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Hardwareinfo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FirmwareInfo = new System.Windows.Forms.TextBox();
            this.DeviceInfo = new System.Windows.Forms.TextBox();
            this.DeviceList = new System.Windows.Forms.ComboBox();
            this.ExitDevice = new System.Windows.Forms.Button();
            this.InitDevice = new System.Windows.Forms.Button();
            this.FingerLEDOn = new System.Windows.Forms.Button();
            this.groupCapture = new System.Windows.Forms.GroupBox();
            this.customCaptH = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.customCaptW = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.customCaptY = new System.Windows.Forms.TextBox();
            this.customCaptX = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.TakeManual = new System.Windows.Forms.Button();
            this.TakeAuto = new System.Windows.Forms.Button();
            this.Callback = new System.Windows.Forms.ComboBox();
            this.ImageSize = new System.Windows.Forms.TextBox();
            this.StopCapture = new System.Windows.Forms.Button();
            this.AutoCapture = new System.Windows.Forms.CheckBox();
            this.StartCapture = new System.Windows.Forms.Button();
            this.CaptureMode = new System.Windows.Forms.ComboBox();
            this.groupPreview = new System.Windows.Forms.GroupBox();
            this.advCallbackDrawBtn = new System.Windows.Forms.RadioButton();
            this.callbackDrawBtn = new System.Windows.Forms.RadioButton();
            this.directDrawBtn = new System.Windows.Forms.RadioButton();
            this.groupLCD = new System.Windows.Forms.GroupBox();
            this.DisplayLCD = new System.Windows.Forms.Button();
            this.ResetLCD = new System.Windows.Forms.Button();
            this.FingerLEDOff = new System.Windows.Forms.Button();
            this.groupFingerLEDControl = new System.Windows.Forms.GroupBox();
            this.FingerColor = new System.Windows.Forms.ComboBox();
            this.Finger_LED = new System.Windows.Forms.ComboBox();
            this.DeviceTab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.logBox = new System.Windows.Forms.TextBox();
            this.groupMisc = new System.Windows.Forms.GroupBox();
            this.ManualCalibrate = new System.Windows.Forms.Button();
            this.groupSelfTest = new System.Windows.Forms.GroupBox();
            this.TestRun = new System.Windows.Forms.Button();
            this.TestType = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupKey = new System.Windows.Forms.GroupBox();
            this.groupKeyCallback = new System.Windows.Forms.GroupBox();
            this.CALLBACKPRINT = new System.Windows.Forms.TextBox();
            this.KEYCALLBACK = new System.Windows.Forms.ComboBox();
            this.KEYSTATUS = new System.Windows.Forms.TextBox();
            this.ReadKeycallback = new System.Windows.Forms.Button();
            this.ActivateKey = new System.Windows.Forms.Button();
            this.KeyMask = new System.Windows.Forms.ComboBox();
            this.groupStatusLEDControl = new System.Windows.Forms.GroupBox();
            this.StatusLEDColor = new System.Windows.Forms.ComboBox();
            this.StatusLEDOff = new System.Windows.Forms.Button();
            this.StatusLEDOn = new System.Windows.Forms.Button();
            this.groupWav = new System.Windows.Forms.GroupBox();
            this.WavPlay = new System.Windows.Forms.Button();
            this.groupBeep = new System.Windows.Forms.GroupBox();
            this.BeepPattern = new System.Windows.Forms.ComboBox();
            this.EmitBeep = new System.Windows.Forms.Button();
            this.groupModeLEDControl = new System.Windows.Forms.GroupBox();
            this.Mode_LED = new System.Windows.Forms.ComboBox();
            this.ModeLEDOff = new System.Windows.Forms.Button();
            this.ModeLEDOn = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupLFD = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.readLFDLevel = new System.Windows.Forms.Button();
            this.writeLFDLevel = new System.Windows.Forms.Button();
            this.cbLFDLevel = new System.Windows.Forms.ComboBox();
            this.groupCaptureOptionsEtc = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ContrastEnhancement = new System.Windows.Forms.CheckBox();
            this.AutoCalibrate = new System.Windows.Forms.CheckBox();
            this.NoiseReduction = new System.Windows.Forms.CheckBox();
            this.WriteCaptureOption = new System.Windows.Forms.Button();
            this.ReadCaptureOption = new System.Windows.Forms.Button();
            this.ReductionLevel = new System.Windows.Forms.TextBox();
            this.AutoSensitivity = new System.Windows.Forms.ComboBox();
            this.AdvancedContrastEnhancement = new System.Windows.Forms.CheckBox();
            this.groupRollCaptureOption = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TimeoutTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.WriteProfile = new System.Windows.Forms.Button();
            this.ReadProfile = new System.Windows.Forms.Button();
            this.RollDirections = new System.Windows.Forms.ComboBox();
            this.RollProfiles = new System.Windows.Forms.ComboBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ShowAlloverlay = new System.Windows.Forms.Button();
            this.ClearAlloverlay = new System.Windows.Forms.Button();
            this.HideAlloverlay = new System.Windows.Forms.Button();
            this.groupQuadOverlay = new System.Windows.Forms.GroupBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.m_QuadX3 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.m_QuadY3 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.m_QuadX4 = new System.Windows.Forms.TextBox();
            this.m_QuadY4 = new System.Windows.Forms.TextBox();
            this.m_QuadWidth = new System.Windows.Forms.TextBox();
            this.m_QuadY2 = new System.Windows.Forms.TextBox();
            this.m_QuadX2 = new System.Windows.Forms.TextBox();
            this.m_QuadY1 = new System.Windows.Forms.TextBox();
            this.m_QuadX1 = new System.Windows.Forms.TextBox();
            this.SelectQuadColor = new System.Windows.Forms.ComboBox();
            this.ShowQuadrangle = new System.Windows.Forms.Button();
            this.groupLineOverlay = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.m_LineWidth = new System.Windows.Forms.TextBox();
            this.m_LineY2 = new System.Windows.Forms.TextBox();
            this.m_LineX2 = new System.Windows.Forms.TextBox();
            this.m_LineY1 = new System.Windows.Forms.TextBox();
            this.m_LineX1 = new System.Windows.Forms.TextBox();
            this.SelectLineColor = new System.Windows.Forms.ComboBox();
            this.ShowLine = new System.Windows.Forms.Button();
            this.groupCrossOverlay = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.m_CrossWidth = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.m_CrossDY = new System.Windows.Forms.TextBox();
            this.m_CrossDX = new System.Windows.Forms.TextBox();
            this.m_CrossY = new System.Windows.Forms.TextBox();
            this.m_CrossX = new System.Windows.Forms.TextBox();
            this.SelectCrossColor = new System.Windows.Forms.ComboBox();
            this.ShowCross = new System.Windows.Forms.Button();
            this.groupTextOverlay = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.m_TextY = new System.Windows.Forms.TextBox();
            this.m_Text = new System.Windows.Forms.TextBox();
            this.SelectTextColor = new System.Windows.Forms.ComboBox();
            this.SelectTextSize = new System.Windows.Forms.ComboBox();
            this.m_TextX = new System.Windows.Forms.TextBox();
            this.ShowText = new System.Windows.Forms.Button();
            this.SelectTextAlign = new System.Windows.Forms.ComboBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.seqTargetList = new System.Windows.Forms.CheckedListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.selCaptureDir = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.SegHeight = new System.Windows.Forms.TextBox();
            this.SegWidth = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.setSegmentSize = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rightFingers = new System.Windows.Forms.CheckedListBox();
            this.leftFingers = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.selMinFingerCount = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.groupOverlay = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Type = new System.Windows.Forms.Label();
            this.OverlayColor = new System.Windows.Forms.ComboBox();
            this.OverlayType = new System.Windows.Forms.ComboBox();
            this.ClearAllOverlays = new System.Windows.Forms.Button();
            this.DrawOverlay = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.captureGuideMsg = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewWindow)).BeginInit();
            this.groupDeviceInfo.SuspendLayout();
            this.groupCapture.SuspendLayout();
            this.groupPreview.SuspendLayout();
            this.groupLCD.SuspendLayout();
            this.groupFingerLEDControl.SuspendLayout();
            this.DeviceTab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupMisc.SuspendLayout();
            this.groupSelfTest.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupKey.SuspendLayout();
            this.groupKeyCallback.SuspendLayout();
            this.groupStatusLEDControl.SuspendLayout();
            this.groupWav.SuspendLayout();
            this.groupBeep.SuspendLayout();
            this.groupModeLEDControl.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupLFD.SuspendLayout();
            this.groupCaptureOptionsEtc.SuspendLayout();
            this.groupRollCaptureOption.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupQuadOverlay.SuspendLayout();
            this.groupLineOverlay.SuspendLayout();
            this.groupCrossOverlay.SuspendLayout();
            this.groupTextOverlay.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupOverlay.SuspendLayout();
            this.SuspendLayout();
            // 
            // InitSDK
            // 
            this.InitSDK.Location = new System.Drawing.Point(636, 66);
            this.InitSDK.Name = "InitSDK";
            this.InitSDK.Size = new System.Drawing.Size(86, 23);
            this.InitSDK.TabIndex = 0;
            this.InitSDK.Text = "Init SDK";
            this.InitSDK.UseVisualStyleBackColor = true;
            this.InitSDK.Click += new System.EventHandler(this.InitSDK_Click);
            // 
            // MsgPanel
            // 
            this.MsgPanel.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MsgPanel.Location = new System.Drawing.Point(12, 12);
            this.MsgPanel.Name = "MsgPanel";
            this.MsgPanel.ReadOnly = true;
            this.MsgPanel.Size = new System.Drawing.Size(706, 39);
            this.MsgPanel.TabIndex = 1;
            this.MsgPanel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PreviewWindow
            // 
            this.PreviewWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreviewWindow.Location = new System.Drawing.Point(12, 66);
            this.PreviewWindow.Name = "PreviewWindow";
            this.PreviewWindow.Size = new System.Drawing.Size(471, 432);
            this.PreviewWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PreviewWindow.TabIndex = 2;
            this.PreviewWindow.TabStop = false;
            // 
            // groupDeviceInfo
            // 
            this.groupDeviceInfo.Controls.Add(this.brnViewSoftwareLicense);
            this.groupDeviceInfo.Controls.Add(this.DeviceID);
            this.groupDeviceInfo.Controls.Add(this.label6);
            this.groupDeviceInfo.Controls.Add(this.label5);
            this.groupDeviceInfo.Controls.Add(this.SDKInfo);
            this.groupDeviceInfo.Controls.Add(this.label4);
            this.groupDeviceInfo.Controls.Add(this.Hardwareinfo);
            this.groupDeviceInfo.Controls.Add(this.label3);
            this.groupDeviceInfo.Controls.Add(this.label2);
            this.groupDeviceInfo.Controls.Add(this.FirmwareInfo);
            this.groupDeviceInfo.Controls.Add(this.DeviceInfo);
            this.groupDeviceInfo.Location = new System.Drawing.Point(6, 12);
            this.groupDeviceInfo.Name = "groupDeviceInfo";
            this.groupDeviceInfo.Size = new System.Drawing.Size(220, 227);
            this.groupDeviceInfo.TabIndex = 3;
            this.groupDeviceInfo.TabStop = false;
            this.groupDeviceInfo.Text = "Device info";
            // 
            // brnViewSoftwareLicense
            // 
            this.brnViewSoftwareLicense.Location = new System.Drawing.Point(31, 199);
            this.brnViewSoftwareLicense.Name = "brnViewSoftwareLicense";
            this.brnViewSoftwareLicense.Size = new System.Drawing.Size(173, 20);
            this.brnViewSoftwareLicense.TabIndex = 25;
            this.brnViewSoftwareLicense.Text = "View software license";
            this.brnViewSoftwareLicense.UseVisualStyleBackColor = true;
            this.brnViewSoftwareLicense.Click += new System.EventHandler(this.btnViewSoftwareLicense_Click);
            // 
            // DeviceID
            // 
            this.DeviceID.Location = new System.Drawing.Point(97, 154);
            this.DeviceID.Name = "DeviceID";
            this.DeviceID.ReadOnly = true;
            this.DeviceID.Size = new System.Drawing.Size(107, 21);
            this.DeviceID.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "Device ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "SDK Version";
            // 
            // SDKInfo
            // 
            this.SDKInfo.Location = new System.Drawing.Point(97, 121);
            this.SDKInfo.Name = "SDKInfo";
            this.SDKInfo.ReadOnly = true;
            this.SDKInfo.Size = new System.Drawing.Size(107, 21);
            this.SDKInfo.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "Hardware";
            // 
            // Hardwareinfo
            // 
            this.Hardwareinfo.Location = new System.Drawing.Point(98, 88);
            this.Hardwareinfo.Name = "Hardwareinfo";
            this.Hardwareinfo.ReadOnly = true;
            this.Hardwareinfo.Size = new System.Drawing.Size(106, 21);
            this.Hardwareinfo.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Firmware";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "Model";
            // 
            // FirmwareInfo
            // 
            this.FirmwareInfo.Location = new System.Drawing.Point(97, 55);
            this.FirmwareInfo.Name = "FirmwareInfo";
            this.FirmwareInfo.ReadOnly = true;
            this.FirmwareInfo.Size = new System.Drawing.Size(107, 21);
            this.FirmwareInfo.TabIndex = 5;
            // 
            // DeviceInfo
            // 
            this.DeviceInfo.Location = new System.Drawing.Point(97, 20);
            this.DeviceInfo.Name = "DeviceInfo";
            this.DeviceInfo.ReadOnly = true;
            this.DeviceInfo.Size = new System.Drawing.Size(107, 21);
            this.DeviceInfo.TabIndex = 2;
            // 
            // DeviceList
            // 
            this.DeviceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DeviceList.FormattingEnabled = true;
            this.DeviceList.Location = new System.Drawing.Point(504, 66);
            this.DeviceList.Name = "DeviceList";
            this.DeviceList.Size = new System.Drawing.Size(126, 20);
            this.DeviceList.TabIndex = 1;
            // 
            // ExitDevice
            // 
            this.ExitDevice.Enabled = false;
            this.ExitDevice.Location = new System.Drawing.Point(636, 123);
            this.ExitDevice.Name = "ExitDevice";
            this.ExitDevice.Size = new System.Drawing.Size(86, 23);
            this.ExitDevice.TabIndex = 4;
            this.ExitDevice.Text = "Exit Device";
            this.ExitDevice.UseVisualStyleBackColor = true;
            this.ExitDevice.Click += new System.EventHandler(this.exitDevice_Click);
            // 
            // InitDevice
            // 
            this.InitDevice.Enabled = false;
            this.InitDevice.Location = new System.Drawing.Point(636, 95);
            this.InitDevice.Name = "InitDevice";
            this.InitDevice.Size = new System.Drawing.Size(86, 23);
            this.InitDevice.TabIndex = 3;
            this.InitDevice.Text = "Init Device";
            this.InitDevice.UseVisualStyleBackColor = true;
            this.InitDevice.Click += new System.EventHandler(this.initDevice_Click);
            // 
            // FingerLEDOn
            // 
            this.FingerLEDOn.Location = new System.Drawing.Point(119, 18);
            this.FingerLEDOn.Name = "FingerLEDOn";
            this.FingerLEDOn.Size = new System.Drawing.Size(34, 23);
            this.FingerLEDOn.TabIndex = 12;
            this.FingerLEDOn.Text = "On";
            this.FingerLEDOn.UseVisualStyleBackColor = true;
            this.FingerLEDOn.Click += new System.EventHandler(this.FingerLEDON_Click);
            // 
            // groupCapture
            // 
            this.groupCapture.Controls.Add(this.customCaptH);
            this.groupCapture.Controls.Add(this.label49);
            this.groupCapture.Controls.Add(this.customCaptW);
            this.groupCapture.Controls.Add(this.label48);
            this.groupCapture.Controls.Add(this.label47);
            this.groupCapture.Controls.Add(this.customCaptY);
            this.groupCapture.Controls.Add(this.customCaptX);
            this.groupCapture.Controls.Add(this.label46);
            this.groupCapture.Controls.Add(this.label45);
            this.groupCapture.Controls.Add(this.TakeManual);
            this.groupCapture.Controls.Add(this.TakeAuto);
            this.groupCapture.Controls.Add(this.Callback);
            this.groupCapture.Controls.Add(this.ImageSize);
            this.groupCapture.Controls.Add(this.StopCapture);
            this.groupCapture.Controls.Add(this.AutoCapture);
            this.groupCapture.Controls.Add(this.StartCapture);
            this.groupCapture.Controls.Add(this.CaptureMode);
            this.groupCapture.Location = new System.Drawing.Point(504, 147);
            this.groupCapture.Name = "groupCapture";
            this.groupCapture.Size = new System.Drawing.Size(241, 197);
            this.groupCapture.TabIndex = 4;
            this.groupCapture.TabStop = false;
            this.groupCapture.Text = "Capture";
            // 
            // customCaptH
            // 
            this.customCaptH.Location = new System.Drawing.Point(203, 167);
            this.customCaptH.Name = "customCaptH";
            this.customCaptH.Size = new System.Drawing.Size(34, 21);
            this.customCaptH.TabIndex = 20;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(183, 170);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(20, 12);
            this.label49.TabIndex = 19;
            this.label49.Text = "h :";
            // 
            // customCaptW
            // 
            this.customCaptW.Location = new System.Drawing.Point(144, 167);
            this.customCaptW.Name = "customCaptW";
            this.customCaptW.Size = new System.Drawing.Size(34, 21);
            this.customCaptW.TabIndex = 18;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(121, 170);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(23, 12);
            this.label48.TabIndex = 17;
            this.label48.Text = "w :";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(183, 147);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(20, 12);
            this.label47.TabIndex = 16;
            this.label47.Text = "y :";
            // 
            // customCaptY
            // 
            this.customCaptY.Location = new System.Drawing.Point(203, 144);
            this.customCaptY.Name = "customCaptY";
            this.customCaptY.Size = new System.Drawing.Size(34, 21);
            this.customCaptY.TabIndex = 15;
            // 
            // customCaptX
            // 
            this.customCaptX.Location = new System.Drawing.Point(143, 144);
            this.customCaptX.Name = "customCaptX";
            this.customCaptX.Size = new System.Drawing.Size(34, 21);
            this.customCaptX.TabIndex = 14;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(123, 147);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(20, 12);
            this.label46.TabIndex = 13;
            this.label46.Text = "x :";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(5, 147);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(112, 12);
            this.label45.TabIndex = 12;
            this.label45.Text = "Custom Capt. Size";
            // 
            // TakeManual
            // 
            this.TakeManual.Enabled = false;
            this.TakeManual.Location = new System.Drawing.Point(132, 76);
            this.TakeManual.Name = "TakeManual";
            this.TakeManual.Size = new System.Drawing.Size(86, 23);
            this.TakeManual.TabIndex = 11;
            this.TakeManual.Text = "Take Manual";
            this.TakeManual.UseVisualStyleBackColor = true;
            this.TakeManual.Click += new System.EventHandler(this.TakeManual_Click);
            // 
            // TakeAuto
            // 
            this.TakeAuto.Enabled = false;
            this.TakeAuto.Location = new System.Drawing.Point(132, 105);
            this.TakeAuto.Name = "TakeAuto";
            this.TakeAuto.Size = new System.Drawing.Size(86, 23);
            this.TakeAuto.TabIndex = 10;
            this.TakeAuto.Text = "Take Auto";
            this.TakeAuto.UseVisualStyleBackColor = true;
            this.TakeAuto.Click += new System.EventHandler(this.TakeAuto_Click);
            // 
            // Callback
            // 
            this.Callback.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Callback.FormattingEnabled = true;
            this.Callback.Items.AddRange(new object[] {
            "None",
            "Save Image",
            "Segment",
            "Enlarge",
            "Segment & Save",
            "Sequence Check",
            "Save(19794-4:2011)"});
            this.Callback.Location = new System.Drawing.Point(15, 106);
            this.Callback.Name = "Callback";
            this.Callback.Size = new System.Drawing.Size(107, 20);
            this.Callback.TabIndex = 9;
            // 
            // ImageSize
            // 
            this.ImageSize.Location = new System.Drawing.Point(15, 78);
            this.ImageSize.Name = "ImageSize";
            this.ImageSize.ReadOnly = true;
            this.ImageSize.Size = new System.Drawing.Size(107, 21);
            this.ImageSize.TabIndex = 6;
            // 
            // StopCapture
            // 
            this.StopCapture.Enabled = false;
            this.StopCapture.Location = new System.Drawing.Point(132, 47);
            this.StopCapture.Name = "StopCapture";
            this.StopCapture.Size = new System.Drawing.Size(86, 23);
            this.StopCapture.TabIndex = 8;
            this.StopCapture.Text = "Stop";
            this.StopCapture.UseVisualStyleBackColor = true;
            this.StopCapture.Click += new System.EventHandler(this.StopCapture_Click);
            // 
            // AutoCapture
            // 
            this.AutoCapture.AutoSize = true;
            this.AutoCapture.Location = new System.Drawing.Point(15, 47);
            this.AutoCapture.Name = "AutoCapture";
            this.AutoCapture.Size = new System.Drawing.Size(49, 16);
            this.AutoCapture.TabIndex = 7;
            this.AutoCapture.Text = "Auto";
            this.AutoCapture.UseVisualStyleBackColor = true;
            // 
            // StartCapture
            // 
            this.StartCapture.Enabled = false;
            this.StartCapture.Location = new System.Drawing.Point(132, 18);
            this.StartCapture.Name = "StartCapture";
            this.StartCapture.Size = new System.Drawing.Size(86, 23);
            this.StartCapture.TabIndex = 6;
            this.StartCapture.Text = "Start";
            this.StartCapture.UseVisualStyleBackColor = true;
            this.StartCapture.Click += new System.EventHandler(this.StartCapture_Click);
            // 
            // CaptureMode
            // 
            this.CaptureMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CaptureMode.FormattingEnabled = true;
            this.CaptureMode.Items.AddRange(new object[] {
            "Disabled",
            "Roll",
            "Flat One",
            "Flat Two",
            "Flat Four(Left)",
            "Flat Four(Right)",
            "Palm(Left)",
            "Palm(Right)",
            "Flat One(Ex)",
            "Flat Two(Ex)",
            "Side Palm(Left)",
            "Side Palm(Right)",
            "Writer\'s Palm(Left)",
            "Writer\'s Palm(Right)",
            "Upper Palm(L)",
            "Upper Palm(R)",
            "Lower Palm(L)",
            "Lower Palm(R)",
            "Custom(Flat)"});
            this.CaptureMode.Location = new System.Drawing.Point(15, 18);
            this.CaptureMode.Name = "CaptureMode";
            this.CaptureMode.Size = new System.Drawing.Size(107, 20);
            this.CaptureMode.TabIndex = 6;
            this.CaptureMode.SelectedIndexChanged += new System.EventHandler(this.CaptureMode_SelectedIndexChanged);
            // 
            // groupPreview
            // 
            this.groupPreview.Controls.Add(this.advCallbackDrawBtn);
            this.groupPreview.Controls.Add(this.callbackDrawBtn);
            this.groupPreview.Controls.Add(this.directDrawBtn);
            this.groupPreview.Location = new System.Drawing.Point(505, 345);
            this.groupPreview.Name = "groupPreview";
            this.groupPreview.Size = new System.Drawing.Size(236, 49);
            this.groupPreview.TabIndex = 5;
            this.groupPreview.TabStop = false;
            this.groupPreview.Text = "Preview";
            // 
            // advCallbackDrawBtn
            // 
            this.advCallbackDrawBtn.AutoSize = true;
            this.advCallbackDrawBtn.Location = new System.Drawing.Point(143, 21);
            this.advCallbackDrawBtn.Name = "advCallbackDrawBtn";
            this.advCallbackDrawBtn.Size = new System.Drawing.Size(93, 16);
            this.advCallbackDrawBtn.TabIndex = 6;
            this.advCallbackDrawBtn.Text = "AdvCallback";
            this.advCallbackDrawBtn.UseVisualStyleBackColor = true;
            this.advCallbackDrawBtn.CheckedChanged += new System.EventHandler(this.advCallbackDrawBtn_CheckedChanged);
            // 
            // callbackDrawBtn
            // 
            this.callbackDrawBtn.AutoSize = true;
            this.callbackDrawBtn.Location = new System.Drawing.Point(68, 21);
            this.callbackDrawBtn.Name = "callbackDrawBtn";
            this.callbackDrawBtn.Size = new System.Drawing.Size(72, 16);
            this.callbackDrawBtn.TabIndex = 5;
            this.callbackDrawBtn.Text = "Callback";
            this.callbackDrawBtn.UseVisualStyleBackColor = true;
            this.callbackDrawBtn.CheckedChanged += new System.EventHandler(this.callbackDrawBtn_CheckedChanged);
            // 
            // directDrawBtn
            // 
            this.directDrawBtn.AutoSize = true;
            this.directDrawBtn.Checked = true;
            this.directDrawBtn.Location = new System.Drawing.Point(6, 21);
            this.directDrawBtn.Name = "directDrawBtn";
            this.directDrawBtn.Size = new System.Drawing.Size(55, 16);
            this.directDrawBtn.TabIndex = 4;
            this.directDrawBtn.TabStop = true;
            this.directDrawBtn.Text = "Direct";
            this.directDrawBtn.UseVisualStyleBackColor = true;
            this.directDrawBtn.CheckedChanged += new System.EventHandler(this.directDrawBtn_CheckedChanged);
            // 
            // groupLCD
            // 
            this.groupLCD.Controls.Add(this.DisplayLCD);
            this.groupLCD.Controls.Add(this.ResetLCD);
            this.groupLCD.Location = new System.Drawing.Point(509, 127);
            this.groupLCD.Name = "groupLCD";
            this.groupLCD.Size = new System.Drawing.Size(199, 77);
            this.groupLCD.TabIndex = 7;
            this.groupLCD.TabStop = false;
            this.groupLCD.Text = "LCD(RS-F)";
            // 
            // DisplayLCD
            // 
            this.DisplayLCD.Location = new System.Drawing.Point(77, 48);
            this.DisplayLCD.Name = "DisplayLCD";
            this.DisplayLCD.Size = new System.Drawing.Size(115, 21);
            this.DisplayLCD.TabIndex = 1;
            this.DisplayLCD.Text = "Display File Data";
            this.DisplayLCD.UseVisualStyleBackColor = true;
            this.DisplayLCD.Click += new System.EventHandler(this.DisplayLCD_Click);
            // 
            // ResetLCD
            // 
            this.ResetLCD.Location = new System.Drawing.Point(77, 20);
            this.ResetLCD.Name = "ResetLCD";
            this.ResetLCD.Size = new System.Drawing.Size(115, 21);
            this.ResetLCD.TabIndex = 0;
            this.ResetLCD.Text = "Reset LCD";
            this.ResetLCD.UseVisualStyleBackColor = true;
            this.ResetLCD.Click += new System.EventHandler(this.ResetLCD_Click);
            // 
            // FingerLEDOff
            // 
            this.FingerLEDOff.Location = new System.Drawing.Point(159, 19);
            this.FingerLEDOff.Name = "FingerLEDOff";
            this.FingerLEDOff.Size = new System.Drawing.Size(38, 23);
            this.FingerLEDOff.TabIndex = 13;
            this.FingerLEDOff.Text = "OFF";
            this.FingerLEDOff.UseVisualStyleBackColor = true;
            this.FingerLEDOff.Click += new System.EventHandler(this.FingerLEDOFF_Click);
            // 
            // groupFingerLEDControl
            // 
            this.groupFingerLEDControl.Controls.Add(this.FingerColor);
            this.groupFingerLEDControl.Controls.Add(this.Finger_LED);
            this.groupFingerLEDControl.Controls.Add(this.FingerLEDOff);
            this.groupFingerLEDControl.Controls.Add(this.FingerLEDOn);
            this.groupFingerLEDControl.Location = new System.Drawing.Point(251, 71);
            this.groupFingerLEDControl.Name = "groupFingerLEDControl";
            this.groupFingerLEDControl.Size = new System.Drawing.Size(254, 78);
            this.groupFingerLEDControl.TabIndex = 14;
            this.groupFingerLEDControl.TabStop = false;
            this.groupFingerLEDControl.Text = "Finger LED Control";
            // 
            // FingerColor
            // 
            this.FingerColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FingerColor.FormattingEnabled = true;
            this.FingerColor.Items.AddRange(new object[] {
            "Green",
            "Red",
            "Yellow"});
            this.FingerColor.Location = new System.Drawing.Point(6, 46);
            this.FingerColor.Name = "FingerColor";
            this.FingerColor.Size = new System.Drawing.Size(107, 20);
            this.FingerColor.TabIndex = 14;
            // 
            // Finger_LED
            // 
            this.Finger_LED.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Finger_LED.FormattingEnabled = true;
            this.Finger_LED.Items.AddRange(new object[] {
            "All",
            "Little(L)",
            "Ring(L)",
            "Middle(L)",
            "Index(L)",
            "Thumb(L)",
            "Thumb(R)",
            "Index(R)",
            "Middle(R)",
            "Ring(R)",
            "Little(R)",
            "Thums",
            "Four(L)",
            "Four(R)"});
            this.Finger_LED.Location = new System.Drawing.Point(6, 20);
            this.Finger_LED.Name = "Finger_LED";
            this.Finger_LED.Size = new System.Drawing.Size(107, 20);
            this.Finger_LED.TabIndex = 12;
            // 
            // DeviceTab
            // 
            this.DeviceTab.AccessibleName = "";
            this.DeviceTab.Controls.Add(this.tabPage1);
            this.DeviceTab.Controls.Add(this.tabPage2);
            this.DeviceTab.Controls.Add(this.tabPage3);
            this.DeviceTab.Controls.Add(this.tabPage4);
            this.DeviceTab.Controls.Add(this.tabPage5);
            this.DeviceTab.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.DeviceTab.Location = new System.Drawing.Point(8, 582);
            this.DeviceTab.Name = "DeviceTab";
            this.DeviceTab.SelectedIndex = 0;
            this.DeviceTab.Size = new System.Drawing.Size(737, 272);
            this.DeviceTab.TabIndex = 15;
            this.DeviceTab.Tag = "";
            this.DeviceTab.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DeviceTab_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.AccessibleName = "";
            this.tabPage1.Controls.Add(this.logBox);
            this.tabPage1.Controls.Add(this.groupMisc);
            this.tabPage1.Controls.Add(this.groupSelfTest);
            this.tabPage1.Controls.Add(this.groupDeviceInfo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(729, 246);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Device";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(239, 72);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(471, 159);
            this.logBox.TabIndex = 14;
            // 
            // groupMisc
            // 
            this.groupMisc.Controls.Add(this.ManualCalibrate);
            this.groupMisc.Location = new System.Drawing.Point(488, 16);
            this.groupMisc.Name = "groupMisc";
            this.groupMisc.Size = new System.Drawing.Size(237, 50);
            this.groupMisc.TabIndex = 13;
            this.groupMisc.TabStop = false;
            this.groupMisc.Text = "Misc.";
            // 
            // ManualCalibrate
            // 
            this.ManualCalibrate.Location = new System.Drawing.Point(11, 16);
            this.ManualCalibrate.Name = "ManualCalibrate";
            this.ManualCalibrate.Size = new System.Drawing.Size(211, 23);
            this.ManualCalibrate.TabIndex = 12;
            this.ManualCalibrate.Text = "Calibrate";
            this.ManualCalibrate.UseVisualStyleBackColor = true;
            this.ManualCalibrate.Click += new System.EventHandler(this.ManualCalibration_Click);
            // 
            // groupSelfTest
            // 
            this.groupSelfTest.Controls.Add(this.TestRun);
            this.groupSelfTest.Controls.Add(this.TestType);
            this.groupSelfTest.Location = new System.Drawing.Point(232, 16);
            this.groupSelfTest.Name = "groupSelfTest";
            this.groupSelfTest.Size = new System.Drawing.Size(246, 50);
            this.groupSelfTest.TabIndex = 11;
            this.groupSelfTest.TabStop = false;
            this.groupSelfTest.Text = "Self-Test";
            // 
            // TestRun
            // 
            this.TestRun.Location = new System.Drawing.Point(145, 16);
            this.TestRun.Name = "TestRun";
            this.TestRun.Size = new System.Drawing.Size(86, 23);
            this.TestRun.TabIndex = 12;
            this.TestRun.Text = "Run";
            this.TestRun.UseVisualStyleBackColor = true;
            this.TestRun.Click += new System.EventHandler(this.TestRun_Click);
            // 
            // TestType
            // 
            this.TestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TestType.FormattingEnabled = true;
            this.TestType.Items.AddRange(new object[] {
            "Illumination",
            "Dirt"});
            this.TestType.Location = new System.Drawing.Point(17, 18);
            this.TestType.Name = "TestType";
            this.TestType.Size = new System.Drawing.Size(107, 20);
            this.TestType.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupKey);
            this.tabPage2.Controls.Add(this.groupStatusLEDControl);
            this.tabPage2.Controls.Add(this.groupWav);
            this.tabPage2.Controls.Add(this.groupBeep);
            this.tabPage2.Controls.Add(this.groupModeLEDControl);
            this.tabPage2.Controls.Add(this.groupLCD);
            this.tabPage2.Controls.Add(this.groupFingerLEDControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(729, 246);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupKey
            // 
            this.groupKey.Controls.Add(this.groupKeyCallback);
            this.groupKey.Controls.Add(this.KEYSTATUS);
            this.groupKey.Controls.Add(this.ReadKeycallback);
            this.groupKey.Controls.Add(this.ActivateKey);
            this.groupKey.Controls.Add(this.KeyMask);
            this.groupKey.Location = new System.Drawing.Point(21, 16);
            this.groupKey.Name = "groupKey";
            this.groupKey.Size = new System.Drawing.Size(224, 179);
            this.groupKey.TabIndex = 18;
            this.groupKey.TabStop = false;
            this.groupKey.Text = "Key";
            // 
            // groupKeyCallback
            // 
            this.groupKeyCallback.Controls.Add(this.CALLBACKPRINT);
            this.groupKeyCallback.Controls.Add(this.KEYCALLBACK);
            this.groupKeyCallback.Location = new System.Drawing.Point(9, 84);
            this.groupKeyCallback.Name = "groupKeyCallback";
            this.groupKeyCallback.Size = new System.Drawing.Size(209, 88);
            this.groupKeyCallback.TabIndex = 19;
            this.groupKeyCallback.TabStop = false;
            this.groupKeyCallback.Text = "Callback";
            // 
            // CALLBACKPRINT
            // 
            this.CALLBACKPRINT.Location = new System.Drawing.Point(6, 55);
            this.CALLBACKPRINT.Name = "CALLBACKPRINT";
            this.CALLBACKPRINT.ReadOnly = true;
            this.CALLBACKPRINT.Size = new System.Drawing.Size(197, 21);
            this.CALLBACKPRINT.TabIndex = 12;
            // 
            // KEYCALLBACK
            // 
            this.KEYCALLBACK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KEYCALLBACK.FormattingEnabled = true;
            this.KEYCALLBACK.Items.AddRange(new object[] {
            "None",
            "Print",
            "Beep"});
            this.KEYCALLBACK.Location = new System.Drawing.Point(6, 20);
            this.KEYCALLBACK.Name = "KEYCALLBACK";
            this.KEYCALLBACK.Size = new System.Drawing.Size(102, 20);
            this.KEYCALLBACK.TabIndex = 14;
            // 
            // KEYSTATUS
            // 
            this.KEYSTATUS.Location = new System.Drawing.Point(6, 55);
            this.KEYSTATUS.Name = "KEYSTATUS";
            this.KEYSTATUS.Size = new System.Drawing.Size(101, 21);
            this.KEYSTATUS.TabIndex = 12;
            // 
            // ReadKeycallback
            // 
            this.ReadKeycallback.Location = new System.Drawing.Point(114, 55);
            this.ReadKeycallback.Name = "ReadKeycallback";
            this.ReadKeycallback.Size = new System.Drawing.Size(85, 23);
            this.ReadKeycallback.TabIndex = 15;
            this.ReadKeycallback.Text = "Read Key";
            this.ReadKeycallback.UseVisualStyleBackColor = true;
            this.ReadKeycallback.Click += new System.EventHandler(this.ReadKeycallback_Click);
            // 
            // ActivateKey
            // 
            this.ActivateKey.Location = new System.Drawing.Point(114, 20);
            this.ActivateKey.Name = "ActivateKey";
            this.ActivateKey.Size = new System.Drawing.Size(85, 23);
            this.ActivateKey.TabIndex = 14;
            this.ActivateKey.Text = "Activate";
            this.ActivateKey.UseVisualStyleBackColor = true;
            this.ActivateKey.Click += new System.EventHandler(this.ActiveKey_Click);
            // 
            // KeyMask
            // 
            this.KeyMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KeyMask.FormattingEnabled = true;
            this.KeyMask.Items.AddRange(new object[] {
            "None",
            "up",
            "down",
            "Left",
            "Right",
            "play",
            "stop",
            "Foot",
            "All"});
            this.KeyMask.Location = new System.Drawing.Point(6, 20);
            this.KeyMask.Name = "KeyMask";
            this.KeyMask.Size = new System.Drawing.Size(102, 20);
            this.KeyMask.TabIndex = 14;
            // 
            // groupStatusLEDControl
            // 
            this.groupStatusLEDControl.Controls.Add(this.StatusLEDColor);
            this.groupStatusLEDControl.Controls.Add(this.StatusLEDOff);
            this.groupStatusLEDControl.Controls.Add(this.StatusLEDOn);
            this.groupStatusLEDControl.Location = new System.Drawing.Point(251, 154);
            this.groupStatusLEDControl.Name = "groupStatusLEDControl";
            this.groupStatusLEDControl.Size = new System.Drawing.Size(252, 50);
            this.groupStatusLEDControl.TabIndex = 16;
            this.groupStatusLEDControl.TabStop = false;
            this.groupStatusLEDControl.Text = "Status LED (RS-D)";
            // 
            // StatusLEDColor
            // 
            this.StatusLEDColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StatusLEDColor.FormattingEnabled = true;
            this.StatusLEDColor.Items.AddRange(new object[] {
            "Green",
            "Red",
            "Yellow"});
            this.StatusLEDColor.Location = new System.Drawing.Point(6, 20);
            this.StatusLEDColor.Name = "StatusLEDColor";
            this.StatusLEDColor.Size = new System.Drawing.Size(107, 20);
            this.StatusLEDColor.TabIndex = 12;
            // 
            // StatusLEDOff
            // 
            this.StatusLEDOff.Location = new System.Drawing.Point(159, 19);
            this.StatusLEDOff.Name = "StatusLEDOff";
            this.StatusLEDOff.Size = new System.Drawing.Size(38, 23);
            this.StatusLEDOff.TabIndex = 13;
            this.StatusLEDOff.Text = "OFF";
            this.StatusLEDOff.UseVisualStyleBackColor = true;
            this.StatusLEDOff.Click += new System.EventHandler(this.StatusLEDOff_Click);
            // 
            // StatusLEDOn
            // 
            this.StatusLEDOn.Location = new System.Drawing.Point(119, 18);
            this.StatusLEDOn.Name = "StatusLEDOn";
            this.StatusLEDOn.Size = new System.Drawing.Size(34, 23);
            this.StatusLEDOn.TabIndex = 12;
            this.StatusLEDOn.Text = "On";
            this.StatusLEDOn.UseVisualStyleBackColor = true;
            this.StatusLEDOn.Click += new System.EventHandler(this.StatusLEDOn_Click);
            // 
            // groupWav
            // 
            this.groupWav.Controls.Add(this.WavPlay);
            this.groupWav.Location = new System.Drawing.Point(511, 73);
            this.groupWav.Name = "groupWav";
            this.groupWav.Size = new System.Drawing.Size(199, 48);
            this.groupWav.TabIndex = 17;
            this.groupWav.TabStop = false;
            this.groupWav.Text = "Wav";
            // 
            // WavPlay
            // 
            this.WavPlay.Location = new System.Drawing.Point(114, 14);
            this.WavPlay.Name = "WavPlay";
            this.WavPlay.Size = new System.Drawing.Size(78, 21);
            this.WavPlay.TabIndex = 2;
            this.WavPlay.Text = "Play";
            this.WavPlay.UseVisualStyleBackColor = true;
            this.WavPlay.Click += new System.EventHandler(this.PlayWav_Click);
            // 
            // groupBeep
            // 
            this.groupBeep.Controls.Add(this.BeepPattern);
            this.groupBeep.Controls.Add(this.EmitBeep);
            this.groupBeep.Location = new System.Drawing.Point(511, 16);
            this.groupBeep.Name = "groupBeep";
            this.groupBeep.Size = new System.Drawing.Size(204, 50);
            this.groupBeep.TabIndex = 16;
            this.groupBeep.TabStop = false;
            this.groupBeep.Text = "Beep";
            // 
            // BeepPattern
            // 
            this.BeepPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BeepPattern.FormattingEnabled = true;
            this.BeepPattern.Items.AddRange(new object[] {
            "None",
            "Beep1",
            "Beep2"});
            this.BeepPattern.Location = new System.Drawing.Point(5, 20);
            this.BeepPattern.Name = "BeepPattern";
            this.BeepPattern.Size = new System.Drawing.Size(107, 20);
            this.BeepPattern.TabIndex = 12;
            // 
            // EmitBeep
            // 
            this.EmitBeep.AccessibleName = "";
            this.EmitBeep.Location = new System.Drawing.Point(119, 18);
            this.EmitBeep.Name = "EmitBeep";
            this.EmitBeep.Size = new System.Drawing.Size(78, 23);
            this.EmitBeep.TabIndex = 12;
            this.EmitBeep.Text = "Emit";
            this.EmitBeep.UseVisualStyleBackColor = true;
            this.EmitBeep.Click += new System.EventHandler(this.Emitbeep_Click);
            // 
            // groupModeLEDControl
            // 
            this.groupModeLEDControl.Controls.Add(this.Mode_LED);
            this.groupModeLEDControl.Controls.Add(this.ModeLEDOff);
            this.groupModeLEDControl.Controls.Add(this.ModeLEDOn);
            this.groupModeLEDControl.Location = new System.Drawing.Point(251, 18);
            this.groupModeLEDControl.Name = "groupModeLEDControl";
            this.groupModeLEDControl.Size = new System.Drawing.Size(254, 50);
            this.groupModeLEDControl.TabIndex = 15;
            this.groupModeLEDControl.TabStop = false;
            this.groupModeLEDControl.Text = "Mode LED Controllor";
            // 
            // Mode_LED
            // 
            this.Mode_LED.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Mode_LED.FormattingEnabled = true;
            this.Mode_LED.Items.AddRange(new object[] {
            "All",
            "Left",
            "Right",
            "Thumb",
            "Roll",
            "Power LED"});
            this.Mode_LED.Location = new System.Drawing.Point(6, 20);
            this.Mode_LED.Name = "Mode_LED";
            this.Mode_LED.Size = new System.Drawing.Size(107, 20);
            this.Mode_LED.TabIndex = 12;
            // 
            // ModeLEDOff
            // 
            this.ModeLEDOff.Location = new System.Drawing.Point(159, 19);
            this.ModeLEDOff.Name = "ModeLEDOff";
            this.ModeLEDOff.Size = new System.Drawing.Size(38, 23);
            this.ModeLEDOff.TabIndex = 13;
            this.ModeLEDOff.Text = "OFF";
            this.ModeLEDOff.UseVisualStyleBackColor = true;
            this.ModeLEDOff.Click += new System.EventHandler(this.ModeLEDOff_Click);
            // 
            // ModeLEDOn
            // 
            this.ModeLEDOn.Location = new System.Drawing.Point(119, 18);
            this.ModeLEDOn.Name = "ModeLEDOn";
            this.ModeLEDOn.Size = new System.Drawing.Size(34, 23);
            this.ModeLEDOn.TabIndex = 12;
            this.ModeLEDOn.Text = "On";
            this.ModeLEDOn.UseVisualStyleBackColor = true;
            this.ModeLEDOn.Click += new System.EventHandler(this.ModeLEDOn_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupLFD);
            this.tabPage3.Controls.Add(this.groupCaptureOptionsEtc);
            this.tabPage3.Controls.Add(this.groupRollCaptureOption);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(729, 246);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupLFD
            // 
            this.groupLFD.Controls.Add(this.label7);
            this.groupLFD.Controls.Add(this.readLFDLevel);
            this.groupLFD.Controls.Add(this.writeLFDLevel);
            this.groupLFD.Controls.Add(this.cbLFDLevel);
            this.groupLFD.Location = new System.Drawing.Point(13, 135);
            this.groupLFD.Name = "groupLFD";
            this.groupLFD.Size = new System.Drawing.Size(315, 74);
            this.groupLFD.TabIndex = 19;
            this.groupLFD.TabStop = false;
            this.groupLFD.Text = "LFD Option(for D, G10, S60)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "LFD Level";
            // 
            // readLFDLevel
            // 
            this.readLFDLevel.Location = new System.Drawing.Point(234, 14);
            this.readLFDLevel.Name = "readLFDLevel";
            this.readLFDLevel.Size = new System.Drawing.Size(57, 23);
            this.readLFDLevel.TabIndex = 18;
            this.readLFDLevel.Text = "Read";
            this.readLFDLevel.UseVisualStyleBackColor = true;
            this.readLFDLevel.Click += new System.EventHandler(this.ReadLFDLevel_Click);
            // 
            // writeLFDLevel
            // 
            this.writeLFDLevel.Location = new System.Drawing.Point(233, 44);
            this.writeLFDLevel.Name = "writeLFDLevel";
            this.writeLFDLevel.Size = new System.Drawing.Size(59, 23);
            this.writeLFDLevel.TabIndex = 18;
            this.writeLFDLevel.Text = "Write";
            this.writeLFDLevel.UseVisualStyleBackColor = true;
            this.writeLFDLevel.Click += new System.EventHandler(this.SetLFDLevel_Click);
            // 
            // cbLFDLevel
            // 
            this.cbLFDLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLFDLevel.FormattingEnabled = true;
            this.cbLFDLevel.Items.AddRange(new object[] {
            "OFF",
            "1",
            "2",
            "*3",
            "4",
            "5"});
            this.cbLFDLevel.Location = new System.Drawing.Point(100, 17);
            this.cbLFDLevel.Name = "cbLFDLevel";
            this.cbLFDLevel.Size = new System.Drawing.Size(117, 20);
            this.cbLFDLevel.TabIndex = 13;
            this.cbLFDLevel.SelectedIndexChanged += new System.EventHandler(this.cbLFDLevel_SelectedIndexChanged);
            // 
            // groupCaptureOptionsEtc
            // 
            this.groupCaptureOptionsEtc.Controls.Add(this.label11);
            this.groupCaptureOptionsEtc.Controls.Add(this.label13);
            this.groupCaptureOptionsEtc.Controls.Add(this.ContrastEnhancement);
            this.groupCaptureOptionsEtc.Controls.Add(this.AutoCalibrate);
            this.groupCaptureOptionsEtc.Controls.Add(this.NoiseReduction);
            this.groupCaptureOptionsEtc.Controls.Add(this.WriteCaptureOption);
            this.groupCaptureOptionsEtc.Controls.Add(this.ReadCaptureOption);
            this.groupCaptureOptionsEtc.Controls.Add(this.ReductionLevel);
            this.groupCaptureOptionsEtc.Controls.Add(this.AutoSensitivity);
            this.groupCaptureOptionsEtc.Controls.Add(this.AdvancedContrastEnhancement);
            this.groupCaptureOptionsEtc.Location = new System.Drawing.Point(354, 16);
            this.groupCaptureOptionsEtc.Name = "groupCaptureOptionsEtc";
            this.groupCaptureOptionsEtc.Size = new System.Drawing.Size(348, 193);
            this.groupCaptureOptionsEtc.TabIndex = 15;
            this.groupCaptureOptionsEtc.TabStop = false;
            this.groupCaptureOptionsEtc.Text = "Other Capture Option";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 12);
            this.label11.TabIndex = 23;
            this.label11.Text = "Post-Processing";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "Auto Capture";
            // 
            // ContrastEnhancement
            // 
            this.ContrastEnhancement.AutoSize = true;
            this.ContrastEnhancement.Location = new System.Drawing.Point(254, 111);
            this.ContrastEnhancement.Name = "ContrastEnhancement";
            this.ContrastEnhancement.Size = new System.Drawing.Size(71, 16);
            this.ContrastEnhancement.TabIndex = 12;
            this.ContrastEnhancement.Text = "Contrast";
            this.ContrastEnhancement.UseVisualStyleBackColor = true;
            // 
            // AutoCalibrate
            // 
            this.AutoCalibrate.AutoSize = true;
            this.AutoCalibrate.Location = new System.Drawing.Point(15, 69);
            this.AutoCalibrate.Name = "AutoCalibrate";
            this.AutoCalibrate.Size = new System.Drawing.Size(113, 16);
            this.AutoCalibrate.TabIndex = 12;
            this.AutoCalibrate.Text = "Auto Calibration";
            this.AutoCalibrate.UseVisualStyleBackColor = true;
            // 
            // NoiseReduction
            // 
            this.NoiseReduction.AutoSize = true;
            this.NoiseReduction.Location = new System.Drawing.Point(119, 110);
            this.NoiseReduction.Name = "NoiseReduction";
            this.NoiseReduction.Size = new System.Drawing.Size(57, 16);
            this.NoiseReduction.TabIndex = 13;
            this.NoiseReduction.Text = "Noise";
            this.NoiseReduction.UseVisualStyleBackColor = true;
            // 
            // WriteCaptureOption
            // 
            this.WriteCaptureOption.Location = new System.Drawing.Point(239, 60);
            this.WriteCaptureOption.Name = "WriteCaptureOption";
            this.WriteCaptureOption.Size = new System.Drawing.Size(95, 23);
            this.WriteCaptureOption.TabIndex = 14;
            this.WriteCaptureOption.Text = "Write";
            this.WriteCaptureOption.UseVisualStyleBackColor = true;
            this.WriteCaptureOption.Click += new System.EventHandler(this.WriteOtherOption_Click);
            // 
            // ReadCaptureOption
            // 
            this.ReadCaptureOption.Location = new System.Drawing.Point(239, 26);
            this.ReadCaptureOption.Name = "ReadCaptureOption";
            this.ReadCaptureOption.Size = new System.Drawing.Size(95, 23);
            this.ReadCaptureOption.TabIndex = 12;
            this.ReadCaptureOption.Text = "Read";
            this.ReadCaptureOption.UseVisualStyleBackColor = true;
            this.ReadCaptureOption.Click += new System.EventHandler(this.ReadOtherOption_Click);
            // 
            // ReductionLevel
            // 
            this.ReductionLevel.Location = new System.Drawing.Point(177, 108);
            this.ReductionLevel.Name = "ReductionLevel";
            this.ReductionLevel.Size = new System.Drawing.Size(71, 21);
            this.ReductionLevel.TabIndex = 17;
            // 
            // AutoSensitivity
            // 
            this.AutoSensitivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AutoSensitivity.FormattingEnabled = true;
            this.AutoSensitivity.Items.AddRange(new object[] {
            "Normal",
            "High",
            "Higher",
            "Strict",
            "Disabled"});
            this.AutoSensitivity.Location = new System.Drawing.Point(111, 28);
            this.AutoSensitivity.Name = "AutoSensitivity";
            this.AutoSensitivity.Size = new System.Drawing.Size(115, 20);
            this.AutoSensitivity.TabIndex = 13;
            // 
            // AdvancedContrastEnhancement
            // 
            this.AdvancedContrastEnhancement.AutoSize = true;
            this.AdvancedContrastEnhancement.Location = new System.Drawing.Point(15, 91);
            this.AdvancedContrastEnhancement.Name = "AdvancedContrastEnhancement";
            this.AdvancedContrastEnhancement.Size = new System.Drawing.Size(213, 16);
            this.AdvancedContrastEnhancement.TabIndex = 15;
            this.AdvancedContrastEnhancement.Text = "Advanced Contrast Enhancement";
            this.AdvancedContrastEnhancement.UseVisualStyleBackColor = true;
            // 
            // groupRollCaptureOption
            // 
            this.groupRollCaptureOption.Controls.Add(this.label9);
            this.groupRollCaptureOption.Controls.Add(this.label10);
            this.groupRollCaptureOption.Controls.Add(this.TimeoutTextBox);
            this.groupRollCaptureOption.Controls.Add(this.label8);
            this.groupRollCaptureOption.Controls.Add(this.WriteProfile);
            this.groupRollCaptureOption.Controls.Add(this.ReadProfile);
            this.groupRollCaptureOption.Controls.Add(this.RollDirections);
            this.groupRollCaptureOption.Controls.Add(this.RollProfiles);
            this.groupRollCaptureOption.Location = new System.Drawing.Point(13, 12);
            this.groupRollCaptureOption.Name = "groupRollCaptureOption";
            this.groupRollCaptureOption.Size = new System.Drawing.Size(315, 104);
            this.groupRollCaptureOption.TabIndex = 0;
            this.groupRollCaptureOption.TabStop = false;
            this.groupRollCaptureOption.Text = "RollCaptureOption";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "Timeout(ms)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(42, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 12);
            this.label10.TabIndex = 22;
            this.label10.Text = "Direction";
            // 
            // TimeoutTextBox
            // 
            this.TimeoutTextBox.Location = new System.Drawing.Point(102, 72);
            this.TimeoutTextBox.Name = "TimeoutTextBox";
            this.TimeoutTextBox.Size = new System.Drawing.Size(115, 21);
            this.TimeoutTextBox.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "Profile";
            // 
            // WriteProfile
            // 
            this.WriteProfile.Location = new System.Drawing.Point(223, 43);
            this.WriteProfile.Name = "WriteProfile";
            this.WriteProfile.Size = new System.Drawing.Size(59, 23);
            this.WriteProfile.TabIndex = 14;
            this.WriteProfile.Text = "Write";
            this.WriteProfile.UseVisualStyleBackColor = true;
            this.WriteProfile.Click += new System.EventHandler(this.WriteRollOption_Click);
            // 
            // ReadProfile
            // 
            this.ReadProfile.Location = new System.Drawing.Point(223, 17);
            this.ReadProfile.Name = "ReadProfile";
            this.ReadProfile.Size = new System.Drawing.Size(57, 23);
            this.ReadProfile.TabIndex = 12;
            this.ReadProfile.Text = "Read";
            this.ReadProfile.UseVisualStyleBackColor = true;
            this.ReadProfile.Click += new System.EventHandler(this.ReadRollOption_Click);
            // 
            // RollDirections
            // 
            this.RollDirections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RollDirections.FormattingEnabled = true;
            this.RollDirections.Items.AddRange(new object[] {
            "L to R",
            "R to L",
            "Automatic",
            "Auto M"});
            this.RollDirections.Location = new System.Drawing.Point(102, 46);
            this.RollDirections.Name = "RollDirections";
            this.RollDirections.Size = new System.Drawing.Size(115, 20);
            this.RollDirections.TabIndex = 12;
            // 
            // RollProfiles
            // 
            this.RollProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RollProfiles.FormattingEnabled = true;
            this.RollProfiles.Items.AddRange(new object[] {
            "Low",
            "Normal",
            "High"});
            this.RollProfiles.Location = new System.Drawing.Point(102, 20);
            this.RollProfiles.Name = "RollProfiles";
            this.RollProfiles.Size = new System.Drawing.Size(115, 20);
            this.RollProfiles.TabIndex = 13;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ShowAlloverlay);
            this.tabPage4.Controls.Add(this.ClearAlloverlay);
            this.tabPage4.Controls.Add(this.HideAlloverlay);
            this.tabPage4.Controls.Add(this.groupQuadOverlay);
            this.tabPage4.Controls.Add(this.groupLineOverlay);
            this.tabPage4.Controls.Add(this.groupCrossOverlay);
            this.tabPage4.Controls.Add(this.groupTextOverlay);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(729, 246);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ShowAlloverlay
            // 
            this.ShowAlloverlay.Location = new System.Drawing.Point(590, 43);
            this.ShowAlloverlay.Name = "ShowAlloverlay";
            this.ShowAlloverlay.Size = new System.Drawing.Size(135, 23);
            this.ShowAlloverlay.TabIndex = 37;
            this.ShowAlloverlay.Text = "Show All";
            this.ShowAlloverlay.UseVisualStyleBackColor = true;
            this.ShowAlloverlay.Click += new System.EventHandler(this.ShowAll_Click);
            // 
            // ClearAlloverlay
            // 
            this.ClearAlloverlay.Location = new System.Drawing.Point(590, 70);
            this.ClearAlloverlay.Name = "ClearAlloverlay";
            this.ClearAlloverlay.Size = new System.Drawing.Size(135, 23);
            this.ClearAlloverlay.TabIndex = 36;
            this.ClearAlloverlay.Text = "Clear All";
            this.ClearAlloverlay.UseVisualStyleBackColor = true;
            this.ClearAlloverlay.Click += new System.EventHandler(this.ClearAll_Click);
            // 
            // HideAlloverlay
            // 
            this.HideAlloverlay.Location = new System.Drawing.Point(590, 16);
            this.HideAlloverlay.Name = "HideAlloverlay";
            this.HideAlloverlay.Size = new System.Drawing.Size(135, 23);
            this.HideAlloverlay.TabIndex = 35;
            this.HideAlloverlay.Text = "Hide All";
            this.HideAlloverlay.UseVisualStyleBackColor = true;
            this.HideAlloverlay.Click += new System.EventHandler(this.HideAll_Click);
            // 
            // groupQuadOverlay
            // 
            this.groupQuadOverlay.Controls.Add(this.label40);
            this.groupQuadOverlay.Controls.Add(this.label36);
            this.groupQuadOverlay.Controls.Add(this.label41);
            this.groupQuadOverlay.Controls.Add(this.label37);
            this.groupQuadOverlay.Controls.Add(this.label38);
            this.groupQuadOverlay.Controls.Add(this.label39);
            this.groupQuadOverlay.Controls.Add(this.label32);
            this.groupQuadOverlay.Controls.Add(this.m_QuadX3);
            this.groupQuadOverlay.Controls.Add(this.label33);
            this.groupQuadOverlay.Controls.Add(this.label34);
            this.groupQuadOverlay.Controls.Add(this.m_QuadY3);
            this.groupQuadOverlay.Controls.Add(this.label35);
            this.groupQuadOverlay.Controls.Add(this.m_QuadX4);
            this.groupQuadOverlay.Controls.Add(this.m_QuadY4);
            this.groupQuadOverlay.Controls.Add(this.m_QuadWidth);
            this.groupQuadOverlay.Controls.Add(this.m_QuadY2);
            this.groupQuadOverlay.Controls.Add(this.m_QuadX2);
            this.groupQuadOverlay.Controls.Add(this.m_QuadY1);
            this.groupQuadOverlay.Controls.Add(this.m_QuadX1);
            this.groupQuadOverlay.Controls.Add(this.SelectQuadColor);
            this.groupQuadOverlay.Controls.Add(this.ShowQuadrangle);
            this.groupQuadOverlay.Location = new System.Drawing.Point(444, 9);
            this.groupQuadOverlay.Name = "groupQuadOverlay";
            this.groupQuadOverlay.Size = new System.Drawing.Size(140, 218);
            this.groupQuadOverlay.TabIndex = 28;
            this.groupQuadOverlay.TabStop = false;
            this.groupQuadOverlay.Text = "Quadrangle";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 161);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(35, 12);
            this.label40.TabIndex = 55;
            this.label40.Text = "Color";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(83, 107);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(19, 12);
            this.label36.TabIndex = 58;
            this.label36.Text = "Y4";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(3, 135);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(35, 12);
            this.label41.TabIndex = 54;
            this.label41.Text = "Width";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(83, 78);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(19, 12);
            this.label37.TabIndex = 59;
            this.label37.Text = "Y3";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(11, 106);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(19, 12);
            this.label38.TabIndex = 60;
            this.label38.Text = "X4";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(11, 78);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(19, 12);
            this.label39.TabIndex = 61;
            this.label39.Text = "X3";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(83, 52);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(19, 12);
            this.label32.TabIndex = 54;
            this.label32.Text = "Y2";
            // 
            // m_QuadX3
            // 
            this.m_QuadX3.Location = new System.Drawing.Point(41, 75);
            this.m_QuadX3.Name = "m_QuadX3";
            this.m_QuadX3.Size = new System.Drawing.Size(26, 21);
            this.m_QuadX3.TabIndex = 34;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(83, 23);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(19, 12);
            this.label33.TabIndex = 55;
            this.label33.Text = "Y1";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(11, 51);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(19, 12);
            this.label34.TabIndex = 56;
            this.label34.Text = "X2";
            // 
            // m_QuadY3
            // 
            this.m_QuadY3.Location = new System.Drawing.Point(110, 75);
            this.m_QuadY3.Name = "m_QuadY3";
            this.m_QuadY3.Size = new System.Drawing.Size(26, 21);
            this.m_QuadY3.TabIndex = 33;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(11, 23);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(19, 12);
            this.label35.TabIndex = 57;
            this.label35.Text = "X1";
            // 
            // m_QuadX4
            // 
            this.m_QuadX4.Location = new System.Drawing.Point(41, 102);
            this.m_QuadX4.Name = "m_QuadX4";
            this.m_QuadX4.Size = new System.Drawing.Size(26, 21);
            this.m_QuadX4.TabIndex = 32;
            // 
            // m_QuadY4
            // 
            this.m_QuadY4.Location = new System.Drawing.Point(110, 102);
            this.m_QuadY4.Name = "m_QuadY4";
            this.m_QuadY4.Size = new System.Drawing.Size(26, 21);
            this.m_QuadY4.TabIndex = 31;
            // 
            // m_QuadWidth
            // 
            this.m_QuadWidth.Location = new System.Drawing.Point(45, 129);
            this.m_QuadWidth.Name = "m_QuadWidth";
            this.m_QuadWidth.Size = new System.Drawing.Size(92, 21);
            this.m_QuadWidth.TabIndex = 26;
            // 
            // m_QuadY2
            // 
            this.m_QuadY2.Location = new System.Drawing.Point(110, 48);
            this.m_QuadY2.Name = "m_QuadY2";
            this.m_QuadY2.Size = new System.Drawing.Size(26, 21);
            this.m_QuadY2.TabIndex = 25;
            // 
            // m_QuadX2
            // 
            this.m_QuadX2.Location = new System.Drawing.Point(41, 47);
            this.m_QuadX2.Name = "m_QuadX2";
            this.m_QuadX2.Size = new System.Drawing.Size(26, 21);
            this.m_QuadX2.TabIndex = 24;
            // 
            // m_QuadY1
            // 
            this.m_QuadY1.Location = new System.Drawing.Point(110, 20);
            this.m_QuadY1.Name = "m_QuadY1";
            this.m_QuadY1.Size = new System.Drawing.Size(26, 21);
            this.m_QuadY1.TabIndex = 21;
            // 
            // m_QuadX1
            // 
            this.m_QuadX1.Location = new System.Drawing.Point(41, 21);
            this.m_QuadX1.Name = "m_QuadX1";
            this.m_QuadX1.Size = new System.Drawing.Size(26, 21);
            this.m_QuadX1.TabIndex = 22;
            // 
            // SelectQuadColor
            // 
            this.SelectQuadColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectQuadColor.FormattingEnabled = true;
            this.SelectQuadColor.Items.AddRange(new object[] {
            "Black",
            "Red",
            "Green",
            "Blue"});
            this.SelectQuadColor.Location = new System.Drawing.Point(45, 156);
            this.SelectQuadColor.Name = "SelectQuadColor";
            this.SelectQuadColor.Size = new System.Drawing.Size(92, 20);
            this.SelectQuadColor.TabIndex = 16;
            // 
            // ShowQuadrangle
            // 
            this.ShowQuadrangle.Location = new System.Drawing.Point(5, 187);
            this.ShowQuadrangle.Name = "ShowQuadrangle";
            this.ShowQuadrangle.Size = new System.Drawing.Size(135, 23);
            this.ShowQuadrangle.TabIndex = 6;
            this.ShowQuadrangle.Text = "Show";
            this.ShowQuadrangle.UseVisualStyleBackColor = true;
            this.ShowQuadrangle.Click += new System.EventHandler(this.ShowQuardangle_Click);
            // 
            // groupLineOverlay
            // 
            this.groupLineOverlay.Controls.Add(this.label28);
            this.groupLineOverlay.Controls.Add(this.label29);
            this.groupLineOverlay.Controls.Add(this.label30);
            this.groupLineOverlay.Controls.Add(this.label31);
            this.groupLineOverlay.Controls.Add(this.label27);
            this.groupLineOverlay.Controls.Add(this.label26);
            this.groupLineOverlay.Controls.Add(this.m_LineWidth);
            this.groupLineOverlay.Controls.Add(this.m_LineY2);
            this.groupLineOverlay.Controls.Add(this.m_LineX2);
            this.groupLineOverlay.Controls.Add(this.m_LineY1);
            this.groupLineOverlay.Controls.Add(this.m_LineX1);
            this.groupLineOverlay.Controls.Add(this.SelectLineColor);
            this.groupLineOverlay.Controls.Add(this.ShowLine);
            this.groupLineOverlay.Location = new System.Drawing.Point(298, 11);
            this.groupLineOverlay.Name = "groupLineOverlay";
            this.groupLineOverlay.Size = new System.Drawing.Size(140, 155);
            this.groupLineOverlay.TabIndex = 27;
            this.groupLineOverlay.TabStop = false;
            this.groupLineOverlay.Text = "Line";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(83, 53);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(19, 12);
            this.label28.TabIndex = 50;
            this.label28.Text = "Y2";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(83, 24);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(19, 12);
            this.label29.TabIndex = 51;
            this.label29.Text = "Y1";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(11, 51);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(19, 12);
            this.label30.TabIndex = 52;
            this.label30.Text = "X2";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(11, 23);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(19, 12);
            this.label31.TabIndex = 53;
            this.label31.Text = "X1";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(3, 104);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(35, 12);
            this.label27.TabIndex = 50;
            this.label27.Text = "Color";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(3, 78);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(35, 12);
            this.label26.TabIndex = 50;
            this.label26.Text = "Width";
            // 
            // m_LineWidth
            // 
            this.m_LineWidth.Location = new System.Drawing.Point(44, 74);
            this.m_LineWidth.Name = "m_LineWidth";
            this.m_LineWidth.Size = new System.Drawing.Size(92, 21);
            this.m_LineWidth.TabIndex = 26;
            // 
            // m_LineY2
            // 
            this.m_LineY2.Location = new System.Drawing.Point(110, 48);
            this.m_LineY2.Name = "m_LineY2";
            this.m_LineY2.Size = new System.Drawing.Size(26, 21);
            this.m_LineY2.TabIndex = 25;
            // 
            // m_LineX2
            // 
            this.m_LineX2.Location = new System.Drawing.Point(41, 47);
            this.m_LineX2.Name = "m_LineX2";
            this.m_LineX2.Size = new System.Drawing.Size(26, 21);
            this.m_LineX2.TabIndex = 24;
            // 
            // m_LineY1
            // 
            this.m_LineY1.Location = new System.Drawing.Point(110, 20);
            this.m_LineY1.Name = "m_LineY1";
            this.m_LineY1.Size = new System.Drawing.Size(26, 21);
            this.m_LineY1.TabIndex = 21;
            // 
            // m_LineX1
            // 
            this.m_LineX1.Location = new System.Drawing.Point(41, 21);
            this.m_LineX1.Name = "m_LineX1";
            this.m_LineX1.Size = new System.Drawing.Size(26, 21);
            this.m_LineX1.TabIndex = 22;
            // 
            // SelectLineColor
            // 
            this.SelectLineColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectLineColor.FormattingEnabled = true;
            this.SelectLineColor.Items.AddRange(new object[] {
            "Black",
            "Red",
            "Green",
            "Blue"});
            this.SelectLineColor.Location = new System.Drawing.Point(44, 101);
            this.SelectLineColor.Name = "SelectLineColor";
            this.SelectLineColor.Size = new System.Drawing.Size(92, 20);
            this.SelectLineColor.TabIndex = 16;
            // 
            // ShowLine
            // 
            this.ShowLine.Location = new System.Drawing.Point(1, 127);
            this.ShowLine.Name = "ShowLine";
            this.ShowLine.Size = new System.Drawing.Size(135, 23);
            this.ShowLine.TabIndex = 6;
            this.ShowLine.Text = "Show";
            this.ShowLine.UseVisualStyleBackColor = true;
            this.ShowLine.Click += new System.EventHandler(this.ShowLineOverlay_Click);
            // 
            // groupCrossOverlay
            // 
            this.groupCrossOverlay.Controls.Add(this.label25);
            this.groupCrossOverlay.Controls.Add(this.label24);
            this.groupCrossOverlay.Controls.Add(this.label23);
            this.groupCrossOverlay.Controls.Add(this.label22);
            this.groupCrossOverlay.Controls.Add(this.label21);
            this.groupCrossOverlay.Controls.Add(this.m_CrossWidth);
            this.groupCrossOverlay.Controls.Add(this.label20);
            this.groupCrossOverlay.Controls.Add(this.m_CrossDY);
            this.groupCrossOverlay.Controls.Add(this.m_CrossDX);
            this.groupCrossOverlay.Controls.Add(this.m_CrossY);
            this.groupCrossOverlay.Controls.Add(this.m_CrossX);
            this.groupCrossOverlay.Controls.Add(this.SelectCrossColor);
            this.groupCrossOverlay.Controls.Add(this.ShowCross);
            this.groupCrossOverlay.Location = new System.Drawing.Point(152, 9);
            this.groupCrossOverlay.Name = "groupCrossOverlay";
            this.groupCrossOverlay.Size = new System.Drawing.Size(140, 162);
            this.groupCrossOverlay.TabIndex = 20;
            this.groupCrossOverlay.TabStop = false;
            this.groupCrossOverlay.Text = "Cross";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 77);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(35, 12);
            this.label25.TabIndex = 49;
            this.label25.Text = "Width";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 105);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 12);
            this.label24.TabIndex = 48;
            this.label24.Text = "Color";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(89, 53);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(20, 12);
            this.label23.TabIndex = 47;
            this.label23.Text = "dY";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(17, 53);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(20, 12);
            this.label22.TabIndex = 46;
            this.label22.Text = "dX";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(89, 24);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(13, 12);
            this.label21.TabIndex = 45;
            this.label21.Text = "Y";
            // 
            // m_CrossWidth
            // 
            this.m_CrossWidth.Location = new System.Drawing.Point(44, 74);
            this.m_CrossWidth.Name = "m_CrossWidth";
            this.m_CrossWidth.Size = new System.Drawing.Size(92, 21);
            this.m_CrossWidth.TabIndex = 26;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(17, 23);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(13, 12);
            this.label20.TabIndex = 44;
            this.label20.Text = "X";
            // 
            // m_CrossDY
            // 
            this.m_CrossDY.Location = new System.Drawing.Point(110, 48);
            this.m_CrossDY.Name = "m_CrossDY";
            this.m_CrossDY.Size = new System.Drawing.Size(26, 21);
            this.m_CrossDY.TabIndex = 25;
            // 
            // m_CrossDX
            // 
            this.m_CrossDX.Location = new System.Drawing.Point(41, 47);
            this.m_CrossDX.Name = "m_CrossDX";
            this.m_CrossDX.Size = new System.Drawing.Size(26, 21);
            this.m_CrossDX.TabIndex = 24;
            // 
            // m_CrossY
            // 
            this.m_CrossY.Location = new System.Drawing.Point(110, 20);
            this.m_CrossY.Name = "m_CrossY";
            this.m_CrossY.Size = new System.Drawing.Size(26, 21);
            this.m_CrossY.TabIndex = 21;
            // 
            // m_CrossX
            // 
            this.m_CrossX.Location = new System.Drawing.Point(41, 21);
            this.m_CrossX.Name = "m_CrossX";
            this.m_CrossX.Size = new System.Drawing.Size(26, 21);
            this.m_CrossX.TabIndex = 22;
            // 
            // SelectCrossColor
            // 
            this.SelectCrossColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectCrossColor.FormattingEnabled = true;
            this.SelectCrossColor.Items.AddRange(new object[] {
            "Black",
            "Red",
            "Green",
            "Blue"});
            this.SelectCrossColor.Location = new System.Drawing.Point(44, 101);
            this.SelectCrossColor.Name = "SelectCrossColor";
            this.SelectCrossColor.Size = new System.Drawing.Size(92, 20);
            this.SelectCrossColor.TabIndex = 16;
            // 
            // ShowCross
            // 
            this.ShowCross.Location = new System.Drawing.Point(1, 127);
            this.ShowCross.Name = "ShowCross";
            this.ShowCross.Size = new System.Drawing.Size(135, 23);
            this.ShowCross.TabIndex = 6;
            this.ShowCross.Text = "Show";
            this.ShowCross.UseVisualStyleBackColor = true;
            this.ShowCross.Click += new System.EventHandler(this.ShowCrossOverlay_Click);
            // 
            // groupTextOverlay
            // 
            this.groupTextOverlay.Controls.Add(this.label19);
            this.groupTextOverlay.Controls.Add(this.label18);
            this.groupTextOverlay.Controls.Add(this.label17);
            this.groupTextOverlay.Controls.Add(this.label16);
            this.groupTextOverlay.Controls.Add(this.label15);
            this.groupTextOverlay.Controls.Add(this.label14);
            this.groupTextOverlay.Controls.Add(this.m_TextY);
            this.groupTextOverlay.Controls.Add(this.m_Text);
            this.groupTextOverlay.Controls.Add(this.SelectTextColor);
            this.groupTextOverlay.Controls.Add(this.SelectTextSize);
            this.groupTextOverlay.Controls.Add(this.m_TextX);
            this.groupTextOverlay.Controls.Add(this.ShowText);
            this.groupTextOverlay.Controls.Add(this.SelectTextAlign);
            this.groupTextOverlay.Location = new System.Drawing.Point(6, 6);
            this.groupTextOverlay.Name = "groupTextOverlay";
            this.groupTextOverlay.Size = new System.Drawing.Size(140, 181);
            this.groupTextOverlay.TabIndex = 12;
            this.groupTextOverlay.TabStop = false;
            this.groupTextOverlay.Text = "Text";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(17, 50);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(33, 12);
            this.label19.TabIndex = 43;
            this.label19.Text = "Align";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 77);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 12);
            this.label18.TabIndex = 42;
            this.label18.Text = "Color";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 104);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 12);
            this.label17.TabIndex = 41;
            this.label17.Text = "Size";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(20, 132);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 12);
            this.label16.TabIndex = 40;
            this.label16.Text = "Text";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(88, 24);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(13, 12);
            this.label15.TabIndex = 39;
            this.label15.Text = "Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 12);
            this.label14.TabIndex = 38;
            this.label14.Text = "X";
            // 
            // m_TextY
            // 
            this.m_TextY.Location = new System.Drawing.Point(107, 20);
            this.m_TextY.Name = "m_TextY";
            this.m_TextY.Size = new System.Drawing.Size(26, 21);
            this.m_TextY.TabIndex = 20;
            // 
            // m_Text
            // 
            this.m_Text.Location = new System.Drawing.Point(56, 127);
            this.m_Text.Name = "m_Text";
            this.m_Text.Size = new System.Drawing.Size(77, 21);
            this.m_Text.TabIndex = 19;
            // 
            // SelectTextColor
            // 
            this.SelectTextColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectTextColor.FormattingEnabled = true;
            this.SelectTextColor.Items.AddRange(new object[] {
            "Black",
            "Red",
            "Green",
            "Blue"});
            this.SelectTextColor.Location = new System.Drawing.Point(56, 75);
            this.SelectTextColor.Name = "SelectTextColor";
            this.SelectTextColor.Size = new System.Drawing.Size(77, 20);
            this.SelectTextColor.TabIndex = 17;
            // 
            // SelectTextSize
            // 
            this.SelectTextSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectTextSize.FormattingEnabled = true;
            this.SelectTextSize.Items.AddRange(new object[] {
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "24",
            "28"});
            this.SelectTextSize.Location = new System.Drawing.Point(56, 101);
            this.SelectTextSize.Name = "SelectTextSize";
            this.SelectTextSize.Size = new System.Drawing.Size(77, 20);
            this.SelectTextSize.TabIndex = 16;
            // 
            // m_TextX
            // 
            this.m_TextX.Location = new System.Drawing.Point(41, 20);
            this.m_TextX.Name = "m_TextX";
            this.m_TextX.Size = new System.Drawing.Size(26, 21);
            this.m_TextX.TabIndex = 12;
            // 
            // ShowText
            // 
            this.ShowText.Location = new System.Drawing.Point(15, 154);
            this.ShowText.Name = "ShowText";
            this.ShowText.Size = new System.Drawing.Size(118, 23);
            this.ShowText.TabIndex = 6;
            this.ShowText.Text = "Show";
            this.ShowText.UseVisualStyleBackColor = true;
            this.ShowText.Click += new System.EventHandler(this.ShowTextOverlay_Click);
            // 
            // SelectTextAlign
            // 
            this.SelectTextAlign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectTextAlign.FormattingEnabled = true;
            this.SelectTextAlign.Items.AddRange(new object[] {
            "Left",
            "Center",
            "Right"});
            this.SelectTextAlign.Location = new System.Drawing.Point(56, 47);
            this.SelectTextAlign.Name = "SelectTextAlign";
            this.SelectTextAlign.Size = new System.Drawing.Size(77, 20);
            this.SelectTextAlign.TabIndex = 6;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox5);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(729, 246);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Callback Option";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(501, 10);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(215, 230);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Capture Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.seqTargetList);
            this.groupBox7.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox7.Location = new System.Drawing.Point(9, 78);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(195, 140);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Target Images (Seq. Check)";
            // 
            // seqTargetList
            // 
            this.seqTargetList.FormattingEnabled = true;
            this.seqTargetList.Location = new System.Drawing.Point(22, 19);
            this.seqTargetList.Name = "seqTargetList";
            this.seqTargetList.Size = new System.Drawing.Size(145, 79);
            this.seqTargetList.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.selCaptureDir);
            this.groupBox6.Location = new System.Drawing.Point(9, 20);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(196, 51);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Direction";
            // 
            // selCaptureDir
            // 
            this.selCaptureDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selCaptureDir.FormattingEnabled = true;
            this.selCaptureDir.Items.AddRange(new object[] {
            "Default",
            "Left",
            "Right"});
            this.selCaptureDir.Location = new System.Drawing.Point(46, 20);
            this.selCaptureDir.Name = "selCaptureDir";
            this.selCaptureDir.Size = new System.Drawing.Size(121, 20);
            this.selCaptureDir.TabIndex = 0;
            this.selCaptureDir.SelectedIndexChanged += new System.EventHandler(this.selCaptureDir_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(14, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(485, 230);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Segmentation Options";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.SegHeight);
            this.groupBox4.Controls.Add(this.SegWidth);
            this.groupBox4.Controls.Add(this.label44);
            this.groupBox4.Controls.Add(this.label43);
            this.groupBox4.Controls.Add(this.setSegmentSize);
            this.groupBox4.Location = new System.Drawing.Point(335, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(140, 199);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Custom Size";
            // 
            // SegHeight
            // 
            this.SegHeight.Location = new System.Drawing.Point(101, 56);
            this.SegHeight.Name = "SegHeight";
            this.SegHeight.Size = new System.Drawing.Size(29, 21);
            this.SegHeight.TabIndex = 4;
            // 
            // SegWidth
            // 
            this.SegWidth.Location = new System.Drawing.Point(36, 56);
            this.SegWidth.Name = "SegWidth";
            this.SegWidth.Size = new System.Drawing.Size(29, 21);
            this.SegWidth.TabIndex = 3;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(75, 59);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(20, 12);
            this.label44.TabIndex = 2;
            this.label44.Text = "h :";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(7, 59);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(23, 12);
            this.label43.TabIndex = 1;
            this.label43.Text = "w :";
            // 
            // setSegmentSize
            // 
            this.setSegmentSize.AutoSize = true;
            this.setSegmentSize.Location = new System.Drawing.Point(16, 25);
            this.setSegmentSize.Name = "setSegmentSize";
            this.setSegmentSize.Size = new System.Drawing.Size(103, 16);
            this.setSegmentSize.TabIndex = 0;
            this.setSegmentSize.Text = "Segment Size";
            this.setSegmentSize.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rightFingers);
            this.groupBox3.Controls.Add(this.leftFingers);
            this.groupBox3.Location = new System.Drawing.Point(10, 76);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(317, 143);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Missing Fingers";
            // 
            // rightFingers
            // 
            this.rightFingers.FormattingEnabled = true;
            this.rightFingers.Items.AddRange(new object[] {
            "Right Little",
            "Right Ring",
            "Right Middle",
            "Right Index"});
            this.rightFingers.Location = new System.Drawing.Point(169, 25);
            this.rightFingers.Name = "rightFingers";
            this.rightFingers.Size = new System.Drawing.Size(117, 68);
            this.rightFingers.TabIndex = 3;
            // 
            // leftFingers
            // 
            this.leftFingers.BackColor = System.Drawing.SystemColors.Window;
            this.leftFingers.FormattingEnabled = true;
            this.leftFingers.Items.AddRange(new object[] {
            "Left Little",
            "Left Ring",
            "Left Middle",
            "Left Index"});
            this.leftFingers.Location = new System.Drawing.Point(28, 25);
            this.leftFingers.Name = "leftFingers";
            this.leftFingers.Size = new System.Drawing.Size(117, 68);
            this.leftFingers.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.selMinFingerCount);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Location = new System.Drawing.Point(8, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 52);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensitivity";
            // 
            // selMinFingerCount
            // 
            this.selMinFingerCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selMinFingerCount.FormattingEnabled = true;
            this.selMinFingerCount.Items.AddRange(new object[] {
            "4",
            "3",
            "2",
            "1"});
            this.selMinFingerCount.Location = new System.Drawing.Point(184, 18);
            this.selMinFingerCount.Name = "selMinFingerCount";
            this.selMinFingerCount.Size = new System.Drawing.Size(121, 20);
            this.selMinFingerCount.TabIndex = 1;
            this.selMinFingerCount.SelectedIndexChanged += new System.EventHandler(this.selMinFingerCount_SelectedIndexChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(9, 22);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(169, 12);
            this.label42.TabIndex = 0;
            this.label42.Text = "Minimum number of fingers :";
            // 
            // groupOverlay
            // 
            this.groupOverlay.Controls.Add(this.label1);
            this.groupOverlay.Controls.Add(this.Type);
            this.groupOverlay.Controls.Add(this.OverlayColor);
            this.groupOverlay.Controls.Add(this.OverlayType);
            this.groupOverlay.Controls.Add(this.ClearAllOverlays);
            this.groupOverlay.Controls.Add(this.DrawOverlay);
            this.groupOverlay.Location = new System.Drawing.Point(504, 395);
            this.groupOverlay.Name = "groupOverlay";
            this.groupOverlay.Size = new System.Drawing.Size(237, 80);
            this.groupOverlay.TabIndex = 6;
            this.groupOverlay.TabStop = false;
            this.groupOverlay.Text = "Overlay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Color";
            // 
            // Type
            // 
            this.Type.AutoSize = true;
            this.Type.Location = new System.Drawing.Point(18, 23);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(34, 12);
            this.Type.TabIndex = 16;
            this.Type.Text = "Type";
            // 
            // OverlayColor
            // 
            this.OverlayColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OverlayColor.FormattingEnabled = true;
            this.OverlayColor.Items.AddRange(new object[] {
            "Black",
            "Red",
            "Green",
            "Blue"});
            this.OverlayColor.Location = new System.Drawing.Point(68, 49);
            this.OverlayColor.Name = "OverlayColor";
            this.OverlayColor.Size = new System.Drawing.Size(65, 20);
            this.OverlayColor.TabIndex = 18;
            // 
            // OverlayType
            // 
            this.OverlayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OverlayType.FormattingEnabled = true;
            this.OverlayType.Items.AddRange(new object[] {
            "Text",
            "Cross",
            "Line",
            "Quadrangle"});
            this.OverlayType.Location = new System.Drawing.Point(68, 20);
            this.OverlayType.Name = "OverlayType";
            this.OverlayType.Size = new System.Drawing.Size(65, 20);
            this.OverlayType.TabIndex = 12;
            // 
            // ClearAllOverlays
            // 
            this.ClearAllOverlays.Enabled = false;
            this.ClearAllOverlays.Location = new System.Drawing.Point(143, 46);
            this.ClearAllOverlays.Name = "ClearAllOverlays";
            this.ClearAllOverlays.Size = new System.Drawing.Size(70, 23);
            this.ClearAllOverlays.TabIndex = 16;
            this.ClearAllOverlays.Text = "Clear All";
            this.ClearAllOverlays.UseVisualStyleBackColor = true;
            this.ClearAllOverlays.Click += new System.EventHandler(this.ClearAllOverlays_Click);
            // 
            // DrawOverlay
            // 
            this.DrawOverlay.Enabled = false;
            this.DrawOverlay.Location = new System.Drawing.Point(143, 17);
            this.DrawOverlay.Name = "DrawOverlay";
            this.DrawOverlay.Size = new System.Drawing.Size(70, 23);
            this.DrawOverlay.TabIndex = 17;
            this.DrawOverlay.Text = "Draw";
            this.DrawOverlay.UseVisualStyleBackColor = true;
            this.DrawOverlay.Click += new System.EventHandler(this.DrawOverlay_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(16, 511);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(98, 12);
            this.label50.TabIndex = 16;
            this.label50.Text = "Capture Guide : ";
            // 
            // captureGuideMsg
            // 
            this.captureGuideMsg.Location = new System.Drawing.Point(115, 504);
            this.captureGuideMsg.Name = "captureGuideMsg";
            this.captureGuideMsg.ReadOnly = true;
            this.captureGuideMsg.Size = new System.Drawing.Size(362, 21);
            this.captureGuideMsg.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 849);
            this.Controls.Add(this.captureGuideMsg);
            this.Controls.Add(this.label50);
            this.Controls.Add(this.groupOverlay);
            this.Controls.Add(this.DeviceTab);
            this.Controls.Add(this.groupPreview);
            this.Controls.Add(this.groupCapture);
            this.Controls.Add(this.PreviewWindow);
            this.Controls.Add(this.ExitDevice);
            this.Controls.Add(this.MsgPanel);
            this.Controls.Add(this.InitDevice);
            this.Controls.Add(this.InitSDK);
            this.Controls.Add(this.DeviceList);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PreviewWindow)).EndInit();
            this.groupDeviceInfo.ResumeLayout(false);
            this.groupDeviceInfo.PerformLayout();
            this.groupCapture.ResumeLayout(false);
            this.groupCapture.PerformLayout();
            this.groupPreview.ResumeLayout(false);
            this.groupPreview.PerformLayout();
            this.groupLCD.ResumeLayout(false);
            this.groupFingerLEDControl.ResumeLayout(false);
            this.DeviceTab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupMisc.ResumeLayout(false);
            this.groupSelfTest.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupKey.ResumeLayout(false);
            this.groupKey.PerformLayout();
            this.groupKeyCallback.ResumeLayout(false);
            this.groupKeyCallback.PerformLayout();
            this.groupStatusLEDControl.ResumeLayout(false);
            this.groupWav.ResumeLayout(false);
            this.groupBeep.ResumeLayout(false);
            this.groupModeLEDControl.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupLFD.ResumeLayout(false);
            this.groupLFD.PerformLayout();
            this.groupCaptureOptionsEtc.ResumeLayout(false);
            this.groupCaptureOptionsEtc.PerformLayout();
            this.groupRollCaptureOption.ResumeLayout(false);
            this.groupRollCaptureOption.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupQuadOverlay.ResumeLayout(false);
            this.groupQuadOverlay.PerformLayout();
            this.groupLineOverlay.ResumeLayout(false);
            this.groupLineOverlay.PerformLayout();
            this.groupCrossOverlay.ResumeLayout(false);
            this.groupCrossOverlay.PerformLayout();
            this.groupTextOverlay.ResumeLayout(false);
            this.groupTextOverlay.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupOverlay.ResumeLayout(false);
            this.groupOverlay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InitSDK;
        private System.Windows.Forms.TextBox MsgPanel;
        private System.Windows.Forms.PictureBox PreviewWindow;
        private System.Windows.Forms.GroupBox groupDeviceInfo;
        private System.Windows.Forms.TextBox DeviceInfo;
        private System.Windows.Forms.ComboBox DeviceList;
        private System.Windows.Forms.TextBox FirmwareInfo;
        private System.Windows.Forms.Button ExitDevice;
        private System.Windows.Forms.Button InitDevice;
        private System.Windows.Forms.GroupBox groupCapture;
        private System.Windows.Forms.CheckBox AutoCapture;
        private System.Windows.Forms.Button StartCapture;
        private System.Windows.Forms.ComboBox CaptureMode;
        private System.Windows.Forms.Button StopCapture;
        private System.Windows.Forms.ComboBox Callback;
        private System.Windows.Forms.TextBox ImageSize;
        private System.Windows.Forms.GroupBox groupPreview;
        private System.Windows.Forms.TextBox SDKInfo;
        private System.Windows.Forms.Button TakeAuto;
        private System.Windows.Forms.Button TakeManual;
        private System.Windows.Forms.GroupBox groupLCD;
        private System.Windows.Forms.Button ResetLCD;
        private System.Windows.Forms.Button DisplayLCD;
        private System.Windows.Forms.TextBox Hardwareinfo;
        private System.Windows.Forms.Button FingerLEDOn;
        private System.Windows.Forms.Button FingerLEDOff;
        private System.Windows.Forms.GroupBox groupFingerLEDControl;
        private System.Windows.Forms.ComboBox Finger_LED;
        private System.Windows.Forms.TabControl DeviceTab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupModeLEDControl;
        private System.Windows.Forms.ComboBox Mode_LED;
        private System.Windows.Forms.Button ModeLEDOff;
        private System.Windows.Forms.Button ModeLEDOn;
        private System.Windows.Forms.ComboBox FingerColor;
        private System.Windows.Forms.GroupBox groupSelfTest;
        private System.Windows.Forms.ComboBox TestType;
        private System.Windows.Forms.Button TestRun;
        private System.Windows.Forms.GroupBox groupMisc;
        private System.Windows.Forms.Button ManualCalibrate;
        private System.Windows.Forms.GroupBox groupBeep;
        private System.Windows.Forms.ComboBox BeepPattern;
        private System.Windows.Forms.Button EmitBeep;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupWav;
        private System.Windows.Forms.Button WavPlay;
        private System.Windows.Forms.GroupBox groupRollCaptureOption;
        private System.Windows.Forms.Button WriteProfile;
        private System.Windows.Forms.Button ReadProfile;
        private System.Windows.Forms.ComboBox RollDirections;
        private System.Windows.Forms.ComboBox RollProfiles;
        private System.Windows.Forms.GroupBox groupCaptureOptionsEtc;
        private System.Windows.Forms.Button WriteCaptureOption;
        private System.Windows.Forms.Button ReadCaptureOption;
        private System.Windows.Forms.ComboBox AutoSensitivity;
        private System.Windows.Forms.CheckBox ContrastEnhancement;
        private System.Windows.Forms.CheckBox NoiseReduction;
        private System.Windows.Forms.TextBox ReductionLevel;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupTextOverlay;
        private System.Windows.Forms.TextBox m_TextX;
        private System.Windows.Forms.Button ShowText;
        private System.Windows.Forms.ComboBox SelectTextAlign;
        private System.Windows.Forms.TextBox m_Text;
        private System.Windows.Forms.ComboBox SelectTextColor;
        private System.Windows.Forms.ComboBox SelectTextSize;
        private System.Windows.Forms.GroupBox groupCrossOverlay;
        private System.Windows.Forms.TextBox m_CrossY;
        private System.Windows.Forms.TextBox m_CrossX;
        private System.Windows.Forms.ComboBox SelectCrossColor;
        private System.Windows.Forms.Button ShowCross;
        private System.Windows.Forms.TextBox m_TextY;
        private System.Windows.Forms.GroupBox groupLineOverlay;
        private System.Windows.Forms.TextBox m_LineWidth;
        private System.Windows.Forms.TextBox m_LineY2;
        private System.Windows.Forms.TextBox m_LineX2;
        private System.Windows.Forms.TextBox m_LineY1;
        private System.Windows.Forms.TextBox m_LineX1;
        private System.Windows.Forms.ComboBox SelectLineColor;
        private System.Windows.Forms.Button ShowLine;
        private System.Windows.Forms.TextBox m_CrossWidth;
        private System.Windows.Forms.TextBox m_CrossDY;
        private System.Windows.Forms.TextBox m_CrossDX;
        private System.Windows.Forms.GroupBox groupQuadOverlay;
        private System.Windows.Forms.TextBox m_QuadWidth;
        private System.Windows.Forms.TextBox m_QuadY2;
        private System.Windows.Forms.TextBox m_QuadX2;
        private System.Windows.Forms.TextBox m_QuadY1;
        private System.Windows.Forms.TextBox m_QuadX1;
        private System.Windows.Forms.ComboBox SelectQuadColor;
        private System.Windows.Forms.Button ShowQuadrangle;
        private System.Windows.Forms.TextBox m_QuadX3;
        private System.Windows.Forms.TextBox m_QuadY3;
        private System.Windows.Forms.TextBox m_QuadX4;
        private System.Windows.Forms.TextBox m_QuadY4;
        private System.Windows.Forms.Button ShowAlloverlay;
        private System.Windows.Forms.Button ClearAlloverlay;
        private System.Windows.Forms.Button HideAlloverlay;
        private System.Windows.Forms.GroupBox groupStatusLEDControl;
        private System.Windows.Forms.ComboBox StatusLEDColor;
        private System.Windows.Forms.Button StatusLEDOff;
        private System.Windows.Forms.Button StatusLEDOn;
        private System.Windows.Forms.ComboBox cbLFDLevel;
        private System.Windows.Forms.CheckBox AdvancedContrastEnhancement;
        private System.Windows.Forms.CheckBox AutoCalibrate;
        private System.Windows.Forms.TextBox TimeoutTextBox;
        private System.Windows.Forms.GroupBox groupKey;
        private System.Windows.Forms.GroupBox groupKeyCallback;
        private System.Windows.Forms.TextBox CALLBACKPRINT;
        private System.Windows.Forms.ComboBox KEYCALLBACK;
        private System.Windows.Forms.TextBox KEYSTATUS;
        private System.Windows.Forms.Button ReadKeycallback;
        private System.Windows.Forms.Button ActivateKey;
        private System.Windows.Forms.ComboBox KeyMask;
        private System.Windows.Forms.TextBox DeviceID;
        private System.Windows.Forms.GroupBox groupLFD;
        private System.Windows.Forms.Button writeLFDLevel;
        private System.Windows.Forms.Button readLFDLevel;
        private System.Windows.Forms.GroupBox groupOverlay;
        private System.Windows.Forms.ComboBox OverlayColor;
        private System.Windows.Forms.ComboBox OverlayType;
        private System.Windows.Forms.Button ClearAllOverlays;
        private System.Windows.Forms.Button DrawOverlay;
        private System.Windows.Forms.Label Type;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox selMinFingerCount;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox SegWidth;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.CheckBox setSegmentSize;
        private System.Windows.Forms.TextBox SegHeight;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox selCaptureDir;
        private System.Windows.Forms.CheckedListBox rightFingers;
        private System.Windows.Forms.CheckedListBox leftFingers;
        private System.Windows.Forms.RadioButton callbackDrawBtn;
        private System.Windows.Forms.RadioButton directDrawBtn;
        private System.Windows.Forms.RadioButton advCallbackDrawBtn;
        private System.Windows.Forms.TextBox customCaptX;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox customCaptY;
        private System.Windows.Forms.TextBox customCaptH;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox customCaptW;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox captureGuideMsg;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckedListBox seqTargetList;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button brnViewSoftwareLicense;
    }
}

