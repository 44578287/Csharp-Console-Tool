using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern IntPtr GetForegroundWindow();

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern int GetWindowTextLength(IntPtr hWnd);

[DllImport("User32.dll", CharSet = CharSet.Auto)]
static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);   //获取线程ID

string GetCaptionOfActiveWindow()
{
    var strTitle = string.Empty;
    var handle = GetForegroundWindow();  
    var intLength = GetWindowTextLength(handle) + 1;
    var stringBuilder = new StringBuilder(intLength);
    if (GetWindowText(handle, stringBuilder, intLength) > 0)
    {
        strTitle = stringBuilder.ToString();
    }
    return strTitle;
}


static void Chk检测游戏进程(List<string> DataInName)
{
    //获取所有进程名为Tetris的进程,并将相关数据以数组方式存储
    Process[] ps = Process.GetProcesses();
    //Process[] ps = Process.GetProcessesByName("obs64");
    //获取最后一个进程的序列下标(因为数组编号0开始的,所以要减1
    //ps.Length为查找到的进程数量
    /*foreach (var DataIn in ps)
    {
        if (DataIn.ProcessName == "obs64" || DataIn.ProcessName == "Arduino IDE")
        {
            string? ExeTitle = DataIn.MainWindowTitle;//主模块标题
            Console.WriteLine(ExeTitle);
        }
    }*/

    var exp1 = ps.Where(a => DataInName.Exists(t => a.ProcessName.Contains(t))).ToList();
    foreach (var item in exp1)
    {
        Console.WriteLine(item.MainWindowTitle+" "+ item.Id);
    }


    /*if (ps.Length > 0)
    {
        int PorcNum = ps.Length;//获取进程名的进程数量
        int LastNum = ps.Length - 1;
        int LastProcID = ps[LastNum].Id;//获取最有一个进程的ID          
        DateTime LastStartTime = ps[LastNum].StartTime;//获取最后一个进程的启动时间
        Console.WriteLine(LastStartTime);
        string? ExeDir = ps[LastNum].MainModule?.FileName;//获取最后一个进程主模块的完整程序路径（绝对路径）
        Console.WriteLine(ExeDir);
        string? ExeTitle = ps[LastNum].MainWindowTitle;//主模块标题
        Console.WriteLine(ExeTitle);
        long Ram = ps[LastNum].PrivateMemorySize64;//专用内存
        Console.WriteLine(Ram/1024/1024);
        return true;
    }
    else
    {
        return false;
    }*/
}

static bool Chk检测游戏进程1(List<string> ProcessName, List<AppData> AppData)
{
    Process[] ALL = Process.GetProcesses();
    AppData.Clear();

    var AllData = ALL.Where(a => ProcessName.Exists(t => a.ProcessName.Contains(t))).ToList();
    foreach (var DataIn in AllData)
    {
        AppData.Add
            (
                new()
                {
                    ProcessName = DataIn.ProcessName,
                    ProcessTitleName = DataIn.MainWindowTitle,
                    ProcessId = DataIn.Id,
                    ProcessStartTime = DataIn.StartTime,
                    ProcessRam = DataIn.PrivateMemorySize64,
                    Status = true
                }
            );
    }
    if (AppData != null)
        return true;
    return false;
}


for (; ; )
{
    //Console.WriteLine(GetCaptionOfActiveWindow());
    //Console.WriteLine(GetForegroundWindow());

    //Console.WriteLine(GetWindowText(0));
    //Console.WriteLine(GetWindowTextLength());
    //Thread.Sleep(3000);    //睡眠3s，用来选择活动窗口
    //IntPtr hWnd = GetForegroundWindow();    //获取活动窗口句柄
    int calcID = 0;    //进程ID
    int calcTD = 0;    //线程ID
    //calcTD = GetWindowThreadProcessId(hWnd, out calcID);
    //Process myProcess = Process.GetProcessById(calcID);
    //Console.WriteLine(calcTD);
    //Console.WriteLine(GetCaptionOfActiveWindow());
    //Chk检测游戏进程(new() { "obs64", "Arduino IDE" });
    List<AppData> appDatas = new();
    if (Chk检测游戏进程1(new() { "obs64", "Arduino IDE" }, appDatas))
    {
        foreach (var DataIn in appDatas)
        {
            Console.WriteLine(DataIn.ProcessName + " " + DataIn.ProcessTitleName + " " + DataIn.ProcessId);
        }
    }
    //Console.WriteLine("进程名：" + myProcess?.ProcessName + "\n" + "进程ID：" + calcID +  "\n" + "程序路径：" /*+ myProcess?.MainModule?.FileName*/);  //在MessageBox中显示获取的信息
    //Console.ReadKey();



    Thread.Sleep(2500);
}


public class AppData
{
    public string? ProcessName { get; set; } = null;//进程名
    public string? ProcessTitleName { get; set; } = null;//标题
    public int? ProcessId { get; set; } = null;//进程ID
    public DateTime? ProcessStartTime { get; set; } = null;//获取最后一个进程的启动时间
    public long? ProcessRam { get; set; } = null;//专用内存
    public bool Status { get; set; } = false;//存活状态
}