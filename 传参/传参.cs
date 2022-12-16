using System.Collections.Generic;
using System.Diagnostics;
// See https://aka.ms/new-console-template for more information
/*Console.WriteLine("Hello, World!");
void add1(ref int i)
{
    i = 10;
}
int C = 0;
add1(ref C);
Console.WriteLine(C);*/

List<AppData> AppData = new();
List < Process >? AllProcessesData = new();


for (; ; )
{
    Chk检测游戏进程(new List<string>{ "cloudmusic" }, AppData, AllProcessesData);
    Console.WriteLine(AppData.Count);
    Console.WriteLine(AllProcessesData?.Count);
    Thread.Sleep(3000);
}


static bool Chk检测游戏进程(List<string> ProcessName, List<AppData> AppData, List<Process> AllProcessesDataIn)
{
    Process[] AllProcessesData = Process.GetProcesses();
    AllProcessesDataIn.Clear();
    AppData.Clear();

    foreach (var DataIn in AllProcessesData)
    {
        AllProcessesDataIn.Add(DataIn);
    }
    
    var AllData = AllProcessesData.Where(a => ProcessName.Exists(t => a.ProcessName.Contains(t))).ToList();
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
public class AppData
{
    public string? ProcessName { get; set; } = null;//进程名
    public string? ProcessTitleName { get; set; } = null;//标题
    public int? ProcessId { get; set; } = null;//进程ID
    public DateTime? ProcessStartTime { get; set; } = null;//获取最后一个进程的启动时间
    public long? ProcessRam { get; set; } = null;//专用内存
    public bool Status { get; set; } = false;//存活状态
}