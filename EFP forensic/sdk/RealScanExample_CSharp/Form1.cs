using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using RealScan;
using System.Windows;
using System.Drawing.Text;
using System.Threading;
using System.Diagnostics;



namespace RealScanUICSharp
{
    public partial class Form1 : Form
    {
        int m_result = 0;
        int m_errorcode = 0;

        int m_overlayHandle;
        int deviceHandle = 0;
        int deviceType_ = 0;

        bool m_automatic;
        bool m_advancedContrast;
        int m_manualContrast;
        int m_rollDirection, m_rollTime, m_rollProfile;
        int m_mode, m_option;
        bool m_contrastEnhancement, m_noiseReduction;
        int m_reductionLevel;

        int m_keyCode;

        string m_errorMsg = " ";
        bool m_prevStopped = true;

        bool m_bCaptureModeSelected = false;
        int m_captureMode = 0;
        int m_minCount = 0;
        int m_captureDir = 0;
        int m_slapType = 0;
        int m_fingerCount = 0;

        int m_nCustomSegWidth = 0;
        int m_nCustomSegHeight = 0;

        int m_nCustomCaptX;
        int m_nCustomCaptY;
        int m_nCustomCaptW;
        int m_nCustomCaptH;

        string m_strLicenseFile = "";

        int m_nCheckNIST = 0;
        public const int DF_NIST_LEFT4  = 0x01;
        public const int DF_NIST_RIGHT4 = 0x02;
        public const int DF_NIST_THUMB2 = 0x04;
        public const int DF_NIST_ALL    = DF_NIST_LEFT4 | DF_NIST_RIGHT4 | DF_NIST_THUMB2;

        byte[] m_left4Fingers;
        byte[] m_right4Fingers;
        byte[] m_thumb2Fingers;

        int m_nLeft4imageWidth;
        int m_nLeft4imageHeight;
        int m_nRight4imageWidth;
        int m_nRight4imageHeight;
        int m_nThumb2ImageWidth;
        int m_nThumb2ImageHeight;

        enum PrevMode
        {
            directDraw,
            callbackDraw,
            advCallbackDraw
        }

        enum callbackMode
        {
            none,
            save,
            segment,
            enlarge,
            saveNseg,
            seqCheck,
            save19794_4_2011
            
            
        }
        public struct DeviceIndexInfo
        {
            public bool isInit;
            public int deviceIndex;
            public int deviceHandle;
            public DeviceIndexInfo(bool init, int index, int handle)
            {
                isInit = init;
                deviceIndex = index;
                deviceHandle = handle;
            }
        };

        private PrevMode _selectedPrevMode;
        RSPreviewDataCallback previewCallback;
        RSAdvPreviewCallback advPreviewCallback;
        CaptureDataCallback rawCaptureCallback;
        //AdvCaptureDataCallback advCaptureCallback;
        AdvCaptureDataCallbackInt advCaptureCallback;

        RSHotPluggingCallback hotplugingCallback;

        private RSAdvCaptureUiCallback _advUiCb;
        private readonly RSRawCaptureCallback _rawUiCb;

        private Thread autoCaptureThread = null;
        delegate void afterAutoCaptureCallback(int captureResult);

        int capturedImageWidth;
        int capturedImageHeight;
        IntPtr capturedImageData;

        byte[][] m_pSeqCheckTargetImages = new byte[5][];
        int[] m_nSeqCheckTargetWidths = new int[5];
        int[] m_nSeqCheckTargetHeights = new int[5];
        int[] m_nSeqCheckTargetSlapTypes = new int[5];
        int m_numOfTargerFingers = 0;
        int m_numOfTargets;

        const int LFD_THRESHOLD = 730;
        private ManualResetEvent _allCallbacksGone = new ManualResetEvent(false);
        List<DeviceIndexInfo> deviceIndexList = new List<DeviceIndexInfo>();
        int initializedDeviceIndex = -1;

        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        //[Conditional("DEBUG")]
        private void AllocateConsole()
        {
            AllocConsole();
        }

        //[Conditional("DEBUG")]
        private void DeallocateConsole()
        {
            FreeConsole();
        }

        public Form1()
        {
            InitializeComponent();

            Callback.SelectedIndex = 0;
            Finger_LED.SelectedIndex = 0;
            FingerColor.SelectedIndex = 0;
            Mode_LED.SelectedIndex = 0;
            TestType.SelectedIndex = 0;
            BeepPattern.SelectedIndex = 0;
            StatusLEDColor.SelectedIndex = 0;
            AutoSensitivity.SelectedIndex = 0;
            cbLFDLevel.SelectedIndex = 0;
            RollProfiles.SelectedIndex = 1;
            RollDirections.SelectedIndex = 2;
            SelectTextAlign.SelectedIndex = 0;
            SelectTextColor.SelectedIndex = 0;
            SelectTextSize.SelectedIndex = 0;
            SelectLineColor.SelectedIndex = 0;
            SelectCrossColor.SelectedIndex = 0;
            SelectQuadColor.SelectedIndex = 0;
            KeyMask.SelectedIndex = 0;
            KEYCALLBACK.SelectedIndex = 0;
            OverlayType.SelectedIndex = 0;
            OverlayColor.SelectedIndex = 0;
            CaptureMode.SelectedIndex = 0;//test용
            customCaptX.Text = "0";
            customCaptY.Text = "0";
            customCaptW.Text = "800";
            customCaptH.Text = "750";

            rawCaptureCallback = new CaptureDataCallback(captureDataCallback);
            advCaptureCallback = new AdvCaptureDataCallbackInt(advCaptureDataCallbackInt);

            previewCallback = new RSPreviewDataCallback(previewDataCallback);
            advPreviewCallback = new RSAdvPreviewCallback(advPreviewDataCallback);
            hotplugingCallback = new RSHotPluggingCallback(hotpluggingDataCallback);

            _selectedPrevMode = PrevMode.directDraw;

            _advUiCb = advCaptureCallbackInt;
            _rawUiCb = captureCallbackInt;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            RealScanSDK.RS_ExitDevice(deviceHandle);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            RealScanSDK.RS_ExitDevice(deviceHandle);
        }

        private void InitSDK_Click(object sender, EventArgs e)
        {
            deviceIndexList.Clear();
            int numOfDevice = 0;
            RSSDKInfo sdkInfo = new RSSDKInfo();

            m_result = RealScanSDK.RS_InitSDK(null, 0, ref numOfDevice);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }

            m_result = RealScanSDK.RS_GetSDKInfo(ref sdkInfo);
            if (m_result == RealScanSDK.RS_SUCCESS)
            {
                SDKInfo.Text = System.Text.Encoding.ASCII.GetString(sdkInfo.version);
                SDKInfo.Text += " ";
                SDKInfo.Text += System.Text.Encoding.ASCII.GetString(sdkInfo.buildDate);
            }

            DeviceList.Items.Clear();

            MsgPanel.Text = "The SDK is initialized successfully";

            for (int i = 0; i < numOfDevice; i++)
            {
                int devId = -1;
                int devType = 0;
                int devStatus = 0;
                int devHandle = -1;

                // Get device ID
                if (RealScanSDK.RS_GetDeviceId(i, ref devId) != RealScanSDK.RS_SUCCESS)
                    continue;

                // Get device status and type
                if (RealScanSDK.RS_GetDeviceStatus(devId, ref devHandle, ref devType, ref devStatus) != RealScanSDK.RS_SUCCESS)
                    continue;


                DeviceIndexInfo info = new DeviceIndexInfo(false, devId, devHandle);

                deviceIndexList.Add(info);


                // Compose display name string
                string name = $"{i}) {GetDeviceNameByType(devType)}";
                DeviceList.Items.Add(name);
            }

            if (numOfDevice > 0)
            {
                DeviceList.SelectedIndex = 0;
            }
            InitDevice.Enabled = true;


            RealScanSDK.RS_StartHotPlugging();
            RealScanSDK.RS_RegisterHotPluggingCallback(hotplugingCallback);

        }

        private void initDevice_Click(object sender, EventArgs e)
        {
            RSDeviceInfo deviceInfo = new RSDeviceInfo();


            if (DeviceList.Items.Count == 0 || deviceIndexList.Count == 0)
            {
                MsgPanel.Text = "No device is selected or device list is empty.";
                return;
            }


            int selectedIndex = DeviceList.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= deviceIndexList.Count)
            {
                MsgPanel.Text = "No device is selected or device list is empty.";
                return;
            }


            int deviceIndex = deviceIndexList[selectedIndex].deviceIndex;


            m_result = RealScanSDK.RS_InitDevice(deviceIndex, ref deviceHandle);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }

            AutoCalibrate.Checked = true;
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw) RealScanSDK.RS_StopViewWindow(deviceHandle);
            }
            m_prevStopped = true;

            TimeoutTextBox.Text = Convert.ToString(0);
            ReductionLevel.Text = Convert.ToString(100);

            m_result = RealScanSDK.RS_GetDeviceInfo(deviceHandle, ref deviceInfo);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }
            DeviceIndexInfo info = deviceIndexList[DeviceList.SelectedIndex];
            info.isInit = true;
            info.deviceHandle = deviceHandle;

            deviceIndexList[DeviceList.SelectedIndex] = info;
            initializedDeviceIndex = deviceIndexList[DeviceList.SelectedIndex].deviceIndex;

            DeviceInfo.Text = System.Text.Encoding.ASCII.GetString(deviceInfo.productName);
            DeviceID.Text = System.Text.Encoding.ASCII.GetString(deviceInfo.deviceID);
            FirmwareInfo.Text = System.Text.Encoding.ASCII.GetString(deviceInfo.firmwareVersion);
            FirmwareInfo.Text = System.Text.Encoding.ASCII.GetString(deviceInfo.firmwareVersion);
            Hardwareinfo.Text = System.Text.Encoding.ASCII.GetString(deviceInfo.HardwareVersion);

            InitDevice.Enabled = false;
            ExitDevice.Enabled = true;
            DeviceList.Enabled = false;

            deviceType_ = deviceInfo.deviceType;
            if (deviceInfo.deviceType != RealScanSDK.RS_DEVICE_REALSCAN_F)
            {
                ResetLCD.Enabled = false;
                DisplayLCD.Enabled = false;
            }

            MsgPanel.Text = "The device is initialized successfully";
            
            selMinFingerCount.SelectedIndex = -1;

            SetPreview();

            _allCallbacksGone.Reset();

            reload_ledControl();
        }
        private void reload_ledControl()
        {

            if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_S60)
            {
                groupModeLEDControl.Text = "Mode LED (RS-G10)";
                groupModeLEDControl.Enabled = false;

                groupFingerLEDControl.Text = "Mode LED Color";
                groupFingerLEDControl.Enabled = true;

                Finger_LED.Items.Clear();
                Finger_LED.Items.Add("0. All");
                Finger_LED.Items.Add("1. Left");
                Finger_LED.Items.Add("2. Right");
                Finger_LED.Items.Add("3. Thumb");
                Finger_LED.Items.Add("4. Roll");
                Finger_LED.SelectedIndex = 0; 


                FingerColor.Items.Clear();

                //FingerColor.Items.Add("0. OFF");
                FingerColor.Items.Add("1. RED");
                FingerColor.Items.Add("2. GREEN");
                FingerColor.Items.Add("3. YELLOW");
                FingerColor.Items.Add("4. BLUE");
                FingerColor.Items.Add("5. Magenta");
                FingerColor.Items.Add("6. Cyan");
                FingerColor.Items.Add("7. White");

                FingerColor.SelectedIndex = 0; 



                groupStatusLEDControl.Text = "Status LED(RS-D & RS-S60 & RSSG10)";
                groupStatusLEDControl.Enabled = true;

                StatusLEDOn.Enabled = true;
                StatusLEDOff.Enabled = false;

                StatusLEDColor.Items.Clear();
                StatusLEDColor.Items.Add("0. OFF");
                StatusLEDColor.Items.Add("1. RED");
                StatusLEDColor.Items.Add("2. GREEN");
                StatusLEDColor.Items.Add("3. YELLOW");
                StatusLEDColor.Items.Add("4. BLUE");
                StatusLEDColor.Items.Add("5. Magenta");
                StatusLEDColor.Items.Add("6. Cyan");
                StatusLEDColor.Items.Add("7. White");

                StatusLEDColor.SelectedIndex = 0; 



            }
            else if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10 || deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10I)
            {
                groupModeLEDControl.Text = "Mode LED (RS-G10)";
                groupModeLEDControl.Enabled = false;

                groupFingerLEDControl.Text = "Mode LED Color";
                groupFingerLEDControl.Enabled = true;




                Finger_LED.Items.Clear();
                Finger_LED.Items.Add("0. All");
                Finger_LED.Items.Add("1. Left");
                Finger_LED.Items.Add("2. Right");
                Finger_LED.Items.Add("3. Thumb");
                Finger_LED.Items.Add("4. Roll");
                Finger_LED.SelectedIndex = 0; 



                FingerColor.Items.Clear();

                //FingerColor.Items.Add("0. OFF");
                FingerColor.Items.Add("1. RED");
                FingerColor.Items.Add("2. GREEN");
                FingerColor.Items.Add("3. BLUE");


                FingerColor.SelectedIndex = 0; 



                groupStatusLEDControl.Text = "Status LED(RS-D & RS-S60 & RSSG10)";
                groupStatusLEDControl.Enabled = true;

                StatusLEDOn.Enabled = true;
                StatusLEDOff.Enabled = false;


                StatusLEDColor.Items.Clear();
                StatusLEDColor.Items.Add("0. OFF");
                StatusLEDColor.Items.Add("1. RED");
                StatusLEDColor.Items.Add("2. GREEN");
                StatusLEDColor.Items.Add("3. BLUE");


                StatusLEDColor.SelectedIndex = 0; 
            }
            else
            {
                groupModeLEDControl.Text = "Mode LED (RS-G10)";
                groupModeLEDControl.Enabled = true;

                groupFingerLEDControl.Text = "Finger LED(RS-G10)";
                groupFingerLEDControl.Enabled = true;



                Finger_LED.Items.Clear();
                Finger_LED.Items.Add("0. All");
                Finger_LED.Items.Add("1. Little(L)");
                Finger_LED.Items.Add("2. Ring(L)");
                Finger_LED.Items.Add("3. Middle(L)");
                Finger_LED.Items.Add("4. Index(L)");
                Finger_LED.Items.Add("5. Thumb(L)");
                Finger_LED.Items.Add("6. Thumb(R)");
                Finger_LED.Items.Add("7. Index(R)");
                Finger_LED.Items.Add("8. Middle(R)");
                Finger_LED.Items.Add("9. Ring(R)");
                Finger_LED.Items.Add("10. Little(R)");
                Finger_LED.SelectedIndex = 0;  


                FingerColor.Items.Clear();
                FingerColor.Items.Add("0. GREEN");
                FingerColor.Items.Add("1. RED");
                FingerColor.Items.Add("2. YELLOW");
                FingerColor.SelectedIndex = 0;  

                if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_D)
                {
                    groupStatusLEDControl.Text = "Status LED(RS-D & RS-S60 & RSSG10)";
                    groupStatusLEDControl.Enabled = true;

                    StatusLEDOn.Enabled = true;
                    StatusLEDOff.Enabled = true;

                    StatusLEDColor.Items.Clear();
                    StatusLEDColor.Items.Add("0. GREEN");
                    StatusLEDColor.Items.Add("1. RED");
                    StatusLEDColor.Items.Add("2. YELLOW");

                    StatusLEDColor.SelectedIndex = 0; 

                }
                else
                {
                    groupStatusLEDControl.Text = "Status LED(RS-D & RS-S60 & RSSG10)";
                    StatusLEDOn.Enabled = false;
                    StatusLEDOff.Enabled = false;
                }

            }
        }
        private void exitDevice_Click(object sender, EventArgs e)
        {
            _allCallbacksGone.Reset();

            m_result = RealScanSDK.RS_AbortCapture(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS && m_result != RealScanSDK.RS_ERR_CAPTURE_IS_NOT_RUNNING)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;

                return;
            }


            if (!m_prevStopped)
            {
                switch (_selectedPrevMode)
                {
                    case PrevMode.directDraw:
                        m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
                        if (m_result != RealScanSDK.RS_SUCCESS)
                        {
                            RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                            return;
                        }
                        break;
                    case PrevMode.callbackDraw:
                        m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, null);
                        if (m_result != RealScanSDK.RS_SUCCESS)
                        {
                            RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                            return;
                        }
                        break;
                    case PrevMode.advCallbackDraw:
                        m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, null);
                        if (m_result != RealScanSDK.RS_SUCCESS)
                        {
                            RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                            return;
                        }
                        break;
                    default:
                        m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
                        m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, null);
                        m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, null);
                        break;
                }

                m_prevStopped = true;
            }
            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;

                return;
            }
            m_result = RealScanSDK.RS_ExitDevice(deviceHandle);
            

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }


            for (int i = 0; i < deviceIndexList.Count; i++)
            {
                if (deviceIndexList[i].deviceHandle == deviceHandle)
                {
                    DeviceIndexInfo info = deviceIndexList[i];
                    info.isInit = false;
                    deviceIndexList[i] = info;
                    break;
                }
            }

            deviceHandle = -1;
            initializedDeviceIndex = -1;

            CaptureMode.SelectedIndex = 0;
            Callback.SelectedIndex = 0;

            DeviceInfo.Text = "";
            FirmwareInfo.Text = "";
            DeviceID.Text = "";
            FirmwareInfo.Text = "";
            Hardwareinfo.Text = "";
            ImageSize.Text = "";

            InitSDK.Enabled = true;
            InitDevice.Enabled = true;
            ExitDevice.Enabled = false;
            StartCapture.Enabled = false;
            DeviceList.Enabled = true;
            StopCapture.Enabled = false;
            TakeManual.Enabled = false;
            TakeAuto.Enabled = false;
            ResetLCD.Enabled = false;
            DisplayLCD.Enabled = false;

            ResetLCD.Enabled = true;
            DisplayLCD.Enabled = true;

            MsgPanel.Text = "The device is disconnected successfully";
        }

        private void CaptureMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] tmpCaptureModes = new int[19] {RealScanSDK.RS_CAPTURE_DISABLED,RealScanSDK.RS_CAPTURE_ROLL_FINGER,RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER,
                                                RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS, RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS,
                                                RealScanSDK.RS_CAPTURE_FLAT_LEFT_PALM, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_PALM, RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX,
                                                RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX, RealScanSDK.RS_CAPTURE_FLAT_LEFT_SIDE_PALM, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_SIDE_PALM,
                                                RealScanSDK.RS_CAPTURE_FLAT_LEFT_WRITERS_PALM, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM,
                                                RealScanSDK.RS_CAPTURE_FLAT_LEFT_UPPER_PALM, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_UPPER_PALM,
                                                RealScanSDK.RS_CAPTURE_FLAT_LEFT_LOWER_PALM, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_LOWER_PALM, RealScanSDK.RS_CAPTURE_FLAT_MANUAL};

            for (int i = 0; i < 19; i++)
            {
                if (i == CaptureMode.SelectedIndex)
                {
                    m_captureMode = tmpCaptureModes[i];
                    break;
                }
            }

            if (m_captureMode == RealScanSDK.RS_CAPTURE_DISABLED)
            {
                return;
            }
            else if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_MANUAL)
            {
                this.Update();

                if (customCaptX.Text == "" || customCaptY.Text == "" || customCaptW.Text == "" || customCaptH.Text == "")
                {
                    MessageBox.Show("Please set all the text boxes filled");
                    m_bCaptureModeSelected = false;
                    return;
                }

                m_nCustomCaptX = Convert.ToInt32(customCaptX.Text);
                m_nCustomCaptY = Convert.ToInt32(customCaptY.Text);
                m_nCustomCaptW = Convert.ToInt32(customCaptW.Text);
                m_nCustomCaptH = Convert.ToInt32(customCaptH.Text);

                m_result = RealScanSDK.RS_SetManualCaptureMode(deviceHandle, m_nCustomCaptX, m_nCustomCaptY, m_nCustomCaptW, m_nCustomCaptH, 0, true);

                if ( m_result == RealScanSDK.RS_ERR_INVALID_PARAM )
                {
                    MessageBox.Show("Please insert correct capture position,size");
                    m_bCaptureModeSelected = false;
                    return;
                }
                m_slapType = RealScanSDK.RS_SLAP_MANUAL;
            }
            else
            {
                switch (m_captureMode)
                {
                    case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER:
                    case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX:
                        m_slapType = RealScanSDK.RS_SLAP_ONE_FINGER;
                        m_fingerCount = 1;
                        break;

                    case RealScanSDK.RS_CAPTURE_ROLL_FINGER:
                    case RealScanSDK.RS_CAPTURE_ROLL_FINGER_EX:
                        m_slapType = RealScanSDK.RS_SLAP_ONE_FINGER_ROLL;
                        m_fingerCount = 1;
                        break;

                    case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                    case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                        m_slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                        m_fingerCount = 2;
                        break;

                    case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                        m_slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                        m_fingerCount = 4;
                        break;

                    case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
                        m_slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                        m_fingerCount = 4;
                        break;
                    
                    default:
                        break;
                }

                int[] nCaptDir = new int[3] { RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT, RealScanSDK.RS_CAPTURE_DIRECTION_LEFT, RealScanSDK.RS_CAPTURE_DIRECTION_RIGHT };

                m_result = RealScanSDK.RS_SetCaptureModeWithDir(deviceHandle, m_captureMode, nCaptDir[m_captureDir], 0, true);
            }

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                CaptureMode.SelectedIndex = 0;
                m_bCaptureModeSelected = false;
                return;
            }

            ImageSize.Text = "";

            if (CaptureMode.SelectedIndex == 0) return;
            /*
            else if (CaptureMode.SelectedIndex == RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS)
            {
                for (int i = 0; i < 4; i++)
                {
                    leftFingers.SetItemCheckState(i, CheckState.Unchecked);
                    rightFingers.SetItemCheckState(i, CheckState.Indeterminate);
                }
            }
            else if (CaptureMode.SelectedIndex == RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS)
            {
                for (int i = 0; i < 4; i++)
                {
                    rightFingers.SetItemCheckState(i, CheckState.Unchecked);
                    leftFingers.SetItemCheckState(i, CheckState.Indeterminate);
                }
            }
            */

            int imageWidth = 0;
            int imageHeight = 0;

            m_result = RealScanSDK.RS_GetImageSize(deviceHandle, ref imageWidth, ref imageHeight);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                m_bCaptureModeSelected = false;
                return;
            }

            m_bCaptureModeSelected = true;
            ImageSize.Text = imageWidth.ToString() + "x" + imageHeight.ToString();
            StartCapture.Enabled = true;
            TakeAuto.Enabled = true;
        }

        private void previewCallbackInt(int errorCode, byte[] imageData, int imageWidth, int imageHeight)
        {
           
            int nWidth = imageWidth;
            int nHeight = imageHeight;
            int nPitch = nWidth % 4;

            if (nWidth % 4 != 0) nWidth -= nWidth % 4;

            byte[] bData = new byte[nWidth * nHeight];

            for (int i = 0; i < nHeight; i++)
            {
                for (int j = 0; j < nWidth; j++)
                    bData[i * nWidth + j] = imageData[i * imageWidth + j];
            }

            Bitmap Canvas = new Bitmap(nWidth, nHeight, PixelFormat.Format8bppIndexed);
            BitmapData CanvasData = Canvas.LockBits(new Rectangle(0, 0, nWidth, nHeight),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            Marshal.Copy(bData, 0, CanvasData.Scan0, bData.Length);

            Canvas.UnlockBits(CanvasData);

            ColorPalette grayscalePalette = Canvas.Palette;
            for (int i = 0; i < 256; i++)
                grayscalePalette.Entries[i] = Color.FromArgb(i, i, i);
            Canvas.Palette = grayscalePalette;

            PreviewWindow.Image = Canvas;

            
        }
        
        private void advPreviewCallbackInt(int errorCode, byte[] imageData, int imageWidth, int imageHeight, int quality, int status)
        {
            int nWidth = imageWidth;
            int nHeight = imageHeight;
            int nPitch = nWidth % 4;

            if (nWidth % 4 != 0) nWidth -= nWidth % 4;

            byte[] bData = new byte[nWidth * nHeight];

            for (int i = 0; i < nHeight; i++)
            {
                for (int j = 0; j < nWidth; j++)
                    bData[i * nWidth + j] = imageData[i * imageWidth + j];
            }

            Bitmap Canvas = new Bitmap(nWidth, nHeight, PixelFormat.Format8bppIndexed);
            BitmapData CanvasData = Canvas.LockBits(new Rectangle(0, 0, nWidth, nHeight),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            Marshal.Copy(bData, 0, CanvasData.Scan0, bData.Length);

            Canvas.UnlockBits(CanvasData);

            ColorPalette grayscalePalette = Canvas.Palette;
            for (int i = 0; i < 256; i++)
                grayscalePalette.Entries[i] = Color.FromArgb(i, i, i);
            Canvas.Palette = grayscalePalette;

            PreviewWindow.Image = Canvas;

            string statusMsg = " ";
            RealScanSDK.RS_GetCaptureStatusString(status, ref statusMsg);
            captureGuideMsg.Text = statusMsg;

        }

        private void OnHotPlugging(int deviceId, bool isConnected)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnHotPlugging(deviceId, isConnected)));
                return;
            }

            int numOfDevice = 0;
            RealScanSDK.RS_GetNumOfDevice(ref numOfDevice);

            if (isConnected)
            {
                DeviceList.Items.Clear();

                for (int i = 0; i < numOfDevice; ++i)
                {
                    int id = -1;
                    int devType = 0;
                    int devStatus = 0;
                    int handle = -1;
                    int status = -1;

                    int result = RealScanSDK.RS_GetDeviceId(i, ref id);
                    if (result != RealScanSDK.RS_SUCCESS) continue;

                    result = RealScanSDK.RS_GetDeviceStatus(id, ref handle, ref devType, ref status);
                    if (result != RealScanSDK.RS_SUCCESS) continue;

                    int index = deviceIndexList.FindIndex(item => item.deviceIndex == id);

                    if (index == -1)
                    {
                        DeviceIndexInfo info = new DeviceIndexInfo(false, id, handle);

                        if (handle != -1)
                        {
                            deviceHandle = handle;
                            info.isInit = true;
                            info.deviceHandle = handle;
                            initializedDeviceIndex = id;
                        }
                        deviceIndexList.Add(info);
                    }
                }

                for (int i = 0; i < deviceIndexList.Count; ++i)
                {
                    int devHandle = -1, devType = 0, devStatus = 0;
                    int retStatus = RealScanSDK.RS_GetDeviceStatus(deviceIndexList[i].deviceIndex, ref devHandle, ref devType, ref devStatus);
                    if (retStatus != RealScanSDK.RS_SUCCESS) continue;

                    string name = $"{i}) {GetDeviceNameByType(devType)}";
                    DeviceList.Items.Add(name);
                }

                int tmpIdx = -1;
                for (int i = 0; i < deviceIndexList.Count; ++i)
                {
                    if (deviceIndexList[i].isInit)
                    {
                        tmpIdx = i;
                        break;
                    }
                }

                if (tmpIdx == -1)
                {
                    if (numOfDevice > 0)
                    {
                        DeviceList.SelectedIndex = 0;
                        DeviceList.Enabled = true;
                    }
                    else
                    {
                        DeviceList.Enabled = false;
                    }
                }
                else
                {
                    if (numOfDevice > 0)
                    {
                        DeviceList.SelectedIndex = tmpIdx;
                        DeviceList.Enabled = false;
                    }

                    int result = RealScanSDK.RS_GetDeviceHandle(deviceId, ref deviceHandle);

                    if (result == RealScanSDK.RS_SUCCESS)
                    {
                        InitDevice.Enabled = false;
                        ExitDevice.Enabled = true;
                        StartCapture.Enabled = true;
                        TakeAuto.Enabled = true;

                        Thread infoThread = new Thread(() =>
                        {
                            Thread.Sleep(200);
                            RSDeviceInfo deviceInfo = new RSDeviceInfo();
                            int infoResult = RealScanSDK.RS_GetDeviceInfo(deviceHandle, ref deviceInfo);

                            Invoke(new Action(() =>
                            {
                                if (infoResult != RealScanSDK.RS_SUCCESS)
                                {
                                    RealScanSDK.RS_GetErrString(infoResult, ref m_errorMsg);
                                    MsgPanel.Text = m_errorMsg;
                                }
                                else
                                {
                                    DeviceInfo.Text = Encoding.ASCII.GetString(deviceInfo.productName).TrimEnd('\0');
                                    DeviceID.Text = Encoding.ASCII.GetString(deviceInfo.deviceID).TrimEnd('\0');
                                    FirmwareInfo.Text = Encoding.ASCII.GetString(deviceInfo.firmwareVersion).TrimEnd('\0');
                                    Hardwareinfo.Text = Encoding.ASCII.GetString(deviceInfo.HardwareVersion).TrimEnd('\0');

                                    SetPreview();

                                    reload_ledControl();
                                }
                            }));
                        });
                        infoThread.IsBackground = true;
                        infoThread.Start();
                    }
                }
            }
            else
            {
                int remIdx = deviceIndexList.FindIndex(d => d.deviceIndex == deviceId);

                int removedHandle = -1;
                bool wasCurrent = false;

                if (remIdx >= 0)
                {
                    var info = deviceIndexList[remIdx]; 
                    removedHandle = info.deviceHandle;
                    wasCurrent = (info.deviceIndex == initializedDeviceIndex) || (removedHandle == deviceHandle);
                }


               
                int removed = deviceIndexList.RemoveAll(d => d.deviceIndex == deviceId);


                if (wasCurrent)
                {
                   
                   

                    DeviceList.Enabled = true;
                    InitDevice.Enabled = true;
                    ExitDevice.Enabled = false;
                    StartCapture.Enabled = false;
                    StopCapture.Enabled = false;
                    TakeManual.Enabled = false;
                    TakeAuto.Enabled = false;

                    deviceHandle = -1;
                    initializedDeviceIndex = -1;

                    DeviceInfo.Text = "";
                    FirmwareInfo.Text = "";
                    DeviceID.Text = "";
                    Hardwareinfo.Text = "";
                    ImageSize.Text = "";

                    DeviceList.BeginUpdate();
                    DeviceList.Items.Clear();
                    for (int i = 0; i < deviceIndexList.Count; ++i)
                    {
                        int h = -1, t = 0, s = 0;
                       
                        if (RealScanSDK.RS_GetDeviceStatus(deviceIndexList[i].deviceIndex, ref h, ref t, ref s) != RealScanSDK.RS_SUCCESS)
                            t = 0; 
                        DeviceList.Items.Add($"{i}) {GetDeviceNameByType(t)}");
                    }
                    DeviceList.EndUpdate();

                    DeviceList.SelectedIndex = (DeviceList.Items.Count > 0 ? 0 : -1);
                    if (DeviceList.Items.Count == 0) DeviceList.Enabled = false;
                }
                else
                {

                    if (remIdx >= 0 && remIdx < DeviceList.Items.Count)
                    {
                        DeviceList.BeginUpdate();
                        try
                        {
                            DeviceList.Items.RemoveAt(remIdx);
                           
                            if (DeviceList.SelectedIndex >= DeviceList.Items.Count)
                                DeviceList.SelectedIndex = (DeviceList.Items.Count > 0 ? DeviceList.Items.Count - 1 : -1);
                        }
                        finally
                        {
                            DeviceList.EndUpdate();
                        }
                    }
                   
                }
            }
        }
    




        private string GetDeviceNameByType(int deviceType)
        {
            switch (deviceType)
            {
                case RealScanSDK.RS_DEVICE_REALSCAN_10:
                    return "RealScan-10";
                case RealScanSDK.RS_DEVICE_REALSCAN_10F:
                    return "RealScan-10F";
                case RealScanSDK.RS_DEVICE_REALSCAN_D:
                    return "RealScan-D";
                case RealScanSDK.RS_DEVICE_REALSCAN_DF:
                    return "RealScan-DF";
                case RealScanSDK.RS_DEVICE_REALSCAN_F:
                    return "RealScan-F";
                case RealScanSDK.RS_DEVICE_REALSCAN_F_C:
                    return "RealScan-FC";
                case RealScanSDK.RS_DEVICE_REALSCAN_F_C_IBIOS:
                    return "IBIOS P10iD-2";
                case RealScanSDK.RS_DEVICE_REALSCAN_F_C_V02:
                    return "RealScan-FC";
                case RealScanSDK.RS_DEVICE_REALSCAN_G10:
                    return "RealScan-G10";
                case RealScanSDK.RS_DEVICE_REALSCAN_G10F:
                    return "RealScan-G10F";
                case RealScanSDK.RS_DEVICE_REALSCAN_G1:
                    return "RealScan-G1";
                case RealScanSDK.RS_DEVICE_REALSCAN_S60:
                    return "RealScan S60";
                case RealScanSDK.RS_DEVICE_REALSCAN_G10I:
                    return "RealScan-G10i";
                case RealScanSDK.RS_DEVICE_REALSCAN_G10_IBIOS:
                    return "IBIOS S10iD-2";
                case RealScanSDK.RS_DEVICE_REALSCAN_SG10:
                    return "RealScan SG10";
                case RealScanSDK.RS_DEVICE_REALSCAN_SG10I:
                    return "RealScan SG10i";
                default:
                    return "Unknown Device";
            }
        }


        private void hotpluggingDataCallback(int deviceId, bool isConnected)
        {
            RSHotPluggingCallback callback = new RSHotPluggingCallback(OnHotPlugging);
            Invoke(callback, deviceId, isConnected);
        }

        private int GetSlapType(int nCaptureMode, ref int pnSlapType)
        {
            switch (nCaptureMode)
            {
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                    pnSlapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER:
                case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX:
                    pnSlapType = RealScanSDK.RS_SLAP_ONE_FINGER;
                    break;
                case RealScanSDK.RS_CAPTURE_ROLL_FINGER:
                case RealScanSDK.RS_CAPTURE_ROLL_FINGER_EX:
                    pnSlapType = RealScanSDK.RS_SLAP_ONE_FINGER_ROLL;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                    pnSlapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
                    pnSlapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                    break;
                default:
                    MsgPanel.Text = "Cannot segment in this mode";
                    return - 1;
            }
            return 0;
        }

        private void captureCallbackInt(int errorCode, IntPtr safeimageData, int imageWidth, int imageHeight)
        {
            int nRetVal = RealScanSDK.RS_ERR_UNKNOWN;
            m_result = errorCode;
            if (errorCode != RealScanSDK.RS_SUCCESS && errorCode != RealScanSDK.RS_WRN_TOO_POOR_QUALITY && errorCode != RealScanSDK.RS_WRN_BAD_SCAN
                && errorCode != RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER && errorCode != RealScanSDK.RS_ERR_SEGMENT_WRONG_HAND)
            {
                RealScanSDK.RS_GetErrString(errorCode, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                StartCapture.Enabled = true;
                StopCapture.Enabled = false;
                TakeManual.Enabled = false;
                TakeAuto.Enabled = true;
                return;
            }

            int nLFDLevel = 0;
            RealScanSDK.RS_GetLFDLevel(deviceHandle, ref nLFDLevel);
            if (nLFDLevel == 0)
            {
                MsgPanel.Text = "An image is captured successfully";
            }
            else
            {
                RSLFDResult sLFDResult = new RSLFDResult();
                nRetVal = RealScanSDK.RS_GetLFDResult(deviceHandle, ref sLFDResult);
                if (nRetVal != RealScanSDK.RS_SUCCESS)
                {
                    MsgPanel.Text = "An image is captured successfully";
                }
                else
                {
                    int nFinalLFDResult = RealScanSDK.RS_LFD_LIVE;
                    String strScores = "";
                    for (int i = 0; i < sLFDResult.nNumofFinger; i++)
                    {
                        String strScore = "";
                        if (sLFDResult.arsLFDInfo[i].nResult == RealScanSDK.RS_LFD_FAKE) nFinalLFDResult = RealScanSDK.RS_LFD_FAKE;
                        strScore = String.Format("{0}({1}) ", sLFDResult.arsLFDInfo[i].nResult == RealScanSDK.RS_LFD_LIVE ? "L" : "F",
                            sLFDResult.arsLFDInfo[i].nScore);
                        strScores += strScore;
                    }
                    MsgPanel.Text = String.Format("{0} {1}", nFinalLFDResult == RealScanSDK.RS_LFD_LIVE ? "[LIVE FINGER]" : "[FAKE FINGER]", strScores);
                }
            }

            SaveSeqCheckTargetProcess(safeimageData, imageWidth, imageHeight);

            if (Callback.SelectedIndex == (int)callbackMode.save)
            {
                SaveImageCaptureProcess(safeimageData, imageWidth, imageHeight);
            }
            else if (Callback.SelectedIndex == (int)callbackMode.save19794_4_2011)
            {
                SaveImage19794_4_2011_CaptureProcess(safeimageData, imageWidth, imageHeight);
            }
           
            else if (Callback.SelectedIndex == (int)callbackMode.enlarge)
            {
                EnlargeCaptureProcess(safeimageData, imageWidth, imageHeight);
            }
            else if ((Callback.SelectedIndex == (int)callbackMode.segment || Callback.SelectedIndex == (int)callbackMode.saveNseg))
            {
                int nMinFinger = 4;
                nRetVal = RealScanSDK.RS_GetMinimumFinger(deviceHandle, ref nMinFinger);
                if (nMinFinger == 4)
                {
                    int numOfFingers = 0;
                    IntPtr[] ImageBuffer = new IntPtr[4];
                    int[] ImageWidth = new int[4];
                    int[] ImageHeight = new int[4];
                    RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

                    SegmentCaptureProcess(safeimageData, imageWidth, imageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight);

                    if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                    {
                        SegmentSaveImageCaptureProcess(safeimageData, imageWidth, imageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (ImageBuffer[i] != (IntPtr)0)
                        {
                            RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                        }
                    }
                }
                else
                {
                    int nCaptureMode = 0;
                    int nCaptureOption = 0;
                    nRetVal = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref nCaptureMode, ref nCaptureOption);
                    if (nRetVal != RealScanSDK.RS_SUCCESS) return;

                    int nSlapType = 0;
                    nRetVal = GetSlapType(nCaptureMode, ref nSlapType);
                    if (nRetVal == RealScanSDK.RS_SUCCESS)
                    {
                        RSSegmentInfo sSegmentInfo = new RSSegmentInfo();
                        sSegmentInfo.arsFingerInfo = new RSFingerInfo[4];
                        for (int i = 0; i < 4; i++)
                        {
                            sSegmentInfo.arsFingerInfo[i].arsPoint = new RSPoint[4];
                            sSegmentInfo.arsFingerInfo[i].nWidth = 1000;
                            sSegmentInfo.arsFingerInfo[i].nHeight = 1000;
                        }

                        byte[] pbyFinImg1 = new byte[1000 * 1000];
                        byte[] pbyFinImg2 = new byte[1000 * 1000];
                        byte[] pbyFinImg3 = new byte[1000 * 1000];
                        byte[] pbyFinImg4 = new byte[1000 * 1000];

                        RSMissingInfo sMissingInfo = new RSMissingInfo();
                        if (nSlapType == RealScanSDK.RS_SLAP_LEFT_FOUR)
                        {
                            if (leftFingers.GetItemChecked(0)) sMissingInfo.nFirstfinger = 1;
                            if (leftFingers.GetItemChecked(1)) sMissingInfo.nSecondfinger = 1;
                            if (leftFingers.GetItemChecked(2)) sMissingInfo.nThirdfinger = 1;
                            if (leftFingers.GetItemChecked(3)) sMissingInfo.nFourthfinger = 1;
                        }
                        else
                        {
                            if (rightFingers.GetItemChecked(3)) sMissingInfo.nFirstfinger = 1; // Index
                            if (rightFingers.GetItemChecked(2)) sMissingInfo.nSecondfinger = 1; // Middle
                            if (rightFingers.GetItemChecked(1)) sMissingInfo.nThirdfinger = 1; // Ring
                            if (rightFingers.GetItemChecked(0)) sMissingInfo.nFourthfinger = 1; // Little
                        }
                        nRetVal = RealScanSDK.RS_SegmentExMissingFinger(safeimageData, imageWidth, imageHeight, nSlapType, ref sSegmentInfo,
                        pbyFinImg1, pbyFinImg2, pbyFinImg3, pbyFinImg4, sMissingInfo);







                        if (nRetVal == RealScanSDK.RS_SUCCESS || (nRetVal == RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER && sSegmentInfo.nFingerCnt >= nMinFinger))
                        {
                            MsgPanel.Text = "Success Quality:";
                            for (int i = 0; i < sSegmentInfo.nFingerCnt; i++)
                            {
                                if (nRetVal == RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER)
                                {
                                    MsgPanel.Text += "[" + sSegmentInfo.arsFingerInfo[i].nImageQuality + "] ";
                                }
                                else
                                {
                                    MsgPanel.Text += "[" + sSegmentInfo.arsFingerInfo[i].nFingerType + ":" + sSegmentInfo.arsFingerInfo[i].nImageQuality + "] ";
                                }


                                DrawSegmentBox(
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[0].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[0].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[1].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[1].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[3].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[3].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[2].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[2].y,
                                    imageWidth, imageHeight);
                            }

                            if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                            {
                                RSSlapInfoArray slapInfoA = new RSSlapInfoArray();
                                slapInfoA.RSSlapInfoA = new RSSlapInfo[4];
                                IntPtr[] ImageBuffer = new IntPtr[4];
                                int[] ImageWidth = new int[4];
                                int[] ImageHeight = new int[4];

                                slapInfoA.RSSlapInfoA[0].fingerType = sSegmentInfo.arsFingerInfo[0].nFingerType;
                                slapInfoA.RSSlapInfoA[1].fingerType = sSegmentInfo.arsFingerInfo[1].nFingerType;
                                slapInfoA.RSSlapInfoA[2].fingerType = sSegmentInfo.arsFingerInfo[2].nFingerType;
                                slapInfoA.RSSlapInfoA[3].fingerType = sSegmentInfo.arsFingerInfo[3].nFingerType;


                                if (sSegmentInfo.arsFingerInfo[0].nWidth != 0 && sSegmentInfo.arsFingerInfo[0].nHeight != 0)
                                {
                                    ImageBuffer[0] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[0].nWidth * sSegmentInfo.arsFingerInfo[0].nHeight);
                                    Marshal.Copy(pbyFinImg1, 0, ImageBuffer[0], sSegmentInfo.arsFingerInfo[0].nWidth * sSegmentInfo.arsFingerInfo[0].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[1].nWidth != 0 && sSegmentInfo.arsFingerInfo[1].nHeight != 0)
                                {
                                    ImageBuffer[1] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[1].nWidth * sSegmentInfo.arsFingerInfo[1].nHeight);
                                    Marshal.Copy(pbyFinImg2, 0, ImageBuffer[1], sSegmentInfo.arsFingerInfo[1].nWidth * sSegmentInfo.arsFingerInfo[1].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[2].nWidth != 0 && sSegmentInfo.arsFingerInfo[2].nHeight != 0)
                                {
                                    ImageBuffer[2] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[2].nWidth * sSegmentInfo.arsFingerInfo[2].nHeight);
                                    Marshal.Copy(pbyFinImg3, 0, ImageBuffer[2], sSegmentInfo.arsFingerInfo[2].nWidth * sSegmentInfo.arsFingerInfo[2].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[3].nWidth != 0 && sSegmentInfo.arsFingerInfo[3].nHeight != 0)
                                {
                                    ImageBuffer[3] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[3].nWidth * sSegmentInfo.arsFingerInfo[3].nHeight);
                                    Marshal.Copy(pbyFinImg4, 0, ImageBuffer[3], sSegmentInfo.arsFingerInfo[3].nWidth * sSegmentInfo.arsFingerInfo[3].nHeight);
                                }

                                ImageWidth[0] = sSegmentInfo.arsFingerInfo[0].nWidth;
                                ImageWidth[1] = sSegmentInfo.arsFingerInfo[1].nWidth;
                                ImageWidth[2] = sSegmentInfo.arsFingerInfo[2].nWidth;
                                ImageWidth[3] = sSegmentInfo.arsFingerInfo[3].nWidth;

                                ImageHeight[0] = sSegmentInfo.arsFingerInfo[0].nHeight;
                                ImageHeight[1] = sSegmentInfo.arsFingerInfo[1].nHeight;
                                ImageHeight[2] = sSegmentInfo.arsFingerInfo[2].nHeight;
                                ImageHeight[3] = sSegmentInfo.arsFingerInfo[3].nHeight;

                                SegmentSaveImageCaptureProcess(safeimageData, imageWidth, imageHeight, sSegmentInfo.nFingerCnt, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);

                                Marshal.FreeHGlobal(ImageBuffer[0]);
                                Marshal.FreeHGlobal(ImageBuffer[1]);
                                Marshal.FreeHGlobal(ImageBuffer[2]);
                                Marshal.FreeHGlobal(ImageBuffer[3]);

                            }
                        }
                        else
                        {
                            RealScanSDK.RS_GetErrString(nRetVal, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                        }
                    }
                }
            }
            else if (Callback.SelectedIndex == (int)callbackMode.seqCheck)
            {
                SequenceCheckProcess(safeimageData, imageWidth, imageHeight);
            }

            Marshal.FreeHGlobal(safeimageData);

            StartCapture.Enabled = true;
            StopCapture.Enabled = false;
            TakeManual.Enabled = false;
            TakeAuto.Enabled = true;
        }

        private void DrawSegmentBox(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4, int imageWidth, int imageHeight)
        {
            int overlayHandle = -1;


            RealScan.RSOverlayQuadrangle quad = new RSOverlayQuadrangle();
            quad.pos = new RSPoint[4];
            quad.color = 0x00ff0000;

            RSRect rect = new RSRect();
            RealScanSDK.GetClientRect(PreviewWindow.Handle, ref rect);

            quad.pos[0].x = x1 * rect.right / imageWidth;
            quad.pos[0].y = y1 * rect.bottom / imageHeight;
            quad.pos[1].x = x2 * rect.right / imageWidth;
            quad.pos[1].y = y2 * rect.bottom / imageHeight;
            quad.pos[2].x = x3 * rect.right / imageWidth;
            quad.pos[2].y = y3 * rect.bottom / imageHeight;
            quad.pos[3].x = x4 * rect.right / imageWidth;
            quad.pos[3].y = y4 * rect.bottom / imageHeight;

            m_result = RealScanSDK.RS_AddOverlayQuadrangle(deviceHandle, ref quad, ref overlayHandle);
            m_result = RealScanSDK.RS_ShowOverlay(overlayHandle, true);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                MsgPanel.Text = "Cannot overlay for quadrangle" + m_result;
                return;
            }

        }

        private void advCaptureCallbackInt(int errorCode, IntPtr safeimageData, int imageWidth, int imageHeight, int score)
        {
            int nRetVal = RealScanSDK.RS_ERR_UNKNOWN;
            m_result = errorCode;
            if (errorCode != RealScanSDK.RS_SUCCESS && errorCode != RealScanSDK.RS_WRN_TOO_POOR_QUALITY && errorCode != RealScanSDK.RS_WRN_BAD_SCAN
                && errorCode != RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER && errorCode != RealScanSDK.RS_ERR_SEGMENT_WRONG_HAND)
            {
                RealScanSDK.RS_GetErrString(errorCode, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                StartCapture.Enabled = true;
                StopCapture.Enabled = false;
                TakeManual.Enabled = false;
                TakeAuto.Enabled = true;
                return;
            }

            int nLFDLevel = 0;
            RealScanSDK.RS_GetLFDLevel(deviceHandle, ref nLFDLevel);
            if (nLFDLevel == 0)
            {
                MsgPanel.Text = "An image is captured successfully";
            }
            else
            {
                RSLFDResult sLFDResult = new RSLFDResult();
                nRetVal = RealScanSDK.RS_GetLFDResult(deviceHandle, ref sLFDResult);
                if (nRetVal != RealScanSDK.RS_SUCCESS)
                {
                    MsgPanel.Text = "An image is captured successfully";
                }
                else
                {
                    int nFinalLFDResult = RealScanSDK.RS_LFD_LIVE;
                    String strScores = "";
                    for (int i = 0; i < sLFDResult.nNumofFinger; i++)
                    {
                        String strScore = "";
                        if (sLFDResult.arsLFDInfo[i].nResult == RealScanSDK.RS_LFD_FAKE) nFinalLFDResult = RealScanSDK.RS_LFD_FAKE;
                        strScore = String.Format("{0}({1}) ", sLFDResult.arsLFDInfo[i].nResult == RealScanSDK.RS_LFD_LIVE ? "L" : "F",
                            sLFDResult.arsLFDInfo[i].nScore);
                        strScores += strScore;
                    }
                    MsgPanel.Text = String.Format("{0} {1}", nFinalLFDResult == RealScanSDK.RS_LFD_LIVE ? "[LIVE FINGER]" : "[FAKE FINGER]", strScores);
                }

            }

            SaveSeqCheckTargetProcess(safeimageData, imageWidth, imageHeight);

            if (Callback.SelectedIndex == (int)callbackMode.save)
            {
                SaveImageCaptureProcess(safeimageData, imageWidth, imageHeight);
            }
            else if (Callback.SelectedIndex == (int)callbackMode.save19794_4_2011)
            {
                SaveImage19794_4_2011_CaptureProcess(safeimageData, imageWidth, imageHeight);
            }
          
            else if (Callback.SelectedIndex == (int)callbackMode.enlarge)
            {
                EnlargeCaptureProcess(safeimageData, imageWidth, imageHeight);
            }
            else if ((Callback.SelectedIndex == (int)callbackMode.segment || Callback.SelectedIndex == (int)callbackMode.saveNseg))
            {
                int nMinFinger = 4;
                nRetVal = RealScanSDK.RS_GetMinimumFinger(deviceHandle, ref nMinFinger);
                if (nMinFinger == 4)
                {
                    int numOfFingers = 0;
                    IntPtr[] ImageBuffer = new IntPtr[4];
                    int[] ImageWidth = new int[4];
                    int[] ImageHeight = new int[4];
                    RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

                    SegmentCaptureProcess(safeimageData, imageWidth, imageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight);

                    if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                    {
                        SegmentSaveImageCaptureProcess(safeimageData, imageWidth, imageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (ImageBuffer[i] != (IntPtr)0)
                        {
                            RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                        }
                    }
                }
                else
                {
                    int nCaptureMode = 0;
                    int nCaptureOption = 0;
                    nRetVal = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref nCaptureMode, ref nCaptureOption);
                    if (nRetVal != RealScanSDK.RS_SUCCESS) return;

                    int nSlapType = 0;
                    nRetVal = GetSlapType(nCaptureMode, ref nSlapType);
                    if (nRetVal == RealScanSDK.RS_SUCCESS)
                    {
                       RSSegmentInfo sSegmentInfo = new RSSegmentInfo();
                        sSegmentInfo.arsFingerInfo = new RSFingerInfo[4];
                        for (int i = 0; i < 4; i++)
                        {
                            sSegmentInfo.arsFingerInfo[i].arsPoint = new RSPoint[4];
                            sSegmentInfo.arsFingerInfo[i].nWidth = 1000;
                            sSegmentInfo.arsFingerInfo[i].nHeight = 1000;
                        }

                        byte[] pbyFinImg1 = new byte[1000 * 1000];
                        byte[] pbyFinImg2 = new byte[1000 * 1000];
                        byte[] pbyFinImg3 = new byte[1000 * 1000];
                        byte[] pbyFinImg4 = new byte[1000 * 1000];

                        RSMissingInfo sMissingInfo = new RSMissingInfo();
                        if (nSlapType == RealScanSDK.RS_SLAP_LEFT_FOUR)
                        {
                            if (leftFingers.GetItemChecked(0)) sMissingInfo.nFirstfinger = 1;
                            if (leftFingers.GetItemChecked(1)) sMissingInfo.nSecondfinger = 1;
                            if (leftFingers.GetItemChecked(2)) sMissingInfo.nThirdfinger = 1;
                            if (leftFingers.GetItemChecked(3)) sMissingInfo.nFourthfinger = 1;
                        }
                        else
                        {
                            if (rightFingers.GetItemChecked(3)) sMissingInfo.nFirstfinger = 1; // Index
                            if (rightFingers.GetItemChecked(2)) sMissingInfo.nSecondfinger = 1; // Middle
                            if (rightFingers.GetItemChecked(1)) sMissingInfo.nThirdfinger = 1; // Ring
                            if (rightFingers.GetItemChecked(0)) sMissingInfo.nFourthfinger = 1; // Little
                        }
                        nRetVal = RealScanSDK.RS_SegmentExMissingFinger(safeimageData, imageWidth, imageHeight, nSlapType, ref sSegmentInfo,
                        pbyFinImg1, pbyFinImg2, pbyFinImg3, pbyFinImg4, sMissingInfo);


                        if (nRetVal == RealScanSDK.RS_SUCCESS || (nRetVal == RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER && sSegmentInfo.nFingerCnt >= nMinFinger))
                        {
                            MsgPanel.Text = "Success Quality:";
                            for (int i = 0; i < sSegmentInfo.nFingerCnt; i++)
                            {
                                if (nRetVal == RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER)
                                {
                                    MsgPanel.Text += "[" + sSegmentInfo.arsFingerInfo[i].nImageQuality + "] ";
                                }
                                else
                                {
                                    MsgPanel.Text += "[" + sSegmentInfo.arsFingerInfo[i].nFingerType + ":" + sSegmentInfo.arsFingerInfo[i].nImageQuality + "] ";
                                }


                                DrawSegmentBox(
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[0].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[0].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[1].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[1].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[3].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[3].y,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[2].x,
                                    sSegmentInfo.arsFingerInfo[i].arsPoint[2].y,
                                    imageWidth, imageHeight);
                            }

                            if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                            {
                                RSSlapInfoArray slapInfoA = new RSSlapInfoArray();
                                slapInfoA.RSSlapInfoA = new RSSlapInfo[4];
                                IntPtr[] ImageBuffer = new IntPtr[4];
                                int[] ImageWidth = new int[4];
                                int[] ImageHeight = new int[4];

                                slapInfoA.RSSlapInfoA[0].fingerType = sSegmentInfo.arsFingerInfo[0].nFingerType;
                                slapInfoA.RSSlapInfoA[1].fingerType = sSegmentInfo.arsFingerInfo[1].nFingerType;
                                slapInfoA.RSSlapInfoA[2].fingerType = sSegmentInfo.arsFingerInfo[2].nFingerType;
                                slapInfoA.RSSlapInfoA[3].fingerType = sSegmentInfo.arsFingerInfo[3].nFingerType;


                                if (sSegmentInfo.arsFingerInfo[0].nWidth != 0 && sSegmentInfo.arsFingerInfo[0].nHeight != 0)
                                {
                                    ImageBuffer[0] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[0].nWidth * sSegmentInfo.arsFingerInfo[0].nHeight);
                                    Marshal.Copy(pbyFinImg1, 0, ImageBuffer[0], sSegmentInfo.arsFingerInfo[0].nWidth * sSegmentInfo.arsFingerInfo[0].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[1].nWidth != 0 && sSegmentInfo.arsFingerInfo[1].nHeight != 0)
                                {
                                    ImageBuffer[1] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[1].nWidth * sSegmentInfo.arsFingerInfo[1].nHeight);
                                    Marshal.Copy(pbyFinImg2, 0, ImageBuffer[1], sSegmentInfo.arsFingerInfo[1].nWidth * sSegmentInfo.arsFingerInfo[1].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[2].nWidth != 0 && sSegmentInfo.arsFingerInfo[2].nHeight != 0)
                                {
                                    ImageBuffer[2] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[2].nWidth * sSegmentInfo.arsFingerInfo[2].nHeight);
                                    Marshal.Copy(pbyFinImg3, 0, ImageBuffer[2], sSegmentInfo.arsFingerInfo[2].nWidth * sSegmentInfo.arsFingerInfo[2].nHeight);
                                }

                                if (sSegmentInfo.arsFingerInfo[3].nWidth != 0 && sSegmentInfo.arsFingerInfo[3].nHeight != 0)
                                {
                                    ImageBuffer[3] = Marshal.AllocHGlobal(sSegmentInfo.arsFingerInfo[3].nWidth * sSegmentInfo.arsFingerInfo[3].nHeight);
                                    Marshal.Copy(pbyFinImg4, 0, ImageBuffer[3], sSegmentInfo.arsFingerInfo[3].nWidth * sSegmentInfo.arsFingerInfo[3].nHeight);
                                }

                                ImageWidth[0] = sSegmentInfo.arsFingerInfo[0].nWidth;
                                ImageWidth[1] = sSegmentInfo.arsFingerInfo[1].nWidth;
                                ImageWidth[2] = sSegmentInfo.arsFingerInfo[2].nWidth;
                                ImageWidth[3] = sSegmentInfo.arsFingerInfo[3].nWidth;

                                ImageHeight[0] = sSegmentInfo.arsFingerInfo[0].nHeight;
                                ImageHeight[1] = sSegmentInfo.arsFingerInfo[1].nHeight;
                                ImageHeight[2] = sSegmentInfo.arsFingerInfo[2].nHeight;
                                ImageHeight[3] = sSegmentInfo.arsFingerInfo[3].nHeight;

                                SegmentSaveImageCaptureProcess(safeimageData, imageWidth, imageHeight, sSegmentInfo.nFingerCnt, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);

                                Marshal.FreeHGlobal(ImageBuffer[0]);
                                Marshal.FreeHGlobal(ImageBuffer[1]);
                                Marshal.FreeHGlobal(ImageBuffer[2]);
                                Marshal.FreeHGlobal(ImageBuffer[3]);

                            }
                        }
                        else
                        {
                            RealScanSDK.RS_GetErrString(nRetVal, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                        }
                    }
                }
            }
            else if (Callback.SelectedIndex == (int)callbackMode.seqCheck)
            {
                SequenceCheckProcess(safeimageData, imageWidth, imageHeight);
            }

            Marshal.FreeHGlobal(safeimageData);




            StartCapture.Enabled = true;
            StopCapture.Enabled = false;
            TakeManual.Enabled = false;
            TakeAuto.Enabled = true;
        }

        private void captureDataCallback(int deviceHandle, int errorCode, IntPtr imageData, int imageWidth, int imageHeight)
        {
            if (imageData == IntPtr.Zero) return;
            if (IsDisposed || !IsHandleCreated) return;



            int size = imageWidth * imageHeight;

           
            byte[] buffer = new byte[size];
            Marshal.Copy(imageData, buffer, 0, size);

           
            IntPtr safeBuffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, safeBuffer, size);

            try
            {
                BeginInvoke(_rawUiCb, errorCode, safeBuffer, imageWidth, imageHeight);
            }
            catch (ObjectDisposedException)
            {
                Marshal.FreeHGlobal(safeBuffer);
            }
            catch (InvalidOperationException)
            {
                Marshal.FreeHGlobal(safeBuffer);
            }
        }

        private void advCaptureDataCallbackInt(int deviceHandle, int errorCode, IntPtr frameInfo)
        {
            
            if (frameInfo == IntPtr.Zero)
                return;

           
            RSFrameInfo fi;
            try
            {
                fi = (RSFrameInfo)Marshal.PtrToStructure(frameInfo, typeof(RSFrameInfo));
            }
            catch (ArgumentException)
            {
                
                return;
            }

            
            IntPtr imageData = fi.pImage;
            int imageWidth = fi.iWidth;
            int imageHeight = fi.iHeight;
            int liveness = fi.iLiveness;

            if (imageData == IntPtr.Zero || imageWidth <= 0 || imageHeight <= 0)
                return;

           
            if (IsDisposed || !IsHandleCreated || _advUiCb == null)
                return;


            int size = imageWidth * imageHeight;

            byte[] buffer = new byte[size];
            Marshal.Copy(imageData, buffer, 0, size);

            IntPtr safeBuffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, safeBuffer, size);


            try
            {
                BeginInvoke(_advUiCb, errorCode, safeBuffer, imageWidth, imageHeight, liveness);

            }
            catch (ObjectDisposedException)
            {
                Marshal.FreeHGlobal(safeBuffer);
            }
            catch (InvalidOperationException)
            {
                Marshal.FreeHGlobal(safeBuffer);
            }
        }

        private void previewDataCallback(int deviceHandle, int errorCode, IntPtr imageData, int imageWidth, int imageHeight)
        {
           
            if (imageData == IntPtr.Zero || imageWidth <= 0 || imageHeight <= 0) return;


            int size = checked(imageWidth * imageHeight);
            var prevImageData = new byte[size];
            Marshal.Copy(imageData, prevImageData, 0, size);

            if (!IsDisposed && IsHandleCreated)
            {
                
                BeginInvoke((Action)(() =>
                {
                    previewCallbackInt(errorCode, prevImageData, imageWidth, imageHeight);
                }));
            }

        }

        private void advPreviewDataCallback(int deviceHandle, int errorCode, IntPtr imageData, int imageWidth, int imageHeight, int quality, int status)
        {
            if (imageData == IntPtr.Zero || imageWidth <= 0 || imageHeight <= 0) return;

            int size = checked(imageWidth * imageHeight);
            var prevImageData = new byte[size];
            Marshal.Copy(imageData, prevImageData, 0, size);

            if (!IsDisposed && IsHandleCreated)
            {
                BeginInvoke((Action)(() =>
                {
                    if (IsDisposed) return;
                    advPreviewCallbackInt(errorCode, prevImageData, imageWidth, imageHeight, quality, status);
                }));
            }
        }

        private void SaveSeqCheckTargetProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            if( m_slapType < RealScanSDK.RS_SLAP_ONE_FINGER )
	        {
                bool bIsOverlapped = false;
                int nOverlappedIndex = m_numOfTargets;
                for (int i = 0; i < m_numOfTargets; i++)
                {
                    if (m_nSeqCheckTargetSlapTypes[i] == m_slapType)
                    {
                        bIsOverlapped = true;
                        nOverlappedIndex = i;
                        break;
                    }
                }

                if(bIsOverlapped == true)
                {
                    int newSize = imageWidth * imageHeight;
                    int oldSize = m_nSeqCheckTargetWidths[nOverlappedIndex] *
                                  m_nSeqCheckTargetHeights[nOverlappedIndex];

                    
                    if (m_pSeqCheckTargetImages[nOverlappedIndex] == null ||
                        m_pSeqCheckTargetImages[nOverlappedIndex].Length < newSize ||
                        newSize > oldSize)
                    {
                        m_pSeqCheckTargetImages[nOverlappedIndex] = new byte[newSize];
                    }

                   
                    Marshal.Copy(imageData,
                                 m_pSeqCheckTargetImages[nOverlappedIndex],
                                 0,
                                 newSize);

                    m_nSeqCheckTargetWidths[nOverlappedIndex] = imageWidth;
                    m_nSeqCheckTargetHeights[nOverlappedIndex] = imageHeight;
                }
                else
                {
                    m_pSeqCheckTargetImages[m_numOfTargets] = new byte[imageWidth * imageHeight];
                    Marshal.Copy(imageData, m_pSeqCheckTargetImages[m_numOfTargets], 0, imageWidth * imageHeight);
                    m_nSeqCheckTargetWidths[m_numOfTargets] = imageWidth;
                    m_nSeqCheckTargetHeights[m_numOfTargets] = imageHeight;
                    m_nSeqCheckTargetSlapTypes[m_numOfTargets] = m_slapType;
                    m_numOfTargets++;
                    m_numOfTargerFingers += m_fingerCount;

                    switch (m_slapType)
                    {
                        case RealScanSDK.RS_SLAP_TWO_THUMB:
                            seqTargetList.Items.Add("TWO THUMBS");
                            break;
                        case RealScanSDK.RS_SLAP_LEFT_FOUR:
                            seqTargetList.Items.Add("LEFT FOUR FINGERS");
                            break;
                        case RealScanSDK.RS_SLAP_RIGHT_FOUR:
                            seqTargetList.Items.Add("RIGHT FOUR FINGERS");
                            break;
                    }
                }
	        }
        }



        private void SegmentSaveImageCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int numOfFingers, RSSlapInfoArray slapInfo, IntPtr[] ImageBuffer, int[] ImageWidth, int[] ImageHeight)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.InitialDirectory = ".\\";
                    saveDialog.FilterIndex = 1;

                    var result = saveDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        RealScanSDK.RS_SaveBitmap(imageData, imageWidth, imageHeight, saveDialog.FileName + ".bmp");
                        if (m_result != RealScanSDK.RS_SUCCESS)
                        {
                            RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                        }

                        int wsqBufferLength = imageWidth * imageHeight * 2;
                        byte[] wsqBuffer = new byte[wsqBufferLength];
                        int retCode = RealScanSDK.RS_EncodeWSQ(imageData, imageWidth, imageHeight, 0.75f, wsqBuffer, ref wsqBufferLength);

                        
                        FileStream fs = System.IO.File.Open(saveDialog.FileName + ".wsq", FileMode.Create);

                        
                        using (BinaryWriter wr = new BinaryWriter(fs))
                        {
                            wr.Write(wsqBuffer, 0, wsqBufferLength); // bytes
                        }

                       
                        fs = System.IO.File.Open(saveDialog.FileName + ".raw", FileMode.Create);

                        
                        using (BinaryWriter wr = new BinaryWriter(fs))
                        {
                            byte[] managedArray = new byte[imageWidth * imageHeight];
                            Marshal.Copy(imageData, managedArray, 0, m_nCustomSegWidth * m_nCustomSegHeight);

                            wr.Write(managedArray, 0, imageWidth * imageHeight); // bytes
                        }

                        for (int i = 0; i < numOfFingers; i++)
                        {

                            if (ImageWidth[i] == 0 || ImageHeight[i] == 0)
                            {
                                continue;
                            }
                            if (setSegmentSize.Checked)
                                RealScanSDK.RS_SaveBitmap(ImageBuffer[i], m_nCustomSegWidth, m_nCustomSegHeight, saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".bmp");
                            else
                                RealScanSDK.RS_SaveBitmap(ImageBuffer[i], ImageWidth[i], ImageHeight[i], saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".bmp");
                            if (m_result != RealScanSDK.RS_SUCCESS)
                            {
                                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                                MsgPanel.Text = m_errorMsg;
                            }



                            if (setSegmentSize.Checked)
                            {
                                wsqBufferLength = m_nCustomSegWidth * m_nCustomSegHeight * 2;
                                byte[] swsqBuffer = new byte[wsqBufferLength];
                                retCode = RealScanSDK.RS_EncodeWSQ(ImageBuffer[i], m_nCustomSegWidth, m_nCustomSegHeight, 0.75f, swsqBuffer, ref wsqBufferLength);

                                
                                fs = System.IO.File.Open(saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".wsq", FileMode.Create);

                              
                                using (BinaryWriter wr = new BinaryWriter(fs))
                                {
                                    wr.Write(wsqBuffer, 0, wsqBufferLength); // bytes
                                }

                               
                                fs = System.IO.File.Open(saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".raw", FileMode.Create);

                               
                                using (BinaryWriter wr = new BinaryWriter(fs))
                                {
                                    byte[] managedArray = new byte[m_nCustomSegWidth * m_nCustomSegHeight];
                                    Marshal.Copy(ImageBuffer[i], managedArray, 0, m_nCustomSegWidth * m_nCustomSegHeight);

                                    wr.Write(managedArray, 0, m_nCustomSegWidth * m_nCustomSegHeight); // bytes
                                }
                            }
                            else
                            {
                                wsqBufferLength = ImageWidth[i] * ImageHeight[i] * 2;
                                byte[] swsqBuffer = new byte[wsqBufferLength];
                                retCode = RealScanSDK.RS_EncodeWSQ(ImageBuffer[i], ImageWidth[i], ImageHeight[i], 0.75f, swsqBuffer, ref wsqBufferLength);

                                
                                fs = System.IO.File.Open(saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".wsq", FileMode.Create);

                               
                                using (BinaryWriter wr = new BinaryWriter(fs))
                                {
                                    wr.Write(swsqBuffer, 0, wsqBufferLength); // bytes
                                }

                                
                                fs = System.IO.File.Open(saveDialog.FileName + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".raw", FileMode.Create);


                                using (BinaryWriter wr = new BinaryWriter(fs))
                                {
                                    byte[] managedArray = new byte[ImageWidth[i] * ImageHeight[i]];
                                    Marshal.Copy(ImageBuffer[i], managedArray, 0, ImageWidth[i] * ImageHeight[i]);

                                    wr.Write(managedArray, 0, ImageWidth[i] * ImageHeight[i]); // bytes
                                }

                            }
                        }
                    }
                }
            }));
        }
        private void SaveImageCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.InitialDirectory = ".:\\";
            saveDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.DefaultExt = "bmp";
           
           
            if (saveDialog.ShowDialog(this) == DialogResult.OK) 
            {
                m_result = RealScanSDK.RS_SaveBitmap(imageData, imageWidth, imageHeight, saveDialog.FileName);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                else
                {
                    saveDialog.Dispose();
                }
            }
        }

        private void SaveImageAnsiNist_2011_CaptureProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            
            if(m_nCheckNIST != DF_NIST_ALL)
            {
               
                if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS)
                {
                    m_left4Fingers = new byte[imageWidth * imageHeight];
                    Marshal.Copy(imageData, m_left4Fingers, 0, imageWidth * imageHeight);

                    m_nLeft4imageWidth = imageWidth;
                    m_nLeft4imageHeight = imageHeight;

                    m_nCheckNIST += DF_NIST_LEFT4;
                }

                if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS)
                {
                    m_right4Fingers = new byte[imageWidth * imageHeight];
                    Marshal.Copy(imageData, m_right4Fingers, 0, imageWidth * imageHeight);
                    m_nRight4imageWidth = imageWidth;
                    m_nRight4imageHeight = imageHeight;
                    m_nCheckNIST += DF_NIST_RIGHT4;
                }

                if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS || m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX)
                {
                    m_thumb2Fingers = new byte[imageWidth * imageHeight];
                    Marshal.Copy(imageData, m_thumb2Fingers, 0, imageWidth * imageHeight);
                    m_nThumb2ImageWidth = imageWidth;
                    m_nThumb2ImageHeight = imageHeight;

                    m_nCheckNIST += DF_NIST_THUMB2;
                }

                if (m_nCheckNIST != DF_NIST_ALL)
                {
                    string strMessage = "You need to capture ";
                    string strValue = "";


                    if ((m_nCheckNIST & DF_NIST_LEFT4) != DF_NIST_LEFT4)
                    {
                        strValue = "Left 4 fingers ";
                        strMessage += strValue;
                    }

                    if ((m_nCheckNIST & DF_NIST_RIGHT4) != DF_NIST_RIGHT4)
                    {
                        if (strValue.Length > 0)
                        {
                            strValue = ", Right 4 fingers ";
                        }
                        else
                        {
                            strValue = "Right 4 fingers ";
                        }

                        strMessage += strValue;
                    }

                    if ((m_nCheckNIST & DF_NIST_THUMB2) != DF_NIST_THUMB2)
                    {
                        if (strValue.Length > 0)
                        {
                            strValue = ", Thumb 2 fingers ";
                        }
                        else
                        {
                            strValue = "Thumb 2 fingers ";
                        }
                        strMessage += strValue;
                    }

                    MsgPanel.Text = strMessage;



                    return;
                }
            }
            
        


            int nSize = Marshal.SizeOf(typeof(RSEType1Info));
            IntPtr ptr = Marshal.AllocHGlobal(nSize);
          
            int result = RS_AN2K11.RSE_SetType1Info("CPS", 4, "KR/KOR1", "KR/KOR2", "0747439609K", "TCR", ptr);
                        
            result = RS_AN2K11.RSE_CreateFileCS(ptr);

            RSEType1Info type1Info = new RSEType1Info();
            type1Info = (RSEType1Info)Marshal.PtrToStructure(ptr, typeof(RSEType1Info));


            if (result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;

            }

            Marshal.FreeHGlobal(ptr);

            //
            // make type 2 record 
            //
            RSEType2Info type2Info = new RSEType2Info();
            string value;

            switch (deviceType_)
            {
                case 0x00: // RS_DEVICE_REALSCAN_10:
                    value = "RealScan-10";
                    break;

                case 0x01: // RS_DEVICE_REALSCAN_10F:
                    value = "RealScan-10F";
                    break;

                case 0x10: // RS_DEVICE_REALSCAN_D:
                    value = "RealScan-D";
                    break;

                case 0x11: // RS_DEVICE_REALSCAN_DF:
                    value = "RealScan-DF";
                    break;

                case 0x20: // RS_DEVICE_REALSCAN_F
                    value = "RealScan-F";
                    break;

                case 0x30: // RS_DEVICE_REALSCAN_G10
                    value = "RealScan-G10";
                    break;

                case 0x31: // RS_DEVICE_REALSCAN_G10F
                    value = "RealScan-G10F";
                    break;

                case 0x32: // RS_DEVICE_REALSCAN_G1
                    value = "RealScan-G1";// added by jieney - 2011.11.24
                    break;

                case 0x33: // RS_DEVICE_REALSCAN_G10I
                    value = "RealScan-G10i";// added by JSH - 2022.04.06
                    break;


                case 0x40: // RS_DEVICE_REALSCAN_S60
                    value = "RealScan S60";// added by JSH - 2022.04.06
                    break;

                case 0x50: // RS_DEVICE_REALSCAN_SG10
                    value = "RealScan SG10";
                    break;
                case 0x51: // RS_DEVICE_REALSCAN_SG10
                    value = "RealScan SG10i";
                    break;
                default:
                    value = "Unknown";
                    break;
            }

            type2Info.systemModel = value;

            value = "Xperix Inc";


            type2Info.systemMaker = value;


            value = DeviceID.Text;

            type2Info.serialNo = value;

            type2Info.numCustomFields = 2;
            type2Info.customFields = new RSEType2CustomField[500];
            type2Info.customFields[0] = new RSEType2CustomField();
            type2Info.customFields[1] = new RSEType2CustomField();
            type2Info.customFields[0].fieldID = 5;
            type2Info.customFields[0].fieldValue = "Suspect Name"; //String2ByteArray("Suspect Name",255);
            type2Info.customFields[1].fieldID = 6;
            type2Info.customFields[1].fieldValue = "Father Name";// String2ByteArray("Father Name", 255);

            int IDC = 0;
            type2Info.IDC = IDC++;

            int type2InfoSize = Marshal.SizeOf(typeof(RSEType2Info));
            IntPtr type2InfoPtr = Marshal.AllocHGlobal(type2InfoSize);
            Marshal.StructureToPtr(type2Info, type2InfoPtr, false);

            result = RS_AN2K11.RSE_CreateType2RecordCS(type2InfoPtr);

            Marshal.FreeHGlobal(type2InfoPtr);

            if (result != RealScanSDK.RS_SUCCESS)
            {
                RS_AN2K11.RSE_FreeFileCS();
                return;
            }

            result = RS_AN2K11.RSE_AppendRecordCS();

            if (result != RealScanSDK.RS_SUCCESS)
            {

                RS_AN2K11.RSE_FreeRecordCS();
                RS_AN2K11.RSE_FreeFileCS();

                return;
            }

            IntPtr ImageBuffer;
            ImageBuffer = Marshal.AllocHGlobal(m_nLeft4imageWidth * m_nLeft4imageHeight);
            Marshal.Copy(m_left4Fingers, 0, ImageBuffer, m_nLeft4imageWidth * m_nLeft4imageHeight);
            processRecordType14(type1Info, ref IDC, ImageBuffer, m_nLeft4imageWidth, m_nLeft4imageHeight, RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS);
            Marshal.FreeHGlobal(ImageBuffer);

            ImageBuffer = Marshal.AllocHGlobal(m_nRight4imageWidth * m_nRight4imageHeight);
            Marshal.Copy(m_right4Fingers, 0, ImageBuffer, m_nRight4imageWidth * m_nRight4imageHeight);
            processRecordType14(type1Info, ref IDC, ImageBuffer, m_nRight4imageWidth, m_nRight4imageHeight, RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS);
            Marshal.FreeHGlobal(ImageBuffer);
            ImageBuffer = Marshal.AllocHGlobal(m_nThumb2ImageWidth * m_nThumb2ImageHeight);
            Marshal.Copy(m_thumb2Fingers, 0, ImageBuffer, m_nThumb2ImageWidth * m_nThumb2ImageHeight);
            processRecordType14(type1Info, ref IDC, ImageBuffer, m_nThumb2ImageWidth, m_nThumb2ImageHeight, RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS);
            Marshal.FreeHGlobal(ImageBuffer);

            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.InitialDirectory = ".:\\";
            saveDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.DefaultExt = "bin";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {


                result = RS_AN2K11.RSE_WriteFileCS(saveDialog.FileName);
                

                saveDialog.Dispose();
            }

            RS_AN2K11.RSE_FreeFileCS();
            m_nCheckNIST = 0;
        }

        private void processRecordType14(RSEType1Info type1Info, ref int IDC, IntPtr imageData, int imageWidth, int imageHeight, int captureMode)
        {

            int result = 0;
            
            // 
            // Make type 14 record
            //
            //	int numOfFinger;
            //	RSESlapInfo* slapInfo;
            RSEType14Info recordInfo = new RSEType14Info();


            recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_PLAIN;

            recordInfo.sourceAgency = type1Info.originatingAgencyIdentifier;
            recordInfo.captureDate = type1Info.date;

            if (captureMode == RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS || captureMode == RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS || captureMode == RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS)
            {
                recordInfo.IDC = IDC++;

                int slapType = 0;
                switch (captureMode)
                {
                    case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                    case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                        slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                        break;

                    case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                        slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                        break;

                    case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
                        slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                        break;

                    default:
                        slapType = 0;
                        break;
                }

                int numOfFingers = 0;
                RSSlapInfoArray slapInfoA = new RSSlapInfoArray();
                IntPtr slapInfoArray;


                int _size = Marshal.SizeOf(typeof(RSSlapInfoArray));
                slapInfoArray = Marshal.AllocHGlobal(_size);
                Marshal.StructureToPtr(slapInfoA, slapInfoArray, true);

                IntPtr[] ImageBuffer = new IntPtr[4];
                int[] ImageWidth = new int[4];
                int[] ImageHeight = new int[4];

                m_result = RealScanSDK.RS_Segment4(imageData, imageWidth, imageHeight, slapType, ref numOfFingers, ref slapInfoArray, ref ImageBuffer[0], ref ImageWidth[0],
                                                ref ImageHeight[0], ref ImageBuffer[1], ref ImageWidth[1], ref ImageHeight[1], ref ImageBuffer[2], ref ImageWidth[2],
                                                ref ImageHeight[2], ref ImageBuffer[3], ref ImageWidth[3], ref ImageHeight[3]);

                recordInfo.horizontalLine = imageWidth;
                recordInfo.verticalLine = imageHeight;
                recordInfo.scaleUnit = 1;
                recordInfo.horizontalPixelScale = 500;
                recordInfo.verticalPixelScale = 500;
                recordInfo.compressionAlgorithm = RS_AN2K11.RSE_IMG_COMP_WSQ20;
                recordInfo.bpp = 8;
                recordInfo.numOfFingerPosition = 1;

                recordInfo.fingerPosition = new int[6];

                if (captureMode == RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS)
                {
                    recordInfo.fingerPosition[0] = RS_AN2K11.RSE_FGP_PLAIN_LEFT_FOUR;
                }
                else if (captureMode == RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS)
                {
                    recordInfo.fingerPosition[0] = RS_AN2K11.RSE_FGP_PLAIN_RIGHT_FOUR;
                }
                else if (captureMode == RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS)
                {
                    recordInfo.fingerPosition[0] = RS_AN2K11.RSE_FGP_PLAIN_TWO_THUMBS;
                }

                recordInfo.numOfSegment = numOfFingers;
                recordInfo.numOfQuality = numOfFingers;


                slapInfoA = (RSSlapInfoArray)Marshal.PtrToStructure(slapInfoArray, typeof(RSSlapInfoArray));

                recordInfo.segPosition = new RSESegmentPosition[4];
                recordInfo.imgQuality = new RSEImageQuality[4];
                for (int i = 0; i < numOfFingers; i++)
                {
                    recordInfo.segPosition[i].fingerNumber = slapInfoA.RSSlapInfoA[i].fingerType;

                    int left, right;
                    int top, bottom;

                    left = right = slapInfoA.RSSlapInfoA[i].fingerPosition[0].x;
                    top = bottom = slapInfoA.RSSlapInfoA[i].fingerPosition[0].y;

                    for (int j = 1; j < 4; j++)
                    {
                        if (left > slapInfoA.RSSlapInfoA[i].fingerPosition[j].x || left < 0)
                        {
                            left = slapInfoA.RSSlapInfoA[i].fingerPosition[j].x;
                        }

                        if (right < slapInfoA.RSSlapInfoA[i].fingerPosition[j].x)
                        {
                            right = slapInfoA.RSSlapInfoA[i].fingerPosition[j].x;
                        }

                        if (top > slapInfoA.RSSlapInfoA[i].fingerPosition[j].y || top < 0)
                        {
                            top = slapInfoA.RSSlapInfoA[i].fingerPosition[j].y;
                        }

                        if (bottom < slapInfoA.RSSlapInfoA[i].fingerPosition[j].y)
                        {
                            bottom = slapInfoA.RSSlapInfoA[i].fingerPosition[j].y;
                        }
                    }

                    recordInfo.segPosition[i].left = left;
                    recordInfo.segPosition[i].right = right;
                    recordInfo.segPosition[i].top = top;
                    recordInfo.segPosition[i].bottom = bottom;

                    recordInfo.imgQuality[i].fingerNumber = slapInfoA.RSSlapInfoA[i].fingerType;
                    recordInfo.imgQuality[i].score = slapInfoA.RSSlapInfoA[i].imageQuality;


                    if (ImageBuffer[i] != (IntPtr)0)
                    {
                        RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                    }
                }

                if (slapInfoArray != (IntPtr)0)
                {
                    RealScanSDK.RS_FreeImageData(slapInfoArray);
                }

                //Marshal.FreeHGlobal(slapInfoArray);


                int wsqBufferLength = imageWidth * imageHeight * 2;
                byte[] wsqBuffer = new byte[wsqBufferLength];
                int retCode = RealScanSDK.RS_EncodeWSQ(imageData, imageWidth, imageHeight, 0.75f, wsqBuffer, ref wsqBufferLength);

                recordInfo.imageLen = wsqBufferLength;
                recordInfo.imageData = Marshal.AllocHGlobal(recordInfo.imageLen);
                Marshal.Copy(wsqBuffer, 0, recordInfo.imageData, wsqBufferLength);

                result = RS_AN2K11.RSE_CreateType14RecordCS(ref recordInfo);


                if (result != RS_AN2K11.RSE_SUCCESS)
                {
                    RS_AN2K11.RSE_FreeFileCS();
                    return;
                }

                result = RS_AN2K11.RSE_AppendRecordCS();

                if (result != RS_AN2K11.RSE_SUCCESS)
                {
                    RS_AN2K11.RSE_FreeRecordCS();
                    RS_AN2K11.RSE_FreeFileCS();
                    return;
                }

                Marshal.FreeHGlobal(recordInfo.imageData);
            }
        }


        private void SaveImage19794_4_2011_CaptureProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.InitialDirectory = ".:\\";
            saveDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.DefaultExt = "bin";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {

                int fingerPosition = 0;

                if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS)
                {
                    fingerPosition = RealScanSDK.RS_ISO19794_FGP_PLAIN_LEFT_FOUR;
                }
                else if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS)
                {
                    fingerPosition = RealScanSDK.RS_ISO19794_FGP_PLAIN_RIGHT_FOUR;
                }
                else if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS)
                {
                    fingerPosition = RealScanSDK.RS_ISO19794_FGP_PLAIN_TWO_THUMBS;
                }
                else
                {
                    MsgPanel.Text = "Capture Mode is not supported";
                    return ;
                }



                int jp2BufferLength = imageWidth * imageHeight * 2;
                byte[] jp2Buffer = new byte[jp2BufferLength];
                int retCode = RealScanSDK.RS_EncodeJP2(imageData, imageWidth, imageHeight, jp2Buffer, ref jp2BufferLength);



                IntPtr output = (IntPtr)0;
                int outputLen = 0;


                IntPtr intputJP2Image = Marshal.AllocHGlobal(jp2BufferLength);
                Marshal.Copy(jp2Buffer, 0, intputJP2Image, jp2BufferLength);

                m_result = RealScanSDK.RS_GenerateIso19794_4_2011Ex(deviceHandle, intputJP2Image, jp2BufferLength, imageWidth, imageHeight, fingerPosition, RealScanSDK.RSE_COMP_19794_JPEG2000, ref output, ref outputLen);

                Marshal.FreeHGlobal(intputJP2Image);
                //m_result = RealScanSDK.RS_SaveBitmap(imageData, imageWidth, imageHeight, saveDialog.FileName);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;

                    if (capturedImageData != (IntPtr)0)
                    {
                        RealScanSDK.RS_FreeImageData(capturedImageData);
                    }
                }
                else
                {

                    FileStream fs = File.Open(saveDialog.FileName, FileMode.Create);

                   
                    using (BinaryWriter wr = new BinaryWriter(fs))
                    {
                        byte[] managedArray = new byte[outputLen];
                        Marshal.Copy(output, managedArray, 0, outputLen);

                        wr.Write(managedArray, 0, outputLen); // bytes
                    }



                    if (output != (IntPtr)0)
                    {
                        RealScanSDK.RS_FreeImageData(output);
                    }
                    saveDialog.Dispose();
                }
            }
        }
        private void EnlargeCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            Form imageform = new Form();

            if (imageform.BackgroundImage != null)
            {
                imageform.BackgroundImage = null;
            }

            imageform.FormBorderStyle = FormBorderStyle.FixedDialog;
            imageform.Refresh();
            imageform.Height = imageHeight;
            imageform.Width = imageWidth;
            imageform.MaximizeBox = false;
            imageform.MinimizeBox = false;

            Bitmap FingerPrintImageBuffer = new Bitmap(imageWidth, imageHeight, (imageWidth + 3) / 4 * 4, PixelFormat.Format8bppIndexed, imageData);
            ColorPalette BufferPalette = FingerPrintImageBuffer.Palette;
            Color[] entries = BufferPalette.Entries;

            for (int i = 0; i < 256; i++)
            {
                Color ForGrayScaleColor = new Color();
                ForGrayScaleColor = Color.FromArgb((byte)i, (byte)i, (byte)i);
                entries[i] = ForGrayScaleColor;
            }
            FingerPrintImageBuffer.Palette = BufferPalette;

            Image image = FingerPrintImageBuffer;

            Bitmap blank = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(blank);
            g.Clear(Color.White);
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            Bitmap tempimage = new Bitmap(blank);

            blank.Dispose();
            image.Dispose();

            image = new Bitmap(tempimage);
            imageform.BackgroundImage = image;

            tempimage.Dispose();
            imageform.Show();
        }
        private void SegmentCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int deviceHandle, ref RSSlapInfoArray slapInfo,
                                           ref int numOfFingers, ref IntPtr[] ImageBuffer, ref int[] ImageWidth, ref int[] ImageHeight)
        {
            RSSlapInfoArray slapInfoA = new RSSlapInfoArray();
            IntPtr slapInfoArray;

            int captureMode = 0;
            int captureOption = 0;
            int slapType = 1;

            for (int i = 0; i < 4; i++)
            {
                ImageBuffer[i] = (IntPtr)0;
                ImageWidth[i] = 0;
                ImageHeight[i] = 0;
            }

            int _size = Marshal.SizeOf(typeof(RSSlapInfoArray));
            slapInfoArray = Marshal.AllocHGlobal(_size);
            Marshal.StructureToPtr(slapInfoA, slapInfoArray, true);

            int fingerType = 0;
            int[] missingFingerArray = new int[] { 0, 0, 0, 0 };

            int n = 0;
            if (m_captureDir != RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT)
            {
                int captureDir = RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT;
                m_result = RealScanSDK.RS_GetCaptureModeWithDir(deviceHandle, ref captureMode, ref captureDir, ref captureOption);
            }
            else
            {
                m_result = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref captureMode, ref captureOption);
            }


            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            switch (captureMode)
            {
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                    slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER:
                case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX:
                    slapType = RealScanSDK.RS_SLAP_ONE_FINGER;
                    break;

                case RealScanSDK.RS_CAPTURE_ROLL_FINGER:
                case RealScanSDK.RS_CAPTURE_ROLL_FINGER_EX:
                    slapType = RealScanSDK.RS_SLAP_ONE_FINGER_ROLL;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                    slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;


                    int[] leftFingerMap =
                                    {
                        RealScanSDK.RS_FGP_LEFT_LITTLE, // chk 0 → 10
                        RealScanSDK.RS_FGP_LEFT_RING,   // chk 1 → 9
                        RealScanSDK.RS_FGP_LEFT_MIDDLE, // chk 2 → 8
                        RealScanSDK.RS_FGP_LEFT_INDEX   // chk 3 → 7
                    };

                    for (int i = 0; i < 4; i++)
                    {
                        if (leftFingers.GetItemChecked(i))
                            missingFingerArray[n++] = leftFingerMap[i];
                    }


                    fingerType = RealScanSDK.RS_FGP_LEFT_LITTLE;
                    break;

                case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
                    slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                    int[] rightFingerMap =
                    {
                        RealScanSDK.RS_FGP_RIGHT_LITTLE, // 0
                        RealScanSDK.RS_FGP_RIGHT_RING,   // 1
                        RealScanSDK.RS_FGP_RIGHT_MIDDLE, // 2
                        RealScanSDK.RS_FGP_RIGHT_INDEX   // 3
                    };

                    for (int i = 0; i < 4; i++)
                    {
                        if (rightFingers.GetItemChecked(i))
                            missingFingerArray[n++] = rightFingerMap[i];
                    }
                    fingerType = RealScanSDK.RS_FGP_RIGHT_INDEX;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_MANUAL:
                    slapType = RealScanSDK.RS_SLAP_MANUAL;
                    break;
                default:
                    MsgPanel.Text = "Cannot segment in this mode";
                    return;
            }

            int minfinger = 4;
            RealScanSDK.RS_GetMinimumFinger(deviceHandle, ref minfinger);

            if (captureMode == RealScanSDK.RS_CAPTURE_FLAT_MANUAL && minfinger == 2)
            {
                slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
            }

            if (setSegmentSize.Checked)
            {
                m_nCustomSegWidth = Convert.ToInt32(SegWidth.Text);
                m_nCustomSegHeight = Convert.ToInt32(SegHeight.Text);

                m_result = RealScanSDK.RS_Segment4WithSize(imageData, imageWidth, imageHeight, slapType, ref numOfFingers, ref slapInfoArray,
                                                            ref ImageBuffer[0], ref ImageBuffer[1], ref ImageBuffer[2], ref ImageBuffer[3],
                                                            m_nCustomSegWidth, m_nCustomSegHeight);
            }
            else
            {
                m_result = RealScanSDK.RS_Segment4(imageData, imageWidth, imageHeight, slapType, ref numOfFingers, ref slapInfoArray, ref ImageBuffer[0], ref ImageWidth[0],
                                                 ref ImageHeight[0], ref ImageBuffer[1], ref ImageWidth[1], ref ImageHeight[1], ref ImageBuffer[2], ref ImageWidth[2],
                                                 ref ImageHeight[2], ref ImageBuffer[3], ref ImageWidth[3], ref ImageHeight[3]);
            }


            if (m_result == RealScanSDK.RS_SUCCESS || m_result == RealScanSDK.RS_ERR_SEGMENT_WRONG_HAND || m_result == RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER)
            {
                if (m_result == RealScanSDK.RS_ERR_SEGMENT_WRONG_HAND)
                {
                    MsgPanel.Text = "Captured fingers in wrong hand: ";
                }

                else
                {
                    MsgPanel.Text = $"Captured {numOfFingers} fingers ";
                }


                MsgPanel.Text += "Quality:";

                slapInfoA = (RSSlapInfoArray)Marshal.PtrToStructure(slapInfoArray, typeof(RSSlapInfoArray));
                if (slapInfoArray != (IntPtr)0)
                {
                    RealScanSDK.RS_FreeImageData(slapInfoArray);
                }

                int overlayHandle = -1;
                int j = 0;
                int missingCount = n;



                if (missingCount > 1)
                {

                    Array.Sort(missingFingerArray, 0, missingCount);


                    if (slapType == RealScanSDK.RS_SLAP_LEFT_FOUR)
                    {
                        Array.Reverse(missingFingerArray, 0, missingCount);
                    }
                }

                for (int i = 0; i < numOfFingers; i++)
                {
                    if (slapInfoA.RSSlapInfoA[i].fingerType == RealScanSDK.RS_FGP_UNKNOWN)
                    {

                        if (slapType == RealScanSDK.RS_SLAP_LEFT_FOUR)
                        {
                            while (j < missingCount && fingerType == missingFingerArray[j])
                            {
                                fingerType--;
                                j++;
                            }

                            slapInfoA.RSSlapInfoA[i].fingerType = fingerType--;
                        }
                        else if (slapType == RealScanSDK.RS_SLAP_RIGHT_FOUR)
                        {


                            while (j < missingCount && fingerType == missingFingerArray[j])
                            {
                                fingerType++;
                                j++;
                            }

                            slapInfoA.RSSlapInfoA[i].fingerType = fingerType++;
                        }
                    }

                    slapInfo = slapInfoA;

                    RealScan.RSOverlayQuadrangle quad = new RSOverlayQuadrangle();
                    quad.pos = new RSPoint[4];
                    quad.color = 0x00ff0000;

                    RSRect rect = new RSRect();
                    RealScanSDK.GetClientRect(PreviewWindow.Handle, ref rect);

                    quad.pos[0].x = slapInfoA.RSSlapInfoA[i].fingerPosition[0].x * rect.right / imageWidth;
                    quad.pos[0].y = slapInfoA.RSSlapInfoA[i].fingerPosition[0].y * rect.bottom / imageHeight;
                    quad.pos[1].x = slapInfoA.RSSlapInfoA[i].fingerPosition[1].x * rect.right / imageWidth;
                    quad.pos[1].y = slapInfoA.RSSlapInfoA[i].fingerPosition[1].y * rect.bottom / imageHeight;
                    quad.pos[2].x = slapInfoA.RSSlapInfoA[i].fingerPosition[3].x * rect.right / imageWidth;
                    quad.pos[2].y = slapInfoA.RSSlapInfoA[i].fingerPosition[3].y * rect.bottom / imageHeight;
                    quad.pos[3].x = slapInfoA.RSSlapInfoA[i].fingerPosition[2].x * rect.right / imageWidth;
                    quad.pos[3].y = slapInfoA.RSSlapInfoA[i].fingerPosition[2].y * rect.bottom / imageHeight;

                    m_result = RealScanSDK.RS_AddOverlayQuadrangle(deviceHandle, ref quad, ref overlayHandle);
                    m_result = RealScanSDK.RS_ShowOverlay(overlayHandle, true);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        MsgPanel.Text = "Cannot overlay for quadrangle" + m_result;
                        return;
                    }
                }

                for (int i = 0; i < numOfFingers; i++)
                {
                    MsgPanel.Text += "[" + slapInfoA.RSSlapInfoA[i].fingerType + ":" + slapInfoA.RSSlapInfoA[i].imageQuality + "] ";
                }
            }
            else
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }


        }

        private void SequenceCheckProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            int result = -1;
            string[] pFingerMsg = {
            "None",
            "Right Thumb",
            "Right Index",
            "Right Middle",
            "Right Ring",
            "Right Little",
            "Left Thumb",
            "Left Index",
            "Left Middle",
            "Left Ring",
            "Left Little"};

            int seqCheckResult = -1;
            const int nSecuLev = 5;    // (0~7)
            int numOfSlaps = seqTargetList.Items.Count;

            if (numOfSlaps == 0)
                MsgPanel.Text = "There is no target image";
            else
            {
                bool bIsTarget = false;
                bool bIsMatched = false;



                for (int i = 0; i < numOfSlaps; i++)
                {
                    if (seqTargetList.GetItemChecked(i))
                    {
                        bIsTarget = true;

                        IntPtr target = Marshal.AllocHGlobal(m_nSeqCheckTargetWidths[i] * m_nSeqCheckTargetHeights[i]);
                        Marshal.Copy(m_pSeqCheckTargetImages[i], 0, target, m_nSeqCheckTargetWidths[i] * m_nSeqCheckTargetHeights[i]);

                        RSMissingInfo sMissingInfo = new RSMissingInfo();


                        if (m_nSeqCheckTargetSlapTypes[i] == RealScanSDK.RS_SLAP_LEFT_FOUR)
                        {
                            if (leftFingers.GetItemChecked(0)) sMissingInfo.nFirstfinger = 1;
                            if (leftFingers.GetItemChecked(1)) sMissingInfo.nSecondfinger = 1;
                            if (leftFingers.GetItemChecked(2)) sMissingInfo.nThirdfinger = 1;
                            if (leftFingers.GetItemChecked(3)) sMissingInfo.nFourthfinger = 1;
                        }
                        else
                        {
                            if (rightFingers.GetItemChecked(3)) sMissingInfo.nFirstfinger = 1; // Index
                            if (rightFingers.GetItemChecked(2)) sMissingInfo.nSecondfinger = 1; // Middle
                            if (rightFingers.GetItemChecked(1)) sMissingInfo.nThirdfinger = 1; // Ring
                            if (rightFingers.GetItemChecked(0)) sMissingInfo.nFourthfinger = 1; // Little
                        }

                        bool existMissingInfo = false;
                        if (sMissingInfo.nFirstfinger > 0) existMissingInfo = true;
                        else if (sMissingInfo.nSecondfinger > 0) existMissingInfo = true;
                        else if (sMissingInfo.nThirdfinger > 0) existMissingInfo = true;
                        else if (sMissingInfo.nFourthfinger > 0) existMissingInfo = true;

                        if (existMissingInfo)
                        {
                            result = RealScanSDK.RS_SequenceCheckMissingFinger(1, ref imageData, ref imageWidth, ref imageHeight, target,
                         m_nSeqCheckTargetWidths[i], m_nSeqCheckTargetHeights[i], m_nSeqCheckTargetSlapTypes[i], ref seqCheckResult, nSecuLev, ref sMissingInfo);

                        }
                        else
                        {
                            result = RealScanSDK.RS_SequenceCheck(1, ref imageData, ref imageWidth, ref imageHeight, target,
                         m_nSeqCheckTargetWidths[i], m_nSeqCheckTargetHeights[i], m_nSeqCheckTargetSlapTypes[i], ref seqCheckResult, nSecuLev);

                        }



                        Marshal.FreeHGlobal(target);

                        if (result == RealScanSDK.RS_SUCCESS)
                        {
                            MsgPanel.Text = "This finger is the" + pFingerMsg[seqCheckResult] + "finger";
                            bIsMatched = true;
                            break;
                        }
                    }
                }

                if (!bIsTarget)
                    MsgPanel.Text = "You didn't select target images";
                else if (!bIsMatched)
                    MsgPanel.Text = "There isn't matched finger";
            }
        }

        private void StartCapture_Click(object sender, EventArgs e)
        {
            CaptureMode_SelectedIndexChanged(null, null);

            if (!m_bCaptureModeSelected)
            {
                MsgPanel.Text = "Capture mode isn't selected successfully";
                return;
            }

            if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_MANUAL)
            {
                string msg = "Do you want to capture in custom size" + "(" + m_nCustomCaptW + "x" + m_nCustomCaptH + ")";
                DialogResult msgRet = MessageBox.Show(msg, "Custom Size Capture", MessageBoxButtons.YesNo);
                if (msgRet == DialogResult.No)
                    return;
                MsgPanel.Text = "Place fingers on the sensor";
            }
            else if(m_captureMode == RealScanSDK.RS_CAPTURE_ROLL_FINGER || m_captureMode == RealScanSDK.RS_CAPTURE_ROLL_FINGER_EX)
            {
                MsgPanel.Text = "Roll a finger on the sensor";
            }
            else
            {
                MsgPanel.Text = "Place fingers on the sensor";
            }
            SetPreview();
            if (AutoCapture.Checked)
            {
                if (_selectedPrevMode == PrevMode.advCallbackDraw)
                {
                    m_result = RealScanSDK.RS_RegisterAdvCaptureDataCallback(deviceHandle, advCaptureCallback);
                }
                else
                {
                    m_result = RealScanSDK.RS_RegisterCaptureDataCallback(deviceHandle, rawCaptureCallback);
                }

                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
            else
            {
                RealScanSDK.RS_RegisterCaptureDataCallback(deviceHandle, (CaptureDataCallback)null);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }

            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_StartCapture(deviceHandle, AutoCapture.Checked, 0);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                int ret = RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }
            StartCapture.Enabled = false;
            StopCapture.Enabled = true;
            TakeManual.Enabled = true;
            TakeAuto.Enabled = false;
        }

        private void StopCapture_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_AbortCapture(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }
            else
            {
                MsgPanel.Text = "Capture is aborted";
            }
            StartCapture.Enabled = true;
            StopCapture.Enabled = false;
            TakeManual.Enabled = false;
            TakeAuto.Enabled = true;
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void TakeAuto_Click(object sender, EventArgs e)
        {
            CaptureMode_SelectedIndexChanged(null, null);

            if (!m_bCaptureModeSelected)
            {
                MsgPanel.Text = "Capture mode isn't selected successfully";
                return;
            }

            if (m_captureMode == RealScanSDK.RS_CAPTURE_FLAT_MANUAL)
            {
                string msg = "Do you want to capture in custom size" + "(" + m_nCustomCaptW + "x" + m_nCustomCaptH + ")";
                DialogResult msgRet = MessageBox.Show(msg, "Custom Size Capture", MessageBoxButtons.YesNo);
                if (msgRet == DialogResult.No)
                    return;
                MsgPanel.Text = "Place fingers on the sensor";
            }
            else if (m_captureMode == RealScanSDK.RS_CAPTURE_ROLL_FINGER || m_captureMode == RealScanSDK.RS_CAPTURE_ROLL_FINGER_EX)
            {
                MsgPanel.Text = "Roll a finger on the sensor";
            }
            else
            {
                MsgPanel.Text = "Place fingers on the sensor";
            }
            SetPreview();
            this.autoCaptureThread = new Thread(new ThreadStart(this.DoautoCapture));

            this.autoCaptureThread.Start();
            StopCapture.Enabled = true;
        }

        private void DoautoCapture()
        {
            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                if (MsgPanel.InvokeRequired)
                {
                    afterAutoCaptureCallback callback = new afterAutoCaptureCallback(captureProcess);
                    try
                    {
                        this.Invoke(callback, m_result);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }   
                return;
            }

            m_result = RealScanSDK.RS_TakeImageData(deviceHandle, 10000, ref capturedImageData, ref capturedImageWidth, ref capturedImageHeight);

            //if (m_result != RealScanSDK.RS_SUCCESS)
            //{
            //    if (capturedImageData != (IntPtr)0)
            //    {
            //        RealScanSDK.RS_FreeImageData(capturedImageData);
            //    }
            //}

            if (MsgPanel.InvokeRequired)
            {
                afterAutoCaptureCallback callback = new afterAutoCaptureCallback(captureProcess);
                try
                {
                    this.Invoke(callback, m_result);
                }
                catch
                {
                }
            }
            else
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            return;
        }

        private void captureProcess(int captureResult)
        {
            if (captureResult == RealScanSDK.RS_SUCCESS)
            {
                MsgPanel.Text = "An image is captured successfully ";

                SaveSeqCheckTargetProcess(capturedImageData, capturedImageWidth, capturedImageHeight);
            }
            else
            {
                RealScanSDK.RS_GetErrString(captureResult, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

           

           
          


            if (Callback.SelectedIndex == (int)callbackMode.save) 
            {
                SaveImageCaptureProcess(capturedImageData, capturedImageWidth, capturedImageHeight);
            }
            else if (Callback.SelectedIndex == (int)callbackMode.save19794_4_2011)
            {
                SaveImage19794_4_2011_CaptureProcess(capturedImageData, capturedImageWidth, capturedImageHeight);
            }
                  
            else if (Callback.SelectedIndex == (int)callbackMode.enlarge) 
            {
                EnlargeCaptureProcess(capturedImageData, capturedImageWidth, capturedImageHeight);
            }
            else if ((Callback.SelectedIndex == (int)callbackMode.segment || Callback.SelectedIndex == (int)callbackMode.saveNseg))
            {
                if ( CaptureMode.SelectedIndex == 6 || CaptureMode.SelectedIndex == 7)
                {
                    MsgPanel.Text = "The command is not supported.";
                }
                else
                {
                    int numOfFingers = 0;
                    IntPtr[] ImageBuffer = new IntPtr[4];
                    int[] ImageWidth = new int[4];
                    int[] ImageHeight = new int[4];
                    RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

                    SegmentCaptureProcess(capturedImageData, capturedImageWidth, capturedImageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight);

                    if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                    {
                        SegmentSaveImageCaptureProcess(capturedImageData, capturedImageWidth, capturedImageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (ImageBuffer[i] != (IntPtr)0)
                        {
                            RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                        }
                    }
                }
            }
            else if (Callback.SelectedIndex == (int)callbackMode.seqCheck)
            {
                SequenceCheckProcess(capturedImageData, capturedImageWidth, capturedImageHeight);
            }

            if (capturedImageData != (IntPtr)0)
            {
                RealScanSDK.RS_FreeImageData(capturedImageData);
            }
        }

        private void takeManual()
        {
            CheckForIllegalCrossThreadCalls = false;

            int imageWidth = 0;
            int imageHeight = 0;
            IntPtr imageData = (IntPtr)0;

            
            m_result = RealScanSDK.RS_TakeCurrentImageData(deviceHandle, 5000, ref imageData, ref imageWidth, ref imageHeight);
           

            if (m_result != RealScanSDK.RS_SUCCESS && m_result != RealScanSDK.RS_WRN_TOO_POOR_QUALITY && m_result != RealScanSDK.RS_WRN_BAD_SCAN
                && m_result != RealScanSDK.RS_ERR_SEGMENT_FEWER_FINGER && m_result != RealScanSDK.RS_ERR_SEGMENT_WRONG_HAND)
            {
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                if (imageData != (IntPtr)0)
                {
                    RealScanSDK.RS_FreeImageData(imageData);
                }

                StartCapture.Enabled = true;
                StopCapture.Enabled = false;
                TakeManual.Enabled = false;
                TakeAuto.Enabled = true;
                return;
            }
            MsgPanel.Text = "An image is captured successfully";

            SaveSeqCheckTargetProcess(imageData, imageWidth, imageHeight);

            if (Callback.SelectedIndex == (int)callbackMode.save)
            {
                SaveImageCaptureProcess(imageData, imageWidth, imageHeight);
            }
            else if (Callback.SelectedIndex == (int)callbackMode.save19794_4_2011)
            {
                SaveImage19794_4_2011_CaptureProcess(imageData, imageWidth, imageHeight);
            }
            else if (Callback.SelectedIndex == (int)callbackMode.enlarge)
            {
                EnlargeCaptureProcess(imageData, imageWidth, imageHeight);
            }
            else if ((Callback.SelectedIndex == (int)callbackMode.segment || Callback.SelectedIndex == (int)callbackMode.saveNseg))
            {
                int numOfFingers = 0;
                IntPtr[] ImageBuffer = new IntPtr[4];
                int[] ImageWidth = new int[4];
                int[] ImageHeight = new int[4];
                RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

                SegmentCaptureProcess(imageData, imageWidth, imageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight);
                if (Callback.SelectedIndex == (int)callbackMode.saveNseg)
                {
                    SegmentSaveImageCaptureProcess(imageData, imageWidth, imageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);
                }
                for (int i = 0; i < 4; i++)
                {
                    if (ImageBuffer[i] != (IntPtr)0)
                    {
                        RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                    }
                }
            }
            else if (Callback.SelectedIndex == (int)callbackMode.seqCheck)
            {
                SequenceCheckProcess(imageData, imageWidth, imageHeight);
            }

            if (imageData != (IntPtr)0)
            {
                RealScanSDK.RS_FreeImageData(imageData);
            }
            StartCapture.Enabled = true;
            StopCapture.Enabled = false;
            TakeManual.Enabled = false;
            TakeAuto.Enabled = true;
        }

        private void TakeManual_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(this.takeManual));
            t.Start();
        }

        private void ResetLCD_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_ResetLCD(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void DisplayLCD_Click(object sender, EventArgs e)
        {
            Stream temp;
            OpenFileDialog openDialog = new OpenFileDialog();
            byte[] inputImage = new byte[RealScanSDK.RS_LCD_WIDTH_MAX * RealScanSDK.RS_LCD_HEIGHT_MAX];
            IntPtr outputImage = (IntPtr)0;

            openDialog.InitialDirectory = "c:\\";
            openDialog.Filter = "r08 files (*.r08)|*.r08";
            openDialog.FilterIndex = 1;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                temp = openDialog.OpenFile();
                if (temp != null)
                {
                    temp.Read(inputImage, 0, RealScanSDK.RS_LCD_WIDTH_MAX * RealScanSDK.RS_LCD_HEIGHT_MAX);

                    m_errorcode = RealScanSDK.RS_MakeLCDData(inputImage, inputImage, inputImage, RealScanSDK.RS_LCD_WIDTH_MAX, RealScanSDK.RS_LCD_HEIGHT_MAX, ref outputImage);
                    if (m_result == RealScanSDK.RS_SUCCESS)
                    {
                        m_result = RealScanSDK.RS_DisplayLCD(deviceHandle, outputImage, RealScanSDK.RS_LCD_DATA_SIZE_MAX, 0, 0,
                                                             RealScanSDK.RS_LCD_WIDTH_MAX, RealScanSDK.RS_LCD_HEIGHT_MAX);

                        if (m_result == RealScanSDK.RS_SUCCESS)
                        {
                            MsgPanel.Text = "Display LCD is successfully done!!";
                        }
                        else
                        {
                            RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                            MsgPanel.Text = m_errorMsg;
                        }
                    }
                    else
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        MsgPanel.Text = m_errorMsg;
                    }
                    temp.Close();
                }
            }
        }

        private void FingerLEDON_Click(object sender, EventArgs e)
        {
            int selFinger = Finger_LED.SelectedIndex;
            int selColor = FingerColor.SelectedIndex+1;

            if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_S60 || deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10 || deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10I)
            {
                int modeLedIndex; // 0=All, 1..4
                if (selFinger <= 4) modeLedIndex = selFinger;  
                else modeLedIndex = 0;                         

                
                int colorValueForMask = selColor; 
                                                 

               
                byte[] maskBytes = new byte[4]; 
                if (modeLedIndex == 0)
                {
                  
                    for (int i = 0; i < 4; i++) maskBytes[i] = (byte)colorValueForMask;
                }
                else
                {
                    int byteIndex = 4 - modeLedIndex;          // 1→3, 2→2, 3→1, 4→0
                    if (byteIndex < 0 || byteIndex > 3) byteIndex = 0;
                    maskBytes[byteIndex] = (byte)colorValueForMask;
                }
                int nModeLEDColorMask = BitConverter.ToInt32(maskBytes, 0);

                // 3-4) RS_SetModeLEDColor
                m_result = RealScanSDK.RS_SetModeLEDColor(deviceHandle, nModeLEDColorMask);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                    return;
                }

                // 3-5) RS_SetModeLED (On)
                m_result = RealScanSDK.RS_SetModeLED(deviceHandle, modeLedIndex, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                    return;
                }
            }
            else
            {
                int[] fingerMap = new int[] {
                    RealScanSDK.RS_FINGER_ALL,
                    RealScanSDK.RS_FINGER_LEFT_LITTLE,
                    RealScanSDK.RS_FINGER_LEFT_RING,
                    RealScanSDK.RS_FINGER_LEFT_MIDDLE,
                    RealScanSDK.RS_FINGER_LEFT_INDEX,
                    RealScanSDK.RS_FINGER_LEFT_THUMB,
                    RealScanSDK.RS_FINGER_RIGHT_THUMB,
                    RealScanSDK.RS_FINGER_RIGHT_INDEX,
                    RealScanSDK.RS_FINGER_RIGHT_MIDDLE,
                    RealScanSDK.RS_FINGER_RIGHT_RING,
                    RealScanSDK.RS_FINGER_RIGHT_LITTLE
                };

               
                int[] colorMapZeroBased = new int[] {
                    RealScanSDK.RS_LED_GREEN,
                    RealScanSDK.RS_LED_RED,
                    RealScanSDK.RS_LED_YELLOW
                };

               
                if (selFinger < 0 || selFinger >= fingerMap.Length) selFinger = 0;
                if (selColor < 0 || selColor >= colorMapZeroBased.Length) selColor = 0;

                int m_FingerLED = fingerMap[selFinger];
                int m_FingerColor = colorMapZeroBased[selColor];

               
                bool USE_ONE_BASED_COLOR = false; 
                if (USE_ONE_BASED_COLOR) m_FingerColor += 1;

                m_result = RealScanSDK.RS_SetFingerLED(deviceHandle, m_FingerLED, m_FingerColor);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }



        }

        private void FingerLEDOFF_Click(object sender, EventArgs e)
        {
            int selFinger = Finger_LED.SelectedIndex;
            if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_S60 || deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10 || deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10I)
            {
               
                int modeLedIndex = (selFinger >= 0 && selFinger <= 4) ? selFinger : 0;

                m_result = RealScanSDK.RS_SetModeLED(deviceHandle, modeLedIndex, false);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                    return;
                }

            }
            else
            {
                int m_FingerLED = Finger_LED.SelectedIndex;
                int[] Fingerbuffer = new int[11] { RealScanSDK.RS_FINGER_ALL, RealScanSDK.RS_FINGER_LEFT_LITTLE, RealScanSDK.RS_FINGER_LEFT_RING,
                                                    RealScanSDK.RS_FINGER_LEFT_MIDDLE, RealScanSDK.RS_FINGER_LEFT_INDEX, RealScanSDK.RS_FINGER_LEFT_THUMB,
                                                    RealScanSDK.RS_FINGER_RIGHT_RING, RealScanSDK.RS_FINGER_RIGHT_LITTLE, RealScanSDK.RS_FINGER_TWO_THUMB,
                                                    RealScanSDK.RS_FINGER_LEFT_FOUR, RealScanSDK.RS_FINGER_RIGHT_FOUR };
                for (int i = 0; i < 3; i++)
                {
                    if (i == Finger_LED.SelectedIndex)
                    {
                        m_FingerLED = Fingerbuffer[i];
                    }
                }

                m_result = RealScanSDK.RS_SetFingerLED(deviceHandle, m_FingerLED, RealScanSDK.RS_LED_OFF);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }

        }
        private void ModeLEDOff_Click(object sender, EventArgs e)
        {
            int LEDModeType = 0;
            int[] LEDModeTypeBuffer = new int[6]
            {
                RealScanSDK.RS_LED_MODE_ALL,
                RealScanSDK.RS_LED_MODE_LEFT_FINGER4,
                RealScanSDK.RS_LED_MODE_RIGHT_FINGER4,
                RealScanSDK.RS_LED_MODE_TWO_THUMB,
                RealScanSDK.RS_LED_MODE_ROLL,
                RealScanSDK.RS_LED_POWER
            };

            
            if (Mode_LED.SelectedIndex >= 0 && Mode_LED.SelectedIndex < LEDModeTypeBuffer.Length)
            {
                LEDModeType = LEDModeTypeBuffer[Mode_LED.SelectedIndex];
            }
            else
            {
                MsgPanel.Text = "The selected mode is invalid";
                return;
            }

           
            m_result = RealScanSDK.RS_SetModeLED(deviceHandle, LEDModeType, false);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void TestRun_Click(object sender, EventArgs e)
        {
            int TestTypes = 0;
            int[] TestTypeBuffer = new int[2] { RealScanSDK.RS_SELFTEST_ILLUMINATION, RealScanSDK.RS_SELFTEST_DIRT };

            for (int i = 0; i < 2; i++)
            {
                if (i == TestType.SelectedIndex)
                {
                    TestTypes = TestTypeBuffer[i];
                }
            }

            m_result = RealScanSDK.RS_SelfTest(deviceHandle, TestTypes);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            else
            {
                MsgPanel.Text = "Device OK ";
            }
        }

        private void ManualCalibration_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_Calibrate(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            else
            {
                MsgPanel.Text = "Calibration succeed.";
            }
        }

        private void Emitbeep_Click(object sender, EventArgs e)
        {
            int Beeptype = 0;
            byte[] Beepbuffer = new byte[3] { RealScanSDK.RS_BEEP_PATTERN_NONE, RealScanSDK.RS_BEEP_PATTERN_1, RealScanSDK.RS_BEEP_PATTERN_2 };

            for (int i = 0; i < 3; i++)
            {
                if (i == BeepPattern.SelectedIndex)
                {
                    Beeptype = Beepbuffer[i];
                }
            }

            m_result = RealScanSDK.RS_Beep(deviceHandle, Beeptype);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void PlayWav_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            string filename = " ";

            if (dialogResult == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                if (filename == String.Empty)
                {
                    return;
                }
            }

            m_result = RealScanSDK.RS_PlayWav(deviceHandle, filename);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ReadRollOption_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_GetRollFingerOption(deviceHandle, ref m_rollDirection, ref m_rollTime, ref  m_rollProfile);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            RollDirections.SelectedIndex = m_rollDirection;
            TimeoutTextBox.Text = Convert.ToString(m_rollTime);
            RollProfiles.SelectedIndex = m_rollProfile - 1;
        }

        private void ShowTextOverlay_Click(object sender, EventArgs e)
        {
            RealScan.RSOverlayText text = new RSOverlayText();

            if (m_TextX.Text == "" || m_TextY.Text == "")
            {
                MessageBox.Show("Please set all the text boxes filled");
                
                return;
            }
            
            text.pos.x = Convert.ToInt32(m_TextX.Text);
            text.pos.y = Convert.ToInt32(m_TextY.Text);
            text.alignment = SelectTextAlign.SelectedIndex;
            int[] FontSizebuffer = new int[9] { 8, 10, 12, 14, 16, 18, 20, 24, 28 };
            ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };

            for (int i = 0; i < 4; i++)
            {
                if (i == SelectTextColor.SelectedIndex)
                {
                    text.color = ColorBuffer[i];
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (i == SelectTextSize.SelectedIndex)
                {
                    text.fontSize = FontSizebuffer[i];
                }
            }

            int strLength = m_Text.Text.Length;

            byte[] textbuffer = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(m_Text.Text.ToCharArray()));
            int len = m_Text.Text.Length;
            byte[] textbuffer2 = new byte[128];
            textbuffer2[len] = 0;
            System.Buffer.BlockCopy(textbuffer, 0, textbuffer2, 0, len);
            text.text = textbuffer2;

            Font GetFontInfo = new Font("System", text.fontSize);
            int FontSize = Convert.ToInt32(GetFontInfo.Size);
            int FontLength = GetFontInfo.Name.Length;
            string GetFontNameBuffer = GetFontInfo.Name;
            byte[] Fontbuffer = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(GetFontNameBuffer));
            byte[] FontNames = new byte[32];
            System.Buffer.BlockCopy(Fontbuffer, 0, FontNames, 0, FontLength);
            text.fontName = FontNames;

            m_result = RealScanSDK.RS_AddOverlayText(deviceHandle, ref text, ref m_overlayHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void StatusLEDOn_Click(object sender, EventArgs e)
        {

            int sel = StatusLEDColor.SelectedIndex;
            if (sel < 0) sel = 0;
            int colorParam;

            if (deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_S60 ||
                deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10 ||
                deviceType_ == RealScanSDK.RS_DEVICE_REALSCAN_SG10I)
            {
                // MFC: RS_SetStatusLED(handle, m_StatusLEDColor)
                colorParam = sel; // 0-based 그대로
            }
            else
            {
                // MFC: RS_SetStatusLED(handle, m_StatusLEDColor + 1)
                colorParam = sel + 1; // 1-based로 보정
            }



            m_result = RealScanSDK.RS_SetStatusLED(deviceHandle, colorParam);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

        }
        private void WriteOtherOption_Click(object sender, EventArgs e)
        {
            bool m_AutoCalibrate = AutoCalibrate.Checked;
            bool m_AdvContrast = AdvancedContrastEnhancement.Checked;
            bool m_ContrastEnhancement = ContrastEnhancement.Checked;
            bool m_NoiseReduction = NoiseReduction.Checked;
            int m_AutoSensitivity = 0x00;
            int m_ReductionLevel = 0;

            int[] SensitivityBuffer = new int[4] {RealScanSDK.RS_AUTO_SENSITIVITY_NORMAL,RealScanSDK.RS_AUTO_SENSITIVITY_HIGH,
                                                  RealScanSDK.RS_AUTO_SENSITIVITY_HIGHER,RealScanSDK. RS_AUTO_SENSITIVITY_DISABLED};
            for (int i = 0; i < 4; i++)
            {
                if (i == AutoSensitivity.SelectedIndex)
                {
                    m_AutoSensitivity = SensitivityBuffer[i];
                }
            }

            try
            {
                m_ReductionLevel = Convert.ToInt32(ReductionLevel.Text);
            }
            catch (InvalidCastException)
            {
                return;
            }
            if(m_captureMode != RealScanSDK.RS_CAPTURE_FLAT_MANUAL)
            {
                m_result = RealScanSDK.RS_SetCaptureMode(deviceHandle, m_mode, m_AutoSensitivity, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
           

            m_result = RealScanSDK.RS_SetAutomaticCalibrate(deviceHandle, m_AutoCalibrate);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_SetAdvancedContrastEnhancement(deviceHandle, m_AdvContrast);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_SetPostProcessingEx(deviceHandle, m_ContrastEnhancement, m_NoiseReduction, m_ReductionLevel);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ReadOtherOption_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref m_mode, ref m_option);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            AutoSensitivity.SelectedIndex = m_option;

            m_result = RealScanSDK.RS_GetAutomaticCalibrate(deviceHandle, ref m_automatic);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            AutoCalibrate.Checked = m_automatic;

            m_result = RealScanSDK.RS_GetAdvancedContrastEnhancement(deviceHandle, ref m_advancedContrast);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            AdvancedContrastEnhancement.Checked = m_advancedContrast;

            m_result = RealScanSDK.RS_GetPostProcessingEx(deviceHandle, ref m_contrastEnhancement, ref m_noiseReduction, ref m_reductionLevel);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            ContrastEnhancement.Checked = m_contrastEnhancement;
            NoiseReduction.Checked = m_noiseReduction;
            ReductionLevel.Text = Convert.ToString(m_reductionLevel);
        }

        private void DeviceTab_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics TabText = e.Graphics;
            Font TabTextFont = new Font(e.Font, FontStyle.Underline);

            StringFormat StringStyle = new StringFormat();
            StringStyle.Alignment = StringAlignment.Center;
            StringStyle.LineAlignment = StringAlignment.Center;

            TabText.DrawString("Device", TabTextFont, Brushes.Black, this.DeviceTab.GetTabRect(0), StringStyle);
            TabText.DrawString("I/O", TabTextFont, Brushes.Black, this.DeviceTab.GetTabRect(1), StringStyle);
            TabText.DrawString("Option", TabTextFont, Brushes.Black, this.DeviceTab.GetTabRect(2), StringStyle);
            TabText.DrawString("Overlay", TabTextFont, Brushes.Black, this.DeviceTab.GetTabRect(3), StringStyle);
            TabText.DrawString("Callback Option", TabTextFont, Brushes.Black, this.DeviceTab.GetTabRect(4), StringStyle);
        }

        private void WriteRollOption_Click(object sender, EventArgs e)
        {
            int RollProfile = 0x00;
            int RollDirection = 0x00;
            int RollTimeout = Convert.ToInt32(TimeoutTextBox.Text);

            int[] RollProfileBuffer = new int[3] { RealScanSDK.RS_ROLL_PROFILE_LOW, RealScanSDK.RS_ROLL_PROFILE_NORMAL, RealScanSDK.RS_ROLL_PROFILE_HIGH };
            for (int i = 0; i < 3; i++)
            {
                if (i == RollProfiles.SelectedIndex)
                {
                    RollProfile = RollProfileBuffer[i];
                }
            }

            int[] RollDirectionsBuffer = new int[4] { RealScanSDK.RS_ROLL_DIR_L2R, RealScanSDK.RS_ROLL_DIR_R2L, RealScanSDK.RS_ROLL_DIR_AUTO, RealScanSDK.RS_ROLL_DIR_AUTO_M };
            for (int i = 0; i < 3; i++)
            {
                if (i == RollDirections.SelectedIndex)
                {
                    RollDirection = RollDirectionsBuffer[i];
                }
            }
            m_result = RealScanSDK.RS_SetRollFingerOption(deviceHandle, RollDirection, RollTimeout, RollProfile);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ReadKeycallback_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_GetKeyStatus(deviceHandle, ref m_keyCode);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                KEYSTATUS.Text = " ";
                return;
            }
            KEYSTATUS.Text = Convert.ToString(m_keyCode);
        }

        private void keypadCallback(int deviceHandle, uint keyCode)
        {
            if (KEYCALLBACK.SelectedIndex == 1)
            {
                CALLBACKPRINT.Text = "Key Code Read:" + keyCode;
            }
            else if (KEYCALLBACK.SelectedIndex == 2)
            {
                if (keyCode == 0x20)
                {
                    RealScanSDK.RS_Beep(deviceHandle, RealScanSDK.RS_BEEP_PATTERN_1);
                }
                else
                {
                    RealScanSDK.RS_Beep(deviceHandle, RealScanSDK.RS_BEEP_PATTERN_2);
                }
            }
            this.Update();
        }

        private void keypadCallbackInt(int deviceHandle, uint keyCode)
        {
            RSKeypadCallbackInt callback = new RSKeypadCallbackInt(keypadCallback);
            Invoke(callback, deviceHandle, keyCode);
        }

        private void ActiveKey_Click(object sender, EventArgs e)
        {
            uint m_keyMAsk = 0;

            RSKeypadCallback keypadCallback = new RSKeypadCallback(keypadCallbackInt);
            uint[] keymaskbuffer = new uint[9] { RealScanSDK.RS_REALSCANF_NO_KEY, RealScanSDK.RS_REALSCANF_UP_KEY, RealScanSDK.RS_REALSCANF_DOWN_KEY, 
                                                 RealScanSDK.RS_REALSCANF_LEFT_KEY, RealScanSDK.RS_REALSCANF_RIGHT_KEY, RealScanSDK.RS_REALSCANF_PLAY_KEY, 
                                                 RealScanSDK.RS_REALSCANF_STOP_KEY, RealScanSDK.RS_REALSCANF_FOOTSWITCH, RealScanSDK.RS_REALSCANF_ALL_KEYS };
            for (int i = 0; i < 9; i++)
            {
                if (i == KeyMask.SelectedIndex)
                {
                    m_keyMAsk = keymaskbuffer[i];
                }
            }
            m_result = RealScanSDK.RS_SetActiveKey(deviceHandle, m_keyMAsk);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            RealScanSDK.RS_RegisterKeypadCallback(deviceHandle, keypadCallback);
        }

        private void ShowLineOverlay_Click(object sender, EventArgs e)
        {
            RealScan.RSOverlayLine line = new RSOverlayLine();

            if (m_LineX1.Text == "" || m_LineY1.Text == "" || m_LineX2.Text == "" || m_LineY2.Text == "" || m_LineWidth.Text == "")
            {
                MessageBox.Show("Please set all the text boxes filled");

                return;
            }

            line.startPos.x = Convert.ToInt32(m_LineX1.Text);
            line.startPos.y = Convert.ToInt32(m_LineY1.Text);
            line.endPos.x = Convert.ToInt32(m_LineX2.Text);
            line.endPos.y = Convert.ToInt32(m_LineY2.Text);
            line.width = Convert.ToInt32(m_LineWidth.Text);

            ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
            for (int i = 0; i < 4; i++)
            {
                if (i == SelectLineColor.SelectedIndex)
                {
                    line.color = ColorBuffer[i];
                }
            }

            m_result = RealScanSDK.RS_AddOverlayLine(deviceHandle, ref line, ref m_overlayHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ShowQuardangle_Click(object sender, EventArgs e)
        {
            int overlayHandle = -1;
            RealScan.RSOverlayQuadrangle quad = new RSOverlayQuadrangle();
            quad.pos = new RSPoint[4];

            if (m_QuadX1.Text == "" || m_QuadY1.Text == "" || m_QuadX2.Text == "" || m_QuadY2.Text == "" || 
                m_QuadX3.Text == "" || m_QuadY3.Text == "" || m_QuadX4.Text == "" || m_QuadY4.Text == "" ||
                m_QuadWidth.Text == "")
            {
                MessageBox.Show("Please set all the text boxes filled");

                return;
            }

            quad.pos[0].x = Convert.ToInt32(m_QuadX1.Text);
            quad.pos[0].y = Convert.ToInt32(m_QuadY1.Text);
            quad.pos[1].x = Convert.ToInt32(m_QuadX2.Text);
            quad.pos[1].y = Convert.ToInt32(m_QuadY2.Text);
            quad.pos[2].x = Convert.ToInt32(m_QuadX3.Text);
            quad.pos[2].y = Convert.ToInt32(m_QuadY3.Text);
            quad.pos[3].x = Convert.ToInt32(m_QuadX4.Text);
            quad.pos[3].y = Convert.ToInt32(m_QuadY4.Text);

            quad.width = Convert.ToInt32(m_QuadWidth.Text);

            ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
            for (int i = 0; i < 4; i++)
            {
                if (i == SelectQuadColor.SelectedIndex)
                {
                    quad.color = ColorBuffer[i];
                }
            }

            m_result = RealScanSDK.RS_AddOverlayQuadrangle(deviceHandle, ref quad, ref overlayHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            m_result = RealScanSDK.RS_ShowOverlay(overlayHandle, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ShowCrossOverlay_Click(object sender, EventArgs e)
        {
            RealScan.RSOverlayCross cross = new RSOverlayCross();

            if (m_CrossX.Text == "" || m_CrossY.Text == "" || m_CrossDX.Text == "" || m_CrossDY.Text == "" || m_CrossWidth.Text == "")
            {
                MessageBox.Show("Please set all the text boxes filled");

                return;
            }

            cross.centerPos.x = Convert.ToInt32(m_CrossX.Text);
            cross.centerPos.y = Convert.ToInt32(m_CrossY.Text);

            ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
            for (int i = 0; i < 4; i++)
            {
                if (i == SelectCrossColor.SelectedIndex)
                {
                    cross.color = ColorBuffer[i];
                }
            }

            cross.rangeX = Convert.ToInt32(m_CrossDX.Text);
            cross.rangeY = Convert.ToInt32(m_CrossDY.Text);
            cross.width = Convert.ToInt32(m_CrossWidth.Text);

            m_result = RealScanSDK.RS_AddOverlayCross(deviceHandle, ref cross, ref m_overlayHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void HideAll_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_ShowAllOverlay(deviceHandle, false);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_ShowAllOverlay(deviceHandle, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            MsgPanel.Text = "Clear the Preview Window";
        }

        private void cbLFDLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nRetVal = RealScanSDK.RS_SetLFDLevel(deviceHandle, cbLFDLevel.SelectedIndex);
            if (nRetVal != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(nRetVal, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg; 
                return; 
            }

        }

        private void SetLFDLevel_Click(object sender, EventArgs e)
        {
            int nRetVal = RealScanSDK.RS_ERR_UNKNOWN;
            nRetVal = RealScanSDK.RS_SetLFDLevel(deviceHandle, cbLFDLevel.SelectedIndex);
            if (nRetVal != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(nRetVal, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }
        }

        private void ReadLFDLevel_Click(object sender, EventArgs e)
        {
            int nRetVal = RealScanSDK.RS_ERR_UNKNOWN;
            int nLFDLevel = 0;
            nRetVal = RealScanSDK.RS_GetLFDLevel(deviceHandle, ref nLFDLevel);
            if (nRetVal != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(nRetVal, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
                return;
            }

            cbLFDLevel.SelectedIndex = nLFDLevel;
        }

        private void DrawOverlay_Click(object sender, EventArgs e)
        {
            RSRect rect = new RSRect();
            RealScanSDK.GetClientRect(PreviewWindow.Handle, ref rect);

            if (OverlayType.SelectedIndex == 0)
            {
                RealScan.RSOverlayText text = new RSOverlayText();
                text.pos.x = 0;
                text.pos.y = 0;
                text.alignment = RealScanSDK.RS_TEXT_ALIGN_LEFT;
                text.fontSize = 24;

                ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
                for (int i = 0; i < 4; i++)
                {
                    if (i == OverlayColor.SelectedIndex)
                    {
                        text.color = ColorBuffer[i];
                    }
                }

                string ShowText = "Test Message";
                byte[] textbuffer2 = new byte[128];
                textbuffer2 = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(ShowText));
                byte[] textbuffer3 = new byte[128];
                int Length = ShowText.Length;
                System.Buffer.BlockCopy(textbuffer2, 0, textbuffer3, 0, Length);
                text.text = textbuffer3;

                Font GetFontInfo = new Font("System", 24);
                int FontLength = GetFontInfo.Name.Length;
                string GetFontNameBuffer = GetFontInfo.Name;
                byte[] Fontbuffer = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(GetFontNameBuffer));
                byte[] FontNames = new byte[32];
                System.Buffer.BlockCopy(Fontbuffer, 0, FontNames, 0, FontLength);
                text.fontName = FontNames;

                m_result = RealScanSDK.RS_AddOverlayText(deviceHandle, ref text, ref m_overlayHandle);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
            else if (OverlayType.SelectedIndex == 1)
            {
                RealScan.RSOverlayCross cross = new RSOverlayCross();
                cross.centerPos.x = (rect.right - rect.left) / 2;
                cross.centerPos.y = (rect.bottom - rect.top) / 2;
                cross.rangeX = 10;
                cross.rangeY = 10;
                cross.width = 5;

                ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
                for (int i = 0; i < 4; i++)
                {
                    if (i == OverlayColor.SelectedIndex)
                    {
                        cross.color = ColorBuffer[i];
                    }
                }

                m_result = RealScanSDK.RS_AddOverlayCross(deviceHandle, ref cross, ref m_overlayHandle);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
            else if (OverlayType.SelectedIndex == 2)
            {
                RealScan.RSOverlayLine line = new RSOverlayLine();

                line.startPos.x = rect.left;
                line.startPos.y = (rect.bottom - rect.top) / 2;
                line.endPos.x = rect.right;
                line.endPos.y = (rect.bottom - rect.top) / 2;
                line.width = 5;

                ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
                for (int i = 0; i < 4; i++)
                {
                    if (i == OverlayColor.SelectedIndex)
                    {
                        line.color = ColorBuffer[i];
                    }
                }

                m_result = RealScanSDK.RS_AddOverlayLine(deviceHandle, ref line, ref m_overlayHandle);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                m_result = RealScanSDK.RS_ShowOverlay(m_overlayHandle, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
            else if (OverlayType.SelectedIndex == 3)
            {
                int overlayHandle = -1;
                RealScan.RSOverlayQuadrangle quad = new RSOverlayQuadrangle();
                quad.pos = new RSPoint[4];

                quad.pos[0].x = rect.left;
                quad.pos[0].y = rect.top;
                quad.pos[1].x = rect.right - 1;
                quad.pos[1].y = rect.top;
                quad.pos[2].x = rect.right - 1;
                quad.pos[2].y = rect.bottom - 1;
                quad.pos[3].x = rect.left;
                quad.pos[3].y = rect.bottom - 1;

                quad.width = 5;

                ulong[] ColorBuffer = new ulong[4] { 0x00000000, 0x000000ff, 0x0000ff00, 0x00ff0000 };
                for (int i = 0; i < 4; i++)
                {
                    if (i == OverlayColor.SelectedIndex)
                    {
                        quad.color = ColorBuffer[i];
                    }
                }

                m_result = RealScanSDK.RS_AddOverlayQuadrangle(deviceHandle, ref quad, ref overlayHandle);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
                m_result = RealScanSDK.RS_ShowOverlay(overlayHandle, true);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    MsgPanel.Text = m_errorMsg;
                }
            }
        }

        private void ClearAllOverlays_Click(object sender, EventArgs e)
        {
            int result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void StatusLEDOff_Click(object sender, EventArgs e)
        {
            int result = RealScanSDK.RS_SetStatusLED(deviceHandle, RealScanSDK.RS_LED_OFF);

            if (result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void ModeLEDOn_Click(object sender, EventArgs e)
        {
            int LEDModeType = 0;
            int[] LEDModeTypeBuffer = new int[6]  {RealScanSDK.RS_LED_MODE_ALL,RealScanSDK.RS_LED_MODE_LEFT_FINGER4,RealScanSDK.RS_LED_MODE_RIGHT_FINGER4,
                                                      RealScanSDK.RS_LED_MODE_TWO_THUMB,RealScanSDK.RS_LED_MODE_ROLL,RealScanSDK.RS_LED_POWER};
            for (int i = 0; i < 6; i++)
            {
                if (i == Mode_LED.SelectedIndex)
                {
                    LEDModeType = LEDModeTypeBuffer[i];
                }
            }
            m_result = RealScanSDK.RS_SetModeLED(deviceHandle, LEDModeType, true);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
        }

        private void selMinFingerCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(selMinFingerCount.SelectedIndex < 0 )
            {
                return;
            }

            m_minCount = 4 - selMinFingerCount.SelectedIndex;

            m_result = RealScanSDK.RS_SetMinimumFinger(deviceHandle, m_minCount);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MsgPanel.Text = m_errorMsg;
            }
            else
                MsgPanel.Text = "Setting the minimum finger count is done successfully";
        }

        private void selCaptureDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_captureDir = selCaptureDir.SelectedIndex;
        }

        private void directDrawBtn_CheckedChanged(object sender, EventArgs e)
        {

            var rb = (RadioButton)sender;
            if (!rb.Checked)
                return;

            RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (!m_prevStopped)
            {


               
                if (_selectedPrevMode == PrevMode.callbackDraw)
                {
                    m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, null);

                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);

                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }
               
                else if (_selectedPrevMode == PrevMode.advCallbackDraw)
                {

                    m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, null);

                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);

                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }
                _selectedPrevMode = PrevMode.directDraw;

                PreviewWindow.Image = null;
                PreviewWindow.Invalidate();
                
                SetPreview();
            }
            else
            {


                m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, null);
                m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, null);


                _selectedPrevMode = PrevMode.directDraw;
                PreviewWindow.Image = null;
                PreviewWindow.Invalidate();
                SetPreview();

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            m_strLicenseFile = path + "\\LICENSE.TXT";

            FileInfo fi = new FileInfo(m_strLicenseFile);
            
            if (fi.Exists) {
                this.brnViewSoftwareLicense.Show();    
            }
            else {
                this.brnViewSoftwareLicense.Hide();
            }
        }

        private void btnViewSoftwareLicense_Click(object sender, EventArgs e)
        {
            Process.Start(m_strLicenseFile);
        }


        private void callbackDrawBtn_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;

           
            if (!rb.Checked)
                return;
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw)
                {
                    m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }
                else if (_selectedPrevMode == PrevMode.advCallbackDraw)
                {
                    m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, null);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }

                m_prevStopped = true;
            }
            PreviewWindow.Image = null;
            PreviewWindow.Invalidate();
            _selectedPrevMode = PrevMode.callbackDraw;
            SetPreview();
        }

        private void advCallbackDrawBtn_CheckedChanged(object sender, EventArgs e)
        {

            var rb = (RadioButton)sender;

           
            if (!rb.Checked)
                return;
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw)
                {
                    m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }
                else if (_selectedPrevMode == PrevMode.callbackDraw)
                {
                    m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, null);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        MsgPanel.Text = m_errorMsg;
                        return;
                    }
                }

             }
            PreviewWindow.Image = null;
            PreviewWindow.Invalidate();
            _selectedPrevMode = PrevMode.advCallbackDraw;
            SetPreview();
        }

        private void SetPreview()
        {

            switch (this._selectedPrevMode)
            {
                case PrevMode.directDraw:


                    RSRect rect = new RSRect();
                    RealScanSDK.GetClientRect(PreviewWindow.Handle, ref rect);


                    m_result = RealScanSDK.RS_SetViewWindow(this.deviceHandle, PreviewWindow.Handle, rect, false);


                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);

                        MsgPanel.Text = m_errorMsg;
                        return;
                    }

                    m_prevStopped = false;

                    ClearAllOverlays.Enabled = true;
                    DrawOverlay.Enabled = true;
                    PreviewWindow.Show();
                    PreviewWindow.Invalidate();
                    break;

                case PrevMode.callbackDraw:

                    m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, previewCallback);

                    break;

                case PrevMode.advCallbackDraw:

                    m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, advPreviewCallback);

                    break;
            }


        }

        public void log(string msg)
        {
            CheckForIllegalCrossThreadCalls = false;
            logBox.AppendText("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + msg + "\n");
        }
    }

}


