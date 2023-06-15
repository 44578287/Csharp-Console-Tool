using System.Net;
using System.Net.Sockets;
using LoongEgg.LoongLogger;

namespace TCP_UDP客户端
{
    internal class TCP
    {
        public class Connection : IDisposable
        {
            private Socket socket;
            private EndPoint remoteEP;
            private string host;
            private int port;
            private Thread receiveThread;
            private bool disposedValue;

            public Connection(string host, int port)
            {
                this.host = host;
                this.port = port;
            }

            public void ConnectTCP()
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                remoteEP = new IPEndPoint(IPAddress.Parse(host), port);

                try
                {
                    socket.Connect(remoteEP);

                    Logger.WriteInfor($"TCP已连接! Host:{host}:{port}");

                    receiveThread = new Thread(ReceiveLoop);
                    receiveThread.Start();
                }
                catch (Exception ex)
                {
                    Logger.WriteError("连接到服务器失败: " + ex.Message);
                    throw;
                }
            }

            public void ConnectUDP()
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                remoteEP = new IPEndPoint(IPAddress.Parse(host), port);

                try
                {
                    socket.Connect(remoteEP);

                    Logger.WriteInfor($"UDP已连接! Host:{host}:{port}");

                    receiveThread = new Thread(ReceiveLoop);
                    receiveThread.Start();
                }
                catch (Exception ex)
                {
                    Logger.WriteError("连接到服务器失败: " + ex.Message);
                    throw;
                }
            }

            public event Action<string> DataReceived;
            public event Action<string> DataSent;

            private void ReceiveLoop()
            {
                while (true)
                {
                    try
                    {
                        byte[] data = new byte[1024];
                        int bytesReceived = socket.Receive(data);

                        if (bytesReceived > 0)
                        {
                            string message = System.Text.Encoding.UTF8.GetString(data, 0, bytesReceived);
                            Logger.WriteInfor("收到数据: " + message);
                            DataReceived?.Invoke(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError("接收数据失败: " + ex.Message);
                        throw;
                    }
                }
            }

            public void Send(string message)
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, ar =>
                {
                    try
                    {
                        int bytesSent = socket.EndSend(ar);
                        Logger.WriteInfor("发送数据: " + message);
                        DataSent?.Invoke(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError("接收数据失败: " + ex.Message);
                    }
                }, null);
            }

            public void SendFile(string fileName)
            {
                byte[] fileData = File.ReadAllBytes(fileName);
                byte[] fileNameData = System.Text.Encoding.UTF8.GetBytes(Path.GetFileName(fileName));
                byte[] data = new byte[fileNameData.Length + fileData.Length + 1];
                fileNameData.CopyTo(data, 0);
                data[fileNameData.Length] = 0;
                fileData.CopyTo(data, fileNameData.Length + 1);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, ar =>
                {
                    try
                    {
                        int bytesSent = socket.EndSend(ar);
                        Logger.WriteInfor("发送文件: " + fileName);
                        DataSent?.Invoke(fileName);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError("发送文件失败: " + fileName + ", " + ex.Message);
                    }
                }, null);
            }

            public void Close()
            {
                try
                {
                    if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    Logger.WriteInfor("连接关闭");
                }
                catch (Exception ex)
                {
                    Logger.WriteError("关闭连接失败: " + ex.Message);
                    throw;
                }
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        Close();
                        socket.Dispose();
                    }

                    disposedValue = true;
                }
            }

            ~Connection()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

    }
}
