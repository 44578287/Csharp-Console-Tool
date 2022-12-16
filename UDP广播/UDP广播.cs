/*
public static void UDP广播(string 发送信息, int 广播端口)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            iep1 = new IPEndPoint(IPAddress.Broadcast, 广播端口);//255.255.255.255:1234 广播IP&端口
            data = Encoding.UTF8.GetBytes(发送信息);
            sock.SendTo(data, iep1);
        }








*/

//Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;


    class UDP广播
    {
        private static Socket sock;
        private static IPEndPoint iep1;
        private static byte[] data;
        //private static data;
        static void Main(string[] args)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,ProtocolType.Udp);
            iep1 =new IPEndPoint(IPAddress.Broadcast, 1234);//255.255.255.255:1234 广播IP&端口

        //string hostname = Dns.GetHostName();
        //data = Encoding.ASCII.GetBytes(hostname);
        //data = "你好鴨!小捷物联网设备";
        //data = Encoding.UTF8.GetBytes("你好鴨!小捷物联网设备");

        sock.SetSocketOption(SocketOptionLevel.Socket,
            SocketOptionName.Broadcast, 1);

        Thread t = new Thread(BroadcastMessage);
            t.Start();
            //sock.Close();

            //Console.ReadKey();

        }

        private static void BroadcastMessage()
        {
            while (true)
            {
            data = Encoding.UTF8.GetBytes("你好鴨!小捷物联网设备");
            sock.SendTo(data, iep1);
                Console.WriteLine("发送广播");
                //Console.WriteLine(IPAddress.Broadcast);
                Thread.Sleep(2000);
            }

        }

    }