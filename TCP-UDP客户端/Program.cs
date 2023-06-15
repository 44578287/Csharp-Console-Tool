
using LoongEgg.LoongLogger;
using TCP_UDP客户端;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数

TCP.Connection connection = new ("127.0.0.1", 8787);
 connection.ConnectUDP(); //or connection.ConnectUDP();

while (true)
{
    Thread.Sleep(5000);
    try
    {
         connection.Send("你也好");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    //NetworkManager.Dispose();
}


//connection.DataReceived += message => Console.WriteLine("Received message: " + message);