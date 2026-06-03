using System;
using System.Runtime.InteropServices;


namespace RealScan
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEPoint
    {
        int x;
        int y;
    };

    //
    // Slap information
    //
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSESlapInfo
    {
        public int fingerType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSEPoint[] fingerPosition;
        public int imageQuality;
        public int rotation;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] reserved;
    };


    /* characters in items */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEItem
    {
        public int numBytes;   /* Always contains the current byte size of the entire */
        /* item including any trailing US separator.           */
        public int numChars;   /* Number of characters currently in value, NOT  */
        /* including the NULL terminator.                */
        public int allocChars; /* Number of allocated characters for the value, */
        /* including the NULL terminator.                */
        public IntPtr value;  /* Must keep NULL terminated.              */
        public int usChar;
    };

    /* items in subfields */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSESubfield
    {
        public int numBytes;
        public int numItems;
        public int allocItems;
        public IntPtr items;
        public int rsChar;
    };

    /* subfields in fields */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEField
    {
        public IntPtr id;
        public UInt16 recordType;
        public UInt16 fieldInt;
        public int numBytes;
        public int numSubfields;
        public int allocSubfields;
        public IntPtr subfields;
        public int gsChar;
    };

    /* fields in records */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSERecord
    {
        public UInt16 type;
        public int totalBytes;
        public int numBytes;
        public int numFields;
        public int allocFields;
        public IntPtr fields;
        public int fsChar;
    };

    /* records in ANSI_NIST file */
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEFile
    {
        public UInt16 version;
        public int numBytes;
        public int numRecords;
        public int allocRecords;
        public IntPtr records;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType1Info
    {
        public int recordLength;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string versionNumber;
        public int numOfRecord;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 99)]
        public int[] recordType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 99)]
        public int[] recordIDC;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string typeOfTransaction;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string date;
        public int priority;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string destinationAgencyIdentifier;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string originatingAgencyIdentifier;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string transactionControlNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string transactionControlReferenceNumber;
        public double nativeScanningResolution;
        public double nominalTransmittingResolution;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string domainName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string greenwichMeanTime;
        public int characterSet;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType2CustomField
    {
        public int fieldID;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string fieldValue;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class RSEType2Info
    {
        public int recordLength;
        public int IDC;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemMaker;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemModel;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string serialNo;

        public int numCustomFields;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 500)]
        public RSEType2CustomField[] customFields;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType4Info
    {
        public int recordLength; // read only
        public int IDC;
        public int impressionType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public char[] fingerPosition;
        public int imageScanningResolution;
        public int horizontalLine;
        public int verticalLine;
        public int compressionAlgorithm;
        public int imageLen;
        public IntPtr imageData;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSESegmentPosition
    {
        public int fingerNumber;
        public int left;
        public int right;
        public int top;
        public int bottom;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEImageQuality
    {
        public int fingerNumber;
        public int score;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType10Info
    {
        public int recordLength;
        public int IDC;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string imageType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sourceAgency;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string captureDate;
        public int horizontalLine;
        public int verticalLine;
        public int scaleUnit;
        public int horizontalPixelScale;
        public int verticalPixelScale;
        public int compressionAlgorithm;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string colorSpace;
        public int shps;
        public int svps;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string subjectPose;
        public int poseOffsetAngle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 126)]
        public string comment;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string smtcode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string tattooClass;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 160)]
        public string tattooSubclass;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string tattooDesc;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemMaker;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string serialNo;
        public int imageDataLen;
        public IntPtr imageData;

        public int numCustomFields;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 500)]
        public RSEType2CustomField[] customFields;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType14Info
    {
        public int recordLength;
        public int IDC;
        public int impressionType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sourceAgency;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string captureDate;
        public int horizontalLine;
        public int verticalLine;
        public int scaleUnit;
        public int horizontalPixelScale;
        public int verticalPixelScale;
        public int compressionAlgorithm;
        public int bpp;
        public int numOfFingerPosition;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] fingerPosition;
        public int numOfSegment;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSESegmentPosition[] segPosition;
        public int numOfQuality;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public RSEImageQuality[] imgQuality;
        public int shps;
        public int svps;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 126)]
        public string comment;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemMaker;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string serialNo;
        public int imageLen;
        public IntPtr imageData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType15Info
    {
        public int recordLength;
        public int IDC;
        public int impressionType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sourceAgency;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string captureDate;
        public int horizontalLine;
        public int verticalLine;
        public int scaleUnit;
        public int horizontalPixelScale;
        public int verticalPixelScale;
        public int compressionAlgorithm;
        public int bpp;
        public int fgp;
        public int shps;
        public int svps;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 126)]
        public string comment;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemMaker;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string serialNo;
        public int imageLen;
        public IntPtr imageData;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RSEType17Info
    {
        public int recordLength;
        public int IDC;
        public int eyeLabel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string sourceAgency;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string captureDate;
        public int horizontalLine;
        public int verticalLine;
        public int scaleUnit;
        public int horizontalPixelScale;
        public int verticalPixelScale;
        public int compressionAlgorithm;
        public int bpp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string colorSpace;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemMaker;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string systemModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string serialNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string eyeColor;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 126)]
        public string comment;
        public int shps;
        public int svps;
        public int imageLen;
        public IntPtr imageData;
    }




    class RS_AN2K11
    {

           /* ====================================================================== */
        /*  FILE API (C# Stateful Wrapper)                                        */
        /* ====================================================================== */

        /**
        * @brief Creates a new ANSI/NIST file using the given Type-1 info.
        *
        * This C# wrapper corresponds to ::RSE_CreateFile but manages the
        * file as an internal state in the DLL instead of returning an
        * ::RSEFile pointer.
        *
        * @param[in] info
        * Pointer to an unmanaged @c RSEType1Info structure containing
        * Type-1 transaction information (TOT, DAI, ORI, TCN, TCR, etc.).
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -106  \b RSE_ERR_INVALID_NIST_VERSION
        *
        * @retval -400  \b RSE_ERR_CANNOT_CREATE_ANSI_FILE
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        * @retval -409  \b RSE_ERR_CANNOT_CREATE_FIELD
        * @retval -410  \b RSE_ERR_CANNOT_APPEND_FIELD
        * @retval -411  \b RSE_ERR_CANNOT_CREATE_SUBFIELD
        * @retval -412  \b RSE_ERR_CANNOT_APPEND_SUBFIELD
        * @retval -413  \b RSE_ERR_CANNOT_CREATE_ITEM
        * @retval -414  \b RSE_ERR_CANNOT_APPEND_ITEM
        *
        * @retval -408  \b RSE_ERR_CANNOT_UPDATE_RECORD
        *
        * @code
        * // Example C# usage:
        * IntPtr pInfo = Marshal.AllocHGlobal(Marshal.SizeOf<RSEType1Info>());
        * try
        * {
        *     // Fill Type-1 info in unmanaged memory
        *     RSE_SetType1Info("CAR", 1, "DEST", "ORIG", "TCN123", "TCR1", pInfo);
        *
        *     int rv = RSE_CreateFileCS(pInfo);
        *     if (rv == 0)
        *     {
        *         // Use the internal file (append records, write to disk, etc.)
        *         RSE_FreeFileCS();
        *     }
        * }
        * finally
        * {
        *     Marshal.FreeHGlobal(pInfo);
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateFileCS")]
        public static extern int RSE_CreateFileCS(IntPtr info);

        /**
        * @brief Reads an ANSI/NIST file from disk into the internal file context.
        *
        * This C# wrapper corresponds to ::RSE_ReadFile, but the loaded file
        * is kept in the DLL's internal state (no @c RSEFile* is returned).
        *
        * @param[in] filename
        * Path to an existing ANSI/NIST file.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -401  \b RSE_ERR_CANNOT_READ_ANSI_FILE
        *
        * @code
        * int rv = RSE_ReadFileCS("sample.nist");
        * if (rv == 0)
        * {
        *     Console.WriteLine("Loaded successfully.");
        *     RSE_FreeFileCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_ReadFileCS")]
        public static extern int RSE_ReadFileCS(string filename);

        /**
        * @brief Writes the current ANSI/NIST file (internal context) to disk.
        *
        * @param[in] filename
        * Output file path for the ANSI/NIST file.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -402  \b RSE_ERR_CANNOT_WRITE_ANSI_FILE
        *
        * @code
        * // Assume RSE_CreateFileCS or RSE_ReadFileCS was called previously.
        * int rv = RSE_WriteFileCS("output.nist");
        * if (rv != 0)
        * {
        *     Console.WriteLine("Write failed: " + rv);
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_WriteFileCS")]
        public static extern int RSE_WriteFileCS(string filename);

        /**
        * @brief Frees the current ANSI/NIST file in the internal context.
        *
        * This releases all resources associated with the current file
        * managed inside the DLL.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        *
        * @code
        * // After finishing with the internal file:
        * RSE_FreeFileCS();
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_FreeFileCS")]
        public static extern int RSE_FreeFileCS();


        /* ====================================================================== */
        /*  RECORD API (C# Stateful Wrapper)                                      */
        /* ====================================================================== */

        /**
        * @brief Appends the current record to the internal file.
        *
        * This corresponds to ::RSE_AppendRecord, but both the file and
        * record are managed as internal contexts in the DLL.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -406  \b RSE_ERR_CANNOT_INSERT_RECORD
        *
        * @code
        * // Create a Type-14 record in the internal record context:
        * RSEType14Info t14 = new RSEType14Info();
        * // (Fill t14 fields: dimensions, image data, etc.)
        * if (RSE_CreateType14RecordCS(ref t14) == 0)
        * {
        *     // Append to the current file:
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_AppendRecordCS")]
        public static extern int RSE_AppendRecordCS();

        /**
        * @brief Inserts the current record at a specific index in the file.
        *
        * @param[in] index
        * Zero-based index at which the current record will be inserted.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -406  \b RSE_ERR_CANNOT_INSERT_RECORD
        *
        * @code
        * // Insert current record at the beginning:
        * RSE_InsertRecordCS(0);
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_InsertRecordCS")]
        public static extern int RSE_InsertRecordCS(int index);

        /**
        * @brief Deletes a record at the specified index from the file.
        *
        * @param[in] index
        * Zero-based index of the record to delete.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -407  \b RSE_ERR_CANNOT_DELETE_RECORD
        *
        * @code
        * // Delete the second record in the file:
        * int rv = RSE_DeleteRecordCS(1);
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_DeleteRecordCS")]
        public static extern int RSE_DeleteRecordCS(int index);

        /**
        * @brief Reads a standalone record from file into the internal record context.
        *
        * This corresponds to ::RSE_ReadRecord. The record is stored inside the
        * DLL as the "current record".
        *
        * @param[in] recordFilename
        * Path to the file containing the record.
        * @param[in] recordType
        * Type ID of the record to read (e.g., 14 for Type-14).
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -404  \b RSE_ERR_CANNOT_READ_RECORD
        *
        * @code
        * int rv = RSE_ReadRecordCS("rec14.nist", 14);
        * if (rv == 0)
        * {
        *     // Access record-related APIs that use the internal record context...
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_ReadRecordCS")]
        public static extern int RSE_ReadRecordCS(string recordFilename, int recordType);

        /**
        * @brief Writes the current record to a file.
        *
        * @param[in] recordFilename
        * Output file path for the record.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -405  \b RSE_ERR_CANNOT_WRITE_RECORD
        *
        * @code
        * int rv = RSE_WriteRecordCS("type4.nist");
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_WriteRecordCS")]
        public static extern int RSE_WriteRecordCS(string recordFilename);

        /**
        * @brief Frees the current record in the internal record context.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        *
        * @code
        * // After finishing with the internal record:
        * RSE_FreeRecordCS();
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_FreeRecordCS")]
        public static extern int RSE_FreeRecordCS();

        /**
        * @brief Creates an empty record of the specified type in the internal context.
        *
        * This corresponds to ::RSE_CreateRecord, but the created record is stored
        * as the current record inside the DLL.
        *
        * @param[in] recordType
        * ANSI/NIST record type (e.g., 1, 4, 10, 14, etc.).
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        *
        * @code
        * // Create an empty Type-4 record in the internal context:
        * int rv = RSE_CreateRecordCS(4);
        * if (rv == 0)
        * {
        *     // Modify the record with field APIs, then append to file...
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateRecordCS")]
        public static extern int RSE_CreateRecordCS(int recordType);


        /* ====================================================================== */
        /*  TYPE-SPECIFIC RECORD CREATION (C# Stateful Wrapper)                   */
        /* ====================================================================== */

        /**
        * @brief Creates a Type-1 ANSI/NIST record using the provided Type-1 info.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Pointer to an unmanaged @c RSEType1Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -106  \b RSE_ERR_INVALID_NIST_VERSION
        *
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        * @retval -409  \b RSE_ERR_CANNOT_CREATE_FIELD
        * @retval -410  \b RSE_ERR_CANNOT_APPEND_FIELD
        *
        * @code
        * IntPtr pInfo = Marshal.AllocHGlobal(Marshal.SizeOf<RSEType1Info>());
        * try
        * {
        *     // Fill Type-1 info in unmanaged memory (or via RSE_SetType1Info)
        *     int rv = RSE_CreateType1RecordCS(pInfo);
        *     if (rv == 0)
        *     {
        *         // Append to file, etc.
        *         RSE_AppendRecordCS();
        *         RSE_FreeRecordCS();
        *     }
        * }
        * finally
        * {
        *     Marshal.FreeHGlobal(pInfo);
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType1RecordCS")]
        public static extern int RSE_CreateType1RecordCS(IntPtr info);

        /**
        * @brief Creates a Type-2 ANSI/NIST record using the provided Type-2 info.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Pointer to an unmanaged @c RSEType2Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0  \b RSE_SUCCESS on success, otherwise a negative error code
        *            defined in @c RSEErrorCode.
        *
        * @code
        * IntPtr pInfo = Marshal.AllocHGlobal(Marshal.SizeOf<RSEType2Info>());
        * try
        * {
        *     // Fill Type-2 info in unmanaged memory.
        *     int rv = RSE_CreateType2RecordCS(pInfo);
        *     if (rv == 0)
        *     {
        *         RSE_AppendRecordCS();
        *         RSE_FreeRecordCS();
        *     }
        * }
        * finally
        * {
        *     Marshal.FreeHGlobal(pInfo);
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType2RecordCS")]
        public static extern int RSE_CreateType2RecordCS(IntPtr info);

        /**
        * @brief Creates a Type-4 fingerprint record from the given info.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Reference to a managed @c RSEType4Info structure containing
        * image and metadata fields.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        * @retval -404  \b RSE_ERR_CANNOT_READ_RECORD
        *
        * @code
        * RSEType4Info info4 = new RSEType4Info();
        * info4.impressionType = 0;
        * info4.horizontalLine = 640;
        * info4.verticalLine   = 640;
        * // Fill image buffer fields as required...
        *
        * if (RSE_CreateType4RecordCS(ref info4) == 0)
        * {
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType4RecordCS")]
        public static extern int RSE_CreateType4RecordCS(ref RSEType4Info info);

        /**
        * @brief Creates a Type-10 facial image record.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Reference to a managed @c RSEType10Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -101  \b RSE_ERR_INVALID_PARAM
        * @retval -106  \b RSE_ERR_INVALID_NIST_VERSION
        *
        * @retval -105  \b RSE_ERR_MEM_FULL
        *
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        * @retval -409  \b RSE_ERR_CANNOT_CREATE_FIELD
        * @retval -410  \b RSE_ERR_CANNOT_APPEND_FIELD
        * @retval -411  \b RSE_ERR_CANNOT_CREATE_SUBFIELD
        * @retval -412  \b RSE_ERR_CANNOT_APPEND_SUBFIELD
        * @retval -413  \b RSE_ERR_CANNOT_CREATE_ITEM
        * @retval -414  \b RSE_ERR_CANNOT_APPEND_ITEM
        *
        * @retval -408  \b RSE_ERR_CANNOT_UPDATE_RECORD
        *
        * @code
        * RSEType10Info i10 = new RSEType10Info();
        * // Fill i10 fields (imageData, imageDataLen, dimensions, etc.)
        * if (RSE_CreateType10RecordCS(ref i10) == 0)
        * {
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType10RecordCS")]
        public static extern int RSE_CreateType10RecordCS(ref RSEType10Info info);

        /**
        * @brief Creates a Type-14 high-resolution fingerprint record.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Reference to a managed @c RSEType14Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        * @retval -106  \b RSE_ERR_INVALID_NIST_VERSION
        * @retval -105  \b RSE_ERR_MEM_FULL
        *
        * @retval -403  \b RSE_ERR_CANNOT_CREATE_RECORD
        * @retval -409  \b RSE_ERR_CANNOT_CREATE_FIELD
        * @retval -410  \b RSE_ERR_CANNOT_APPEND_FIELD
        * @retval -411  \b RSE_ERR_CANNOT_CREATE_SUBFIELD
        * @retval -412  \b RSE_ERR_CANNOT_APPEND_SUBFIELD
        * @retval -413  \b RSE_ERR_CANNOT_CREATE_ITEM
        * @retval -414  \b RSE_ERR_CANNOT_APPEND_ITEM
        *
        * @retval -408  \b RSE_ERR_CANNOT_UPDATE_RECORD
        *
        * @code
        * RSEType14Info info14 = new RSEType14Info();
        * // Fill info14 fields (dimensions, scales, compression, image data, etc.)
        * if (RSE_CreateType14RecordCS(ref info14) == 0)
        * {
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType14RecordCS")]
        public static extern int RSE_CreateType14RecordCS(ref RSEType14Info info);

        /**
        * @brief Creates a Type-15 palmprint image record.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Reference to a managed @c RSEType15Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0  \b RSE_SUCCESS on success, otherwise a negative error code
        *            defined in @c RSEErrorCode.
        *
        * @code
        * RSEType15Info info15 = new RSEType15Info();
        * // Fill palm image fields, dimensions, etc.
        * if (RSE_CreateType15RecordCS(ref info15) == 0)
        * {
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType15RecordCS")]
        public static extern int RSE_CreateType15RecordCS(ref RSEType15Info info);

        /**
        * @brief Creates a Type-17 iris / other biometric image record.
        *
        * The created record is stored as the current record in the internal
        * context of the DLL.
        *
        * @param[in] info
        * Reference to a managed @c RSEType17Info structure.
        *
        * @return Function success status as an error code.
        *
        * @retval 0  \b RSE_SUCCESS on success, otherwise a negative error code
        *            defined in @c RSEErrorCode.
        *
        * @code
        * RSEType17Info info17 = new RSEType17Info();
        * // Fill iris/other biometric image fields, dimensions, etc.
        * if (RSE_CreateType17RecordCS(ref info17) == 0)
        * {
        *     RSE_AppendRecordCS();
        *     RSE_FreeRecordCS();
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_CreateType17RecordCS")]
        public static extern int RSE_CreateType17RecordCS(ref RSEType17Info info);


        /* ====================================================================== */
        /*  TYPE-1 INFO & STANDARD (C# Wrapper)                                   */
        /* ====================================================================== */

        /**
        * @brief Fills a Type-1 header structure with transaction information.
        *
        * C# wrapper for ::RSE_SetType1Info. The @c info pointer must refer
        * to an unmanaged @c RSEType1Info buffer allocated by the caller.
        *
        * @param[in] TOT
        * Type of Transaction (TOT).
        * @param[in] priority
        * Transaction priority (0–9, values outside are ignored).
        * @param[in] DAI
        * Destination Agency Identifier.
        * @param[in] ORI
        * Originating Agency Identifier.
        * @param[in] TCN
        * Transaction Control Number.
        * @param[in] TCR
        * Transaction Control Reference.
        * @param[out] info
        * Pointer to an unmanaged @c RSEType1Info structure to be filled.
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -100  \b RSE_ERR_NO_DEVICE
        *
        * @code
        * IntPtr pInfo = Marshal.AllocHGlobal(Marshal.SizeOf<RSEType1Info>());
        * try
        * {
        *     int rv = RSE_SetType1Info("CAR", 1, "DEST", "ORG", "TC12345", "TCR001", pInfo);
        *     if (rv == 0)
        *     {
        *         // pInfo now contains a valid RSEType1Info structure.
        *         RSE_CreateFileCS(pInfo);
        *     }
        * }
        * finally
        * {
        *     Marshal.FreeHGlobal(pInfo);
        * }
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_SetType1Info")]
        public static extern int RSE_SetType1Info(string TOT,
                                                int priority,
                                                string DAI,
                                                string ORI,
                                                string TCN,
                                                string TCR,
                                                IntPtr info);

        /**
        * @brief Sets the ANSI/NIST version profile used by the DLL.
        *
        * @param[in] NISTVersionCode
        * Version index to apply.
        * - 0 = 2000
        * - 1 = 2007
        * - 2 = 2011
        * - 3 = 2013
        * - 4 = 2015
        *
        * @return Function success status as an error code.
        *
        * @retval 0     \b RSE_SUCCESS
        * @retval -106  \b RSE_ERR_INVALID_NIST_VERSION
        *
        * @code
        * // Set to ANSI/NIST 2011
        * int rv = RSE_SetStandard(2);
        * @endcode
        */
        [DllImport("RS_AN2K11.dll", CharSet = CharSet.Ansi, EntryPoint = "RSE_SetStandard")]
        public static extern int RSE_SetStandard(int NISTVersionCode);


        /**
        * @var RSE_SUCCESS
        * @brief API function executed successfully.
        */
        public const int RSE_SUCCESS = 0;

        /**
        * @var RSE_ERR_NO_DEVICE
        * @brief SDK is not initialized or no RealScan device context is available.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_ReadFileCS()
        * - RSE_WriteFileCS()
        * - RSE_FreeFileCS()
        * - RSE_AppendRecordCS()
        * - RSE_InsertRecordCS()
        * - RSE_DeleteRecordCS()
        * - RSE_ReadRecordCS()
        * - RSE_WriteRecordCS()
        * - RSE_FreeRecordCS()
        * - RSE_CreateRecordCS()
        * - RSE_CreateType1RecordCS()
        * - RSE_CreateType2RecordCS()
        * - RSE_CreateType4RecordCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        * - RSE_CreateType15RecordCS()
        * - RSE_CreateType17RecordCS()
        * - RSE_SetType1Info()
        * - RSE_SetStandard()
        */
        public const int RSE_ERR_NO_DEVICE = -100;

        /**
        * @var RSE_ERR_INVALID_PARAM
        * @brief One or more input parameters are invalid or null.
        *
        * @note Returned by:
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        * - RSE_SetType1Info()
        */
        public const int RSE_ERR_INVALID_PARAM = -101;

        /**
        * @var RSE_ERR_CANNOT_READ_FILE
        * @brief Failed to read a regular file from disk.
        */
        public const int RSE_ERR_CANNOT_READ_FILE = -102;

        /**
        * @var RSE_ERR_CANNOT_WRITE_FILE
        * @brief Failed to write a regular file to disk.
        */
        public const int RSE_ERR_CANNOT_WRITE_FILE = -103;

        /**
        * @var RSE_ERR_UNSUPPORTED_FILE
        * @brief Unsupported file format or invalid file signature detected.
        */
        public const int RSE_ERR_UNSUPPORTED_FILE = -104;

        /**
        * @var RSE_ERR_MEM_FULL
        * @brief Memory allocation failed due to insufficient system memory.
        *
        * @note Returned by:
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_MEM_FULL = -105;

        /**
        * @var RSE_ERR_INVALID_NIST_VERSION
        * @brief Unsupported ANSI/NIST standard version code.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType1RecordCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        * - RSE_SetStandard()
        */
        public const int RSE_ERR_INVALID_NIST_VERSION = -106; // ADDED BY VISHNU AGGARWAL 15.10.2025

        /**
        * @var RSE_ERR_CANNOT_SEGMENT
        * @brief Segmentation of slap image failed.
        */
        public const int RSE_ERR_CANNOT_SEGMENT = -200;

        /**
        * @var RSE_ERR_CANNOT_GET_QUALITY
        * @brief Quality score calculation failed.
        */
        public const int RSE_ERR_CANNOT_GET_QUALITY = -201;

        /**
        * @var RSE_ERR_NO_MATCH
        * @brief Fingerprint matching returned no result.
        */
        public const int RSE_ERR_NO_MATCH = -202;

        /**
        * @var RSE_ERR_CANNOT_EXTRACT_TEMPLATE
        * @brief Failed to extract fingerprint template.
        */
        public const int RSE_ERR_CANNOT_EXTRACT_TEMPLATE = -203;

        /**
        * @var RSE_ERR_SEGMENT_WRONG_HAND
        * @brief Segmentation detected incorrect hand (left/right mismatch).
        */
        public const int RSE_ERR_SEGMENT_WRONG_HAND = -204;

        /**
        * @var RSE_ERR_SEGMENT_FEWER_FINGER
        * @brief The number of segmented fingers is fewer than expected.
        */
        public const int RSE_ERR_SEGMENT_FEWER_FINGER = -205;

        /**
        * @var RSE_ERR_CANNOT_GET_CONTRAST
        * @brief Failed to compute or retrieve image contrast.
        */
        public const int RSE_ERR_CANNOT_GET_CONTRAST = -280;

        /**
        * @var RSE_ERR_CANNOT_COMPRESS_WSQ
        * @brief WSQ compression failed.
        */
        public const int RSE_ERR_CANNOT_COMPRESS_WSQ = -300;

        /**
        * @var RSE_ERR_CANNOT_DECOMPRESS_WSQ
        * @brief WSQ decompression failed.
        */
        public const int RSE_ERR_CANNOT_DECOMPRESS_WSQ = -301;

        /**
        * @var RSE_ERR_CANNOT_WRITE_WSQ_FILE
        * @brief Writing WSQ-encoded file failed.
        */
        public const int RSE_ERR_CANNOT_WRITE_WSQ_FILE = -302;

        /**
        * @var RSE_ERR_WSQ_SIZE_LIMIT
        * @brief WSQ output size exceeded the allowed limit.
        */
        public const int RSE_ERR_WSQ_SIZE_LIMIT = -303;

        /**
        * @var RSE_ERR_CANNOT_CREATE_ANSI_FILE
        * @brief Failed to allocate or build ANSI/NIST file structure.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        */
        public const int RSE_ERR_CANNOT_CREATE_ANSI_FILE = -400;

        /**
        * @var RSE_ERR_CANNOT_READ_ANSI_FILE
        * @brief Reading ANSI/NIST file from disk failed.
        *
        * @note Returned by:
        * - RSE_ReadFileCS()
        */
        public const int RSE_ERR_CANNOT_READ_ANSI_FILE = -401;

        /**
        * @var RSE_ERR_CANNOT_WRITE_ANSI_FILE
        * @brief Writing ANSI/NIST file to disk failed.
        *
        * @note Returned by:
        * - RSE_WriteFileCS()
        */
        public const int RSE_ERR_CANNOT_WRITE_ANSI_FILE = -402;

        /**
        * @var RSE_ERR_CANNOT_CREATE_RECORD
        * @brief Failed to create a new ANSI/NIST record.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateRecordCS()
        * - RSE_CreateType1RecordCS()
        * - RSE_CreateType2RecordCS()
        * - RSE_CreateType4RecordCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        * - RSE_CreateType15RecordCS()
        * - RSE_CreateType17RecordCS()
        */
        public const int RSE_ERR_CANNOT_CREATE_RECORD = -403;

        /**
        * @var RSE_ERR_CANNOT_READ_RECORD
        * @brief Failed to read a single record from file.
        *
        * @note Returned by:
        * - RSE_ReadRecordCS()
        * - RSE_CreateType4RecordCS()
        */
        public const int RSE_ERR_CANNOT_READ_RECORD = -404;

        /**
        * @var RSE_ERR_CANNOT_WRITE_RECORD
        * @brief Failed to write a single record to file.
        *
        * @note Returned by:
        * - RSE_WriteRecordCS()
        */
        public const int RSE_ERR_CANNOT_WRITE_RECORD = -405;

        /**
        * @var RSE_ERR_CANNOT_INSERT_RECORD
        * @brief Inserting a record into ANSI/NIST file failed.
        *
        * @note Returned by:
        * - RSE_InsertRecordCS()
        * - RSE_AppendRecordCS()
        */
        public const int RSE_ERR_CANNOT_INSERT_RECORD = -406;

        /**
        * @var RSE_ERR_CANNOT_DELETE_RECORD
        * @brief Deleting a record from ANSI/NIST file failed.
        *
        * @note Returned by:
        * - RSE_DeleteRecordCS()
        */
        public const int RSE_ERR_CANNOT_DELETE_RECORD = -407;

        /**
        * @var RSE_ERR_CANNOT_UPDATE_RECORD
        * @brief Updating an existing record failed (e.g., LEN update failure).
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_UPDATE_RECORD = -408;

        /**
        * @var RSE_ERR_CANNOT_CREATE_FIELD
        * @brief Failed to create a new field (e.g., Type-1, Type-10, Type-14 field).
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType1RecordCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_CREATE_FIELD = -409;

        /**
        * @var RSE_ERR_CANNOT_APPEND_FIELD
        * @brief Failed to append the field to a record structure.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType1RecordCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_APPEND_FIELD = -410;

        /**
        * @var RSE_ERR_CANNOT_CREATE_SUBFIELD
        * @brief Failed to allocate or create a subfield.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_CREATE_SUBFIELD = -411;

        /**
        * @var RSE_ERR_CANNOT_APPEND_SUBFIELD
        * @brief Failed to append a subfield to a field.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_APPEND_SUBFIELD = -412;

        /**
        * @var RSE_ERR_CANNOT_CREATE_ITEM
        * @brief Failed to create an item (e.g., value container).
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_CREATE_ITEM = -413;

        /**
        * @var RSE_ERR_CANNOT_APPEND_ITEM
        * @brief Failed to append an item to a subfield.
        *
        * @note Returned by:
        * - RSE_CreateFileCS()
        * - RSE_CreateType10RecordCS()
        * - RSE_CreateType14RecordCS()
        */
        public const int RSE_ERR_CANNOT_APPEND_ITEM = -414;


        /// @cond 
        public const int RSE_MAX_RECORD = 99;

        public const int RSE_MAX_TYPE2_CUSTOM_FIELDS = 500;
        // Image compression codes for ANSI/NIST
        public const int RSE_IMG_COMP_NONE = 0;
        public const int RSE_IMG_COMP_WSQ20 = 1;
        public const int RSE_IMG_COMP_JPEGB = 2;
        public const int RSE_IMG_COMP_JPEGL = 3;
        public const int RSE_IMG_COMP_JP2 = 4;
        public const int RSE_IMG_COMP_JP2L = 5;
        public const int RSE_IMG_COMP_PNG = 6;

        // Impression type
        public const int RSE_IMP_LIVESCAN_PLAIN = 0;
        public const int RSE_IMP_LIVESCAN_ROLLED = 1;
        public const int RSE_IMP_NON_LIVESCAN_PLAIN = 2;
        public const int RSE_IMP_NON_LIVESCAN_ROLLED = 3;
        public const int RSE_IMP_LATENT = 7;
        public const int RSE_IMP_SWIPE = 8;
        public const int RSE_IMP_LIVESCAN_CONTACTLESS = 9;
        public const int RSE_IMP_LIVESCAN_PALM = 10;                        // ADDED BY VISHNU AGGARWAL 08.12.2025
        public const int RSE_IMP_LIVESCAN_OPTICAL_CONTACT_PLAIN = 20;
        public const int RSE_IMP_LIVESCAN_OPTICAL_CONTACT_ROLLED = 21;
        public const int RSE_IMP_OTHER = 28;
        public const int RSE_IMP_UNKNOWN = 29;


        //
        // Finger position
        //
        public const int RSE_FGP_UNKNOWN = 0;
        public const int RSE_FGP_RIGHT_THUMB = 1;
        public const int RSE_FGP_RIGHT_INDEX = 2;
        public const int RSE_FGP_RIGHT_MIDDLE = 3;
        public const int RSE_FGP_RIGHT_RING = 4;
        public const int RSE_FGP_RIGHT_LITTLE = 5;
        public const int RSE_FGP_LEFT_THUMB = 6;
        public const int RSE_FGP_LEFT_INDEX = 7;
        public const int RSE_FGP_LEFT_MIDDLE = 8;
        public const int RSE_FGP_LEFT_RING = 9;
        public const int RSE_FGP_LEFT_LITTLE = 10;
        public const int RSE_FGP_PLAIN_RIGHT_THUMB = 11;
        public const int RSE_FGP_PLAIN_LEFT_THUMB = 12;
        public const int RSE_FGP_PLAIN_RIGHT_FOUR = 13;
        public const int RSE_FGP_PLAIN_LEFT_FOUR = 14;
        public const int RSE_FGP_PLAIN_TWO_THUMBS = 15;
        public const int RSE_FGP_EJI_OR_TIP = 16;

        // ADDED BY VISHNU AGGARWAL 08.12.2025
        public const int RSE_FGP_UNKNOWN_PALM = 20;
        public const int RSE_FGP_RIGHT_FULL_PALM = 21;
        public const int RSE_FGP_RIGHT_WRITERS_PALM = 22;
        public const int RSE_FGP_LEFT_FULL_PALM = 23;
        public const int RSE_FGP_LEFT_WRITERS_PALM = 24;
        public const int RSE_FGP_RIGHT_LOWER_PALM = 25;
        public const int RSE_FGP_RIGHT_UPPER_PALM = 26;
        public const int RSE_FGP_LEFT_LOWER_PALM = 27;
        public const int RSE_FGP_LEFT_UPPER_PALM = 28;
        public const int RSE_FGP_RIGHT_OTHER_PALM = 29;
        public const int RSE_FGP_LEFT_OTHER_PALM = 30;

        


        // Slap type
        public const int RSE_SLAP_LEFT_FOUR = 1;
        public const int RSE_SLAP_RIGHT_FOUR = 2;
        public const int RSE_SLAP_FOUR_FINGER = 3;
        public const int RSE_SLAP_TWO_THUMB = 4;
        public const int RSE_SLAP_TWO_FINGER = 5;
        public const int RSE_SLAP_ONE_FINGER = 6;
        public const int RSE_SLAP_ONE_FINGER_ROLL = 7; // since 2011.07.12

        // NIST VERSION	CODE FOR NIST VERSION				// ADDED BY VISHNU AGGARWAL 15.10.2025

        public const int RSE_VER_NIST_2000 = 0;
        public const int RSE_VER_NIST_2007 = 1;
        public const int RSE_VER_NIST_2011 = 2;
        public const int RSE_VER_NIST_2011_UPDATE_2013 = 3;
        public const int RSE_VER_NIST_2011_UPDATE_2015 = 4;     // DEFAULT VERSION 



        //
        // ISR SCANNING FOR TYPE 4							// ADDED BY VISHNU AGGARWAL 15.10.2025
        //
        public const int RSE_ISR_MINIMUM_SCANNING_RESOLUTION = 0;
        public const int RSE_ISR_NATIVE_SCANNING_RESOLUTION = 1;

        /// @endcond
    }
}
