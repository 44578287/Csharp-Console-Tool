/*
引用参数 using System.Diagnostics;

调用EXE方法
bool 调用exe(string runFilePath, params string[] args)
{
    string s = "";
    foreach (string arg in args)
    {
        s = s + arg + " ";
    }
    s = s.Trim();
    Process process = new Process();//创建进程对象    
    ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
    process.StartInfo = startInfo;
    process.Start();
    return true;
}

调用方法
调用exe("目标exe路径",回传参数(array))





*/
using System.Diagnostics;


string 当前路径 = System.IO.Directory.GetCurrentDirectory();

string 模组版本 = "8";
string 模组地址 = "https://link.jscdn.cn/sharepoint/aHR0cHM6Ly9nOTk2NDk1Ny1teS5zaGFyZXBvaW50LmNvbS86dTovZy9wZXJzb25hbC9nOTk2NDk1N19nOTk2NDk1N19vbm1pY3Jvc29mdF9jb20vRVM0QzlnV1FMS3RQaUFCVVNuemZlY0VCWTZPSnpwYjNteXhPeGYxRlRSZDhsdz9lPUNkOHFtMw.rar";


Console.WriteLine(当前路径);
Console.WriteLine(当前路径+"\\tool\\aria2.exe");

/*
string exe_path = 当前路径+/tool/;  // 被调exe
string[] the_args = { "1", "2", "3", "4" };   // 被调exe接受的参数
*/



bool 调用exe(string runFilePath, params string[] args)
{
    string s = "";
    foreach (string arg in args)
    {
        s = s + arg + " ";
    }
    s = s.Trim();
    Process process = new Process();//创建进程对象    
    ProcessStartInfo startInfo = new ProcessStartInfo(runFilePath, s); // 括号里是(程序名,参数)
    process.StartInfo = startInfo;
    //process.StartInfo.CreateNoWindow = true;
    process.Start();
    process.WaitForExit();  //等待程序执行完退出进程
    process.Close();
    return true;
}


/*string exe_path = 当前路径 + "\\tool\\aria2.exe";  // 被调exe
string[] the_args = { "--split=12",模组地址 };   // 被调exe接受的参数
调用exe(exe_path, the_args);*/

调用exe(当前路径 + "\\tool\\tcping.exe","10.10.10.254");
