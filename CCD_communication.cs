using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace CAPC
{
    class CCD_communication
    {
        /*
         * 断开连接测试110次，重启服务端30次  都能成功重连
         */


        /// <summary>
        /// 连接用client
        /// </summary>
        public Socket client;//连接被断开后，对象就会被释放
        private byte[] ReceiveBuf = new byte[2048];
        public static object sendLock = new object();
        public event Action<string> receiveAction;
        
        private bool isServeRun = false;

        /// <summary>
        /// 返回视觉通信是否连接
        /// </summary>
        public bool ccdIsConnect { get { return isConnect(); } }


        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern void OutputDebugString(string message);

        //构造函数
        public CCD_communication(string serveIp, int servePort)
        {
            try
            {
                //try毛线
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(serveIp), servePort);
                if (client != null && client.Connected)
                {
                    client.Close();
                }
                lock (sendLock)
                {
                    client.Connect(ipep);
                    client.BeginReceive(ReceiveBuf, 0, ReceiveBuf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
                    isServeRun = true;
                }
            }
            catch (Exception ex)
            {
                OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}");
            }
        }


        //通信异步接收函数
        private void Receive(IAsyncResult ia)
        {
            try
            {
                client = ia.AsyncState as Socket;
                int count = client.EndReceive(ia);
                client.BeginReceive(ReceiveBuf, 0, ReceiveBuf.Length, SocketFlags.None, new AsyncCallback(Receive), client);
                string context = Encoding.Default.GetString(ReceiveBuf, 0, count);
                if (context.Length > 0)
                {
                    receiveAction(context);
                }
                else
                {
                    OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  收到CCD发送来的空字符串");
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        public void Send(string sendStr)
        {
            try
            {
                lock (sendLock)
                {
                    byte[] sendBuf = Encoding.Default.GetBytes(sendStr);//buf不能与接收共用用
                    client.Send(sendBuf, 0, sendBuf.Length, SocketFlags.None);
                }
            }
            catch (Exception ex)
            { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
        }

        private bool isConnect()
        {
            if (isServeRun)
            {
                if (client.Poll(10, SelectMode.SelectRead))
                {
                    try
                    {
                        int nRead = client.Receive(ReceiveBuf);
                        if (nRead == 0)
                        {
                            client.Close();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        { OutputDebugString($"{Form1.GetCurSourceFileName()}: {Form1.GetLineNum()}  {ex.Message}"); }
                        return false;
                    }
                }
                return true;//服务端没启动时，也会返回true。。。
            }
            else
            {
                return false;
            }
        }
    }
}
