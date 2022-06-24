using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using CAPC.Config;
using System.Xml;
using System.Data.OleDb;

namespace CAPC.userForm
{
    public partial class ParameterConfig : Form
    {

        private Beckhoff.Forms.TcAdsPlcServer _adsPlcServer;
        private string _HlrealStructPath = string.Empty;
        private string _HuintStructPath = string.Empty;
        private string _switchStructPath = string.Empty;
        private UInt16[] uintArray;

        private const int doubleArrayLength= 300;//370-620机器人
        private double[] doubleArray;
        private bool[] switchArray;
        private string[] colname;

        private string path1;
        private DataTable dt1,dtread;

        /// <summary>
        /// 机器人左右手下拉list
        /// </summary>
        List<ComboBox> scaraHand_List;

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        public ParameterConfig()
        {
            InitializeComponent();
            tabControl2.Visible = false;
            this.tabPage5.Parent = this.tabControl2;

            timer1.Enabled = true;
            timer1.Interval = 200;
            uintArray = new UInt16[100];
            doubleArray = new double[doubleArrayLength];
            switchArray = new bool[200];
            colname = new string[23];
            _adsPlcServer = new Beckhoff.Forms.TcAdsPlcServer(801, Form1.PLCams);
            _HlrealStructPath = Path.Combine(Directory.GetCurrentDirectory(), "Parameter\\Hlreal.xml");
            _HuintStructPath = Path.Combine(Directory.GetCurrentDirectory(), "Parameter\\Huint.xml");
            _switchStructPath = Path.Combine(Directory.GetCurrentDirectory(), "Parameter\\Switch.xml");
            path1 = Path.Combine(Directory.GetCurrentDirectory(), "Data.xls");

            try
            {
                if (!File.Exists(_switchStructPath))
                    MessageBox.Show("switch.xml 文件丢失");
                if (!File.Exists(_HlrealStructPath))
                    MessageBox.Show("Hlreal.xml文件丢失");
                if (!File.Exists(_HuintStructPath))
                    MessageBox.Show("Huint.xml文件丢失");
            }
            catch(Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

            loadLrealArrayToHMI();
            LoadUintArrayFromXMLToHMI();
            LoadToHMI();
            loadLrealArrayToPlc();
            loadUintArrayToPlc();
            LoadSwitchArrayToPlc();

            CbbItem();

            try
            {
                if (!File.Exists(path1))
                    MessageBox.Show("历史数据文件丢失");
                else
                {
                    dt1 = ReadExcelToTable(path1);
                    //dt1.Columns.Remove("F18");
                    //dt1.Columns.Remove("F19");
                    //dt1.Columns.Remove("F20");
                    //dt1.Columns.Remove("F21");
                    //dt1.Columns.Remove("F22");
                    //dt1.Columns.Remove("F23");
                    //dt1.Columns.Remove("F24");
                    //dt1.Columns.Remove("F25");
                    //dt1.Columns.Remove("F26");
                    //dt1.Columns.Remove("F27");
                    dtread = dt1.Copy();
                    for (int i = 0; i < dtread.Columns.Count; i++)
                    {
                        colname[i]=dtread.Columns[i].ColumnName;
                    }
                    for (int j = 5; j < colname.Count(); j++)
                    {
                        dtread.Columns.Remove(colname[j]);
                    }
                    if (Form1.key)
                        dataGridView1.DataSource = dt1;
                    else
                        dataGridView1.DataSource = dtread;

                    //Rlreal10.Text = Hlreal95.Value.ToString();控件已删除
                    //Rlreal11.Text = Hlreal96.Value.ToString();
                    //Rlreal12.Text = Hlreal97.Value.ToString();
                    
          /*          for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (Convert.ToDouble(dt1.Rows[i][2])== Convert.ToDouble(Rlreal10.Text)控件删除导致 赋值不完整
                            && Convert.ToDouble(dt1.Rows[i][3]) == Convert.ToDouble(Rlreal11.Text)
                            && Convert.ToDouble(dt1.Rows[i][4]) == Convert.ToDouble(Rlreal12.Text))
                            textBox1.Text = dt1.Rows[i][1].ToString();
                    }*/
                }
            }
            catch(Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }
        #region LrealArrayLoadToPlc
        private void loadOneLrealToPlc(Control container)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown || c is Label))
                    loadOneLrealToPlc(c);
                else
                {
                    if (c.Name.Contains("Hlreal") && (c is NumericUpDown || c is Label))
                    {
                        int i = Convert.ToInt16(c.Name.Replace("Hlreal", "").Trim());
                        if(i< doubleArrayLength)
                        {
                            doubleArray[i] = Convert.ToDouble(c.Text);
                        }
                        else if(!(i>370 && i<620)) //Hlreal370-620为机器人使用，不需写入PLC
                        {
                            OutputDebugString($"超出hlreal数组长度，{c.Name}");
                        }
                        
                    }
                }
            }
        }
        private void loadLrealArrayToPlc()
        {
            try
            {
                loadOneLrealToPlc(this);
                plcParameterOp.WriteRealArray(_adsPlcServer, ".Hlreal", doubleArrayLength, doubleArray);
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }
        #endregion
        #region  UintArrayLoadToPlc
        private void loadOneUintToPlc(Control container)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown))
                    loadOneUintToPlc(c);
                else
                {
                    if (c.Name.Contains("Huint") && (c is NumericUpDown))
                    {
                        int i = Convert.ToInt16(c.Name.Replace("Huint", "").Trim());
                        uintArray[i] = Convert.ToUInt16(c.Text);
                    }
                }
            }
        }
        private void loadUintArrayToPlc()
        {
            try
            {
                loadOneUintToPlc(this);
                plcParameterOp.WriteRealArray(_adsPlcServer, ".Huint", 100, uintArray);
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }
        #endregion

        #region SaveLrealArrayToXML
        private void SaveLreal(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown || c is ComboBox || c is Label))
                    SaveLreal(c, xmlDoc);
                else
                {
                    if (c.Name.Contains("Hlreal") && (c is NumericUpDown))
                    {
                        string cc = c.Name.Replace("Hlreal", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            root.InnerText = c.Text.Trim();
                        }
                    }
                    else if (c.Name.Contains("Hlreal") && (c is ComboBox))
                    {
                        string cc = c.Name.Replace("Hlreal", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            root.InnerText = (c.Text == "L") ? "0" : "1";
                        }
                    }
                    else if (c.Name.Contains("Hlreal") && (c is Label))
                    {
                        string cc = c.Name.Replace("Hlreal", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            root.InnerText = c.Text.Trim();
                        }
                    }
                }
            }
        }
        private void SaveLrealArrayToXML()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_HlrealStructPath);
                SaveLreal(this, xmlDoc);
                xmlDoc.Save(_HlrealStructPath);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion
        #region SaveUintArrayToXml
        private void SaveUint(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown))
                    SaveUint(c, xmlDoc);
                else
                {
                    if (c.Name.Contains("Huint") && (c is NumericUpDown))
                    {
                        string cc = c.Name.Replace("Huint", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            root.InnerText = c.Text.Trim();
                        }
                    }
                }
            }
        }
        private void SaveUintArrayToXML()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_HuintStructPath);
                SaveUint(this, xmlDoc);
                xmlDoc.Save(_HuintStructPath);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion

        #region LoadLrealArrayFromXMLToHMI
        private void EnumLreal(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown) && !(c is ComboBox))
                    EnumLreal(c, xmlDoc);
                else
                {
                    try
                    {
                        if (c.Name.Contains("Hlreal") && (c is NumericUpDown))
                        {
                            string cc = c.Name.Replace("Hlreal", "m");
                            XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                            if (root != null)
                            { c.Text = root.InnerText; }
                        }
                        else if (c.Name.Contains("Hlreal") && (c is ComboBox))
                        {
                            string cc = c.Name.Replace("Hlreal", "m");
                            XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                            if (root != null)
                            { c.Text = (root.InnerText.ToString() == "0") ? "L" : "R"; }
                        }
                        else if (c.Name.Contains("Hlreal") && (c is Label))
                        {
                            string cc = c.Name.Replace("Hlreal", "m");
                            XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                            if (root != null)
                            { c.Text = root.InnerText; }
                        }
                    }
                    catch (Exception ex)
                    { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                }
            }
        }
        private void loadLrealArrayToHMI()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_HlrealStructPath);
                EnumLreal(this, xmlDoc);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

        }
        #endregion
        #region LoadUintArrayFromXMLToHMI
        private void EnumUint(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is NumericUpDown))
                    EnumUint(c, xmlDoc);
                else
                {
                    if (c.Name.Contains("Huint") && (c is NumericUpDown))
                    {
                        string cc = c.Name.Replace("Huint", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        { c.Text = root.InnerText; }
                    }
                }
            }
        }
        private void LoadUintArrayFromXMLToHMI()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_HuintStructPath);
                EnumUint(this, xmlDoc);
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }
        #endregion

        #region LoadToPlc Switch
        private void LoadOneSwitchToPlc(Control container)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is CheckBox))
                    LoadOneSwitchToPlc(c);
                else
                {
                    if (c.Name.Contains("Switch") && (c is CheckBox))
                    {
                        int i = Convert.ToInt16(c.Name.Replace("Switch", "").Trim());
                        if (((CheckBox)c).Checked)
                            switchArray[i] = true;
                        else
                            switchArray[i] = false;
                    }
                }
            }
        }

        private void LoadSwitchArrayToPlc()
        {
            try
            {
                LoadOneSwitchToPlc(this);
                plcParameterOp.WriteRealArray(_adsPlcServer, ".Switch", 200, switchArray);
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }
        #endregion
        #region  SaveToXML Switch
        private void SaveSwitch(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is CheckBox))
                    SaveSwitch(c, xmlDoc);
                else
                {
                    if (c.Name.Contains("Switch") && (c is CheckBox))
                    {
                        string cc = c.Name.Replace("Switch", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        {
                            root.InnerText = ((CheckBox)c).Checked.ToString();
                        }
                    }
                }
            }
        }
        private void SaveSwitchArrayToXML()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_switchStructPath);
                SaveSwitch(this, xmlDoc);
                xmlDoc.Save(_switchStructPath);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion
        #region LoadToHMI Switch
        private void EnumSwitch(Control container, XmlDocument xmlDoc)
        {
            foreach (Control c in container.Controls)
            {
                //c is the child control here
                if (c.HasChildren && !(c is CheckBox))
                    EnumSwitch(c, xmlDoc);
                else
                {
                    if (c.Name.Contains("Switch") && (c is CheckBox))
                    {
                        string cc = c.Name.Replace("Switch", "m");
                        XmlNode root = xmlDoc.SelectSingleNode("//" + cc);//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                        if (root != null)
                        { ((CheckBox)c).Checked = (root.InnerText == "True") ? true : false; }
                    }
                }
            }
        }
        private void LoadToHMI()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_switchStructPath);
                EnumSwitch(this, xmlDoc);
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }
        #endregion

        #region 读写EXCEL
        public static DataTable ReadExcelToTable(string path)//excel存放的路径
        {
            try
            {

                //连接字符串
                //string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; // Office 07及以上版本 不能出现多余的空格 而且分号注意
                string connstring = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';"; //Office 07以下版本 
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                    conn.Close();
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                    //string sql = string.Format("SELECT * FROM [{0}] WHERE [日期] is not null", firstSheetName); //查询字符串

                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    //for (int i = 0; i < set.Tables[0].Rows.Count; i++)
                    //{
                    //    if (set.Tables[0].Rows[i][0].ToString() == "")
                    //        set.Tables[0].Rows[i].Delete();
                    //}
                    //set.Tables[0].AcceptChanges();
                    return set.Tables[0];
                }
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); return null; }
            

        }

        public static void DTToExcel(string Path, System.Data.DataTable dt)
        {
            string strCon = string.Empty;
            DataTable olddt;
            FileInfo file = new FileInfo(Path);
            string extension = file.Extension;
            switch (extension)
            {
                case ".xls":
                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=2;'";
                    //strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=0;'";
                    //strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=2;'";
                    break;
                case ".xlsx":
                    //strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=Excel 12.0;";
                    //strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2;'";    //出现错误了
                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2;'";
                    break;
                default:
                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=2;'";
                    break;
            }
            try
            {
                using (OleDbConnection con = new OleDbConnection(strCon))
                {
                    con.Open();
                    //获取旧数据
                    DataTable sheetsName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, strCon);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    set.Tables[0].TableName = firstSheetName;
                    olddt = set.Tables[0];

                    //更新添加数据
                    StringBuilder strSQL = new StringBuilder();
                    OleDbCommand cmd;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i < 10)
                        {
                            if (i >= olddt.Rows.Count)    //添加数据
                            {   //strSQL.Clear();
                                strSQL.Remove(0, strSQL.Length);
                                strSQL.Append("INSERT INTO [").Append(olddt.TableName).Append("]");
                                strSQL.Append("(");
                                StringBuilder strvalue = new StringBuilder();
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    strSQL.Append(dt.Columns[j].ColumnName + ",");
                                    strvalue.Append("'" + dt.Rows[i][j] + "'");
                                    if (j != dt.Columns.Count - 1)
                                    {
                                        strvalue.Append(",");
                                    }
                                }
                                strSQL = strSQL.Remove(strSQL.Length - 1, 1);
                                strSQL.Append(") VALUES(" + strvalue.ToString() + ")");
                                cmd = new OleDbCommand(strSQL.ToString(), con);
                                cmd.ExecuteNonQuery();
                            }
                            else  //更新数据
                            {
                                strSQL.Remove(0, strSQL.Length);
                                strSQL.Append("UPDATE [").Append(olddt.TableName).Append("] SET ");
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    strSQL.Append(string.Format("{0}='{1}',", dt.Columns[j].ColumnName, dt.Rows[i][j]));
                                }
                                strSQL = strSQL.Remove(strSQL.Length - 1, 1);
                                strSQL.Append(string.Format(" where {0}='{1}'", dt.Columns[0].ColumnName, olddt.Rows[i][0]));
                                cmd = new OleDbCommand(strSQL.ToString(), con);
                                //cmd.CommandText = strSQL.ToString();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }
        }

        public static Boolean FileIsUsed(String fileFullName)
        {
            Boolean result = false;
            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = false;
            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用
             //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                    result = false;
                }
                catch (System.IO.IOException ioEx)
                {
                    result = true;
                }
                catch (System.Exception ex)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }//end: 如果文件存在的处理逻辑
             //返回指示文件是否已被其它程序使用的值
            return result;
        }
        #endregion


        #region Cbb控件集合
        /// <summary>
        /// Cbb控件集合
        /// </summary>
        private void CbbItem()
        {
            scaraHand_List = new List<ComboBox>()
            { Hlreal504, Hlreal509, Hlreal514 , Hlreal519, Hlreal524, Hlreal529,Hlreal534,Hlreal539,Hlreal544,Hlreal549,Hlreal554,
            Hlreal559,Hlreal564,Hlreal569,Hlreal574,Hlreal579,Hlreal584,Hlreal589,Hlreal594,Hlreal599,Hlreal604,Hlreal609};
            foreach(ComboBox singleC in scaraHand_List)
            {
                singleC.Items.Add("L");
                singleC.Items.Add("R");
            }
        }
        #endregion

        #region 参数写入Scara
        /// <summary>
        /// 参数写入1#Scara
        /// </summary>
        private void WriteToScara_1()
        {
            for (int i = 0; i < Form1.tcpServer.ClientList.Count; i++)
            {
                if (Form1.tcpServer.ClientList[i].Ip.Address.ToString() == Form1.IP_1 && Form1.tcpServer.ClientList[i].Ip.Port == Form1.Port_1)
                {
                    try
                    {
                        if (Form1.HaveConnect)
                        {
                            List<NumericUpDown> scaraPoint_List = new List<NumericUpDown>() { Hlreal500, Hlreal501, Hlreal502, Hlreal503, Hlreal505, Hlreal506, Hlreal507, Hlreal508, Hlreal510, Hlreal511, Hlreal512, Hlreal513, Hlreal515, Hlreal516, Hlreal517, Hlreal518, Hlreal520, Hlreal521, Hlreal522, Hlreal523, Hlreal525, Hlreal526, Hlreal527, Hlreal528, Hlreal530, Hlreal531, Hlreal532, Hlreal533, Hlreal535, Hlreal536, Hlreal537, Hlreal538, Hlreal540, Hlreal541, Hlreal542, Hlreal543, Hlreal545, Hlreal546, Hlreal547, Hlreal548, Hlreal550, Hlreal551, Hlreal552, Hlreal553, Hlreal555, Hlreal556, Hlreal557, Hlreal558, Hlreal560, Hlreal561, Hlreal562, Hlreal563, Hlreal565, Hlreal566, Hlreal567, Hlreal568, Hlreal570, Hlreal571, Hlreal572, Hlreal573, Hlreal575, Hlreal576, Hlreal577, Hlreal578, Hlreal580, Hlreal581, Hlreal582, Hlreal583, Hlreal585, Hlreal586, Hlreal587, Hlreal588, Hlreal590, Hlreal591, Hlreal592, Hlreal593, Hlreal595, Hlreal596, Hlreal597, Hlreal598, Hlreal600, Hlreal601, Hlreal602, Hlreal603, Hlreal605, Hlreal606, Hlreal607, Hlreal608 };

                            int P_num = 0;
                            for(int i_NumericUpDown=0; i_NumericUpDown < 87; i_NumericUpDown += 4)
                            {
                                string ComboBox_Hreal = (int.Parse(scaraPoint_List[i_NumericUpDown].Name.Replace("Hlreal", ""))  + 4).ToString();
                                string Pstring = (100 + P_num).ToString();

                                foreach (ComboBox singleC in scaraHand_List)
                                {
                                    if (singleC.Name.Contains(ComboBox_Hreal))
                                    {
                                        OutputDebugString($"P{Pstring};{scaraPoint_List[i_NumericUpDown].Name};{scaraPoint_List[i_NumericUpDown + 1].Name};{scaraPoint_List[i_NumericUpDown + 2].Name};{scaraPoint_List[i_NumericUpDown + 3].Value};{singleC.Name}\r\n");
                                        OutputDebugString($"P{Pstring};{scaraPoint_List[i_NumericUpDown].Value};{scaraPoint_List[i_NumericUpDown + 1].Value};{scaraPoint_List[i_NumericUpDown + 2].Value};{scaraPoint_List[i_NumericUpDown + 3].Value};{singleC.Text}\r\n");
                                        Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, $"P{Pstring};{scaraPoint_List[i_NumericUpDown].Value};{scaraPoint_List[i_NumericUpDown + 1].Value};{scaraPoint_List[i_NumericUpDown + 2].Value};{scaraPoint_List[i_NumericUpDown + 3].Value};{singleC.Text}\r\n");
                                        break;
                                    }
                                }
                                P_num++;
                            }
                            //Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, "P100" + ";" + Hlreal100.Value + ";" + Hlreal101.Value + ";" + Hlreal102.Value + ";" + Hlreal103.Value + ";" + Hlreal104.Text + '\r' + '\n');

                            Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, "P1AutoSpeed" + ";" + Huint98.Value / 100 + ";" + '\r' + '\n');
                            Form1.tcpServer.SendToClient(Form1.tcpServer.ClientList[i].Ip, "P1ManualSpeed" + ";" + Huint99.Value / 100 + ";" + '\r' + '\n');
                        }
                    }
                    catch (Exception ex)
                    { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                }
            }
        }
        #endregion

        //保存参数按钮 不属于任何tabpage
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                #region 保存配方参数
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i][1].ToString().Equals(textBox1.Text))
                    {
                        dt1.Rows[i][0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //dt1.Rows[i][5] = Convert.ToString(Convert.ToDouble(Hlreal37.Value));控件已删除
                        //dt1.Rows[i][6] = Convert.ToString(Convert.ToDouble(Hlreal39.Value));
                        //dt1.Rows[i][7] = Convert.ToString(Convert.ToDouble(Hlreal36.Value));
                        //dt1.Rows[i][8] = Convert.ToString(Convert.ToDouble(Hlreal38.Value));
                        //dt1.Rows[i][9] = Convert.ToString(Convert.ToDouble(Hlreal53.Value));
                        //dt1.Rows[i][10] = Convert.ToString(Convert.ToDouble(Hlreal59.Value));
                        //dt1.Rows[i][11] = Convert.ToString(Convert.ToDouble(Hlreal76.Value));
                        //dt1.Rows[i][12] = Convert.ToString(Convert.ToDouble(Hlreal80.Value));
                        //dt1.Rows[i][13] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint21.Value)));
                        //dt1.Rows[i][14] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint22.Value)));
                        //dt1.Rows[i][15] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint23.Value)));
                        //dt1.Rows[i][16] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint24.Value)));
                        //dt1.Rows[i][17] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint25.Value)));
                        //dt1.Rows[i][18] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint26.Value)));
                        //dt1.Rows[i][19] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint27.Value)));
                        //dt1.Rows[i][20] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint28.Value)));
                        //dt1.Rows[i][21] = Convert.ToString(Convert.ToDouble(Hlreal10.Value)); 控件已删除
                        dt1.Rows[i][22] = Convert.ToString(Convert.ToDouble(Hlreal91.Value));
                        break;
                    }
                }

                if (FileIsUsed(path1))
                    MessageBox.Show("历史数据文件占用");
                else
                    DTToExcel(path1, dt1);
                #endregion

                SaveLrealArrayToXML();
                SaveUintArrayToXML();
                loadLrealArrayToPlc();
                loadUintArrayToPlc();
                SaveSwitchArrayToXML();
                LoadSwitchArrayToPlc();
                WriteToScara_1(); 

                //plcParameterOp.SetPLCVar(_adsPlcServer, ".Rlreal[10]", Rlreal10.Text);
                //plcParameterOp.SetPLCVar(_adsPlcServer, ".Rlreal[11]", Rlreal11.Text);
                //plcParameterOp.SetPLCVar(_adsPlcServer, ".Rlreal[12]", Rlreal11.Text);
            }
            catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ee.Message}"); }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_adsPlcServer.PlcIsRunning && Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".HMI_Run")))
                {
                    //AXIS9ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS9ACTPos")));控件已删除
                    //AXIS10ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS10ACTPos")));
                    //AXIS11ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS11ACTPos")));
                    //AXIS12ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS12ACTPos")));
                    //AXIS13ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS13ACTPos")));
                    //AXIS14ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS14ACTPos")));
                    //AXIS15ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS15ACTPos")));
                    //AXIS16ACTPos.Text = Convert.ToString(Convert.ToDecimal(plcParameterOp.GetPLCVar(_adsPlcServer, ".AXIS16ACTPos")));
                    //if (Math.Abs(Convert.ToDouble(Hlreal85.Text) - Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[85]"))) > 0.1
                    //    && Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[85]")) != 0)
                    //    Hlreal85.Text = Convert.ToString(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[85]"));
                    //if (Math.Abs(Convert.ToDouble(Hlreal86.Text) - Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[86]"))) > 0.1
                    //    && Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[86]")) != 0)
                    //    Hlreal86.Text = Convert.ToString(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[86]"));
                    //if (Math.Abs(Convert.ToDouble(Hlreal87.Text) - Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[87]"))) > 0.1
                    //    && Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[87]")) != 0)
                    //    Hlreal87.Text = Convert.ToString(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[87]"));
                    //if (Math.Abs(Convert.ToDouble(Hlreal88.Text) - Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[88]"))) > 0.1
                    //    && Convert.ToDouble(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[88]")) != 0)
                    //    Hlreal88.Text = Convert.ToString(plcParameterOp.GetPLCVar(_adsPlcServer, ".Hlreal[88]"));


                    
                }

            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }

            if (!Form1.key)
            {
                tabPage1.Parent = tabControl2;
                tabPage2.Parent = tabControl2;
                tabPage3.Parent = tabControl2;
                tabPage4.Parent = tabControl2;
                tabPage7.Parent = tabControl2;
                tabPage8.Parent = tabControl2;
            }
            else
            {
                tabPage7.Parent = tabControl1;
                tabPage8.Parent = tabControl1;
                tabPage1.Parent = tabControl1;
                tabPage2.Parent = tabControl1;
                tabPage3.Parent = tabControl1;
                tabPage4.Parent = tabControl1;
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

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu13_10", true);
            //Hlreal85.Text = "0";控件已删除
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu13_10", false);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu14_10", true);
            //Hlreal86.Text = "0"; 控件已删除
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu14_10", false);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu15_10", true);
            //Hlreal87.Text = "0"; 控件已删除
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu15_10", false);
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu16_10", true);
            //Hlreal88.Text = "0"; 控件已删除
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            plcParameterOp.SetPLCVar(_adsPlcServer, ".Hmanu16_10", false);
        }
        
        

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar) && e.KeyChar!='.')
            {
                e.Handled = true;
            }

        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);


        /// <summary>
        /// 涂胶轨迹显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tujiao_disp_3z_Click(object sender, EventArgs e)
        {
            try
            {
                List<decimal> tujiao_data_list = new List<decimal>();//这里decimal，显示exe那里是double
                Button btn = (Button)sender;
                if(btn.Name.Contains("3z"))
                {
                    tujiao_data_list.Add(Hlreal180.Value);
                    tujiao_data_list.Add(Hlreal181.Value);
                    tujiao_data_list.Add(Hlreal182.Value);
                    tujiao_data_list.Add(Hlreal183.Value);
                    tujiao_data_list.Add(Hlreal184.Value);
                    tujiao_data_list.Add(Hlreal185.Value);
                    tujiao_data_list.Add(Hlreal186.Value);
                    tujiao_data_list.Add(Hlreal187.Value);
                    tujiao_data_list.Add(Hlreal188.Value);
                }
                if (btn.Name.Contains("4z"))
                {
                    tujiao_data_list.Add(Hlreal200.Value);
                    tujiao_data_list.Add(Hlreal201.Value);
                    tujiao_data_list.Add(Hlreal202.Value);
                    tujiao_data_list.Add(Hlreal203.Value);
                    tujiao_data_list.Add(Hlreal204.Value);
                    tujiao_data_list.Add(Hlreal205.Value);
                    tujiao_data_list.Add(Hlreal206.Value);
                    tujiao_data_list.Add(Hlreal207.Value);
                    tujiao_data_list.Add(Hlreal208.Value);
                }
                using (StreamWriter sw = new StreamWriter(".\\涂胶轨迹显示\\涂胶段长.txt"))//默认append为false 没有则创建，有则覆盖
                {
                    foreach(var sdata in tujiao_data_list)
                    {
                        sw.WriteLine(sdata.ToString());//WriteLine就不用\r\n 自动换行
                    }
                }
                WinExec(".\\涂胶轨迹显示\\涂胶轨迹显示.exe", 1);
            }
            catch (Exception ex) { OutputDebugString($"{ex.Message}       {ex.StackTrace}"); }
        }


        //配方切换按钮
        private void button7_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(plcParameterOp.GetPLCVar(_adsPlcServer, ".MState_Starting")))
                MessageBox.Show("设备运行中，请停机后再切换！");
            else
            {
                try
                {
                    bool flag = false;
                    DataRow drr = dt1.NewRow();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i][1].ToString().Equals(textBox1.Text))
                        {
                            flag = true;
                            drr = dt1.Rows[i];
                            break;
                        }
                        else
                            flag = false;
                    }
                    if (!flag)
                    {
                        drr[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        drr[1] = textBox1.Text;
                        drr[2] = Rlreal10.Text;
                        drr[3] = Rlreal11.Text;
                        drr[4] = Rlreal12.Text;
                        //double length= (Convert.ToDouble(Hlreal95.Text) - Convert.ToDouble(Rlreal10.Text));控件已删除
                        //double weigth = (Convert.ToDouble(Hlreal96.Text) - Convert.ToDouble(Rlreal11.Text));
                        //double high = (Convert.ToDouble(Hlreal97.Text) - Convert.ToDouble(Rlreal12.Text));
                        //drr[5] = Convert.ToString(Convert.ToDouble(Hlreal37.Value));控件已删除
                        //drr[6] = Convert.ToString(Convert.ToDouble(Hlreal39.Value) + weigth / 2);
                        //drr[7] = Convert.ToString(Convert.ToDouble(Hlreal36.Value) + weigth / 2);
                        //drr[8] = Convert.ToString(Convert.ToDouble(Hlreal38.Value) + weigth / 2);
                        //drr[9] = Convert.ToString(Convert.ToDouble(Hlreal53.Value) + length / 2);
                        //drr[10] = Convert.ToString(Convert.ToDouble(Hlreal59.Value) - length / 2);
                        //drr[11] = Convert.ToString(Convert.ToDouble(Hlreal76.Value) - high);
                        //drr[12] = Convert.ToString(Convert.ToDouble(Hlreal80.Value) + high);
                        //drr[13] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint21.Value) - length / 2));
                        //drr[14] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint22.Value) - length / 2));
                        //drr[15] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint23.Value) - length / 2));
                        //drr[16] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint24.Value) - length / 2));
                        //drr[17] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint25.Value) + weigth / 2));
                        //drr[18] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint26.Value) + weigth / 2));
                        //drr[19] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint27.Value) - length));
                        //drr[20] = Convert.ToString(Convert.ToInt16(Convert.ToDouble(Huint28.Value) - length));
                        ////drr[21] = Convert.ToString(Convert.ToDouble(Hlreal10.Value) - length);控件已删除
                        //drr[22] = Convert.ToString(Convert.ToDouble(Hlreal91.Value) - length);

                        dt1.Rows.InsertAt(drr, 0);
                    }

                    Rlreal10.Text = drr[2].ToString();
                    Rlreal11.Text = drr[3].ToString();
                    Rlreal12.Text = drr[4].ToString();
                    //Hlreal95.Text = drr[2].ToString();
                    //Hlreal96.Text = drr[3].ToString();
                    //Hlreal97.Text = drr[4].ToString();
                    //Hlreal37.Value = Convert.ToDecimal(drr[5].ToString());控件已删除
                    //Hlreal39.Value = Convert.ToDecimal(drr[6].ToString());
                    //Hlreal36.Value = Convert.ToDecimal(drr[7].ToString());
                    //Hlreal38.Value = Convert.ToDecimal(drr[8].ToString());
                    //Hlreal53.Value = Convert.ToDecimal(drr[9].ToString());
                    //Hlreal59.Value = Convert.ToDecimal(drr[10].ToString());
                    //Hlreal76.Value = Convert.ToDecimal(drr[11].ToString());
                    //Hlreal80.Value = Convert.ToDecimal(drr[12].ToString());
                    //Huint21.Value = Convert.ToDecimal(drr[13].ToString());
                    //Huint22.Value = Convert.ToDecimal(drr[14].ToString());
                    //Huint23.Value = Convert.ToDecimal(drr[15].ToString());
                    //Huint24.Value = Convert.ToDecimal(drr[16].ToString());
                    //Huint25.Value = Convert.ToDecimal(drr[17].ToString());
                    //Huint26.Value = Convert.ToDecimal(drr[18].ToString());
                    //Huint27.Value = Convert.ToDecimal(drr[19].ToString());
                    //Huint28.Value = Convert.ToDecimal(drr[20].ToString());
                    //Hlreal10.Value = Convert.ToDecimal(drr[21].ToString());控件已删除
                    Hlreal91.Value = Convert.ToDecimal(drr[22].ToString());//更新了numeric控件，是否需要点击下保存，将配方更新到PLC/机器人/XML/EXCEL???

                }
                catch (Exception ee) { MessageBox.Show($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}   数据保存错误！"); }
            }
        }
    }
}

