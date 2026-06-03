using RealScan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xperix_442Rolled_Images2NIST
{
    public partial class frmMain : Form
    {

        Bitmap[] bitmap = new Bitmap[16];

        public frmMain()
        {
            InitializeComponent();
            cbNISTVersion.SelectedIndex = 2;
            btnRefresh_Click(null, null);
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //Get_NFIQ2_Quality();
            pbLeftSlap.Image = null;
            pbTwoThumbs.Image = null;
            pbRightSlap.Image = null;
            pbRightThumb.Image = pbRightIndex.Image = pbRightMiddle.Image = pbRightRing.Image = pbRightLittle.Image = null;
            pbLeftThumb.Image = pbLeftIndex.Image = pbLeftMiddle.Image = pbLeftRing.Image = pbLeftLittle.Image = null;
            txtUID.Text = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            txtName.Text = "";
            for (int i = 0; i < 16; i++)
            {
                bitmap[i] = null;
            }
            btnCreateNIST.Enabled = false;

        }


        public void WriteLog(string log)
        {
            try
            {
                if (log != "" && !log.StartsWith("-"))
                {
                    string log_location = Application.StartupPath + "\\Logs";
                    bool flag = !Directory.Exists(log_location);
                    if (flag)
                        Directory.CreateDirectory(log_location);
                    string log_file_location = Application.StartupPath + "\\Logs\\" + txtUID.Text + ".txt";
                    File.AppendAllText(log_file_location, Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + log);
                }
            }
            catch
            {

            }
        }

        public void Load_Fingerprint(int idx)
        {
            bitmap[idx] = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Application.StartupPath;
            ofd.Filter = "Bitmap files (*.bmp)|*.bmp";
            ofd.FilterIndex = 1;
            ofd.Title = "Select a Bitmap File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                bitmap[idx]  = new Bitmap(ofd.FileName);

                if (idx == 1)
                    pbRightThumb.Image = Image.FromFile(ofd.FileName);
                if (idx == 2)
                    pbRightIndex.Image = Image.FromFile(ofd.FileName);
                if (idx == 3)
                    pbRightMiddle.Image = Image.FromFile(ofd.FileName);
                if (idx == 4)
                    pbRightRing.Image = Image.FromFile(ofd.FileName);
                if (idx == 5)
                    pbRightLittle.Image = Image.FromFile(ofd.FileName);
                if (idx == 6)
                    pbLeftThumb.Image = Image.FromFile(ofd.FileName);
                if (idx == 7)
                    pbLeftIndex.Image = Image.FromFile(ofd.FileName);
                if (idx == 8)
                    pbLeftMiddle.Image = Image.FromFile(ofd.FileName);
                if (idx == 9)
                    pbLeftRing.Image = Image.FromFile(ofd.FileName);
                if (idx == 10)
                    pbLeftLittle.Image = Image.FromFile(ofd.FileName);

                if (idx == 13)
                    pbRightSlap.Image = Image.FromFile(ofd.FileName);
                if (idx == 14)
                    pbLeftSlap.Image = Image.FromFile(ofd.FileName);
                if (idx == 15)
                    pbTwoThumbs.Image = Image.FromFile(ofd.FileName);

                btnCreateNIST.Enabled = true;
            }
        }

        private byte[] ConvertBitmap(Bitmap bmp)
        {

            // Ensure image is 8bpp grayscale
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new InvalidOperationException("Image must be 8-bit grayscale (Format8bppIndexed).");
            }

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            int stride = bmpData.Stride;
            int height = bmp.Height;
            int width = bmp.Width;

            byte[] rawData = new byte[width * height];

            // Copy pixel data line by line, accounting for stride
            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        rawData[y * width + x] = ptr[y * stride + x];
                    }
                }
            }

            bmp.UnlockBits(bmpData);

            return rawData;
        }
        private void SaveImageAnsiNist()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Name");
                txtName.Focus();
                return;
            }


            int nSize = Marshal.SizeOf(typeof(RSEType1Info));
            IntPtr ptr = Marshal.AllocHGlobal(nSize);

            int NIST_VERSION = cbNISTVersion.SelectedIndex;

            int result = RS_AN2K11.RSE_SetStandard(NIST_VERSION);
            if (result != RealScanSDK.RS_SUCCESS)
            {

                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                return;
            }
            result = RS_AN2K11.RSE_SetType1Info("CPS", 4, "KR/KOR1", "KR/KOR2", "0747439609K", "TCR", ptr);
            if (result != RealScanSDK.RS_SUCCESS)
            {

                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                return;
            }
            result = RS_AN2K11.RSE_CreateFileCS(ptr);
            if (result != RealScanSDK.RS_SUCCESS)
            {

                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                return;
            }
            RSEType1Info type1Info = new RSEType1Info();
            type1Info = (RSEType1Info)Marshal.PtrToStructure(ptr, typeof(RSEType1Info));

            Marshal.FreeHGlobal(ptr);

            //
            // make type 2 record 
            //
            RSEType2Info type2Info = new RSEType2Info();

            type2Info.numCustomFields = 2;
            type2Info.customFields = new RSEType2CustomField[500];
            type2Info.customFields[0] = new RSEType2CustomField();
            type2Info.customFields[1] = new RSEType2CustomField();
            type2Info.customFields[0].fieldID = 5;
            type2Info.customFields[0].fieldValue = txtUID.Text.Trim();
            type2Info.customFields[1].fieldID = 6;
            type2Info.customFields[1].fieldValue = txtName.Text.Trim();

            int IDC = 0;
            type2Info.IDC = IDC++;

            int type2InfoSize = Marshal.SizeOf(typeof(RSEType2Info));
            IntPtr type2InfoPtr = Marshal.AllocHGlobal(type2InfoSize);
            Marshal.StructureToPtr(type2Info, type2InfoPtr, false);

            result = RS_AN2K11.RSE_CreateType2RecordCS(type2InfoPtr);

            Marshal.FreeHGlobal(type2InfoPtr);

            if (result != RealScanSDK.RS_SUCCESS)
            {
                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                RS_AN2K11.RSE_FreeFileCS();
                return;
            }

            result = RS_AN2K11.RSE_AppendRecordCS();

            if (result != RealScanSDK.RS_SUCCESS)
            {
                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                RS_AN2K11.RSE_FreeRecordCS();
                RS_AN2K11.RSE_FreeFileCS();

                return;
            }



            for (int idx = 1; idx < 16; idx++)
            {
                int finger_position = idx;
                if (bitmap[idx] != null)
                {

                    byte[] rawdata = ConvertBitmap(bitmap[idx]);
                    int Width = bitmap[idx].Width;
                    int Height = bitmap[idx].Height;
                    IntPtr ImageBuffer;
                    ImageBuffer = Marshal.AllocHGlobal(Width * Height);
                    Marshal.Copy(rawdata, 0, ImageBuffer, Width * Height);

                    if (NIST_VERSION > 1)    // for NIST 2011, 2011:2013 and 2011:2015
                        processRecordType14(type1Info, ref IDC, ImageBuffer, Width, Height, finger_position);
                    else
                        processRecordType4(type1Info, ref IDC, ImageBuffer, Width, Height, finger_position);

                }
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = ".:\\";
            saveDialog.Filter = "nist files (*.nist)|*.nist|All files (*.*)|*.*";
            saveDialog.FilterIndex = 1;
            saveDialog.DefaultExt = "nist";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                result = RS_AN2K11.RSE_WriteFileCS(saveDialog.FileName);
                if (result != RealScanSDK.RS_SUCCESS)
                {
                    string m_errorMsg = "";
                    RealScanSDK.RS_GetErrString(result, ref m_errorMsg);
                    MessageBox.Show(m_errorMsg.ToString());
                    RS_AN2K11.RSE_FreeFileCS();
                    return;
                }
                saveDialog.Dispose();
                MessageBox.Show("NIST File Create Successfully", "Success");
            }
            RS_AN2K11.RSE_FreeFileCS();

        }

        private void processRecordType14(RSEType1Info type1Info, ref int IDC, IntPtr imageData, int imageWidth, int imageHeight, int finger_position)
        {

            int result = 0;

            // 
            // Make type 14 record

            RSEType14Info recordInfo = new RSEType14Info();

            if (finger_position > 10)
                recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_PLAIN;
            else
                recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_ROLLED;

            recordInfo.sourceAgency = type1Info.originatingAgencyIdentifier;
            recordInfo.captureDate = type1Info.date;

            recordInfo.IDC = IDC++;
            int slapType = RealScanSDK.RS_SLAP_ONE_FINGER_ROLL;
            switch (finger_position)
            {
                case 15:
                    slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                    break;

                case 14:
                    slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                    break;

                case 13:
                    slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
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

            int m_result = RealScanSDK.RS_Segment4(imageData, imageWidth, imageHeight, slapType, ref numOfFingers, ref slapInfoArray, ref ImageBuffer[0], ref ImageWidth[0],
                                            ref ImageHeight[0], ref ImageBuffer[1], ref ImageWidth[1], ref ImageHeight[1], ref ImageBuffer[2], ref ImageWidth[2],
                                            ref ImageHeight[2], ref ImageBuffer[3], ref ImageWidth[3], ref ImageHeight[3]);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                return;
            }

            recordInfo.horizontalLine = imageWidth;
            recordInfo.verticalLine = imageHeight;
            recordInfo.scaleUnit = 1;
            recordInfo.horizontalPixelScale = 500;
            recordInfo.verticalPixelScale = 500;
            recordInfo.compressionAlgorithm = RS_AN2K11.RSE_IMG_COMP_WSQ20;
            recordInfo.bpp = 8;
            recordInfo.numOfFingerPosition = 1;

            recordInfo.fingerPosition = new int[6];
            recordInfo.fingerPosition[0] = finger_position;

            recordInfo.numOfSegment = numOfFingers;
            recordInfo.numOfQuality = numOfFingers;


            slapInfoA = (RSSlapInfoArray)Marshal.PtrToStructure(slapInfoArray, typeof(RSSlapInfoArray));

            recordInfo.segPosition = new RSESegmentPosition[4];
            recordInfo.imgQuality = new RSEImageQuality[4];
            for (int i = 0; i < numOfFingers; i++)
            {
                if (finger_position > 10)
                    recordInfo.segPosition[i].fingerNumber = slapInfoA.RSSlapInfoA[i].fingerType;
                else
                    recordInfo.segPosition[i].fingerNumber = finger_position;

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

                // ===== Added boundary-correction logic =====

                // 1) Prevent negative values
                if (left < 0) left = 0;
                if (top < 0) top = 0;
                if (right < 0) right = 0;
                if (bottom < 0) bottom = 0;

                // 2) Ensure left ≤ right and top ≤ bottom
                if (right < left) right = left;
                if (bottom < top) bottom = top;

                // 3) Clamp coordinates to the image boundary
                if (right >= imageWidth) right = imageWidth - 1;
                if (bottom >= imageHeight) bottom = imageHeight - 1;



                recordInfo.segPosition[i].left = left;
                recordInfo.segPosition[i].right = right;
                recordInfo.segPosition[i].top = top;
                recordInfo.segPosition[i].bottom = bottom;


                if (finger_position > 10)
                    recordInfo.imgQuality[i].fingerNumber = slapInfoA.RSSlapInfoA[i].fingerType;
                else
                    recordInfo.imgQuality[i].fingerNumber = finger_position;
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

        private void processRecordType4(RSEType1Info type1Info, ref int IDC, IntPtr imageData, int imageWidth, int imageHeight, int finger_position)
        {

            int result = 0;
            RSEType4Info recordInfo = new RSEType4Info();

            recordInfo.IDC = IDC++;
            if (finger_position > 10)
                recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_PLAIN;
            else
                recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_ROLLED;

            recordInfo.imageScanningResolution = RS_AN2K11.RSE_ISR_NATIVE_SCANNING_RESOLUTION;
            recordInfo.horizontalLine = imageWidth;
            recordInfo.verticalLine = imageHeight;
            recordInfo.compressionAlgorithm = RS_AN2K11.RSE_IMG_COMP_WSQ20;
            recordInfo.fingerPosition = new char[6];
            recordInfo.fingerPosition[0] = (char)finger_position;


            int wsqBufferLength = imageWidth * imageHeight * 2;
            byte[] wsqBuffer = new byte[wsqBufferLength];
            int m_result = RealScanSDK.RS_EncodeWSQ(imageData, imageWidth, imageHeight, 0.75f, wsqBuffer, ref wsqBufferLength);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                string m_errorMsg = "";
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg.ToString());
                return;
            }

            recordInfo.imageLen = wsqBufferLength;
            recordInfo.imageData = Marshal.AllocHGlobal(recordInfo.imageLen);
            Marshal.Copy(wsqBuffer, 0, recordInfo.imageData, wsqBufferLength);
            result = RS_AN2K11.RSE_CreateType4RecordCS(ref recordInfo);
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

        private void processRecordType15(RSEType1Info type1Info, ref int IDC, IntPtr imageData, int imageWidth, int imageHeight, int palm_position)
        {

            int result = 0;
           // Make type 15 record

            RSEType15Info recordInfo = new RSEType15Info();

            recordInfo.impressionType = RS_AN2K11.RSE_IMP_LIVESCAN_PALM; 
         
            recordInfo.sourceAgency = type1Info.originatingAgencyIdentifier;
            recordInfo.captureDate = type1Info.date;

            recordInfo.IDC = IDC++;

            recordInfo.horizontalLine = imageWidth;
            recordInfo.verticalLine = imageHeight;
            recordInfo.scaleUnit = 1;
            recordInfo.horizontalPixelScale = 500;
            recordInfo.verticalPixelScale = 500;
            recordInfo.compressionAlgorithm = RS_AN2K11.RSE_IMG_COMP_WSQ20;
            recordInfo.bpp = 8;
            recordInfo.fgp = palm_position;   

            recordInfo.shps = 1;
            recordInfo.svps = 1;


            int wsqBufferLength = imageWidth * imageHeight * 2;
            byte[] wsqBuffer = new byte[wsqBufferLength];
            int retCode = RealScanSDK.RS_EncodeWSQ(imageData, imageWidth, imageHeight, 0.75f, wsqBuffer, ref wsqBufferLength);

            recordInfo.imageLen = wsqBufferLength;
            recordInfo.imageData = Marshal.AllocHGlobal(recordInfo.imageLen);
            Marshal.Copy(wsqBuffer, 0, recordInfo.imageData, wsqBufferLength);

            result = RS_AN2K11.RSE_CreateType15RecordCS(ref recordInfo);


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


        private void btnCreateNIST_Click(object sender, EventArgs e)
        {
   
            int numOfDevice = 0;
            string m_errorMsg = "";
            int m_result = RealScanSDK.RS_InitSDK(null, 0, ref numOfDevice);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {

                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg);
                return;
            }


            SaveImageAnsiNist();
            RealScanSDK.RS_ExitAllDevices();
        }

        private void lblLeftThumb_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(6);
        }

        private void lblRightThumb_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(1);
        }

        private void lblRightIndex_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(2);
        }

        private void lblRightMiddle_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(3);
        }

        private void lblRightRing_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(4);
        }

        private void lblRightLittle_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(5);
        }

        private void lblLeftIndex_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(7);
        }

        private void lblLeftMiddle_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(8);
        }

        private void lblLeftRing_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(9);
        }

        private void lblLeftLittle_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(10);
        }

        private void lblRightSlap_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(13);
        }

        private void lblLeftSlap_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(14);
        }

        private void lblTwoThumbs_Click(object sender, EventArgs e)
        {
            Load_Fingerprint(15);
        }


        public void Get_NFIQ2_Quality()
        {
            Bitmap bmp = new Bitmap("d:\\wet_finger.bmp");
            int deviceHandle = 0;
            byte[] rawdata = ConvertBitmap(bmp);
            int Width = bmp.Width;
            int Height = bmp.Height;
            IntPtr ImageBuffer;
            ImageBuffer = Marshal.AllocHGlobal(Width * Height);
            Marshal.Copy(rawdata, 0, ImageBuffer, Width * Height);

            int numOfDevice = 0;
            string m_errorMsg = "";
            int m_result = RealScanSDK.RS_InitSDK(null, 0, ref numOfDevice);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {

                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg);
                return;
            }

            m_result = RealScanSDK.RS_InitDevice(0, ref deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg);
                return;
            }

            int IQ = 2;
            m_result = RealScanSDK.RS_SetParam(deviceHandle, RealScanSDK.RS_GVAR_FP_QUALITY_MODE, ref IQ);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg);
            }
            int nfiq = 0;

            m_result = RealScanSDK.RS_GetQualityScore(ImageBuffer, Width, Height, ref nfiq);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show(m_errorMsg);
            }
            MessageBox.Show("NFIQ2:"+ nfiq);
            RealScanSDK.RS_ExitAllDevices();
        }


    }
}
