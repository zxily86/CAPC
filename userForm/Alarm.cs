using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace CAPC.userForm
{
    public partial class Alarm : Form
    {
        string strlogDate;
        string path,docpath;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public Alarm()
        {
            InitializeComponent();
            strlogDate = DateTime.Now.ToString("yyyy-MM-dd");
            docpath = System.Environment.CurrentDirectory + "\\LogFile";
            path = System.Environment.CurrentDirectory + "\\LogFile" + "\\LogFile" + strlogDate + ".txt";
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)//今天报警文件存在时，将该TXT文件里的都读到LISTBOX上显示
                {
                    lvWarnAndError.Items.Add(line.ToString());
                }
                sr.Close();
            }
            else
                lvWarnAndError.Items.Add("无报警");


            FileInfo[] files = new DirectoryInfo(docpath).GetFiles();
            FileComparer fc = new FileComparer();
            Array.Sort(files, fc);
            int len;
            if (files.Length > 15)
                len = 15;
            else
                len = files.Length;
            string[] files_back = files.Select(f=>f.Name).ToArray();
            string[] files_i=new string[len];
            for (int i = 0; i < len; i++)
            {
                files_back[i] = files_back[i].Substring(7,10);
                files_i[i] = files_back[i];
            }
            if (files_i.Length > 0)
            {
                if (files_i[0].CompareTo(strlogDate) != 0)//当今天还没报警TXT产生时，在combobox下拉加入今天的年月日
                    data.Items.Add(strlogDate);
            }
            else
                data.Items.Add(strlogDate);//一个报警TXT都没时，在combobox下拉加入今天的年月日
            data.Items.AddRange(files_i);
        }

        public class FileComparer : IComparer
        {
            int IComparer.Compare(Object o1, Object o2)
            {
                FileInfo fi1 = o1 as FileInfo;
                FileInfo fi2 = o2 as FileInfo;
                return fi2.CreationTime.CompareTo(fi1.CreationTime);
            }
        }

        private void data_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (data.Text != "")
            {
                path = System.Environment.CurrentDirectory + "\\LogFile" + "\\LogFile" + data.Text + ".txt";
                lvWarnAndError.Items.Clear();
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(path, Encoding.UTF8);
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lvWarnAndError.Items.Add(line.ToString());
                    }
                    sr.Close();
                }
                else
                    lvWarnAndError.Items.Add("无报警");
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
        }
        #endregion

    }
}
