using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.FGGigE;
using Cognex.VisionPro.ImagingDevice;
using Cognex.VisionPro.PMAlign;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Net.NetworkInformation;
using Cognex.VisionPro.ToolGroup;
using System.Data.SqlClient;

namespace VisionCog
{
    public partial class FrmMain : Form
    {
        SocketClient mClient;
        byte[] msg = new byte[38];
        Boolean m_bPortOpened = false;
        const string dirPath = ".\\saveImages";
        const string RESULTLOGDIR= ".\\Result";

        private SerialPort LightPort;

        string Lightmsg = "";
        int LFretryCnt = 0;
        int RFretryCnt = 0;

        int LFCogContinous = 0;
        int RFCogContinous = 0;
        
        List<double> LFXVal = new List<double>();
        List<double> LFYVal = new List<double>();
        List<long> LFTimeElapsed = new List<long>();

        List<double> RFXVal = new List<double>();
        List<double> RFYVal = new List<double>();
        List<long> RFTimeElapsed = new List<long>();

        System.Diagnostics.Stopwatch lfsw = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch rfsw = new System.Diagnostics.Stopwatch();

        System.Diagnostics.Stopwatch mLFTimeContinous = new System.Diagnostics.Stopwatch();
        System.Diagnostics.Stopwatch mRFTimeContinous = new System.Diagnostics.Stopwatch();


        ICogAcqFifo[] mAcqFifo = new ICogAcqFifo[2];
        CogAcqFifoTool mTool = new CogAcqFifoTool();
        StreamWriter mLFSW, mRFSW;

        List<string> LFTools = new List<string>();
        List<string> RFTools = new List<string>();

        List<string> LFOutputs = new List<string>();
        List<string> RFOutputs = new List<string>();

        public FrmMain()
        {
            InitializeComponent();

            Log.LogEvent += new LogEventHandler(addLog);
            msg[0] = 0xFF;
            msg[1] = 0xAA;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Data.CurrentCartype = 0;
            Initial();
            delCogImage("", "", null);
            Data.barCode = "";
        }

        private void globalSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stop();

            FrmGlobalSet dlg = new FrmGlobalSet();
            dlg.ShowDialog();

            Initial();
        }

        #region Server Socket
        private void Connect_Server()
        {
            mClient = new SocketClient(Data.ServerIP, Data.ServerPort, 8192, true);
            mClient.DataReceiveEventByte += clientVision_DataReceiveEventByte;
            mClient.Connect();
            tb_Conn.BackColor = Color.Gray;
            tb_Conn.Text = "Disabled";
        }

        private void clientVision_DataReceiveEventByte(byte[] ba)
        {
            try {
                if(ba.Length > 3 &&(ba[0]==0xFF && ba[1]==0xAA))
                {
                    UpdateReceivedData(ba);
                }

            }
            catch (Exception ex)
            {
                Log.LogStr("clientVision_DataReceiveEventByte", ex.ToString());
            }
        }

        #region // add log string 아래쪽 로그 리스트에 업데이트
        delegate void UpdateListCallback(String str11, String str2);
        public void addLog(string str1, string str2)
        {
            ListViewItem newitem;

            newitem = new ListViewItem(DateTime.Now.ToString("HH:mm:ss.fff"));
            newitem.SubItems.Add(str1);
            newitem.SubItems.Add(str2);
            try
            {
                if (this.lvLog.InvokeRequired)
                {
                    UpdateListCallback d = new UpdateListCallback(addLog);
                    this.Invoke(d, new object[] { str1, str2 });
                }
                else
                {
                    lvLog.Items.Insert(0, newitem);
                    if (lvLog.Items.Count > 0) lvLog.AutoScrollOffset = lvLog.Items[lvLog.Items.Count - 1].Position;
                    if (lvLog.Items.Count >= 50) lvLog.Items.RemoveAt(lvLog.Items.Count - 1);
                }
            }
            catch { }
        }

        #endregion

        delegate void UpdateReceivedDataCallback(byte[] bary);


        byte preStart = 0x00;
        public void UpdateReceivedData(byte[] bary)
        {
            ListViewItem newitem = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            newitem.SubItems.Add(BitConverter.ToString(bary));

            if (this.lvData.InvokeRequired)
            {
                UpdateReceivedDataCallback d = new UpdateReceivedDataCallback(UpdateReceivedData);
                this.Invoke(d, new object[] { bary });
            }
            else
            {
                lvData.Items.Insert(0, newitem);
                lvData.AutoScrollOffset = lvData.Items[lvData.Items.Count - 1].Position;
                if (lvData.Items.Count >= 30) lvData.Items.RemoveAt(lvData.Items.Count - 1);

                Data.CarType = bary[2];

                Data.MeasurePart = bary[3];

                Data.StartSignal = bary[4];

                byte len = bary[5];
                Data.barCode = Encoding.Default.GetString(bary, 6, len);
                txt_Barcode.Text = Data.barCode;

                if (tb_Cartype.Text != Data.CarType.ToString())
                {
                    ClearUi();
                    LoadPatMat();
                }

                tb_Cartype.Text = Data.CarType.ToString();

                tb_Part.Text = Convert.ToString(Data.MeasurePart, 2).PadLeft(8, '0').Substring(4);
                tb_Start.Text = Data.StartSignal.ToString();

                if (preStart != Data.StartSignal)
                {
                    if (Data.StartSignal == 0x01)
                    {
                        LFretryCnt = RFretryCnt = 0;
                        tb_StatusLF.Text = "0";
                        tb_StatusRF.Text = "0";

                        Delay(500);
                        serverGrap();
                    }
                    else
                    {
                        //uiClear();
                        Delay(500);
                        tb_StatusLF.Text = "0";
                        tb_StatusRF.Text = "0";
                        ClearData();
                    }
                }
                preStart = Data.StartSignal;

                if (mClient != null && mClient.CONNECTED)
                {
                    mClient.SendData(msg);
                }
            }
        }

  

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mClient != null)
            {
                if (mClient.CONNECTED)
                {
                    tb_Conn.BackColor = Color.Lime;
                    tb_Conn.Text = "Connected";
                }
                else
                {
                    tb_Conn.BackColor = Color.Red;
                    tb_Conn.Text = "Waiting";
                }
            }
            else
            {
                tb_Conn.BackColor = Color.Gray;
                tb_Conn.Text = "Disabled";
            }

            txt_Barcode.Text = Data.barCode;

            //smsg = "";
            //for (int i = 0; i < msg.Length; i++)
            //{
            //    smsg += String.Format("{0:X2}", msg[i]);
            //    if ((i % 10) == 9) smsg += " ";
            //}
            //lbMessage.Text = smsg;
        }
        #endregion

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
            Log.LogEvent -= new LogEventHandler(addLog);// remove 
        }

        private void Initial()
        {
            GlobalFunction.Load_GlobalData();

            Connect_Server();

            if(Data.COGDISONOFF == "OFF")
            {
                cogToolBlockEditV21.LocalDisplayVisible = false;
            }
            else
            {
                cogToolBlockEditV21.LocalDisplayVisible = true;
            }

   
            if (Data.VisionSet[Data.CurrentCartype].SelTool != null && Data.VisionSet[Data.CurrentCartype].SelTool != "")
            {
               
                
                CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();

                Data.LF_Vision.Enable = false;
                Data.RF_Vision.Enable = false;

                tb_StatusLF.BackColor = Color.Red;
                tb_StatusRF.BackColor = Color.Red;
                tb_StatusLF.Text = "2";
                tb_StatusRF.Text = "2";

                for (int i = 0; i < frameGrabbers.Count; i++)
                {
                    if (frameGrabbers[i].SerialNumber == Data.CameraIP1)
                    {
                        mAcqFifo[0] = frameGrabbers[i].CreateAcqFifo("Generic GigEVision (Mono)", CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);
                        mAcqFifo[0].OwnedGigEVisionTransportParams.LatencyLevel = 0;
                        mAcqFifo[0].OwnedGigEVisionTransportParams.PacketSize = 8164;

                        //int a, b;
                        //mAcqFifo[0].OwnedROIParams.GetROIXYWidthHeight(out a, out b, out Data.LF_Vision.Get_H, out Data.LF_Vision.Get_V);
                        //mAcqFifo[0].OwnedGigEVisionTransportParams.TransportTimeout = 500;
                        //mAcqFifo[0].TimeoutEnabled = true;
                        //mAcqFifo[0].Timeout = 1000;

                        Data.LF_Vision.Enable = true;
                        tb_StatusLF.BackColor = Color.Lime;
                        tb_StatusLF.Text = "0";
                    }
                    else if (frameGrabbers[i].SerialNumber == Data.CameraIP2)
                    {
                        mAcqFifo[1] = frameGrabbers[i].CreateAcqFifo("Generic GigEVision (Mono)", CogAcqFifoPixelFormatConstants.Format8Grey, 0, false);
                        mAcqFifo[1].OwnedGigEVisionTransportParams.LatencyLevel = 0;
                        mAcqFifo[1].OwnedGigEVisionTransportParams.PacketSize = 8164;

                        //int a, b;
                        //mAcqFifo[1].OwnedROIParams.GetROIXYWidthHeight(out a, out b, out Data.RF_Vision.Get_H, out Data.RF_Vision.Get_V);
                        //mAcqFifo[1].OwnedGigEVisionTransportParams.TransportTimeout = 500;
                        //mAcqFifo[1].TimeoutEnabled = true;
                        //mAcqFifo[1].Timeout = 1000;

                        Data.RF_Vision.Enable = true;
                        tb_StatusRF.BackColor = Color.Lime;
                        tb_StatusRF.Text = "0";
                    }
                }
            }
            else
            {
                MessageBox.Show("Select VisionPro Project!");
                Log.LogStr("Err", "Select VisionPro Project!");
            }

            timer1.Start();
            //tmr_Sequence.Interval = 10 * 60 * 1000; // data 삭제
            tmr_Sequence.Start();
            LightPort = new SerialPort();
            LightPortOpen();
            LoadPatMat();
        }

        private void Stop()
        {
            timer1.Stop();
            tmr_Sequence.Stop();
            if (mClient != null)
            {
                mClient.DataReceiveEventByte -= clientVision_DataReceiveEventByte;
                mClient.Close();
            }

            if (cogToolBlockEditV21.Subject.Tools.Count > 0)
            {
                if (Data.UseToolblock == 0)
                {
                    cogToolBlockEditV21.Subject.Tools["LFPatMat"].Ran -= new EventHandler(SubjectLF_Ran);
                    cogToolBlockEditV21.Subject.Tools["RFPatMat"].Ran -= new EventHandler(SubjectRF_Ran);
                }else
                {
                    cogToolBlockEditV21.Subject.Ran -= new EventHandler(Subject_Ran);
                }
            }
            for (int i = 0; i < mAcqFifo.Length; i++) {
                if (mAcqFifo[i] != null)
                {
                    mAcqFifo[i].Flush();
                    mAcqFifo[i] = null;
                }
            }

            Data.LF_Vision.Enable = false;
            Data.RF_Vision.Enable = false;
            
            tb_StatusLF.BackColor = Color.Red;
            tb_StatusRF.BackColor = Color.Red;
            tb_StatusLF.Text = "2";
            tb_StatusRF.Text = "2";
            
            CogFrameGrabbers frameGrabbers = new CogFrameGrabbers();
            foreach (ICogFrameGrabber fg in frameGrabbers)
            {
                fg.Disconnect(true);
            }

            if (LightPort.IsOpen) LightOnOff(false);
            ClosePort();
            ClosePort();
            ClearData();
            ClearUi();

            
        }

        private void LoadCogToolsName()
        {
            LFTools.Clear();
            RFTools.Clear();
            
            CogToolCollection at = cogToolBlockEditV21.Subject.Tools;

            string allTools1 = "";
            string allTools2 = "";

            for (int i = 0; i < at.Count; i++)
            {
                if (at[i].Name.IndexOf("LF") >= 0)
                {
                    LFTools.Add(at[i].Name);
                    allTools1 = allTools1 + at[i].Name + ",";
                }
                if (at[i].Name.IndexOf("RF") >= 0)
                {
                    RFTools.Add(at[i].Name);
                    allTools2 = allTools2 + at[i].Name + ",";
                }
            }
            Log.LogStr("LoadPatMat", "LF ==========" + allTools1);
            Log.LogStr("LoadPatMat", "RF ==========" + allTools2);

            CogToolBlockTerminalCollection Outputs = cogToolBlockEditV21.Subject.Outputs;
            for (int i = 0; i < Outputs.Count; i++)
            {
                if (Outputs[i].Name.IndexOf("LF") >= 0)
                {
                    LFOutputs.Add(Outputs[i].Name);
                }
                if (Outputs[i].Name.IndexOf("RF") >= 0)
                {
                    RFOutputs.Add(Outputs[i].Name);
                }
            }
        }

        #region Vision 
        private void LoadPatMat()
        {
            if (cogToolBlockEditV21.Subject.Tools.Count > 0)
            {
                if (Data.UseToolblock == 0)
                {
                    cogToolBlockEditV21.Subject.Tools["LFPatMat"].Ran -= new EventHandler(SubjectLF_Ran);
                    cogToolBlockEditV21.Subject.Tools["RFPatMat"].Ran -= new EventHandler(SubjectRF_Ran);
                }
                else
                {
                    cogToolBlockEditV21.Subject.Ran -= new EventHandler(Subject_Ran);
                }
            }
            try
            {
                Data.CurrentCartype = 0;
                for (int i = 0; i < Data.VisionCnt; i++)
                {
                    if(Data.VisionSet[i].CarType == Convert.ToInt16(Data.CarType))
                    {
                        Data.CurrentCartype = i;
                        break;
                    }
                }

                cogToolBlockEditV21.Subject = CogSerializer.LoadObjectFromFile(Data.VisionSet[Data.CurrentCartype].SelTool) as CogToolBlock;

                LoadCogToolsName();

                if (Data.UseToolblock == 0)
                {
                    cogToolBlockEditV21.Subject.Tools["LFPatMat"].Ran += new EventHandler(SubjectLF_Ran);
                    cogToolBlockEditV21.Subject.Tools["RFPatMat"].Ran += new EventHandler(SubjectRF_Ran);
                }
                else
                {
                    cogToolBlockEditV21.Subject.Ran += new EventHandler(Subject_Ran);
                }
                
                if (LightPort.IsOpen) LightSet(0, Data.VisionSet[Data.CurrentCartype].LFBrightness);
                Thread.Sleep(30);
                if (LightPort.IsOpen) LightSet(1, Data.VisionSet[Data.CurrentCartype].RFBrightness);

                ClearData();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                Log.LogStr("LoadPatMat", ex.ToString());
                tb_Cartype.Text = "0";
                Data.CarType = 0;
                return;
            }



        }

        private static DateTime Delay(int MS)

        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        void Subject_Ran(object sender, EventArgs e)
        {
            Int32 LFCnt = 0, RFCnt = 0;
            bool LFFinish = false, RFFinish = false;
            LFCnt = (Int32)cogToolBlockEditV21.Subject.Outputs["LF_Cnt"].Value;
            RFCnt = (Int32)cogToolBlockEditV21.Subject.Outputs["RF_Cnt"].Value;

            if ((ICogImage)cogToolBlockEditV21.Subject.Inputs["LFImage"].Value != null)
            {
                saveCogImage(Data.barCode + "_LF", (ICogImage)cogToolBlockEditV21.Subject.Inputs["LFImage"].Value);
            }
            if ((ICogImage)cogToolBlockEditV21.Subject.Inputs["RFImage"].Value != null)
            {
                saveCogImage(Data.barCode + "_RF", (ICogImage)cogToolBlockEditV21.Subject.Inputs["RFImage"].Value);
            }

            if (Data.CaptureType == 0)
            {
                if (Data.LF_Vision.Enable && LFCnt > 0)//|| LF_Score < 0.6)
                {
                    Data.LF_Vision.LValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value;
                    Data.LF_Vision.HValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value;
                    tb_DataLFL.Text = string.Format("{0:F2}", Data.LF_Vision.LValue);
                    tb_DataLFH.Text = string.Format("{0:F2}", Data.LF_Vision.HValue);

                    Log.LogStr("SubjectLF_Ran", "Onec capture " + Data.LF_Vision.LValue + " " + Data.LF_Vision.HValue);

                    Data.LF_Vision.State = 1;
                    Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                    Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                    Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                    tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                    tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                    tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);

                    //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                    //tb_DataLFX_mm.Text = ret[0];
                    //tb_DataLFY_mm.Text = ret[1];
                    double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                    double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                    int cnt = 0;
                    for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                    {
                     //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                        if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                        {
                            break;
                        }
                    }
                    Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                    if (tmpL > Data.VisionSet[cnt].LFMaxV)
                    {
                        tb_DataLFX_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                    }
                    if (tmpH > Data.VisionSet[cnt].LFMaxH)
                    {
                        tb_DataLFY_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                    }
                    tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                    LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                    lfsw.Reset();
                    LFFinish = true;
                }
                else
                {
                    Data.LF_Vision.State = 2;
                    tb_DataLFX_mm.Text = "0.0";
                    tb_DataLFY_mm.Text = "0.0";
                    tb_DataLFScale_change.Text = "0.0";
                }
                if (Data.RF_Vision.Enable && RFCnt > 0)//|| RF_Score < 0.6)
                {
                    Data.RF_Vision.LValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value;
                    Data.RF_Vision.HValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value;
                    tb_DataRFL.Text = string.Format("{0:F2}", Data.RF_Vision.LValue);
                    tb_DataRFH.Text = string.Format("{0:F2}", Data.RF_Vision.HValue);

                    Log.LogStr("SubjectRF_Ran", "Onec capture " + Data.RF_Vision.LValue + " " + Data.RF_Vision.HValue);

                    Data.RF_Vision.State = 1;
                    Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                    Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                    Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                    tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                    tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                    tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);

                    //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                    //tb_DataRFX_mm.Text = ret[0];
                    //tb_DataRFY_mm.Text = ret[1];

                    double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                    double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                    int cnt = 0;
                    for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                    {
                        if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                        {
                            break;
                        }
                    }
                    Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue))+ " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                    if (tmpL > Data.VisionSet[cnt].RFMaxV)
                    {
                        tb_DataRFX_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                    }
                    if (tmpH > Data.VisionSet[cnt].RFMaxH)
                    {
                        tb_DataRFY_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                    }
                    tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                    RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                    rfsw.Reset();
                    RFFinish = true;
                }
                else
                {
                    Data.RF_Vision.State = 2;
                    tb_DataRFX_mm.Text = "0.0";
                    tb_DataRFY_mm.Text = "0.0";
                    tb_DataRFScale_change.Text = "0.0";
                }

                if (LightPort.IsOpen)
                    LightOnOff(false);
            }
            else if (Data.CaptureType == 1)
            {
                LFCogContinous++;

                if (LFCogContinous < Data.ContinousGrap)
                {
                    if (Data.LF_Vision.Enable && LFCnt > 0)
                    {
                        LFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value);
                        LFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value);
                    }
                    if (Data.RF_Vision.Enable && RFCnt >= 0)
                    {
                        RFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value);
                        RFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value);
                    }

                    getImage();
                }
                else
                {
                    if (LFXVal.Count > 0 && LFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.LF_Vision.LValue = LFXVal.Average();
                            Data.LF_Vision.HValue = LFYVal.Average();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Average());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Average());
                            Log.LogStr("SubjectLF_Ran", "Count AVG === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.LF_Vision.LValue = LFXVal.Min();
                            Data.LF_Vision.HValue = LFYVal.Min();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Min());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Min());
                            Log.LogStr("SubjectLF_Ran", "Count MIN === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.LF_Vision.LValue = LFXVal.Max();
                            Data.LF_Vision.HValue = LFYVal.Max();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Max());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Max());
                            Log.LogStr("SubjectLF_Ran", "Count MAX === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }

                        Data.LF_Vision.State = 1;
                        Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                        Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                        Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                        tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                        tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                        tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);

                        if (Data.LF_Vision.State == 1 && Data.LF_Vision.OriginLValue != 0 && Data.LF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                            //tb_DataLFX_mm.Text = ret[0];
                            //tb_DataLFY_mm.Text = ret[1];
                            //tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            //tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].LFMaxV)
                            {
                                tb_DataLFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].LFMaxH)
                            {
                                tb_DataLFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            }
                            tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                            LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                            lfsw.Reset();
                        }
                        else
                        {
                            Data.LF_Vision.State = 2;
                            tb_DataLFX_mm.Text = "0.0";
                            tb_DataLFY_mm.Text = "0.0";
                            tb_DataLFScale_change.Text = "0.0";
                        }
                        LFFinish = true;
                    }
                    else
                    {
                        Data.LF_Vision.State = 2;
                        tb_DataLFX_mm.Text = "0.0";
                        tb_DataLFY_mm.Text = "0.0";
                        tb_DataLFScale_change.Text = "0.0";
                    }

                    if (RFXVal.Count > 0 && RFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.RF_Vision.LValue = RFXVal.Average();
                            Data.RF_Vision.HValue = RFYVal.Average();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Average());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Average());
                            Log.LogStr("SubjectRF_Ran", "Count AVG === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.RF_Vision.LValue = RFXVal.Min();
                            Data.RF_Vision.HValue = RFYVal.Min();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Min());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Min());
                            Log.LogStr("SubjectRF_Ran", "Count MIN === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.RF_Vision.LValue = RFXVal.Max();
                            Data.RF_Vision.HValue = RFYVal.Max();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Max());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Max());
                            Log.LogStr("SubjectRF_Ran", "Count MAX === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }

                        Data.RF_Vision.State = 1;
                        Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                        Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                        Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                        tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                        tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                        tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);

                        if (Data.RF_Vision.State == 1 && Data.RF_Vision.OriginLValue != 0 && Data.RF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                            //tb_DataRFX_mm.Text = ret[0];
                            //tb_DataRFY_mm.Text = ret[1];
                            //tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            //tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].RFMaxV)
                            {
                                tb_DataRFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].RFMaxH)
                            {
                                tb_DataRFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            }
                            tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                            RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                            rfsw.Reset();

                        }
                        else
                        {
                            Data.RF_Vision.State = 2;
                            tb_DataRFX_mm.Text = "0.0";
                            tb_DataRFY_mm.Text = "0.0";
                            tb_DataRFScale_change.Text = "0.0";
                        }
                        RFFinish = true;
                    }
                    else
                    {
                        Data.RF_Vision.State = 2;
                        tb_DataRFX_mm.Text = "0.0";
                        tb_DataRFY_mm.Text = "0.0";
                        tb_DataRFScale_change.Text = "0.0";
                    }

                    if (LightPort.IsOpen) LightOnOff(false);
                }
            }
            else if (Data.CaptureType == 2)
            {
                if (mLFTimeContinous.ElapsedMilliseconds < Data.ContinousTimeGrap)
                {
                    if (Data.LF_Vision.Enable && LFCnt > 0)
                    {
                        LFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value);
                        LFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value);
                    }
                    if (Data.RF_Vision.Enable && RFCnt > 0)//|| RF_Score < 0.6)
                    {
                        RFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value);
                        RFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value);
                    }
                    LFCogContinous++;
                    getImage();
                }
                else
                {
                    //Log.LogStr("SubjectLF_Ran", "Capture Count = " + LFCogContinous.ToString());
                    if (LFXVal.Count > 0 && LFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.LF_Vision.LValue = LFXVal.Average();
                            Data.LF_Vision.HValue = LFYVal.Average();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Average());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Average());
                            Log.LogStr("SubjectLF_Ran", "Time AVG === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);

                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.LF_Vision.LValue = LFXVal.Min();
                            Data.LF_Vision.HValue = LFYVal.Min();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Min());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Min());
                            Log.LogStr("SubjectLF_Ran", "Time MIN === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.LF_Vision.LValue = LFXVal.Max();
                            Data.LF_Vision.HValue = LFYVal.Max();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Max());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Max());
                            Log.LogStr("SubjectLF_Ran", "Time MAX === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }

                        Data.LF_Vision.State = 1;
                        Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                        Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                        Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                        tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                        tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                        tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);
                        //gc_convert_Front_LEFT_first
                        if (Data.LF_Vision.State == 1 && Data.LF_Vision.OriginLValue != 0 && Data.LF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                            //tb_DataLFX_mm.Text = ret[0];
                            //tb_DataLFY_mm.Text = ret[1];
                            //tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            //tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].LFMaxV)
                            {
                                tb_DataLFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].LFMaxH)
                            {
                                tb_DataLFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            }
                            tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                            LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                            lfsw.Reset();
                        }
                        else
                        {
                            Data.LF_Vision.State = 2;
                            tb_DataLFX_mm.Text = "0.0";
                            tb_DataLFScale_change.Text = "0.0";
                            tb_DataLFY_mm.Text = "0.0";
                            tb_DataLFScale_change.Text = "0.0";
                        }
                        LFFinish = true;
                    }
                    else
                    {
                        Data.LF_Vision.State = 2;
                        tb_DataLFX_mm.Text = "0.0";
                        tb_DataLFY_mm.Text = "0.0";
                        tb_DataLFScale_change.Text = "0.0";
                    }

                    if (RFXVal.Count > 0 && RFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.RF_Vision.LValue = RFXVal.Average();
                            Data.RF_Vision.HValue = RFYVal.Average();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Average());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Average());
                            Log.LogStr("SubjectRF_Ran", "Time AVG === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.RF_Vision.LValue = RFXVal.Min();
                            Data.RF_Vision.HValue = RFYVal.Min();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Min());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Min());
                            Log.LogStr("SubjectRF_Ran", "Time MIN === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.RF_Vision.LValue = RFXVal.Max();
                            Data.RF_Vision.HValue = RFYVal.Max();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Max());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Max());
                            Log.LogStr("SubjectRF_Ran", "Time MAX === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }

                        Data.RF_Vision.State = 1;
                        Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                        Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                        Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                        tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                        tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                        tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);

                        if (Data.RF_Vision.State == 1 && Data.RF_Vision.OriginLValue != 0 && Data.RF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                            //tb_DataRFX_mm.Text = ret[0];
                            //tb_DataRFY_mm.Text = ret[1];
                            //tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            //tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].RFMaxV)
                            {
                                tb_DataRFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].RFMaxH)
                            {
                                tb_DataRFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            }
                            tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                            RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                            rfsw.Reset();
                        }
                        else
                        {
                            Data.RF_Vision.State = 2;
                            tb_DataRFX_mm.Text = "0.0";
                            tb_DataRFY_mm.Text = "0.0";
                            tb_DataRFScale_change.Text = "0.0";
                        }
                        RFFinish = true;
                    }
                    else
                    {
                        Data.RF_Vision.State = 2;
                        tb_DataRFX_mm.Text = "0.0";
                        tb_DataRFY_mm.Text = "0.0";
                        tb_DataRFScale_change.Text = "0.0";
                    }
                    mLFTimeContinous.Reset();
                    mLFTimeContinous.Stop();
                    if (LightPort.IsOpen) LightOnOff(false);

                }
            }
            

            if (!Data.LF_Vision.Enable /*|| LFCnt < 1*/)//|| LF_Score < 0.6)
            {
                Data.LF_Vision.State = 2;
                Data.LF_Vision.OriginLValue = 0;
                Data.LF_Vision.OriginHValue = 0;
                Data.LF_Vision.LValue = 0;
                Data.LF_Vision.HValue = 0;
                Data.LF_Vision.Scale = 0;
                tb_StatusLF.Text = "2";
                tb_DataLFL.Text = "0";
                tb_DataLFH.Text = "0";
                tb_DataLFX_origin.Text = "0";
                tb_DataLFY_origin.Text = "0";
                tb_DataLFX_mm.Text = "0";
                tb_DataLFY_mm.Text = "0";
                tb_DataLFScale_change.Text = "0";
            }

            //Log.LogStr("Test", "=======================LF");
            if (LFFinish && RFFinish) SaveResult();
            else saveDatabase();
        }

        void SubjectLF_Ran(object sender, EventArgs e)
        {
            Int32 LFCnt = 0;
            LFCnt = (Int32)cogToolBlockEditV21.Subject.Outputs["LF_Cnt"].Value;

            if ((ICogImage)cogToolBlockEditV21.Subject.Inputs["LFImage"].Value != null)
            {
                saveCogImage(Data.barCode + "_LF", (ICogImage)cogToolBlockEditV21.Subject.Inputs["LFImage"].Value);
            }

            if (Data.CaptureType == 0)
            {
                if (Data.LF_Vision.Enable && LFCnt > 0)//|| LF_Score < 0.6)
                {
                    Data.LF_Vision.LValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value;
                    Data.LF_Vision.HValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value;
                    tb_DataLFL.Text = string.Format("{0:F2}", Data.LF_Vision.LValue);
                    tb_DataLFH.Text = string.Format("{0:F2}", Data.LF_Vision.HValue);

                    Log.LogStr("SubjectLF_Ran", "Onec capture " + Data.LF_Vision.LValue + " " + Data.LF_Vision.HValue);

                    Data.LF_Vision.State = 1;
                    Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                    Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                    Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                    tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                    tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                    tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);

                    //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                    //tb_DataLFX_mm.Text = ret[0];
                    //tb_DataLFY_mm.Text = ret[1];

                    //tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                    //tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                    double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                    double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                    int cnt = 0;
                    for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                    {
                        //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                        if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                        {
                            break;
                        }
                    }
                    Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                    if (tmpL > Data.VisionSet[cnt].LFMaxV)
                    {
                        tb_DataLFX_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                    }
                    if (tmpH > Data.VisionSet[cnt].LFMaxH)
                    {
                        tb_DataLFY_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                    }
                    tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                    LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                    lfsw.Reset();
                }
                else
                {
                    Data.LF_Vision.State = 2;
                    tb_DataLFX_mm.Text = "0.0";
                    tb_DataLFY_mm.Text = "0.0";
                    tb_DataLFScale_change.Text = "0.0";
                }

                if (LightPort.IsOpen) LightOnOff(0, false);
            }
            else if (Data.CaptureType == 1)
            {
                LFCogContinous++;

                if (LFCogContinous < Data.ContinousGrap)
                {
                    if (Data.LF_Vision.Enable && LFCnt > 0)//|| LF_Score < 0.6)
                    {
                        LFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value);
                        LFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value);
                    }
                    getLFImage(false);
                }
                else
                {
                    if (LFXVal.Count > 0 && LFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.LF_Vision.LValue = LFXVal.Average();
                            Data.LF_Vision.HValue = LFYVal.Average();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Average());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Average());
                            Log.LogStr("SubjectLF_Ran", "Count AVG === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.LF_Vision.LValue = LFXVal.Min();
                            Data.LF_Vision.HValue = LFYVal.Min();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Min());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Min());
                            Log.LogStr("SubjectLF_Ran", "Count MIN === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.LF_Vision.LValue = LFXVal.Max();
                            Data.LF_Vision.HValue = LFYVal.Max();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Max());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Max());
                            Log.LogStr("SubjectLF_Ran", "Count MAX === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }

                        Data.LF_Vision.State = 1;
                        Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                        Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                        Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                        tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                        tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                        tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);

                        if (Data.LF_Vision.State == 1 && Data.LF_Vision.OriginLValue != 0 && Data.LF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                            //tb_DataLFX_mm.Text = ret[0];
                            //tb_DataLFY_mm.Text = ret[1];

                            //tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            //tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].LFMaxV)
                            {
                                tb_DataLFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            }

                            if (tmpH > Data.VisionSet[cnt].LFMaxH)
                            {
                                tb_DataLFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            }
                            tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                            LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                            lfsw.Reset();

                        }
                        else
                        {
                            Data.LF_Vision.State = 2;
                            tb_DataLFX_mm.Text = "0.0";
                            tb_DataLFY_mm.Text = "0.0";
                            tb_DataLFScale_change.Text = "0.0";
                        }
                    }
                    else
                    {
                        Data.LF_Vision.State = 2;
                        tb_DataLFX_mm.Text = "0.0";
                        tb_DataLFY_mm.Text = "0.0";
                        tb_DataLFScale_change.Text = "0.0";

                    }
                    if (LightPort.IsOpen) LightOnOff(0, false);
                }
            }
            else if (Data.CaptureType == 2)
            {
                if (mLFTimeContinous.ElapsedMilliseconds < Data.ContinousTimeGrap)
                {
                    if (Data.LF_Vision.Enable && LFCnt > 0)//|| LF_Score < 0.6)
                    {
                        LFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value);
                        LFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value);
                    }
                    LFCogContinous++;
                    getLFImage(false);
                }
                else
                {
                    //Log.LogStr("SubjectLF_Ran", "Capture Count = " + LFCogContinous.ToString());
                    if (LFXVal.Count > 0 && LFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.LF_Vision.LValue = LFXVal.Average();
                            Data.LF_Vision.HValue = LFYVal.Average();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Average());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Average());
                            Log.LogStr("SubjectLF_Ran", "Time AVG === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);

                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.LF_Vision.LValue = LFXVal.Min();
                            Data.LF_Vision.HValue = LFYVal.Min();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Min());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Min());
                            Log.LogStr("SubjectLF_Ran", "Time MIN === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.LF_Vision.LValue = LFXVal.Max();
                            Data.LF_Vision.HValue = LFYVal.Max();
                            tb_DataLFL.Text = string.Format("{0:F2}", LFXVal.Max());
                            tb_DataLFH.Text = string.Format("{0:F2}", LFYVal.Max());
                            Log.LogStr("SubjectLF_Ran", "Time MAX === " + Data.LF_Vision.LValue + " ==== " + Data.LF_Vision.HValue);
                        }

                        Data.LF_Vision.State = 1;
                        Data.LF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginLPos"].Value;
                        Data.LF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["LF_OriginHPos"].Value;
                        Data.LF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["LF_Scale"].Value;
                        tb_StatusLF.Text = Data.LF_Vision.State.ToString();
                        tb_DataLFX_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginLValue);
                        tb_DataLFY_origin.Text = string.Format("{0:F2}", Data.LF_Vision.OriginHValue);
                        //gc_convert_Front_LEFT_first
                        if (Data.LF_Vision.State == 1 && Data.LF_Vision.OriginLValue != 0 && Data.LF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureLF(Data.LF_Vision.LValue, Data.LF_Vision.HValue, Data.LF_Vision.Scale);
                            //tb_DataLFX_mm.Text = ret[0];
                            //tb_DataLFY_mm.Text = ret[1];

                            //tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            //tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            double tmpL = Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            double tmpH = Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                //   Log.LogStr("LIMITER", "CAR TYPE " + Data.VisionSet[i].CarType.ToString() + " " + Data.CarType.ToString());
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "LF MAX H V" + Data.VisionSet[cnt].LFMaxV + " " + Data.VisionSet[cnt].LFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].LFMaxV)
                            {
                                tb_DataLFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFX_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].LFMaxH)
                            {
                                tb_DataLFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataLFY_mm.Text = string.Format("{0:F4}", Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue);
                            }
                            tb_DataLFScale_change.Text = string.Format("{0:F7}", Data.LF_Vision.Scale);
                            LFTimeElapsed.Add(lfsw.ElapsedMilliseconds);
                            lfsw.Reset();
                        }
                        else
                        {
                            Data.LF_Vision.State = 2;
                            tb_DataLFX_mm.Text = "0.0";
                            tb_DataLFY_mm.Text = "0.0";
                            tb_DataLFScale_change.Text = "0.0";
                        }
                    }
                    else
                    {
                        Data.LF_Vision.State = 2;
                        tb_DataLFX_mm.Text = "0.0";
                        tb_DataLFY_mm.Text = "0.0";
                        tb_DataLFScale_change.Text = "0.0";
                    }

                    mLFTimeContinous.Reset();
                    mLFTimeContinous.Stop();
                    if (LightPort.IsOpen) LightOnOff(0, false);

                }
            }
            if (!Data.LF_Vision.Enable /*|| LFCnt < 1*/)//|| LF_Score < 0.6)
            {
                Data.LF_Vision.State = 2;
                Data.LF_Vision.OriginLValue = 0;
                Data.LF_Vision.OriginHValue = 0;
                Data.LF_Vision.LValue = 0;
                Data.LF_Vision.HValue = 0;
                Data.LF_Vision.Scale = 0;
                tb_StatusLF.Text = "2";
                tb_DataLFL.Text = "0";
                tb_DataLFH.Text = "0";
                tb_DataLFX_origin.Text = "0";
                tb_DataLFY_origin.Text = "0";
                tb_DataLFX_mm.Text = "0";
                tb_DataLFY_mm.Text = "0";
                tb_DataLFScale_change.Text = "0";
            }

            //Log.LogStr("Test", "=======================LF");

        }

        void SubjectRF_Ran(object sender, EventArgs e)
        {
            Int32 RFCnt = 0;
            RFCnt = (Int32)cogToolBlockEditV21.Subject.Outputs["RF_Cnt"].Value;

            if ((ICogImage)cogToolBlockEditV21.Subject.Inputs["RFImage"].Value != null)
            {
                saveCogImage(Data.barCode+"_RF", (ICogImage)cogToolBlockEditV21.Subject.Inputs["RFImage"].Value);
            }

            if (Data.CaptureType == 0)
            {
                if (Data.RF_Vision.Enable && RFCnt > 0)//|| RF_Score < 0.6)
                {
                    Data.RF_Vision.LValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value;
                    Data.RF_Vision.HValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value;
                    tb_DataRFL.Text = string.Format("{0:F2}", Data.RF_Vision.LValue);
                    tb_DataRFH.Text = string.Format("{0:F2}", Data.RF_Vision.HValue);

                    Log.LogStr("SubjectRF_Ran", "Onec capture " + Data.RF_Vision.LValue + " " + Data.RF_Vision.HValue);

                    Data.RF_Vision.State = 1;
                    Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                    Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                    Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                    tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                    tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                    tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);

                    //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                    //tb_DataRFX_mm.Text = ret[0];
                    //tb_DataRFY_mm.Text = ret[1];
                    //tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                    //tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                    double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                    double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                    int cnt = 0;
                    for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                    {
                        if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                        {
                            break;
                        }
                    }
                    Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                    if (tmpL > Data.VisionSet[cnt].RFMaxV)
                    {
                        tb_DataRFX_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                    }
                    if (tmpH > Data.VisionSet[cnt].RFMaxH)
                    {
                        tb_DataRFY_mm.Text = "0";
                    }
                    else
                    {
                        tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                    }
                    tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                    RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                    rfsw.Reset();
                }
                else
                {
                    Data.RF_Vision.State = 2;
                    tb_DataRFX_mm.Text = "0.0";
                    tb_DataRFY_mm.Text = "0.0";
                    tb_DataRFScale_change.Text = "0.0";
                }

                if (LightPort.IsOpen) LightOnOff(0, false);
            }
            else if (Data.CaptureType == 1)
            {
                RFCogContinous++;
                if (RFCogContinous < Data.ContinousGrap)
                {
                    if (Data.RF_Vision.Enable && RFCnt > 0)//|| RF_Score < 0.6)
                    {
                        RFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value);
                        RFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value);
                    }
                    getRFImage(false);

                }
                else
                {
                    if (RFXVal.Count > 0 && RFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.RF_Vision.LValue = RFXVal.Average();
                            Data.RF_Vision.HValue = RFYVal.Average();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Average());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Average());
                            Log.LogStr("SubjectRF_Ran", "Count AVG === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.RF_Vision.LValue = RFXVal.Min();
                            Data.RF_Vision.HValue = RFYVal.Min();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Min());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Min());
                            Log.LogStr("SubjectRF_Ran", "Count MIN === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.RF_Vision.LValue = RFXVal.Max();
                            Data.RF_Vision.HValue = RFYVal.Max();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Max());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Max());
                            Log.LogStr("SubjectRF_Ran", "Count MAX === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }

                        Data.RF_Vision.State = 1;
                        Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                        Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                        Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                        tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                        tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                        tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);

                        if (Data.RF_Vision.State == 1 && Data.RF_Vision.OriginLValue != 0 && Data.RF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                            //tb_DataRFX_mm.Text = ret[0];
                            //tb_DataRFY_mm.Text = ret[1];
                            //tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            //tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].RFMaxV)
                            {
                                tb_DataRFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].RFMaxH)
                            {
                                tb_DataRFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            }
                            tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                            RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                            rfsw.Reset();
                        }
                        else
                        {
                            Data.RF_Vision.State = 2;
                            tb_DataRFX_mm.Text = "0.0";
                            tb_DataRFY_mm.Text = "0.0";
                            tb_DataRFScale_change.Text = "0.0";
                        }
                    }
                    else
                    {
                        Data.RF_Vision.State = 2;
                        tb_DataRFX_mm.Text = "0.0";
                        tb_DataRFY_mm.Text = "0.0";
                        tb_DataRFScale_change.Text = "0.0";
                    }
                    if (LightPort.IsOpen) LightOnOff(0, false);
                }
            }
            else if (Data.CaptureType == 2)
            {
                if (mRFTimeContinous.ElapsedMilliseconds < Data.ContinousTimeGrap)
                {
                    if (Data.RF_Vision.Enable && RFCnt > 0)//|| RF_Score < 0.6)
                    {
                        RFXVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value);
                        RFYVal.Add((double)cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value);
                    }
                    RFCogContinous++;
                    getRFImage(false);
                }
                else
                {
                    if (RFXVal.Count > 0 && RFYVal.Count > 0)
                    {
                        if (Data.MeasureParam == "AVG")
                        {
                            Data.RF_Vision.LValue = RFXVal.Average();
                            Data.RF_Vision.HValue = RFYVal.Average();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Average());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Average());
                            Log.LogStr("SubjectRF_Ran", "Time AVG === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MIN")
                        {
                            Data.RF_Vision.LValue = RFXVal.Min();
                            Data.RF_Vision.HValue = RFYVal.Min();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Min());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Min());
                            Log.LogStr("SubjectRF_Ran", "Time MIN === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }
                        if (Data.MeasureParam == "MAX")
                        {
                            Data.RF_Vision.LValue = RFXVal.Max();
                            Data.RF_Vision.HValue = RFYVal.Max();
                            tb_DataRFL.Text = string.Format("{0:F2}", RFXVal.Max());
                            tb_DataRFH.Text = string.Format("{0:F2}", RFYVal.Max());
                            Log.LogStr("SubjectRF_Ran", "Time MAX === " + Data.RF_Vision.LValue + " ==== " + Data.RF_Vision.HValue);
                        }

                        Data.RF_Vision.State = 1;
                        Data.RF_Vision.OriginLValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginLPos"].Value;
                        Data.RF_Vision.OriginHValue = (double)cogToolBlockEditV21.Subject.Outputs["RF_OriginHPos"].Value;
                        Data.RF_Vision.Scale = (double)cogToolBlockEditV21.Subject.Outputs["RF_Scale"].Value;
                        tb_StatusRF.Text = Data.RF_Vision.State.ToString();
                        tb_DataRFX_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginLValue);
                        tb_DataRFY_origin.Text = string.Format("{0:F2}", Data.RF_Vision.OriginHValue);
                        //gc_convert_Front_LEFT_first
                        if (Data.RF_Vision.State == 1 && Data.RF_Vision.OriginLValue != 0 && Data.RF_Vision.OriginHValue != 0)
                        {
                            //string[] ret = measureRF(Data.RF_Vision.LValue, Data.RF_Vision.HValue, Data.RF_Vision.Scale);
                            //tb_DataRFX_mm.Text = ret[0];
                            //tb_DataRFY_mm.Text = ret[1];
                            //tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            //tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            double tmpL = Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            double tmpH = Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            int cnt = 0;
                            for (cnt = 0; cnt < Data.VisionCnt; cnt++)
                            {
                                if (Data.VisionSet[cnt].CarType.ToString() == Data.CarType.ToString())
                                {
                                    break;
                                }
                            }
                            Log.LogStr("LIMITER", "RF MAX H V" + Data.VisionSet[cnt].RFMaxV + " " + Data.VisionSet[cnt].RFMaxH + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue)) + " " + String.Format("{0:F4}", Math.Abs(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue)));
                            if (tmpL > Data.VisionSet[cnt].RFMaxV)
                            {
                                tb_DataRFX_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFX_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue);
                            }
                            if (tmpH > Data.VisionSet[cnt].RFMaxH)
                            {
                                tb_DataRFY_mm.Text = "0";
                            }
                            else
                            {
                                tb_DataRFY_mm.Text = string.Format("{0:F4}", Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue);
                            }
                            tb_DataRFScale_change.Text = string.Format("{0:F7}", Data.RF_Vision.Scale);
                            RFTimeElapsed.Add(rfsw.ElapsedMilliseconds);
                            rfsw.Reset();
                        }
                        else
                        {
                            Data.RF_Vision.State = 2;
                            tb_DataRFX_mm.Text = "0.0";
                            tb_DataRFY_mm.Text = "0.0";
                            tb_DataRFScale_change.Text = "0.0";
                        }
                    }
                    else
                    {
                        Data.RF_Vision.State = 2;
                        tb_DataRFX_mm.Text = "0.0";
                        tb_DataRFY_mm.Text = "0.0";
                        tb_DataRFScale_change.Text = "0.0";
                    }
                    mRFTimeContinous.Reset();
                    mRFTimeContinous.Stop();
                    if (LightPort.IsOpen) LightOnOff(0, false);

                }
            }
            if (!Data.RF_Vision.Enable/* || RFCnt < 1*/)//|| RF_Score < 0.6)
            {
                Data.RF_Vision.State = 2;
                Data.RF_Vision.OriginLValue = 0;
                Data.RF_Vision.OriginHValue = 0;
                Data.RF_Vision.LValue = 0;
                Data.RF_Vision.HValue = 0;
                Data.RF_Vision.Scale = 0;
                tb_StatusRF.Text = "2";
                tb_DataRFL.Text = "0";
                tb_DataRFX_origin.Text = "0";
                tb_DataRFY_origin.Text = "0";
                tb_DataRFH.Text = "0";
                tb_DataRFX_mm.Text = "0";
                tb_DataRFY_mm.Text = "0";
                tb_DataRFScale_change.Text = "0";
            }


            //Log.LogStr("Test","=======================RF");
        }

        /// <summary>
        /// 카메라가 250mm을 기준으로 Vision Pro에서 검출된 x,y 축에 대해 10px당 1mm의 이동을 보이며
        /// 250mm을 기준으로 원근은 0.003~0.004 scale당 1mm 의 이동이 있었음. (스케일의 소수점 4자리 이하는 변동이 항상 존재)
        /// https://pxcalc.com/
        /// 2464*2056   0.0879mm  dpi 289.11
        /// 1232*1024   0.176mm  dpi 144.32
        /// pixel * 2.54 / dpi = cm
        /// https://yaraba.tistory.com/1212
        /// https://butterguy.tistory.com/entry/%EC%A7%81%EA%B0%81-%EC%A2%8C%ED%91%9C%EA%B3%84%EB%A5%BC-%ED%86%B5%ED%95%B4-x-y-%EC%9C%84%EC%B9%98-%EA%B5%AC%ED%95%98%EA%B8%B0
        /// </summary>
        private string[] measureLF(double xValue, double yValue, double Scale)
        {

            double Angle = double.Parse(Data.LFCameraDEGREE) / 180 * Math.PI;

            //double mX = Math.Abs(Data.LF_Vision.OriginLValue * Math.Cos(Angle) - Data.LF_Vision.OriginHValue * Math.Sin(Angle));
            //double mY = Math.Abs(Data.LF_Vision.OriginLValue * Math.Sin(Angle) + Data.LF_Vision.OriginHValue * Math.Cos(Angle));

            //double mX1 = Math.Abs(Data.LF_Vision.LValue * Math.Cos(Angle) - Data.LF_Vision.HValue * Math.Sin(Angle));
            //double mY1 = Math.Abs(Data.LF_Vision.LValue * Math.Sin(Angle) + Data.LF_Vision.HValue * Math.Cos(Angle));

            //double movedX = (xValue - Data.LF_Vision.OriginLValue) * Math.Cos(Angle) - (yValue - Data.LF_Vision.OriginHValue) * Math.Sin(Angle) + Data.LF_Vision.OriginHValue;
            double movedY = (xValue - Data.LF_Vision.OriginLValue) * Math.Sin(Angle) + (yValue - Data.LF_Vision.OriginHValue) * Math.Cos(Angle) 
                            + Data.LF_Vision.OriginHValue;

            string[] ret = { String.Format("{0:0.000}", ((xValue * Scale) - Data.LF_Vision.OriginLValue) / double.Parse(Data.FPXPARAM)),
                String.Format("{0:0.000}", (movedY - Data.LF_Vision.OriginHValue) / double.Parse(Data.FPXPARAM))};
            //var fovH = (Data.LF_Vision.WorkingDistance * Data.CameraData.SensorH) / Data.CameraData.Lens;
            //var fovV = (Data.LF_Vision.WorkingDistance * Data.CameraData.SensorV) / Data.CameraData.Lens;
            //string[] ret = { String.Format("{0:0.000}", ((xValue * Scale) - Data.LF_Vision.OriginLValue) * (fovH/Data.LF_Vision.Get_H)),
            //    String.Format("{0:0.000}", ((yValue * Scale) - Data.LF_Vision.OriginHValue) *(fovV/Data.LF_Vision.Get_V)) };

            return ret;
        }


        private string[] measureRF(double xValue, double yValue, double Scale)
        {
            double Angle = double.Parse(Data.RFCameraDEGREE) / 180 * Math.PI;

            //double mX = Math.Abs(Data.RF_Vision.OriginLValue * Math.Cos(Angle) - Data.RF_Vision.OriginHValue * Math.Sin(Angle));
            //double mY = Math.Abs(Data.RF_Vision.OriginLValue * Math.Sin(Angle) + Data.RF_Vision.OriginHValue * Math.Cos(Angle));

            //double mX1 = Math.Abs(Data.RF_Vision.LValue * Math.Cos(Angle) - Data.RF_Vision.HValue * Math.Sin(Angle));
            //double mY1 = Math.Abs(Data.RF_Vision.LValue * Math.Sin(Angle) + Data.RF_Vision.HValue * Math.Cos(Angle));

            //double movedX = (xValue - Data.RF_Vision.OriginLValue) * Math.Cos(Angle) - (yValue - Data.RF_Vision.OriginHValue) * Math.Sin(Angle) + Data.RF_Vision.OriginHValue;
            double movedY = (xValue - Data.RF_Vision.OriginLValue) * Math.Sin(Angle) + (yValue - Data.RF_Vision.OriginHValue) * Math.Cos(Angle) + Data.RF_Vision.OriginHValue;

            string[] ret = { String.Format("{0:0.000}", ((xValue * Scale) - Data.RF_Vision.OriginLValue) / double.Parse(Data.FPXPARAM)),
                String.Format("{0:0.000}", (movedY - Data.RF_Vision.OriginHValue) / double.Parse(Data.FPXPARAM))};

            //var fovH = (Data.RF_Vision.WorkingDistance * Data.CameraData.SensorH) / Data.CameraData.Lens;
            //var fovV = (Data.RF_Vision.WorkingDistance * Data.CameraData.SensorV) / Data.CameraData.Lens;
            //string[] ret = { String.Format("{0:0.000}", ((xValue * Scale) - Data.RF_Vision.OriginLValue) * (fovH/Data.RF_Vision.Get_H)),
            //    String.Format("{0:0.000}", ((yValue * Scale) - Data.RF_Vision.OriginHValue) *(fovV/Data.RF_Vision.Get_V)) };
            return ret;

        }


        private int getLFImage(bool testFlag) // 0: OK 1:err 2:timeout 3:not enable
        {
            if (!lfsw.IsRunning)
            {
                lfsw.Start();
            }
            else
            {
                lfsw.Restart();
            }

            int trignum0 = 4, ret = 2;
            //CB_LFVISION = false;
            if (!Data.LF_Vision.Enable)
            {
                return ret;
            }
            //Console.WriteLine("Trigger 0 : " + trignum0);
            //Log.LogStr("getLFImage", "Trigger 0 : " + trignum0);
            ret = 0;

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                this.Invoke(new Action(delegate ()
                {
                    try
                    {
                        LFretryCnt = 0;
                        cogToolBlockEditV21.Subject.Inputs["LFImage"].Value = mAcqFifo[0].Acquire(out trignum0);//.ScaleImage(Data.LF_Vision.PixelH, Data.LF_Vision.PixelV); //mAcqFifo[0].CompleteAcquire(acqTicket0, out completeTicket0, out trignum0);
                        try
                        {
                            for (int i = 0; i < LFTools.Count; i++)
                            {
                                //Log.LogStr("getLFImage","11111111111111111");
                                cogToolBlockEditV21.Subject.Tools[LFTools[i]].Run();

                                //cogToolBlockEditV21.Subject.Tools["LFPatMat"].Run();
                                //Log.LogStr("getLFImage", "2222222222222222");
                            }
                            ret = 1;
                        }
                        catch (Exception e)
                        {
                            Log.LogStr("getLFImage err", e.ToString());
                            ret = 2;
                        }

                    }
                    catch (CogAcqAbnormalException)
                    {
                        if (!testFlag)
                        {
                            //getLFImage(false, savedDirname);
                        }
                        ret = 2;
                    }
                    catch (CogAcqTimingException)
                    {
                        ret = 2;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        ret = 2;
                    }
                    finally
                    {
                    }
                }));
            }));
            thread.Start();


            return ret;
        }

        private int getRFImage(bool testFlag) // 0: OK 1:err 2:timeout 3:not enable
        {
            if (!rfsw.IsRunning)
            {
                rfsw.Start();
            }
            else
            {
                rfsw.Restart();
            }
            int trignum1 = 4, ret = 2;
            //CB_RFVISION = false;
            if (!Data.RF_Vision.Enable)
            {
                return ret;
            }

            Thread thread = new Thread(new ThreadStart(delegate ()
            {
                this.Invoke(new Action(delegate ()
                {
                    try
                    {
                        RFretryCnt = 0;
                        cogToolBlockEditV21.Subject.Inputs["RFImage"].Value = mAcqFifo[1].Acquire(out trignum1);//.ScaleImage(Data.RF_Vision.PixelH, Data.RF_Vision.PixelV);//mAcqFifo[1].CompleteAcquire(acqTicket1, out completeTicket1, out trignum1); 
                        try
                        {
                            for (int i = 0; i < RFTools.Count; i++)
                            {
                                cogToolBlockEditV21.Subject.Tools[RFTools[i]].Run();
                            }
                            ret = 1;

                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e.ToString());
                            Log.LogStr("getRFImage", e.ToString());
                            ret = 2;
                        }
                    }
                    catch (CogAcqAbnormalException)
                    {
                    //MessageBox.Show("Buffer Over");
                    ret = 2;
                    }
                    catch (CogAcqTimingException)
                    {
                    //MessageBox.Show("TimeOut");
                    ret = 2;
                    }
                    catch (Exception ex)
                    {
                    //MessageBox.Show(ex.ToString());
                    ret = 2;
                    }
                    finally
                    {
                    //LightOnOff(1, false);
                    //Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                }
                }));
            }));
            thread.Start();
            return ret;
        }

        #endregion
        
        
        private void tmr_Sequence_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;

            if ((date.Hour == 6 && date.Minute >= 5) && (date.Hour == 6 && date.Minute < 10)) delCogImage("", "", null);

            this.Text ="AnG Vision "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }


        private void SaveResult()
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string[] splitdate = date.Split('-');
            string time = DateTime.Now.ToString("HH:mm:ss");
            string filename = Data.barCode + "_" + time + ".dat";
            bool dirExists = System.IO.Directory.Exists(RESULTLOGDIR);

            if (!dirExists)
            {
                System.IO.Directory.CreateDirectory(RESULTLOGDIR);
            }
            dirExists = System.IO.Directory.Exists(RESULTLOGDIR + "\\" + splitdate[0]);
            if (!dirExists)
            {
                System.IO.Directory.CreateDirectory(RESULTLOGDIR + "\\" + splitdate[0]);
            }
            dirExists = System.IO.Directory.Exists(RESULTLOGDIR + "\\" + splitdate[0] + "\\" + splitdate[1]);
            if (!dirExists)
            {
                System.IO.Directory.CreateDirectory(RESULTLOGDIR + "\\" + splitdate[0] + "\\" + splitdate[1]);
            }
            dirExists = System.IO.Directory.Exists(RESULTLOGDIR + "\\" + splitdate[0] + "\\" + splitdate[1] + "\\" + splitdate[2]);
            if (!dirExists)
            {
                System.IO.Directory.CreateDirectory(RESULTLOGDIR + "\\" + splitdate[0] + "\\" + splitdate[1] + "\\" + splitdate[2]);
            }

            FileStream fs = new FileStream(RESULTLOGDIR + "\\" + splitdate[0] + "\\" + splitdate[1] + "\\" + splitdate[2] + "\\" + filename.Replace(":",""), FileMode.Append, FileAccess.Write);
            StreamWriter mStreamWriter = new StreamWriter(fs, System.Text.Encoding.UTF8);

            //FL.WriteLine("Car:" + tb_Cartype.Text + "," + "MeasureType:" + GlobalValue.MeasureParam + "," + "X:" + tb_DataLFX_mm.Text + "," + "Y:" + tb_DataLFY_mm.Text + "," + "TIME:" + lfsw.ElapsedMilliseconds.ToString());
            //Console.WriteLine("Car:" + tb_Cartype.Text + "MeasureType:" + GlobalValue.MeasureParam + "," + "X:" + tb_DataLFX_mm.Text + "," + "Y:" + tb_DataLFY_mm.Text + "," + "TIME:" + lfsw.ElapsedMilliseconds.ToString());
            mStreamWriter.WriteLine("[Result]");
            mStreamWriter.WriteLine("Date=" + date + " " + time);
            mStreamWriter.WriteLine("BarCode=" + Data.barCode);
            mStreamWriter.WriteLine("CarType=" + tb_Cartype.Text);

            double LFTimeElapse = 0.0;
            double RFTimeElapse = 0.0;
            if(LFTimeElapsed.Count > 0) {
                LFTimeElapse = LFTimeElapsed.Average();
            }
            if(RFTimeElapsed.Count > 0)
            {
                RFTimeElapse = RFTimeElapsed.Average();
            }
            
            
            double totalTime = LFTimeElapse + RFTimeElapse;
            mStreamWriter.WriteLine("TotalLapTime=" + totalTime);
            if(tb_StatusLF.Text == "1") { 
                mStreamWriter.WriteLine("LF=OK");
            }
            else if (tb_StatusLF.Text == "2")
            {
                mStreamWriter.WriteLine("LF=NG");
            }
            else
            {
                mStreamWriter.WriteLine("LF=NA");
            }

            if (tb_StatusRF.Text == "1")
            {
                mStreamWriter.WriteLine("RF=OK");
            }
            else if (tb_StatusRF.Text == "2")
            {
                mStreamWriter.WriteLine("RF=NG");
            }
            else
            {
                mStreamWriter.WriteLine("RF=NA");
            }

            

            mStreamWriter.WriteLine("LFTime=" + LFTimeElapse);
            mStreamWriter.WriteLine("RFTime=" + RFTimeElapse);
            
            mStreamWriter.WriteLine("LFDataCnt=" + LFXVal.Count);
            mStreamWriter.WriteLine("RFDataCnt=" + RFXVal.Count);


            mStreamWriter.WriteLine("LFHVision=" + tb_DataLFX_mm.Text);
            mStreamWriter.WriteLine("LFVVision=" + tb_DataLFY_mm.Text);
            mStreamWriter.WriteLine("RFHVision=" + tb_DataRFX_mm.Text);
            mStreamWriter.WriteLine("RFVVision=" + tb_DataRFY_mm.Text);

            List<int> MaxArr = new List<int>();
            MaxArr.Add(LFXVal.Count);
            MaxArr.Add(RFXVal.Count);

            for (int i = 0; i < MaxArr.Max() ; i++)
            {
                double LFX = 0.0;
                double LFY = 0.0;
                double RFX = 0.0;
                double RFY = 0.0;

                if(LFXVal.Count > 0 && LFXVal.Count <= MaxArr.Max())
                {
                    LFX = LFXVal[i];
                    //Console.WriteLine("LFXVal: " + LFXVal[i]);
                }
                if(LFYVal.Count > 0 && LFYVal.Count <= MaxArr.Max())
                {
                    LFY = LFYVal[i];
                    //Console.WriteLine("LFYVal: " + LFYVal[i]);
                }
                string[] FLret = measureLF(LFX, LFY, Data.LF_Vision.Scale);
                if(LFX == 0.0)
                {
                    FLret[0] = "0.0";
                }
                if(LFY == 0.0)
                {
                    FLret[1] = "0.0";
                }
                
                if(RFXVal.Count > 0 && RFXVal.Count <= MaxArr.Max())
                {
                    RFX = RFXVal[i];
                }
                if(RFYVal.Count > 0 && RFYVal.Count <= MaxArr.Max())
                {
                    RFY = RFYVal[i];
                }
                string[] FRret = measureRF(RFX, RFY, Data.RF_Vision.Scale);
                if (RFX == 0.0)
                {
                    FRret[0] = "0.0";
                }
                if (RFY == 0.0)
                {
                    FRret[1] = "0.0";
                }
                
                
                mStreamWriter.WriteLine(string.Format("{0:D3}", i)+"=" + FLret[0] + ", "+ FLret[1] + ", " + FRret[0] + ", " + FRret[1] );
                //000=LFH, LFV, RFH, RFV, LRH, LRV, RRH,RRV 
                //001=LFH, LFV, RFH, RFV, LRH, LRV, RRH,RRV
                //002=LFH, LFV, RFH, RFV, LRH, LRV, RRH,RRV
            }


            bool resultSaveDB = saveDatabase();
            Log.LogStr("Vision", "Try SaveDB Result =" + resultSaveDB);

            lfsw.Reset();
            rfsw.Reset();
            //lfsw.Stop();

            LFXVal.Clear();
            LFYVal.Clear();
            RFXVal.Clear();
            RFYVal.Clear();
            LFTimeElapsed.Clear();
            RFTimeElapsed.Clear();

            mStreamWriter.Close();

        }

        private bool saveDatabase()
        {
            Log.LogStr("Vision", "SaveDB");
            using (SqlConnection dbcon = new SqlConnection(Data.DBPath))
            {
                string strQuery = "";
                int ReTest = 0;
                try
                {
                    dbcon.Open();
                    strQuery = "SELECT COUNT(*) FROM VISION WHERE BODY_NO = '" + Data.barCode + "'";
                    SqlCommand myDataCmd = new SqlCommand(strQuery, dbcon);
                    ReTest = Convert.ToInt32(myDataCmd.ExecuteScalar());
                    myDataCmd.Dispose();
                    dbcon.Close();
                    ReTest++;
                }
                catch
                {
                    Log.LogStr("[ERR]", "DB FAIL1  - > " + strQuery);
                    
                }

                try { 
                    
                    strQuery = "insert into VISION(DATETIME,BODY_NO,CARTYPE,TESTCOUNT,LF_HEIGHT,LF_VERTICAL,RF_HEIGHT,RF_VERTICAL) Values('" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                        "','" + Data.barCode + 
                        "','" + Data.CarType.ToString() + 
                        "','" + ReTest +
                        "','" + (float)(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue) + 
                        "','" + (float)(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue) +
                        "','" + (float)(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue) +
                        "','" + (float)(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue) +
                        "')";
                    dbcon.Open();
                    SqlCommand myDataIn = new SqlCommand(strQuery, dbcon);
                    myDataIn.ExecuteNonQuery();
                    myDataIn.Dispose();
                    dbcon.Close();
                    return true;
                }
                catch
                {
                    Log.LogStr("[ERR]", "DB FAIL1_1  - > " + strQuery);
                    return false;
                }

                //strQuery = "";
                /*
                try { 
                        strQuery = "UPDATE VISION SET " +
                        "DATETIME = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "'," +
                        "BODY_NO = '" + Data.barCode + "'," +
                        "CARTYPE = '" + Data.CarType.ToString() + "'," +
                        "TESTCOUNT = '" + ReTest + "'," +
                        "LF_HEIGHT = '" + (float)(Data.LF_Vision.OriginLValue - Data.LF_Vision.LValue) + "'," +
                        "LF_VERTICAL = '" + (float)(Data.LF_Vision.OriginHValue - Data.LF_Vision.HValue) + "'," +
                        "RF_HEIGHT = '" + (float)(Data.RF_Vision.OriginLValue - Data.RF_Vision.LValue) + "'," +
                        "RF_VERTICAL = '" + (float)(Data.RF_Vision.OriginHValue - Data.RF_Vision.HValue) + "',";
                    dbcon.Open();
                    SqlCommand myData = new SqlCommand(strQuery, dbcon);
                    myData.ExecuteNonQuery();
                    myData.Dispose();
                    dbcon.Close();

                    return true;
                }
                catch
                {
                    Log.LogStr("[ERR]", "DB FAIL2  - > " + strQuery);
                    return false;
                }
                */
            }
        }

        private void ClearData()
        {
            //for (int i = 0; i < LFOutputs.Count; i++)
            //{
            //    cogToolBlockEditV21.Subject.Outputs[LFOutputs[i]].Value = null;
            //}
            //for (int i = 0; i < RFOutputs.Count; i++)
            //{
            //    cogToolBlockEditV21.Subject.Outputs[RFOutputs[i]].Value = null;
            //}

            cogToolBlockEditV21.Subject.Outputs["LF_LPos"].Value = null;
            cogToolBlockEditV21.Subject.Outputs["LF_HPos"].Value = null;
            cogToolBlockEditV21.Subject.Outputs["LF_Cnt"].Value = null;

            cogToolBlockEditV21.Subject.Outputs["RF_LPos"].Value = null;
            cogToolBlockEditV21.Subject.Outputs["RF_HPos"].Value = null;
            cogToolBlockEditV21.Subject.Outputs["RF_Cnt"].Value = null;


            Data.LF_Vision.State = 0;
            Data.LF_Vision.LValue = 0;
            Data.LF_Vision.HValue = 0;
            Data.LF_Vision.Scale = 0;
            Data.LF_Vision.OriginLValue = 0;
            Data.LF_Vision.OriginHValue = 0;
            Data.RF_Vision.State = 0;
            Data.RF_Vision.LValue = 0;
            Data.RF_Vision.HValue = 0;
            Data.RF_Vision.Scale = 0;
            Data.RF_Vision.OriginLValue = 0;
            Data.RF_Vision.OriginHValue = 0;

            //tb_StatusLF.Text = "0";
            //tb_StatusRF.Text = "0";

            //tb_DataLFL.Text = "0";
            //tb_DataLFH.Text = "0";

        }

        private void ClearUi()
        {
            tb_StatusLF.Text = "0";
            tb_DataLFL.Text = "0";
            tb_DataLFH.Text = "0";
            tb_DataLFX_origin.Text = "0";
            tb_DataLFY_origin.Text = "0";
            tb_DataLFScale_change.Text = "0";
            tb_DataLFX_mm.Text = "0";
            tb_DataLFY_mm.Text = "0";

            tb_StatusRF.Text = "0";
            tb_DataRFL.Text = "0";
            tb_DataRFH.Text = "0";
            tb_DataRFX_origin.Text = "0";
            tb_DataRFY_origin.Text = "0";
            tb_DataRFScale_change.Text = "0";
            tb_DataRFX_mm.Text = "0";
            tb_DataRFY_mm.Text = "0";


        }


        private Boolean ReceiveBufEmptyChk()
        {
            int nLen;
            string strTemp = "";

            if ((nLen = LightPort.BytesToRead) > 0)
            {
                strTemp += LightPort.ReadExisting();

                Log.LogStr("Light", string.Format("[RECV] = [{0}]", strTemp));

                if (strTemp.IndexOf(Lightmsg) >= 0) return true;
                else return false;
            }

            return false;
        }
        
        #region LightPort
        private bool LightPortOpen()
        {
            ClosePort();
            LightPort.PortName = Data.LightPort;
            LightPort.BaudRate = 38400;
            LightPort.Parity = Parity.None;
            LightPort.DataBits = 8;
            LightPort.StopBits = StopBits.One;

            //LightPort.Encoding = new System.Text.ASCIIEncoding();
            LightPort.DiscardNull = false;    // NULL 값 송수신

            try
            {
                LightPort.Open();
            }
            catch
            {
            }
            finally
            {
                if (LightPort.IsOpen)
                {
                    Log.LogStr("LightPort", Data.LightPort + " Open OK");
                    m_bPortOpened = true;
                    //SendLight("loadbright 0\r");
                }
                else
                {
                    Log.LogStr("LightPort", Data.LightPort + " Open Fail");
                    m_bPortOpened = false;
                }
            }
            return m_bPortOpened;
        }

        public Boolean ClosePort()
        {
            if (LightPort.IsOpen)
            {
                LightOnOff(false);
                Log.LogStr("LightPort", Data.LightPort + " Close");
                LightPort.Close();
            }

            return true;
        }

        private bool LightOnOff(bool OnOff)
        {
            if (!LightPort.IsOpen)
            {
                Log.LogStr("Light", "Can not Send Data - Port Closed");

                //LightPortOpen();
                return false;
            }
            if (OnOff)
                SendLight("setonex ff");
            else SendLight("setonex 00");

            return true;
        }
        private bool LightOnOff(int ch, bool OnOff)
        {
            if (!LightPort.IsOpen)
            {
                Log.LogStr("Light", "Can not Send Data - Port Closed");

                //LightPortOpen();
                return false;
            }
            if (OnOff)
                SendLight("seton " + ch + " 1");
            else SendLight("seton " + ch + " 0");

            return true;
        }

        private void LightSet(int ch, int value)
        {
            if (!LightPort.IsOpen)
            {
                Log.LogStr("Light", "Can not Send Data - Port Closed");
                // return;
                //LightPortOpen();
                return;
            }
            if (value > 255) value = 255;
            //string temp = string.Format("setbrightness {0} {1:000}\r", ch,value);
            string temp = string.Format("# {0} {1}", ch, value);
            //Console.WriteLine(temp);
            SendLight(temp);
        }

        public void SendLight(string _strMsg)
        {
            if (!LightPort.IsOpen)
            {
                Log.LogStr("Light", "Can not Send Data - Port Closed");
                return;
            }

            try
            {
                byte[] bSendData = new byte[1024];
                string strTmp = "";
                Lightmsg = _strMsg;

                Encoding.ASCII.GetBytes(_strMsg, 0, _strMsg.Length, bSendData, 0);
                LightPort.Write(bSendData, 0, _strMsg.Length);
                LightPort.Write(_strMsg + "\r\n");
                LightPort.WriteLine("\r");


                for (int i = 0; i < _strMsg.Length + 1; i++)
                {
                    strTmp += string.Format("{0:X2} ", bSendData[i]);
                }

                Log.LogStr("Light", string.Format("[SEND] = [{0}]", strTmp));
            }
            catch (Exception ex)
            {
                Log.LogStr("SendLight", ex.ToString());
            }
        }
        #endregion
        
        #region COG FILE IO
        private void saveCogImage(string fileName, ICogImage imageOut)
        {
            Cognex.VisionPro.ImageFile.CogImageFileBMP cogBMPImage = new Cognex.VisionPro.ImageFile.CogImageFileBMP();
            
            //Log.LogStr("saveCogImage","Start   "+ fileName);

            string path = DateTime.Now.ToString("yyyyMMdd");
            string name = DateTime.Now.ToString("HHmmssfff");

            bool dirExists = System.IO.Directory.Exists(dirPath + "\\" + path);

            if (!dirExists)
            {
                System.IO.Directory.CreateDirectory(dirPath + "\\" + path);
            }
            cogBMPImage.Open(string.Concat(dirPath + "\\" + path + "\\", fileName, "_" + name + ".bmp"), Cognex.VisionPro.ImageFile.CogImageFileModeConstants.Write);
            cogBMPImage.Append((CogImage8Grey)imageOut);
            ((IDisposable)cogBMPImage).Dispose();

            //Log.LogStr("saveCogImage", "Stop   " + fileName);
        }
        private double getSaveSeconds()
        {
            double ret = 0.0;

            ret = (double)Data.MaxSaveDay * 24.0 * 60.0 * 60.0;

            return ret;
        }

        private void delCogImage(string dirName, string fileName, ICogImage imageOut)
        {
            DirectoryInfo Info = new DirectoryInfo(dirPath);
            List<string> dirList = new List<string>();
            DateTime nowDate = DateTime.Now;
            DateTime olderstDate;
            TimeSpan timeDiff;

            string[] files = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var stm = new FileInfo(files[i]);
                //string a = string.Format("{0:yyyyMMdd}", stm.CreationTime);

                olderstDate = stm.CreationTime;
                timeDiff = nowDate - olderstDate;
                if (timeDiff.TotalSeconds > getSaveSeconds())
                {
                    Log.LogStr("delete", files[i]);
                    stm.Delete();
                }
            }
        }
        #endregion
        
        private void serverGrap()
        {
            //Log.LogStr("Test", "=======================Start");
            LFretryCnt = RFretryCnt = 0;
            RFXVal.Clear();
            RFYVal.Clear();
            RFCogContinous = 0;
            LFXVal.Clear();
            LFYVal.Clear();
            LFCogContinous = 0;
            lfsw.Reset();
            rfsw.Reset();
            lfsw.Stop();
            rfsw.Stop();
            mRFTimeContinous.Reset();
            mRFTimeContinous.Stop();
            mLFTimeContinous.Reset();
            mLFTimeContinous.Stop();

            if (LightPort.IsOpen) LightSet(0, Data.VisionSet[Data.CurrentCartype].LFBrightness);
            if (LightPort.IsOpen) LightSet(1, Data.VisionSet[Data.CurrentCartype].RFBrightness);

            Log.LogStr("Sequence LF Light On set value LFBrightness: ", Data.VisionSet[Data.CurrentCartype].LFBrightness.ToString());
            Log.LogStr("Sequence RF Light On set value RFBrightness: " , Data.VisionSet[Data.CurrentCartype].RFBrightness.ToString());

            // Note that we are calling the .NET garbage collector every 5th acquisition to cleanup
            // objects on the heap.
            GC.Collect(2, GCCollectionMode.Forced);

            if (Data.UseToolblock == 0)
            {
                if (Data.LF_Vision.Enable && Convert.ToString(Data.MeasurePart, 2).PadLeft(8, '0').Substring(4)[0] == '1')
                {
                    if (LightPort.IsOpen) LightOnOff(0, true);
                    if (Data.CaptureType == 2)
                    {
                        mLFTimeContinous.Start();
                    }
                    getLFImage(false);
                }

                if (Data.RF_Vision.Enable && Convert.ToString(Data.MeasurePart, 2).PadLeft(8, '0').Substring(4)[1] == '1')
                {
                    if (LightPort.IsOpen) LightOnOff(1, true);
                    if (Data.CaptureType == 2)
                    {
                        mRFTimeContinous.Start();
                    }
                    getRFImage(false);

                }
            }
            else
            {
                if ((Data.LF_Vision.Enable && Convert.ToString(Data.MeasurePart, 2).PadLeft(8, '0').Substring(4)[0] == '1')
                    || (Data.RF_Vision.Enable && Convert.ToString(Data.MeasurePart, 2).PadLeft(8, '0').Substring(4)[1] == '1'))
                {
                    if (LightPort.IsOpen) LightOnOff(true);
                    if (Data.CaptureType == 2)
                    {
                        mLFTimeContinous.Start();
                    }
                    getImage();
                }
            }

        }


        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //yyb
            Data.CarType = byte.Parse(tb_Cartype.Text);
            LoadPatMat();

        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
            }
            catch { }
        }

        private void uiClear()
        {
            tb_DataLFH.Text = "0";
            tb_DataLFL.Text = "0";
            tb_DataLFX_mm.Text = "0";
            tb_DataLFY_mm.Text = "0";
            tb_DataLFX_origin.Text = "0";
            tb_DataLFY_origin.Text = "0";
            tb_DataLFScale_change.Text = "0";
            tb_DataRFH.Text = "0";
            tb_DataRFL.Text = "0";
            tb_DataRFX_mm.Text = "0";
            tb_DataRFY_mm.Text = "0";
            tb_DataRFX_origin.Text = "0";
            tb_DataRFY_origin.Text = "0";
            tb_DataRFScale_change.Text = "0";
        }

        private void startGrapOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.LogStr("TEST", "start grap ");
            uiClear();
            LFretryCnt = RFretryCnt = 0;
            RFXVal.Clear();
            RFYVal.Clear();
            RFCogContinous = 0;
            LFXVal.Clear();
            LFYVal.Clear();
            LFCogContinous = 0;
            lfsw.Reset();
            rfsw.Reset();
            lfsw.Stop();
            rfsw.Stop();
            mRFTimeContinous.Reset();
            mRFTimeContinous.Stop();
            mLFTimeContinous.Reset();
            mLFTimeContinous.Stop();


            if (LightPort.IsOpen) LightSet(0, Data.VisionSet[Data.CurrentCartype].LFBrightness);
            if (LightPort.IsOpen) LightSet(1, Data.VisionSet[Data.CurrentCartype].RFBrightness);

            Log.LogStr("Sequence LF Light On set value LFBrightness: ", Data.VisionSet[Data.CurrentCartype].LFBrightness.ToString());
            Log.LogStr("Sequence RF Light On set value RFBrightness: ", Data.VisionSet[Data.CurrentCartype].RFBrightness.ToString());

            GC.Collect(2, GCCollectionMode.Forced);

            if (Data.UseToolblock == 0)
            {
                if (Data.LF_Vision.Enable)
                {
                    if (LightPort.IsOpen) LightOnOff(0, true);
                    if (Data.CaptureType == 2)
                    {
                        mLFTimeContinous.Start();
                    }
                    getLFImage(false);
                }
                if (Data.RF_Vision.Enable)
                {
                    if (LightPort.IsOpen) LightOnOff(1, true);
                    if (Data.CaptureType == 2)
                    {
                        mRFTimeContinous.Start();
                    }
                    getRFImage(false);
                }
            }
            else
            {

                if (Data.LF_Vision.Enable || Data.RF_Vision.Enable)
                {
                    if (LightPort.IsOpen) LightOnOff(true);
                    if (Data.CaptureType == 2)
                    {
                        mLFTimeContinous.Start();
                    }

                    getImage();
                }
            }
        }

        private int getImage() // 0: OK 1:err 2:timeout 3:not enable
        {
            if (!lfsw.IsRunning)
            {
                lfsw.Start();
            }
            else
            {
                lfsw.Restart();
            }
            int trignum1 = 4, ret = 2;

            try
            {
                if (Data.LF_Vision.Enable)
                    cogToolBlockEditV21.Subject.Inputs["LFImage"].Value = mAcqFifo[0].Acquire(out trignum1);//.ScaleImage(Data.LF_Vision.PixelH, Data.LF_Vision.PixelV);
                if (Data.RF_Vision.Enable)
                    cogToolBlockEditV21.Subject.Inputs["RFImage"].Value = mAcqFifo[1].Acquire(out trignum1);//.ScaleImage(Data.RF_Vision.PixelH, Data.RF_Vision.PixelV);

                cogToolBlockEditV21.Subject.Run();
            }
            catch (CogAcqAbnormalException a)
            {
                Log.LogStr("CogAcqAbnormalException", a.ToString());
                //MessageBox.Show("Buffer Over");
                ret = 2;
            }
            catch (CogAcqTimingException)
            {
                //MessageBox.Show("TimeOut");
                ret = 2;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Log.LogStr("getImage", ex.ToString());
                ret = 2;
            }
            finally
            {
                if (LightPort.IsOpen)
                    LightOnOff(false);
                //Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            }

            return ret;
        }

      

        private void tb_DataLFX_mm_TextChanged(object sender, EventArgs e)
        {
            byte[] b = BitConverter.GetBytes(Convert.ToDouble(tb_DataLFX_mm.Text));
            Array.Copy(b, 0, msg, 6, 8);
        }

        private void tb_DataLFY_mm_TextChanged(object sender, EventArgs e)
        {
            byte[] b = BitConverter.GetBytes(Convert.ToDouble(tb_DataLFY_mm.Text));
            Array.Copy(b, 0, msg, 14, 8);
        }

        private void tb_DataRFX_mm_TextChanged(object sender, EventArgs e)
        {
            byte[] b = BitConverter.GetBytes(Convert.ToDouble(tb_DataRFX_mm.Text));
            Array.Copy(b, 0, msg, 22, 8);
        }

        private void tb_DataRFY_mm_TextChanged(object sender, EventArgs e)
        {
            byte[] b = BitConverter.GetBytes(Convert.ToDouble(tb_DataRFY_mm.Text));
            Array.Copy(b, 0, msg, 30, 8);
        }

        private void tb_StatusLF_TextChanged(object sender, EventArgs e)
        {
            msg[4] = Convert.ToByte(tb_StatusLF.Text);
        }

        private void tb_StatusRF_TextChanged(object sender, EventArgs e)
        {
            msg[5] = Convert.ToByte(tb_StatusRF.Text);
        }

        private void toolboxrunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.LogStr("toolboxrun", "start grap ");
            uiClear();
            LFretryCnt = RFretryCnt = 0;
            RFXVal.Clear();
            RFYVal.Clear();
            RFCogContinous = 0;
            LFXVal.Clear();
            LFYVal.Clear();
            LFCogContinous = 0;
            lfsw.Reset();
            rfsw.Reset();
            lfsw.Stop();
            rfsw.Stop();
            mRFTimeContinous.Reset();
            mRFTimeContinous.Stop();
            mLFTimeContinous.Reset();
            mLFTimeContinous.Stop();


            if (LightPort.IsOpen) LightSet(0, Data.VisionSet[Data.CurrentCartype].LFBrightness);
            if (LightPort.IsOpen) LightSet(1, Data.VisionSet[Data.CurrentCartype].RFBrightness);

            Log.LogStr("Sequence LF Light On set value LFBrightness: ", Data.VisionSet[Data.CurrentCartype].LFBrightness.ToString());
            Log.LogStr("Sequence RF Light On set value RFBrightness: ", Data.VisionSet[Data.CurrentCartype].RFBrightness.ToString());

            GC.Collect(2, GCCollectionMode.Forced);


            int trignum0 = 4;
            if (Data.LF_Vision.Enable)
            {
                if (LightPort.IsOpen) LightOnOff(0, true);
                cogToolBlockEditV21.Subject.Inputs["LFImage"].Value = mAcqFifo[0].Acquire(out trignum0);//.ScaleImage(Data.LF_Vision.PixelH, Data.LF_Vision.PixelV); //mAcqFifo[0].CompleteAcquire(acqTicket0, out completeTicket0, out trignum0);
            }
            if (Data.RF_Vision.Enable)
            {
                if (LightPort.IsOpen) LightOnOff(1, true);
                cogToolBlockEditV21.Subject.Inputs["RFImage"].Value = mAcqFifo[1].Acquire(out trignum0);//.ScaleImage(Data.RF_Vision.PixelH, Data.RF_Vision.PixelV); //mAcqFifo[0].CompleteAcquire(acqTicket0, out completeTicket0, out trignum0);

            }

            try
            {
                cogToolBlockEditV21.Subject.Run();
            }
            catch (Exception ex)
            {
                Log.LogStr("getLFImage err", ex.ToString());
            }

        }

        private void teachingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileCloseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mLFSW.Close();
            mRFSW.Close();
        }

    }
}
