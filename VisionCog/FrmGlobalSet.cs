using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VisionCog
{
    public partial class FrmGlobalSet : Form
    {
        DataGridView gv;

        public FrmGlobalSet()
        {
            InitializeComponent();
        }

        private void FrmGlobalSet_Load(object sender, EventArgs e)
        {
            string[] sPort;
            sPort = System.IO.Ports.SerialPort.GetPortNames();

            for (int i = 0; i < sPort.Length; i++)
            {
                cb_Serial1.Items.Add(sPort[i]);
            }
            string[] sPort1;
            sPort1 = System.IO.Ports.SerialPort.GetPortNames();


            tbIP.Text = Data.ServerIP;
            tbPort.Text = Convert.ToString(Data.ServerPort);

            tbCarmera1.Text = Data.CameraIP1;
            tbCarmera2.Text = Data.CameraIP2;
            tbFLDegree.Text = Data.LFCameraDEGREE;
            tbFRDegree.Text = Data.RFCameraDEGREE;

            maxSaveDaytextBox.Text = Data.MaxSaveDay.ToString();
            tbRetryCnt.Text = Data.RetryCnt.ToString();

            cb_Serial1.Text = Data.LightPort;
            cbCogDisOnOff.Text = Data.COGDISONOFF;
            txt_CaptureCnt.Text = Data.ContinousGrap.ToString();
            txt_CaptureTime.Text = Data.ContinousTimeGrap.ToString();
            cbMeasureParam.Text = Data.MeasureParam;
            cb_CaptureType.SelectedIndex = Data.CaptureType;
            txt_Pixelmm.Text = Data.FPXPARAM.ToString();

            txt_PxH_L.Text = Data.LF_Vision.PixelH.ToString();
            txt_PxV_L.Text = Data.LF_Vision.PixelV.ToString();
            txt_WD_L.Text = Data.LF_Vision.WorkingDistance.ToString();
            //txt_MaxH_L.Text = Data.LF_Vision.MaxH.ToString();
            //txt_MaxV_L.Text = Data.LF_Vision.MaxV.ToString();

            txt_PxH_R.Text = Data.RF_Vision.PixelH.ToString();
            txt_PxV_R.Text = Data.RF_Vision.PixelV.ToString();
            txt_WD_R.Text = Data.RF_Vision.WorkingDistance.ToString();
            //txt_MaxH_R.Text = Data.RF_Vision.MaxH.ToString();
            //txt_MaxV_R.Text = Data.RF_Vision.MaxV.ToString();

            txt_SSizeH.Text = Data.CameraData.SensorH.ToString();
            txt_SSizeV.Text = Data.CameraData.SensorV.ToString();
            txt_Lens.Text = Data.CameraData.Lens.ToString();

            dgv_Vision.RowCount = Data.VisionCnt;
            for (int i = 0; i < Data.VisionCnt; i++)
            {
                dgv_Vision.Rows[i].Cells[0].Value = Data.VisionSet[i].CarType;
                dgv_Vision.Rows[i].Cells[1].Value = Data.VisionSet[i].SelTool;
                dgv_Vision.Rows[i].Cells[3].Value = Data.VisionSet[i].LFBrightness.ToString();
                dgv_Vision.Rows[i].Cells[4].Value = Data.VisionSet[i].RFBrightness.ToString();
                dgv_Vision.Rows[i].Cells[5].Value = Data.VisionSet[i].LFMaxV.ToString();
                dgv_Vision.Rows[i].Cells[6].Value = Data.VisionSet[i].LFMaxH.ToString();
                dgv_Vision.Rows[i].Cells[7].Value = Data.VisionSet[i].RFMaxV.ToString();
                dgv_Vision.Rows[i].Cells[8].Value = Data.VisionSet[i].RFMaxH.ToString();
            }

            cb_Toolblock.SelectedIndex = Data.UseToolblock;
            cb_CaptureType_SelectedIndexChanged(null, null);

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Save_Data()) MessageBox.Show("Save Ok");
            else MessageBox.Show("Save Fail");
        }

        private bool Save_Data()
        {
            string strTmpPath = Application.StartupPath + "\\Setup.dat";
            FileInfo f = new FileInfo(strTmpPath);

            if (f.Exists)
            {
                try
                {
                    CodeINI.WriteIniFilePath(strTmpPath, "Server", "ServerIP", tbIP.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Server", "ServerPort", tbPort.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "IP1", tbCarmera1.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "IP2", tbCarmera2.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LFDEGREE", tbFLDegree.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RFDEGREE", tbFRDegree.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "FPXPARAM", txt_Pixelmm.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LPixelH", txt_PxH_L.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LPixelV", txt_PxV_L.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LSensorSize", txt_SSizeV.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LWD", txt_WD_L.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LMaxH", txt_MaxH_L.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "LMaxV", txt_MaxV_L.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RPixelH", txt_PxH_R.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RPixelV", txt_PxV_R.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RWD", txt_WD_R.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RMaxH", txt_MaxH_R.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "RMaxV", txt_MaxV_R.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "SensorH", txt_SSizeH.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "SensorV", txt_SSizeV.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Camera", "Lens", txt_Lens.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "Parameter", "MaxSavingDay", maxSaveDaytextBox.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "Parameter", "RetryCount", tbRetryCnt.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "Port", cb_Serial1.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "DisplayUse", cbCogDisOnOff.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "CaptureCount", txt_CaptureCnt.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "CaptureType", cb_CaptureType.SelectedIndex.ToString());
                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "CaptureTime", txt_CaptureTime.Text);
                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "ValueType", cbMeasureParam.Text);

                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "UseToolblock", cb_Toolblock.SelectedIndex.ToString());

                    CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", "Count", dgv_Vision.RowCount.ToString());
                    for (int i = 0; i < dgv_Vision.RowCount; i++)
                    {
                        CodeINI.WriteIniFilePath(strTmpPath, "VisionPro", i.ToString(), dgv_Vision.Rows[i].Cells[0].Value.ToString() + "," + 
                            dgv_Vision.Rows[i].Cells[1].Value.ToString()+","+
                            dgv_Vision.Rows[i].Cells[3].Value.ToString() + "," + 
                            dgv_Vision.Rows[i].Cells[4].Value.ToString() + "," +
                            dgv_Vision.Rows[i].Cells[5].Value.ToString() + "," +
                            dgv_Vision.Rows[i].Cells[6].Value.ToString() + "," +
                            dgv_Vision.Rows[i].Cells[7].Value.ToString() + "," +
                            dgv_Vision.Rows[i].Cells[8].Value.ToString() + ","
                            );
                    }

                    Log.LogStr("Global", "Global Data Save Ok");
                }
                catch
                {
                    Log.LogStr("Global", "Global Data Save Fail");
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

        private void btn_Select_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Application.StartupPath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //tbVppPath.Text = dlg.FileName;
            }
        }

        private void dgv_Vision_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gv = (DataGridView)sender;
                Point pt = new Point(e.X, e.Y);

                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Add");
                menu.Items.Add("Del");

                menu.ItemClicked += new ToolStripItemClickedEventHandler(menu_ItemClicked);
                menu.Show(gv, pt);
            }
        }
        int selectDel = -1;
        void menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Add":
                    gv.Rows.Add();
                    //MessageBox.Show(gv.RowCount.ToString());
                    dgv_Vision.Rows[gv.RowCount - 1].Cells[0].Value = (gv.RowCount - 1).ToString();
                    break;
                case "Del":
                    if (gv.Rows.Count > 1)
                    {
                        gv.Rows.RemoveAt(selectDel);
                        //gv.Rows.Remove(gv.Rows[gv.Rows.Count - 1]); ;
                    }
                    break;
                default:
                    break;
            }
        }

        private void dgv_Vision_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectDel = e.RowIndex;
            if (dgv_Vision.Columns[e.ColumnIndex].HeaderText != "Sel")
                return;

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Application.StartupPath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dgv_Vision.Rows[e.RowIndex].Cells[1].Value = @".\CogTool" + @"\" + Path.GetFileName(dlg.FileName);
            }
        }

        private void cb_CaptureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cb_CaptureType.SelectedIndex == 1)
            {
                gb_CaptureCnt.Enabled = true;
                gb_CaptureTime.Enabled = false;
            }
            else if (cb_CaptureType.SelectedIndex == 2)
            {
                gb_CaptureCnt.Enabled = false;
                gb_CaptureTime.Enabled = true;
            }
            else
            {
                gb_CaptureCnt.Enabled = false;
                gb_CaptureTime.Enabled = false;
            }
        }


    }
}

