using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace VisionCog
{
    public struct CogDATA
    {
        public int CarType;
        public string SelTool;
        public int LFBrightness;
        public int RFBrightness;
        public int LFMaxH;
        public int LFMaxV;
        public int RFMaxH;
        public int RFMaxV;
    }

    public struct GlobalFunction
    {
        public static bool Load_GlobalData()
        {
            string strTmpPath = Application.StartupPath + "\\Setup.dat";
            FileInfo f = new FileInfo(strTmpPath);

            if (f.Exists)
            {
                try
                {
                    Data.ServerIP = CodeINI.ReadIniFilePath(strTmpPath, "Server", "ServerIP");
                    Data.ServerPort = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Server", "ServerPort"));
                    Data.DBPath = CodeINI.ReadIniFilePath(strTmpPath, "Server", "DBPath");


                    //GlobalValue.VppPath = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "Path");

                    Data.CameraIP1 = CodeINI.ReadIniFilePath(strTmpPath, "Camera", "IP1");
                    Data.CameraIP2 = CodeINI.ReadIniFilePath(strTmpPath, "Camera", "IP2");
                    Data.LFCameraDEGREE = CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LFDEGREE");
                    Data.RFCameraDEGREE = CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RFDEGREE");
                    Data.FPXPARAM = CodeINI.ReadIniFilePath(strTmpPath, "Camera", "FPXPARAM");

                    Data.LF_Vision.PixelH = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LPixelH"));
                    Data.LF_Vision.PixelV = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LPixelV"));
                    Data.LF_Vision.WorkingDistance = Convert.ToDouble( CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LWD"));
                    //Data.LF_Vision.MaxH = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LMaxH"));
                    //Data.LF_Vision.MaxV = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "LMaxV"));


                    Data.RF_Vision.PixelH = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RPixelH"));
                    Data.RF_Vision.PixelV = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RPixelV"));
                    Data.RF_Vision.WorkingDistance = Convert.ToDouble(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RWD"));
                    //Data.RF_Vision.MaxH = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RMaxH"));
                    //Data.RF_Vision.MaxV = Convert.ToInt32(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "RMaxV"));


                    Data.CameraData.SensorH = Convert.ToDouble(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "SensorH"));
                    Data.CameraData.SensorV = Convert.ToDouble(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "SensorV"));
                    Data.CameraData.Lens = Convert.ToDouble(CodeINI.ReadIniFilePath(strTmpPath, "Camera", "Lens"));

                    Data.MaxSaveDay = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "Parameter", "MaxSavingDay"));
                    Data.RetryCnt = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "Parameter", "RetryCount"));

                    Data.LightPort = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "Port");
                    Data.DistancePort = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "DistancePort");

                    Data.COGDISONOFF = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "DisplayUse");
                    Data.ContinousGrap = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "CaptureCount"));
                    Data.ContinousTimeGrap = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "CaptureTime"));
                    Data.CaptureType = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "CaptureType"));
                    Data.MeasureParam = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "ValueType");

                    Data.UseToolblock = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "UseToolblock"));
                    
                    Data.VisionCnt = Convert.ToInt16(CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", "Count"));
                    Data.VisionSet = new CogDATA[Data.VisionCnt];
                    for (int i = 0; i < Data.VisionCnt; i++)
                    {
                        string[] tmp = CodeINI.ReadIniFilePath(strTmpPath, "VisionPro", i.ToString()).Split(',');
                        Data.VisionSet[i].CarType = Convert.ToInt32(tmp[0]);
                        Data.VisionSet[i].SelTool = tmp[1];
                        Data.VisionSet[i].LFBrightness = Convert.ToInt32(tmp[2]);
                        Data.VisionSet[i].RFBrightness = Convert.ToInt32(tmp[3]);
                        Data.VisionSet[i].LFMaxV = Convert.ToInt32(tmp[4]);
                        Data.VisionSet[i].LFMaxH = Convert.ToInt32(tmp[5]);
                        Data.VisionSet[i].RFMaxV = Convert.ToInt32(tmp[6]);
                        Data.VisionSet[i].RFMaxH = Convert.ToInt32(tmp[7]);
                    }

                    


                    Log.LogStr("Global", "Global Data Load Ok");
                }
                catch
                {
                    Log.LogStr("Global", "Global Data Load Fail");
                    return false;
                }
            }
            else
            {
                Log.LogStr("Global", "Global Data File Error");
                return false;
            }
            return true;
        }
    }

    struct _VSIONDATA
    {
        public bool Enable;
        public byte State;
        public double OriginLValue;
        public double OriginHValue;
        public double LValue;
        public double HValue;
        public double Scale;

        public int PixelH;
        public int PixelV;
        public int MaxH;
        public int MaxV;
        public double WorkingDistance;

        public int Get_H;
        public int Get_V;
    }

    struct _CAMERASET
    {
        public double Lens;
        public double SensorH;
        public double SensorV;
    }

    class Data
    {
        public static byte CarType;
        public static byte MeasurePart;
        //public static byte MeasureDirection;
        public static byte StartSignal;
        public static string barCode;

        public static _VSIONDATA RF_Vision;
        public static _VSIONDATA LF_Vision;

        public static _CAMERASET CameraData;

        public static string Barcode;
        public static string Locate;
        public static string ServerIP;
        public static int ServerPort;
        public static string LightPort;
        public static string DistancePort;

        public static string CameraIP1;
        public static string CameraIP2;
        public static string LFCameraDEGREE; //카메라 각도
        public static string RFCameraDEGREE;
        public static string FPXPARAM; //1mm per ? pixel


        public static int VisionCnt;
        public static CogDATA[] VisionSet = new CogDATA[1];
        public static int MaxSaveDay;
        public static int RetryCnt;
        public static int ContinousGrap;
        public static int ContinousTimeGrap;
        public static string COGDISONOFF;
        public static string MeasureParam;
        public static int CaptureType;

        public static int CurrentCartype;

        public static int UseToolblock;
       
        public static string DBPath;
    }

    struct _CALVSIONDATA
    {
        public bool Enable;
        public double X;
        public double Y;
        public double Rad;
    }
    class CAL_Data
    {
        public static _CALVSIONDATA RF_Vision;
        public static _CALVSIONDATA LF_Vision;
    }

}

