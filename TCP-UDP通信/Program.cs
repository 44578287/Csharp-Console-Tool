using LoongEgg.LoongLogger;
using TCP_UDP通信;
Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数

TCP2.NetworkManager NetworkManager = new(false, 8787);
//Task.Run(async () => await NetworkManager.WaitForTCPConnection());


while (true)
{
    Thread.Sleep(5000);
    try
    {
        await NetworkManager.SendToClient(0, "你好");
    }
    catch (Exception e) 
    {
        Console.WriteLine(e.Message);
    }
    //NetworkManager.Dispose();
}

