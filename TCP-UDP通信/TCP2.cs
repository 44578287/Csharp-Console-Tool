using System.Net;
using System.Net.Sockets;
using System.Text;
using LoongEgg.LoongLogger;

namespace TCP_UDP通信
{
    internal class TCP2
    {
        // NetworkManager类，实现IDisposable接口
        public class NetworkManager //: IDisposable
        {
            private readonly TcpListener? tcpListener; // TCP监听器
            private readonly int udpPort; // UDP端口号
            private readonly Dictionary<int, TcpClient> tcpClients = new Dictionary<int, TcpClient>(); // 存储TCP客户端的字典
            private int clientId = 0; // 客户端ID，从0开始自增

            private Task TEMP_Task;
            //private CancellationTokenSource cts = new CancellationTokenSource();


            // 定义TCP连接、断开连接、接收消息的事件
            public event Action<int>? TcpConnect;
            public event Action<int>? TcpDisconnect;
            public event Action<int, string>? TcpReceiveMessage;
            // 定义UDP接收消息的事件
            public event Action<string, string>? UdpReceiveMessage;

            // 构造函数，根据useTCP参数判断使用TCP还是UDP，port为端口号
            public NetworkManager(bool useTCP, int port)
            {
                udpPort = port;
                if (useTCP) // 使用TCP
                {
                    tcpListener = new TcpListener(IPAddress.Any, port);
                    tcpListener.Start(); // 开始监听
                                         // 在异步任务中等待TCP连接
                    TEMP_Task = WaitForConnection();
                    TEMP_Task.ConfigureAwait(false);
                    Logger.WriteInfor($"以TCP模式启动服务器 Port:{port} 服务器TaskID:{TEMP_Task.Id}");
                }
                else // 使用UDP
                {
                    using var udpServer = new UdpClient(port); // 创建UDP服务器
                                                               // 在异步任务中等待UDP请求
                    TEMP_Task = WaitForUdpRequests(udpServer);
                    TEMP_Task.ConfigureAwait(false);
                    Logger.WriteInfor($"以UDP模式启动服务器 Port:{port} 服务器TaskID:{TEMP_Task.Id}");
                }
            }

            // 等待TCP连接的异步任务
            private async Task WaitForConnection()
            {
                while (true) // 一直等待连接
                {
                    try
                    {
                        var tcpClient = await tcpListener!.AcceptTcpClientAsync(); // 异步等待TCP连接
                        ProcessConnection(tcpClient, clientId++); // 处理连接
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError($"接受客户端连接时出错：{ex.Message}");
                    }
                }
            }

            // 处理TCP连接
            private void ProcessConnection(TcpClient tcpClient, int id)
            {
                tcpClients[id] = tcpClient; // 将TCP客户端存储到字典中
                TcpConnect?.Invoke(id); // 触发TCP连接事件
                Logger.WriteInfor($"新连接客户端 ID:{id}");

                ProcessConnectionAsync(tcpClient, id).ConfigureAwait(false); // 异步处理连接
            }

            // 异步处理TCP连接
            private async Task ProcessConnectionAsync(TcpClient tcpClient, int id)
            {
                var buffer = new byte[1024]; // 缓冲区大小为1024字节
                var stream = tcpClient.GetStream();

                try
                {
                    while (true) // 一直等待接收消息
                    {
                        var readBytes = await stream.ReadAsync(buffer, 0, buffer.Length); // 异步等待接收消息
                        if (readBytes == 0) break; // 接收到0字节表示连接已断开

                        var receivedMessage = Encoding.UTF8.GetString(buffer, 0, readBytes); // 将接收到的字节转换为字符串
                        Logger.WriteInfor($"从客户端{id}接收到TCP消息：{receivedMessage.TrimEnd('\0')}");
                        TcpReceiveMessage?.Invoke(id, receivedMessage.TrimEnd('\0')); // 触发TCP接收消息事件
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteError($"处理客户端{id}的TCP通信时出错：{ex.Message}");
                }
                finally
                {
                    tcpClients.Remove(id); // 从字典中删除TCP客户端
                    tcpClient.Dispose(); // 释放TCP连接资源
                    Logger.WriteInfor($"已与客户端{id}断开TCP连接");
                    TcpDisconnect?.Invoke(id); // 触发TCP断开连接事件
                }
            }

            // 等待UDP请求的异步任务
            private async Task WaitForUdpRequests(UdpClient udpServer)
            {
                while (true) // 一直等待请求
                {
                    try
                    {
                        var requestBytes = await udpServer.ReceiveAsync(); // 异步等待UDP请求
                        Logger.WriteInfor($"从客户端{requestBytes.RemoteEndPoint}接收到UDP消息：{Encoding.UTF8.GetString(requestBytes.Buffer)}");
                        UdpReceiveMessage?.Invoke(requestBytes.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(requestBytes.Buffer).TrimEnd('\0')); // 触发UDP接收消息事件
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteError($"处理UDP请求时出错：{ex.Message}");
                    }
                }
            }

            // 向指定ID的TCP客户端发送消息
            public async Task SendToClient(int id, string message)
            {
                if (!tcpClients.TryGetValue(id, out var tcpClient))
                {
                    throw new ArgumentException($"未找到指定ID的客户端: {id}");
                }

                var buffer = Encoding.UTF8.GetBytes(message); // 将消息转换为字节
                await tcpClient.GetStream().WriteAsync(buffer, 0, buffer.Length); // 异步写入流

                Logger.WriteInfor($"已向客户端{id}发送TCP消息: {message}");
            }

            // 向指定IP和端口号的UDP客户端发送消息
            public async Task SendToUdp(string ip, int port, string message)
            {
                using var udpClient = new UdpClient(); // 创建UDP客户端
                var bytes = Encoding.UTF8.GetBytes(message); // 将消息转换为字节
                await udpClient.SendAsync(bytes, bytes.Length, ip, port); // 异步发送消息

                Logger.WriteInfor($"已向{ip}:{port}发送UDP消息：{message}");
            }

            // 根据ID获取TCP客户端的IP地址
            public string? GetClientIP(int id)
            {
                if (!tcpClients.TryGetValue(id, out var tcpClient))
                {
                    throw new ArgumentException($"未找到指定ID的客户端: {id}");
                }
                return tcpClient?.Client?.RemoteEndPoint?.ToString()?.Split(':')[0]; // 返回客户端的IP地址
            }

            // 向指定ID的TCP客户端发送文件
            public async Task SendFileToClient(int id, string filePath)
            {
                if (!tcpClients.TryGetValue(id, out var tcpClient))
                {
                    throw new ArgumentException($"未找到指定ID的客户端: {id}");
                }

                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read); // 创建文件流
                await fileStream.CopyToAsync(tcpClient.GetStream()); // 异步复制文件流到TCP连接流中

                Logger.WriteInfor($"已向客户端{id}发送文件: {filePath}");
            }

            // 从TCP客户端接收文件
            public async Task ReceiveFile(TcpClient tcpClient, string filePath)
            {
                var stream = tcpClient.GetStream(); // 获取TCP连接流
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write); // 创建文件流
                await stream.CopyToAsync(fileStream); // 将TCP连接流中的数据复制到文件流中

                Logger.WriteInfor($"已从客户端{tcpClient.Client.RemoteEndPoint}接收文件: {filePath}");
            }

            // 向指定IP和端口号的UDP客户端发送文件
            public async Task SendFileToUdp(string ip, int port, string filePath)
            {
                using var udpClient = new UdpClient(); // 创建UDP客户端
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read); // 创建文件流

                var buffer = new byte[1024]; // 缓冲区大小为1024字节
                int readBytes;

                while ((readBytes = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0) // 一直读取缓冲区直到文件结尾
                {
                    await udpClient.SendAsync(buffer, readBytes, ip, port); // 异步发送缓冲区中的数据
                }

                Logger.WriteInfor($"已向{ip}:{port}发送文件: {filePath}");
            }

            // 释放资源
            /*public void Dispose()
            {
                tcpListener!.Stop(); // 停止TCP监听
                foreach (var tcpClient in tcpClients.Values) // 释放所有TCP客户端资源
                {
                    tcpClient.Dispose();
                }
            }*/
        }
    }
}
