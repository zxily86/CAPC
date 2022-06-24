#define debug
using Beckhoff.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAPC.userForm;
using System.Configuration;
using BenXHSocket;
using CAPC.Config;
using System.Net;
using System.Threading;
using System.IO;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CAPC
{
    public partial class Form1 : Form
    {
        public static string serverPath;
        //Scara通讯
        public static string PLCams;
        private static string serverIP;
        private static int port;
        public static BenXHSocket.BXHTcpServer tcpServer;
        private bool TcpSerOn = false;
        public static bool HaveConnect;
        public static bool scara_con, godown;
        public static string scara_state, x, y, z, r, hand, laststate, CAPCmode;

        public static string IP_1;
        public static int Port_1;
        public static string userN;

        private string state_form = "";
        private TcAdsPlcServer _tcAdsPlcServer;

        Dictionary<int, string> _alarmMessage;
        Dictionary<int, string> _alarmMessage_new;
        Dictionary<int, string> _WarningMessage;
        List<bool> _plcWarning;
        List<bool> _plcErrorList;
        List<int> _indexarlarm;
        List<int> _indexarlarm_old;
        private string beginTime;
        public static bool key;
        private bool first = true;
        private DialogResult result;

        private Home _home;
        private IOView _io;
        private ManualForm _manual;
        private ParameterConfig _parameter;
        private Alarm _alarm;
        private UserRightForm _userright;
        //视觉字段
        private CCD_communication cCD_communication;
        private string ccdServeIP;
        private int ccdServePort;

        /// <summary>
        /// scara机器人发来的状态字符串，当状态有改变时才进行置位复位
        /// </summary>
        private string receiveScaraState = string.Empty;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public Form1()
        {
            InitializeComponent();

            try
            {
                timer1.Enabled = true;
                timer_err.Enabled = true;
                timer_err.Interval = 500;
                godown = false;

                serverPath = ConfigurationManager.AppSettings["ServerPath"];

                //Scara通讯
                PLCams = ConfigurationManager.AppSettings["PLCams"];
                serverIP = ConfigurationManager.AppSettings["ServerIP"];
                port = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);

                //视觉通信
                //ccdServeIP = ConfigurationManager.AppSettings["ccdServeIP"];
                //ccdServePort = int.Parse(ConfigurationManager.AppSettings["ccdServePort"]);
                
                cCD_communication = new CCD_communication(ccdServeIP, ccdServePort);
                cCD_communication.receiveAction += ccdReceive;

                //从配置文件中读取参数

                Control.CheckForIllegalCrossThreadCalls = false;
                //新线程允许访问UI控件

                if (tcpServer == null)
                {
                    Scara.Items.Add(string.Format("服务端监听程序尚未开启！{0}:{1}", serverIP, port));

                    BXHTcpServer.pushSockets = new PushSockets(Rev);
                    //pushSockets静态的委托, Rev：委托的实现方法

                    tcpServer = new BXHTcpServer();
                    //新建服务器对象
                }


                scara_state = "Null";
                x = "Null";
                y = "Null";
                z = "Null";
                r = "Null";


                _tcAdsPlcServer = new TcAdsPlcServer(801, PLCams);

                plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".SCARA_ready", false);

                #region 报警
                _alarmMessage = new Dictionary<int, string>();
                _alarmMessage_new = new Dictionary<int, string>();
                _plcWarning = new List<bool>();
                _plcErrorList = new List<bool>();
                _indexarlarm = new List<int>();
                _indexarlarm_old = new List<int>();
                lvWarnAndError.Columns.Add("代号", 50, HorizontalAlignment.Left);
                lvWarnAndError.Columns.Add("报警信息", 320, HorizontalAlignment.Left);
                beginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ");
                errorMassage();

                #endregion
                #region 获取屏幕分辨率
                int ScreenWidth = Screen.AllScreens[0].WorkingArea.Width;
                int Screenheight = Screen.AllScreens[0].WorkingArea.Height;//减去任务栏高度

                if (ScreenWidth < 1350)
                {
                    if (MessageBox.Show("屏幕分辨率异常，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        this.Close();
                    }
                }

                #endregion
                BackupParameters();
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        /// <summary>
        /// 取得当前源码的哪一行
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            try
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
                //经测试，在timer或者递归里快速多次调用，内存占用不上涨，如果内存上涨那是用了messagebox
                return st.GetFrame(0).GetFileLineNumber();
            }
            catch (Exception ex)
            {
                OutputDebugString($"{ex.Message}    {ex.StackTrace}");
                return int.MaxValue;
            }
        }

        /// <summary>
        /// 取当前源码的源文件名
        /// </summary>
        /// <returns></returns>
        public static string GetCurSourceFileName()
        {
            try
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
                string result = Regex.Match(st.GetFrame(0).GetFileName(), @"[^\\]+$").Value;
                return result;
            }
            catch (Exception ex)
            {
               
                return "";
            }
        }

        private void button_home_Click(object sender, EventArgs e)
        {
            if (state_form != "Home")
            {
                panel2.Controls.Clear();
                try
                {
                    if (state_form == "Home")
                    {
                        Application.OpenForms["Home"].Close();
                        _home = null;
                    }
                    if (state_form == "IOView")
                    {
                        //Application.OpenForms["IOView"].Dispose();
                        Application.OpenForms["IOView"].Close();//.Close()就会触发.Dispose()，实测不能降低内存占用
                        _io = null;
                    }

                    else if (state_form == "ManualForm")
                    {
                        Application.OpenForms["ManualForm"].Close();
                        _manual = null;
                    }

                    else if (state_form == "ParameterConfig")
                    {
                        Application.OpenForms["ParameterConfig"].Close();
                        _parameter = null;
                    }

                    else if (state_form == "Alarm")
                    {
                        Application.OpenForms["Alarm"].Close();
                        _alarm = null;
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

                if (Application.OpenForms["Home"] == null)
                {
                    _home = new Home();
                    _home.TopLevel = false;
                    _home.Show();
                    _home.Dock = DockStyle.Fill;
                    _home.Parent = panel2;
                    state_form = "Home";
                }
                else//有Name为FrmChildren的子船体，就直接show()
                {
                    //Application.OpenForms["Home"].TopLevel = false;
                    Application.OpenForms["Home"].Activate();
                    Application.OpenForms["Home"].Dock = DockStyle.Fill;
                    Application.OpenForms["Home"].Parent = panel2;
                    state_form = "Home";
                }
            }
            GC.Collect();
        }

        private void button_io_Click(object sender, EventArgs e)
        {
            if (state_form != "IOView")
            {
                panel2.Controls.Clear();
                try
                {
                    if (state_form == "Home")
                    {
                        Application.OpenForms["Home"].Close();
                        _home = null;
                    }
                    if (state_form == "IOView")
                    {
                        //Application.OpenForms["IOView"].Dispose();
                        Application.OpenForms["IOView"].Close();
                        _io = null;
                    }

                    else if (state_form == "ManualForm")
                    {
                        Application.OpenForms["ManualForm"].Close();
                        _manual = null;
                    }

                    else if (state_form == "ParameterConfig")
                    {
                        Application.OpenForms["ParameterConfig"].Close();
                        _parameter = null;
                    }

                    else if (state_form == "Alarm")
                    {
                        Application.OpenForms["Alarm"].Close();
                        _alarm = null;
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

                if (Application.OpenForms["IOView"] == null)
                {
                    _io = new IOView();
                    _io.TopLevel = false;
                    _io.Show();
                    _io.Parent = panel2;
                    _io.Dock = DockStyle.Fill;
                    state_form = "IOView";
                }
                else//有Name为FrmChildren的子船体，就直接show()
                {
                    //Application.OpenForms["IOView"].TopLevel = false;
                    Application.OpenForms["IOView"].Activate();
                    Application.OpenForms["IOView"].Parent = panel2;
                    Application.OpenForms["IOView"].Dock = DockStyle.Fill;
                    state_form = "IOView";
                }
            }
            GC.Collect();
        }

        private void button_manual_Click(object sender, EventArgs e)
        {
            if (result == DialogResult.OK)
            {
                if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".MState_Starting")))
                    MessageBox.Show("设备运行中，请停机后再进入");
                else
                {
                    if (state_form != "ManualForm")
                    {
                        panel2.Controls.Clear();
                        try
                        {
                            if (state_form == "Home")
                            {
                                Application.OpenForms["Home"].Close();
                                _home = null;
                            }
                            if (state_form == "IOView")
                            {
                                //Application.OpenForms["IOView"].Dispose();
                                Application.OpenForms["IOView"].Close();
                                _io = null;
                            }

                            else if (state_form == "ManualForm")
                            {
                                Application.OpenForms["ManualForm"].Close();
                                _manual = null;
                            }

                            else if (state_form == "ParameterConfig")
                            {
                                Application.OpenForms["ParameterConfig"].Close();
                                _parameter = null;
                            }

                            else if (state_form == "Alarm")
                            {
                                Application.OpenForms["Alarm"].Close();
                                _alarm = null;
                            }
                        }
                        catch (Exception ex)
                        { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

                        if (Application.OpenForms["ManualForm"] == null)
                        {
                            _manual = new ManualForm();
                            _manual.send_To_CCD += send_to_CCD;
                            _manual.TopLevel = false;
                            _manual.Show();
                            _manual.Parent = panel2;
                            _manual.Dock = DockStyle.Fill;
                            state_form = "ManualForm";
                        }
                        else//有Name为FrmChildren的子船体，就直接show()
                        {
                            //Application.OpenForms["ManualForm"].TopLevel = false;
                            Application.OpenForms["ManualForm"].Activate();
                            Application.OpenForms["ManualForm"].Parent = panel2;
                            Application.OpenForms["ManualForm"].Dock = DockStyle.Fill;
                            state_form = "ManualForm";
                        }
                    }
                }
            }
            else
                MessageBox.Show("权限不足，请重新操作");

            GC.Collect();
        }

        private void button_para_Click(object sender, EventArgs e)
        {
            if (result == DialogResult.OK)
            {
                if (state_form != "ParameterConfig")
                {
                    panel2.Controls.Clear();
                    try
                    {
                        int a = 0;
                        if (state_form == "Home")
                        {
                            Application.OpenForms["Home"].Close();
                            _home = null;
                        }
                        if (state_form == "IOView")
                        {
                            //Application.OpenForms["IOView"].Dispose();
                            Application.OpenForms["IOView"].Close();
                            _io = null;
                        }

                        else if (state_form == "ManualForm")
                        {
                            Application.OpenForms["ManualForm"].Close();
                            _manual = null;
                        }

                        else if (state_form == "ParameterConfig")
                        {
                            Application.OpenForms["ParameterConfig"].Close();
                            _parameter = null;
                        }

                        else if (state_form == "Alarm")
                        {
                            Application.OpenForms["Alarm"].Close();
                            _alarm = null;
                        }
                    }
                    catch(Exception ex)
                   { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }   

                    if (Application.OpenForms["ParameterConfig"] == null)
                    {
                        _parameter = new ParameterConfig();
                        _parameter.TopLevel = false;
                        _parameter.Show();
                        _parameter.Parent = panel2;
                        _parameter.Dock = DockStyle.Fill;
                        state_form = "ParameterConfig";
                    }
                    else//有Name为FrmChildren的子船体，就直接show()
                    {
                        //Application.OpenForms["ParameterConfig"].TopLevel = false;
                        Application.OpenForms["ParameterConfig"].Activate();
                        Application.OpenForms["ParameterConfig"].Parent = panel2;
                        Application.OpenForms["ParameterConfig"].Dock = DockStyle.Fill;
                        state_form = "ParameterConfig";
                    }
                }
            }
            else
                MessageBox.Show("权限不足，请重新操作");
            GC.Collect();
        }

        private void button_data_Click(object sender, EventArgs e)
        {
            //if (File.Exists(serverPath))
            //    System.Diagnostics.Process.Start(serverPath);
            //else
            //    MessageBox.Show("该程序不存在", "警告");
        }

        private void button_err_Click(object sender, EventArgs e)
        {
            if (state_form != "Alarm")
            {
                panel2.Controls.Clear();
                try
                {
                    if (state_form == "Home")
                    {
                        Application.OpenForms["Home"].Close();
                        _home = null;
                    }
                    if (state_form == "IOView")
                    {
                        //Application.OpenForms["IOView"].Dispose();
                        Application.OpenForms["IOView"].Close();
                        _io = null;
                    }

                    else if (state_form == "ManualForm")
                    {
                        Application.OpenForms["ManualForm"].Close();
                        _manual = null;
                    }

                    else if (state_form == "ParameterConfig")
                    {
                        Application.OpenForms["ParameterConfig"].Close();
                        _parameter = null;
                    }

                    else if (state_form == "Alarm")
                    {
                        Application.OpenForms["Alarm"].Close();
                        _alarm = null;
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                if (Application.OpenForms["Alarm"] == null)
                {
                    _alarm = new Alarm();
                    _alarm.TopLevel = false;
                    _alarm.Show();
                    _alarm.Parent = panel2;
                    _alarm.Dock = DockStyle.Fill;
                    state_form = "Alarm";
                }
                else//有Name为FrmChildren的子船体，就直接show()
                {
                    //Application.OpenForms["Alarm"].TopLevel = false;
                    Application.OpenForms["Alarm"].Parent = panel2;
                    Application.OpenForms["Alarm"].Dock = DockStyle.Fill;
                    Application.OpenForms["Alarm"].Activate();
                    state_form = "Alarm";
                }
            }
            GC.Collect();
        }

        private void button_user_Click(object sender, EventArgs e)
        {
            _userright = new UserRightForm();
            result = _userright.ShowDialog();
            _userright = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_tcAdsPlcServer.PlcIsRunning)
            {
                if (first)
                {
                    button_home_Click(null, EventArgs.Empty);
                    this.WindowState = FormWindowState.Maximized;
                    this.MaximizeBox = false;
                    this.MinimizeBox = false;
                    if (Application.OpenForms["ParameterConfig"] == null)
                    {
                        _parameter = new ParameterConfig();
                        _parameter.TopLevel = false;
                        _parameter.Show(); //OutputDebugString显示程序运行到此，但没有窗体显示，_parameter.Parent = panel2;没有导致
                    }
                    else//有Name为FrmChildren的子船体，就直接show()
                    {
                        Application.OpenForms["ParameterConfig"].Activate();
                    }
                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".HMI_Run", true);
                    first = false;
                    //if (File.Exists(serverPath))
                    //    System.Diagnostics.Process.Start(serverPath);
                }

                try
                {
                    int i = Convert.ToInt16(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".MState"));
                    if (i == 0)
                        machinestate.Text = "设备未准备好，请复位";
                    else if (i == 1)
                        machinestate.Text = "设备已经准备好，可启动";
                    else if (i == 2)
                        machinestate.Text = "设备正在复位中，请等待";
                    else if (i == 3)
                        machinestate.Text = "设备运行中";
                    else if (i == 4)
                        machinestate.Text = "设备正在停止中，请等待";
                    else if (i == 5)
                        machinestate.Text = "请根据提示处理故障";
                    else if (i == 6)
                        machinestate.Text = "设备自动运行中，但部分模块故障，请根据提示处理";
                    else if (i == 7)
                        machinestate.Text = "设备未回原点，请回原点";
                    else if (i == 8)
                        machinestate.Text = "设备自动运行中，部分模块已准备好，请启动";
                    else if (i == 9)
                        machinestate.Text = "设备自动运行中，正在复位中，请稍候";
                    else
                        machinestate.Text = "设备未准备好，请复位";

                    switch(i)
                    {
                        case 0:
                        case 2:
                        case 5:
                        case 6:
                        case 7:
                            machinestate.ForeColor = Color.Red;
                            break;
                        default:
                            machinestate.ForeColor = Color.Black;
                            break;

                    }

                    //CAPCmode = Convert.ToString(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ActMode"));

                    //if (!Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut1")) &&
                    //    !Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut2")) &&
                    //    !Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut3")) &&
                    //    !Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut4")) &&
                    //    Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".MState_Starting")) && godown)
                    //{
                    //    listBox1.Items.Insert(0, string.Format("{0}:下料端走料", DateTime.Now.ToString()));
                    //    godown = false;
                    //}
                    //if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut1")) ||
                    //   Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut2")) ||
                    //   Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut3")) ||
                    //   Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".ReadyForPut4")))
                    //    godown = true;

                    if (listBox1.Items.Count > 10)
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

            }
            else
            {
                machinestate.ForeColor = Color.Red;
                machinestate.Text = "PLC通讯错误";
            }
                

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (key)
                    user.Text = "管理员";
                else
                    user.Text = "操作人员";
            }
            else
                user.Text = "未登录";

            clock.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        }
        private void timer_err_Tick(object sender, EventArgs e)
        {
            if (_tcAdsPlcServer.PlcIsRunning)
            {
                try
                {//勿忘清空各大LIST
                    #region 从PLC读出现有设备状态装载到集合中
                    _plcErrorList.Clear();
                    //
                    object list = plcParameterOp.ReadArray(_tcAdsPlcServer, ".Err", 599);
                    _plcErrorList = ((bool[])list).ToList();

                    //把现有报警信息装载到集合中
                    _indexarlarm.Clear();
                    int index = 0;
                    foreach (bool b in _plcErrorList)
                    {
                        if (b)
                        {
                            _indexarlarm.Add(index);
                        }
                        index++;
                    }
                    #endregion


                    #region debug输出不在添加目录里的报警代码
                    //foreach (int errNum in _indexarlarm) //查不在添加目录时启用
                    //{
                    //    try
                    //    {
                    //        string _alarmString = _alarmMessage[errNum];
                    //    }
                    //    catch { OutputDebugString($"未知报警代码{errNum}"); };

                    //}
                    //OutputDebugString("************************************************************************");
                    #endregion


                    #region 与上一次扫描比较得到新增报警集合
                    List<int> _indexadd = new List<int>();
                    _indexadd.Clear();
                    foreach (int i in _indexarlarm)
                    {
                        _indexadd.Add(i);
                        foreach (int ii in _indexarlarm_old)
                        {
                            if (i == ii)
                            {
                                _indexadd.Remove(i);
                            }
                        }
                    }
                    #endregion

                    #region 与上一次扫描比较得到减少报警集合
                    List<int> _indexsub = new List<int>();
                    _indexsub.Clear();
                    foreach (int i in _indexarlarm_old)
                    {
                        _indexsub.Add(i);
                        foreach (int ii in _indexarlarm)
                        {
                            if (i == ii)
                            {
                                _indexsub.Remove(i);
                            }
                        }
                    }
                    #endregion

                    #region 更新上一次扫描的集合
                    foreach (int i in _indexadd)
                    {
                        _indexarlarm_old.Add(i);//将新增的加到LIST后面，然后LISTVIEW反向插入，可将新增的显示在上面
                    }
                    foreach (int i in _indexsub)
                    {
                        _indexarlarm_old.Remove(i);
                    }
                    #endregion

                    #region 把当前报警打印到界面

                    if (_indexadd.Count > 0 || _indexsub.Count > 0)
                    {
                        lvWarnAndError.Items.Clear();
                        foreach (int i in _indexarlarm_old)
                        {
                            
                            try
                            {
                                ListViewItem item = lvWarnAndError.Items.Insert(0, i.ToString());
                                item.SubItems.Add(_alarmMessage[i]);//报警代码不在Dictionary里会报错转到catch导致txt没有
                            }
                            catch (Exception et)
                            {
                                ListViewItem item = lvWarnAndError.Items.Insert(0, $"{9999}");
                                item.SubItems.Add("异常!!!" + $" {i} "+et.Message);//将异常输出到当前报警的ListView
                            }
                        }
                    }
                    #endregion

                    #region 把新增的报警记录到文档
                    foreach (int i in _indexadd)
                    {
                        
                        try
                        {
                            string strlogDate = DateTime.Now.ToString("yyyy-MM-dd");
                            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\LogFile" + "\\LogFile" + strlogDate + ".txt", true))
                            {
                                if(!_alarmMessage[i].Contains("警告"))
                                {
                                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + _alarmMessage[i]);//每次启动时都会记录一次全部存在的报警到txt
                                }
                            }
                        }
                        catch (Exception et)
                        {
                            ListViewItem item = lvWarnAndError.Items.Insert(0, $"{9999}");
                            item.SubItems.Add("异常!!!" + $" {i} " + et.Message);//将异常输出到当前报警的ListView
                        }
                    }
                    #endregion
                }
                catch (Exception et)
                {
                    ListViewItem item = lvWarnAndError.Items.Insert(0, $"{9999}");
                    item.SubItems.Add("异常!!!" + et.Message);//将异常输出到当前报警的ListView
                }
            }
        }

        private void errorMassage()
        {
            #region
            _alarmMessage.Add(1, "MCP4倍福模异常");
            _alarmMessage.Add(2, "MCP6倍福模异常");
            _alarmMessage.Add(3, "MCP7倍福模异常");
            _alarmMessage.Add(4, "CU2508交换机异常");
            _alarmMessage.Add(5, "FESTO异常");
            _alarmMessage.Add(6, "未定义");
            _alarmMessage.Add(7, "未定义");
            _alarmMessage.Add(8, "未定义");
            _alarmMessage.Add(9, "未定义");
            _alarmMessage.Add(10, "上料顶升伺服离线");
            _alarmMessage.Add(11, "上料旋转伺服离线");
            _alarmMessage.Add(12, "1#传输伺服离线");
            _alarmMessage.Add(13, "2#传输伺服离线");
            _alarmMessage.Add(14, "3#传输伺服离线");
            _alarmMessage.Add(15, "4#传输伺服离线");
            _alarmMessage.Add(16, "5#传输伺服离线");
            _alarmMessage.Add(17, "6#传输伺服离线");
            _alarmMessage.Add(18, "R2R-1伺服离线");
            _alarmMessage.Add(19, "三嘴涂胶X轴伺服离线");
            _alarmMessage.Add(20, "冲压机构伺服离线");
            _alarmMessage.Add(21, "四嘴涂胶X轴伺服离线");
            _alarmMessage.Add(22, "四嘴涂胶Y轴伺服离线");
            _alarmMessage.Add(23, "Memrance伺服离线");
            _alarmMessage.Add(24, "旋转贴合伺服离线");
            _alarmMessage.Add(25, "R2R-2伺服离线");
            _alarmMessage.Add(26, "三嘴涂胶Y轴伺服离线");
            _alarmMessage.Add(27, "DD伺服离线");
            _alarmMessage.Add(28, "下料X伺服离线");
            _alarmMessage.Add(29, "下料Y伺服离线");
            _alarmMessage.Add(30, "下料内部升降步进离线");
            _alarmMessage.Add(31, "下料外部升降步进离线");
            _alarmMessage.Add(32, "下料传输步进离线");
            _alarmMessage.Add(33, "上料外部升降步进离线");
            _alarmMessage.Add(34, "上料内部升降步进离线");
            _alarmMessage.Add(35, "上料传输步进离线");
            _alarmMessage.Add(36, "SCARA未连接");
            _alarmMessage.Add(37, "未定义");
            _alarmMessage.Add(38, "未定义");
            _alarmMessage.Add(39, "未定义");
            _alarmMessage.Add(40, "上料顶升伺服驱动异常");
            _alarmMessage.Add(41, "上料旋转伺服驱动异常");
            _alarmMessage.Add(42, "1#传输伺服驱动异常");
            _alarmMessage.Add(43, "2#传输伺服驱动异常");
            _alarmMessage.Add(44, "3#传输伺服驱动异常");
            _alarmMessage.Add(45, "4#传输伺服驱动异常");
            _alarmMessage.Add(46, "5#传输伺服驱动异常");
            _alarmMessage.Add(47, "6#传输伺服驱动异常");
            _alarmMessage.Add(48, "R2R-1伺服驱动异常");
            _alarmMessage.Add(49, "三嘴涂胶X轴伺服驱动异常");
            _alarmMessage.Add(50, "冲压机构伺服驱动异常");
            _alarmMessage.Add(51, "四嘴涂胶X轴伺服驱动异常");
            _alarmMessage.Add(52, "四嘴涂胶Y轴伺服驱动异常");
            _alarmMessage.Add(53, "Memrance伺服驱动异常");
            _alarmMessage.Add(54, "旋转贴合伺服驱动异常");
            _alarmMessage.Add(55, "R2R-2伺服驱动异常");
            _alarmMessage.Add(56, "三嘴涂胶Y轴伺服驱动异常");
            _alarmMessage.Add(57, "DD伺服驱动异常");
            _alarmMessage.Add(58, "下料X伺服驱动异常");
            _alarmMessage.Add(59, "下料Y伺服驱动异常");
            _alarmMessage.Add(60, "下料内部升降步进驱动异常");
            _alarmMessage.Add(61, "下料外部升降步进驱动异常");
            _alarmMessage.Add(62, "下料传输步进驱动异常");
            _alarmMessage.Add(63, "上料外部升降步进驱动异常");
            _alarmMessage.Add(64, "上料内部升降步进驱动异常");
            _alarmMessage.Add(65, "上料传输步进驱动异常");
            _alarmMessage.Add(66, "1#卷对卷变频器1异常");
            _alarmMessage.Add(67, "1#卷对卷变频器2异常");
            _alarmMessage.Add(68, "冲压变频器异常");
            _alarmMessage.Add(69, "Memrance变频器异常");
            _alarmMessage.Add(70, "撕膜变频器异常");
            _alarmMessage.Add(71, "2#卷对卷变频器1异常");
            _alarmMessage.Add(72, "2#卷对卷变频器2异常");
            _alarmMessage.Add(73, "未定义");
            _alarmMessage.Add(74, "未定义");

            _alarmMessage.Add(75, "驱动断电故障");
            _alarmMessage.Add(76, "气源压力低");
            _alarmMessage.Add(77, "1#安全门打开");
            _alarmMessage.Add(78, "2#安全门打开");
            _alarmMessage.Add(79, "3#安全门打开");
            _alarmMessage.Add(80, "4#安全门打开");
            _alarmMessage.Add(81, "5#安全门打开");
            _alarmMessage.Add(82, "急停按钮摁下");
            //////////////////
            _alarmMessage.Add(100, "上料顶升伺服复位异常");
            _alarmMessage.Add(101, "上料旋转伺服复位异常");
            _alarmMessage.Add(102, "三嘴涂胶X伺服复位异常");
            _alarmMessage.Add(103, "三嘴涂胶Y伺服复位异常");
            _alarmMessage.Add(104, "四嘴涂胶X伺服复位异常");
            _alarmMessage.Add(105, "四嘴涂胶Y伺服复位异常");
            _alarmMessage.Add(106, "旋转贴合伺服复位异常");
            _alarmMessage.Add(107, "DD转盘复位异常");
            _alarmMessage.Add(108, "下料机构X伺服复位异常");
            _alarmMessage.Add(109, "下料机构Y伺服复位异常");
            _alarmMessage.Add(110, "下料内部升降步进复位异常");
            _alarmMessage.Add(111, "下料外部升降步进复位异常");
            _alarmMessage.Add(112, "下料传输步进复位异常");
            _alarmMessage.Add(113, "上料外部升降步进复位异常");
            _alarmMessage.Add(114, "上料内部升降步进复位异常");
            _alarmMessage.Add(115, "上料传输步进复位异常");
            _alarmMessage.Add(116, "SCARA故障");
            _alarmMessage.Add(117, "未定义");
            _alarmMessage.Add(118, "未定义");
            _alarmMessage.Add(119, "未定义");
            _alarmMessage.Add(120, "未定义");
            _alarmMessage.Add(121, "未定义");
            _alarmMessage.Add(122, "未定义");
            _alarmMessage.Add(123, "未定义");
            _alarmMessage.Add(124, "未定义");
            _alarmMessage.Add(125, "未定义");
            _alarmMessage.Add(126, "未定义");
            _alarmMessage.Add(127, "未定义");
            _alarmMessage.Add(128, "未定义");
            _alarmMessage.Add(129, "未定义");
            _alarmMessage.Add(130, "未定义");
            _alarmMessage.Add(131, "未定义");
            _alarmMessage.Add(132, "未定义");
            _alarmMessage.Add(133, "未定义");
            _alarmMessage.Add(134, "未定义");
            _alarmMessage.Add(135, "未定义");
            _alarmMessage.Add(136, "未定义");
            _alarmMessage.Add(137, "未定义");
            _alarmMessage.Add(138, "未定义");
            _alarmMessage.Add(139, "未定义");
            ///////////////
            _alarmMessage.Add(140, "上料顶升伺服正限");
            _alarmMessage.Add(141, "上料顶升伺服反限");
            _alarmMessage.Add(142, "三嘴涂胶X轴正限");
            _alarmMessage.Add(143, "三嘴涂胶X轴反限");
            _alarmMessage.Add(144, "三嘴涂胶Y轴正限");
            _alarmMessage.Add(145, "三嘴涂胶Y轴反限");
            _alarmMessage.Add(146, "四嘴涂胶X轴正限");
            _alarmMessage.Add(147, "四嘴涂胶X轴反限");
            _alarmMessage.Add(148, "四嘴涂胶Y轴正限");
            _alarmMessage.Add(149, "四嘴涂胶Y轴反限");
            _alarmMessage.Add(150, "旋转贴合正限");
            _alarmMessage.Add(151, "旋转贴合反限");
            _alarmMessage.Add(152, "下料机构X轴正限");
            _alarmMessage.Add(153, "下料机构X轴反限");
            _alarmMessage.Add(154, "下料机构Y轴正限");
            _alarmMessage.Add(155, "下料机构Y轴反限");
            _alarmMessage.Add(156, "下料内部升降步进正限");
            _alarmMessage.Add(157, "下料内部升降步进反限");
            _alarmMessage.Add(158, "下料外部升降步进正限");
            _alarmMessage.Add(159, "下料外部升降步进反限");
            _alarmMessage.Add(160, "下料传输步进正限");
            _alarmMessage.Add(161, "下料传输步进反限");
            _alarmMessage.Add(162, "上料外部升降步进正限");
            _alarmMessage.Add(163, "上料外部升降步进反限");
            _alarmMessage.Add(164, "上料内部升降步进正限");
            _alarmMessage.Add(165, "上料内部升降步进反限");
            _alarmMessage.Add(166, "上料传输步进正限");
            _alarmMessage.Add(167, "上料传输步进反限");
            _alarmMessage.Add(168, "未定义");
            _alarmMessage.Add(169, "未定义");
            _alarmMessage.Add(170, "未定义");
            _alarmMessage.Add(171, "未定义");
            _alarmMessage.Add(172, "未定义");
            _alarmMessage.Add(173, "未定义");
            _alarmMessage.Add(174, "未定义");
            _alarmMessage.Add(175, "未定义");
            _alarmMessage.Add(176, "未定义");
            _alarmMessage.Add(177, "未定义");
            _alarmMessage.Add(178, "未定义");
            _alarmMessage.Add(179, "未定义");
            ////////
            _alarmMessage.Add(180, "1#转盘上料Y气缸回异常");
            _alarmMessage.Add(181, "1#转盘上料Y气缸出异常");
            _alarmMessage.Add(182, "1#转盘上料Z气缸回异常");
            _alarmMessage.Add(183, "1#转盘上料Z气缸出异常");
            _alarmMessage.Add(184, "1#贴膜定位X1气缸回异常");
            _alarmMessage.Add(185, "1#贴膜定位X1气缸出异常");
            _alarmMessage.Add(186, "1#贴膜定位Y1气缸回异常");
            _alarmMessage.Add(187, "1#贴膜定位Y1气缸出异常");
            _alarmMessage.Add(188, "1#贴膜定位Z2机构顶升气缸回异常");
            _alarmMessage.Add(189, "1#贴膜定位Z2机构顶升气缸出异常");
            _alarmMessage.Add(190, "1#贴膜定位Y2气缸回异常");
            _alarmMessage.Add(191, "1#贴膜定位Y2气缸出异常");
            _alarmMessage.Add(192, "1#贴膜定位Z1-PCB顶升气缸回异常");
            _alarmMessage.Add(193, "1#贴膜定位Z1-PCB顶升气缸出异常");
            _alarmMessage.Add(194, "1#贴膜定位X2气缸回异常");
            _alarmMessage.Add(195, "1#贴膜定位X2气缸出异常");
            _alarmMessage.Add(196, "1#贴膜横移X气缸回异常");
            _alarmMessage.Add(197, "1#贴膜横移X气缸出异常");
            _alarmMessage.Add(198, "1#贴膜排废旋转气缸回异常");
            _alarmMessage.Add(199, "1#贴膜排废旋转气缸出异常");
            _alarmMessage.Add(200, "1#贴膜排废顶升Z气缸回异常");
            _alarmMessage.Add(201, "1#贴膜排废顶升Z气缸出异常");
            _alarmMessage.Add(202, "1#贴膜压膜Z气缸回异常");
            _alarmMessage.Add(203, "1#贴膜压膜Z气缸出异常");
            _alarmMessage.Add(204, "1#冲压定位X1气缸回异常");
            _alarmMessage.Add(205, "1#冲压定位X1气缸出异常");
            _alarmMessage.Add(206, "1#冲压定位Y1气缸回异常");
            _alarmMessage.Add(207, "1#冲压定位Y1气缸出异常");
            _alarmMessage.Add(208, "1#冲压定位Z2机构顶升气缸回异常");
            _alarmMessage.Add(209, "1#冲压定位Z2机构顶升气缸出异常");
            _alarmMessage.Add(210, "1#冲压定位Y2气缸回异常");
            _alarmMessage.Add(211, "1#冲压定位Y2气缸出异常");
            _alarmMessage.Add(212, "1#冲压定位Z1-PCB顶升气缸回异常");
            _alarmMessage.Add(213, "1#冲压定位Z1-PCB顶升气缸出异常");
            _alarmMessage.Add(214, "1#冲压定位X2气缸回异常");
            _alarmMessage.Add(215, "1#冲压定位X2气缸出异常");
            _alarmMessage.Add(216, "1#冲压气缸回异常");
            _alarmMessage.Add(217, "1#冲压气缸出异常");
            _alarmMessage.Add(218, "1#点胶定位Z1机构顶升气缸回异常");
            _alarmMessage.Add(219, "1#点胶定位Z1机构顶升气缸出异常");
            _alarmMessage.Add(220, "1#点胶定位Y1气缸回异常");
            _alarmMessage.Add(221, "1#点胶定位Y1气缸出异常");
            _alarmMessage.Add(222, "1#点胶定位X1气缸回异常");
            _alarmMessage.Add(223, "1#点胶定位X1气缸出异常");
            _alarmMessage.Add(224, "1#点胶定位Y2气缸回异常");
            _alarmMessage.Add(225, "1#点胶定位Y2气缸出异常");
            _alarmMessage.Add(226, "1#点胶定位X2气缸回异常");
            _alarmMessage.Add(227, "1#点胶定位X2气缸出异常");
            _alarmMessage.Add(228, "2#三嘴点胶头Z1气缸回异常");
            _alarmMessage.Add(229, "2#三嘴点胶头Z1气缸出异常");
            _alarmMessage.Add(230, "2#四嘴点胶头Z2气缸回异常");
            _alarmMessage.Add(231, "2#四嘴点胶头Z2气缸出异常");
            _alarmMessage.Add(232, "2#检测定位Y2气缸回异常");
            _alarmMessage.Add(233, "2#检测定位Y2气缸出异常");
            _alarmMessage.Add(234, "2#检测定位X气缸回异常");
            _alarmMessage.Add(235, "2#检测定位X气缸出异常");
            _alarmMessage.Add(236, "2#检测定位Z气缸回异常");
            _alarmMessage.Add(237, "2#检测定位Z气缸出异常");
            _alarmMessage.Add(238, "2#检测定位Y1气缸回异常");
            _alarmMessage.Add(239, "2#检测定位Y1气缸出异常");
            _alarmMessage.Add(240, "2#检测顶升Z气缸回异常");
            _alarmMessage.Add(241, "2#检测顶升Z气缸出异常");
            _alarmMessage.Add(242, "2#NG剔除X气缸回异常");
            _alarmMessage.Add(243, "2#NG剔除X气缸出异常");
            _alarmMessage.Add(244, "2#NG剔除Z气缸回异常");
            _alarmMessage.Add(245, "2#NG剔除Z气缸出异常");
            _alarmMessage.Add(246, "2#NG剔除Y气缸回异常");
            _alarmMessage.Add(247, "2#NG剔除Y气缸出异常");
            _alarmMessage.Add(248, "2#贴膜横移Y气缸回异常");
            _alarmMessage.Add(249, "2#贴膜横移Y气缸出异常");
            _alarmMessage.Add(250, "2#贴膜压膜Z气缸回异常");
            _alarmMessage.Add(251, "2#贴膜压膜Z气缸出异常");
            _alarmMessage.Add(252, "2#贴膜定位X2气缸回异常");
            _alarmMessage.Add(253, "2#贴膜定位X2气缸出异常");
            _alarmMessage.Add(254, "2#贴膜定位Y1气缸回异常");
            _alarmMessage.Add(255, "2#贴膜定位Y1气缸出异常");
            _alarmMessage.Add(256, "2#贴膜定位Z2机构顶升气缸回异常");
            _alarmMessage.Add(257, "2#贴膜定位Z2机构顶升气缸出异常");
            _alarmMessage.Add(258, "2#贴膜定位Y2气缸回异常");
            _alarmMessage.Add(259, "2#贴膜定位Y2气缸出异常");
            _alarmMessage.Add(260, "2#贴膜定位X1气缸回异常");
            _alarmMessage.Add(261, "2#贴膜定位X1气缸出异常");
            _alarmMessage.Add(262, "2#贴膜定位Z1-PCB顶升气缸回异常");
            _alarmMessage.Add(263, "2#贴膜定位Z1-PCB顶升气缸出异常");
            _alarmMessage.Add(264, "2#撕膜顶升Z-PCB顶升气缸回异常");
            _alarmMessage.Add(265, "2#撕膜顶升Z-PCB顶升气缸出异常");
            _alarmMessage.Add(266, "2#撕膜横移Y气缸回异常");
            _alarmMessage.Add(267, "2#撕膜横移Y气缸出异常");
            _alarmMessage.Add(268, "2#翻转定位Z气缸回异常");
            _alarmMessage.Add(269, "2#翻转定位Z气缸出异常");
            _alarmMessage.Add(270, "2#翻转定位X气缸回异常");
            _alarmMessage.Add(271, "2#翻转定位X气缸出异常");
            _alarmMessage.Add(272, "2#翻转定位Y1气缸回异常");
            _alarmMessage.Add(273, "2#翻转定位Y1气缸出异常");
            _alarmMessage.Add(274, "2#翻转定位Y2气缸回异常");
            _alarmMessage.Add(275, "2#翻转定位Y2气缸出异常");
            _alarmMessage.Add(276, "2#贴膜排废Z气缸回异常");
            _alarmMessage.Add(277, "2#贴膜排废Z气缸出异常");
            _alarmMessage.Add(278, "3#转盘工位5校准Y2气缸回异常");
            _alarmMessage.Add(279, "3#转盘工位5校准Y2气缸出异常");
            _alarmMessage.Add(280, "3#转盘工位5校准X2气缸回异常");
            _alarmMessage.Add(281, "3#转盘工位5校准X2气缸出异常");
            _alarmMessage.Add(282, "3#贴膜横移X气缸回异常");
            _alarmMessage.Add(283, "3#贴膜横移X气缸出异常");
            _alarmMessage.Add(284, "转盘静压垂直取片气缸回异常");
            _alarmMessage.Add(285, "转盘静压垂直取片气缸出异常");
            _alarmMessage.Add(286, "转盘静压横移气缸回异常");
            _alarmMessage.Add(287, "转盘静压横移气缸出异常");
            _alarmMessage.Add(288, "3#贴膜压膜Z气缸回异常");
            _alarmMessage.Add(289, "3#贴膜压膜Z气缸出异常");
            _alarmMessage.Add(290, "3#转盘工位2校准Y1气缸回异常");
            _alarmMessage.Add(291, "3#转盘工位2校准Y1气缸出异常");
            _alarmMessage.Add(292, "3#转盘工位2校准X1气缸回异常");
            _alarmMessage.Add(293, "3#转盘工位2校准X1气缸出异常");
            _alarmMessage.Add(294, "3#贴带压带Z气缸回异常");
            _alarmMessage.Add(295, "3#贴带压带Z气缸出异常");
            _alarmMessage.Add(296, "3#贴带横移X气缸回异常");
            _alarmMessage.Add(297, "3#贴带横移X气缸出异常");
            _alarmMessage.Add(298, "3#贴带剪刀Z气缸回异常");
            _alarmMessage.Add(299, "3#贴带剪刀Z气缸出异常");
            _alarmMessage.Add(300, "3#贴带压轮Z气缸回异常");
            _alarmMessage.Add(301, "3#贴带压轮Z气缸出异常");
            _alarmMessage.Add(302, "3#贴带送料X气缸回异常");
            _alarmMessage.Add(303, "3#贴带送料X气缸出异常");
            _alarmMessage.Add(304, "3#压合Z1静压气缸回异常");
            _alarmMessage.Add(305, "3#压合Z1静压气缸出异常");
            _alarmMessage.Add(306, "3#压合Z2支撑气缸回异常");
            _alarmMessage.Add(307, "3#压合Z2支撑气缸出异常");
            _alarmMessage.Add(308, "3#贴膜顶升吸附Z气缸回异常");
            _alarmMessage.Add(309, "3#贴膜顶升吸附Z气缸出异常");
            _alarmMessage.Add(310, "4#上料托盘位移X1气缸回异常");
            _alarmMessage.Add(311, "4#上料托盘位移X1气缸出异常");
            _alarmMessage.Add(312, "4#上料托盘位移Z1气缸回异常");
            _alarmMessage.Add(313, "4#上料托盘位移Z1气缸出异常");
            _alarmMessage.Add(314, "4#下料托盘位移Z2气缸回异常");
            _alarmMessage.Add(315, "4#下料托盘位移Z2气缸出异常");
            _alarmMessage.Add(316, "4#下料托盘位移X2气缸回异常");
            _alarmMessage.Add(317, "4#下料托盘位移X2气缸出异常");
            _alarmMessage.Add(318, "4#下料模组Z气缸回异常");
            _alarmMessage.Add(319, "4#下料模组Z气缸出异常");
            _alarmMessage.Add(320, "VI3_V23 回异常");
            _alarmMessage.Add(321, "VI3_V23 出异常");
            _alarmMessage.Add(322, "VI3_V24 回异常");
            _alarmMessage.Add(323, "VI3_V24 出异常");
            _alarmMessage.Add(324, "未定义");
            ////////////
            _alarmMessage.Add(340, "上料吸附真空吸附异常");
            _alarmMessage.Add(341, "1#贴膜校准吸附真空吸附异常");
            _alarmMessage.Add(342, "1#冲压校准吸附真空吸附异常");
            _alarmMessage.Add(343, "转盘静压取片真空吸附异常");
            _alarmMessage.Add(344, "1#贴膜排废吸附真空吸附异常");
            _alarmMessage.Add(345, "1#贴膜排废吹气真空吸附异常");
            _alarmMessage.Add(346, "未定义");
            _alarmMessage.Add(347, "未定义");
            _alarmMessage.Add(348, "未定义");
            _alarmMessage.Add(349, "转盘吸附真空吸附异常1");
            _alarmMessage.Add(350, "转盘吸附真空吸附异常2");
            _alarmMessage.Add(351, "转盘吸附真空吸附异常3");
            _alarmMessage.Add(352, "转盘吸附真空吸附异常4");
            _alarmMessage.Add(353, "转盘吸附真空吸附异常5");
            _alarmMessage.Add(354, "转盘吸附真空吸附异常6");
            _alarmMessage.Add(355, "转盘吸附真空吸附异常7");
            _alarmMessage.Add(356, "转盘吸附真空吸附异常8");
            _alarmMessage.Add(357, "转盘吸附真空吸附异常9");
            _alarmMessage.Add(358, "转盘吸附真空吸附异常10");
            _alarmMessage.Add(359, "转盘吸附真空吸附异常11");
            _alarmMessage.Add(360, "转盘吸附真空吸附异常12");
            _alarmMessage.Add(361, "2#贴膜排废吹气异常");
            _alarmMessage.Add(362, "2#贴膜定位真空吸附异常");
            _alarmMessage.Add(363, "2#撕膜定位吸附真空吸附异常");
            _alarmMessage.Add(364, "2#贴膜排废真空吸附异常");
            _alarmMessage.Add(365, "3#贴膜顶升吸附真空吸附异常");
            _alarmMessage.Add(366, "SCARA吸附真空吸附异常");
            _alarmMessage.Add(367, "旋转贴合吸附真空吸附异常");
            _alarmMessage.Add(368, "3#贴膜排废吸附真空吸附异常");
            _alarmMessage.Add(369, "4#下料模组吸附真空吸附异常");
            _alarmMessage.Add(370, "4#上料托盘吸附真空吸附异常");
            _alarmMessage.Add(371, "4#下料托盘吸附真空吸附异常");
            _alarmMessage.Add(372, "转盘工位7静压故障");
            _alarmMessage.Add(373, "未定义");
            _alarmMessage.Add(374, "未定义");
            _alarmMessage.Add(375, "未定义");
            _alarmMessage.Add(376, "未定义");
            _alarmMessage.Add(377, "未定义");
            _alarmMessage.Add(378, "未定义");
            _alarmMessage.Add(379, "未定义");
            ///////////
            _alarmMessage.Add(380, "检测装置未就绪");
            _alarmMessage.Add(381, "等待检测结果超时报警");
            _alarmMessage.Add(382, "转盘上料工位有料异常");
            _alarmMessage.Add(383, "激光切割1异常");
            _alarmMessage.Add(384, "激光切割2异常");
            _alarmMessage.Add(385, "激光切割3异常");
            _alarmMessage.Add(386, "激光打标异常");
            _alarmMessage.Add(387, "R2R1缺料");
            _alarmMessage.Add(388, "Memrance缺料");
            _alarmMessage.Add(389, "R2R2缺料");
            _alarmMessage.Add(390, "冲压缺料");
            _alarmMessage.Add(391, "撕膜缺料");
            _alarmMessage.Add(392, "贴带缺料");
            _alarmMessage.Add(393, "转盘上料端缺料");
            _alarmMessage.Add(394, "托盘上料端缺料");
            _alarmMessage.Add(395, "托盘下料端缺料");
            _alarmMessage.Add(396, "转盘工位12未检测到PCB");
            _alarmMessage.Add(397, "五号传输皮带有料未取走");
            _alarmMessage.Add(398, "下料模组取片失败");
            _alarmMessage.Add(399, "传输皮带1叠片");
            _alarmMessage.Add(400, "相机未准备就绪");
            _alarmMessage.Add(401, "PCB检测无结果");
            _alarmMessage.Add(402, "机器人上料NG容器超出容量");
            _alarmMessage.Add(403, "涂胶NG容器超出容量");
            _alarmMessage.Add(404, "未定义");
            _alarmMessage.Add(405, "未定义");
            _alarmMessage.Add(406, "未定义");
            _alarmMessage.Add(407, "未定义");
            _alarmMessage.Add(408, "未定义");
            _alarmMessage.Add(409, "未定义");
            /////////////////


            _alarmMessage.Add(430, "警告：安全门1打开！");
            _alarmMessage.Add(431, "警告：安全门2打开！");
            _alarmMessage.Add(432, "警告：安全门3打开！");
            _alarmMessage.Add(433, "警告：安全门4打开！");
            _alarmMessage.Add(434, "警告：旋转供料缺料！");
            _alarmMessage.Add(435, "警告：卷对卷1缺料！");
            _alarmMessage.Add(436, "警告：冲压缺料！");
            _alarmMessage.Add(437, "警告：卷对卷2缺料！");
            _alarmMessage.Add(438, "警告：撕保护膜缺料！");
            _alarmMessage.Add(439, "警告：托盘上料点缺料！");
            _alarmMessage.Add(440, "警告：托盘下料点缺料！");
            _alarmMessage.Add(441, "警告：贴带缺料！");
            _alarmMessage.Add(442, "请检查皮带1首端传感器！");
            _alarmMessage.Add(443, "请检查皮带1末端传感器！");
            _alarmMessage.Add(444, "请检查皮带2首端传感器!");
            _alarmMessage.Add(445, "请检查皮带2末端传感器!");
            _alarmMessage.Add(446, "请检查皮带3首端传感器!");
            _alarmMessage.Add(447, "请检查皮带3末端传感器!");
            _alarmMessage.Add(448, "请检查皮带4首端传感器!");
            _alarmMessage.Add(449, "请检查皮带4末端传感器!");
            _alarmMessage.Add(450, "请检查皮带5首端传感器!");
            _alarmMessage.Add(451, "请检查皮带5末端传感器!");
            _alarmMessage.Add(452, "请检查皮带6首端传感器!");
            _alarmMessage.Add(453, "警告：机器人离线！");
            _alarmMessage.Add(454, "警告：Memrance缺料！");
            //_alarmMessage.Add(455, "");
            //_alarmMessage.Add(456, "");
            //_alarmMessage.Add(457, "");



            ///////////////////////////////////////////



            _alarmMessage.Add(600, "未定义");
            _alarmMessage.Add(601, "未定义");
            _alarmMessage.Add(602, "转盘上料端缺料");
            _alarmMessage.Add(603, "托盘上料端缺料");
            _alarmMessage.Add(604, "托盘下料端缺料");
            _alarmMessage.Add(605, "未定义");
            _alarmMessage.Add(606, "未定义");
            _alarmMessage.Add(607, "未定义");
            _alarmMessage.Add(608, "未定义");
            _alarmMessage.Add(609, "未定义");
            _alarmMessage.Add(610, "未定义");
            _alarmMessage.Add(611, "未定义");
            _alarmMessage.Add(612, "未定义");
            _alarmMessage.Add(613, "未定义");
            _alarmMessage.Add(614, "未定义");
            _alarmMessage.Add(615, "未定义");
            _alarmMessage.Add(616, "未定义");
            _alarmMessage.Add(617, "未定义");
            _alarmMessage.Add(618, "未定义");
            _alarmMessage.Add(619, "未定义");
            _alarmMessage.Add(620, "未定义");
            _alarmMessage.Add(621, "未定义");
            _alarmMessage.Add(622, "未定义");
            _alarmMessage.Add(623, "未定义");
            _alarmMessage.Add(624, "未定义");
            _alarmMessage.Add(625, "未定义");
            _alarmMessage.Add(626, "未定义");
            _alarmMessage.Add(627, "未定义");
            _alarmMessage.Add(628, "未定义");
            _alarmMessage.Add(629, "未定义");
            _alarmMessage.Add(630, "未定义");
            _alarmMessage.Add(631, "未定义");
            _alarmMessage.Add(632, "未定义");
            _alarmMessage.Add(633, "未定义");
            _alarmMessage.Add(634, "未定义");
            _alarmMessage.Add(635, "未定义");
            _alarmMessage.Add(636, "未定义");
            _alarmMessage.Add(637, "未定义");
            _alarmMessage.Add(638, "未定义");
            _alarmMessage.Add(639, "未定义");
            _alarmMessage.Add(640, "未定义");
            _alarmMessage.Add(641, "未定义");
            #endregion

        }

        #region SCARA
        #region 开启服务
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StaetTCP(object sender, EventArgs e)
        {
            #region 开启SCARA通信服务端
            if (!TcpSerOn)
            {
                //开启服务
                try
                {
                    if (serverIP != null && serverIP != "" && port >= 0)
                    {
                        tcpServer.InitSocket(IPAddress.Parse(serverIP), port);

                        //初始化服务器对象，IP,PORT

                        tcpServer.Start();
                        //开启服务

                        Scara.Items.Insert(0, string.Format("服务端监听启动成功！监听：{0}:{1}", serverIP, port.ToString()));

                        TcpSerOn = true;
                    }
                }
                catch (Exception ex)
                {
                    Scara.Items.Insert(0, string.Format("服务器启动失败！原因：{0}", ex.Message));
                }
            }
            #endregion

            //视觉通信自动重连
            await reConnectCCD();//视觉电脑未插上时，client.Connect(ipep)非常费时（界面打开 半天才显示），2秒task创建一个线程，八九秒释放一个，导致线程数一直增加有1000+;视觉电脑插上网线就不慢了

            #region listbox items 保持一定长度
            try
            {
                //视觉listbox
                if (listBox1.Items.Count > 20)
                    listBox1.Items.RemoveAt(listBox1.Items.Count - 1);

                //scara 视觉listbox
                if (Scara.Items.Count > 20)
                    Scara.Items.RemoveAt(Scara.Items.Count - 1);
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}    {ex.StackTrace}"); }
            #endregion
        }

        #endregion

        #region 处理接收到客户端的请求和数据

        static int[] numbers = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static int[] times = { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sks"></param>
        private void Rev(BenXHSocket.Sockets sks)
        {
            this.Invoke(new ThreadStart(
                delegate
                {
                    if (sks.ex != null)
                    {
                        if (sks.ClientDispose)
                        {
                            try
                            {
                                Scara.Items.Insert(0, (string.Format("客户端：{0}异常下线！", DateTime.Now.ToString(), sks.Ip)));
                                for (int i = 0; i < 8; i++)
                                {
                                    numbers[i] = 0;
                                    times[i] = 0;
                                }
                                if (sks.Ip.Address.ToString() == IP_1 && sks.Ip.Port == Port_1)
                                {
                                    scara_con = false;
                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".SCARA_ready", false);
                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Err[36]", true);
                                }
                            }
                            catch (Exception ex)
                            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

                        }
                        Scara.Items.Insert(0, sks.ex.Message);
                    }
                    else
                    {
                        if (sks.NewClientFlag)
                        {
                            Scara.Items.Insert(0, (string.Format("客户端：{0}链接", sks.Ip)));
                            HaveConnect = true;
                        }
                        else if (sks.Offset == 0)
                        {
                            Scara.Items.Insert(0, (string.Format("客户端:{0}正常下线", sks.Ip)));
                            for (int i = 0; i < 8; i++)
                            {
                                numbers[i] = 0;
                                times[i] = 0;
                            }
                            for (int i = 0; i < tcpServer.ClientList.Count; i++)
                            {
                                if (tcpServer.ClientList[i].Ip == sks.Ip)
                                {
                                    tcpServer.ClientList.Remove(tcpServer.ClientList[i]);
                                }
                            }
                            try
                            {
                                if (sks.Ip.Address.ToString() == IP_1 && sks.Ip.Port == Port_1)
                                {
                                    scara_con = false;
                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".SCARA_ready", false);
                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Err[36]", true);
                                }
                            }
                            catch (Exception ex)
                            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                        }
                        else
                        {
                            byte[] buffer = new byte[sks.Offset];
                            Array.Copy(sks.RecBuffer, buffer, sks.Offset);
                            string str = Encoding.Default.GetString(buffer);
                            try
                            {
                                #region 获取Scara机器人IP和端口号
                                if (str == "Scara_IP\r\n")
                                {
                                    IP_1 = sks.Ip.Address.ToString();
                                    Port_1 = sks.Ip.Port;
                                    try
                                    {
                                        plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".SCARA_ready", true);
                                        plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Err[36]", false);
                                    }
                                    catch (Exception ex)
                                    { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                                }
                                #endregion
                                #region 错误
                                else if (str.Contains("Error"))
                                {
                                    Scara.Items.Insert(0, string.Format(str));
                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Err[116]", true);
                                }
                                #endregion
                                #region Scara处理
                                else if (str != null)
                                {
                                    try
                                    {
                                        string[] buf = str.Split('/');
                                        if (buf.Length > 1)
                                        {
                                            #region 点位
                                            scara_state = buf[0];
                                            if(buf[0]!=receiveScaraState)
                                            {
                                                receiveScaraState = buf[0];
                                                if (buf[0] == "First")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1000", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1000", false);
                                                }
                                                else if (buf[0] == "PickWait")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1001", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1001", false);

                                                }
                                                else if (buf[0] == "Pick1")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1002", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1002", false);
                                                }
                                                else if (buf[0] == "Pick2")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1003", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1003", false);
                                                }
                                                else if (buf[0] == "Pick3")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1004", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1004", false);
                                                }
                                                else if (buf[0] == "Pick4")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1005", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1005", false);
                                                }
                                                else if (buf[0] == "Pick5")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1006", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1006", false);
                                                }
                                                else if (buf[0] == "Pick6")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1007", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1007", false);
                                                }
                                                else if (buf[0] == "Pick7")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1008", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1008", false);
                                                }
                                                else if (buf[0] == "Pick8")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1009", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1009", false);
                                                }
                                                else if (buf[0] == "Pick9")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1010", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1010", false);
                                                }
                                                else if (buf[0] == "Pick10")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1011", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1011", false);
                                                }
                                                else if (buf[0] == "Pick11")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1012", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1012", false);
                                                }
                                                else if (buf[0] == "Pick12")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1013", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1013", false);
                                                }
                                                else if (buf[0] == "Pick13")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1014", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1014", false);
                                                }
                                                else if (buf[0] == "Pick14")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1015", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1015", false);
                                                }
                                                else if (buf[0] == "Pick15")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1016", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1016", false);
                                                }
                                                else if (buf[0] == "Photo")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1017", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1017", false);
                                                }
                                                else if (buf[0] == "NG")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1018", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1018", false);
                                                }
                                                else if (buf[0] == "PutWait")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1019", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1019", false);
                                                }
                                                else if (buf[0] == "Put")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1020", true);
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1020", false);
                                                }
                                                else if (buf[0] == "Running")
                                                {
                                                    setAll_scara_IX_false();
                                                }
                                                else if (buf[0] == "HasPut")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1021", true);
                                                }
                                                else if (buf[0] == "Running")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1022", true);
                                                }
                                                else if (buf[0] == "Stoped")
                                                {
                                                    setAll_scara_IX_false();
                                                    plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1023", true);
                                                }
                                            }
                                            #endregion
                                            #region 坐标
                                            string[] pos = buf[1].Split(':');
                                            x = Regex.Replace(pos[1], "[a-z]", "", RegexOptions.IgnoreCase).Trim();
                                            y = Regex.Replace(pos[2], "[a-z]", "", RegexOptions.IgnoreCase).Trim();
                                            z = Regex.Replace(pos[3], "[a-z]", "", RegexOptions.IgnoreCase).Trim();
                                            r = Regex.Replace(pos[4], "[a-z]", "", RegexOptions.IgnoreCase).Trim();
                                            #endregion
                                            #region 手臂方向
                                            if (buf[2].Contains("L"))
                                                hand = "Left";
                                            else
                                                hand = "Right";
                                            #endregion
                                            #region 安全位置
                                            if (buf[4].Contains("2") && buf[4] != laststate)
                                            {
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leaveOut", true);
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leavePut", false);
                                                laststate = buf[4];
                                            }
                                            if (buf[4].Contains("3") && buf[4] != laststate)
                                            {
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leavePut", true);
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leaveOut", false);
                                                laststate = buf[4];
                                            }
                                            if (buf[4].Contains("1") && buf[4] != laststate)
                                            {
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leaveOut", true);
                                                //plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".Scara_leavePut", true);
                                                laststate = buf[4];
                                            }
                                            #endregion
                                        }
                                    }
                                    catch (Exception ex)
                                    { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                                }
                                #endregion
                                for (int i = 0; i < tcpServer.ClientList.Count; i++)
                                {
                                    if (HaveConnect && tcpServer.ClientList[i].Ip.Address.ToString() == IP_1 && tcpServer.ClientList[i].Ip.Port != Port_1)
                                        tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Reqdata" + '\r' + '\n');
                                    //IP相等 PORT不相等，机器人发送用201，接收用202，两个client。故此tcpServer只能接受机器人连接，不能接受其他连接
                                }
                            }
                            catch (Exception ex)
                            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                        }
                    }
                }
                )
                );
        }

        /// <summary>
        /// 将SCARA机器人的所有的到位IX复位掉
        /// </summary>
        private void setAll_scara_IX_false()
        {
            for(int i=1000;i<=1023;i++)
            {
                plcParameterOp.SetPLCVar(_tcAdsPlcServer, $".IX{i}", false);//".IX1000"
            }
        }


        #endregion

        #region 关闭服务
        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcpServer.Stop();
            Application.Exit();
            if (_tcAdsPlcServer.PlcIsRunning)
                plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".HMI_Run", false);
        }
        #endregion

        #region 读取PLC指令
        private void State_TW()
        {
            for (int i = 0; i < tcpServer.ClientList.Count; i++)
            {
                try
                {
                    if (HaveConnect && tcpServer.ClientList[i].Ip.Address.ToString() == IP_1 && tcpServer.ClientList[i].Ip.Port == Port_1)
                    {
                        //First
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1000")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "First" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1000", false);
                        }
                        //PickWait
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1001")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "PickWait" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1001", false);
                        }
                        //Pick1
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1002")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick1" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1002", false);
                        }
                        //Pick2
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1003")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick2" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1003", false);
                        }
                        //Pick3
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1004")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick3" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1004", false);
                        }
                        //Pick4
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1005")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick4" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1005", false);
                        }
                        //Pick5
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1006")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick5" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1006", false);
                        }
                        //Pick6
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1007")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick6" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1007", false);
                        }
                        //Pick7
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1008")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick7" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1008", false);
                        }
                        //Pick8
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1009")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick8" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1009", false);
                        }
                        //Pick9
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1010")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick9" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1010", false);
                        }
                        //Pick10
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1011")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick10" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1011", false);
                        }
                        //Pick11
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1012")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick11" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1012", false);
                        }
                        //Pick12
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1013")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick12" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1013", false);
                        }
                        //Pick13
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1014")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick13" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1014", false);
                        }
                        //Pick14
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1015")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick14" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1015", false);
                        }
                        //Pick15
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1016")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Pick15" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1016", false);
                        }
                        //Photo
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1017")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Photo" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1017", false);
                        }
                        //NG
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1018")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "NG" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1018", false);
                        }
                        //PutWait
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1019")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "PutWait" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1019", false);
                        }
                        //Put
                        if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1020")) && scara_state != "Running")
                        {
                            tcpServer.SendToClient(tcpServer.ClientList[i].Ip, "Put" + '\r' + '\n');
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1020", false);
                        }
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            }
        }
        private void timer_tw_Tick(object sender, EventArgs e)
        {
            State_TW();

            //发送拍照指令
            ccdSend();
        }
        #endregion

        #endregion

        #region 视觉通信
        /// <summary>
        /// 视觉通信接收到的消息在此处理
        /// </summary>
        /// <param name="recString"></param>
        private void ccdReceive(string recString)
        {
            try
            {
                if(recString!=null)
                {
                    if(recString.StartsWith("#") && recString.EndsWith("*") )//   C2-OK命令结束是 */r/n
                    {
                        if(recString.Contains("READY-OK"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1025", true);
                        }
                        else if(recString.Contains("READY-NG"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1026", true);
                            listBox1.Items.Insert(0, recString);
                        }
                        else if (recString.Contains("C1-OK"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1030", true);
                        }
                        else if (recString.Contains("C1-NG"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1031", true);
                            listBox1.Items.Insert(0, recString);
                        }
                        else if (recString.Contains("C2-OK"))
                        {
                            //string recString = "#C2-OK|X:123.456|Y:123.456|U:123.456*";
                            string[] recStringSplit = recString.Split('|');
                            recStringSplit[1] = recStringSplit[1].Replace("X:", "");
                            recStringSplit[2] = recStringSplit[2].Replace("Y:", "");

                            recStringSplit[3] = recStringSplit[2].Replace("U:", "");
                            recStringSplit[3] = recStringSplit[2].Replace("*", "");

                            //测试收到的数值能否转float
                            try
                            {
                                float.Parse(recStringSplit[1]);
                                float.Parse(recStringSplit[2]);
                                float.Parse(recStringSplit[3]);
                            }
                            catch (Exception ex)
                            {
                                OutputDebugString($"{ex.Message}   {ex.StackTrace}");
                                return;//转换失败直接返回，PLC那边会计算超时
                            }

                            for (int i = 0; i < Form1.tcpServer.ClientList.Count; i++)
                            {
                                if (Form1.tcpServer.ClientList[i].Ip.Address.ToString() == Form1.IP_1 && Form1.tcpServer.ClientList[i].Ip.Port == Form1.Port_1)
                                {
                                    try
                                    {
                                        if (Form1.HaveConnect)
                                        {
                                            Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, "Offset" + ";" + recStringSplit[1] + ";" + recStringSplit[2] + ";" + recStringSplit[3] + '\r' + '\n');
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        OutputDebugString($"{ex.Message}   {ex.StackTrace}");
                                        return;//直接返回，PLC那边会计算超时
                                    }
                                }
                            }

                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1027", true);

                            
                        }
                        else if (recString.Contains("C2-NG"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1029", true);
                            listBox1.Items.Insert(0, recString);
                        }
                        else if (recString.Contains("C2-REVERSE"))
                        {
                            plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".IX1028", true);
                            listBox1.Items.Insert(0, recString);
                        }
                        else if (recString.Contains("C3-OK"))
                        {
                            //
                        }
                        else if (recString.Contains("C3-NG"))
                        {
                            listBox1.Items.Insert(0, recString);
                        }


                    }
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        //发送视觉拍照指令，根据PLC命令
        private void ccdSend()
        {
            try
            {
                //if (cCD_communication.client == null || !cCD_communication.ccdIsConnect)//查询导致界面更卡
                {
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1025")))
                    {
                        cCD_communication.Send("#READY*");
                        plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1025", false);
                    }
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1026")))
                    {
                        cCD_communication.Send("#C2*");
                        plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1026", false);
                    }
                    if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_tcAdsPlcServer, ".TW1027")))
                    {
                        cCD_communication.Send("#C1*");
                        plcParameterOp.SetPLCVar(_tcAdsPlcServer, ".TW1027", false);
                    }
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        /// <summary>
        /// 视觉通信自动重连
        /// </summary>
        private async Task reConnectCCD()
        {
            Task first = Task.Run(() =>
            {
                try
                {
                    if (cCD_communication.client == null || !cCD_communication.ccdIsConnect)//断开连接后，cCD_communication，里面的socket对象会释放，但释放没那么快
                    {
                        listBox1.Items.Insert(0, "与视觉工控机通信失败，请检查视觉软件是否开启！");
                        cCD_communication = new CCD_communication(ccdServeIP, ccdServePort);
                        cCD_communication.receiveAction += ccdReceive;
                    }
                }
                catch (Exception ex)
                { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            });
            await first;
        }

        private void send_to_CCD(string str)
        {
            try
            {
                cCD_communication.Send(str);
            }
            catch (Exception ex)
            { OutputDebugString($"{ex.Message}         {ex.StackTrace}"); }
        }
        #endregion

        #region 备份参数
        #region 读写ini
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private const string date_ini_path = @"D:\参数备份\config.ini";

        private void iniWriteValue(string section, string key, string value,string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        private string iniReadValue(string section, string key,string path)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();
        }
        #endregion
        /// <summary>
        /// 备份Parameter文件夹 每天第一次运行时
        /// </summary>
        private void BackupParameters()
        {
            try
            {
                //每次启动时，判断使用天数有无超出
                useDate_limit();

                string dateOld = iniReadValue("op", "date", date_ini_path);
                if(dateOld != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    iniWriteValue("op", "date", DateTime.Now.ToString("yyyy-MM-dd"), date_ini_path);
                    string paraNameWithDate = "Parameter"+DateTime.Now.ToString("yyyy-MM-dd");
                    string destPath = @"D:\参数备份\" + paraNameWithDate;
                    CopyFolder(@".\Parameter", destPath);

                    //日期改变时，使用天数加1
                    useDate_add();
                }
            }
            catch(Exception ex)
            {
                OutputDebugString($"{ex.Message}       {ex.StackTrace}");
            }

        }

        /// <summary>
        /// 复制文件夹及文件（不含根目录）
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest,true);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return 1;
            }
            catch (Exception ex)
            {
                OutputDebugString($"{ex.Message}       {ex.StackTrace}");
                return 0;
            }

        }

        #endregion

        #region 判断使用天数
        private const string useDate_ini_path = @"C:\Windows\System32\useDate.ini";

        private void useDate_limit()//每次打开都判断
        {
            try
            {
                string current_days_str= iniReadValue("op", "current_days", useDate_ini_path);
                string max_days_str= iniReadValue("op", "max_days", useDate_ini_path);

                int current_days_int = int.Parse(current_days_str);
                int max_days_int= int.Parse(max_days_str);
                if(current_days_int> max_days_int)
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                OutputDebugString($"{ex.Message}       {ex.StackTrace}");
            }
        }

        private void useDate_add()//日期改变时调用
        {
            try
            {
                string current_days_str = iniReadValue("op", "current_days", useDate_ini_path);
                int current_days_int = int.Parse(current_days_str);
                current_days_int++;
                iniWriteValue("op", "current_days", current_days_int.ToString(), useDate_ini_path);
            }
            catch (Exception ex)
            {
                OutputDebugString($"{ex.Message}       {ex.StackTrace}");
            }
        }

        #endregion





        //视觉通信listbox右键清空
        private void clear_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        //机器人listbox右键清空
        private void clear_scara_Click(object sender, EventArgs e)
        {
            this.Scara.Items.Clear();
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
            #region Form1_Load时在今天历史报警txt加上分界线
            try
            {
                string strlogDate = DateTime.Now.ToString("yyyy-MM-dd");
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(System.Environment.CurrentDirectory + "\\LogFile" + "\\LogFile" + strlogDate + ".txt", true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "*********************************************系统启动运行*********************************************");
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
            #endregion
        }
        #endregion
    }
}
