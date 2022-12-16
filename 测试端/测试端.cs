using System;
using System.Net;
using System.Net.Sockets;
namespace SimpleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Client();
                //关闭连接后可提示继续连接
                Console.WriteLine("End Client");
                Console.WriteLine("是否继续？是[Y]/否[N]");
                string ss = Console.ReadLine();
                if (!(ss == "Y" || ss == "y"))
                {
                    break;
                }
            }
        }
        private static void Client()
        {
            //指定要连接的服务器IP，本机是127.0.0.1
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //端口号
            EndPoint ep = new IPEndPoint(ip, 33678);
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            //连接到指定主机
            client.Connect(ep);
            string s = "Hello, Server";
            byte[] data = System.Text.Encoding.Default.GetBytes(s);
            //第一次握手，告诉服务器我已经连接
            client.Send(data, SocketFlags.Partial);
            while (true)
            {
                //第二次握手，确认服务端已准备好
                client.Receive(data);
                //第三次握手，从键盘读取数据
                string info = Console.ReadLine();
                //转换数据类型
                data = System.Text.Encoding.Default.GetBytes(info);
                //传输数据到服务器
                client.Send(data);
                if (info.StartsWith("_quit"))
                {
                    break;
                }
            }
            //关闭连接
            client.Close();
        }
    }
}
