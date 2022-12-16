using Downloader;
using System.Net;
using System.Reflection;
using static 下载.Download;
using ShellProgressBar;
using 下载;

string 当前路径 = System.IO.Directory.GetCurrentDirectory();



/*await DownloadBuilder.New()
    .WithUrl("https://cloud.445720.xyz/api/v3/file/source/221/%E5%85%89%E5%BD%B1-2.rar?sign=BvnhScZudn7aRG4miejfJQ0Ql0BChAvIV2vum8I-ZbM%3D%3A0")
    .WithDirectory(当前路径 + "/测试")
    .Build()
    .StartAsync();*/



/*string httpUrl = "https://cloud.445720.xyz/api/v3/file/source/221/%E5%85%89%E5%BD%B1-2.rar?sign=BvnhScZudn7aRG4miejfJQ0Ql0BChAvIV2vum8I-ZbM%3D%3A0";
string saveUrl = 当前路径  + "/测试/1.zip";
int threadNumber = 10;
MultiDownload md = new MultiDownload(threadNumber, httpUrl, saveUrl);
md.Start();*/


// 请求退出执行
/*bool RequestToQuit = false;
// 标记完成方法
void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1750, Action<int> childAction = null)
{
	var initialMessage = pbar.Message;
	for (var i = 0; i < ticks && !RequestToQuit; i++)
	{
		pbar.Message = $"Start {i + 1} of {ticks} {Console.CursorTop}/{Console.WindowHeight}: {initialMessage}";
		childAction?.Invoke(i);
		Thread.Sleep(sleep);
		pbar.Tick($"End {i + 1} of {ticks} {Console.CursorTop}/{Console.WindowHeight}: {initialMessage}");
	}
}


const int totalTicks = 100;
var options = new ProgressBarOptions
{
	// 设置前景色
	ForegroundColor = ConsoleColor.Yellow,
	// 设置完成时的前景色
	ForegroundColorDone = ConsoleColor.DarkGreen,
	// 设置背景色
	BackgroundColor = ConsoleColor.DarkGray,
	// 设置背景字符
	//BackgroundCharacter = '\u2593'
};
using (var pbar = new ProgressBar(totalTicks, "进度条", options))
{
	TickToCompletion(pbar, totalTicks, sleep: 100);
}*/

exe.CurlDw("https://cloud.445720.xyz/api/v3/file/source/221/%E5%85%89%E5%BD%B1-2.rar?sign=BvnhScZudn7aRG4miejfJQ0Ql0BChAvIV2vum8I-ZbM%3D%3A0", 当前路径+"/测试","1.rar");

//curl -L -o C:\Users\g9964\Desktop\vs2022\测试1_控制台\下载\bin\Debug\net6.0\测试\1.rar https://cloud.445720.xyz/api/v3/file/source/221/%E5%85%89%E5%BD%B1-2.rar?sign=BvnhScZudn7aRG4miejfJQ0Ql0BChAvIV2vum8I-ZbM%3D%3A0

//curl -L -O https://cloud.445720.xyz/api/v3/file/source/221/%E5%85%89%E5%BD%B1-2.rar?sign=BvnhScZudn7aRG4miejfJQ0Ql0BChAvIV2vum8I-ZbM%3D%3A0
