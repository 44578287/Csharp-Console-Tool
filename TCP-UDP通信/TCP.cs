using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LoongEgg.LoongLogger;


namespace TCP_UDP通信
{
    public class NetworkManager
    {
        // TCP侦听器
        private TcpListener? tcpListener;
        // UDP端口号
        private readonly int udpPort;

        // 已连接的TCP客户端的字典
        private readonly Dictionary<int, TcpClient> tcpClients = new Dictionary<int, TcpClient>();
        // 以连接的客户端ID
        private readonly Dictionary<int, string> ClientIP = new();

        // 分配给下一个客户端的ID
        private int clientId = 0;
        //TCP连接新客户端
        public event Action<int>? TcpConnect;
        //TCP断开客户端
        public event Action<int>? TcpDisconnect;
        //TCP接收信息回调
        public event Action<int, string>? TcpReceiveMessage;
        //UDP接收信息回调
        public event Action<string, string>? UdpReceiveMessage;

        public NetworkManager(bool useTCP, int Port)
        {
            this.udpPort = Port;
            Task task;
            if (useTCP)
            {
                tcpListener = new TcpListener(IPAddress.Any, Port);
                tcpListener.Start();
                task = WaitForConnection();
                Task.Run(async () => await task);
                Logger.WriteInfor($"以TCP模式启动服务器 Port:{Port} 服务器TaskID:{task.Id}");
            }
            else
            {
                var udpServer = new UdpClient(Port);
                task = WaitForUdpRequests(udpServer);
                Task.Run(async () => await task);
                Logger.WriteInfor($"以UDP模式启动服务器 Port:{Port} 服务器TaskID:{task.Id}");
            }
        }

        // 异步等待TCP客户端连接
        private async Task WaitForConnection()
        {
            while (true)
            {
                try
                {
                    // 异步等待新的TCP客户端连接
                    var tcpClient = await tcpListener!.AcceptTcpClientAsync();
                    // 处理连接请求，为客户端分配ID，并开启新的线程处理客户端通信
                    ProcessConnection(tcpClient, clientId++);
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"接受客户端连接时出错：{ex.Message}");
                }
            }
        }

        // 处理新的TCP客户端连接
        private void ProcessConnection(TcpClient tcpClient, int id)
        {
            // 添加新的TCP客户端到字典中
            tcpClients[id] = tcpClient;
            TcpConnect?.Invoke(id);
            Logger.WriteInfor($"新连接客户端 ID:{id}");

            Task.Run(async () =>
            {
                // 获取TCP客户端的流
                var stream = tcpClient.GetStream();

                try
                {
                    // 处理客户端的通讯
                    while (tcpClient.Connected)
                    {
                        // 读取客户端发送的消息
                        var buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            // 客户端已经断开连接
                            break;
                        }
                        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Logger.WriteInfor($"从客户端{id}接收到TCP消息：{receivedMessage.TrimEnd('\0')}");
                        TcpReceiveMessage?.Invoke(id, receivedMessage.TrimEnd('\0'));
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"处理客户端{id}的TCP通信时出错：{ex.Message}");
                }
                finally
                {
                    // 关闭连接并从字典中移除对应的客户端
                    tcpClients.Remove(id);
                    tcpClient?.Dispose();
                    Logger.WriteInfor($"已与客户端{id}断开TCP连接");
                    TcpDisconnect?.Invoke(id);
                }
            });
        }

        // 等待UDP请求
        private async Task WaitForUdpRequests(UdpClient udpServer)
        {
            var remoteEP = new IPEndPoint(IPAddress.Any, udpPort);

            while (true)
            {
                try
                {
                    // 异步等待UDP客户端请求
                    var requestBytes = await udpServer.ReceiveAsync();

                    Logger.WriteInfor($"从客户端{requestBytes.RemoteEndPoint}接收到UDP消息：{Encoding.UTF8.GetString(requestBytes.Buffer)}");
                    UdpReceiveMessage?.Invoke(requestBytes.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(requestBytes.Buffer));//UDP接收信息回调
                    //var response = Encoding.UTF8.GetBytes($"服务器已接收到UDP消息：{Encoding.UTF8.GetString(requestBytes.Buffer)}");

                    // 向UDP客户端返回响应
                    //await udpServer.SendAsync(response, response.Length, requestBytes.RemoteEndPoint);
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"处理UDP请求时出错：{ex.Message}");
                }
            }
        }

        // 透过指定ID向TCP客户端发送指定消息
        public async Task SendToClient(int id, string message)
        {
            if (!tcpClients.TryGetValue(id, out var tcpClient))
            {
                throw new ArgumentException($"未找到指定ID的客户端: {id}");
            }

            var stream = tcpClient.GetStream();

            var buffer = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);

            Logger.WriteInfor($"已向客户端{id}发送TCP消息: {message}");
        }

        // 透过UDP向指定IP和端口发送指定消息
        public async Task SendToUdp(string ip, int port, string message)
        {
            using (var udpClient = new UdpClient())
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await udpClient.SendAsync(bytes, bytes.Length, ip, port);

                Logger.WriteInfor($"已向{ip}:{port}发送UDP消息：{message}");
            }
        }
        //获取客户端IP
        public string? GetClientIP(int id)
        {
            if (!tcpClients.TryGetValue(id, out var tcpClient))
            {
                throw new ArgumentException($"未找到指定ID的客户端: {id}");
            }
            return tcpClient?.Client?.RemoteEndPoint?.ToString()?.Split(':')[0];
        }

        // 透过指定ID向TCP客户端发送文件
        public async Task SendFileToClient(int id, string filePath)
        {
            if (!tcpClients.TryGetValue(id, out var tcpClient))
            {
                throw new ArgumentException($"未找到指定ID的客户端: {id}");
            }

            var stream = tcpClient.GetStream();

            // 读取文件
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[1024];
                int readBytes;

                // 发送文件内容
                while ((readBytes = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await stream.WriteAsync(buffer, 0, readBytes);
                }
            }

            Logger.WriteInfor($"已向客户端{id}发送文件: {filePath}");
        }

        // 处理客户端发送的文件
        public async Task ReceiveFile(TcpClient tcpClient, string filePath)
        {
            var stream = tcpClient.GetStream();

            // 读取文件内容并保存到磁盘
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[1024];
                int readBytes;

                while ((readBytes = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, readBytes);
                }
            }

            Logger.WriteInfor($"已从客户端{tcpClient.Client.RemoteEndPoint}接收文件: {filePath}");
        }

        // 透过UDP向指定IP和端口发送文件
        public async Task SendFileToUdp(string ip, int port, string filePath)
        {
            using (var udpClient = new UdpClient())
            {
                // 读取文件
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    int readBytes;

                    // 发送文件内容
                    while ((readBytes = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await udpClient.SendAsync(buffer, readBytes, ip, port);
                    }
                }

                Logger.WriteInfor($"已向{ip}:{port}发送文件: {filePath}");
            }
        }

    }
}
