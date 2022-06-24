using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beckhoff.Forms;
using TwinCAT.Ads;
namespace CAPC.Config
{
    public class plcParameterOp
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public static object GetPLCVar(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    return tcAdsPlcServer1.PlcClient.ReadSymbol(plcVariableName);
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return null;
                }
            }
            return 0;
        }

        public static bool SetPLCVar(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName, object plcValue)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    tcAdsPlcServer1.PlcClient.WriteSymbol(plcVariableName, plcValue);
                    return true;
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}  {plcVariableName}              {ex.StackTrace}");
                    return false;
                }
            }
            return false;
        }

        public static object ReadArray(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName, int nArraySize)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    return tcAdsPlcServer1.PlcClient.ReadAny(plcVariableName, typeof(bool[]), new int[] { nArraySize });
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public static Object ReadUIntArray(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName, int nArraySize)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    return tcAdsPlcServer1.PlcClient.ReadAny(plcVariableName, typeof(UInt16[]), new int[] { nArraySize });
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public static object ReadBoolArray(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName, int nArraySize)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    return tcAdsPlcServer1.PlcClient.ReadAny(plcVariableName, typeof(bool[]), new int[] { nArraySize });
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public static object ReadRealArray(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcVariableName, int nArraySize)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    return tcAdsPlcServer1.PlcClient.ReadAny(plcVariableName, typeof(double[]), new int[] { nArraySize });
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public static bool WriteRealArray(Beckhoff.Forms.TcAdsPlcServer tcAdsPlcServer1, string plcName, int nArraySize, object array)
        {
            if (tcAdsPlcServer1 != null && tcAdsPlcServer1.PlcIsRunning)
            {
                try
                {
                    tcAdsPlcServer1.PlcClient.WriteAny(plcName, array);
                    return true;
                }
                catch (Exception ex)
                {
                    OutputDebugString($"{ex.Message}              {ex.StackTrace}");
                    return false;
                }

            }
            else
                return false;
        }

    }
}
