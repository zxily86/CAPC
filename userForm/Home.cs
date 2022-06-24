using CAPC.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using ReWriteTextBox;

namespace CAPC.userForm
{
    public partial class Home : Form
    {
        private Beckhoff.Forms.TcAdsPlcServer _adsPlcServer;

        private Dictionary<string, string> sysButton_dic = new Dictionary<string, string>();


        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public Home()
        {
            InitializeComponent();

            try
            {
                _adsPlcServer = new Beckhoff.Forms.TcAdsPlcServer(801, Form1.PLCams);
                //this.pictureBox1.Image?.Dispose();
                //this.pictureBox1.Image = Properties.Resources.微信截图_20210303132448;

                timer1.Enabled = true;
                timer1.Interval = 300;

                sysButton_dic.Add("btn_sys_reset", ".Hmanu100");
                sysButton_dic.Add("btn_sys_start", ".Hmanu101");
                sysButton_dic.Add("btn_sys_pause", ".Hmanu104");
                sysButton_dic.Add("btn_sys_stop", ".Hmanu102");
                sysButton_dic.Add("btn_sys_purge", ".Hmanu103");
            }

            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }    
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_adsPlcServer.PlcIsRunning)
                {
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".PLC_Run")))
                    {
                        this.lbl_0_zhuanPan.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".Record_Watch_000")).ToString("000000");
                        this.lbl_1_scara.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".Record_Watch_001")).ToString("000000");
                        this.lbl_2_moZu.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".Record_Watch_002")).ToString("000000");
                        this.lbl_3_UPH.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".UPH")).ToString("000000");//UPH计数（PLC中清零）

                        this.lbl_scara_ng.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".Record_Watch_003")).ToString("000000");
                        this.lbl_tujiao_ng.Text = Convert.ToUInt32(plcParameterOp.GetPLCVar(_adsPlcServer, ".Record_Watch_005")).ToString("000000");


                        //各工位是否报警显示
                        this.stateDis_zhuanpan_shangliao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err1"));
                        this.stateDis_r2r_1.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err2"));
                        this.stateDis_chongya.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err3"));
                        this.stateDis_sanzui_tujiao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err4"));
                        this.stateDis_sizui_tujiao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err5"));
                        this.stateDis_jiance.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err6"));
                        this.stateDis_r2r_2.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err7"));
                        this.stateDis_simo.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err8"));
                        this.stateDis_dingwei_1.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err9"));
                        this.stateDis_r2r_3.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err10"));
                        this.stateDis_dingwei_2.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err11"));
                        this.stateDis_xuanzhuanTiehe.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err12"));
                        this.stateDis_jingya.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err13"));
                        this.stateDis_tiemo_fanglou.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err14"));
                        this.stateDis_dabiao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err15"));
                        this.stateDis_tuopan_xialiao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err16"));
                        this.stateDis_tuopan_shangliao.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err17"));
                        this.stateDis_scara.IsErr = Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".Err18"));
                        
                    }
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }
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
        }


        #endregion

        

        //生产计数清零
        private void btn_count_clear_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("重要操作，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    plcParameterOp.SetPLCVar(_adsPlcServer, $".RRudint[{i}]", 0);//".RRudint[0]"  .Record_Watch[0]
                    //}
                    plcParameterOp.SetPLCVar(_adsPlcServer, ".Clear_Data", true);
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }
        }

        //NG计数清零
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("重要操作，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    plcParameterOp.SetPLCVar(_adsPlcServer, $".RRudint[{i}]", 0);//".RRudint[0]"  .Record_Watch[0]
                    //}
                    plcParameterOp.SetPLCVar(_adsPlcServer, ".Clear_NG", true);
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }
        }

        private void btn_sys_start_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //
                ButtonSquare btn = (ButtonSquare)sender;
                btn.ButtonState = true;

                plcParameterOp.SetPLCVar(_adsPlcServer, sysButton_dic[btn.Name], true);
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }
        }

        private void btn_sys_start_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                ButtonSquare btn = (ButtonSquare)sender;
                btn.ButtonState = false;

                plcParameterOp.SetPLCVar(_adsPlcServer, sysButton_dic[btn.Name], false);
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}  {ex.StackTrace}"); }
        }
    }
}
