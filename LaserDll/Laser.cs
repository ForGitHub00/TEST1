using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserDll
{
    public class Laser { 
    public const int MAX_INTERFACE_COUNT = 5;
    public const int MAX_RESOULUTIONS = 6;

    static public uint m_uiResolution = 0;
    static public uint m_hLLT = 0;
    static public CLLTI.TScannerType m_tscanCONTROLType;

    [STAThread]
    public static void main() {
        Init();
    }

    [STAThread]
    public static void Init() {
        uint[] auiFirewireInterfaces = new uint[MAX_INTERFACE_COUNT];
        uint[] auiResolutions = new uint[MAX_RESOULUTIONS];
        uint uiShutterTime = 11;
        uint uiIdleTime = 900;

        m_hLLT = 0;
        m_uiResolution = 0;


        m_hLLT = CLLTI.CreateLLTDevice(CLLTI.TInterfaceType.INTF_TYPE_ETHERNET);
        CLLTI.SetDeviceInterface(m_hLLT, 3232240897, 0);
        CLLTI.Connect(m_hLLT);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SERIAL, 214020058);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_LASERPOWER, 2);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MEASURINGFIELD, 0);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TRIGGER, 0);      //0
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, 1);  //100
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, 3999);  //900
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROCESSING_PROFILEDATA, 2639);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_THRESHOLD, 3200);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_MAINTENANCEFUNCTIONS, 2);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGFREQUENCY, 2);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_ANALOGOUTPUTMODES, 2);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CMMTRIGGER, 0);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_REARRANGEMENT_PROFILE, 2149096449);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_PROFILE_FILTER, 0);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_RS422_INTERFACE_FUNCTION, 4);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SATURATION, 4);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_TEMPERATURE, 3034);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_CAPTURE_QUALITY, 3034);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHARPNESS, 0);


        CLLTI.GetLLTType(m_hLLT, ref m_tscanCONTROLType);
        CLLTI.GetResolutions(m_hLLT, auiResolutions, auiResolutions.GetLength(0));

        m_uiResolution = auiResolutions[0];

        CLLTI.SetResolution(m_hLLT, m_uiResolution);
        CLLTI.SetProfileConfig(m_hLLT, CLLTI.TProfileConfig.PROFILE);

        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_SHUTTERTIME, uiShutterTime);
        CLLTI.SetFeature(m_hLLT, CLLTI.FEATURE_FUNCTION_IDLETIME, uiIdleTime);

    }

    public static void GetProfile(out double[] adValueX, out double[] adValueZ) {
        int iRetValue;
        uint uiLostProfiles = 0;
        adValueX = new double[m_uiResolution];
        adValueZ = new double[m_uiResolution];

        //Resize the profile buffer to the maximal profile size
        byte[] abyProfileBuffer = new byte[m_uiResolution * 4 + 16];
        byte[] abyTimestamp = new byte[16];

        CLLTI.TransferProfiles(m_hLLT, CLLTI.TTransferProfileType.NORMAL_TRANSFER, 1);

        //Sleep for a while to warm up the transfer
        System.Threading.Thread.Sleep(12);

        //Gets 1 profile in "polling-mode" and PURE_PROFILE configuration
        CLLTI.GetActualProfile(m_hLLT, abyProfileBuffer, abyProfileBuffer.GetLength(0), CLLTI.TProfileConfig.PURE_PROFILE, ref uiLostProfiles);

        iRetValue = CLLTI.ConvertProfile2Values(m_hLLT, abyProfileBuffer, m_uiResolution, CLLTI.TProfileConfig.PURE_PROFILE, m_tscanCONTROLType,
          0, 1, null, null, null, adValueX, adValueZ, null, null);

        //if (((iRetValue & CLLTI.CONVERT_X) == 0) || ((iRetValue & CLLTI.CONVERT_Z) == 0)) {
        //    Console.WriteLine("Error during Converting of profile data", iRetValue);
        //    return;
        //}
    }

    
    }
}
