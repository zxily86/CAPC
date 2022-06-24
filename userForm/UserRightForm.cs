using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace CAPC.userForm
{
    public partial class UserRightForm : Form
    {
        private string _userRightPath;
        private Password _password;
        public UserRightForm()
        {
            InitializeComponent();
            _userRightPath = Path.Combine(Directory.GetCurrentDirectory(), "Parameter\\UserRight.xml");
            this.KeyPress += UserRightForm_KeyPress;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(_userRightPath);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//" + textBox1.Text.Trim().ToLower());//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string mima = (root.SelectSingleNode("mima")).InnerText;
                    if (textBox2.Text.Trim() == mima)
                    {
                        if (textBox1.Text.Trim().ToLower() == "admin")
                            Form1.key = true;
                        else
                            Form1.key = false;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        Form1.userN = textBox1.Text.Trim();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("密码错误，请重新输入密码");
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
                else
                {
                    MessageBox.Show("用户不存在");
                    //Console.Read();  
                }
            }
            catch { }
        }

        private void UserRightForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='\r')
                button1_Click(null, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _password = new Password();
            _password.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Form1.key = false;   //旧的有bug，按取消键会直接获取操作员权限（即使不输任何账户密码）
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
