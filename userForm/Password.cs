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
    public partial class Password : Form
    {
        private string _userRightPath;
        public Password()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            _userRightPath = Path.Combine(Directory.GetCurrentDirectory(), "Parameter\\UserRight.xml");

            //_userRight = new UserRightSt();
            //if (File.Exists(_userRightPath))
            //{
            //    _userRight = GetXMLFile(_userRightPath);
            //}
            //else
            //    MessageBox.Show("wo");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(_userRightPath);
                //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                XmlNode root = xmlDoc.SelectSingleNode("//"+textBox4.Text);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                if (root != null)
                {
                    string mima = (root.SelectSingleNode("mima")).InnerText;
                    if (textBox1.Text.Trim() == mima)
                    {
                        if (textBox2.Text.Trim() == null || textBox3.Text.Trim() == null)
                        {
                            MessageBox.Show("新密码不能为空");
                        }
                        else
                        {
                            if (textBox2.Text.Trim() == textBox3.Text.Trim())
                            {
                                try
                                {
                                    (root.SelectSingleNode("mima")).InnerText = textBox2.Text.Trim();
                                    xmlDoc.Save(_userRightPath);
                                }
                                catch { }
                                finally
                                {
                                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                                    MessageBox.Show("成功修改密码");
                                }
                            }
                            else
                                MessageBox.Show("新密码两次输入不同，请重新输入");
                        }
                    }
                    else
                        MessageBox.Show("旧密码错误");
                }
                else
                    MessageBox.Show("用户名错误");
            }
            catch { }
        }
    }
}
