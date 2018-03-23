using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using Lib4Net.Logs;
using System.Text;
using System.Configuration;

namespace ADSystem.API.JSYHServer.server
{
    public class TCPHelper
    {
        /// <summary>
        /// tcp请求
        /// </summary>
        /// <param name="requestParams">请求参数</param>
        /// <returns></returns>
        public static string Request(string requestParams,ILogger logger)
        {
            try
            {
                string server = ConfigurationManager.AppSettings["tcp_server"];
                int port =Convert.ToInt32(ConfigurationManager.AppSettings["tcp_server_port"]);
                //定义主机的IP及端口
                IPAddress ip = IPAddress.Parse(server);
                IPEndPoint ipEnd = new IPEndPoint(ip, port);
                //定义套接字类型
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(ipEnd);
                }
                //异常处理
                catch (SocketException ex)
                {
                    logger.Info("Fail to connect tcp server");
                    logger.Fatal(ex.Message, ex);
                    return null;
                }
                logger.Info("tcp请求的参数:" + requestParams);
                //发送的数据
                byte[] sendData = Encoding.GetEncoding("GB2312").GetBytes(requestParams);
                //发送数据
                socket.Send(sendData);
                int readSize = 0;
                byte[] message = new byte[1024];
                StringBuilder Respone = new StringBuilder();
                do
                {
                    readSize = socket.Receive(message, message.Length, 0);
                    Respone.Append(Encoding.GetEncoding("GB18030").GetString(message, 0, readSize));
                }
                while (readSize > 0);
                socket.Close();
                logger.Info("响应的参数:" + Respone.ToString());
                return Respone.ToString();
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                logger.Fatal(ex.Message, ex);
                return null;
            }
        }
    }
}