using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace RealScan
{
    //
    //  Structure Declarations
    //

    //
    //  Related Device Structure
    //
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           // Device Information
    public struct RSDeviceInfo
    {
        public int deviceType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] productName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] deviceID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] firmwareVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] HardwareVersion;
        //public int lastInputTime;
        //public int lastScanTime;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public int[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //SDK Information
    public struct RSSDKInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] product;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] buildDate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] reserved;
    };
    //
    //  Related Overlay Structure
    //
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]            //Make OverlayText
    public struct RSOverlayText
    {
        public RSPoint pos;
        public int alignment;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] text;
        public int fontSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] fontName;
        public ulong color;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] reserved;
        //unsigned alignment;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Make OverlayCross
    public struct RSOverlayCross
    {

        public RSPoint centerPos;
        public int rangeX;
        public int rangeY;
        public ulong color;
        public int width;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] reserved;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Make OverlayLine
    public struct RSOverlayLine
    {
        public RSPoint startPos;
        public RSPoint endPos;
        public ulong color;
        public int width;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] reserved;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Make OverlayQuadrangle
    public struct RSOverlayQuadrangle
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSPoint[] pos;
        public ulong color;
        public int width;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public int[] reserved;
    };
    //
    //  Related Misc. Structure
    //
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSPoint
    {
        public int x;
        public int y;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSRect
    {
        public int x;
        public int y;
        public int right;
        public int bottom;
        public int left;
        public int top;
    };
    //
    //  Related Slap Structure
    //
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Slap Information     
    public struct RSSlapInfo
    {
        public int fingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSPoint[] fingerPosition;
        public int imageQuality;
        public int rotation;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public int[] reserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSSlapInfoArray
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSSlapInfo[] RSSlapInfoA;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Frame Information
    public struct RSFrameInfo
    {
        public IntPtr pImage;
        public IntPtr pQualityMap;
        public int iWidth;
        public int iHeight;
        public int iNFIQ;
        public int iQuality;
        public int iLiveness;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Frame Information
    public struct RSLFDInfo
    {
        public int nResult;
        public int nScore;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]           //Frame Information
    public struct RSLFDResult
    {
        public int nNumofFinger;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSLFDInfo[] arsLFDInfo;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSFingerInfo
    {
        public int nFingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSPoint[] arsPoint;
        public int nImageQuality;
        public int nRotation;
        public int nWidth;
        public int nHeight;
        public int nReserved;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSSegmentInfo
    {
        public int nFingerCnt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSFingerInfo[] arsFingerInfo;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSMissingInfo
    {
        public int nFirstfinger;
        public int nSecondfinger;
        public int nThirdfinger;
        public int nFourthfinger;
    };




    //
    // Delegates
    //

    /**
    * @brief Callback function for raw image capture.
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param errorCode    [IN] Capture result or error code.
    * @param imageData    [IN] Pointer to raw image data.
    * @param imageWidth   [IN] Width of the captured image in pixels.
    * @param imageHeight  [IN] Height of the captured image in pixels.
    */
    public delegate void CaptureDataCallback(
        int deviceHandle,
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight
    );

    /**
    * @brief Advanced callback function for receiving raw frame information.
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param errorCode    [IN] Capture result or error code.
    * @param frameInfo    [IN] Pointer to RSFrameInfo structure containing
    *                           captured image and metadata.
    */
    public delegate void AdvCaptureDataCallbackInt(
        int deviceHandle,
        int errorCode,
        IntPtr pFrameInfo
    );

    /**
    * @brief Advanced UI callback function for receiving processed frame information.
    *
    * @param errorCode    [IN] Capture result or error code.
    * @param imageData    [IN] Pointer to processed image data.
    * @param imageWidth   [IN] Width of the rendered/processed image.
    * @param imageHeight  [IN] Height of the rendered/processed image.
    * @param score        [IN] Quality or algorithm score (e.g., liveness score).
    */
    public delegate void RSAdvCaptureUiCallback(
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight,
        int score
    );

    /**
    * @brief Callback for raw capture result without device handle.
    *
    * @param errorCode    [IN] Capture result or error code.
    * @param imageData    [IN] Pointer to raw captured image data.
    * @param imageWidth   [IN] Image width in pixels.
    * @param imageHeight  [IN] Image height in pixels.
    */
    public delegate void RSRawCaptureCallback(
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight
    );

    /**
    * @brief Advanced raw capture callback including LFD/quality score.
    *
    * @param errorCode    [IN] Capture result or error code.
    * @param imageData    [IN] Pointer to raw captured image data.
    * @param imageWidth   [IN] Image width in pixels.
    * @param imageHeight  [IN] Image height in pixels.
    * @param lfdScore     [IN] Liveness or quality score from algorithm/device.
    */
    public delegate void RSAdvCaptureCallback(
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight,
        int lfdScore
    );

    /**
    * @brief Preview callback delivering raw preview frame data.
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param errorCode    [IN] Preview result or error code.
    * @param imageData    [IN] Pointer to preview image data.
    * @param imageWidth   [IN] Width of the preview image in pixels.
    * @param imageHeight  [IN] Height of the preview image in pixels.
    */
    public delegate void RSPreviewDataCallback(
        int deviceHandle,
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight
    );

    /**
    * @brief Advanced preview callback including quality and status information.
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param errorCode    [IN] Preview result or error code.
    * @param imageData    [IN] Pointer to preview image data.
    * @param imageWidth   [IN] Width of the preview frame.
    * @param imageHeight  [IN] Height of the preview frame.
    * @param quality      [IN] Estimated image quality score.
    * @param status       [IN] Capture status flags (e.g., finger detect, movement).
    */
    public delegate void RSAdvPreviewCallback(
        int deviceHandle,
        int errorCode,
        IntPtr imageData,
        int imageWidth,
        int imageHeight,
        int quality,
        int status
    );

    /**
    * @brief Preview callback using managed byte array.
    *
    * @param errorCode    [IN] Preview result or error code.
    * @param imageData    [IN] Preview image data buffer.
    * @param imageWidth   [IN] Image width in pixels.
    * @param imageHeight  [IN] Image height in pixels.
    */
    public delegate void RSRawPreviewCallback(
        int errorCode,
        byte[] imageData,
        int imageWidth,
        int imageHeight
    );

    /**
    * @brief Advanced preview callback using managed byte array with quality/status.
    *
    * @param errorCode    [IN] Preview result or error code.
    * @param imageData    [IN] Preview image data buffer.
    * @param imageWidth   [IN] Image width in pixels.
    * @param imageHeight  [IN] Image height in pixels.
    * @param quality      [IN] Estimated preview quality score.
    * @param status       [IN] Capture status flags.
    */
    public delegate void RSRawAdvPreviewCallback(
        int errorCode,
        byte[] imageData,
        int imageWidth,
        int imageHeight,
        int quality,
        int status
    );

    /**
    * @brief Callback function for keypad input events.
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param keyCode      [IN] Key code of the pressed keypad button.
    */
    public delegate void RSKeypadCallback(
        int deviceHandle,
        uint keyCode
    );

    /**
    * @brief Integer-type keypad callback (same as RSKeypadCallback).
    *
    * @param deviceHandle [IN] Handle to the opened device.
    * @param keyCode      [IN] Key code of the pressed keypad button.
    */
    public delegate void RSKeypadCallbackInt(
        int deviceHandle,
        uint keyCode
    );

    /**
    * @brief Callback function for device hot plugging (connection or disconnection).
    *
    * @param deviceId     [IN] ID of the device that triggered the event.
    * @param isConnected  [IN] true if the device is connected; false if disconnected.
    */
    public delegate void RSHotPluggingCallback(
        int deviceId,
        bool isConnected
    );



    //
    // API Declarations
    //    

    public class RealScanSDK
    {
        /** 
        * @var RS_SUCCESS
        * @brief Return value indicating that the API function executed successfully.
        * API functions return this value upon success,
        * while different error codes may be returned in case of failure. 
        */
        public const int RS_SUCCESS = 0;

        /** 
        * @var RS_ERR_UNKNOWN
        * @brief Unknown error occurred.
        */
        public const int RS_ERR_UNKNOWN = -1;

        /** 
        * @var RS_ERR_NOT_SUPPORT
        * @brief Operation is not supported. 
        * 
        */
        public const int RS_ERR_NOT_SUPPORT = -2;

        /** 
        * @var RS_ERR_SMALL_BUFFER
        * @brief Buffer provided is too small. 
        *
        * @note
        * - RS_GenerateIso19794_4_2011Ex() 
        */
        public const int RS_ERR_SMALL_BUFFER = -4;

        /** 
        * @var RS_ERR_NO_DATA
        * @brief No data available or accessible. 
        * 
        * @note
        * - RS_GetLFDResult() 
        * - RS_TakeCurrentImageData()
        * - RS_TakeCurrentImageDataEx()
        * - RS_TakeCurrentImageDataSegment()
        * - RS_TakeCurrentImageDataSegmentWithSize()
        */
        public const int RS_ERR_NO_DATA = -5;

        /** 
        * @var RS_ERR_CANNOT_LOAD_DLL
        * @brief Failed to load DLL or shared library.
        */
        public const int RS_ERR_CANNOT_LOAD_DLL = -6;


        //========================
        // Generic errors
        //========================

        /** 
        * @var RS_ERR_SDK_UNINITIALIZED
        * @brief SDK not initialized.
        *
        * @note
        *  - RS_AbortCapture()
        *  - RS_Beep()
        *  - RS_Calibrate()
        *  - RS_ExitDevice()
        *  - RS_GetAdvancedContrastEnhancement()
        *  - RS_GetAutomaticCalibrate()
        *  - RS_GetCaptureMode()
        *  - RS_GetCaptureModeWithDir()
        *  - RS_GetDeviceStatus()
        *  - RS_GetHaloProcessing()
        *  - RS_GetImageSize()
        *  - RS_GetManualCaptureMode()
        *  - RS_GetMinimumFinger()
        *  - RS_GetPostProcessing()
        *  - RS_GetPostProcessingEx()
        *  - RS_GetPreProcessing()
        *  - RS_GetRollFingerOption()
        *  - RS_InitDevice()
        *  - RS_InitDeviceById()
        *  - RS_IsCapturing()
        *  - RS_PlayWav()
        *  - RS_PowerCheck()
        *  - RS_RegisterAdvCaptureDataCallback()
        *  - RS_RegisterAdvPreviewCallback()
        *  - RS_RegisterCaptureCallback()
        *  - RS_RegisterFingerOnCallback()
        *  - RS_RegisterKeypadCallback()
        *  - RS_RegisterPreviewCallback()
        *  - RS_ResetDevice()
        *  - RS_ResetLCD()
        *  - RS_SetAdvancedContrastEnhancement()
        *  - RS_SetAutomaticCalibrate()
        *  - RS_SetCaptureBeep()
        *  - RS_SetCaptureMode()
        *  - RS_SetCaptureModeWithDir()
        *  - RS_SetFingerLED()
        *  - RS_SetHaloProcessing()
        *  - RS_SetManualCaptureMode()
        *  - RS_SetMinimumFinger()
        *  - RS_SetModeLED()
        *  - RS_SetPostProcessing()
        *  - RS_SetPostProcessingEx()
        *  - RS_SetPreProcessing()
        *  - RS_SetRollFingerOption()
        *  - RS_SetStatusLED()
        *  - RS_StartCapture()
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        *  - RS_TurnOnBacklight_RSF()
        */
        public const int RS_ERR_SDK_UNINITIALIZED = -10;

        /** 
        * @var RS_ERR_SDK_ALREADY_INITIALIZED
        * @brief SDK has already been initialized.
        *
        * @note
        *  - RS_InitDeviceById()
        *  - RS_InitSDK()
        *  - RS_StartHotPlugging()
        */
        public const int RS_ERR_SDK_ALREADY_INITIALIZED = -11;

        /** 
        * @var RS_ERR_INSUFFICIENT_HARDWARE
        * @brief Required hardware is insufficient.
        */
        public const int RS_ERR_INSUFFICIENT_HARDWARE = -12;

        /** 
        * @var RS_ERR_INVALID_PARAM
        * @brief Invalid function parameter passed.
        *
        * @note
        *  - RS_Beep()
        *  - RS_DisplayLCD()
        *  - RS_EncodeJP2()
        *  - RS_EncodeWSQ()
        *  - RS_GetParam()
        *  - RS_IsFinger()
        *  - RS_PlayWav()
        *  - RS_RegisterAdvCaptureDataCallback()
        *  - RS_RegisterAdvPreviewCallback()
        *  - RS_RegisterCaptureCallback()
        *  - RS_RegisterFingerOnCallback()
        *  - RS_RegisterKeypadCallback()
        *  - RS_RegisterPreviewCallback()
        *  - RS_ResetLCD()
        *  - RS_SequenceCheck()
        *  - RS_SequenceCheckMissingFinger()
        *  - RS_SetCaptureBeep()
        *  - RS_SetCaptureMode()
        *  - RS_SetCaptureModeWithDir()
        *  - RS_SetFingerLED()
        *  - RS_SetManualCaptureMode()
        *  - RS_SetModeLED()
        *  - RS_SetModeLEDColor()
        *  - RS_SetParam()
        *  - RS_SetRollFingerOption()
        *  - RS_SetStatusLED()
        *  - RS_TurnOnBacklight_RSF()
        */
        public const int RS_ERR_INVALID_PARAM = -13;

        /** 
        * @var RS_ERR_MEM_FULL
        * @brief Memory allocation failed or insufficient memory.
        *
        * @note
        *  - RS_EncodeJP2()
        *  - RS_EncodeWSQ()
        *  - RS_SaveBitmap()
        *  - RS_SaveBitmapMem()
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_SequenceCheck()
        *  - RS_SequenceCheckMissingFinger()
        */
        public const int RS_ERR_MEM_FULL = -14;

        /** 
        * @var RS_ERR_NOT_YET_SUPPORTED
        * @brief Feature not supported in this version.
        */
        public const int RS_ERR_NOT_YET_SUPPORTED = -15;

        /** 
        * @var RS_ERR_CANNOT_OPEN_FILE
        * @brief Failed to open a file. 
        * 
        * @note 
        *  - RS_PlayWav()
        */
        public const int RS_ERR_CANNOT_OPEN_FILE = -16;

        /** 
        * @var RS_ERR_CANNOT_READ_FILE
        * @brief Failed to read from a file. 
        * 
        * @note 
        *  - RS_PlayWav()
        */
        public const int RS_ERR_CANNOT_READ_FILE = -17;

        /** 
        * @var RS_ERR_CANNOT_WRITE_FILE
        * @brief Failed to write to a file.
        * 
        * @note 
        *  - RS_SaveBitmap()
        */
        public const int RS_ERR_CANNOT_WRITE_FILE = -18;


        //========================
        // Device related errors
        //========================

        /** 
        * @var RS_ERR_NO_DEVICE
        * @brief No connected device found.
        *
        * @note
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_SequenceCheck()
        *  - RS_SequenceCheckMissingFinger()
        */
        public const int RS_ERR_NO_DEVICE = -100;

        /** 
        * @var RS_ERR_INVALID_DEVICE_INDEX
        * @brief Invalid device index.
        *
        * @note
        *  - RS_GetDeviceId()
        *  - RS_GetDeviceStatus()
        *  - RS_InitDevice()
        *  - RS_InitDeviceById()
        */
        public const int RS_ERR_INVALID_DEVICE_INDEX = -101;

        /** 
        * @var RS_ERR_INVALID_HANDLE
        * @brief Invalid device handle.
        *
        * @note
        *  - RS_AbortCapture()
        *  - RS_Beep()
        *  - RS_Calibrate()
        *  - RS_ExitDevice()
        *  - RS_GetAdvancedContrastEnhancement()
        *  - RS_GetAutomaticCalibrate()
        *  - RS_GetCaptureMode()
        *  - RS_GetCaptureModeWithDir()
        *  - RS_GetDeviceInfo()
        *  - RS_GetDeviceStatus()
        *  - RS_GetHaloProcessing()
        *  - RS_GetImageSize()
        *  - RS_GetManualCaptureMode()
        *  - RS_GetMinimumFinger()
        *  - RS_GetParam()
        *  - RS_GetPostProcessing()
        *  - RS_GetPostProcessingEx()
        *  - RS_GetPreProcessing()
        *  - RS_GetQualityScore()
        *  - RS_GetRollFingerOption()
        *  - RS_IsCapturing()
        *  - RS_IsFinger()
        *  - RS_PlayWav()
        *  - RS_RegisterAdvCaptureDataCallback()
        *  - RS_RegisterAdvPreviewCallback()
        *  - RS_RegisterCaptureCallback()
        *  - RS_RegisterFingerOnCallback()
        *  - RS_RegisterKeypadCallback()
        *  - RS_RegisterPreviewCallback()
        *  - RS_ResetDevice()
        *  - RS_ResetLCD()
        *  - RS_SetAdvancedContrastEnhancement()
        *  - RS_SetAutomaticCalibrate()
        *  - RS_SetCaptureBeep()
        *  - RS_SetCaptureMode()
        *  - RS_SetCaptureModeWithDir()
        *  - RS_SetFingerLED()
        *  - RS_SetHaloProcessing()
        *  - RS_SetManualCaptureMode()
        *  - RS_SetMinimumFinger()
        *  - RS_SetModeLED()
        *  - RS_SetModeLEDColor()
        *  - RS_SetParam()
        *  - RS_SetPostProcessing()
        *  - RS_SetPostProcessingEx()
        *  - RS_SetPreProcessing()
        *  - RS_SetRollFingerOption()
        *  - RS_SetStatusLED()
        *  - RS_StartCapture()
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        *  - RS_TurnOnBacklight_RSF()
        */
        public const int RS_ERR_INVALID_HANDLE = -102;

        /** 
        * @var RS_ERR_CANNOT_INIT_DEVICE
        * @brief Failed to initialize device. 
        */
        public const int RS_ERR_CANNOT_INIT_DEVICE = -103;

        /** 
        * @var RS_ERR_UNSUPPORTED_DEVICE
        * @brief Unsupported device model. 
        *
        * @note
        * - RS_GetCurrentProcessedImage()
        * - RS_GetCurrentSynthesisImage()
        */
        public const int RS_ERR_UNSUPPORTED_DEVICE = -104;

        /** 
        * @var RS_ERR_CANNOT_GET_USB_DEVICE
        * @brief Cannot get USB device reference.
        *
        * @note
        * - RS_InitSDK()
        */
        public const int RS_ERR_CANNOT_GET_USB_DEVICE = -105;

        /** 
        * @var RS_ERR_DEVICE_ALREADY_INITIALIZED
        * @brief Device is already initialized.
        *
        * @note
        * - RS_InitDevice()
        */
        public const int RS_ERR_DEVICE_ALREADY_INITIALIZED = -106;

        /** 
        * @var RS_ERR_CANNOT_OPEN_DEVICE
        * @brief Cannot open device connection.
        *
        * @note
        * - RS_InitDeviceById()
        * - RS_InitDevice()
        */
        public const int RS_ERR_CANNOT_OPEN_DEVICE = -107;

        /** 
        * @var RS_ERR_CANNOT_WRITE_USB
        * @brief USB write failed.
        */
        public const int RS_ERR_CANNOT_WRITE_USB = -108;

        /** 
        * @var RS_ERR_WRITE_USB_TIMEOUT
        * @brief USB write timeout.
        */
        public const int RS_ERR_WRITE_USB_TIMEOUT = -109;

        /** 
        * @var RS_ERR_CANNOT_READ_USB
        * @brief USB read failed.
        */
        public const int RS_ERR_CANNOT_READ_USB = -110;

        /** 
        * @var RS_ERR_READ_USB_TIMEOUT
        * @brief USB read timeout.
        */
        public const int RS_ERR_READ_USB_TIMEOUT = -111;

        /** 
        * @var RS_ERR_INVALID_CAMERA_MODE
        * @brief Invalid camera mode specified.
        */
        public const int RS_ERR_INVALID_CAMERA_MODE = -112;

        /** 
        * @var RS_ERR_UNSUPPORTED_WAV_FORMAT
        * @brief WAV file format is not supported. 
        *
        * @note
        * - RS_PlayWav()
        */
        public const int RS_ERR_UNSUPPORTED_WAV_FORMAT = -113;

        /** 
        * @var RS_ERR_UNSUPPORTED_COMMAND
        * @brief Unsupported command sent to device.
        *
        * @note
        * - RS_DisplayLCD()
        * - RS_GetAutomaticCalibrate()
        * - RS_GetAutomaticContrast()
        * - RS_GetKeyStatus()
        * - RS_GetLFDLevel()
        * - RS_GetLFDResult()
        * - RS_GetLFDScore()
        * - RS_GetManualContrast()
        * - RS_PlayWav()
        * - RS_PowerCheck()
        * - RS_RegisterKeypadCallback()
        * - RS_ResetDevice()
        * - RS_ResetLCD()
        * - RS_SelfTest()
        * - RS_SetActiveKey()
        * - RS_SetAutomaticCalibrate()
        * - RS_SetAutomaticContrast()
        * - RS_SetCaptureBeep()
        * - RS_SetFingerLED()
        * - RS_SetLFDLevel()
        * - RS_SetManualContrast()
        * - RS_SetModeLED()
        * - RS_SetModeLEDColor()
        * - RS_SetParam()
        * - RS_SetPreProcessing()
        * - RS_SetStatusLED()
        * - RS_TurnOnBacklight_RSF()
        */
        public const int RS_ERR_UNSUPPORTED_COMMAND = -114;

        /** 
        * @var RS_ERR_SENSOR_DIRTY
        * @brief Sensor is dirty and needs cleaning.
        *
        * @note
        *  - RS_InitDevice()
        *  - RS_RegisterFingerOnCallback()
        *  - RS_SelfTest()
        *  - RS_StartCapture()
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_SENSOR_DIRTY = -115;

        /** 
        * @var RS_ERR_FINGER_EXIST
        * @brief Finger already exists on the sensor.
        * 
        * @note
        * - RS_InitDevice()
        * - RS_IsFinger()
        * - RS_SelfTest()
        * - RS_StartCapture()
        * - RS_TakeCurrentImageData()
        * - RS_TakeCurrentImageDataEx()
        * - RS_TakeCurrentImageDataSegment()
        * - RS_TakeCurrentImageDataSegmentWithSize()
        * - RS_TakeCurrentImageEx()
        * - RS_TakeImage()
        * - RS_TakeImageData()
        * - RS_TakeImageDataEx()
        * - RS_TakeImageDataSegment()
        * - RS_TakeImageDataSegmentWithSize()
        * - RS_TakeImageEx()
        */
        public const int RS_ERR_FINGER_EXIST = -116;

        /** 
        * @var RS_ERR_TOO_STRONG_LIGHT
        * @brief Too strong ambient light detected. 
        * 
        * @note
        * - RS_InitDevice()
        * - RS_SelfTest()
        */
        public const int RS_ERR_TOO_STRONG_LIGHT = -117;

        /** 
        * @var RS_ERR_INVALID_DEVICE_CONNECTION
        * @brief Invalid connection with device.
        */
        public const int RS_ERR_INVALID_DEVICE_CONNECTION = -124;

        /** 
        * @var RS_ERR_LOCKED_DEVICE
        * @brief Device is locked and cannot be used.
        */
        public const int RS_ERR_LOCKED_DEVICE = -126;

        /** 
        * @var RS_ERR_DEVICE_NOT_INITIALIZED
        * @brief Device was not initialized.
        */
        public const int RS_ERR_DEVICE_NOT_INITIALIZED = -127;

        /** 
        * @var RS_ERR_BACKGROUD_DARK
        * @brief Background Image is too dark.
        */
        public const int RS_ERR_BACKGROUD_DARK = -128;

        /** 
        * @var RS_ERR_BG_NOT_REGISTERED
        * @brief Background Image is not registered.
        */
        public const int RS_ERR_BG_NOT_REGISTERED = -129;


        //========================
        // Capture related errors
        //========================

        /** 
        * @var RS_ERR_INVALID_CAPTURE_MODE
        * @brief Invalid capture mode specified.
        *
        * @note
        * - RS_SetCaptureMode()
        * - RS_SetCaptureModeWithDir()
        * - RS_SetManualCaptureMode()
        * - RS_GetMinimumFinger()
        * - RS_SetMinimumFinger()
        */
        public const int RS_ERR_INVALID_CAPTURE_MODE = -200;

        /** 
        * @var RS_ERR_CAPTURE_DISABLED
        * @brief Capture function is disabled.
        *
        * @note
        * - RS_GetCurrentProcessedImage()
        * - RS_GetCurrentSynthesisImage()
        * - RS_GetImageSize()
        * - RS_StartCapture()
        * - RS_TakeCurrentImageData()
        * - RS_TakeCurrentImageDataEx()
        * - RS_TakeCurrentImageDataSegment()
        * - RS_TakeCurrentImageDataSegmentWithSize()
        * - RS_TakeCurrentImageEx()
        * - RS_TakeImage()
        * - RS_TakeImageData()
        * - RS_TakeImageDataEx()
        * - RS_TakeImageDataSegment()
        * - RS_TakeImageDataSegmentWithSize()
        * - RS_TakeImageEx()
        */
        public const int RS_ERR_CAPTURE_DISABLED = -201;

        /** 
        * @var RS_ERR_CAPTURE_TIMEOUT
        * @brief Capture operation timed out.
        *
        * @note
        * - RS_RegisterFingerOnCallback()
        * - RS_TakeCurrentImageData()
        * - RS_TakeCurrentImageDataEx()
        * - RS_TakeCurrentImageDataSegment()
        * - RS_TakeCurrentImageDataSegmentWithSize()
        * - RS_TakeCurrentImageEx()
        * - RS_TakeImage()
        * - RS_TakeImageData()
        * - RS_TakeImageDataEx()
        * - RS_TakeImageDataSegment()
        * - RS_TakeImageDataSegmentWithSize()
        * - RS_TakeImageEx()
        */
        public const int RS_ERR_CAPTURE_TIMEOUT = -202;

        /** 
        * @var RS_ERR_CAPTURE_ABORTED
        * @brief Capture was aborted manually.
        */
        public const int RS_ERR_CAPTURE_ABORTED = -203;

        /** 
        * @var RS_ERR_CAPTURE_CAMERA
        * @brief Camera error during capture.
        */
        public const int RS_ERR_CAPTURE_CAMERA = -204;

        /** 
        * @var RS_ERR_CAPTURE_NO_PREVIEW
        * @brief No preview image available.
        */
        public const int RS_ERR_CAPTURE_NO_PREVIEW = -205;

        /** 
        * @var RS_ERR_ROLL_PART_LIFT
        * @brief Finger partially lifted during rolling.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_PART_LIFT = -206;

        /** 
        * @var RS_ERR_ROLL_DIRTY
        * @brief Rolling image is dirty.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_DIRTY = -207;

        /** 
        * @var RS_ERR_ROLL_TOO_FAST
        * @brief Finger rolled too fast.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_TOO_FAST = -208;

        /** 
        * @var RS_ERR_ROLL_SHIFTED
        * @brief Finger rolled off the expected path.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_SHIFTED = -209;

        /** 
        * @var RS_ERR_ROLL_DRY
        * @brief Finger too dry during capture.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_DRY = -210;

        /** 
        * @var RS_ERR_ROLL_WRONG_DIR
        * @brief Rolling in wrong direction.
        *
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_WRONG_DIR = -211;

        /** 
        * @var RS_ERR_CAPTURE_IS_RUNNING
        * @brief Capture is currently running.
        *
        * @note
        *  - RS_Calibrate()
        *  - RS_ExitDevice()
        *  - RS_GetAdvancedContrastEnhancement()
        *  - RS_GetHaloProcessing()
        *  - RS_GetMinimumFinger()
        *  - RS_InitDevice()
        *  - RS_SetAdvancedContrastEnhancement()
        *  - RS_SetAutomaticCalibrate()
        *  - RS_SetCaptureMode()
        *  - RS_SetCaptureModeWithDir()
        *  - RS_SetManualCaptureMode()
        *  - RS_SetPostProcessing()
        *  - RS_SetPostProcessingEx()
        *  - RS_SetSegRotateOption()
        *  - RS_StartCapture()
        */
        public const int RS_ERR_CAPTURE_IS_RUNNING = -212;

        /** 
        * @var RS_ERR_CAPTURE_IS_NOT_RUNNING
        * @brief Capture is not currently running.
        */
        public const int RS_ERR_CAPTURE_IS_NOT_RUNNING = -213;

        /** 
        * @var RS_ERR_ROLL_TOO_SHORT
        * @brief Roll was too short to be valid. 
        * 
        * @note
        *  - RS_TakeCurrentImageData()
        *  - RS_TakeCurrentImageDataEx()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeCurrentImageEx()
        *  - RS_TakeImage()
        *  - RS_TakeImageData()
        *  - RS_TakeImageDataEx()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        *  - RS_TakeImageEx()
        */
        public const int RS_ERR_ROLL_TOO_SHORT = -214;

        /** 
        * @var RS_ERR_CANNOT_SEGMENT
        * @brief Failed to segment captured image.
        *
        * @note
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_SequenceCheck()
        *  - RS_SequenceCheckMissingFinger()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        */
        public const int RS_ERR_CANNOT_SEGMENT = -215;

        /** 
        * @var RS_ERR_CANNOT_GET_QUALITY
        * @brief Failed to assess image quality.
        *
        * @note
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        */
        public const int RS_ERR_CANNOT_GET_QUALITY = -216;

        /** 
        * @var RS_ERR_SEGMENT_FEWER_FINGER
        * @brief Fewer fingers detected than expected.
        *
        * @note
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        */
        public const int RS_ERR_SEGMENT_FEWER_FINGER = -217;

        /** 
        * @var RS_ERR_SEGMENT_WRONG_HAND
        * @brief Wrong hand detected in image.
        *
        * @note
        *  - RS_Segment4()
        *  - RS_Segment4WithSize()
        *  - RS_SegmentImages()
        *  - RS_SegmentImagesMissingFinger()
        *  - RS_SegmentImagesMissingFingerEx()
        *  - RS_SegmentMask()
        *  - RS_SegmentMaskEx()
        *  - RS_SegmentMissingFinger()
        *  - RS_SegmentMissingFingerEx()
        *  - RS_SegmentWithSize()
        *  - RS_TakeCurrentImageDataSegment()
        *  - RS_TakeCurrentImageDataSegmentWithSize()
        *  - RS_TakeImageDataSegment()
        *  - RS_TakeImageDataSegmentWithSize()
        */
        public const int RS_ERR_SEGMENT_WRONG_HAND = -218;

        /** 
        * @var RS_ERR_CANNOT_EXTRACT_TEMPLATE
        * @brief Failed to extract fingerprint template.
        */
        public const int RS_ERR_CANNOT_EXTRACT_TEMPLATE = -219;

        /** 
        * @var RS_ERR_NO_MATCH
        * @brief Fingerprint does not match any enrolled template.
        *
        * @note
        *  - RS_FindTarget()
        */
        public const int RS_ERR_NO_MATCH = -220;

        /** 
        * @var RS_ERR_ROLL_NO_FINGER
        * @brief No finger detected during roll.
        */
        public const int RS_ERR_ROLL_NO_FINGER = -221;

        /** 
        * @var RS_ERR_CAPTURE_FINGERS_MOVING
        * @brief Fingers moved during capture.
        */
        public const int RS_ERR_CAPTURE_FINGERS_MOVING = -222;

        /** 
        * @var RS_ERR_CAPTURE_NOTENOUGH_FINGERS
        * @brief Not enough fingers were detected.
        *
        * @note
        *  - RS_GetImageQualityScore()
        *  - RS_IsFinger()
        */
        public const int RS_ERR_CAPTURE_NOTENOUGH_FINGERS = -223;

        /** 
        * @var RS_ERR_CAPTURE_TOOSMALL_FINGERSAREA
        * @brief Finger area is too small. 
        * 
        * @note
        *  - RS_GetImageQualityScore()
        */
        public const int RS_ERR_CAPTURE_TOOSMALL_FINGERSAREA = -224;

        /** 
        * @var RS_ERR_CAPTURE_TOOWEAK_PRINT
        * @brief Fingerprint is too weak. 
        * 
        * @note
        *  - RS_GetImageQualityScore()
        */
        public const int RS_ERR_CAPTURE_TOOWEAK_PRINT = -225;

        /** 
        * @var RS_ERR_CAPTURE_TOOSTRONG_PRINT
        * @brief Fingerprint is too strong or overexposed.
        * 
        * @note
        *  - RS_GetImageQualityScore()
        */
        public const int RS_ERR_CAPTURE_TOOSTRONG_PRINT = -226;

        /** 
        * @var RS_ERR_CAPTURE_TOOMUCH_PRESSURE
        * @brief Too much pressure was applied during capture. 
        * 
        * @note
        *  - RS_GetImageQualityScore()
        */
        public const int RS_ERR_CAPTURE_TOOMUCH_PRESSURE = -227;

        /** 
        * @var RS_ERR_CAPTURE_NOTENOUGH_FRAMES
        * @brief Not enough frames were captured. 
        * 
        * @note
        *  - RS_GetImageQualityScore()
        */
        public const int RS_ERR_CAPTURE_NOTENOUGH_FRAMES = -228;

        /** 
        * @var RS_ERR_CAPTURE_WRONG_MINFINGER
        * @brief Wrong minimum finger count configuration. 
        * 
        * @note
        *  - RS_SetMinimumFinger()
        *  - RS_GetMinimumFinger()
        */
        public const int RS_ERR_CAPTURE_WRONG_MINFINGER = -229;

        /** 
        * @var RS_ERR_ABNORMAL_CAPTURE_SIZE
        * @brief Abnormal capture image size. 
        * 
        * @note
        *  - RS_GetCurrentProcessedImage()
        *  - RS_GetCurrentSynthesisImage()
        */
        public const int RS_ERR_ABNORMAL_CAPTURE_SIZE = -230;

        /** 
        * @var RS_ERR_ABORT_IS_RUNNING
        * @brief Abort process is already in progress.
        */
        public const int RS_ERR_ABORT_IS_RUNNING = -231;

        /** 
        * @var RS_ERR_LFD_PROCESSING
        * @brief Error occurred during LFD (Live Finger Detection) processing. 
        * 
        * @note 
        *  - RS_GetLFDResult()
        */
        public const int RS_ERR_LFD_PROCESSING = -232;


        //========================
        // Viewer related errors
        //========================

        /** 
        * @var RS_ERR_CANNOT_SET_VIEW_WINDOW
        * @brief Failed to assign preview window.
        */
        public const int RS_ERR_CANNOT_SET_VIEW_WINDOW = -300;

        /** 
        * @var RS_ERR_NO_MORE_OVERLAY
        * @brief No more overlays are available.
        */
        public const int RS_ERR_NO_MORE_OVERLAY = -301;

        /** 
        * @var RS_ERR_INVALID_OVERLAY_HANDLE
        * @brief Invalid overlay handle specified.
        */
        public const int RS_ERR_INVALID_OVERLAY_HANDLE = -302;

        /** 
        * @var RS_ERR_CANNOT_SHOW_OVERLAY
        * @brief Failed to render overlay on screen.
        */
        public const int RS_ERR_CANNOT_SHOW_OVERLAY = -303;

        /** 
        * @var RS_ERR_CANNOT_SHOW_PREVIEW
        * @brief Failed to show preview image.
        */
        public const int RS_ERR_CANNOT_SHOW_PREVIEW = -304;


        //========================
        // IO related errors
        //========================

        /** 
        * @var RS_ERR_CANNOT_SET_KEYPAD_CALLBACK
        * @brief Failed to set keypad callback function.
        */
        public const int RS_ERR_CANNOT_SET_KEYPAD_CALLBACK = -400;

        /** 
        * @var RS_ERR_CANNOT_MAKE_LCD_DATA
        * @brief Failed to create LCD display data.
        * 
        * @note 
        *  - RS_MakeLCDData()
        */
        public const int RS_ERR_CANNOT_MAKE_LCD_DATA = -401;

        /** 
        * @var RS_ERR_INVALID_LCD_DATA
        * @brief Invalid LCD data format. 
        * 
        * @note
        *  - RS_MakeLCDData()
        */
        public const int RS_ERR_INVALID_LCD_DATA = -402;


        //========================
        // Warning codes
        //========================

        /** 
        * @var RS_WRN_STRONG_LIGHT
        * @brief Strong ambient light may affect image.
        */
        public const int RS_WRN_STRONG_LIGHT = 117;

        /** 
        * @var RS_WRN_FAKE_FINGER
        * @brief Possible fake finger detected.
        */
        public const int RS_WRN_FAKE_FINGER = 133;

        /** 
        * @var RS_WRN_TOO_POOR_QUALITY
        * @brief Captured image has poor quality.
        */
        public const int RS_WRN_TOO_POOR_QUALITY = 134;

        /** 
        * @var RS_WRN_BAD_SCAN
        * @brief Bad scan detected during image capture.
        */
        public const int RS_WRN_BAD_SCAN = 601;

        /** 
        * @var RS_WRN_BACKGROUND_DIRTY
        * @brief Background is dirty.
        */
        public const int RS_WRN_BACKGROUND_DIRTY = 602;

        /** 
        * @var RS_WRN_BACKGROUND_FINGER_EXIST
        * @brief Background contains a finger.
        */
        public const int RS_WRN_BACKGROUND_FINGER_EXIST = 603;


        //========================
        // Position-related warnings
        //========================
        /// @cond
        public const int RS_BAD_IMAGE = 701;
        public const int RS_FINGER_ON_LEFT_SIDE = 702;
        public const int RS_FINGER_ON_RIGHT_SIDE = 703;
        public const int RS_FINGER_ON_TOP_SIDE = 704;
        public const int RS_FINGER_ON_BOTTOM_SIDE = 705;
        public const int RS_FINGER_ON_TOP_LEFT_SIDE = 706;
        public const int RS_FINGER_ON_TOP_RIGHT_SIDE = 707;
        public const int RS_FINGER_ON_BOTTOM_LEFT_SIDE = 708;
        public const int RS_FINGER_ON_BOTTOM_RIGHT_SIDE = 709;

        public const int RS_PALM_ON_LEFT_SIDE = 712;
        public const int RS_PALM_ON_RIGHT_SIDE = 713;
        public const int RS_PALM_ON_TOP_SIDE = 714;
        public const int RS_PALM_ON_BOTTOM_SIDE = 715;
        public const int RS_PALM_ON_TOP_LEFT_SIDE = 716;
        public const int RS_PALM_ON_TOP_RIGHT_SIDE = 717;
        public const int RS_PALM_ON_BOTTOM_LEFT_SIDE = 718;
        public const int RS_PALM_ON_BOTTOM_RIGHT_SIDE = 719;

        /// @endcond

        /** 
        * @var RS_ERR_PALM_EXIST
        * @brief Palm already exists on the sensor.
        */
        public const int RS_ERR_PALM_EXIST = 720;



        /// @cond

        public const int RS_LFD_LIVE = 0;
        public const int RS_LFD_FAKE = 1;




        // Compression algorithm for 19794-4
        public const int RSE_COMP_19794_NONE = 0x00;
        public const int RSE_COMP_UNCOMPRESSED_BIT_PACKED = 0x01;
        public const int RSE_COMP_19794_WSQ = 0x02;
        public const int RSE_COMP_19794_JPEG = 0x03;
        public const int RSE_COMP_19794_JPEG2000 = 0x04;
        public const int RSE_COMP_19794_PNG = 0x05;

        //
        // Finger position (ISO 19794-4)
        //
        public const int RS_ISO19794_FGP_UNKNOWN = 0;
        public const int RS_ISO19794_FGP_RIGHT_THUMB = 1;
        public const int RS_ISO19794_FGP_RIGHT_INDEX = 2;
        public const int RS_ISO19794_FGP_RIGHT_MIDDLE = 3;
        public const int RS_ISO19794_FGP_RIGHT_RING = 4;
        public const int RS_ISO19794_FGP_RIGHT_LITTLE = 5;
        public const int RS_ISO19794_FGP_LEFT_THUMB = 6;
        public const int RS_ISO19794_FGP_LEFT_INDEX = 7;
        public const int RS_ISO19794_FGP_LEFT_MIDDLE = 8;
        public const int RS_ISO19794_FGP_LEFT_RING = 9;
        public const int RS_ISO19794_FGP_LEFT_LITTLE = 10;
        public const int RS_ISO19794_FGP_PLAIN_RIGHT_THUMB = 11;
        public const int RS_ISO19794_FGP_PLAIN_LEFT_THUMB = 12;
        public const int RS_ISO19794_FGP_PLAIN_RIGHT_FOUR = 13;
        public const int RS_ISO19794_FGP_PLAIN_LEFT_FOUR = 14;
        public const int RS_ISO19794_FGP_PLAIN_TWO_THUMBS = 15;
        public const int RS_ISO19794_FGP_EJI_OR_TIP = 16;



        //
        // Device Status
        //
        public const int RS_DEVICE_STATUS_NONE = 0;
        public const int RS_DEVICE_STATUS_READY = 1;
        public const int RS_DEVICE_STATUS_CONNECTED = 2;
        /// @endcond



        //
        // Device APIs
        //

        /**
        * @brief Initializes the SDK.
        *
        * @param configFileName [IN] Not Used.
        * @param option [IN] Not Used.
        * @param numOfDevice [OUT] Number of devices found and initialized.
        *
        * @return Function success status as an error code.
        * @retval 0 RS_SUCCESS
        * @retval -11 RS_ERR_SDK_ALREADY_INITIALIZED
        * @retval -105 RS_ERR_CANNOT_GET_USB_DEVICE
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int deviceCount = 0;
        * int result = RS_InitSDK(null, 0, ref deviceCount);
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine($"SDK initialized. Devices found: {deviceCount}");
        * } else {
        *     Console.WriteLine($"Failed to initialize SDK. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_InitSDK")]
        public static extern int RS_InitSDK(byte[] configFileName, int option, ref int numOfDevice);
        /**
        * @brief Initializes the specified device.
        *
        * Opens and prepares the device for use based on the given index.
        *
        * @param deviceIndex [IN] Index of the target device. (0-based)
        * @param deviceHandle [OUT] Handle to the initialized device.
        *
        * @return Function success status as an error code.
        * @retval 0 RS_SUCCESS
        * @retval -10 RS_ERR_SDK_UNINITIALIZED
        * @retval -101 RS_ERR_INVALID_DEVICE_INDEX
        * @retval -106 RS_ERR_DEVICE_ALREADY_INITIALIZED
        * @retval -107 RS_ERR_CANNOT_OPEN_DEVICE
        * @retval -115 RS_ERR_SENSOR_DIRTY
        * @retval -116 RS_ERR_FINGER_EXIST
        * @retval -117 RS_ERR_TOO_STRONG_LIGHT
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int handle = 0;
        * int result = RS_InitDevice(0, ref handle);
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine($"Device initialized. Handle: {handle}");
        * } else {
        *     Console.WriteLine($"Failed to initialize device. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_InitDevice")]
        public static extern int RS_InitDevice(int deviceIndex, ref int deviceHandle);

        /**
        * @brief Finalizes and closes the specified device.
        *
        * Releases the resources associated with the device handle and shuts down the device.
        *
        * @param deviceHandle [IN] Handle of the device to be finalized.
        *
        * @return Function success status as an error code.
        * @retval 0 RS_SUCCESS
        * @retval -10 RS_ERR_SDK_UNINITIALIZED
        * @retval -102 RS_ERR_INVALID_HANDLE
        * @retval -212 RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int handle = 0; // Assume valid handle
        * int result = RS_ExitDevice(handle);
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine("Device successfully closed.");
        * } else {
        *     Console.WriteLine($"Failed to exit device. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ExitDevice")]
        public static extern int RS_ExitDevice(int deviceHandle);

        /**
        * @brief Finalizes and closes all connected devices.
        *
        * This function releases the resources associated with all devices and shuts them down.
        *
        * @return Function success status as an error code.
        * @retval 0 RS_SUCCESS
        * @retval -10 RS_ERR_SDK_UNINITIALIZED
        * @retval -102 RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int result = RS_ExitAllDevices();
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine("All devices successfully closed.");
        * } else {
        *     Console.WriteLine($"Failed to exit devices. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ExitAllDevices")]
        public static extern int RS_ExitAllDevices();

        /**
        * @brief Retrieves information about a specified device.
        *
        * This function gets detailed information about the device identified by the provided handle.
        *
        * @param deviceHandle [IN] Handle of the device whose information is to be retrieved.
        * @param deviceInfo [OUT] Reference to the structure where device information will be stored.
        *
        * @return Function success status as an error code.
        * @retval 0 RS_SUCCESS
        * @retval -102 RS_ERR_INVALID_HANDLE
        * @retval -212 RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int handle = 0; // Assume valid handle
        * RSDeviceInfo info;
        * int result = RS_GetDeviceInfo(handle, ref info);
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine("Device info retrieved successfully.");
        * } else {
        *     Console.WriteLine($"Failed to get device info. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetDeviceInfo")]
        public static extern int RS_GetDeviceInfo(int deviceHandle, ref RSDeviceInfo deviceInfo);

        //
        // Capture APIs
        //
        /**
        * @brief Set capture mode for the device.
        *
        * @param deviceHandle [IN] Device handle obtained from RS_GetDeviceHandle.
        *
        * @param captureMode [IN] Capture mode to set.  
        *
        * | Macro Name                             | Value | Device Model                          |
        * |----------------------------------------|-------|----------------------------------------|
        * | RS_CAPTURE_DISABLED                     | 0     | RS-D, RS-F, RS-FC, RS-G10, RS-S60      |
        * | RS_CAPTURE_ROLL_FINGER                  | 1     | RS-D, RS-F, RS-FC, RS-G10, RS-S60      |
        * | RS_CAPTURE_FLAT_SINGLE_FINGER           | 2     | RS-D, RS-F, RS-FC, RS-G10, RS-S60      |
        * | RS_CAPTURE_FLAT_TWO_FINGERS             | 3     | RS-D, RS-F, RS-FC, RS-G10, RS-S60      |
        * | RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS       | 4     | RS-F, RS-FC, RS-G10, RS-S60            |
        * | RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS      | 5     | RS-F, RS-FC, RS-G10, RS-S60            |
        * | RS_CAPTURE_FLAT_LEFT_PALM               | 6     | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_RIGHT_PALM              | 7     | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_SINGLE_FINGER_EX        | 18    | RS-F, RS-FC, RS-G10, RS-S60            |
        * | RS_CAPTURE_FLAT_TWO_FINGERS_EX          | 19    | RS-F, RS-FC, RS-G10, RS-S60            |
        * | RS_CAPTURE_FLAT_AREA_SETTING            | 32    | -                                      |
        * | RS_CAPTURE_FLAT_AREA_SETTING_PREV       | 33    | -                                      |
        * | RS_CAPTURE_FLAT_AREA_SETTING_PREV_ROI   | 34    | -                                      |
        * | RS_CAPTURE_ROLL_FINGER_EX               | 48    | RS-D, RS-F, RS-FC, RS-G10, RS-S60      |
        * | RS_CAPTURE_FLAT_LEFT_SIDE_PALM          | 64    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_RIGHT_SIDE_PALM         | 65    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_LEFT_WRITERS_PALM       | 66    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM      | 67    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_LEFT_UPPER_PALM         | 68    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_RIGHT_UPPER_PALM        | 69    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_LEFT_LOWER_PALM         | 70    | RS-F, RS-FC                             |
        * | RS_CAPTURE_FLAT_RIGHT_LOWER_PALM        | 71    | RS-F, RS-FC                             |
        *
        *
        * @param captureOption [IN] Auto sensitivity option.
        *
        * | Macro Name                   | Value |
        * |------------------------------|-------|
        * | RS_AUTO_SENSITIVITY_NORMAL   | 0     |
        * | RS_AUTO_SENSITIVITY_HIGH     | 1     |
        * | RS_AUTO_SENSITIVITY_HIGHER   | 2     |
        * | RS_AUTO_SENSITIVITY_STRICT   | 3     |
        * | RS_AUTO_SENSITIVITY_DISABLED | 4     |
        *
        *
        * @param withModeLED [IN] Whether to turn on LED indicator for the mode. (Only G10)
        *
        * @return
        * @retval 0   RS_SUCCESS
        * @retval -10 RS_ERR_SDK_UNINITIALIZED
        * @retval -13 RS_ERR_INVALID_PARAM
        * @retval -102 RS_ERR_INVALID_HANDLE
        * @retval -200 RS_ERR_INVALID_CAPTURE_MODE
        * @retval -212 RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * @par Example
        * @code
        * int deviceHandle = 0; // Assume valid handle obtained from RS_GetDeviceHandle()
        *
        * int result = RS_SetCaptureMode(deviceHandle,
        *                                RS_CAPTURE_ROLL_FINGER,
        *                                RS_AUTO_SENSITIVITY_HIGH,
        *                                false);
        *
        * if (result == RS_SUCCESS) {
        *     Console.WriteLine("Capture mode set successfully.");
        * } else {
        *     Console.WriteLine($"Failed to set capture mode. Error code: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetCaptureMode")]
        public static extern int RS_SetCaptureMode(int deviceHandle,
                                                int captureMode,
                                                int captureOption,
                                                bool withModeLED);


        /**
         * @brief Sets the capture mode with direction and sensitivity options.
         *
         * This function configures the fingerprint capture mode for the specified device,
         * including direction and auto sensitivity settings. This is useful for adjusting
         * capture behavior based on the scanning angle.  
         * The `withModeLED` option is supported only on G10 devices and controls the mode indicator LED.
         *
         * @param deviceHandle [IN] Handle of the initialized device.
         *
         * @param captureMode [IN] Capture mode to be set.
         *
         * | Macro Name                              | Value | Device Model                           |
         * |-----------------------------------------|-------|-----------------------------------------|
         * | RS_CAPTURE_DISABLED                      | 0     | RS-D, RS-F, RS-FC, RS-G10, RS-S60       |
         * | RS_CAPTURE_ROLL_FINGER                   | 1     | RS-D, RS-F, RS-FC, RS-G10, RS-S60       |
         * | RS_CAPTURE_FLAT_SINGLE_FINGER            | 2     | RS-D, RS-F, RS-FC, RS-G10, RS-S60       |
         * | RS_CAPTURE_FLAT_TWO_FINGERS              | 3     | RS-D, RS-F, RS-FC, RS-G10, RS-S60       |
         * | RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS        | 4     | RS-F, RS-FC, RS-G10, RS-S60             |
         * | RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS       | 5     | RS-F, RS-FC, RS-G10, RS-S60             |
         * | RS_CAPTURE_FLAT_LEFT_PALM                | 6     | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_RIGHT_PALM               | 7     | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_SINGLE_FINGER_EX         | 18    | RS-F, RS-FC, RS-G10, RS-S60             |
         * | RS_CAPTURE_FLAT_TWO_FINGERS_EX           | 19    | RS-F, RS-FC, RS-G10, RS-S60             |
         * | RS_CAPTURE_FLAT_AREA_SETTING             | 32    | -                                       |
         * | RS_CAPTURE_FLAT_AREA_SETTING_PREV        | 33    | -                                       |
         * | RS_CAPTURE_FLAT_AREA_SETTING_PREV_ROI    | 34    | -                                       |
         * | RS_CAPTURE_ROLL_FINGER_EX                | 48    | RS-D, RS-F, RS-FC, RS-G10, RS-S60       |
         * | RS_CAPTURE_FLAT_LEFT_SIDE_PALM           | 64    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_RIGHT_SIDE_PALM          | 65    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_LEFT_WRITERS_PALM        | 66    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM       | 67    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_LEFT_UPPER_PALM          | 68    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_RIGHT_UPPER_PALM         | 69    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_LEFT_LOWER_PALM          | 70    | RS-F, RS-FC                              |
         * | RS_CAPTURE_FLAT_RIGHT_LOWER_PALM         | 71    | RS-F, RS-FC                              |
         *
         *
         * @param captureDirection [IN] Direction of finger input.
         *
         * | Macro Name                     | Value |
         * |-------------------------------------|-------|
         * | RS_CAPTURE_DIRECTION_DEFAULT        | 0     |
         * | RS_CAPTURE_DIRECTION_LEFT           | 1     |
         * | RS_CAPTURE_DIRECTION_RIGHT          | 2     |
         *
         *
         * @param captureOption [IN] Auto sensitivity option.
         *
         * | Macro Name                          | Value |
         * |--------------------------------------|-------|
         * | RS_AUTO_SENSITIVITY_NORMAL          | 0     |
         * | RS_AUTO_SENSITIVITY_HIGH            | 1     |
         * | RS_AUTO_SENSITIVITY_HIGHER          | 2     |
         * | RS_AUTO_SENSITIVITY_STRICT          | 3     |
         * | RS_AUTO_SENSITIVITY_DISABLED        | 4     |
         *
         *
         * @param withModeLED [IN] Enable or disable the mode LED. (G10 only)
         * - true  : LED on  
         * - false : LED off
         *
         * @return Function success status as an error code.
         * @retval 0    RS_SUCCESS
         * @retval -10  RS_ERR_SDK_UNINITIALIZED
         * @retval -13  RS_ERR_INVALID_PARAM
         * @retval -102 RS_ERR_INVALID_HANDLE
         * @retval -200 RS_ERR_INVALID_CAPTURE_MODE
         * @retval -212 RS_ERR_CAPTURE_IS_RUNNING
         *
         * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
         *
         * \par Example
         * @code
         * int deviceHandle = 0; // Assume valid handle
         *
         * int result = RS_SetCaptureModeWithDir(deviceHandle,
         *                                       RS_CAPTURE_ROLL_FINGER,
         *                                       RS_CAPTURE_DIRECTION_LEFT,
         *                                       RS_AUTO_SENSITIVITY_HIGH,
         *                                       false);
         *
         * if (result == RS_SUCCESS) {
         *     Console.WriteLine("Capture mode with direction set successfully.");
         * } else {
         *     Console.WriteLine($"Failed to set mode. Error: {result}");
         * }
         * @endcode
         */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetCaptureModeWithDir")]
        public static extern int RS_SetCaptureModeWithDir(int deviceHandle,
                                                        int captureMode,
                                                        int captureDirection,
                                                        int captureOption,
                                                        bool withModeLED);


        /**
        * @brief This function allows the user to specify custom coordinates and dimensions for fingerprint capture,
        * providing flexibility for different capture scenarios, such as capturing from specific areas of the sensor.
        *
        * @param deviceHandle [IN] Handle of the initialized device
        * @param imageX [IN] The X coordinate for the capture region.\n
        * | Model          | Value         |
        * |----------------|---------------|
        * | SG10, G10, S60 | 1600          |
        * | FC             | 2500          |
        *
        * @param imageY [IN] The Y coordinate for the capture region.\n
        * | Model          | Value         |
        * |----------------|---------------|
        * | SG10, G10, S60 | 1500          |
        * | FC             | 2500          |
        *
        * @param imageWidth [IN] The width of the capture region (200 to 10000).
        * @param imageHeight [IN] The height of the capture region (200 to 10000).
        *
        * @param captureOption [IN] Auto sensitivity option.\n
        * | Macro Name                      | Value |
        * |----------------------------------|-------|
        * | RS_AUTO_SENSITIVITY_NORMAL       | 0     |
        * | RS_AUTO_SENSITIVITY_HIGH         | 1     |
        * | RS_AUTO_SENSITIVITY_HIGHER       | 2     |
        * | RS_AUTO_SENSITIVITY_STRICT       | 3     |
        * | RS_AUTO_SENSITIVITY_DISABLED     | 4     |
        *
        * @param isFlat [IN] Whether the capture mode is flat (true) or roll (false).
        *
        * @return Function success status as an error code.
        * @retval 0     RS_SUCCESS
        * @retval -13   RS_ERR_INVALID_PARAM
        * @retval -10   RS_ERR_SDK_UNINITIALIZED
        * @retval -102  RS_ERR_INVALID_HANDLE
        * @retval -200  RS_ERR_INVALID_CAPTURE_MODE
        * @retval -212  RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan-S60, RealScan-SG10
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_SetManualCaptureMode(
        *         int deviceHandle,
        *         int imageX,
        *         int imageY,
        *         int imageWidth,
        *         int imageHeight,
        *         int captureOption,
        *         [MarshalAs(UnmanagedType.Bool)] bool isFlat
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_AUTO_SENSITIVITY_HIGH = 1;  // Example value
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume this handle is already obtained
        *
        *         int imageX = 1600;
        *         int imageY = 1500;
        *         int imageWidth = 2000;
        *         int imageHeight = 2000;
        *         int captureOption = RS_AUTO_SENSITIVITY_HIGH;
        *         bool isFlat = true;
        *
        *         int result = RS_SetManualCaptureMode(
        *             deviceHandle,
        *             imageX,
        *             imageY,
        *             imageWidth,
        *             imageHeight,
        *             captureOption,
        *             isFlat
        *         );
        *
        *         if (result == RS_SUCCESS)
        *             Console.WriteLine("Manual capture mode set successfully.");
        *         else
        *             Console.WriteLine($"Failed to set manual capture mode. Error code: {result}");
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetManualCaptureMode")]
        public static extern int RS_SetManualCaptureMode(int deviceHandle, int imageX, int imageY, int imageWidth, int imageHeight, int captureOption, bool isFlat);
        /**
        * @brief Retrieves the current capture mode and auto sensitivity option of the device.     
        * @param deviceHandle [IN] Handle of the initialized device.
        *
        * @param captureMode [OUT] Pointer to an integer that receives the current capture mode.\n
        * | Macro Name | Value | Device Model |
        * |-----------|-------|-------|
        * | \b RS_CAPTURE_DISABLED                | 0   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_ROLL_FINGER            | 1   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER     | 2   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS       | 3   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS | 4   | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS| 5   | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_PALM         | 6   | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_PALM        | 7   | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER_EX  | 18  | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS_EX    | 19  | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING      | 32  | - |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING_PREV | 33  | - |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING_PREV_ROI | 34 | - |
        * | \b RS_CAPTURE_ROLL_FINGER_EX         | 48  | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_SIDE_PALM    | 64  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_SIDE_PALM   | 65  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_WRITERS_PALM | 66  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM| 67  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_UPPER_PALM   | 68  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_UPPER_PALM  | 69  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_LOWER_PALM   | 70  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_LOWER_PALM  | 71  | RS-F, RS-FC |
        *
        * @param captureOption [OUT] Pointer to an integer that receives the current auto sensitivity option.\n
        * | Macro Name | Value |
        * |-------------|-------|
        * | \b RS_AUTO_SENSITIVITY_NORMAL    | 0 |
        * | \b RS_AUTO_SENSITIVITY_HIGH      | 1 |
        * | \b RS_AUTO_SENSITIVITY_HIGHER    | 2 |
        * | \b RS_AUTO_SENSITIVITY_STRICT    | 3 |
        * | \b RS_AUTO_SENSITIVITY_DISABLED  | 4 |
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE   
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_GetCaptureMode(
        *         int deviceHandle,
        *         out int captureMode,
        *         out int captureOption
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume this is a valid device handle
        *
        *         int mode;
        *         int option;
        *
        *         int result = RS_GetCaptureMode(deviceHandle, out mode, out option);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine($"Current capture mode: {mode}");
        *             Console.WriteLine($"Current capture option: {option}");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to get capture mode. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetCaptureMode")]
        public static extern int RS_GetCaptureMode(int deviceHandle, ref int captureMode, ref int captureOption);
        /**
        * @brief Gets the current capture mode, direction, and sensitivity option.
        *
        * This function retrieves the current settings for capture mode, direction, and
        * auto sensitivity for the specified device.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        *
        * @param captureMode [OUT] Pointer to retrieve the current capture mode.\n
        * | Macro Name | Value | Device Model |
        * |-----------|-------|--------------|
        * | \b RS_CAPTURE_DISABLED                | 0   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_ROLL_FINGER             | 1   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER      | 2   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS        | 3   | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS  | 4   | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS | 5   | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_PALM          | 6   | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_PALM         | 7   | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER_EX   | 18  | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS_EX     | 19  | RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING       | 32  | - |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING_PREV  | 33  | - |
        * | \b RS_CAPTURE_FLAT_AREA_SETTING_PREV_ROI | 34 | - |
        * | \b RS_CAPTURE_ROLL_FINGER_EX          | 48  | RS-D, RS-F, RS-FC, RS-G10, RS-S60 |
        * | \b RS_CAPTURE_FLAT_LEFT_SIDE_PALM     | 64  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_SIDE_PALM    | 65  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_WRITERS_PALM  | 66  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM | 67  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_UPPER_PALM    | 68  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_UPPER_PALM   | 69  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_LEFT_LOWER_PALM    | 70  | RS-F, RS-FC |
        * | \b RS_CAPTURE_FLAT_RIGHT_LOWER_PALM   | 71  | RS-F, RS-FC |
        *
        * @param captureDirection [OUT] Pointer to retrieve the current capture direction.\n
        * | Macro Name                       | Value |
        * |----------------------------------|-------|
        * | \b RS_CAPTURE_DIRECTION_DEFAULT  | 0     |
        * | \b RS_CAPTURE_DIRECTION_LEFT     | 1     |
        * | \b RS_CAPTURE_DIRECTION_RIGHT    | 2     |
        *
        * @param captureOption [OUT] Pointer to retrieve the current capture option.\n
        * | Macro Name                          | Value |
        * |--------------------------------------|-------|
        * | \b RS_AUTO_SENSITIVITY_NORMAL       | 0     |
        * | \b RS_AUTO_SENSITIVITY_HIGH         | 1     |
        * | \b RS_AUTO_SENSITIVITY_HIGHER       | 2     |
        * | \b RS_AUTO_SENSITIVITY_STRICT       | 3     |
        * | \b RS_AUTO_SENSITIVITY_DISABLED     | 4     |
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_GetCaptureModeWithDir(
        *         int deviceHandle,
        *         out int captureMode,
        *         out int captureDirection,
        *         out int captureOption
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle
        *
        *         int mode, direction, option;
        *
        *         int result = RS_GetCaptureModeWithDir(deviceHandle,
        *                                              out mode,
        *                                              out direction,
        *                                              out option);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine($"Mode: {mode}, Direction: {direction}, Option: {option}");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to get capture mode. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetCaptureModeWithDir")]
        public static extern int RS_GetCaptureModeWithDir(int deviceHandle, ref int captureMode, ref int captureDirection, ref int captureOption);

        /**
        * @brief Retrieves the image resolution based on the current capture mode.
        *
        * This function returns the final width and height (in pixels) of the image
        * that will be captured with the current capture mode.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        * @param imageWidth [OUT] Pointer to store the image width.
        * @param imageHeight [OUT] Pointer to store the image height.
        *
        * The returned resolution depends on the capture mode:\n
        * | Macro Name                          | imageWidth (px) | imageHeight (px) |
        * |-------------------------------------|------------------|------------------|
        * | \b RS_CAPTURE_ROLL_FINGER           | 800              | 750              |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER    | 800 / 500        | 750 / 500        |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS      | 900              | 900              |
        * | \b RS_CAPTURE_FLAT_XXXX_FOUR_FINGERS| 1600             | 1500             |
        * | \b RS_CAPTURE_FLAT_XXXX_PALM        | 2550 / 2500      | 2550 / 2500      |
        * | \b RS_CAPTURE_FLAT_SINGLE_FINGER_EX | 1600             | 1500             |
        * | \b RS_CAPTURE_FLAT_TWO_FINGERS_EX   | 1600             | 1500             |
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -201  \b RS_ERR_CAPTURE_DISABLED
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_GetImageSize(
        *         int deviceHandle,
        *         out int imageWidth,
        *         out int imageHeight
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle
        *         int width, height;
        *
        *         int result = RS_GetImageSize(deviceHandle, out width, out height);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine($"Capture image size: {width} x {height}");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to get image size. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetImageSize")]
        public static extern int RS_GetImageSize(int deviceHandle, ref int imageWidth, ref int imageHeight);

        /**
        * @brief Starts the fingerprint capture process.
        *
        * Initiates the fingerprint capture operation. You can choose between
        * automatic and manual capture mode, and specify a timeout for the operation.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        * @param autoCapture [IN] Capture mode flag:
        *        - true: Automatic capture
        *        - false: Manual capture
        * @param timeout [IN] Timeout duration in milliseconds.
        *        - Set to 0 for no timeout (infinite wait).
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -115  \b RS_ERR_SENSOR_DIRTY
        * @retval -116  \b RS_ERR_FINGER_EXIST
        * @retval -201  \b RS_ERR_CAPTURE_DISABLED
        * @retval -212  \b RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_StartCapture(
        *         int deviceHandle,
        *         [MarshalAs(UnmanagedType.Bool)] bool autoCapture,
        *         int timeout
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle
        *         bool autoCapture = true;
        *         int timeout = 5000; // 5 seconds
        *
        *         int result = RS_StartCapture(deviceHandle, autoCapture, timeout);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine("Capture started successfully.");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to start capture. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_StartCapture")]
        public static extern int RS_StartCapture(int deviceHandle, bool autoCapture, int timeout);

        /**
        * @brief Aborts the currently running fingerprint capture.
        *
        * Stops any ongoing capture operation immediately.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -114  \b RSC_ERR_CAPTURE_IS_NOT_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_AbortCapture(int deviceHandle);
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle
        *
        *         int result = RS_AbortCapture(deviceHandle);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine("Capture aborted successfully.");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to abort capture. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_AbortCapture")]
        public static extern int RS_AbortCapture(int deviceHandle);

        /**
        * @brief Registers a callback to receive raw image data during capture.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param captureCallback [IN] Callback function to receive raw capture data.
        *
        * @return Status code indicating success or failure.
        * @retval 0     \b RS_SUCCESS 
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED 
        * @retval -102  \b RS_ERR_INVALID_HANDLE 
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // Callback delegate signature matching native function
        *     [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        *     public delegate void RawCaptureCallback(
        *         int deviceHandle,
        *         int errorCode,
        *         IntPtr imageData,
        *         int imageWidth,
        *         int imageHeight
        *     );
        *
        *     // Import function
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_RegisterCaptureDataCallback(
        *         int deviceHandle,
        *         RawCaptureCallback callback
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     // Keep delegate instance alive (GC prevention)
        *     static RawCaptureCallback rawCallbackDelegate = RawCaptureCallbackHandler;
        *
        *     static void RawCaptureCallbackHandler(
        *         int deviceHandle,
        *         int errorCode,
        *         IntPtr imageData,
        *         int imageWidth,
        *         int imageHeight)
        *     {
        *         if (errorCode == RS_SUCCESS)
        *         {
        *             Console.WriteLine($"Raw image received ({imageWidth} x {imageHeight})");
        *
        *             // If needed, convert IntPtr to byte[]
        *             int size = imageWidth * imageHeight;
        *             byte[] buffer = new byte[size];
        *             Marshal.Copy(imageData, buffer, 0, size);
        *
        *             // Process buffer...
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Capture failed. Error code: {errorCode}");
        *         }
        *     }
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid device handle
        *
        *         int result = RS_RegisterCaptureDataCallback(deviceHandle, rawCallbackDelegate);
        *
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine("Raw capture callback registered.");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to register callback. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterCaptureDataCallback")]
        public static extern int RS_RegisterCaptureDataCallback(int deviceHandle, CaptureDataCallback captureCallback);
        /**
        * @brief Registers an advanced callback to receive enhanced raw capture data.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param captureCallback [IN] Advanced callback for raw capture data.
        *
        * @return Status code indicating success or failure.
        * @retval 0     \b RS_SUCCESS 
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED 
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * // Native RSImageInfo structure mapping (simplified)
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSImageInfo
        * {
        *     public int width;
        *     public int height;
        *     // Other fields can be added here if needed.
        * }
        *
        * class Program
        * {
        *     [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        *     public delegate void AdvCaptureCallback(
        *         int deviceHandle,
        *         int errorCode,
        *         IntPtr imageInfo
        *     );
        *
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_RegisterAdvCaptureDataCallback(
        *         int deviceHandle,
        *         AdvCaptureCallback callback
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     // Keep delegate alive to prevent GC
        *     static AdvCaptureCallback advCallbackDelegate = AdvCaptureCallbackHandler;
        *
        *     static void AdvCaptureCallbackHandler(
        *         int deviceHandle,
        *         int errorCode,
        *         IntPtr imageInfoPtr)
        *     {
        *         if (errorCode == RS_SUCCESS && imageInfoPtr != IntPtr.Zero)
        *         {
        *             RSImageInfo info = Marshal.PtrToStructure<RSImageInfo>(imageInfoPtr);
        *             Console.WriteLine($"Advanced capture received. Size: {info.width} x {info.height}");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Advanced capture failed. Error code: {errorCode}");
        *         }
        *     }
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle (initialized elsewhere)
        *
        *         int result = RS_RegisterAdvCaptureDataCallback(deviceHandle, advCallbackDelegate);
        *         if (result == RS_SUCCESS)
        *         {
        *             Console.WriteLine("Advanced capture callback registered successfully.");
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Failed to register advanced capture callback. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterAdvCaptureDataCallback")]
        public static extern int RS_RegisterAdvCaptureDataCallback(int deviceHandle, AdvCaptureDataCallbackInt captureCallback);
        /**
        * @brief Captures and returns a raw fingerprint image based on the current capture mode.
        *
        * Detects the fingerprint according to the set capture mode and returns raw image data.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        * @param timeout      [IN] Capture timeout in milliseconds. (must be > 0)
        * @param imageData    [OUT] Pointer to the buffer receiving raw image data. (unsigned char**)
        * @param imageWidth   [OUT] Width of the returned image.
        * @param imageHeight  [OUT] Height of the returned image.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeImageData(
        *         int deviceHandle,
        *         int timeout,
        *         out IntPtr imageData,
        *         out int imageWidth,
        *         out int imageHeight
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0; // Assume valid handle
        *         IntPtr imagePtr;
        *         int width, height;
        *         int timeout = 5000;
        *
        *         int result = RS_TakeImageData(deviceHandle, timeout,
        *                                       out imagePtr, out width, out height);
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine($"Captured raw image: {width} x {height}");
        *
        *             // Assuming 8-bit grayscale, size = width * height
        *             int size = width * height;
        *             byte[] rawImage = new byte[size];
        *             Marshal.Copy(imagePtr, rawImage, 0, size);
        *
        *             // TODO: use rawImage...
        *             // NOTE: If the SDK provides a function to free imagePtr,
        *             //       it should be called here.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Capture failed. Error code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeImageData")]
        public static extern int RS_TakeImageData(int deviceHandle, int timeout, ref IntPtr imageData, ref int imageWidth, ref int imageHeight);
        /**
        * @brief Captures a fingerprint image and returns raw image data.
        *
        * This function captures a fingerprint image using the specified device
        * handle and returns the raw image data through a pointer. It supports
        * optional LED control and is designed for use in C# and Java environments
        * where raw buffer access is preferred over CImage.
        *
        * @param deviceHandle [IN]  Handle of the initialized fingerprint device.
        * @param timeout      [IN]  Timeout value in milliseconds for the capture operation. If the timeout expires, RS_ERR_CAPTURE_TIMEOUT is returned.
        * @param fingerIndex  [IN]  Specifies the finger(s) to capture.\n
        *
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | RS_FINGER_ALL           | 0     |
        * | RS_FINGER_LEFT_LITTLE   | 1     |
        * | RS_FINGER_LEFT_RING     | 2     |
        * | RS_FINGER_LEFT_MIDDLE   | 3     |
        * | RS_FINGER_LEFT_INDEX    | 4     |
        * | RS_FINGER_LEFT_THUMB    | 5     |
        * | RS_FINGER_RIGHT_THUMB   | 6     |
        * | RS_FINGER_RIGHT_INDEX   | 7     |
        * | RS_FINGER_RIGHT_MIDDLE  | 8     |
        * | RS_FINGER_RIGHT_RING    | 9     |
        * | RS_FINGER_RIGHT_LITTLE  | 10    |
        * | RS_FINGER_TWO_THUMB     | 11    |
        * | RS_FINGER_LEFT_FOUR     | 12    |
        * | RS_FINGER_RIGHT_FOUR    | 13    |
        * | RS_FINGER_TWO_LEFT1     | 14    |
        * | RS_FINGER_TWO_LEFT2     | 15    |
        * | RS_FINGER_TWO_RIGHT2    | 16    |
        * | RS_FINGER_TWO_RIGHT1    | 17    |
        * | RS_PALM_LEFT            | 18    |
        * | RS_PALM_RIGHT           | 19    |
        *
        * @param withLED      [IN]  Enables device LED during capture (only RealScan-G10(i) supports this).
        * @param imageData    [OUT] Pointer to buffer receiving raw fingerprint image data.
        * @param imageWidth   [OUT] Pointer to integer receiving the image width.
        * @param imageHeight  [OUT] Pointer to integer receiving the image height.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        *
        * @note Supported devices: RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // P/Invoke declaration for RS_TakeImageDataEx
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeImageDataEx(
        *         int deviceHandle,
        *         int timeout,
        *         int fingerIndex,
        *         [MarshalAs(UnmanagedType.Bool)] bool withLED,
        *         out IntPtr imageData,
        *         out int imageWidth,
        *         out int imageHeight
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     // Finger index macros (optional convenience constants)
        *     public const int RS_FINGER_LEFT_INDEX = 4;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0;          // Assume valid device
        *         int timeout = 3000;            // Timeout in milliseconds
        *         int fingerIndex = RS_FINGER_LEFT_INDEX;
        *         bool withLED = true;           // Enable LED (RealScan-G10(i) only)
        *
        *         IntPtr imagePtr;
        *         int width, height;
        *
        *         int result = RS_TakeImageDataEx(
        *             deviceHandle,
        *             timeout,
        *             fingerIndex,
        *             withLED,
        *             out imagePtr,
        *             out width,
        *             out height
        *         );
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine($"Image captured: {width} x {height}");
        *
        *             // Assuming 8-bit grayscale: buffer size = width * height
        *             int size = width * height;
        *             byte[] imageData = new byte[size];
        *             Marshal.Copy(imagePtr, imageData, 0, size);
        *
        *             // TODO: use imageData...
        *             // NOTE: If the SDK provides a function to free imagePtr,
        *             //       it should be called here to release native memory.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Error capturing image. Code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeImageDataEx")]
        public static extern int RS_TakeImageDataEx(int deviceHandle, int timeout, int fingerIndex, bool withLED, ref IntPtr imageData, ref int imageWidth, ref int imageHeight);
        /**
        * @brief Captures a fingerprint image with optional LED control and returns the image. (Windows only)
        *
        * This function captures a fingerprint image using the RealScan-G10(i) device.
        * It allows optional control of the device's LED during the capture
        * process. The captured image is returned through the provided CImage
        * pointer.
        *
        * @param deviceHandle [IN]  Handle of the initialized fingerprint device.
        * @param timeout      [IN]  Timeout value in milliseconds for the capture operation. If the operation times out, RS_ERR_CAPTURE_TIMEOUT will be returned.     
        * @param fingerIndex  [IN]  Specifies which finger(s) to capture.\n
        *
        * | Macro Name             | Value |
        * |------------------------|-------|
        * | RS_FINGER_ALL          | 0     |
        * | RS_FINGER_LEFT_LITTLE  | 1     |
        * | RS_FINGER_LEFT_RING    | 2     |
        * | RS_FINGER_LEFT_MIDDLE  | 3     |
        * | RS_FINGER_LEFT_INDEX   | 4     |
        * | RS_FINGER_LEFT_THUMB   | 5     |
        * | RS_FINGER_RIGHT_THUMB  | 6     |
        * | RS_FINGER_RIGHT_INDEX  | 7     |
        * | RS_FINGER_RIGHT_MIDDLE | 8     |
        * | RS_FINGER_RIGHT_RING   | 9     |
        * | RS_FINGER_RIGHT_LITTLE | 10    |
        * | RS_FINGER_TWO_THUMB    | 11    |
        * | RS_FINGER_LEFT_FOUR    | 12    |
        * | RS_FINGER_RIGHT_FOUR   | 13    |
        * | RS_FINGER_TWO_LEFT1    | 14    |
        * | RS_FINGER_TWO_LEFT2    | 15    |
        * | RS_FINGER_TWO_RIGHT2   | 16    |
        * | RS_FINGER_TWO_RIGHT1   | 17    |
        * | RS_PALM_LEFT           | 18    |
        * | RS_PALM_RIGHT          | 19    |
        *
        * @param withLED      [IN]  If true, enables the device LED during capture.
        *                           Only supported on RealScan-G10(i).
        * @param image        [OUT] Pointer to a CImage* that will receive the captured image data.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        * 
        * @note Supported devices: RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // NOTE:
        *     // RS_TakeImageEx returns a CImage* (C++ MFC/ATL type).
        *     // In C#, it is exposed as an opaque IntPtr. Direct manipulation of CImage
        *     // is not possible from managed code; for C#/Java, RS_TakeImageDataEx is recommended.
        *
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeImageEx(
        *         int deviceHandle,
        *         int timeout,
        *         int fingerIndex,
        *         [MarshalAs(UnmanagedType.Bool)] bool withLED,
        *         out IntPtr image);
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_FINGER_LEFT_INDEX = 4;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 1;   // Assume valid device handle
        *         int timeout = 3000;     // 3 seconds
        *         int fingerIndex = RS_FINGER_LEFT_INDEX;
        *         bool withLED = true;    // Only RealScan-G10(i) supports LED control
        *
        *         IntPtr imagePtr;        // CImage* as opaque pointer
        *
        *         int result = RS_TakeImageEx(
        *             deviceHandle,
        *             timeout,
        *             fingerIndex,
        *             withLED,
        *             out imagePtr
        *         );
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine("Capture succeeded (CImage* received).");
        *             // CImage* is a native C++ object; it must be handled/freed on the native side.
        *             // For C# applications, prefer RS_TakeImageDataEx to work with raw byte buffers.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Capture failed. Error: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeImageEx")]
        public static extern int RS_TakeImageEx(int deviceHandle, int timeout, int fingerIndex, bool withLED, ref IntPtr image);
        /**
        * @brief Captures a fingerprint image and returns it as a CImage object. (Windows Only)
        *
        * This function captures a fingerprint image using the specified device and
        * returns the result through a CImage pointer. It supports controlling the device
        * LED based on the capture result. For RealScan-10 and RealScan-G10(i), the LED
        * will change color based on the capture status: green for success, and red for
        * failure.
        *
        * @param deviceHandle  [IN]  Handle of the initialized fingerprint device.
        * @param timeout       [IN]  Timeout value in milliseconds for the capture
        *                             operation. If the timeout expires, RS_ERR_CAPTURE_TIMEOUT is returned.
        * @param fingerIndex   [IN]  Specifies the finger(s) to capture. A value
        *                             representing a specific finger index or group.\n
        *                             (e.g. RS_FINGER_LEFT_INDEX, RS_FINGER_RIGHT_THUMB, etc.)
        * @param withLED       [IN]  Whether to enable the device LED during
        *                             capture (only supported by RealScan-G10(i)).
        * @param image         [OUT] Pointer to a CImage* that will receive the captured
        *                             fingerprint image. On success, *image will point
        *                             to a valid CImage instance containing the captured
        *                             fingerprint.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -5   \b RS_ERR_NO_DATA
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        *
        * @note Supported devices: RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // RS_TakeCurrentImageEx returns a CImage** → Represented as IntPtr in C#
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeCurrentImageEx(
        *         int deviceHandle,
        *         int timeout,
        *         int fingerIndex,
        *         [MarshalAs(UnmanagedType.Bool)] bool withLED,
        *         out IntPtr image
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_FINGER_LEFT_INDEX = 4;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0;       // Assume valid device handle
        *         int timeout = 3000;         // 3 seconds
        *         int fingerIndex = RS_FINGER_LEFT_INDEX;
        *         bool withLED = true;        // LED only supported on RealScan-G10(i)
        *
        *         IntPtr imagePtr;            // CImage* returned from native code
        *
        *         int result = RS_TakeCurrentImageEx(
        *             deviceHandle,
        *             timeout,
        *             fingerIndex,
        *             withLED,
        *             out imagePtr
        *         );
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine("Capture succeeded (CImage* received).");
        *             // NOTE:
        *             // imagePtr is a pointer to a native CImage object.
        *             // C# cannot directly access CImage contents.
        *             //
        *             // If the SDK provides a function like RS_FreeImage(CImage*),
        *             // you MUST call it to avoid memory leaks.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Capture failed. Error: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeCurrentImageEx")]
        public static extern int RS_TakeCurrentImageEx(int deviceHandle, int timeout, int fingerIndex, bool withLED, ref IntPtr image);
        /**
        * @brief Captures a fingerprint image and returns the raw image data. (Windows Only)
        *
        * It supports controlling the device LED based on the capture result. For RealScan-10
        * and RealScan-G10(i), the LED will change color based on the capture status:
        * green for success, and red for failure.
        *
        * @param deviceHandle  [IN]  Handle of the initialized fingerprint device.
        * @param timeout       [IN]  Timeout value in milliseconds for the capture
        *                             operation. If the timeout expires, RS_ERR_CAPTURE_TIMEOUT is returned.
        * @param fingerIndex   [IN]  Specifies the finger(s) to capture. A value
        *                             representing a specific finger index or group.
        * @param withLED       [IN]  Whether to enable the device LED during
        *                             capture (only supported by RealScan-G10(i)).
        * @param image         [OUT] Pointer to a buffer receiving the raw
        *                             fingerprint image data. The buffer will contain the raw image.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -5   \b RS_ERR_NO_DATA
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        *
        * @note Supported devices: RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // P/Invoke for RS_TakeCurrentImageDataEx
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeCurrentImageDataEx(
        *         int deviceHandle,
        *         int timeout,
        *         int fingerIndex,
        *         [MarshalAs(UnmanagedType.Bool)] bool withLED,
        *         out IntPtr imageData
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     // Optional: finger index constant for convenience
        *     public const int RS_FINGER_LEFT_INDEX = 4;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0;      // Assume valid device
        *         int timeout = 3000;        // Timeout in milliseconds
        *         int fingerIndex = RS_FINGER_LEFT_INDEX;
        *         bool withLED = true;       // Enable LED (RealScan-G10(i) only)
        *
        *         IntPtr imagePtr;
        *
        *         int result = RS_TakeCurrentImageDataEx(
        *             deviceHandle,
        *             timeout,
        *             fingerIndex,
        *             withLED,
        *             out imagePtr
        *         );
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine("Image captured successfully.");
        *
        *             // NOTE:
        *             // The SDK returns a pointer to raw image data (unsigned char*).
        *             // To convert it into a managed byte[] buffer, the image size
        *             // (width * height) must be known from the current capture mode
        *             // or another API such as RS_GetImageSize.
        *
        *             // Example (pseudo code assuming width/height are known):
        *             // int width = ...;
        *             // int height = ...;
        *             // int size = width * height; // for 8-bit grayscale
        *             // byte[] buffer = new byte[size];
        *             // Marshal.Copy(imagePtr, buffer, 0, size);
        *             //
        *             // TODO: process buffer...
        *             //
        *             // If the SDK provides a function to free imagePtr, call it here.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Error capturing image. Code: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeCurrentImageDataEx")]
        public static extern int RS_TakeCurrentImageDataEx(int deviceHandle, int timeout, int fingerIndex, bool withLED, ref IntPtr imageData, ref int imageWidth, ref int imageHeight);
        /**
        * @brief Same as RS_TakeCurrentImage except that it returns an array of gray pixel data, not a CImage object. 
        *
        *
        * @param deviceHandle [IN] Handle to the opened fingerprint device.
        * @param timeout [IN] Timeout in milliseconds for the capture operation. If not set, returns RS_ERR_CAPTURE_TIMEOUT.
        * @param imageData [OUT] Pointer to the buffer that will hold the raw image data.
        * @param imageWidth [OUT] Pointer to store the width of the captured image.
        * @param imageHeight [OUT] Pointer to store the height of the captured image.
        *
        * @return Function success status as an error code.
        * @retval 0    \b RS_SUCCESS
        * @retval -5   \b RS_ERR_NO_DATA
        * @retval -10  \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * @retval -115 \b RS_ERR_SENSOR_DIRTY
        * @retval -116 \b RS_ERR_FINGER_EXIST
        * @retval -201 \b RS_ERR_CAPTURE_DISABLED
        * @retval -202 \b RS_ERR_CAPTURE_TIMEOUT
        * @retval -206 \b RS_ERR_ROLL_PART_LIFT
        * @retval -207 \b RS_ERR_ROLL_DIRTY
        * @retval -208 \b RS_ERR_ROLL_TOO_FAST
        * @retval -209 \b RS_ERR_ROLL_SHIFTED
        * @retval -210 \b RS_ERR_ROLL_DRY
        * @retval -211 \b RS_ERR_ROLL_WRONG_DIR
        * @retval -214 \b RS_ERR_ROLL_TOO_SHORT
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // RS_TakeCurrentImageData:
        *     // int RS_TakeCurrentImageData(int deviceHandle, int timeout,
        *     //                             unsigned char** imageData,
        *     //                             int* imageWidth, int* imageHeight);
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_TakeCurrentImageData(
        *         int deviceHandle,
        *         int timeout,
        *         out IntPtr imageData,
        *         out int imageWidth,
        *         out int imageHeight
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *
        *     static void Main()
        *     {
        *         int deviceHandle = 0;  // Assume this is initialized properly
        *         int timeout = 3000;    // 3-second timeout
        *
        *         IntPtr imagePtr;
        *         int width;
        *         int height;
        *
        *         int result = RS_TakeCurrentImageData(deviceHandle,
        *                                             timeout,
        *                                             out imagePtr,
        *                                             out width,
        *                                             out height);
        *
        *         if (result == RS_SUCCESS && imagePtr != IntPtr.Zero)
        *         {
        *             Console.WriteLine($"Capture succeeded: {width} x {height}");
        *
        *             // Assuming 8-bit grayscale pixels: buffer size = width * height
        *             int size = width * height;
        *             byte[] imageData = new byte[size];
        *             Marshal.Copy(imagePtr, imageData, 0, size);
        *
        *             // TODO: process imageData (save to file, convert to Bitmap, etc.)
        *             //
        *             // NOTE:
        *             // If the SDK allocates the buffer internally, make sure to call
        *             // the corresponding free function (e.g. RS_FreeImageBuffer)
        *             // after you are done using imageData.
        *         }
        *         else
        *         {
        *             Console.WriteLine($"Capture failed. Error: {result}");
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_TakeCurrentImageData")]
        public static extern int RS_TakeCurrentImageData(int deviceHandle, int timeout, ref IntPtr imageData, ref int imageWidth, ref int imageHeight);
        /**
        * @brief Verifies whether the sequence of individual finger images matches
        * the order of fingers in the slap image.
        *
        * This function checks if all fingers are properly captured by verifying
        * the order of individual finger images against the slap image using
        * internal matching logic.
        *
        * @param numOfFinger           [IN] Number of individual finger images.
        * @param fingerImageData       [OUT] Array of individual finger image data.
        * @param fingerImageWidth      [OUT] Image Width.     
        * @param fingerImageHeight     [OUT] Image Height.     
        * @param slapImageData         [IN] Image data of the slap image.
        * @param slapImageWidth        [IN] Width of the slap image.
        * @param slapImageHeight       [IN] Height of the slap image.
        * @param slapType              [IN] Type of slap image.\n
        * | Parameter Name           | Value |
        * |--------------------------|-------|
        * | RS_SLAP_LEFT_FOUR        | 1     |
        * | RS_SLAP_RIGHT_FOUR       | 2     |
        * | RS_SLAP_FOUR_FINGER      | 3     |
        * | RS_SLAP_TWO_THUMB        | 4     |
        * | RS_SLAP_TWO_FINGER       | 5     |
        * | RS_SLAP_ONE_FINGER       | 6     |
        * | RS_SLAP_ONE_FINGER_ROLL  | 7     |
        * | RS_SLAP_MANUAL           | 8     |
        * | RS_SLAP_MANUAL_INI       | 9     |
        * | RS_SLAP_MANUAL_ROLL      | 10    |
        * | RS_SLAP_SEAL             | 21    |
        * @param fingerSequenceInSlap  [OUT] Array representing the matched
        * sequence of fingers in the slap. Refer to `slapInfo.fingerType` in
        * `RS_Segment` for valid values.
        * @param securityLevel         [IN] Security level for false acceptance
        * rate (FAR). Recommended value: 1/100,000.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -13   \b RS_ERR_INVALID_PARAM
        * @retval -14   \b RS_ERR_MEM_FULL
        * @retval -100  \b RS_ERR_NO_DEVICE
        * @retval -215  \b RS_ERR_CANNOT_SEGMENT   
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * class Program
        * {
        *     // Native signature assumption:
        *     // int RS_SequenceCheck(
        *     //     int numOfFinger,
        *     //     unsigned char** fingerImageData,
        *     //     int* fingerImageWidth,
        *     //     int* fingerImageHeight,
        *     //     unsigned char* slapImageData,
        *     //     int slapImageWidth,
        *     //     int slapImageHeight,
        *     //     int slapType,
        *     //     int* fingerSequenceInSlap,
        *     //     int securityLevel);
        *
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_SequenceCheck(
        *         int numOfFinger,
        *         ref IntPtr fingerImageData,
        *         ref int fingerImageWidth,
        *         ref int fingerImageHeight,
        *         IntPtr slapImageData,
        *         int slapImageWidth,
        *         int slapImageHeight,
        *         int slapType,
        *         out int fingerSequenceInSlap,
        *         int securityLevel
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_SLAP_LEFT_FOUR = 1;
        *
        *     static void Main()
        *     {
        *         // Example: 1 finger image + 1 slap image, both already loaded into byte[]
        *         byte[] fingerImage = LoadFingerImage(); // user-defined
        *         int fingerWidth = 200;                  // example width
        *         int fingerHeight = 500;                 // example height
        *
        *         byte[] slapImage = LoadSlapImage();     // user-defined
        *         int slapWidth = 800;
        *         int slapHeight = 400;
        *
        *         // Pin managed arrays so we can pass pointers to native code
        *         GCHandle fingerHandle = GCHandle.Alloc(fingerImage, GCHandleType.Pinned);
        *         GCHandle slapHandle   = GCHandle.Alloc(slapImage,   GCHandleType.Pinned);
        *
        *         try
        *         {
        *             IntPtr fingerPtr = fingerHandle.AddrOfPinnedObject();
        *             IntPtr slapPtr   = slapHandle.AddrOfPinnedObject();
        *
        *             int numOfFinger = 1;
        *             int seqResult;
        *             int securityLevel = 100000; // 1/100,000
        *
        *             int result = RS_SequenceCheck(
        *                 numOfFinger,
        *                 ref fingerPtr,
        *                 ref fingerWidth,
        *                 ref fingerHeight,
        *                 slapPtr,
        *                 slapWidth,
        *                 slapHeight,
        *                 RS_SLAP_LEFT_FOUR,
        *                 out seqResult,
        *                 securityLevel
        *             );
        *
        *             if (result == RS_SUCCESS)
        *             {
        *                 Console.WriteLine($"Matched sequence: {seqResult}");
        *             }
        *             else
        *             {
        *                 Console.WriteLine($"Sequence check failed. Error code: {result}");
        *             }
        *         }
        *         finally
        *         {
        *             if (fingerHandle.IsAllocated) fingerHandle.Free();
        *             if (slapHandle.IsAllocated)   slapHandle.Free();
        *         }
        *     }
        *
        *     // Placeholder loader functions
        *     static byte[] LoadFingerImage() { return new byte[200 * 500]; }
        *     static byte[] LoadSlapImage()   { return new byte[800 * 400]; }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SequenceCheck")]
        public static extern int RS_SequenceCheck(int numOfFinger, ref IntPtr fingerImageData, ref int fingerImageWidth, ref int fingerImageHeight, IntPtr slapImageData, int slapImageWidth, int slapImageHeight,
         int slapType, ref int fingerSequenceInSlap, int securityLevel);


        /**
           * @brief Verifies whether the sequence of individual finger images matches
           * the order of fingers in the slap image, considering missing fingers.
           *
           * This function performs sequence verification between individual finger
           * images and a slap image, similar to RS_SequenceCheck. In addition, it allows
           * the caller to specify missing fingers (e.g., amputated or unavailable fingers)
           * through the RSMissingInfo structure, so that the sequence check can be
           * performed while excluding those fingers.
           *
           * @param numOfFinger           [IN] Number of individual finger images.
           * @param fingerImageData       [OUT] Array of individual finger image data.
           * @param fingerImageWidth      [OUT] Image width of each individual finger image.
           * @param fingerImageHeight     [OUT] Image height of each individual finger image.
           * @param slapImageData         [IN] Image data of the slap image.
           * @param slapImageWidth        [IN] Width of the slap image.
           * @param slapImageHeight       [IN] Height of the slap image.
           * @param slapType              [IN] Type of slap image.\n
           * | Parameter Name           | Value |
           * |--------------------------|-------|
           * | RS_SLAP_LEFT_FOUR        | 1     |
           * | RS_SLAP_RIGHT_FOUR       | 2     |
           * | RS_SLAP_FOUR_FINGER      | 3     |
           * | RS_SLAP_TWO_THUMB        | 4     |
           * | RS_SLAP_TWO_FINGER       | 5     |
           * | RS_SLAP_ONE_FINGER       | 6     |
           * | RS_SLAP_ONE_FINGER_ROLL  | 7     |
           * | RS_SLAP_MANUAL           | 8     |
           * | RS_SLAP_MANUAL_INI       | 9     |
           * | RS_SLAP_MANUAL_ROLL      | 10    |
           * | RS_SLAP_SEAL             | 21    |
           *
           * @param fingerSequenceInSlap  [OUT] Variable that receives the matched
           * sequence of the individual finger within the slap image.
           * Refer to `slapInfo.fingerType` in `RS_Segment` for valid values.
           *
           * @param securityLevel         [IN] Security level for false acceptance
           * rate (FAR). Recommended value: 1/100,000.
           *
           * @param missingInfo           [IN] Pointer to an RSMissingInfo structure
           * that specifies which fingers are missing and should be excluded from
           * the sequence verification.
           *
           * @return Function success status as an error code.
           * @retval 0     \b RS_SUCCESS
           * @retval -13   \b RS_ERR_INVALID_PARAM
           * @retval -14   \b RS_ERR_MEM_FULL
           * @retval -100  \b RS_ERR_NO_DEVICE
           * @retval -215  \b RS_ERR_CANNOT_SEGMENT
           *
           * @note This function should be used instead of RS_SequenceCheck when
           * one or more fingers are missing.
           *
           * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
           *       RealScan-G10(i), RealScan S60, RealScan SG10
           *
           * \par Example (C#)
           * @code
           * using System;
           * using System.Runtime.InteropServices;
           *
           * [StructLayout(LayoutKind.Sequential)]
           * public struct RSMissingInfo
           * {
           *     public int nFirstfinger;
           *     public int nSecondfinger;
           *     public int nThirdfinger;
           *     public int nFourthfinger;
           * }
           *
           * class Program
           * {
           *     [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SequenceCheckMissingFinger")]
           *     public static extern int RS_SequenceCheckMissingFinger(
           *         int numOfFinger,
           *         ref IntPtr fingerImageData,
           *         ref int fingerImageWidth,
           *         ref int fingerImageHeight,
           *         IntPtr slapImageData,
           *         int slapImageWidth,
           *         int slapImageHeight,
           *         int slapType,
           *         ref int fingerSequenceInSlap,
           *         int securityLevel,
           *         ref RSMissingInfo missingInfo
           *     );
           *
           *     public const int RS_SUCCESS = 0;
           *     public const int RS_SLAP_LEFT_FOUR = 1;
           *
           *     static void Main()
           *     {
           *         // Example finger and slap images (already loaded)
           *         byte[] fingerImage = new byte[200 * 500];
           *         byte[] slapImage   = new byte[800 * 400];
           *
           *         int fingerWidth = 200;
           *         int fingerHeight = 500;
           *         int slapWidth = 800;
           *         int slapHeight = 400;
           *
           *         GCHandle fingerHandle = GCHandle.Alloc(fingerImage, GCHandleType.Pinned);
           *         GCHandle slapHandle   = GCHandle.Alloc(slapImage,   GCHandleType.Pinned);
           *
           *         try
           *         {
           *             IntPtr fingerPtr = fingerHandle.AddrOfPinnedObject();
           *             IntPtr slapPtr   = slapHandle.AddrOfPinnedObject();
           *
           *             int seqResult = -1;
           *             int securityLevel = 100000; // 1 / 100,000
           *
           *             RSMissingInfo missingInfo = new RSMissingInfo
           *             {
           *                 nFirstfinger  = 1, // Example: first finger is missing
           *                 nSecondfinger = 0,
           *                 nThirdfinger  = 0,
           *                 nFourthfinger = 0
           *             };
           *
           *             int result = RS_SequenceCheckMissingFinger(
           *                 1,
           *                 ref fingerPtr,
           *                 ref fingerWidth,
           *                 ref fingerHeight,
           *                 slapPtr,
           *                 slapWidth,
           *                 slapHeight,
           *                 RS_SLAP_LEFT_FOUR,
           *                 ref seqResult,
           *                 securityLevel,
           *                 ref missingInfo
           *             );
           *
           *             if (result == RS_SUCCESS)
           *             {
           *                 Console.WriteLine($"Matched finger sequence: {seqResult}");
           *             }
           *             else
           *             {
           *                 Console.WriteLine($"Sequence check failed. Error code: {result}");
           *             }
           *         }
           *         finally
           *         {
           *             if (fingerHandle.IsAllocated) fingerHandle.Free();
           *             if (slapHandle.IsAllocated)   slapHandle.Free();
           *         }
           *     }
           * }
           * @endcode
           */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SequenceCheckMissingFinger")]
        public static extern int RS_SequenceCheckMissingFinger(
            int numOfFinger,
            ref IntPtr fingerImageData,
            ref int fingerImageWidth,
            ref int fingerImageHeight,
            IntPtr slapImageData,
            int slapImageWidth,
            int slapImageHeight,
            int slapType,
            ref int fingerSequenceInSlap,
            int securityLevel,
            ref RSMissingInfo missingInfo
        );

        /**
        * @brief Segments a slap image and saves each finger image to individual
        * files.
        *
        * This function is designed for JAVA application integration and provides
        * the same segmentation capability as RS_Segment. The only difference is
        * the output format: this function stores the segmented finger images into
        * files instead of returning them as buffers.
        *
        * @param imageData     [IN]  Raw slap image data.
        * @param imageWidth    [IN]  Width of the slap image in pixels.
        * @param imageHeight   [IN]  Height of the slap image in pixels.
        * @param slapType      [IN]  Type of slap image.\n
        * | Parameter Name           | Value |
        * |--------------------------|-------|
        * | RS_SLAP_LEFT_FOUR        | 1     |
        * | RS_SLAP_RIGHT_FOUR       | 2     |
        * | RS_SLAP_FOUR_FINGER      | 3     |
        * | RS_SLAP_TWO_THUMB        | 4     |
        * | RS_SLAP_TWO_FINGER       | 5     |
        * | RS_SLAP_ONE_FINGER       | 6     |
        * | RS_SLAP_ONE_FINGER_ROLL  | 7     |
        * | RS_SLAP_MANUAL           | 8     |
        * | RS_SLAP_MANUAL_INI       | 9     |
        * | RS_SLAP_MANUAL_ROLL      | 10    |
        * | RS_SLAP_SEAL             | 21    |
        * @param numOfFinger   [OUT] Pointer to store the number of segmented
        * fingers.
        * @param slapInfo      [OUT] Pointer to receive array of RSSlapInfo
        * structures.
        * @param outFilename   [IN]  Base filename used to save output finger image
        * files. File extensions and indices will be automatically appended.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -13   \b RS_ERR_INVALID_PARAM
        * @retval -14   \b RS_ERR_MEM_FULL
        * @retval -100  \b RS_ERR_NO_DEVICE     
        * @retval -215  \b RS_ERR_CANNOT_SEGMENT
        * @retval -216  \b RS_ERR_CANNOT_GET_QUALITY
        * @retval -217  \b RS_ERR_SEGMENT_FEWER_FINGER
        * @retval -218  \b RS_ERR_SEGMENT_WRONG_HAND
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * // Native RSSlapInfo structure mapping (simplified / partial)
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSSlapInfo
        * {
        *     public int fingerType;
        *     public int x;
        *     public int y;
        *     public int width;
        *     public int height;
        *     // Other fields can be added here according to RS_API.h
        * }
        *
        * class Program
        * {
        *     // Native signature (assumed):
        *     // int RS_SegmentImages(
        *     //     unsigned char* imageData,
        *     //     int imageWidth,
        *     //     int imageHeight,
        *     //     int slapType,
        *     //     int* numOfFinger,
        *     //     RSSlapInfo** slapInfo,
        *     //     const char* outFilename);
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_SegmentImages(
        *         IntPtr imageData,
        *         int imageWidth,
        *         int imageHeight,
        *         int slapType,
        *         out int numOfFinger,
        *         out IntPtr slapInfo,
        *         [MarshalAs(UnmanagedType.LPStr)] string outFilename
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_SLAP_LEFT_FOUR = 1;
        *
        *     static void Main()
        *     {
        *         // Example slap image buffer (must be filled with real image data)
        *         int width = 800;
        *         int height = 750;
        *         byte[] slapImage = new byte[width * height]; // 8-bit grayscale example
        *
        *         // Pin the managed array to get a stable pointer
        *         GCHandle slapHandle = GCHandle.Alloc(slapImage, GCHandleType.Pinned);
        *
        *         try
        *         {
        *             IntPtr imagePtr = slapHandle.AddrOfPinnedObject();
        *             int numOfFingers;
        *             IntPtr slapInfoPtr;
        *
        *             int result = RS_SegmentImages(
        *                 imagePtr,
        *                 width,
        *                 height,
        *                 RS_SLAP_LEFT_FOUR,
        *                 out numOfFingers,
        *                 out slapInfoPtr,
        *                 "finger_output"   // Output base filename
        *             );
        *
        *             if (result == RS_SUCCESS)
        *             {
        *                 Console.WriteLine($"Segmentation succeeded. Fingers detected: {numOfFingers}");
        *
        *                 // If you need to inspect slapInfo array:
        *                 // RSSlapInfo info = Marshal.PtrToStructure<RSSlapInfo>(slapInfoPtr);
        *                 // For multiple fingers, step by Marshal.SizeOf<RSSlapInfo>().
        *
        *                 // NOTE: If the SDK allocates slapInfo, check RS_API for
        *                 //       the proper function to free this memory.
        *             }
        *             else
        *             {
        *                 Console.WriteLine($"Segmentation failed. Error code: {result}");
        *             }
        *         }
        *         finally
        *         {
        *             if (slapHandle.IsAllocated)
        *                 slapHandle.Free();
        *         }
        *     }
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SegmentImages")]
        public static extern int RS_SegmentImages(IntPtr imageData, int imageWidth, int imageHeight, int slapType, ref int numOfFinger, ref IntPtr slapInfoArray_, string outFilename);

        /**
        * @brief Segments a slap fingerprint image into four individual fingers and
        * returns the results in separate buffers. This function is designed for
        * cross-platform data exchange, including C# applications.
        *
        * @param imageData           [IN]  Raw grayscale slap image data buffer.
        * @param imageWidth          [IN]  Width of the slap image. (in pixels)
        * @param imageHeight         [IN]  Height of the slap image. (in pixels)
        * @param slapType            [IN]  Type of slap image, defined by constants
        *                                  such as RS_SLAP_LEFT_FOUR, RS_SLAP_RIGHT_FOUR, etc.
        * @param numOfFinger         [OUT] Pointer to an integer to receive the
        *                                  number of segmented fingers.
        * @param slapInfo            [OUT] Pointer to an array of RSSlapInfo
        *                                  structures containing segmented information.
        * @param fingerImageData1    [OUT] Pointer to a buffer receiving the image
        *                                  data for the first finger.
        * @param fingerImageDataWidth1  [OUT] Pointer to an integer receiving the
        *                                  width of the first finger image.
        * @param fingerImageDataHeight1 [OUT] Pointer to an integer receiving the
        *                                  height of the first finger image.
        * @param fingerImageData2    [OUT] Pointer to a buffer receiving the image
        *                                  data for the second finger.
        * @param fingerImageDataWidth2  [OUT] Pointer to an integer receiving the
        *                                  width of the second finger image.
        * @param fingerImageDataHeight2 [OUT] Pointer to an integer receiving the
        *                                  height of the second finger image.
        * @param fingerImageData3    [OUT] Pointer to a buffer receiving the image
        *                                  data for the third finger.
        * @param fingerImageDataWidth3  [OUT] Pointer to an integer receiving the
        *                                  width of the third finger image.
        * @param fingerImageDataHeight3 [OUT] Pointer to an integer receiving the
        *                                  height of the third finger image.
        * @param fingerImageData4    [OUT] Pointer to a buffer receiving the image
        *                                  data for the fourth finger.
        * @param fingerImageDataWidth4  [OUT] Pointer to an integer receiving the
        *                                  width of the fourth finger image.
        * @param fingerImageDataHeight4 [OUT] Pointer to an integer receiving the
        *                                  height of the fourth finger image.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -13   \b RS_ERR_INVALID_PARAM
        * @retval -14   \b RS_ERR_MEM_FULL
        * @retval -100  \b RS_ERR_NO_DEVICE     
        * @retval -215  \b RS_ERR_CANNOT_SEGMENT
        * @retval -216  \b RS_ERR_CANNOT_GET_QUALITY
        * @retval -217  \b RS_ERR_SEGMENT_FEWER_FINGER
        * @retval -218  \b RS_ERR_SEGMENT_WRONG_HAND
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * // Native RSSlapInfo structure mapping (simplified)
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSSlapInfo
        * {
        *     public int fingerType;
        *     public int x;
        *     public int y;
        *     public int width;
        *     public int height;
        *     // Add other fields from RS_API.h as needed.
        * }
        *
        * class Program
        * {
        *     // Native signature (assumed):
        *     // int RS_Segment4(
        *     //     unsigned char* imageData,
        *     //     int imageWidth,
        *     //     int imageHeight,
        *     //     int slapType,
        *     //     int* numOfFinger,
        *     //     RSSlapInfo** slapInfo,
        *     //     unsigned char** fingerImageData1, int* w1, int* h1,
        *     //     unsigned char** fingerImageData2, int* w2, int* h2,
        *     //     unsigned char** fingerImageData3, int* w3, int* h3,
        *     //     unsigned char** fingerImageData4, int* w4, int* h4);
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_Segment4(
        *         IntPtr imageData,
        *         int imageWidth,
        *         int imageHeight,
        *         int slapType,
        *         out int numOfFinger,
        *         out IntPtr slapInfo,
        *         out IntPtr fingerImageData1,
        *         out int fingerImageDataWidth1,
        *         out int fingerImageDataHeight1,
        *         out IntPtr fingerImageData2,
        *         out int fingerImageDataWidth2,
        *         out int fingerImageDataHeight2,
        *         out IntPtr fingerImageData3,
        *         out int fingerImageDataWidth3,
        *         out int fingerImageDataHeight3,
        *         out IntPtr fingerImageData4,
        *         out int fingerImageDataWidth4,
        *         out int fingerImageDataHeight4
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_SLAP_LEFT_FOUR = 1;
        *
        *     static void Main()
        *     {
        *         int width = 800;
        *         int height = 750;
        *
        *         // Example slap image buffer (must be replaced with real data)
        *         byte[] slapImage = new byte[width * height]; // 8-bit grayscale
        *
        *         // Pin the managed array so we can pass a stable pointer to native code
        *         GCHandle slapHandle = GCHandle.Alloc(slapImage, GCHandleType.Pinned);
        *
        *         try
        *         {
        *             IntPtr slapPtr = slapHandle.AddrOfPinnedObject();
        *
        *             int numOfFingers;
        *             IntPtr slapInfoPtr;
        *
        *             IntPtr finger1Ptr, finger2Ptr, finger3Ptr, finger4Ptr;
        *             int finger1Width, finger1Height;
        *             int finger2Width, finger2Height;
        *             int finger3Width, finger3Height;
        *             int finger4Width, finger4Height;
        *
        *             int result = RS_Segment4(
        *                 slapPtr,
        *                 width,
        *                 height,
        *                 RS_SLAP_LEFT_FOUR,
        *                 out numOfFingers,
        *                 out slapInfoPtr,
        *                 out finger1Ptr, out finger1Width, out finger1Height,
        *                 out finger2Ptr, out finger2Width, out finger2Height,
        *                 out finger3Ptr, out finger3Width, out finger3Height,
        *                 out finger4Ptr, out finger4Width, out finger4Height
        *             );
        *
        *             if (result == RS_SUCCESS)
        *             {
        *                 Console.WriteLine($"Segmentation succeeded. Fingers detected: {numOfFingers}");
        *
        *                 // Example: convert first finger to managed byte[]
        *                 if (finger1Ptr != IntPtr.Zero && finger1Width > 0 && finger1Height > 0)
        *                 {
        *                     int size1 = finger1Width * finger1Height;
        *                     byte[] finger1Image = new byte[size1];
        *                     Marshal.Copy(finger1Ptr, finger1Image, 0, size1);
        *                     Console.WriteLine($"Finger 1 image size: {finger1Width} x {finger1Height}");
        *                 }
        *
        *                 // TODO: Similarly handle finger2Ptr, finger3Ptr, finger4Ptr.
        *                 // NOTE: If the SDK allocates the fingerImageData buffers and
        *                 //       slapInfo array, check RS_API for the proper free
        *                 //       functions and call them here to avoid memory leaks.
        *             }
        *             else
        *             {
        *                 Console.WriteLine($"Segmentation failed. Error code: {result}");
        *             }
        *         }
        *         finally
        *         {
        *             if (slapHandle.IsAllocated)
        *                 slapHandle.Free();
        *         }
        *     }
        * }
        * @endcode
        */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_Segment4")]
        public static extern int RS_Segment4(IntPtr imageData, int imageWidth, int imageHeight, int slapType, ref int numOfFinger, ref IntPtr slapInfo, ref IntPtr fingerImageData1, ref int fingerImageDataWidth1, ref int fingerImageDataHeight1, ref IntPtr fingerImageData2, ref int fingerImageDataWidth2, ref int fingerImageDataHeight2, ref IntPtr fingerImageData3, ref int fingerImageDataWidth3, ref int fingerImageDataHeight3, ref IntPtr fingerImageData4, ref int fingerImageDataWidth4, ref int fingerImageDataHeight4);

        /**
        * @brief Segments a slap image (2 or 4 fingers) into individual finger
        * images with a custom crop size.
        *
        * This function provides the same functionality as RS_Segment, designed for
        * C# application integration. It segments a slap image containing two or
        * four fingers into individual finger images with a user-defined crop size.
        * The quality score for each finger based on the NIST standard is also
        * returned.
        *
        * @param imageData           [IN]  Raw slap image data.
        * @param imageWidth          [IN]  Width of the slap image in pixels.
        * @param imageHeight         [IN]  Height of the slap image in pixels.
        * @param slapType            [IN]  Type of slap image.\n
        * | Parameter Name           | Value |
        * |--------------------------|-------|
        * | RS_SLAP_LEFT_FOUR        | 1     |
        * | RS_SLAP_RIGHT_FOUR       | 2     |
        * | RS_SLAP_FOUR_FINGER      | 3     |
        * | RS_SLAP_TWO_THUMB        | 4     |
        * | RS_SLAP_TWO_FINGER       | 5     |
        * | RS_SLAP_ONE_FINGER       | 6     |
        * | RS_SLAP_ONE_FINGER_ROLL  | 7     |
        * | RS_SLAP_MANUAL           | 8     |
        * | RS_SLAP_MANUAL_INI       | 9     |
        * | RS_SLAP_MANUAL_ROLL      | 10    |
        * | RS_SLAP_SEAL             | 21    |
        * @param numOfFinger         [OUT] Pointer to store the number of segmented
        *                              fingers.
        * @param slapInfo            [OUT] Pointer to receive array of RSSlapInfo
        *                              structures.
        * @param fingerImageData1    [OUT] Pointer to the buffer of the first
        *                              finger image.
        * @param fingerImageData2    [OUT] Pointer to the buffer of the second
        *                              finger image.
        * @param fingerImageData3    [OUT] Pointer to the buffer of the third
        *                              finger image.
        * @param fingerImageData4    [OUT] Pointer to the buffer of the fourth
        *                              finger image.
        * @param nCropWidth          [IN]  Desired width of the cropped finger
        *                              images.
        * @param nCropHeight         [IN]  Desired height of the cropped finger
        *                              images.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -13   \b RS_ERR_INVALID_PARAM
        * @retval -14   \b RS_ERR_MEM_FULL
        * @retval -100  \b RS_ERR_NO_DEVICE     
        * @retval -215  \b RS_ERR_CANNOT_SEGMENT
        * @retval -216  \b RS_ERR_CANNOT_GET_QUALITY
        * @retval -217  \b RS_ERR_SEGMENT_FEWER_FINGER
        * @retval -218  \b RS_ERR_SEGMENT_WRONG_HAND
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * using System;
        * using System.Runtime.InteropServices;
        *
        * // Native RSSlapInfo structure mapping (simplified)
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSSlapInfo
        * {
        *     public int fingerType;
        *     public int x;
        *     public int y;
        *     public int width;
        *     public int height;
        *     // Add more fields from RS_API.h if needed.
        * }
        *
        * class Program
        * {
        *     // Assumed native signature:
        *     // int RS_Segment4WithSize(
        *     //     unsigned char* imageData,
        *     //     int imageWidth,
        *     //     int imageHeight,
        *     //     int slapType,
        *     //     int* numOfFinger,
        *     //     RSSlapInfo** slapInfo,
        *     //     unsigned char** fingerImageData1,
        *     //     unsigned char** fingerImageData2,
        *     //     unsigned char** fingerImageData3,
        *     //     unsigned char** fingerImageData4,
        *     //     int nCropWidth,
        *     //     int nCropHeight);
        *     [DllImport("RS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        *     public static extern int RS_Segment4WithSize(
        *         IntPtr imageData,
        *         int imageWidth,
        *         int imageHeight,
        *         int slapType,
        *         out int numOfFinger,
        *         out IntPtr slapInfo,
        *         out IntPtr fingerImageData1,
        *         out IntPtr fingerImageData2,
        *         out IntPtr fingerImageData3,
        *         out IntPtr fingerImageData4,
        *         int nCropWidth,
        *         int nCropHeight
        *     );
        *
        *     public const int RS_SUCCESS = 0;
        *     public const int RS_SLAP_LEFT_FOUR = 1;
        *
        *     static void Main()
        *     {
        *         int width = 800;
        *         int height = 750;
        *         int cropWidth = 300;
        *         int cropHeight = 350;
        *
        *         // Example slap image buffer (must be filled with real slap image data)
        *         byte[] slapImage = new byte[width * height]; // 8-bit grayscale
        *
        *         GCHandle slapHandle = GCHandle.Alloc(slapImage, GCHandleType.Pinned);
        *
        *         try
        *         {
        *             IntPtr slapPtr = slapHandle.AddrOfPinnedObject();
        *
        *             int numOfFingers;
        *             IntPtr slapInfoPtr;
        *             IntPtr finger1Ptr, finger2Ptr, finger3Ptr, finger4Ptr;
        *
        *             int result = RS_Segment4WithSize(
        *                 slapPtr,
        *                 width,
        *                 height,
        *                 RS_SLAP_LEFT_FOUR,
        *                 out numOfFingers,
        *                 out slapInfoPtr,
        *                 out finger1Ptr,
        *                 out finger2Ptr,
        *                 out finger3Ptr,
        *                 out finger4Ptr,
        *                 cropWidth,
        *                 cropHeight
        *             );
        *
        *             if (result == RS_SUCCESS)
        *             {
        *                 Console.WriteLine($"Segmentation with size succeeded. Fingers detected: {numOfFingers}");
        *
        *                 // Each finger image is cropWidth x cropHeight pixels (8-bit)
        *                 int fingerBufSize = cropWidth * cropHeight;
        *
        *                 if (finger1Ptr != IntPtr.Zero)
        *                 {
        *                     byte[] finger1Image = new byte[fingerBufSize];
        *                     Marshal.Copy(finger1Ptr, finger1Image, 0, fingerBufSize);
        *                     Console.WriteLine($"Finger 1 image size: {cropWidth} x {cropHeight}");
        *                 }
        *
        *                 if (finger2Ptr != IntPtr.Zero)
        *                 {
        *                     byte[] finger2Image = new byte[fingerBufSize];
        *                     Marshal.Copy(finger2Ptr, finger2Image, 0, fingerBufSize);
        *                     Console.WriteLine($"Finger 2 image size: {cropWidth} x {cropHeight}");
        *                 }
        *
        *                 if (finger3Ptr != IntPtr.Zero)
        *                 {
        *                     byte[] finger3Image = new byte[fingerBufSize];
        *                     Marshal.Copy(finger3Ptr, finger3Image, 0, fingerBufSize);
        *                     Console.WriteLine($"Finger 3 image size: {cropWidth} x {cropHeight}");
        *                 }
        *
        *                 if (finger4Ptr != IntPtr.Zero)
        *                 {
        *                     byte[] finger4Image = new byte[fingerBufSize];
        *                     Marshal.Copy(finger4Ptr, finger4Image, 0, fingerBufSize);
        *                     Console.WriteLine($"Finger 4 image size: {cropWidth} x {cropHeight}");
        *                 }
        *
        *                 // NOTE:
        *                 // If the SDK allocates memory for slapInfo and fingerImageDataN,
        *                 // be sure to call the appropriate RS_* free functions here
        *                 // to avoid memory leaks.
        *             }
        *             else
        *             {
        *                 Console.WriteLine($"Segmentation failed. Error code: {result}");
        *             }
        *         }
        *         finally
        *         {
        *             if (slapHandle.IsAllocated)
        *                 slapHandle.Free();
        *         }
        *     }
        * }
        * @endcode
        */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_Segment4WithSize")]
        public static extern int RS_Segment4WithSize(IntPtr imageData, int imageWidth, int imageHeight, int slapType, ref int numOfFinger, ref IntPtr slapInfo, ref IntPtr fingerImageData1, ref IntPtr fingerImageData2, ref IntPtr fingerImageData3, ref IntPtr fingerImageData4, int nCropWidth, int nCropHeight);

        /**
        * @brief Segments a slap fingerprint image (2 or 4 fingers) while handling
        * missing fingers, and returns segmented finger images and metadata.
        *
        * This function performs segmentation on a slap image (left four, right four,
        * two thumbs, etc.) and additionally supports cases where one or more fingers
        * are missing. The missing finger information is provided through the
        * RSMissingInfo structure. Segmented finger results are returned through
        * RSSegmentInfo and individual output image buffers.
        *
        * This API is designed for cross-platform integration, including C# applications.
        *
        * @param imageData      [IN]  Pointer to raw grayscale slap image data.
        * @param imageWidth     [IN]  Width of the slap image in pixels.
        * @param imageHeight    [IN]  Height of the slap image in pixels.
        * @param slapType       [IN]  Slap image type.\n
        * | Slap Type Constant       | Value |
        * |--------------------------|-------|
        * | RS_SLAP_LEFT_FOUR        | 1     |
        * | RS_SLAP_RIGHT_FOUR       | 2     |
        * | RS_SLAP_FOUR_FINGER      | 3     |
        * | RS_SLAP_TWO_THUMB        | 4     |
        * | RS_SLAP_TWO_FINGER       | 5     |
        *
        * @param psSegmentInfo  [OUT] Pointer to RSSegmentInfo structure that receives
        *                              segmentation metadata, including finger positions,
        *                              finger types, NFIQ quality scores, etc.
        *
        * @param pbyFinImg1     [OUT] Output buffer for the first finger image.
        * @param pbyFinImg2     [OUT] Output buffer for the second finger image.
        * @param pbyFinImg3     [OUT] Output buffer for the third finger image.
        * @param pbyFinImg4     [OUT] Output buffer for the fourth finger image.
        *                         - Buffers must be pre-allocated by the caller.
        *                         - Image size can be obtained from psSegmentInfo.
        *
        * @param sMissingInfo   [IN]  Structure describing which fingers are missing
        *                              in the slap image. The segmentation process
        *                              excludes these fingers from the output.
        *
        * @return Function success status code.
        * @retval 0      \b RS_SUCCESS
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -14    \b RS_ERR_MEM_FULL
        * @retval -100   \b RS_ERR_NO_DEVICE
        * @retval -215   \b RS_ERR_CANNOT_SEGMENT
        * @retval -217   \b RS_ERR_SEGMENT_FEWER_FINGER
        * @retval -218   \b RS_ERR_SEGMENT_WRONG_HAND
        *
        * @note The caller must ensure all output buffers (pbyFinImg1–4) have enough
        *       memory allocated. The required width/height for each finger image
        *       is returned inside the RSSegmentInfo structure.
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i).
        *
        * \par Example (C# usage)
        * @code
        * RSSegmentInfo segmentInfo = new RSSegmentInfo();
        * RSMissingInfo missingInfo = new RSMissingInfo();
        *
        * // Mark missing finger example:
        * missingInfo.bFinger1Missing = true;
        *
        * int width = 800;
        * int height = 750;
        *
        * byte[] slapImage = LoadSlapImage();       // Fill with real image data
        * byte[] fin1 = new byte[1000*1000];
        * byte[] fin2 = new byte[1000*1000];
        * byte[] fin3 = new byte[1000*1000];
        * byte[] fin4 = new byte[1000*1000];
        *
        * GCHandle handle = GCHandle.Alloc(slapImage, GCHandleType.Pinned);
        * IntPtr imgPtr = handle.AddrOfPinnedObject();
        *
        * int result = RS_SegmentExMissingFinger(
        *     imgPtr, width, height,
        *     RS_SLAP_LEFT_FOUR,
        *     ref segmentInfo,
        *     fin1, fin2, fin3, fin4,
        *     missingInfo
        * );
        *
        * if (result == 0)
        * {
        *     Console.WriteLine("Segmentation succeeded.");
        *     Console.WriteLine($"Fingers segmented: {segmentInfo.nFingerCount}");
        * }
        * else
        * {
        *     Console.WriteLine($"Error: {result}");
        * }
        *
        * handle.Free();
        * @endcode
        */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SegmentExMissingFinger")]
        public static extern int RS_SegmentExMissingFinger(IntPtr imageData, int imageWidth, int imageHeight, int slapType, ref RSSegmentInfo psSegmentInfo, byte[] pbyFinImg1, byte[] pbyFinImg2, byte[] pbyFinImg3, byte[] pbyFinImg4, RSMissingInfo sMissingInfo);

        /**
        * @brief Sets the minimum number of fingers required for a capture operation.
        *
        * This function configures the minimum number of fingers that must be detected
        * before a multi-finger capture (slap or two-finger capture) can proceed.
        * The value affects slap/4-finger/two-thumb capture modes.
        *
        * @param deviceHandle     [IN] Handle of the initialized device.
        * @param minFingerCount   [IN] Minimum number of fingers required for capture.
        *                             Typical values:
        *                             - 1 for single finger
        *                             - 2 for two-finger capture
        *                             - 4 for four-finger slap capture
        *
        * @return Status code indicating success or failure.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -200   \b RS_ERR_INVALID_CAPTURE_MODE
        * @retval -229   \b RS_ERR_CAPTURE_WRONG_MINFINGER
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = deviceHandleFromSDK;
        * int result = RealScanSDK.RS_SetMinimumFinger(deviceHandle, 4);
        *
        * if (result == RS_SUCCESS)
        * {
        *     Console.WriteLine("Minimum finger count successfully updated.");
        * }
        * else
        * {
        *     Console.WriteLine($"Failed to set minimum finger count. Error: {result}");
        * }
        * @endcode
        */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetMinimumFinger")]
        public static extern int RS_SetMinimumFinger(int deviceHandle, int minFingerCount);

        /**
        * @brief Gets the currently set minimum number of fingers required for capture.
        *
        * This function retrieves the minimum number of fingers that must be detected
        * before a multi-finger capture can proceed.
        *
        * @param deviceHandle   [IN]  Handle of the initialized device.
        * @param minFingerCount [OUT] Pointer to an integer that receives the current
        *                             minimum finger count.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -200   \b RS_ERR_INVALID_CAPTURE_MODE
        * @retval -229   \b RS_ERR_CAPTURE_WRONG_MINFINGER
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_GetMinimumFinger(int deviceHandle, out int minFingerCount);
        *
        * int handle = deviceHandle;   // Assume initialized
        * int minCount = 0;
        *
        * int result = RS_GetMinimumFinger(handle, out minCount);
        *
        * if (result == 0) {
        *     Console.WriteLine("Minimum finger count: " + minCount);
        * } else {
        *     Console.WriteLine("Failed to get minimum finger count. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetMinimumFinger")]
        public static extern int RS_GetMinimumFinger(int deviceHandle, ref int pnMinFingerCount);
        /**
        * @brief Returns the NIST/NFIQ quality score for the given fingerprint image.
        *
        * This function evaluates the quality of the fingerprint image according to the NIST standard.
        *
        * @param imageData     [IN] Pointer to grayscale fingerprint image data.
        * @param imageWidth    [IN] Width of the fingerprint image in pixels.
        * @param imageHeight   [IN] Height of the fingerprint image in pixels.
        * @param nistQuality   [OUT] Pointer to an integer receiving the quality score.
        *                         The meaning of the score depends on the NFIQ version
        *                         embedded in the SDK.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -100   \b RS_ERR_NO_DEVICE
        * @retval -216   \b RS_ERR_CANNOT_GET_QUALITY   (NFIQ failed; nistQuality may be set to 255)
        *
        * @note This function does not require device connection once the SDK is initialized,
        *       but a valid RealScan device must exist for the SDK to load the NFIQ module.
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i),
        *       RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // C# example for calling RS_GetQualityScore
        * 
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_GetQualityScore(
        *     byte[] imageData,
        *     int imageWidth,
        *     int imageHeight,
        *     out int nistQuality);
        * 
        * // Load RAW grayscale image data (8-bit)
        * byte[] imageData = File.ReadAllBytes("fingerprint.raw");
        * int width = 800;
        * int height = 750;
        * int quality = 0;
        * 
        * int result = RS_GetQualityScore(imageData, width, height, out quality);
        * 
        * if (result == 0) {
        *     Console.WriteLine("Quality score: " + quality);
        * } else {
        *     Console.WriteLine("Failed to compute quality. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetQualityScore")]
        public static extern int RS_GetQualityScore(IntPtr imageData, int imageWidth, int imageHeight, ref int nistQuality);

        /**
        * @brief Various options of self-test can help checking conditions or status of scanner,
        *        and determining the possibility of its malfunctioning.
        *
        * @param deviceHandle  [IN] Handle to the opened device.
        * @param testType      [IN] Self-Test type.\n
        * | Macro Name                     | Value |
        * |--------------------------------|-------|
        * | RS_SELFTEST_TYPE_ILLUMINATION  | 1     |
        * | RS_SELFTEST_TYPE_DIRTY         | 2     |
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -115   \b RS_ERR_SENSOR_DIRTY
        * @retval -116   \b RS_ERR_FINGER_EXIST
        * @retval -117   \b RS_ERR_TOO_STRONG_LIGHT
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_SelfTest(int deviceHandle, int testType);
        *
        * int handle = deviceHandle;  // Assume initialized
        * int result = RS_SelfTest(handle, RS_SELFTEST_TYPE_DIRTY);
        *
        * if (result == 0) {
        *     Console.WriteLine("Self-test passed.");
        * } else {
        *     Console.WriteLine("Self-test failed. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SelfTest")]
        public static extern int RS_SelfTest(int deviceHandle, int testType);

        /**
        * @brief Checks for foreign substances on the sensor surface and returns the status.
        *
        * This function inspects the scanner surface for dust, smudges, or other contaminants
        * and reports whether cleaning is required. Capture must not be running during this call.
        *
        * @param deviceHandle  [IN] Handle to the opened device.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -212  \b RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan SG10(i), RealScan S60
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_Calibrate(int deviceHandle);
        *
        * int handle = deviceHandle;  // Assume initialized
        * int result = RS_Calibrate(handle);
        *
        * if (result == 0) {
        *     Console.WriteLine("Sensor calibration OK (no contamination detected).");
        * } else {
        *     Console.WriteLine("Calibration failed. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_Calibrate")]
        public static extern int RS_Calibrate(int deviceHandle);

        /**
        * @brief Sets whether to enable or disable fingerprint rotation during segmentation.
        *
        * If enabled, the segmentation process may rotate finger images to correct orientation.
        * If disabled, images are returned without rotation correction.
        *
        * @param isRotating [IN] Set to true to allow rotation, false to disable rotation.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_SetSegRotateOption(bool isRotating);
        *
        * int result = RS_SetSegRotateOption(true);  // Enable rotation
        *
        * if (result == 0) {
        *     Console.WriteLine("Segmentation rotation enabled.");
        * } else {
        *     Console.WriteLine("Failed to set rotate option. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetSegRotateOption")]
        public static extern int RS_SetSegRotateOption(bool isRotating);
        /**
        * @brief Sets the fingerprint image pre-processing mode for RealScan devices.
        *
        * Starting from SDK version V1.7, a new pre-processing algorithm is applied to
        * both preview and final captured images. This algorithm enhances fingerprint
        * clarity, particularly for dry or low-contrast fingerprints.
        *
        * Three pre-processing modes are available:
        *
        * | Macro Name                            | Value | Description |
        * |---------------------------------------|-------|-------------|
        * | RS_HIGH_VISIBILITY_PREPROCESS         | 0     | Enhances ridge visibility; ideal for dry fingerprints and improving contrast. |
        * | RS_BALANCED_PREPROCESS                | 1     | Balanced mode between visibility and quality score. |
        * | RS_HIGH_NFIQ_SCORE_PREPROCESS         | 2     | Optimized to achieve higher NFIQ quality scores. |
        *
        * @param deviceHandle   [IN] Handle to the opened device.
        * @param preprocessMode [IN] Desired pre-processing mode (use macros above).
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_SetPreProcessing(int deviceHandle, int preprocessMode);
        *
        * // Preprocess mode constants
        * public const int RS_HIGH_VISIBILITY_PREPROCESS = 0;
        * public const int RS_BALANCED_PREPROCESS        = 1;
        * public const int RS_HIGH_NFIQ_SCORE_PREPROCESS = 2;
        *
        * int handle = deviceHandle;   // Assume initialized
        *
        * // Example: set pre-processing to High Visibility mode
        * int result = RS_SetPreProcessing(handle, RS_HIGH_VISIBILITY_PREPROCESS);
        *
        * if (result == 0) {
        *     Console.WriteLine("Pre-processing mode set successfully.");
        * } else {
        *     Console.WriteLine("Failed to set pre-processing mode. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetPreProcessing")]
        public static extern int RS_SetPreProcessing(int deviceHandle, int preprocessMode);
        /**
         * @brief For V1.8 or later versions, a new halo-reduction pre-processing algorithm is applied.
         *
         * This algorithm reduces the "halo artifact" that may appear on fingerprint images
         * when high humidity or a temperature difference exists between the user's hand
         * and the device surface.
         *
         * Three halo-processing modes are available:
         *
         * | Macro Name                     | Value | Description |
         * |-------------------------------|-------|-------------|
         * | RS_DISABLE_HALOPROCESS        | 0     | Disable halo-reduction algorithm. |
         * | RS_ENABLE_HALOPROCESS         | 1     | Enable standard halo-reduction. |
         * | RS_ENABLE_HALOPROCESS_EX      | 2     | Enable enhanced (extended) halo-reduction mode. |
         *
         * @param deviceHandle   [IN] Handle to the opened device.
         * @param preprocessMode [IN] One of the halo-processing modes listed above.
         *
         * @return Function success status as an error code.
         * @retval 0     \b RS_SUCCESS
         * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
         * @retval -102  \b RS_ERR_INVALID_HANDLE
         *
         * @note Supported devices:
         *       RealScan-D, RealScan-F, RealScan-FC,
         *       RealScan-G10(i), RealScan SG10(i)
         *
         * \par Example (C#)
         * @code
         * // DLL Import
         * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
         * public static extern int RS_SetHaloProcessing(int deviceHandle, int preprocessMode);
         *
         * // Define constants
         * const int RS_DISABLE_HALOPROCESS   = 0;
         * const int RS_ENABLE_HALOPROCESS    = 1;
         * const int RS_ENABLE_HALOPROCESS_EX = 2;
         *
         * int handle = deviceHandle;  // assume initialized
         *
         * // Disable halo reduction
         * int result = RS_SetHaloProcessing(handle, RS_DISABLE_HALOPROCESS);
         *
         * if (result == 0) {
         *     Console.WriteLine("Halo processing disabled.");
         * } else {
         *     Console.WriteLine("Failed to set halo processing. Error: " + result);
         * }
         * @endcode
         */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetHaloProcessing")]
        public static extern int RS_SetHaloProcessing(int deviceHandle, int haloReduction);


        /**
        * @brief Retrieves the current settings of the pre-processing function.
        *
        * This function returns the currently applied pre-processing mode, such as
        * visibility enhancement, NFIQ optimization, or halo-reduction mode.
        *
        * @param deviceHandle    [IN]  Handle to the opened device.
        * @param preprocessMode  [OUT] Pointer to an integer receiving the current
        *                              pre-processing mode.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_GetPreProcessing(int deviceHandle, out int preprocessMode);
        *
        * int handle = deviceHandle;   // Assume initialized
        * int mode = 0;
        *
        * int result = RS_GetPreProcessing(handle, out mode);
        *
        * if (result == 0) {
        *     Console.WriteLine("Current pre-processing mode: " + mode);
        * } else {
        *     Console.WriteLine("Failed to get pre-processing mode. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetPreProcessing")]
        public static extern int RS_GetPreProcessing(int deviceHandle, ref int preprocessMode);
        /**
        * @brief Two post-processing functions can be applied to the final image.
        *
        * Automatic calibration usually removes background noise, but additional noise may still occur
        * during image acquisition. In such cases, enabling the noise-reduction function can further
        * clean the final image. You can also enable contrast enhancement to improve image clarity.
        *
        * @param deviceHandle        [IN] Handle to the opened device.
        * @param contrastEnhancement [IN] If true, apply the contrast enhancement function.
        * @param noiseReduction      [IN] If true, apply noise reduction to the final image.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -212  \b RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // DLL Import
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_SetPostProcessing(
        *     int deviceHandle,
        *     bool contrastEnhancement,
        *     bool noiseReduction);
        *
        * int handle = deviceHandle; // assume initialized
        *
        * // Enable both contrast enhancement and noise reduction
        * int result = RS_SetPostProcessing(handle, true, true);
        *
        * if (result == 0) {
        *     Console.WriteLine("Post-processing enabled.");
        * } else {
        *     Console.WriteLine("Failed to set post-processing. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetPostProcessing")]
        public static extern int RS_SetPostProcessing(int deviceHandle, bool contrastEnhancement, bool noiseReduction);
        /**
        * @brief Retrieves the current settings of post-processing functions.
        *
        * This function returns whether contrast enhancement and noise reduction
        * are currently enabled for final fingerprint images.
        *
        * @param deviceHandle        [IN]  Handle to the opened device.
        * @param contrastEnhancement [OUT] Pointer receiving whether contrast enhancement is enabled.
        * @param noiseReduction      [OUT] Pointer receiving whether noise reduction is enabled.
        *
        * @return Function success status as an error code.
        * @retval 0     \b RS_SUCCESS
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102  \b RS_ERR_INVALID_HANDLE
        * @retval -212  \b RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // DLL Import
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_GetPostProcessing(
        *     int deviceHandle,
        *     out bool contrastEnhancement,
        *     out bool noiseReduction);
        *
        * int handle = deviceHandle;  // assume initialized
        * bool contrast, noise;
        *
        * int result = RS_GetPostProcessing(handle, out contrast, out noise);
        *
        * if (result == 0)
        * {
        *     Console.WriteLine($"Contrast: {contrast}, Noise Reduction: {noise}");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to retrieve post-processing settings. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetPostProcessing")]
        public static extern int RS_GetPostProcessing(int deviceHandle, ref bool contrastEnhancement, ref bool noiseReduction);


        /**
        * @brief Registers an advanced callback to receive enhanced preview data.
        *
        * @param deviceHandle     [IN] Handle to the opened device.
        * @param captureCallback  [IN] Advanced callback for preview frames.
        *
        * @return Status code indicating success or failure.
        * @retval 0     \b RS_SUCCESS 
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED 
        * @retval -102  \b RS_ERR_INVALID_HANDLE 
        * 
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * // C# delegate definition matching native callback signature
        * [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        * public delegate void AdvPreviewCallback(
        *     int deviceHandle,
        *     int errorCode,
        *     IntPtr imageData,
        *     int imageWidth,
        *     int imageHeight,
        *     int quality,
        *     int status);
        *
        * // Import of native API
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_RegisterAdvPreviewCallback(
        *     int deviceHandle,
        *     AdvPreviewCallback callback);
        *
        * // Example callback handler
        * static void MyAdvPreviewHandler(
        *     int deviceHandle,
        *     int errorCode,
        *     IntPtr imageData,
        *     int imageWidth,
        *     int imageHeight,
        *     int quality,
        *     int status)
        * {
        *     int bufferSize = imageWidth * imageHeight;
        *     byte[] buffer = new byte[bufferSize];
        *
        *     if (imageData != IntPtr.Zero)
        *     {
        *         Marshal.Copy(imageData, buffer, 0, bufferSize);
        *     }
        *
        *     Console.WriteLine($"Preview: {imageWidth}x{imageHeight}, Q={quality}, Status={status}");
        * }
        *
        * // Register callback
        * int deviceHandle = 0; // Assume opened device handle
        * AdvPreviewCallback callback = new AdvPreviewCallback(MyAdvPreviewHandler);
        *
        * int result = RS_RegisterAdvPreviewCallback(deviceHandle, callback);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to register advanced preview callback. Error: " + result);
        * }
        * @endcode
        */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterAdvPreviewCallback")]
        public static extern int RS_RegisterAdvPreviewCallback(int deviceHandle, RSAdvPreviewCallback captureCallback);
        /**
        * @brief Registers a callback to receive raw preview image data.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param captureCallback [IN] Callback function to receive preview frames.
        *
        * @return Status code indicating success or failure.
        * @retval 0     \b RS_SUCCESS 
        * @retval -10   \b RS_ERR_SDK_UNINITIALIZED 
        * @retval -102  \b RS_ERR_INVALID_HANDLE 
        * 
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * // Callback delegate definition
        * [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        * public delegate void PreviewCallback(
        *     int deviceHandle,
        *     int errorCode,
        *     IntPtr imageData,
        *     int imageWidth,
        *     int imageHeight);
        *
        * // Import of native API
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_RegisterPreviewCallback(
        *     int deviceHandle,
        *     PreviewCallback callback);
        *
        * // Example callback implementation
        * static void OnPreviewCallback(
        *     int deviceHandle,
        *     int errorCode,
        *     IntPtr imageData,
        *     int imageWidth,
        *     int imageHeight)
        * {
        *     if (errorCode == 0 && imageData != IntPtr.Zero)
        *     {
        *         int bufferSize = imageWidth * imageHeight;
        *         byte[] buffer = new byte[bufferSize];
        *         Marshal.Copy(imageData, buffer, 0, bufferSize);
        *
        *         Console.WriteLine($"Preview frame received: {imageWidth}x{imageHeight}");
        *     }
        * }
        *
        * // Registering the callback
        * int deviceHandle = 0;  // Assume initialized RealScan device
        * PreviewCallback callback = new PreviewCallback(OnPreviewCallback);
        *
        * int result = RS_RegisterPreviewCallback(deviceHandle, callback);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to register preview callback. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterPreviewCallback")]
        public static extern int RS_RegisterPreviewCallback(int deviceHandle, RSPreviewDataCallback captureCallback);


        //
        // VIEW APIs
        //
        /**
        * @brief Configures a preview window. Preview images will be shown in the specified window in real-time. As shown in Figure 4 Preview Window, 
        * the preview images will be resized to fill the whole surface of the preview window. 
        * 
        * @param deviceHandle [IN] Handle to the opened device.
        * @param windowHandle  [IN] Handle of the preview window.
        * @param drawRectangle  [IN] Client rectangle of the drawing region. The preview images will be scaled to be fit to the rectangle. 
        * @param autoContrast [IN] Reserved for future use.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -300   \b RS_ERR_CANNOT_SET_VIEW_WINDOW
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example
        * @code
        // C# Example (WinForms)
        using System;
        using System.Drawing;
        using System.Runtime.InteropServices;

        class Example
        {
            [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetViewWindow")]
            public static extern int RS_SetViewWindow(
                int deviceHandle,
                IntPtr windowHandle,
                Rectangle drawRect,
                bool autoContrast);

            public void SetPreviewWindow(int deviceHandle, Control viewControl)
            {
                Rectangle rect = viewControl.ClientRectangle;

                int result = RS_SetViewWindow(
                    deviceHandle,
                    viewControl.Handle,
                    rect,
                    false);  // autoContrast (reserved)
            }
        }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetViewWindow")]
        public static extern int RS_SetViewWindow(int deviceHandle, IntPtr windowHandle, RSRect drawRectangle, bool autoContrast);

        /**
        * @brief Stops the previewing process. 
        * 
        * @param deviceHandle [IN] Handle to the opened device.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_StopViewWindow(int deviceHandle);
        *
        * int deviceHandle = 0; // Assume valid device handle
        *
        * int result = RS_StopViewWindow(deviceHandle);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to stop preview. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_StopViewWindow")]
        public static extern int RS_StopViewWindow(int deviceHandle);

        /**
        * @brief Removes all overlays from the specified device.
        *
        * @param deviceHandle [IN] Handle of the device.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS  
        * @retval -10      \b RS_ERR_SDK_UNINITIALIZED  
        * @retval -301     \b RS_ERR_INVALID_OVERLAY_HANDLE  
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_RemoveAllOverlay(int deviceHandle);
        *
        * int deviceHandle = 0; // Assume valid handle
        *
        * int result = RS_RemoveAllOverlay(deviceHandle);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to remove overlays. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RemoveAllOverlay")]
        public static extern int RS_RemoveAllOverlay(int deviceHandle);

        // Overlay APIs
        /**
       * @brief Adds an overlay text to the preview window. To show the overlay, RS_ShowOverlay should be called with the acquired handle.
       * @param deviceHandle [IN] Handle to the opened device.
       * @param text  [IN] RSOverlayText defined as follows;
       * @param overlayhandle  [OUT] Handle of the overlay to be returned.
       *
       * @return Function success status as an error code.
       * @retval 0      \b RS_SUCCESS
       * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
       * @retval -102   \b RS_ERR_INVALID_HANDLE
       * @retval -301   \b RS_ERR_NO_MORE_OVERLAY
       *
       * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
       * 
       * \par Example (C#)
       * @code
       * // Native structs mapping (example)
       * [StructLayout(LayoutKind.Sequential)]
       * public struct RSPoint
       * {
       *     public int x;
       *     public int y;
       * }
       *
       * [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
       * public struct RSOverlayText
       * {
       *     public RSPoint pos;
       *     public uint color;
       *
       *     // Adjust SizeConst to match SDK definition of text buffer length
       *     [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
       *     public string text;
       * }
       *
       * // P/Invoke declaration
       * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
       * public static extern int RS_AddOverlayText(
       *     int deviceHandle,
       *     ref RSOverlayText text,
       *     out int overlayHandle);
       *
       * int deviceHandle = 0; // Assume valid handle
       *
       * RSOverlayText overlayText = new RSOverlayText
       * {
       *     pos = new RSPoint { x = 100, y = 100 },
       *     color = 0x000000FF,   // red (BGRA or RGBA depends on SDK spec)
       *     text = "Sample Text"
       * };
       *
       * int overlayHandle;
       * int result = RS_AddOverlayText(deviceHandle, ref overlayText, out overlayHandle);
       *
       * if (result == 0)
       * {
       *     Console.WriteLine("Overlay created. Handle = " + overlayHandle);
       * }
       * else
       * {
       *     Console.WriteLine("Failed to add overlay text. Error: " + result);
       * }
       * @endcode
       */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_AddOverlayText")]
        public static extern int RS_AddOverlayText(int deviceHandle, ref RSOverlayText text, ref int overlayhandle);
        /**
        * @brief Adds an overlay line to the preview window. To show the overlay, RS_ShowOverlay
        *        should be called with the acquired handle.
        * 
        * @param deviceHandle   [IN] Handle of the device.
        * @param line           [IN] RSOverlayLine structure defined as follows:
        * @code
        * struct RSOverlayLine {
        *     POINT    startPos;    ///< Start position of the line.
        *     POINT    endPos;      ///< End position of the line.
        *     COLORREF color;       ///< Line color (RGB format, e.g., 0x00FF0000).
        *     int      width;       ///< Thickness of the line in pixels.
        *     int      reserved[16];///< Reserved for future use, set to 0.
        * };
        * @endcode
        * @param overlayHandle  [OUT] Handle of the overlay to be returned.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS  
        * @retval -10      \b RS_ERR_SDK_UNINITIALIZED  
        * @retval -102     \b RS_ERR_INVALID_HANDLE  
        * @retval -301     \b RS_ERR_NO_MORE_OVERLAY  
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // C# struct mappings
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSPoint
        * {
        *     public int x;
        *     public int y;
        * }
        *
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSOverlayLine
        * {
        *     public RSPoint startPos;
        *     public RSPoint endPos;
        *     public uint color;      // COLORREF (0x00BBGGRR or 0x00RRGGBB depending on SDK spec)
        *     public int width;
        *
        *     // reserved[16]
        *     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        *     public int[] reserved;
        * }
        *
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_AddOverlayLine(
        *     int deviceHandle,
        *     ref RSOverlayLine line,
        *     out int overlayHandle);
        *
        * int deviceHandle = 0; // Assume valid device handle
        *
        * RSOverlayLine line = new RSOverlayLine
        * {
        *     startPos = new RSPoint { x = 50,  y = 50  },
        *     endPos   = new RSPoint { x = 200, y = 200 },
        *     color    = 0x00FF0000,  // example color
        *     width    = 3,
        *     reserved = new int[16]
        * };
        *
        * int overlayHandle;
        * int result = RS_AddOverlayLine(deviceHandle, ref line, out overlayHandle);
        *
        * if (result == 0)
        * {
        *     Console.WriteLine("Overlay line added. Handle = " + overlayHandle);
        * }
        * else
        * {
        *     Console.WriteLine("Failed to add overlay line. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_AddOverlayLine")]
        public static extern int RS_AddOverlayLine(int deviceHandle, ref RSOverlayLine line, ref int overlayHandle);

        /**
        * @brief Adds an overlay quadrangle (4-point polygon) to the preview window.
        *
        * @param deviceHandle   [IN] Handle of the device.
        * @param quadrangle     [IN] RSOverlayQuadrangle structure defined as follows:
        * @code
        * struct RSOverlayQuadrangle {
        *     POINT    pos[4];      ///< Coordinates of the quadrangle corners (clockwise).
        *     COLORREF color;       ///< Border color of the quadrangle.
        *     int      width;       ///< Border thickness in pixels.
        *     int      reserved[16];///< Reserved for future use, set to 0.
        * };
        * @endcode
        * @param overlayHandle  [OUT] Handle of the overlay to be returned.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS  
        * @retval -10      \b RS_ERR_SDK_UNINITIALIZED  
        * @retval -102     \b RS_ERR_INVALID_HANDLE  
        * @retval -301     \b RS_ERR_NO_MORE_OVERLAY  
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSPoint
        * {
        *     public int x;
        *     public int y;
        * }
        *
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSOverlayQuadrangle
        * {
        *     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        *     public RSPoint[] pos;   // 4 corner points (clockwise)
        *
        *     public uint color;      // COLORREF (check SDK for channel order)
        *     public int  width;      // Border thickness in pixels
        *
        *     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        *     public int[] reserved;  // must be initialized to zeros
        * }
        *
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_AddOverlayQuadrangle(
        *     int deviceHandle,
        *     ref RSOverlayQuadrangle quadrangle,
        *     out int overlayHandle);
        *
        * int deviceHandle = 0; // assume valid handle
        *
        * RSOverlayQuadrangle quad = new RSOverlayQuadrangle
        * {
        *     pos = new RSPoint[4]
        *     {
        *         new RSPoint { x =  50, y =  50 },
        *         new RSPoint { x = 150, y =  50 },
        *         new RSPoint { x = 150, y = 150 },
        *         new RSPoint { x =  50, y = 150 }
        *     },
        *     color    = 0x0000FF00,    // example color
        *     width    = 2,
        *     reserved = new int[16]
        * };
        *
        * int overlayHandle;
        * int result = RS_AddOverlayQuadrangle(deviceHandle, ref quad, out overlayHandle);
        *
        * if (result == 0)
        * {
        *     Console.WriteLine("Quadrangle overlay added. Handle = " + overlayHandle);
        * }
        * else
        * {
        *     Console.WriteLine("Failed to add quadrangle overlay. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_AddOverlayQuadrangle")]
        public static extern int RS_AddOverlayQuadrangle(int deviceHandle, ref RSOverlayQuadrangle quadrangle, ref int overlayHandle);
        /**
        * @brief Shows or hides a specific overlay on the preview window.
        *
        * @param overlayHandle [IN] Handle of the overlay.
        * @param show          [IN] TRUE to show the overlay, FALSE to hide it.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS  
        * @retval -301     \b RS_ERR_INVALID_OVERLAY_HANDLE  
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_ShowOverlay(int overlayHandle, bool show);
        *
        * int overlayHandle = 123; // Assume this was returned from RS_AddOverlayXXX
        *
        * // Show overlay
        * int result = RS_ShowOverlay(overlayHandle, true);
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to show overlay. Error: " + result);
        * }
        *
        * // Hide overlay
        * result = RS_ShowOverlay(overlayHandle, false);
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to hide overlay. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ShowOverlay")]
        public static extern int RS_ShowOverlay(int overlayHandle, bool show);

        /**
        * @brief Adds an overlay cross marker to the preview window.
        *        To display the overlay, call RS_ShowOverlay() with the returned handle.
        * 
        * @param deviceHandle   [IN]  Handle to the opened device.
        * @param cross          [IN]  Pointer to an RSOverlayCross structure.
        * @param overlayHandle  [OUT] Pointer to receive the created overlay handle.
        *
        * @return Function success status as an error code.
        * @retval 0       \b RS_SUCCESS
        * @retval -10     \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102    \b RS_ERR_INVALID_HANDLE
        * @retval -301    \b RS_ERR_NO_MORE_OVERLAY
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * // C# P/Invoke declaration
        * [StructLayout(LayoutKind.Sequential)]
        * public struct RSOverlayCross
        * {
        *     public POINT centerPos;   // Center coordinate of the cross
        *     public int rangeX;        // Horizontal range (half width)
        *     public int rangeY;        // Vertical range (half height)
        *     public uint color;        // COLORREF (0x00BBGGRR)
        *     public int thickness;     // Line thickness
        *     public int reserved;      // Reserved (set to 0)
        * }
        * 
        * [StructLayout(LayoutKind.Sequential)]
        * public struct POINT
        * {
        *     public int X;
        *     public int Y;
        * }
        *
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_AddOverlayCross(
        *     int deviceHandle,
        *     ref RSOverlayCross cross,
        *     out int overlayHandle);
        *
        * // Example usage:
        * int deviceHandle = 0; // Assume valid
        *
        * RSOverlayCross cross = new RSOverlayCross
        * {
        *     centerPos = new POINT { X = 150, Y = 150 },
        *     rangeX = 30,
        *     rangeY = 30,
        *     color = 0x00FF0000,   // Blue (COLORREF format)
        *     thickness = 2,
        *     reserved = 0
        * };
        *
        * int overlayHandle;
        * int result = RS_AddOverlayCross(deviceHandle, ref cross, out overlayHandle);
        *
        * if (result == 0)
        * {
        *     Console.WriteLine("Overlay cross added. Handle = " + overlayHandle);
        * }
        * else
        * {
        *     Console.WriteLine("Failed to add overlay cross. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_AddOverlayCross")]
        public static extern int RS_AddOverlayCross(int deviceHandle, ref RSOverlayCross cross, ref int overlayHandle);
        /**
        * @brief Shows or hides all overlays associated with the specified device.
        *
        * This function controls the visibility of every overlay (text, line, quadrangle,
        * cross, etc.) created for the given device. Passing TRUE makes all overlays visible,
        * while FALSE hides them.
        *
        * @param deviceHandle [IN] Handle of the device.
        * @param show         [IN] TRUE to show all overlays, FALSE to hide them.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS
        * @retval -10      \b RS_ERR_SDK_UNINITIALIZED
        * @retval -301     \b RS_ERR_INVALID_OVERLAY_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // Import declaration
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_ShowAllOverlay(int deviceHandle, bool show);
        *
        * // Usage example
        * int deviceHandle = 0;
        *
        * // Show all overlays
        * int result = RS_ShowAllOverlay(deviceHandle, true);
        *
        * // Hide all overlays
        * result = RS_ShowAllOverlay(deviceHandle, false);
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ShowAllOverlay")]
        public static extern int RS_ShowAllOverlay(int deviceHandle, bool show);
        /**
        * @brief Removes a specific overlay from the preview window.
        *
        * This function deletes a previously created overlay (text, line, quadrangle,
        * cross, etc.) using its overlay handle. After removal, the overlay will no longer
        * appear on the preview window.
        *
        * @param overlayHandle [IN] Handle of the overlay to remove.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS
        * @retval -301     \b RS_ERR_INVALID_OVERLAY_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * // Import declaration
        * [DllImport("RS_SDK.dll", CallingConvention = CallingConvention.StdCall)]
        * public static extern int RS_RemoveOverlay(int overlayHandle);
        *
        * // Usage example
        * int overlayHandle = 10; // Example handle value
        * int result = RS_RemoveOverlay(overlayHandle);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to remove overlay. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RemoveOverlay")]
        public static extern int RS_RemoveOverlay(int overlayHandle);



        //
        // IO APIs
        //
        /**
        * @brief Clears the LCD panel by displaying an image filled with zero (black) pixels.
        *
        * This function resets (cleans) the LCD panel on supported RealScan devices by
        * overwriting the display area with a black image buffer.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported device: **RealScan-F**
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ResetLCD")]
        * public static extern int RS_ResetLCD(int deviceHandle);
        * 
        * int deviceHandle = 0; // Assume valid device handle
        * int result = RS_ResetLCD(deviceHandle);
        *
        * if (result == 0)  // RS_SUCCESS
        * {
        *     Console.WriteLine("LCD reset successfully.");
        * }
        * else
        * {
        *     Console.WriteLine("LCD reset failed. Error = " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_ResetLCD")]
        public static extern int RS_ResetLCD(int deviceHandle);


        /**
        * @brief LCD display device in RealScan-F can show the 16-bit RGB data, which means that it can display 
        *        the image data whose 2 bytes represent 1 pixel on the LCD. This function can change a format of 
        *        image data into the format LCD can display with.
        *
        * @param inputRData   [IN] Pointer to red-channel image data.
        * @param inputGData   [IN] Pointer to green-channel image data.
        * @param inputBData   [IN] Pointer to blue-channel image data.
        * @param inputWidth   [IN] Width of the input image.
        * @param inputHeight  [IN] Height of the input image.
        * @param outputData   [OUT] Pointer to 16-bit RGB output image buffer.
        *
        * @return Function success status as an error code.
        * @retval 0        \b RS_SUCCESS
        * @retval -401     \b RS_ERR_CANNOT_MAKE_LCD_DATA
        * @retval -402     \b RS_ERR_INVALID_LCD_DATA
        * 
        * @note RealScan-F
        * 
        * \par Example (C#)
        * @code
        * const int RS_LCD_WIDTH_MAX  = 320;
        * const int RS_LCD_HEIGHT_MAX = 240;
        *
        * // Load grayscale raw data (test_sample.r08)
        * byte[] raw = File.ReadAllBytes("test_sample.r08");
        *
        * int lcdDataWidth  = RS_LCD_WIDTH_MAX;
        * int lcdDataHeight = raw.Length / lcdDataWidth;
        *
        * if (lcdDataHeight > RS_LCD_HEIGHT_MAX)
        *     lcdDataHeight = RS_LCD_HEIGHT_MAX;
        *
        * // Use the same grayscale for R/G/B channels
        * byte[] r = raw;
        * byte[] g = raw;
        * byte[] b = raw;
        *
        * IntPtr outputPtr = IntPtr.Zero;
        *
        * int result = RS_MakeLCDData(r, g, b, lcdDataWidth, lcdDataHeight, ref outputPtr);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("RS_MakeLCDData failed: " + result);
        *     return;
        * }
        *
        * // Copy output LCD buffer (16-bit RGB)
        * int outSize = lcdDataWidth * lcdDataHeight * 2;
        * byte[] lcdBuffer = new byte[outSize];
        * Marshal.Copy(outputPtr, lcdBuffer, 0, outSize);
        *
        * Console.WriteLine("RS_MakeLCDData succeeded.");
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_DisplayLCD")]
        public static extern int RS_DisplayLCD(int deviceHandle, IntPtr data, int dataLen, int sx, int sy, int width, int height);

        /**
        * @brief LCD display device in RealScan-F can show the 16-bit RGB data, which means that it can display the image data whose 2 bytes represent 1 pixel on the LCD. This function can change a format of image data into the format LCD can display with.
        * @param inputRData [IN] Handle to the opened device.
        * @param inputGData [IN] Pointer to image data to be displayed onto LCD. 
        * @param inputBData [IN] Length of the image data to be displayed onto LCD. DataLen should be width*height*2, since LCD can display only 16-bit RGB data. 
        * @param inputWidth [IN] Starting point of x-position of image data to be displayed. 
        * @param inputHeight [IN] Starting point of y-position of image data to be displayed. 
        * @param outputData [IN] Width of the image displayed onto LCD. The maximum size is defined as RS_LCD_WIDTH_MAX (= 320).      
        * 
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -401   \b RS_ERR_CANNOT_MAKE_LCD_DATA
        * @retval -402   \b RS_ERR_INVALID_LCD_DATA	          
        * 
        * @note RealScan-F
        * 
        * \par Example (C#)
        * @code
        * // P/Invoke declaration
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_MakeLCDData")]
        * public static extern int RS_MakeLCDData(
        *     byte[] inputRData,
        *     byte[] inputGData,
        *     byte[] inputBData,
        *     int    inputWidth,
        *     int    inputHeight,
        *     ref IntPtr outputData);
        *
        * const int RS_LCD_WIDTH_MAX  = 320;
        * const int RS_LCD_HEIGHT_MAX = 240;
        *
        * // Load raw 8-bit grayscale file (test_sample.r08)
        * byte[] lcdDataBuf = File.ReadAllBytes("test_sample.r08");
        *
        * int lcdDataWidth  = RS_LCD_WIDTH_MAX;
        * int lcdDataHeight = lcdDataBuf.Length / lcdDataWidth;
        * if (lcdDataHeight > RS_LCD_HEIGHT_MAX)
        *     lcdDataHeight = RS_LCD_HEIGHT_MAX;
        *
        * // Use the same grayscale data for R/G/B channels
        * byte[] r = lcdDataBuf;
        * byte[] g = lcdDataBuf;
        * byte[] b = lcdDataBuf;
        *
        * IntPtr buf = IntPtr.Zero;
        * int result = RS_MakeLCDData(r, g, b, lcdDataWidth, lcdDataHeight, ref buf);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("RS_MakeLCDData failed. Error: " + result);
        *     return;
        * }
        *
        * Console.WriteLine("RS_MakeLCDData succeeded.");
        * // buf now points to lcdDataWidth * lcdDataHeight * 2 bytes of 16-bit RGB data
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_MakeLCDData")]
        public static extern int RS_MakeLCDData(byte[] inputRData, byte[] inputGData, byte[] inputBData, int inputWidth, int inputHeight, ref IntPtr outputData);

        /**
        * @brief Both RealScan-10 and RealScan-G10(i) have 10 fingerprint LEDs. You can turn on/off these LEDs by RS_SetFingerLED.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param fingerIndex  [IN]\n
        *
        * | Macro Name             | Value |
        * |------------------------|-------|
        * | RS_FINGER_ALL          | 0     |
        * | RS_FINGER_LEFT_LITTLE  | 1     |
        * | RS_FINGER_LEFT_RING    | 2     |
        * | RS_FINGER_LEFT_MIDDLE  | 3     |
        * | RS_FINGER_LEFT_INDEX   | 4     |
        * | RS_FINGER_LEFT_THUMB   | 5     |
        * | RS_FINGER_RIGHT_THUMB  | 6     |
        * | RS_FINGER_RIGHT_INDEX  | 7     |
        * | RS_FINGER_RIGHT_MIDDLE | 8     |
        * | RS_FINGER_RIGHT_RING   | 9     |
        * | RS_FINGER_RIGHT_LITTLE | 10    |
        * | RS_FINGER_TWO_THUMB    | 11    |
        * | RS_FINGER_LEFT_FOUR    | 12    |
        * | RS_FINGER_RIGHT_FOUR   | 13    |
        * | RS_FINGER_TWO_LEFT1    | 14    |
        * | RS_FINGER_TWO_LEFT2    | 15    |
        * | RS_FINGER_TWO_RIGHT2   | 16    |
        * | RS_FINGER_TWO_RIGHT1   | 17    |
        * | RS_PALM_LEFT           | 18    |
        * | RS_PALM_RIGHT          | 19    |
        *
        * @param ledColor  [IN]\n
        *
        * | Macro Name   | Value |
        * |--------------|-------|
        * | RS_LED_OFF   | 0     |
        * | RS_LED_GREEN | 1     |
        * | RS_LED_RED   | 2     |
        * | RS_LED_YELLOW| 3     |
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -114   \b RS_ERR_UNSUPPORTED_COMMAND
        * 
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetFingerLED")]
        * public static extern int RS_SetFingerLED(int deviceHandle, int fingerIndex, int ledColor);
        *
        * // ...
        * int deviceHandle = 0; // Assume this is a valid handle
        *
        * // Turn on GREEN LED for "two left fingers (2)"
        * int result = RS_SetFingerLED(deviceHandle,
        *                              RS_FINGER_TWO_LEFT2,
        *                              RS_LED_GREEN);
        *
        * if (result != 0)  // 0 == RS_SUCCESS
        * {
        *     Console.WriteLine("RS_SetFingerLED failed. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetFingerLED")]
        public static extern int RS_SetFingerLED(int deviceHandle, int Finger_LED, int ledColor);
        /**
        * @brief RealScan-D has one status LED. RealScan-G1 has a blue status LED inside the platen.
        *        You can turn it on/off by RS_SetStatusLED, and RealScan-G1 also supports blinking mode.
        *        Supported devices: RealScan S60, RealScan-D, RealScan SG10(i).
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param ledCode      [IN] LED control code depending on device type.
        *
        * \b RealScan-D\n
        * | Macro Name                | Value |
        * |---------------------------|-------|
        * | RS_LED_MODE_ALL           | 0     |
        * | RS_LED_MODE_LEFT_FINGER4  | 1     |
        * | RS_LED_MODE_RIGHT_FINGER4 | 2     |
        * | RS_LED_MODE_TWO_THUMB     | 3     |
        * | RS_LED_MODE_ROLL          | 4     |
        * | RS_LED_POWER              | 5     |
        *
        * \b RealScan S60\n
        * | Macro Name                | Value |
        * |---------------------------|-------|
        * | RS_S60_LED_STATUS_OFF     | 0     |
        * | RS_S60_LED_STATUS_RED     | 1     |
        * | RS_S60_LED_STATUS_GREEN   | 2     |
        * | RS_S60_LED_STATUS_YELLOW  | 3     |
        * | RS_S60_LED_STATUS_BLUE    | 4     |
        * | RS_S60_LED_STATUS_MAGENTA | 5     |
        * | RS_S60_LED_STATUS_CYAN    | 6     |
        * | RS_S60_LED_STATUS_WHITE   | 7     |
        *
        * \b RealScan SG10(i)\n
        * | Macro Name                | Value |
        * |---------------------------|-------|
        * | RS_SG10_LED_STATUS_OFF    | 0     |
        * | RS_SG10_LED_STATUS_RED    | 1     |
        * | RS_SG10_LED_STATUS_GREEN  | 2     |
        * | RS_SG10_LED_STATUS_BLUE   | 3     |
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -114   \b RS_ERR_UNSUPPORTED_COMMAND
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetStatusLED")]
        * public static extern int RS_SetStatusLED(int deviceHandle, int ledCode);
        *
        * // ...
        * int deviceHandle = 0; // valid handle
        *
        * // Turn S60 LED to BLUE
        * int result = RS_SetStatusLED(deviceHandle, RS_S60_LED_STATUS_BLUE);
        *
        * if (result != 0) {
        *     Console.WriteLine("RS_SetStatusLED failed. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetStatusLED")]
        public static extern int RS_SetStatusLED(int deviceHandle, int ledCode);

        /**
        * @brief Both RealScan-10 and RealScan-G10(i) have 4 mode LEDs that indicate the current
        *        capture mode. You can turn these LEDs on or off using RS_SetModeLED.
        *        (Supported: RealScan-G10(i), RealScan S60, RealScan SG10(i))
        *
        * @param deviceHandle [IN] Handle to the opened device.
        *
        * @param ledIndex [IN] LED index to control.\n
        * | Macro Name                   | Value |
        * |------------------------------|-------|
        * | RS_LED_MODE_ALL              | 0     |
        * | RS_LED_MODE_LEFT_FINGER4     | 1     |
        * | RS_LED_MODE_RIGHT_FINGER4    | 2     |
        * | RS_LED_MODE_TWO_THUMB        | 3     |
        * | RS_LED_MODE_ROLL             | 4     |
        * | RS_LED_POWER                 | 5     |
        *
        * @param isOn [IN] TRUE to turn on the LED, FALSE to turn it off.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -114   \b RS_ERR_UNSUPPORTED_COMMAND
        *
        * @note Supported devices:
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetModeLED")]
        * public static extern int RS_SetModeLED(int deviceHandle, int ledIndex, bool isOn);
        *
        * // ...
        * int deviceHandle = 0;  // valid handle
        *
        * // Turn on POWER LED
        * int result = RS_SetModeLED(deviceHandle, RS_LED_POWER, true);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to set mode LED. Error = " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetModeLED")]
        public static extern int RS_SetModeLED(int deviceHandle, int Mode_LED, bool isOn);

        /**
        * @brief Emits a beep sound. Two predefined beep patterns are available.
        *        (Supported: RealScan-10, RealScan-D, RealScan-F, RealScan-G10(i), RealScan SG10(i))
        *
        * @param deviceHandle [IN] Handle to the opened device.
        *
        * @param beepPattern [IN] Beep pattern to play.\n
        * | Macro Name                 | Value |
        * |----------------------------|-------|
        * | RS_BEEP_PATTERN_NONE       | 0     |
        * | RS_BEEP_PATTERN_1          | 1     |
        * | RS_BEEP_PATTERN_2          | 2     |
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-10, RealScan-D, RealScan-F, RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_Beep")]
        * public static extern int RS_Beep(int deviceHandle, int beepPattern);
        *
        * // ...
        * int deviceHandle = 0;  // valid device handle
        *
        * // Play beep pattern 2
        * int result = RS_Beep(deviceHandle, RS_BEEP_PATTERN_2);
        *
        * if (result != 0)
        * {
        *     Console.WriteLine("Failed to play beep. Error = " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_Beep")]
        public static extern int RS_Beep(int deviceHandle, int beepPattern);

        /**
        * @brief Reads the current status of keypad (button) inputs on the device.
        *
        * RealScan-10 has two keys (PLAY / STOP).  
        * RealScan-D has one input switch and one status LED.  
        * This function returns the current pressed state as a bit mask.
        *
        * @param deviceHandle [IN]  Handle to the opened device.
        * @param keyMask      [OUT] Pointer to a variable that receives the pressed
        *                            key bit mask.
        *                            For RealScan-10, if both keys are pressed, the
        *                            value will be (RS_REALSCAN10_PLAY_KEY | RS_REALSCAN10_STOP_KEY).
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -10    \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -114   \b RS_ERR_UNSUPPORTED_COMMAND
        *
        * @note Supported devices: RealScan-10, RealScan-D
        *
        * \par Example (C#)
        * @code
        * // C# example for calling RS_GetKeyStatus
        *
        * int deviceHandle = 0;     // Assume valid handle
        * int keyMask = 0;          // Variable to store pressed key bits
        *
        * int result = RealScanSDK.RS_GetKeyStatus(deviceHandle, ref keyMask);
        *
        * if (result == 0)  // RS_SUCCESS
        * {
        *     // Check keyMask bits, e.g.:
        *     // if ((keyMask & RS_REALSCAN10_PLAY_KEY) != 0) { }
        *     // if ((keyMask & RS_REALSCAN10_STOP_KEY) != 0) { }
        * }
        * else
        * {
        *     Console.WriteLine("Failed to read key status. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetKeyStatus")]
        public static extern int RS_GetKeyStatus(int deviceHandle, ref int keyCode);
        /**
        * @brief Both RealScan-10 and RealScan-G10(i) have two input switches, four mode LEDs, 
        *        and 10 fingerprint LEDs. RealScan-D device has one input switch and one status LED.
        *
        * This function enables specific keypad inputs by setting a key activation mask.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param keyMask      [IN] Bitmask representing which keys should be activated.
        *
        * | Macro Name                 | Value |
        * |----------------------------|-------|
        * | RS_REALSCAN10_NO_KEY       | 0     |
        * | RS_REALSCAN10_PLAY_KEY     | 32    |
        * | RS_REALSCAN10_STOP_KEY     | 64    |
        * | RS_REALSCAN10_ALL_KEYS     | 127   |
        * | RS_REALSCAND_NO_KEY        | 0     |
        * | RS_REALSCAND_KEY_0         | 32    |
        * | RS_REALSCAND_ALL_KEYS      | 127   |
        * | RS_REALSCANF_NO_KEY        | 0     |
        * | RS_REALSCANF_UP_KEY        | 1     |
        * | RS_REALSCANF_DOWN_KEY      | 2     |
        * | RS_REALSCANF_LEFT_KEY      | 4     |
        * | RS_REALSCANF_RIGHT_KEY     | 8     |
        * | RS_REALSCANF_PLAY_KEY      | 32    |
        * | RS_REALSCANF_STOP_KEY      | 64    |
        * | RS_REALSCANF_FOOTSWITCH    | 128   |
        * | RS_REALSCANF_ALL_KEYS      | 255   |
        *
        * @return Function success status.
        * @retval 0      \b RS_SUCCESS
        * @retval -102   \b RS_ERR_INVALID_HANDLE
        * @retval -114   \b RS_ERR_UNSUPPORTED_COMMAND
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0; 
        *
        * // Enable all keypad inputs for RealScan-10
        * int result = RealScanSDK.RS_SetActiveKey(deviceHandle, RealScanSDK.RS_REALSCAN10_ALL_KEYS);
        *
        * if (result == 0)  // RS_SUCCESS
        * {
        *     Console.WriteLine("Active key mask set successfully.");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to set active key mask. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetActiveKey")]
        public static extern int RS_SetActiveKey(int deviceHandle, uint keyMask);

        /**
        * @brief RealScan-10, RealScan-G10(i), RealScan-F, RealScan-FC, and RealScan SG10(i) 
        *        can play WAV audio files.  
        *
        * The WAV file must meet the following requirements:
        * - 44,100 Hz sample rate  
        * - 16-bit  
        * - Mono PCM format  
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param wavFile      [IN] Path or filename of the WAV file to be played.
        *
        * @return Function success status as an error code.
        * @retval 0       \b RS_SUCCESS
        * @retval -10     \b RS_ERR_SDK_UNINITIALIZED
        * @retval -16     \b RS_ERR_CANNOT_OPEN_FILE
        * @retval -17     \b RS_ERR_CANNOT_READ_FILE
        * @retval -102    \b RS_ERR_INVALID_HANDLE
        * @retval -113    \b RS_ERR_UNSUPPORTED_WAV_FORMAT
        *
        * @note Supported devices:
        *       RealScan-10, RealScan-G10(i), RealScan-F, RealScan-FC, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;
        *
        * // Play WAV file (must be 44.1kHz / 16-bit / Mono PCM)
        * int result = RealScanSDK.RS_PlayWav(deviceHandle, "test.wav");
        *
        * if (result == 0)  // RS_SUCCESS
        * {
        *     Console.WriteLine("WAV playback started.");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to play WAV. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_PlayWav")]
        public static extern int RS_PlayWav(int deviceHandle, String wavFile);
        /**
        * @brief Registers a keypad callback function that is triggered when a key is pressed.  
        *        (Supported on RealScan-D, RealScan-F, RealScan-G10(i))
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param callback     [IN] Callback function pointer.\n
        *                     The callback prototype is:\n
        *                     typedef void (*RSKeypadCallback)(int deviceHandle, unsigned keyCode);
        *
        * @return Function success status as an error code.
        * @retval 0       \b RS_SUCCESS
        * @retval -10     \b RS_ERR_SDK_UNINITIALIZED
        * @retval -102    \b RS_ERR_INVALID_HANDLE
        * @retval -114    \b RS_ERR_UNSUPPORTED_COMMAND
        *
        * @note The callback is invoked asynchronously when a keypad button is pressed.
        *
        * \par Example (C#)
        * @code
        * // 1. Define keypad callback delegate (must match native function signature)
        * public delegate void RSKeypadCallback(int deviceHandle, uint keyCode);
        *
        * // 2. Implement callback function
        * static void KeypadEventHandler(int deviceHandle, uint keyCode)
        * {
        *     switch (keyCode)
        *     {
        *         case RealScanSDK.RS_REALSCAN10_PLAY_KEY:
        *             RealScanSDK.RS_Beep(deviceHandle, RealScanSDK.RS_BEEP_PATTERN_1);
        *             break;
        *
        *         case RealScanSDK.RS_REALSCAN10_STOP_KEY:
        *             RealScanSDK.RS_Beep(deviceHandle, RealScanSDK.RS_BEEP_PATTERN_2);
        *             break;
        *     }
        * }
        *
        * // 3. Register callback
        * int deviceHandle = 0;   // Assume valid initialized device handle
        * int result = RealScanSDK.RS_RegisterKeypadCallback(deviceHandle, KeypadEventHandler);
        *
        * if (result == 0) // RS_SUCCESS
        * {
        *     Console.WriteLine("Keypad callback registered.");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to register keypad callback. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterKeypadCallback")]
        public static extern int RS_RegisterKeypadCallback(int deviceHandle, RSKeypadCallback callback);
        /**
        * @brief By default, the scanner device emits predefined beep sounds as follows:\n
        * Capturing of a rolling finger is started in automatic mode: RS_BEEP_PATTERN_1\n
        * Capture success: RS_BEEP_PATTERN_1\n
        * Capture failure: RS_BEEP_PATTERN_2\n
        * You can change or disable these sounds using RS_SetCaptureBeep.
        * (RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i))
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param startingBeep  [IN] Beep sound for the start of rolling capture in automatic mode.\n
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | RS_BEEP_PATTERN_NONE    | 0     |
        * | RS_BEEP_PATTERN_1       | 1     |
        * | RS_BEEP_PATTERN_2       | 2     |
        *
        * @param successBeep  [IN] Beep sound for the capture success.\n
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | RS_BEEP_PATTERN_NONE    | 0     |
        * | RS_BEEP_PATTERN_1       | 1     |
        * | RS_BEEP_PATTERN_2       | 2     |
        *
        * @param failBeep  [IN] Beep sound for the capture failure.\n
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | RS_BEEP_PATTERN_NONE    | 0     |
        * | RS_BEEP_PATTERN_1       | 1     |
        * | RS_BEEP_PATTERN_2       | 2     |
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -13    RS_ERR_INVALID_HANDLE
        * @retval -102   RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;  // Assume initialized device handle
        *
        * // Change beep: Start = Pattern 1, Success = Pattern 1, Failure = Pattern 2
        * int result = RealScanSDK.RS_SetCaptureBeep(
        *                  deviceHandle,
        *                  RealScanSDK.RS_BEEP_PATTERN_1,
        *                  RealScanSDK.RS_BEEP_PATTERN_1,
        *                  RealScanSDK.RS_BEEP_PATTERN_2);
        *
        * if (result == 0) // RS_SUCCESS
        * {
        *     Console.WriteLine("Capture beep settings updated.");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to set capture beep. Error: " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetCaptureBeep")]
        public static extern int RS_SetCaptureBeep(int deviceHandle, int startingBeep, int successBeep, int failBeep);


        // Misc APIs
        /**
        * @brief Decompresses a WSQ file into raw pixel data in memory.
        * 
        * @param rawBuffer [IN] Raw pixel data in memory.
        * @param width [IN] Width of the rawBuffer.  
        * @param height [IN] Height of the rawBuffer.
        * @param jp2Buffer [OUT] JP2000 raw buffer.
        * @param jp2BufferLen [OUT] JP2000 raw buffer length.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -13    \b RS_ERR_INVALID_PARAM
        * @retval -14    \b RS_ERR_MEM_FULL
        *
        * \par Example (C#)
        * @code
        * // Example values
        * int imageWidth = 500;
        * int imageHeight = 500;
        *
        * // Allocate managed raw image buffer
        * byte[] rawImageData = new byte[imageWidth * imageHeight];
        *
        * // Fill rawImageData[...] with your image data here
        *
        * // Prepare JP2 output buffer
        * byte[] jp2Buffer = new byte[imageWidth * imageHeight * 2];   // Ensure large enough
        * int jp2BufferLen = 0;
        *
        * // Call JP2 encoder
        * int result = RealScanSDK.RS_EncodeJP2(
        *                  rawImageDataPtr,       // IntPtr pointing to image data
        *                  imageWidth,
        *                  imageHeight,
        *                  jp2Buffer,
        *                  ref jp2BufferLen);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("JP2 encoding successful. Output size = " + jp2BufferLen);
        * }
        * else
        * {
        *     Console.WriteLine("RS_EncodeJP2 failed. Error = " + result);
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_EncodeJP2")]
        public static extern int RS_EncodeJP2(IntPtr pixelData, int imageWidth, int imageHeight, byte[] jp2Buffer, ref int jp2BufferLength);
        /**
        * @brief Saves the raw pixel data returned by capturing functions into a BMP file.
        * 
        * @param pixelData [IN] Pointer to the raw pixel data. 
        * @param imageWidth [IN] Width of the image. 
        * @param imageHeight [IN] Height of the image. 
        * @param filename [IN] Name of the BMP file
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -14     \b RS_ERR_MEM_FULL
        * @retval -18     \b RS_ERR_CANNOT_WRITE_FILE
        *
        * \par Example (C#)
        * @code
        * int width = 300;
        * int height = 300;
        *
        * // Allocate raw pixel buffer (grayscale example)
        * byte[] pixelData = new byte[width * height];
        *
        * // Fill pixelData[...] with image content here
        *
        * // Pin buffer to obtain IntPtr
        * GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
        * IntPtr pixelPtr = handle.AddrOfPinnedObject();
        *
        * int result = RealScanSDK.RS_SaveBitmap(pixelPtr, width, height, "test.bmp");
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Bitmap saved successfully.");
        * }
        * else
        * {
        *     Console.WriteLine("Failed to save bitmap. Error = " + result);
        * }
        *
        * handle.Free();
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SaveBitmap")]
        public static extern int RS_SaveBitmap(IntPtr pixelData, int imageWidth, int imageHeight, string filename);


        /**
        * @brief Saves the raw pixel data returned by capturing functions in a memory buffer.
        * 
        * @param pixelData [IN] Pointer to the raw pixel data. 
        * @param imageWidth [IN] Width of the image. 
        * @param imageHeight [IN] Height of the image. 
        * @param imageBuffer [OUT] buffer of a BMP file.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -14     \b RS_ERR_MEM_FULL
        *
        * \par Example (C#)
        * @code
        * int width = 300;
        * int height = 300;
        *
        * // Raw grayscale pixel buffer
        * byte[] pixelData = new byte[width * height];
        *
        * // Fill pixelData[...] with image content
        *
        * // BMP output buffer (allocate a sufficiently large buffer)
        * byte[] bmpBuffer = new byte[width * height * 4]; 
        * // (BMP size varies; adjust as needed)
        *
        * // Pin buffers
        * GCHandle pixelHandle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
        * GCHandle outHandle   = GCHandle.Alloc(bmpBuffer, GCHandleType.Pinned);
        *
        * IntPtr pixelPtr = pixelHandle.AddrOfPinnedObject();
        * IntPtr outPtr   = outHandle.AddrOfPinnedObject();
        *
        * int result = RealScanSDK.RS_SaveBitmapMem(pixelPtr, width, height, bmpBuffer);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Bitmap saved to memory buffer.");
        * }
        * else
        * {
        *     Console.WriteLine($"Failed to save bitmap. Error = {result}");
        * }
        *
        * pixelHandle.Free();
        * outHandle.Free();
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SaveBitmapMem")]
        public static extern int RS_SaveBitmapMem(IntPtr pixelData, int imageWidth, int imageHeight, byte[] imageData);

        /**
        * @brief Compresses the raw pixel data returned by capturing functions into a WSQ file. See FBI's Criminal 
            Justice Information Services (CJIS) document, "WSQ Gray-scale Fingerprint Compressions 
            Specification" for details.
        * 
        * @param rawBuffer [IN] Raw pixel data in memory.
        * @param width [IN] Width of the rawBuffer.  
        * @param height [IN] Height of the rawBuffer. 
        * @param ratio [IN] Specifies the bit rate of the compression. Note that it should be thought of as the level of
        * acceptable distortion rather than a tool to achieve a specific file size or an exact
        * compression ratio. For example, 2.25 and 0.75 roughly yields 5:1 and 15:1 compression
        * respectively. However, the actual compression ratio will vary according to the
        * characteristics of the original image. (Range: 0.1~7.5) wsqBuffer Compressed
        * WSQ pixel data in memory. wsqBufferLen Length of the wsq buffer.
        * @param wsqBuffer [OUT] Compressed WSQ pixel data in memory. 
        * @param wsqBufferLen [OUT] Length of the wsq buffer.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -13     \b RS_ERR_INVALID_PARAM
        * @retval -14     \b RS_ERR_MEM_FULL     
        *
        * \par Example (C#)
        * @code
        * int width = 500;
        * int height = 500;
        *
        * // Raw grayscale image buffer
        * byte[] imageData = new byte[width * height];
        *
        * // Fill imageData[...] with pixel data
        *
        * // Allocate WSQ buffer (initially same size as raw data)
        * int wsqBufferLen = width * height;
        * byte[] wsqBuffer = new byte[wsqBufferLen];
        *
        * // Pin buffers
        * GCHandle imgHandle = GCHandle.Alloc(imageData, GCHandleType.Pinned);
        * GCHandle wsqHandle = GCHandle.Alloc(wsqBuffer, GCHandleType.Pinned);
        *
        * IntPtr imgPtr = imgHandle.AddrOfPinnedObject();
        * IntPtr wsqPtr = wsqHandle.AddrOfPinnedObject();
        *
        * int result = RealScanSDK.RS_EncodeWSQ(
        *                 imgPtr,
        *                 width,
        *                 height,
        *                 0.75f,      // compression ratio
        *                 wsqBuffer,
        *                 ref wsqBufferLen);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"WSQ compression OK. Size = {wsqBufferLen} bytes");
        * }
        * else
        * {
        *     Console.WriteLine($"RS_EncodeWSQ failed. Error = {result}");
        * }
        *
        * imgHandle.Free();
        * wsqHandle.Free();
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_EncodeWSQ")]
        public static extern int RS_EncodeWSQ(IntPtr pixelData, int imageWidth, int imageHeight, float ratio, byte[] wsqBuffer, ref int wsqBufferLength);

        /**
        * @brief Retrieves a short description of the specified error code.
        *
        * This C# wrapper converts the ANSI character buffer returned by
        * RS_GetErrStringChar() into a managed string.
        *
        * @param errorCode [IN] Error code returned by an API.
        * @param errorMsg  [OUT] Error message string.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        *
        * \par Example (C#)
        * @code
        * int deviceCount = 0;
        * int result = RealScanSDK.RS_InitSDK(null, 0, ref deviceCount);
        *
        * if (result != RealScanSDK.RS_SUCCESS)
        * {
        *     string errMsg = "";
        *     RealScanSDK.RS_GetErrString(result, ref errMsg);
        *
        *     Console.WriteLine($"RS_InitSDK failed : {errMsg} ({result})");
        *     return;
        * }
        *
        * Console.WriteLine($"SDK initialized. Device count = {deviceCount}");
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetErrStringChar")]
        public static extern int RS_GetErrStringChar(int errorCode, byte[] errorMsg);

        public static int RS_GetErrString(int errorCode, ref string errorMsg)
        {
            errorMsg = "";
            byte[] strErrMsg = new byte[1024];
            int rv = RS_GetErrStringChar(errorCode, strErrMsg);
            for (int i = 0; i < 1024; i++)
            {
                errorMsg += (char)strErrMsg[i];
                if (strErrMsg[i] == 0) break;
            }
            return rv;
        }

        /**
        * @brief Retrieves a short description of the specified capture status code.
        *
        * @param statusCode [IN] Status code returned by an API.
        * @param statusMsg  [OUT] Reference of the status string.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        *
        * \par Example (C#)
        * @code
        * void AdvPreviewCallback(int deviceHandle, int errorCode,
        *                         IntPtr imageData, int width, int height,
        *                         int quality, int status)
        * {
        *     int captureStatus = status;
        *
        *     string statusMsg = "";
        *     RealScanSDK.RS_GetCaptureStatusString(captureStatus, ref statusMsg);
        *
        *     Console.WriteLine($"Capture Status: {statusMsg}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetCaptureStatusStringChar")]
        public static extern int RS_GetCaptureStatusStringChar(int statusCode, byte[] statusMsg);

        public static int RS_GetCaptureStatusString(int statusCode, ref string statusMsg)
        {
            statusMsg = "";
            byte[] strStatusMsg = new byte[1024];
            int rv = RS_GetCaptureStatusStringChar(statusCode, strStatusMsg);
            for (int i = 0; i < 1024; i++)
            {
                statusMsg += (char)strStatusMsg[i];
                if (strStatusMsg[i] == 0) break;
            }
            return rv;
        }

        /**
          * @brief Gets the version information of the SDK.
          *
          * @param sdkInfo [OUT] Structure that receives SDK version information.
          * 
          * @return Function success status as an error code.
          * @retval 0      RS_SUCCESS
          *
          * \par Example (C#)
          * @code
          * RSSDKInfo info = new RSSDKInfo();
          * int result = RealScanSDK.RS_GetSDKInfo(ref info);
          *
          * if (result == RealScanSDK.RS_SUCCESS)
          * {
          *     Console.WriteLine($"SDK Version: {info.Major}.{info.Minor}.{info.Build}");
          * }
          * else
          * {
          *     Console.WriteLine("Failed to get SDK info.");
          * }
          * @endcode
          */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetSDKInfo")]
        public static extern int RS_GetSDKInfo(ref RSSDKInfo sdkInfo);
        /**
        * @brief Releases the memory allocated by the DLL.
        * @param imageData [IN] Pointer to the image data to be freed.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        *
        * \par Example (C#)
        * @code
        * IntPtr imageData = IntPtr.Zero;
        *
        * // imageData = pointer returned from a capture API such as RS_GetCaptureImage()
        *
        * // Free DLL-allocated buffer
        * RealScanSDK.RS_FreeImageData(imageData);
        *
        * Console.WriteLine("Image memory freed.");
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_FreeImageData")]
        public static extern void RS_FreeImageData(IntPtr imageData);


        // Win32 APIs
        /**
        * @brief Retrieves the client-area rectangle of a window.
        *
        * This function is a Win32 API imported into C#. It retrieves the size of
        * the window's client area (the drawable region), which is commonly used
        * when configuring preview windows in RealScan SDK.
        *
        * @param hWnd [IN] Handle to the window.
        * @param rect [OUT] RSRect structure that receives the client rectangle.
        *
        * @return TRUE if the function succeeds, FALSE otherwise.
        *
        * \par Example (C#)
        * @code
        * RSRect rect = new RSRect();
        * bool ok = RealScanSDK.GetClientRect(previewPanel.Handle, ref rect);
        *
        * if (ok)
        * {
        *     Console.WriteLine($"ClientRect = {rect.Left}, {rect.Top}, {rect.Right}, {rect.Bottom}");
        * }
        * else
        * {
        *     Console.WriteLine("GetClientRect failed.");
        * }
        * @endcode
        */
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, ref RSRect rect);



        //Option APIs

        /**
        * @brief Sets the Live Finger Detection (LFD) level for supported devices.
        *
        * Lower LFD level reduces FRR (False Reject Rate) but may increase the risk
        * of accepting fake fingerprints. Higher levels provide stronger fake finger
        * detection but may slightly increase FRR.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param LFDLevel     [IN] LFD sensitivity level.\n
        *
        * | Macro Name         | Value |
        * |--------------------|-------|
        * | RS_LFD_OFF         | 0     |
        * | RS_LFD_LEVEL_1     | 1     |
        * | RS_LFD_LEVEL_2     | 2     |
        * | RS_LFD_LEVEL_3     | 3     |
        * | RS_LFD_LEVEL_4     | 4     |
        * | RS_LFD_LEVEL_5     | 5     |
        *
        * @return Status code.
        * @retval 0      RS_SUCCESS
        * @retval -114   RS_ERR_UNSUPPORTED_COMMAND
        *
        * @note Supported devices: RealScan-D, RealScan-G10(i), RealScan S60, RealScan SG10(i) 
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;
        * int result = RealScanSDK.RS_SetLFDLevel(deviceHandle, RealScanSDK.RS_LFD_LEVEL_1);
        *
        * if (result != RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Failed to set LFD level.");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetLFDLevel")]
        public static extern int RS_SetLFDLevel(int deviceHandle, int LFDLevel);

        /**
        * @brief When the scanner surface is dirty, image quality may degrade significantly.
        *        With automatic calibration enabled, the SDK attempts to re-calibrate the scanner
        *        at the start of every capture session to reduce background noise.
        *
        * There are two cases when automatic calibration may fail:
        * (1) If the sensor surface is dirtier than the internal threshold,
        *     RS_ERR_SENSOR_DIRTY will be returned.
        *     → Clean the platen surface and retry.
        *
        * (2) If a finger is already placed on the platen before capturing starts,
        *     RS_ERR_FINGER_EXIST will be returned.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param automatic    [IN] TRUE to enable automatic calibration (default ON),
        *                           FALSE to disable it.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -102   RS_ERR_INVALID_HANDLE
        * @retval -212   RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0; // Assume initialized handle
        *
        * int result = RealScanSDK.RS_SetAutomaticCalibrate(deviceHandle, true);
        *
        * if (result != RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"Failed to enable automatic calibration. Error: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetAutomaticCalibrate")]
        public static extern int RS_SetAutomaticCalibrate(int deviceHandle, bool automatic);

        /**
        * @brief Retrieves the current setting of the automatic calibration option.
        *
        * @param deviceHandle  [IN] Handle to the opened device.
        * @param automatic     [OUT] Current automatic calibration setting (true/false).
        *
        * @return Function success status as an error code.
        * @retval 0     RS_SUCCESS
        * @retval -10   RS_ERR_SDK_UNINITIALIZED
        * @retval -102  RS_ERR_INVALID_HANDLE
        * @retval -212  RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;   // Assume initialized device handle
        * bool isAutomatic = false;
        *
        * int result = RealScanSDK.RS_GetAutomaticCalibrate(deviceHandle, ref isAutomatic);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"Automatic calibration is currently: {isAutomatic}");
        * }
        * else
        * {
        *     Console.WriteLine($"Failed to get calibration setting. Error = {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetAutomaticCalibrate")]
        public static extern int RS_GetAutomaticCalibrate(int deviceHandle, ref bool automatic);
        /**
        * @brief [Unsupported] Sets the automatic contrast adjustment option.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param automatic    [IN] Desired setting for automatic contrast.
        *
        * @return Function success status as an error code.
        * @retval -114  RS_ERR_UNSUPPORTED_COMMAND   Function not supported on this device or SDK version.
        *
        * @note This API is currently unsupported and will always return RS_ERR_UNSUPPORTED_COMMAND.
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;  // Assume valid initialized handle
        *
        * int result = RealScanSDK.RS_SetAutomaticContrast(deviceHandle, true);
        *
        * if (result == RealScanSDK.RS_ERR_UNSUPPORTED_COMMAND)
        * {
        *     Console.WriteLine("RS_SetAutomaticContrast is unsupported on this device.");
        * }
        * else
        * {
        *     Console.WriteLine($"Unexpected result: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetAutomaticContrast")]
        public static extern int RS_SetAutomaticContrast(int deviceHandle, bool automatic);
        /**
        * @brief [Unsupported] Retrieves the current setting of the automatic contrast option.
        *
        * @param deviceHandle [IN] Handle to the opened device.
        * @param automatic    [OUT] Current setting of the automatic contrast option.
        *
        * @return Function success status as an error code.
        * @retval -114  RS_ERR_UNSUPPORTED_COMMAND   Function not supported on this device or SDK version.
        *
        * @note This API is currently unsupported and will always return RS_ERR_UNSUPPORTED_COMMAND.
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;          // Assume valid handle
        * bool automatic = false;        // Output variable
        *
        * int result = RealScanSDK.RS_GetAutomaticContrast(deviceHandle, ref automatic);
        *
        * if (result == RealScanSDK.RS_ERR_UNSUPPORTED_COMMAND)
        * {
        *     Console.WriteLine("RS_GetAutomaticContrast is unsupported on this device.");
        * }
        * else
        * {
        *     Console.WriteLine($"Unexpected result: {result}, automatic = {automatic}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetAutomaticContrast")]
        public static extern int RS_GetAutomaticContrast(int deviceHandle, ref bool automatic);

        /**
        * @brief For V1.3 or later versions, a new contrast enhancement algorithm is added. With it, you can capture
        *        images even under difficult lighting conditions such as direct sunlight. Though the new algorithm is
        *        more effective for enhancing image quality, it consumes more CPU resources. The default is off.
        *        The option has no effect when either automatic or manual contrast enhancement is enabled.
        *
        * @param deviceHandle  [IN] Handle to the opened device.
        * @param enabled       [IN] If true, enables the advanced contrast enhancement. Otherwise, it is disabled.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -102   RS_ERR_INVALID_HANDLE
        * @retval -212   RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0; // Assume valid handle
        *
        * int result = RealScanSDK.RS_SetAdvancedContrastEnhancement(deviceHandle, true);
        *
        * if (result != RealScanSDK.RS_SUCCESS)
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"RS_SetAdvancedContrastEnhancement failed: {err} ({result})");
        * }
        * else
        * {
        *     Console.WriteLine("Advanced contrast enhancement enabled.");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetAdvancedContrastEnhancement")]
        public static extern int RS_SetAdvancedContrastEnhancement(int deviceHandle, bool enabled);
        /**
         * @brief Retrieves the current setting of the advanced contrast enhancement.
         *
         * @param deviceHandle  [IN]  Handle to the opened device.
         * @param enabled       [OUT] Current setting of the option.
         *
         * @return Function success status as an error code.
         * @retval 0     RS_SUCCESS
         * @retval -10   RS_ERR_SDK_UNINITIALIZED
         * @retval -102  RS_ERR_INVALID_HANDLE
         * @retval -212  RS_ERR_CAPTURE_IS_RUNNING
         *
         * @note Supported devices:
         *       RealScan-D, RealScan-F, RealScan-FC,
         *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
         *
         * \par Example (C#)
         * @code
         * int deviceHandle = 0;   // Assume valid handle
         * bool enabled = false;
         *
         * int result = RealScanSDK.RS_GetAdvancedContrastEnhancement(deviceHandle, ref enabled);
         *
         * if (result != RealScanSDK.RS_SUCCESS)
         * {
         *     string errMsg = "";
         *     RealScanSDK.RS_GetErrString(result, ref errMsg);
         *     Console.WriteLine($"RS_GetAdvancedContrastEnhancement failed: {errMsg} ({result})");
         * }
         * else
         * {
         *     Console.WriteLine($"Advanced contrast enhancement enabled = {enabled}");
         * }
         * @endcode
         */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetAdvancedContrastEnhancement")]
        public static extern int RS_GetAdvancedContrastEnhancement(int deviceHandle, ref bool enabled);

        /**
        * @brief Retrieves the current roll fingerprint capture options.
        *
        * This function returns the current configuration for roll direction, duration,
        * and roll quality profile.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        * @param rollDirection [OUT] Pointer to store roll direction.\n
        * | Macro Name          | Value |
        * |--------------------------|-------|
        * | RS_ROLL_DIR_L2R      | 0     |
        * | RS_ROLL_DIR_R2L      | 1     |
        * | RS_ROLL_DIR_AUTO     | 2     |
        * | RS_ROLL_DIR_AUTO_M   | 3     |
        *
        * @param rollTime [OUT] Pointer to store roll duration in milliseconds.
        *
        * @param rollProfile [OUT] Pointer to store roll quality profile.\n
        * | Macro Name               | Value |
        * |--------------------------|-------|
        * | RS_ROLL_PROFILE_LOW      | 1     |
        * | RS_ROLL_PROFILE_NORMAL   | 2     |
        * | RS_ROLL_PROFILE_HIGH     | 3     |
        *
        * @return Function success status as an error code.
        * @retval 0     RS_SUCCESS
        * @retval -10   RS_ERR_SDK_UNINITIALIZED
        * @retval -102  RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;   // Assume valid handle
        *
        * int direction = 0;
        * int rollTime = 0;
        * int profile = 0;
        *
        * int result = RealScanSDK.RS_GetRollFingerOption(deviceHandle,
        *                                                 ref direction,
        *                                                 ref rollTime,
        *                                                 ref profile);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"Roll Option → Direction: {direction}, Time: {rollTime}ms, Profile: {profile}");
        * }
        * else
        * {
        *     string errMsg = "";
        *     RealScanSDK.RS_GetErrString(result, ref errMsg);
        *     Console.WriteLine($"RS_GetRollFingerOption failed: {errMsg} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetRollFingerOption")]
        public static extern int RS_GetRollFingerOption(int deviceHandle, ref int rollDirection, ref int rollTime, ref int rollProfile);

        /**
        * @brief [Unsupported] Sets the manual contrast level.
        *
        * @param deviceHandle   [IN] Handle to the opened device.
        * @param contrastLevel  [IN] Desired manual contrast level.
        *
        * @return Function success status as an error code.
        * @retval -114  RS_ERR_UNSUPPORTED_COMMAND   Function not supported on this device or SDK version.
        *
        * @note This API is currently unsupported and will always return RS_ERR_UNSUPPORTED_COMMAND.
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;   // Assume valid handle
        *
        * int result = RealScanSDK.RS_SetManualContrast(deviceHandle, 3);
        *
        * if (result == RealScanSDK.RS_ERR_UNSUPPORTED_COMMAND)
        * {
        *     Console.WriteLine("RS_SetManualContrast is not supported on this device.");
        * }
        * else
        * {
        *     Console.WriteLine($"Unexpected return value: {result}");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetManualContrast")]
        public static extern int RS_SetManualContrast(int deviceHandle, int contrastLevel);
        /**
        * @brief Same as RS_SetPostProcessing except that it can configure the level of background noise reduction.
        *        The available values are between 10(min) and 200(max). The default is 100.
        *
        * @param deviceHandle      [IN] Handle to the opened device.
        * @param contrastEnhancement [IN] If true, apply contrast enhancement. Default is off.
        * @param noiseReduction    [IN] If true, apply noise reduction. Default is off.
        * @param reductionLevel    [IN] Noise reduction level (10–200). Default is 100.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -102   RS_ERR_INVALID_HANDLE
        * @retval -212   RS_ERR_CAPTURE_IS_RUNNING
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;  // Assume valid device handle
        *
        * // Enable both contrast and noise reduction, with reduction level = 100
        * int result = RealScanSDK.RS_SetPostProcessingEx(deviceHandle, true, true, 100);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Post-processing options updated successfully.");
        * }
        * else
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"Failed to set post-processing options: {err} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetPostProcessingEx")]
        public static extern int RS_SetPostProcessingEx(int deviceHandle, bool contrastEnhancement, bool noiseReduction, int reductionLevel);
        /**
        * @brief Retrieves the current settings of post processing functions.
        *
        * @param deviceHandle       [IN]  Handle to the opened device.
        * @param contrastEnhancement [OUT] Current setting of contrast enhancement option.
        * @param noiseReduction      [OUT] Current setting of noise reduction option.
        * @param reductionLevel      [OUT] Current noise reduction level.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -102   RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;  // Assume valid device handle
        * bool contrast = false;
        * bool noise = false;
        * int level = 0;
        *
        * int result = RealScanSDK.RS_GetPostProcessingEx(
        *     deviceHandle,
        *     ref contrast,
        *     ref noise,
        *     ref level
        * );
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"Contrast: {contrast}, Noise: {noise}, Level: {level}");
        * }
        * else
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"Failed to get post-processing settings: {err} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetPostProcessingEx")]
        public static extern int RS_GetPostProcessingEx(int deviceHandle, ref bool contrastEnhancement, ref bool noiseReduction, ref int reductionLevel);
        /**
        * @brief Gets current LFD level used for RealScan-D, RealScan-G10(i), RealScan S60, RealScan SG10(i)(Support is scheduled).
        *
        * @param deviceHandle [IN]  Handle to the opened device.
        * @param LFDLevel     [OUT] The current LFD level.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -114   RS_ERR_UNSUPPORTED_COMMAND
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-G10(i), RealScan S60, RealScan SG10(i)(Support is scheduled)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;   // Assume valid handle
        * int level = 0;
        *
        * int result = RealScanSDK.RS_GetLFDLevel(deviceHandle, ref level);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Current LFD Level = " + level);
        * }
        * else
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"Failed to get LFD level: {err} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetLFDLevel")]
        public static extern int RS_GetLFDLevel(int deviceHandle, ref int LFDLevel);

        /**
        * @brief Retrieves the last captured LFD (Live Finger Detection) scores for each finger.
        *        The results are returned in the RSLFDResult structure.
        *
        * @param deviceHandle [IN]  Handle to the opened device.
        * @param psLFDResult  [OUT] Reference to an RSLFDResult structure receiving LFD results.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -5     RS_ERR_NO_DATA
        * @retval -114   RS_ERR_UNSUPPORTED_COMMAND
        * @retval -232   RS_ERR_LFD_PROCESSING
        * @retval -233   RS_ERR_LFD_FAIL
        *
        * @note Supported devices:
        *       RealScan-D, RealScan-G10(i), RealScan S60, RealScan SG10(i) (Support scheduled)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0;   // Assume valid device handle
        * RealScanSDK.RSLFDResult lfdResult = new RealScanSDK.RSLFDResult();
        *
        * int result = RealScanSDK.RS_GetLFDResult(deviceHandle, ref lfdResult);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("LFD Scores:");
        *     Console.WriteLine($"Thumb:  {lfdResult.thumbScore}");
        *     Console.WriteLine($"Index:  {lfdResult.indexScore}");
        *     Console.WriteLine($"Middle: {lfdResult.middleScore}");
        *     Console.WriteLine($"Ring:   {lfdResult.ringScore}");
        *     Console.WriteLine($"Little: {lfdResult.littleScore}");
        * }
        * else
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"Failed to get LFD result: {err} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetLFDResult")]
        public static extern int RS_GetLFDResult(int deviceHandle, ref RSLFDResult sLFDResult);
        /**
        * @brief [Unsupported] Retrieves the current manual contrast level setting.
        *
        * This API is not supported by current RealScan devices and will always return
        * RS_ERR_UNSUPPORTED_COMMAND.
        *
        * @param deviceHandle   [IN] Handle to the opened device.
        * @param contrastLevel  [OUT] Reference variable to receive the manual contrast level.
        *
        * @return Function success status as an error code.
        * @retval -114  RS_ERR_UNSUPPORTED_COMMAND   Function not supported.
        *
        * @note This API is currently unsupported in all devices and SDK versions.
        *
        * \par Example (C#)
        * @code
        * int contrast = 0;
        * int result = RealScanSDK.RS_GetManualContrast(deviceHandle, ref contrast);
        *
        * if (result == RealScanSDK.RS_ERR_UNSUPPORTED_COMMAND)
        * {
        *     Console.WriteLine("Manual contrast is not supported on this device.");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetManualContrast")]
        public static extern int RS_GetManualContrast(int deviceHandle, ref int contrastLevel);
        /**
        * @brief Sets options for roll fingerprint capture.
        *
        * This function configures the roll direction, roll duration, and roll quality
        * profile for capturing a rolled fingerprint image.
        *
        * @param deviceHandle [IN] Handle of the initialized device.
        *
        * @param rollDirection [IN] Direction of roll.\n
        * | Macro Name            | Value |
        * |-----------------------|-------|
        * | RS_ROLL_DIR_L2R       | 0     |
        * | RS_ROLL_DIR_R2L       | 1     |
        * | RS_ROLL_DIR_AUTO      | 2     |
        * | RS_ROLL_DIR_AUTO_M    | 3     |
        *
        * @param rollTime [IN] Roll duration in milliseconds. (Default: 5000 ms)
        *
        * @param rollProfile [IN] Select roll quality profile.\n
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | RS_ROLL_PROFILE_LOW     | 1     |
        * | RS_ROLL_PROFILE_NORMAL  | 2     |
        * | RS_ROLL_PROFILE_HIGH    | 3     |
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -13    RS_ERR_INVALID_PARAM
        * @retval -10    RS_ERR_SDK_UNINITIALIZED
        * @retval -102   RS_ERR_INVALID_HANDLE
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        *
        * \par Example (C#)
        * @code
        * int deviceHandle = 0; // Assume valid handle
        *
        * int result = RealScanSDK.RS_SetRollFingerOption(
        *                  deviceHandle,
        *                  RealScanSDK.RS_ROLL_DIR_AUTO,
        *                  5000,
        *                  RealScanSDK.RS_ROLL_PROFILE_NORMAL);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Roll option set successfully.");
        * }
        * else
        * {
        *     string errMsg = "";
        *     RealScanSDK.RS_GetErrString(result, ref errMsg);
        *     Console.WriteLine($"Failed to set roll option: {errMsg} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetRollFingerOption")]
        public static extern int RS_SetRollFingerOption(int deviceHandle, int rollDirection, int rollTime, int rollProfile);

        /**
        * @brief This API converts an image buffer to the ISO 19794-4:2011 format.
        *
        * @param deviceType [IN] Device Types.\n
        * | Macro Name                      | Value |
        * |---------------------------------|-------|
        * | RS_DEVICE_REALSCAN_10           | 0     |
        * | RS_DEVICE_REALSCAN_10F          | 1     |
        * | RS_DEVICE_REALSCAN_D            | 10    |
        * | RS_DEVICE_REALSCAN_DF           | 11    |
        * | RS_DEVICE_REALSCAN_F            | 20    |
        * | RS_DEVICE_REALSCAN_F_JP         | 21    |
        * | RS_DEVICE_REALSCAN_F_CN         | 22    |
        * | RS_DEVICE_REALSCAN_F_C          | 23    |
        * | RS_DEVICE_REALSCAN_F_C_IBIOS    | 24    |
        * | RS_DEVICE_REALSCAN_F_C_V02      | 25    |
        * | RS_DEVICE_REALSCAN_G10          | 30    |
        * | RS_DEVICE_REALSCAN_G10F         | 31    |
        * | RS_DEVICE_REALSCAN_G1           | 32    |
        * | RS_DEVICE_REALSCAN_G10I         | 33    |
        * | RS_DEVICE_REALSCAN_G10_IBIOS    | 34    |
        * | RS_DEVICE_REALSCAN_S60          | 40    |
        * | RS_DEVICE_REALSCAN_SG10         | 50    |
        * | RS_DEVICE_REALSCAN_SG10I        | 51    |
        * | RS_DEVICE_UNKNOWN               | 255   |
        *
        * @param imageBuffer [IN] Raw pixel data in memory.
        * @param imageBufferLen [IN] Pixel data length.
        * @param imageWidth [IN] Image width.
        * @param imageHeight [IN] Image height.
        *
        * @param fingerPosition [IN]\n
        * | Macro Name                           | Value |
        * |---------------------------------------|-------|
        * | RS_ISO19794_FGP_UNKNOWN               | 0     |
        * | RS_ISO19794_FGP_RIGHT_THUMB           | 1     |
        * | RS_ISO19794_FGP_RIGHT_INDEX           | 2     |
        * | RS_ISO19794_FGP_RIGHT_MIDDLE          | 3     |
        * | RS_ISO19794_FGP_RIGHT_RING            | 4     |
        * | RS_ISO19794_FGP_RIGHT_LITTLE          | 5     |
        * | RS_ISO19794_FGP_LEFT_THUMB            | 6     |
        * | RS_ISO19794_FGP_LEFT_INDEX            | 7     |
        * | RS_ISO19794_FGP_LEFT_MIDDLE           | 8     |
        * | RS_ISO19794_FGP_LEFT_RING             | 9     |
        * | RS_ISO19794_FGP_LEFT_LITTLE           | 10    |
        * | RS_ISO19794_FGP_PLAIN_RIGHT_THUMB     | 11    |
        * | RS_ISO19794_FGP_PLAIN_LEFT_THUMB      | 12    |
        * | RS_ISO19794_FGP_PLAIN_RIGHT_FOUR      | 13    |
        * | RS_ISO19794_FGP_PLAIN_LEFT_FOUR       | 14    |
        * | RS_ISO19794_FGP_PLAIN_TWO_THUMBS      | 15    |
        * | RS_ISO19794_FGP_EJI_OR_TIP            | 16    |
        *
        * @param nCompressionMode [IN]\n
        * | Macro Name                        | Value |
        * |-----------------------------------|-------|
        * | RSE_COMP_19794_NONE               | 0     |
        * | RSE_COMP_UNCOMPRESSED_BIT_PACKED  | 1     |
        * | RSE_COMP_19794_WSQ                | 2     |
        * | RSE_COMP_19794_JPEG               | 3     |
        * | RSE_COMP_19794_JPEG2000           | 4     |
        * | RSE_COMP_19794_PNG                | 5     |
        *
        * @param output [OUT] Output buffer (ISO formatted image).
        * @param outputLen [OUT] Output buffer length.
        *
        * @return Function success status as an error code.
        * @retval 0      RS_SUCCESS
        * @retval -4     RS_ERR_SMALL_BUFFER
        *
        * \par Example (C#)
        * @code
        * int deviceType = RealScanSDK.RS_DEVICE_REALSCAN_SG10;
        *
        * byte[] imageBuffer = ...;  // Raw grayscale image bytes
        * int bufferLen = imageBuffer.Length;
        * int width = 500;
        * int height = 500;
        *
        * int fingerPosition = RealScanSDK.RS_ISO19794_FGP_LEFT_INDEX;
        * int compressionMode = RealScanSDK.RSE_COMP_19794_WSQ;
        *
        * byte[] outputBuffer = new byte[200000]; // Allocate sufficient size
        * int outputLen = 0;
        *
        * int result = RealScanSDK.RS_GenerateIso19794_4_2011Ex(
        *                  deviceType,
        *                  imageBuffer,
        *                  bufferLen,
        *                  width,
        *                  height,
        *                  fingerPosition,
        *                  compressionMode,
        *                  outputBuffer,
        *                  ref outputLen);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"ISO buffer generated. Size = {outputLen} bytes");
        *     // You may save outputBuffer[..outputLen] to a file
        * }
        * else
        * {
        *     string errMsg = "";
        *     RealScanSDK.RS_GetErrString(result, ref errMsg);
        *     Console.WriteLine($"ISO generation failed: {errMsg} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GenerateIso19794_4_2011Ex")]
        public static extern int RS_GenerateIso19794_4_2011Ex(int deviceHandle, IntPtr pixelData, int pixelDataLen, int imageWidth, int imageHeight, int fingerPosition,
            int compressionMode, ref IntPtr output, ref int outputLen);
        /**
        * @brief Sets parameter value depending on type.
        * @param deviceHandle [IN] Handle to the opened device.
        * @param type  [IN] The type sets a value for supporting types of parameter. (i.e. preview mode). 
        * | Macro Name              | Value |
        * |-------------------------|-------|
        * | \b RS_VAR_PREVIEW_TYPE           | 1     |
        * | \b  RS_GVAR_WRN_LEVEL_TYPE      | 2    |
        * | \b  RS_GVAR_DIRTY_SENSITIVITY_TYPE      | 3    |
        * | \b  RS_GVAR_FP_QUALITY_MODE      | 4     |
        * | \b  RS_GVAR_CHECK_EXTERNAL_LIGHT      | 5     |
        * | \b  RS_HEATER_RUN_ON      | 6     |
        * @param variable  [IN] Pointer to parameter value of specified parameter type. 'variable' must point to adequate storage type matched to parameter type.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -13    \b RS_ERR_INVALID_PARAM
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)(Support is scheduled)
        * 
        * \par Example
        * @code
        * #include "RS_API.h"
        *
        * int main() {
        *     int deviceHandle = 0; 
        *     int previewType = RS_VAR_PREVIEW_TYPE_NONE;     
        *     int result = RS_SetParam(deviceHandle, RS_VAR_PREVIEW_TYPE, &previewType);
        *     if (result != RS_SUCCESS) {
        *         // handle error
        *     }
        *     return 0;
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_SetParam")]
        public static extern int RS_SetParam(int deviceHandle, int type, ref int variable);

        /**
        * @brief Gets parameter value depending on type.
        * @param deviceHandle [IN] Handle to the opened device.
        * @param type  [IN] The type sets a value for supporting types of parameter. (i.e. preview mode). 
        * @param variable  [OUT] Pointer to parameter value of specified parameter type.
        *                     'variable' must point to adequate storage type matched to parameter type.
        *
        * @return Function success status as an error code.
        * @retval 0      \b RS_SUCCESS
        * @retval -13    \b RS_ERR_INVALID_PARAM
        *
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC,
        *       RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example (C#)
        * @code
        * int deviceHandle = 0; // Assume valid device handle
        *
        * // The variable receiving the parameter must match the expected type.
        * int previewType = 0;
        *
        * int result = RealScanSDK.RS_GetParam(
        *                  deviceHandle,
        *                  RealScanSDK.RS_VAR_PREVIEW_TYPE,
        *                  ref previewType);
        *
        * if (result == RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine($"Preview type = {previewType}");
        * }
        * else
        * {
        *     string err = "";
        *     RealScanSDK.RS_GetErrString(result, ref err);
        *     Console.WriteLine($"RS_GetParam failed: {err} ({result})");
        * }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetParam")]
        public static extern int RS_GetParam(int deviceHandle, int type, ref int variable);

        /**
         * @brief It starts the function of automatically managing the device list by monitoring
         * the connection and disconnection of the device.
         * When a Callback function is registered using RS_RegisterHotPlugingCallback, the Callback
         * function is called every time the device is connected or disconnected.
         * 
         * @return Function success status as an error code. 
         * @retval 0        \b RS_SUCCESS
         * @retval -2       \b RS_ERR_NOT_SUPPORT          
         * @retval -11      \b RS_ERR_SDK_ALREADY_INITIALIZED
         * 
         * \par Example (C#)
         * @code
         * // Define callback delegate
         * RealScanSDK.HotPlugCallback hotPlugCallback = (deviceHandle, isConnected) =>
         * {
         *     Console.WriteLine($"Hotplug event: Handle={deviceHandle}, Connected={isConnected}");
         * };
         *
         * // Start monitoring
         * int result = RealScanSDK.RS_StartHotPlugging();
         * Console.WriteLine($"RS_StartHotPlugging : result = {result}");
         *
         * // Register callback
         * result = RealScanSDK.RS_RegisterHotPluggingCallback(hotPlugCallback);
         * Console.WriteLine($"RS_RegisterHotPluggingCallback : result = {result}");
         * @endcode
         */

        // Starts hot plugging monitoring
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_StartHotPlugging")]
        public static extern int RS_StartHotPlugging();
        /**
        * @brief Obtain the ID of Device from the Index of Device.
        * 
        * @param deviceIndex [IN] Index of the device. It should be less than the num Of Device.
        * @param deviceId [OUT] ID of the device.
        * 
        * @return Function success status as an error code. 
        * @retval 0        \b RS_SUCCESS     
        * @retval -101     \b RS_ERR_INVALID_DEVICE_INDEX
        * 
        * \par Example (C#)
        * @code
        * int numOfDevice = 0;
        * int result = RealScanSDK.RS_GetNumOfDevice(ref numOfDevice);
        *
        * if (result != RealScanSDK.RS_SUCCESS)
        * {
        *     Console.WriteLine("Failed to get number of devices.");
        *     return;
        * }
        *
        * for (int i = 0; i < numOfDevice; i++)
        * {
        *     int deviceId = -1;
        *     result = RealScanSDK.RS_GetDeviceId(i, ref deviceId);
        *
        *     if (result == RealScanSDK.RS_SUCCESS)
        *     {
        *         Console.WriteLine($"Device Index {i} → Device ID {deviceId}");
        *     }
        *     else
        *     {
        *         Console.WriteLine($"RS_GetDeviceId failed for index {i}, result = {result}");
        *     }
        * }
        * @endcode
        */
        // Gets the device ID by index
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetDeviceId")]
        public static extern int RS_GetDeviceId(int deviceIndex, ref int deviceId);

        /**
        * @brief Obtain information about connected devices. DeviceIndex is a value that changes every time another
        * device is connected or disconnected, but deviceId is a value that does not change even if another
        * device is disconnected.
        * 
        * @param deviceId [IN] ID of the device. 
        * @param deviceHandle [OUT] Handle of the device. Handle for controlling the device after InitDevice.
        * @param deviceType [OUT] Type of device.\n
        * | Macro Name | Value |
        * |-------------|-------|
        * | \b RS_DEVICE_REALSCAN_10   | 0 |
        * | \b RS_DEVICE_REALSCAN_10F     | 1 |
        * | \b RS_DEVICE_REALSCAN_D   | 10 |
        * | \b RS_DEVICE_REALSCAN_DF   | 11 |
        * | \b RS_DEVICE_REALSCAN_F | 20 |
        * | \b RS_DEVICE_REALSCAN_F_JP | 21 |
        * | \b RS_DEVICE_REALSCAN_F_CN | 22 |
        * | \b RS_DEVICE_REALSCAN_F_C | 23 |
        * | \b RS_DEVICE_REALSCAN_F_C_IBIOS | 24 |
        * | \b RS_DEVICE_REALSCAN_F_C_V02 | 25 |
        * | \b RS_DEVICE_REALSCAN_G10 | 30 |
        * | \b RS_DEVICE_REALSCAN_G10F | 31 |
        * | \b RS_DEVICE_REALSCAN_G1 | 32 |
        * | \b RS_DEVICE_REALSCAN_G10I | 33 |
        * | \b RS_DEVICE_REALSCAN_G10_IBIOS | 34 |
        * | \b RS_DEVICE_REALSCAN_S60 | 40 |
        * | \b RS_DEVICE_REALSCAN_SG10 | 50 |
        * | \b RS_DEVICE_REALSCAN_SG10I | 51 |
        * | \b RS_DEVICE_UNKNOWN | 255 |
        * @param deviceStatus [OUT] Status of device.\n
        * | Macro Name | Value |
        * |-------------|-------|
        * | \b  RS_DEVICE_STATUS_NONE   | 0 |
        * | \b  RS_DEVICE_STATUS_READY     | 1 |
        * | \b RS_DEVICE_STATUS_CONNECTED   | 2 |
        * 
        * @return Function success status as an error code. 
        * @retval 0        \b RS_SUCCESS     
        * @retval -10       \b RS_ERR_SDK_UNINITIALIZED
        * @retval -101     \b RS_ERR_INVALID_DEVICE_INDEX
        * 
        * \par Example
        * @code{.cs}
        * using System;
        *
        * class Program
        * {
        *     static void Main()
        *     {
        *         int numOfDevice = 0;
        *         int result = RealScanSDK.RS_GetNumOfDevice(ref numOfDevice);
        *
        *         int deviceId = -1;
        *         int deviceType = 0;
        *         int deviceStatus = 0;
        *         int deviceHandle = -1;
        *
        *         for (int i = 0; i < numOfDevice; i++)
        *         {
        *             result = RealScanSDK.RS_GetDeviceId(i, ref deviceId);
        *             result = RealScanSDK.RS_GetDeviceStatus(deviceId, ref deviceHandle, ref deviceType, ref deviceStatus);
        *         }
        *     }
        * }
        * @endcode
        */
        // Gets the current status of a device
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetDeviceStatus")]
        public static extern int RS_GetDeviceStatus(int deviceId,
                                                    ref int deviceHandle,
                                                    ref int deviceType,
                                                    ref int deviceStatus);
        /**
        * @brief Initializes a device and acquires its handle. The handle should be used for all subsequent function calls to the device.
        * 
        * @param deviceId [IN] ID of the device. 
        * @param deviceHandle [OUT] Handle of the device. Handle for controlling the device after InitDevice.
        * 
        * @return Function success status as an error code. 
        * @retval 0        \b RS_SUCCESS     
        * @retval -10       \b RS_ERR_SDK_UNINITIALIZED
        * @retval -11       \b RS_ERR_SDK_ALREADY_INITIALIZED
        * @retval -101     \b RS_ERR_INVALID_DEVICE_INDEX
        * @retval -107       \b RS_ERR_CANNOT_OPEN_DEVICE
        * 
        * \par Example
        * @code{.cs}
        * using System;
        *
        * class Program
        * {
        *     static void Main()
        *     {
        *         int numOfDevice = 0;
        *         int deviceHandle = -1;
        *         int result = RealScanSDK.RS_SUCCESS;
        *
        *         // Initialize SDK
        *         result = RealScanSDK.RS_InitSDK(IntPtr.Zero, 0, ref numOfDevice);
        *
        *         int deviceId = -1;
        *         int deviceType = 0;
        *         int deviceStatus = 0;
        *
        *         // Get device status for device index 0
        *         result = RealScanSDK.RS_GetDeviceStatus(0, ref deviceId, ref deviceHandle, ref deviceType, ref deviceStatus);
        *
        *         // Initialize device using deviceId
        *         result = RealScanSDK.RS_InitDeviceById(deviceId, ref deviceHandle);
        *     }
        * }
        * @endcode
        */
        // Initializes the device using its ID
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_InitDeviceById")]
        public static extern int RS_InitDeviceById(int deviceId, ref int deviceHandle);
        /**
        * @brief Retrieves the number of connected devices.
        *
        * @param numOfDevice [OUT] Pointer to an integer where the number of connected devices will be stored.
        *
        * @return Function success status as an error code.
        * @retval 0 \b RS_SUCCESS
        * @retval -102 \b RS_ERR_INVALID_HANDLE
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example
        * @code{.cs}
        using System;

        class Program
        {
            static void Main()
            {
                int num = 0;
                int result = RealScanSDK.RS_GetNumOfDevice(ref num);

                if (result == RealScanSDK.RS_SUCCESS)
                {
                    Console.WriteLine($"Number of connected devices: {num}");
                }
                else
                {
                    Console.WriteLine($"Failed to get number of devices. Error code: {result}");
                }
            }
        }
        * @endcode
        */
        // Gets the number of connected devices
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetNumOfDevice")]
        public static extern int RS_GetNumOfDevice(ref int numOfDevice);

        /**
        * @brief Retrieves the handle of an initialized device.     
        * @param deviceIndex [IN] Index of the device.
        * @param deviceHandle [OUT] Pointer to an integer where the device handle will be stored.
        * @return Function success status as an error code.
        * @retval 0 \b RS_SUCCESS
        * @retval -127 \b RS_ERR_DEVICE_NOT_INITIALIZED     
        * 
        * @note Supported devices: RealScan-D, RealScan-F, RealScan-FC, RealScan-G10(i), RealScan S60, RealScan SG10(i)
        * 
        * \par Example
        * @code{.cs}
        using System;

        class Program
        {
            static void Main()
            {
                // After SDK and device initialization
                int handle = 0;
                int result = RealScanSDK.RS_GetDeviceHandle(0, ref handle);

                if (result == RealScanSDK.RS_SUCCESS)
                {
                    Console.WriteLine($"Device handle: {handle}");
                }
                else
                {
                    Console.WriteLine($"Failed to get device handle. Error code: {result}");
                }
            }
        }
        * @endcode
        */
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_GetDeviceHandle")]
        public static extern int RS_GetDeviceHandle(int deviceIndex, ref int deviceHandle);
        /**
        * @brief Register a callback which will be called when device is plugging and unplugging.
        * 
        * @param captureCallback [IN] Function pointer of hot plugging callback. 
        * 
        * @return Function success status as an error code. 
        * @retval 0        \b RS_SUCCESS     
        * 
        * \par Example
        * @code{.cs}
            using System;
            using System.Runtime.InteropServices;

            class Program
            {
                // Callback delegate declaration
                public delegate void HotPlugCallback(int deviceId, int deviceStatus);

                // Keep delegate instance alive (prevent GC)
                private static HotPlugCallback _callbackInstance = HotPlugEvent;

                static void Main()
                {
                    int result = RealScanSDK.RS_StartHotPlugging();

                    // Register callback
                    result = RealScanSDK.RS_RegisterHotPluggingCallback(_callbackInstance);
                }

                // Callback method
                private static void HotPlugEvent(int deviceId, int deviceStatus)
                {
                    Console.WriteLine($"HotPlug Event → DeviceId: {deviceId}, Status: {deviceStatus}");
                }
            }
        * @endcode
        */
        // Registers a callback function for hot plugging events
        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, EntryPoint = "RS_RegisterHotPluggingCallback")]
        public static extern int RS_RegisterHotPluggingCallback(RSHotPluggingCallback callback);
        /**
           * @brief The RealScan S60 device (non-module) supports 7 status LED colors, which can be controlled using the 
       RS_SetModeLEDColor function, while the RealScan SG10(i) device supports 3 LED colors. 
           * @param deviceHandle [IN] Handle to the opened device.
           * @param LEDColorMask [IN] Color of the LED\n
           *
           *
           * \b RealScan S60\n 
           * | Macro Name                   | Value |
           * |------------------------------|-------|
           * | RS_S60_LED_STATUS_OFF        | 0     |
           * | RS_S60_LED_STATUS_RED        | 1     |
           * | RS_S60_LED_STATUS_GREEN      | 2     |
           * | RS_S60_LED_STATUS_YELLOW     | 3     |
           * | RS_S60_LED_STATUS_BLUE       | 4     |
           * | RS_S60_LED_STATUS_MAGENTA    | 5     |
           * | RS_S60_LED_STATUS_CYAN       | 6     |
           * | RS_S60_LED_STATUS_WHITE      | 7     |
           *
           * \b RealScan SG10(i)\n
           * | Macro Name                   | Value |
           * |------------------------------|-------|
           * | RS_SG10_LED_STATUS_OFF        | 0     |
           * | RS_SG10_LED_STATUS_RED        | 1     |
           * | RS_SG10_LED_STATUS_GREEN      | 2     |
           * | RS_SG10_LED_STATUS_BLUE       | 3     |
           * 
           * @return Function success status as an error code. 
           * @retval 0        \b RS_SUCCESS
           * @retval -13      \b RS_ERR_INVALID_PARAM
           * @retval -114     \b RS_ERR_UNSUPPORTED_COMMAND
           * 
           * @note RealScan S60, RealScan-D, RealScan SG10(i)
           * 
           * \par Example
           * @code{.cs}
       using System;

       class Program
       {
           static void Main()
           {
               int deviceHandle = 0;
               int LEDColorMask = RealScanSDK.RS_S60_LED_STATUS_MAGENTA;

               int result = RealScanSDK.RS_SetModeLEDColor(deviceHandle, LEDColorMask);
               if (result != RealScanSDK.RS_SUCCESS)
               {
                   // ...
                   return;
               }
           }
       }
       * @endcode
       */

        [DllImport("RS_SDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "RS_SetModeLEDColor")]
        public static extern int RS_SetModeLEDColor(int deviceHandle, int ledColorMask);

        /// @cond
        // FingerType

        // Device type
        public const int RS_DEVICE_REALSCAN_10 = 0x00;
        public const int RS_DEVICE_REALSCAN_10F = 0x01;

        public const int RS_DEVICE_REALSCAN_D = 0x10;
        public const int RS_DEVICE_REALSCAN_DF = 0x11;

        public const int RS_DEVICE_REALSCAN_F = 0x20;
        public const int RS_DEVICE_REALSCAN_F_JP = 0x21;
        public const int RS_DEVICE_REALSCAN_F_CN = 0x22;
        public const int RS_DEVICE_REALSCAN_F_C = 0x23;
        public const int RS_DEVICE_REALSCAN_F_C_IBIOS = 0x24;
        public const int RS_DEVICE_REALSCAN_F_C_V02 = 0x25;

        public const int RS_DEVICE_REALSCAN_G10 = 0x30;
        public const int RS_DEVICE_REALSCAN_G10F = 0x31;
        public const int RS_DEVICE_REALSCAN_G1 = 0x32;
        public const int RS_DEVICE_REALSCAN_G10I = 0x33;
        public const int RS_DEVICE_REALSCAN_G10_IBIOS = 0x34;

        public const int RS_DEVICE_REALSCAN_S60 = 0x40;

        public const int RS_DEVICE_REALSCAN_SG10 = 0x50;
        public const int RS_DEVICE_REALSCAN_SG10I = 0x51;
        public const int RS_DEVICE_UNKNOWN = 0xFF;

        // Initialization Mode
        //
        public const int RS_INIT_HIDE_INIDLG = 0x01;
        public const int RS_INIT_SHOW_INIDLG = 0x02;
        public const int RS_INIT_FULL = 0x04;

        //
        // Capture mode
        //
        public const int RS_CAPTURE_DISABLED = 0x00;
        public const int RS_CAPTURE_ROLL_FINGER = 0x01;
        public const int RS_CAPTURE_FLAT_SINGLE_FINGER = 0x02;
        public const int RS_CAPTURE_FLAT_TWO_FINGERS = 0x03;
        public const int RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS = 0x04;
        public const int RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS = 0x05;
        public const int RS_CAPTURE_FLAT_LEFT_PALM = 0x06;
        public const int RS_CAPTURE_FLAT_RIGHT_PALM = 0x07;
        public const int RS_CAPTURE_FLAT_SINGLE_FINGER_EX = 0x12;
        public const int RS_CAPTURE_FLAT_TWO_FINGERS_EX = 0x13;

        public const int RS_CAPTURE_ROLL_FINGER_EX = 0x30;

        public const int RS_CAPTURE_FLAT_LEFT_SIDE_PALM = 0x40;
        public const int RS_CAPTURE_FLAT_RIGHT_SIDE_PALM = 0x41;
        public const int RS_CAPTURE_FLAT_LEFT_WRITERS_PALM = 0x42;
        public const int RS_CAPTURE_FLAT_RIGHT_WRITERS_PALM = 0x43;
        public const int RS_CAPTURE_FLAT_LEFT_UPPER_PALM = 0x44;
        public const int RS_CAPTURE_FLAT_RIGHT_UPPER_PALM = 0x45;
        public const int RS_CAPTURE_FLAT_LEFT_LOWER_PALM = 0x46;
        public const int RS_CAPTURE_FLAT_RIGHT_LOWER_PALM = 0x47;

        public const int RS_CAPTURE_FLAT_MANUAL_INI = 0xfd;
        public const int RS_CAPTURE_FLAT_MANUAL = 0xfe;

        // Capture Direction
        public const int RS_CAPTURE_DIRECTION_DEFAULT = 0x00;
        public const int RS_CAPTURE_DIRECTION_LEFT = 0x01;
        public const int RS_CAPTURE_DIRECTION_RIGHT = 0x02;

        //
        // Options for capturing flat fingers
        //
        public const int RS_AUTO_SENSITIVITY_NORMAL = 0x00;
        public const int RS_AUTO_SENSITIVITY_HIGH = 0x01;
        public const int RS_AUTO_SENSITIVITY_HIGHER = 0x02;
        public const int RS_AUTO_SENSITIVITY_DISABLED = 0x03;

        //
        // Roll direction
        //
        public const int RS_ROLL_DIR_L2R = 0x00;
        public const int RS_ROLL_DIR_R2L = 0x01;
        public const int RS_ROLL_DIR_AUTO = 0x02;
        public const int RS_ROLL_DIR_AUTO_M = 0x03;

        //
        // Roll profile
        //
        public const int RS_ROLL_PROFILE_LOW = 0x01;
        public const int RS_ROLL_PROFILE_NORMAL = 0x02;
        public const int RS_ROLL_PROFILE_HIGH = 0x03;

        //
        // Text alignment
        //
        public const int RS_TEXT_ALIGN_LEFT = 0x00;
        public const int RS_TEXT_ALIGN_CENTER = 0x01;
        public const int RS_TEXT_ALIGN_RIGHT = 0x02;

        //
        // Beeper pattern
        //
        public const int RS_BEEP_PATTERN_NONE = 0;
        public const int RS_BEEP_PATTERN_1 = 1;	// 1 short beep
        public const int RS_BEEP_PATTERN_2 = 2;	// 2 short beeps

        //
        // Keypad code
        //
        public const int RS_REALSCAN10_NO_KEY = 0x00;
        public const int RS_REALSCAN10_PLAY_KEY = 0x20;
        public const int RS_REALSCAN10_STOP_KEY = 0x40;
        public int RS_REALSCAN10_ALL_KEYS = 0x7F;

        public const int RS_REALSCAND_NO_KEY = 0x00;
        public const int RS_REALSCAND_KEY_0 = 0x20;
        public const int RS_REALSCAND_ALL_KEYS = 0x7F;

        public const int RS_REALSCANF_NO_KEY = 0x00;
        public const int RS_REALSCANF_UP_KEY = 0x01;
        public const int RS_REALSCANF_DOWN_KEY = 0x02;
        public const int RS_REALSCANF_LEFT_KEY = 0x04;
        public const int RS_REALSCANF_RIGHT_KEY = 0x08;
        public const int RS_REALSCANF_PLAY_KEY = 0x20;
        public const int RS_REALSCANF_STOP_KEY = 0x40;
        public const int RS_REALSCANF_FOOTSWITCH = 0x80;
        public const int RS_REALSCANF_ALL_KEYS = 0xFF;
        public const int RS_REALSCANG10_NO_KEY = 0x00;
        public const int RS_REALSCANG10_PLAY_KEY = 0x20;
        public const int RS_REALSCANG10_STOP_KEY = 0x40;
        public const int RS_REALSCANG10_ALL_KEYS = 0x7F;
        //                              
        // Finger Index                 
        //                              
        public const int RS_FINGER_ALL = 0x00;
        public const int RS_FINGER_LEFT_LITTLE = 0x01;
        public const int RS_FINGER_LEFT_RING = 0x02;
        public const int RS_FINGER_LEFT_MIDDLE = 0x03;
        public const int RS_FINGER_LEFT_INDEX = 0x04;
        public const int RS_FINGER_LEFT_THUMB = 0x05;
        public const int RS_FINGER_RIGHT_THUMB = 0x06;
        public const int RS_FINGER_RIGHT_INDEX = 0x07;
        public const int RS_FINGER_RIGHT_MIDDLE = 0x08;
        public const int RS_FINGER_RIGHT_RING = 0x09;
        public const int RS_FINGER_RIGHT_LITTLE = 0x0A;
        public const int RS_FINGER_TWO_THUMB = 0x0B;
        public const int RS_FINGER_LEFT_FOUR = 0x0C;
        public const int RS_FINGER_RIGHT_FOUR = 0x0D;
        public const int RS_FINGER_TWO_LEFT1 = 0x0E;
        public const int RS_FINGER_TWO_LEFT2 = 0x0F;
        public const int RS_FINGER_TWO_RIGHT2 = 0x10;
        public const int RS_FINGER_TWO_RIGHT1 = 0x11;
        public const int RS_PALM_LEFT = 0x12;
        public const int RS_PALM_RIGHT = 0x13;

        //
        // Finger position for segmentation
        //
        public const int RS_FGP_UNKNOWN = 0;
        public const int RS_FGP_RIGHT_THUMB = 1;
        public const int RS_FGP_RIGHT_INDEX = 2;
        public const int RS_FGP_RIGHT_MIDDLE = 3;
        public const int RS_FGP_RIGHT_RING = 4;
        public const int RS_FGP_RIGHT_LITTLE = 5;
        public const int RS_FGP_LEFT_THUMB = 6;
        public const int RS_FGP_LEFT_INDEX = 7;
        public const int RS_FGP_LEFT_MIDDLE = 8;
        public const int RS_FGP_LEFT_RING = 9;
        public const int RS_FGP_LEFT_LITTLE = 10;
        public const int RS_FGP_PLAIN_RIGHT_THUMB = 11;
        public const int RS_FGP_PLAIN_LEFT_THUMB = 12;
        public const int RS_FGP_PLAIN_RIGHT_FOUR = 13;
        public const int RS_FGP_PLAIN_LEFT_FOUR = 14;
        public const int RS_FGP_PLAIN_TWO_THUMBS = 15;
        public const int RS_FGP_EJI_OR_TIP = 16;

        //                                  
        // LED for RealScan-10              
        //
        public const int RS_LED_MODE_ALL = 0x00;
        public const int RS_LED_MODE_LEFT_FINGER4 = 0x01;
        public const int RS_LED_MODE_RIGHT_FINGER4 = 0x02;
        public const int RS_LED_MODE_TWO_THUMB = 0x03;
        public const int RS_LED_MODE_ROLL = 0x04;
        public const int RS_LED_POWER = 0x05;

        //
        // LED Colors
        //
        public const int RS_LED_OFF = 0x00;
        public const int RS_LED_GREEN = 0x01;
        public const int RS_LED_RED = 0x02;
        public const int RS_LED_YELLOW = 0x03;

        //
        // LED Status for G1 
        //
        public const int RS_LED_STATUS_OFF = 0x00;
        public const int RS_LED_STATUS_ON = 0x01;
        public const int RS_LED_STATUS_BLINK = 0x02;

        //
        // Slap type
        //
        public const int RS_SLAP_LEFT_FOUR = 1;
        public const int RS_SLAP_RIGHT_FOUR = 2;
        public const int RS_SLAP_FOUR_FINGER = 3;
        public const int RS_SLAP_TWO_THUMB = 4;
        public const int RS_SLAP_TWO_FINGER = 5;
        public const int RS_SLAP_ONE_FINGER = 6;
        public const int RS_SLAP_ONE_FINGER_ROLL = 7;
        public const int RS_SLAP_MANUAL = 8;
        //                                  
        // Security level                   
        //                                  
        public const int RS_SECURITY_1_TO_100 = 0x01;
        public const int RS_SECURITY_1_TO_1000 = 0x02;
        public const int RS_SECURITY_1_TO_10000 = 0x03;
        public const int RS_SECURITY_1_TO_100000 = 0x04;
        public const int RS_SECURITY_1_TO_1000000 = 0x05;
        public const int RS_SECURITY_1_TO_10000000 = 0x06;
        public const int RS_SECURITY_1_TO_100000000 = 0x07;

        //                                  
        // Contrast enhancement
        //
        public const int RS_CONTRAST_ENHANCEMENT_DEFVALUE = 0;
        public const int RS_CONTRAST_ENHANCEMENT_MAXVALUE = 40;

        //
        // Self test type
        //
        public const int RS_SELFTEST_ILLUMINATION = 1;
        public const int RS_SELFTEST_DIRT = 2;

        //
        // LCD Display
        //
        public const int RS_LCD_WIDTH_MAX = 320;
        public const int RS_LCD_HEIGHT_MAX = 240;
        public const int RS_LCD_DATA_SIZE_MAX = 153600;

        //
        // LFD Level for G1 
        //
        public const int RS_LFD_OFF = 0;
        public const int RS_LFD_LEVEL_1 = 1;
        public const int RS_LFD_LEVEL_2 = 2;
        public const int RS_LFD_LEVEL_3 = 3;
        public const int RS_LFD_LEVEL_4 = 4;
        public const int RS_LFD_LEVEL_5 = 5;
        public const int RS_LFD_LEVEL_6 = 6;

        //
        // Preprocessing mode
        //
        public const int RS_HIGH_CONTRAST_PREPROCESS = 0x01;
        public const int RS_HIGH_VISIBILITY_PREPROCESS = 0x02;
        public const int RS_BALANCED_PREPROCESS = 0x03;
        public const int RS_ENHANCE_PREPROCESS = 0x05;
        public const int RS_ENHANCE_PREPROCESS_EX = 0x06;





        // Halo processing
        public const int RS_DISABLE_HALOPROCESS = 0x00;
        public const int RS_ENABLE_HALOPROCESS = 0x01;
        public const int RS_ENABLE_HALOPROCESSEX = 0x02;


        public const int RS_GVAR_FP_QUALITY_MODE = 4;
        /// @endcond

    }
}
