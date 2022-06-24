#define debug
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAPC.Config;
using ReWriteTextBox;

namespace CAPC.userForm
{
    public partial class IOView : Form
    {
        private Beckhoff.Forms.TcAdsPlcServer _adsPlcServer;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public IOView()
        {
            InitializeComponent();

            _adsPlcServer = new Beckhoff.Forms.TcAdsPlcServer(801, Form1.PLCams);
            timer1.Enabled = true;
            timer1.Interval = 200;

            //this.tabControl1.Multiline = true;
            this.tabControl1.SizeMode = TabSizeMode.Normal;
            //this.tabControl1.ItemSize.Height = 25; //报错，还是属性填入吧
            this.tabControl1.SelectedTab = this.tabPage5;
        }

        #region IOVIEW FUNCTION
        private void viewIOs(Control container)
        {
            try
            {
                foreach (Control c in container.Controls)
                {
                    //c is the child control here
                    if (c.HasChildren)
                        viewIOs(c);
                    else
                    {
                        if (c is ButtonRound)
                        {
                            try
                            {
                                ButtonRound btn = (ButtonRound)c;
                                string IOName = "." + c.Name;
                                if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, IOName)))
                                    btn.ButtonState = true;
                                else
                                    btn.ButtonState = false;
                            }
                            catch (Exception e)
                            { MessageBox.Show(c.Name + e.Message); }
                        }
                    }
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion

        #region TIMER EVENT
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (_adsPlcServer.PlcIsRunning)
            {
                try
                {
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".PLC_Run")))
                    {
                        viewIOs(tabControl1.SelectedTab);
                        if (tabControl1.SelectedTab.Name == "tabPage4")
                        {
                            
                        }
                        else if (tabControl1.SelectedTab.Name == "tabPage5")
                        {
                            this.lbl_dd.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS17ACTPos")).ToString("00000.000");//dd
                            this.lbl_AXIS0ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS0ACTPos")).ToString("00000.000");//上料顶升伺服
                            this.lbl_AXIS1ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS1ACTPos")).ToString("00000.000");//上料旋转伺服

                            this.lbl_AXIS9ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS9ACTPos")).ToString("00000.000");//三嘴涂胶X轴伺服
                            this.lbl_AXIS16ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS16ACTPos")).ToString("00000.000");//三嘴涂胶Y轴伺服
                            this.lbl_AXIS14ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS14ACTPos")).ToString("00000.000");//旋转贴合伺服

                            this.lbl_AXIS11ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS11ACTPos")).ToString("00000.000");//四嘴涂胶X轴伺服
                            this.lbl_AXIS12ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS12ACTPos")).ToString("00000.000");//四嘴涂胶Y轴伺服

                            this.lbl_AXIS23ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS23ACTPos")).ToString("00000.000");//上料外部升降步进
                            this.lbl_AXIS24ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS24ACTPos")).ToString("00000.000");//上料内部升降步进
                            this.lbl_AXIS25ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS25ACTPos")).ToString("00000.000");//上料传输步进

                            this.lbl_AXIS20ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS20ACTPos")).ToString("00000.000");//下料内部升降步进
                            this.lbl_AXIS21ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS21ACTPos")).ToString("00000.000");//下料外部升降步进
                            this.lbl_AXIS22ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS22ACTPos")).ToString("00000.000");//下料传输步进

                            this.lbl_AXIS18ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS18ACTPos")).ToString("00000.000");//下料X伺服
                            this.lbl_AXIS19ACTPos.Text = Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS19ACTPos")).ToString("00000.000");//下料Y伺服
                        }
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            }

        }
#endregion


        private void TW50_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW50")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW50", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW50", true);
        }

        private void TW51_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW51")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW51", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW51", true);
        }

        private void TW52_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW52")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW52", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW52", true);
        }

        private void TW53_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW53")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW53", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW53", true);
        }

        private void TW54_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW54")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW54", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW54", true);
        }

        private void TW55_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW55")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW55", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW55", true);
        }

        private void TW56_Click(object sender, EventArgs e)
        {
            //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".TW56")))
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW56", false);
            //else
            //    plcParameterOp.SetPLCVar(_adsPlcServer, ".TW56", true);
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

                        if(!(ctl is TabPage || ctl is TabControl))//不起作用
                        {
                            ctl.Font = new Font(ctl.Font.Name, ps.FontSize * newy, ctl.Font.Style, ctl.Font.Unit);
                        }
                        

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

        
    }
}
