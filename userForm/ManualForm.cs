#define debug
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Beckhoff.Forms;
using CAPC.Config;
using ReWriteTextBox;
using System.Xml;
using System.Collections;
using System.IO;

using System.Threading;
using System.Threading.Tasks;

namespace CAPC.userForm
{
    public partial class ManualForm : Form
    {
        private TcAdsPlcServer _adsPlcServer;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        /// <summary>
        /// 相机标定步骤，设置按钮enable用
        /// </summary>
        private int calibrationStep_int = 1;
        private int calibrationStepOld_int = 0;

        public event Action<string> send_To_CCD;

        public ManualForm()
        {
            InitializeComponent();
            _adsPlcServer = new TcAdsPlcServer(801, Form1.PLCams);
            addmethod(this);
            timer1.Enabled = true;
            timer1.Interval = 300;

            //涂胶模拟点默认选择P1
            this.cbx_tujiaoDian_xuanze.SelectedIndex = 0;
            this.cbx_tujiaoDian_xuanze.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void Manu34_Click(object sender, EventArgs e)
        {
            //DialogResult result = MessageBox.Show("确认卡销已脱开且维护站短气缸在安全位", "安全提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (result == DialogResult.OK)
            //{
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".HManu34", true);
            //}
        }

        private void Manu33_Click(object sender, EventArgs e)
        {
            //DialogResult result = MessageBox.Show("确认卡销已脱开且维护站短气缸在安全位", "安全提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (result == DialogResult.OK)
            //{
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".HManu33", true);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "启动/停止")
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".SetMode", 1);
            //else if (comboBox1.Text == "待机模式")
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".SetMode", 2);
            //else if (comboBox1.Text == "维护模式")
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".SetMode", 3);
            //else if (comboBox1.Text == "印刷模式")
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".SetMode", 4);
        }

        private void buttonSquare1_Click(object sender, EventArgs e)
        {
            //if (!Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".MState_Starting")))
            //{
            //    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".StartCAPC_PLC")))
            //        plcParameterOp.SetPLCVar(_adsPlcServer, ".StartCAPC_PLC", false);
            //    else
            //        plcParameterOp.SetPLCVar(_adsPlcServer, ".StartCAPC_PLC", true);
            //}
        }

        #region SetOrResetBool
        private void addmethod(Control container)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren)
                    addmethod(c);
                else
                {
                    try
                    {
                        if (c.Name.Contains("Hmanu") && (c is ButtonSquare))
                        {
                            c.MouseDown += setBool;
                            c.MouseUp += resetBool;
                        }
                    }
                    catch (Exception e)
                    { MessageBox.Show(c.Name + e.Message); }
                }
            }
        }
        async void setBool(object sender, EventArgs e)
        {
            try
            {
                ButtonSquare btn = (ButtonSquare)sender;
                string H = "." + btn.Name;
                if(H.Contains("Hmanu27")|| H.Contains("Hmanu260") || H.Contains("Hmanu433") || H.Contains("Hmanu434"))
                {
                    if(MessageBox.Show("重要操作，是否继续？","", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==DialogResult.Yes)
                    {
                        plcParameterOp.SetPLCVar(_adsPlcServer, H, true);
                        await threadSleep();
                        plcParameterOp.SetPLCVar(_adsPlcServer, H, false);//弹窗后，没有了c.MouseUp事件了
                    }
                }
                else
                {
                    if (H.Contains("Hmanu_Trace_simu3"))//如果按下的是三嘴涂胶模拟按钮，除了触发，还要将下拉值传送
                    {
                        int tujiaoPoint = this.cbx_tujiaoDian_xuanze.SelectedIndex + 1;
                        plcParameterOp.SetPLCVar(_adsPlcServer, ".Glue_pos", tujiaoPoint);
                    }

                    plcParameterOp.SetPLCVar(_adsPlcServer, H, true);
                }
                
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        private async Task threadSleep()
        {
            Task first = Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            });
            await first;
        }

        void resetBool(object sender, EventArgs e)
        {
            try
            {
                ButtonSquare btn = (ButtonSquare)sender;
                string H1 = "." + btn.Name;
                plcParameterOp.SetPLCVar(_adsPlcServer, H1, false);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion

        #region  状态显示
        private void viewManual(Control control)
        {
            foreach (Control c in control.Controls)
            {
                //c is the child control here
                if (c.HasChildren)
                    viewManual(c);
                else
                {
                    try
                    {
                        if (c.Name.Contains("Hmanu") && (c is ButtonSquare))
                        {
                            ButtonSquare btn = (ButtonSquare)c;
                            string i = string.Empty;

                            i = c.Name.Replace("Hmanu", ".TW");
                            Object B = plcParameterOp.GetPLCVar(_adsPlcServer, i);
                            if(B!=null)
                            {
                                if ((bool)B)
                                    btn.ButtonState = true;
                                else
                                    btn.ButtonState = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label41.Text = Form1.scara_state;
            pos_x.Text = Form1.x.Trim();
            pos_y.Text = Form1.y.Trim();
            pos_z.Text = Form1.z.Trim();
            pos_r.Text = Form1.r.Trim();
            label10.Text = Form1.hand;
            if (!Form1.key)
            {
                tabPage4.Parent = null;
                //Hmanu9_10.Visible = false;  已删除控件
                //Hmanu10_10.Visible = false;已删除控件
                //Hmanu13_7.Visible = false; 已删除控件
                //Hmanu14.Visible = false;
            }
            else
            {
                tabPage4.Parent = tabControl1;
                //Hmanu9_10.Visible = true;已删除控件
                //Hmanu10_10.Visible = true;已删除控件
                //Hmanu13_7.Visible = true;已删除控件
                //Hmanu14.Visible = true;
            }

            if (_adsPlcServer.PlcIsRunning)
            {
                try
                {
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".PLC_Run")))
                    {
                        viewManual(tabControl1.SelectedTab);

                        if(tabControl1.SelectedTab == this.tabPage4)
                        {
                            this.lbl_time_sanzui.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".times3")).ToString("000000");
                            this.lbl_time_sizui.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".times4")).ToString("000000");
                        }
                        
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            }
            //不用管PLC运行否 
            #region 相机标定step
            try
            {
                if(calibrationStepOld_int!=calibrationStep_int)
                {
                    calibrationStepOld_int = calibrationStep_int;
                    this.btn_Calibration_1.Enabled = false;
                    this.btn_Calibration_2.Enabled = false;
                    this.btn_Calibration_3.Enabled = false;
                    this.btn_Calibration_4.Enabled = false;
                    this.btn_Calibration_5.Enabled = false;
                    this.btn_Calibration_6.Enabled = false;
                    this.btn_Calibration_7.Enabled = false;
                    this.btn_Calibration_photograph.Enabled = false;
                    if (Form1.key)
                    {
                        switch (calibrationStep_int)
                        {
                            case 1:
                                this.btn_Calibration_1.Enabled = true;
                                break;
                            case 3:
                                this.btn_Calibration_2.Enabled = true;
                                break;
                            case 5:
                                this.btn_Calibration_3.Enabled = true;
                                break;
                            case 7:
                                this.btn_Calibration_4.Enabled = true;
                                break;
                            case 9:
                                this.btn_Calibration_5.Enabled = true;
                                break;
                            case 11:
                                this.btn_Calibration_6.Enabled = true;
                                break;
                            case 13:
                                this.btn_Calibration_7.Enabled = true;
                                break;
                            case 2:
                            case 4:
                            case 6:
                            case 8:
                            case 10:
                            case 12:
                            case 14:
                                Thread.Sleep(1500);
                                if (Form1.scara_state != "Running")
                                {
                                    this.btn_Calibration_photograph.Enabled = true;
                                }
                                break;
                        }
                    }
                }
            }
            catch(Exception ex)
            { OutputDebugString($"{ex.Message}        {ex.StackTrace}"); }
            #endregion
        }
        #endregion


        private Dictionary<string, string> scaraJog_Dictionary=new Dictionary<string, string>();
        //SCARA坐标系偏移
        private void SCARA_Z_UP_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < Form1.tcpServer.ClientList.Count; i++)
                {
                    if (Form1.tcpServer.ClientList[i].Ip.Address.ToString() == Form1.IP_1 && Form1.tcpServer.ClientList[i].Ip.Port == Form1.Port_1)
                    {
                        if (Form1.HaveConnect)
                        {
                            if (sender is Button)
                            {
                                Button scara_btn = (Button)sender;
                                if (scara_btn.Name.Contains("SCARA") || scara_btn.Name.Contains("Calibration"))
                                {
                                    Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, scaraJog_Dictionary[scara_btn.Name] + ";" + '\r' + '\n');
                                    if(scara_btn.Name.Contains("Calibration"))
                                    {
                                        calibrationStep_int++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}         {ex.StackTrace}"); }
        }

        private void btn_Calibration_photograph_Click(object sender, EventArgs e)
        {
            if (Form1.scara_state == "Running")
            {
                MessageBox.Show("请等待机器人运动完成再点击");
            }
            else
            {
                try
                {
                    calibrationStep_int++;
                    if (calibrationStep_int > 14)
                    {
                        calibrationStep_int = 1;
                    }
                    send_To_CCD("#C3*");
                    using (StreamWriter sw = new StreamWriter(".\\calibrationPoints.csv", true))
                    {
                        sw.Write($"{DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss")},{Form1.x},{Form1.y},{Form1.r}\r\n");
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                { OutputDebugString($"{ex.Message}         {ex.StackTrace}"); }
            } 
        }

        #region 窗体按百分比缩放
        /// <summary>
        /// 同步缩放窗体上控件的大小和字体
        /// </summary>
        private ControlResizer Resizer; //定义缩放类
        public class ControlResizer
        {
            class ControlPosAndSize
            {
                public float FrmWidth { get; set; }
                public float FrmHeight { get; set; }
                public int Left { get; set; }
                public int Top { get; set; }
                public int Width { get; set; }
                public int Height { get; set; }
                public float FontSize { get; set; }

            }

            private Form _form;

            //句柄,大小信息
            private Dictionary<int, ControlPosAndSize> _dic = new Dictionary<int, ControlPosAndSize>();
            public ControlResizer(Form form)
            {
                _form = form;
                _form.Resize += _form_Resize;//绑定窗体大小改变事件

                _form.ControlAdded += form_ControlAdded;  //窗体上新增控件的处理
                _form.ControlRemoved += form_ControlRemoved;

                SnapControlSize(_form);//记录控件和窗体大小
            }

            void form_ControlRemoved(object sender, ControlEventArgs e)
            {
                var key = e.Control.Handle.ToInt32();
                _dic.Remove(key);
            }

            //绑定控件添加事件
            private void form_ControlAdded(object sender, ControlEventArgs e)
            {
                var ctl = e.Control;
                var ps = new ControlPosAndSize
                {
                    FrmHeight = _form.Height,
                    FrmWidth = _form.Width,
                    Width = ctl.Width,
                    Height = ctl.Height,
                    Left = ctl.Left,
                    Top = ctl.Top,
                    FontSize = ctl.Font.Size
                };
                var key = ctl.Handle.ToInt32();
                _dic[key] = ps;
            }

            void _form_Resize(object sender, EventArgs e)
            {
                ResizeControl(_form);
            }

            private void ResizeControl(Control control)
            {
                foreach (Control ctl in control.Controls)
                {
                    var key = ctl.Handle.ToInt32();
                    if (_dic.ContainsKey(key))
                    {
                        var ps = _dic[key];
                        var newx = _form.Width / ps.FrmWidth;
                        var newy = _form.Height / ps.FrmHeight;

                        ctl.Top = (int)(ps.Top * newy);
                        ctl.Height = (int)(ps.Height * newy);

                        ctl.Left = (int)(ps.Left * newx);
                        ctl.Width = (int)(ps.Width * newx);

                        ctl.Font = new Font(ctl.Font.Name, ps.FontSize * newy, ctl.Font.Style, ctl.Font.Unit);

                        if (ctl.Controls.Count > 0)
                        {
                            ResizeControl(ctl);
                        }

                    }

                }
            }

            /// <summary>
            /// 创建控件的大小快照,参数为需要记录大小控件的 容器
            /// </summary>
            private void SnapControlSize(Control control)
            {
                foreach (Control ctl in control.Controls)
                {
                    var ps = new ControlPosAndSize
                    {
                        FrmHeight = _form.Height,
                        FrmWidth = _form.Width,
                        Width = ctl.Width,
                        Height = ctl.Height,
                        Left = ctl.Left,
                        Top = ctl.Top,
                        FontSize = ctl.Font.Size
                    };

                    var key = ctl.Handle.ToInt32();

                    _dic[key] = ps;

                    //绑定添加事件
                    ctl.ControlAdded += form_ControlAdded;
                    ctl.ControlRemoved += form_ControlRemoved;

                    if (ctl.Controls.Count > 0)
                    {
                        SnapControlSize(ctl);
                    }

                }

            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Resizer = new ControlResizer(this);

            //
            scaraJog_Dictionary.Add("SCARA_X_UP", "XP");
            scaraJog_Dictionary.Add("SCARA_X_DOWN", "XM");
            scaraJog_Dictionary.Add("SCARA_Y_UP", "YP");
            scaraJog_Dictionary.Add("SCARA_Y_DOWN", "YM");
            scaraJog_Dictionary.Add("SCARA_Z_UP", "ZP");
            scaraJog_Dictionary.Add("SCARA_Z_DOWN", "ZM");
            scaraJog_Dictionary.Add("SCARA_U_UP", "UP");
            scaraJog_Dictionary.Add("SCARA_U_DOWN", "UM");

            //标定发给机器人命令
            scaraJog_Dictionary.Add("btn_Calibration_1", "Photo");
            scaraJog_Dictionary.Add("btn_Calibration_2", "Photo2");
            scaraJog_Dictionary.Add("btn_Calibration_3", "Photo3");
            scaraJog_Dictionary.Add("btn_Calibration_4", "Photo4");
            scaraJog_Dictionary.Add("btn_Calibration_5", "Photo5");
            scaraJog_Dictionary.Add("btn_Calibration_6", "Photo6");
            scaraJog_Dictionary.Add("btn_Calibration_7", "Photo7");

        }

        #endregion

    }
}