using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calibration {
    public class CLLTI {
        //Sample interface class with all needed functions for the example
        //Note not all functions of the LLT.dll are in this interface.
        //Please add the needed functions.

        #region "Constant declarations"

        public const string DRIVER_DLL_NAME = "LLT.dll";

        public const uint CONVERT_X = 0x00000800;
        public const uint CONVERT_Z = 0x00001000;

        #endregion

        #region "Return Values"
        //General return-values for all functions
        public const int GENERAL_FUNCTION_DEVICE_NAME_NOT_SUPPORTED = 4;
        public const int GENERAL_FUNCTION_PACKET_SIZE_CHANGED = 3;
        public const int GENERAL_FUNCTION_CONTAINER_MODE_HEIGHT_CHANGED = 2;
        public const int GENERAL_FUNCTION_OK = 1;
        public const int GENERAL_FUNCTION_NOT_AVAILABLE = 0;
        #endregion

        #region "Return Values"
        //Function defines for the Get/SetFeature function
        public const uint FEATURE_FUNCTION_SERIAL = 0xf0000410;
        public const uint FEATURE_FUNCTION_LASERPOWER = 0xf0f00824;
        public const uint INQUIRY_FUNCTION_LASERPOWER = 0xf0f00524;
        public const uint FEATURE_FUNCTION_MEASURINGFIELD = 0xf0f00880;
        public const uint INQUIRY_FUNCTION_MEASURINGFIELD = 0xf0f00580;
        public const uint FEATURE_FUNCTION_TRIGGER = 0xf0f00830;
        public const uint INQUIRY_FUNCTION_TRIGGER = 0xf0f00530;
        public const uint FEATURE_FUNCTION_SHUTTERTIME = 0xf0f0081c;
        public const uint INQUIRY_FUNCTION_SHUTTERTIME = 0xf0f0051c;
        public const uint FEATURE_FUNCTION_IDLETIME = 0xf0f00800;
        public const uint INQUIRY_FUNCTION_IDLETIME = 0xf0f00500;
        public const uint FEATURE_FUNCTION_PROCESSING_PROFILEDATA = 0xf0f00804;
        public const uint INQUIRY_FUNCTION_PROCESSING_PROFILEDATA = 0xf0f00504;
        public const uint FEATURE_FUNCTION_THRESHOLD = 0xf0f00810;
        public const uint INQUIRY_FUNCTION_THRESHOLD = 0xf0f00510;
        public const uint FEATURE_FUNCTION_MAINTENANCEFUNCTIONS = 0xf0f0088c;
        public const uint INQUIRY_FUNCTION_MAINTENANCEFUNCTIONS = 0xf0f0058c;
        public const uint FEATURE_FUNCTION_ANALOGFREQUENCY = 0xf0f00828;
        public const uint INQUIRY_FUNCTION_ANALOGFREQUENCY = 0xf0f00528;
        public const uint FEATURE_FUNCTION_ANALOGOUTPUTMODES = 0xf0f00820;
        public const uint INQUIRY_FUNCTION_ANALOGOUTPUTMODES = 0xf0f00520;
        public const uint FEATURE_FUNCTION_CMMTRIGGER = 0xf0f00888;
        public const uint INQUIRY_FUNCTION_CMMTRIGGER = 0xf0f00588;
        public const uint FEATURE_FUNCTION_REARRANGEMENT_PROFILE = 0xf0f0080c;
        public const uint INQUIRY_FUNCTION_REARRANGEMENT_PROFILE = 0xf0f0050c;
        public const uint FEATURE_FUNCTION_PROFILE_FILTER = 0xf0f00818;
        public const uint INQUIRY_FUNCTION_PROFILE_FILTER = 0xf0f00518;
        public const uint FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION = 0xf0f008c0;
        public const uint INQUIRY_FUNCTION_RS422_INTERFACE_FUNCTION = 0xf0f005c0;

        public const uint FEATURE_FUNCTION_SATURATION = 0xf0f00814;
        public const uint INQUIRY_FUNCTION_SATURATION = 0xf0f00514;
        public const uint FEATURE_FUNCTION_TEMPERATURE = 0xf0f0082c;
        public const uint INQUIRY_FUNCTION_TEMPERATURE = 0xf0f0052c;
        public const uint FEATURE_FUNCTION_CAPTURE_QUALITY = 0xf0f008c4;
        public const uint INQUIRY_FUNCTION_CAPTURE_QUALITY = 0xf0f005c4;
        public const uint FEATURE_FUNCTION_SHARPNESS = 0xf0f00808;
        public const uint INQUIRY_FUNCTION_SHARPNESS = 0xf0f00508;
        #endregion

        #region "Enums"

        //specifies the interface type for CreateLLTDevice and IsInterfaceType
        public enum TInterfaceType {
            INTF_TYPE_UNKNOWN = 0,
            INTF_TYPE_SERIAL = 1,
            INTF_TYPE_FIREWIRE = 2,
            INTF_TYPE_ETHERNET = 3
        }

        //specify the callback type
        //if you programming language don't support enums, you can use a signed int
        public enum TCallbackType {
            STD_CALL = 0,
            C_DECL = 1,
        }

        //specify the type of the scanner
        //if you programming language don't support enums, you can use a signed int
        public enum TScannerType {
            StandardType = -1,                    //can't decode scanCONTROL name use standard measurmentrange
            LLT25 = 0,                           //scanCONTROL28xx with 25mm measurmentrange
            LLT100 = 1,                           //scanCONTROL28xx with 100mm measurmentrange
            scanCONTROL28xx_25 = 0,              //scanCONTROL28xx with 25mm measurmentrange
            scanCONTROL28xx_100 = 1,              //scanCONTROL28xx with 100mm measurmentrange
            scanCONTROL28xx_10 = 2,              //scanCONTROL28xx with 10mm measurmentrange
            scanCONTROL28xx_xxx = 999,            //scanCONTROL28xx with no measurmentrange -> use standard measurmentrange
            scanCONTROL27xx_25 = 1000,           //scanCONTROL27xx with 25mm measurmentrange
            scanCONTROL27xx_100 = 1001,           //scanCONTROL27xx with 100mm measurmentrange
            scanCONTROL27xx_50 = 1002,           //scanCONTROL27xx with 50mm measurmentrange
            scanCONTROL2751_100 = 1021,           //scanCONTROL27xx with 100mm measurmentrange
            scanCONTROL2702_50 = 1032,           //scanCONTROL2702 with 50mm measurement range
            scanCONTROL27xx_xxx = 1999,           //scanCONTROL27xx with no measurmentrange -> use standard measurmentrange
        }

        //specify the profile configuration
        //if you programming language don't support enums, you can use a signed int
        public enum TProfileConfig {
            NONE = 0,
            PROFILE = 1,
            CONTAINER = 1,
            VIDEO_IMAGE = 1,
            PURE_PROFILE = 2,
            QUARTER_PROFILE = 3,
            CSV_PROFILE = 4,
            PARTIAL_PROFILE = 5,
        }

        //specify the type for the profile transfer
        //if you programming language don't support enums, you can use a signed int
        public enum TTransferProfileType {
            NORMAL_TRANSFER = 0,
            SHOT_TRANSFER = 1,
            NORMAL_CONTAINER_MODE = 2,
            SHOT_CONTAINER_MODE = 3,
        }
        #endregion

        #region "DLL references"

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_CreateLLTDevice",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern uint CreateLLTDevice(TInterfaceType iInterfaceType);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_DelDevice",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int DelDevice();

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_Connect",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int Connect(uint pLLT);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_Disconnect",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int Disconnect(uint pLLT);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetDeviceInterfaces",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetDeviceInterfaces(uint pLLT, uint[] pInterfaces, int nSize);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_SetDeviceInterface",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetDeviceInterface(uint pLLT, uint nInterface, int nAdditional);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetDeviceName",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetDeviceName(uint pLLT, byte[] pDevName, int nDevNameSize, byte[] pVendName, int nVenNameSize);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetLLTType",
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetLLTType(uint pLLT, ref TScannerType ScannerType);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_TranslateErrorValue",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int TranslateErrorValue(uint pLLT, int ErrorValue, byte[] pString, int nStringSize);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetResolutions",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetResolutions(uint pLLT, uint[] pValue, int nSize);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetFeature",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetFeature(uint pLLT, uint Function, ref uint pValue);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_SetFeature",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetFeature(uint pLLT, uint Function, uint Value);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_SetResolution",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetResolution(uint pLLT, uint Value);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_SetProfileConfig",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int SetProfileConfig(uint pLLT, TProfileConfig Value);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_TransferProfiles",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int TransferProfiles(uint pLLT, TTransferProfileType TransferProfileType, int nEnable);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_ConvertProfile2Values",
          CallingConvention = CallingConvention.StdCall)]
        internal static extern int ConvertProfile2Values(uint pLLT, byte[] pProfile, uint nResolution,
          TProfileConfig ProfileConfig, TScannerType ScannerType, uint nReflection, int nConvertToMM,
          ushort[] pWidth, ushort[] pMaximum, ushort[] pThreshold, double[] pX, double[] pZ,
          uint[] pM0, uint[] pM1);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_GetActualProfile",
           CallingConvention = CallingConvention.StdCall)]
        internal static extern int GetActualProfile(uint pLLT, byte[] pBuffer, int nBuffersize,
          TProfileConfig ProfileConfig, ref uint pLostProfiles);

        [DllImport(DRIVER_DLL_NAME, EntryPoint = "s_Timestamp2TimeAndCount",
           CallingConvention = CallingConvention.StdCall)]
        internal static extern int Timestamp2TimeAndCount(byte[] pBuffer, ref double dTimeShutterOpen, ref double dTimeShutterClose, ref uint uiProfileCount);
        #endregion

        // constructor
        public CLLTI() {
        }
    }
}

