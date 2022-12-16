using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IpType();
        }
        private static void IpType()
        {
            //IP地址
            IPAddress ipaddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipaddr, 33678);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定连接，一定要在监听之前
            socket.Bind(endPoint);
            //开始监听，最大支持同时5个连接
            socket.Listen(5);
            while (true)
            {
                //如果未检测到连接，将阻塞，不再继续执行
                Socket clientSocket = socket.Accept();
                byte[] data = new byte[512];
                //第一次握手，接收到数据确认链接成功
                clientSocket.Receive(data, 0, data.Length, SocketFlags.Partial);
                string s = System.Text.Encoding.Default.GetString(data);
                Console.WriteLine(s.Split('\0')[0]);
                //启动新线程处理客户端的连接信息
                Task.Factory.StartNew(() =>
                {
                    reciiveFromClient(clientSocket);
                });
            }
        }

        private static void reciiveFromClient(Socket client)
        {
            byte[] data;
            try
            {
                while (true)
                {
                    //第二次握手，告诉客户端，服务端已准备好
                    client.Send(System.Text.Encoding.Default.GetBytes("GoOn"));
                    data = new byte[512];
                    //第三次握手，接受数据
                    client.Receive(data, SocketFlags.None);
                    string str = System.Text.Encoding.Default.GetString(data);
                    //创建队列
                    Queue clientQ = new Queue();
                    if (clientQ.Equals(data))//判断数据是否已在队列中，如果不在先添加进队列
                    {
                    }
                    else
                    {
                        //往对列添加数据
                        clientQ.Enqueue(data);
                    }
                    if (str.StartsWith("_quit"))
                    {
                        //如果检测到_quit，关闭连接
                        client.Close();
                        Console.WriteLine("连接关闭");
                        break;
                    }
                    else
                    {
                        //控制台打印数据
                        string info = str.Split('\0')[0];
                        Console.WriteLine("From Client :" + info);
                    }
                    if (str.StartsWith("模组地址"))
                    {
                        string info = "https://123";
                        data = System.Text.Encoding.Default.GetBytes(info);
                        client.Send(data);
                    }
                    else
                    {
                    }
                }
            }
            catch (SocketException e)
            {
                //处理异常
            }
        }
    }
}
