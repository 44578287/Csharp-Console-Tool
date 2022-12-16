/*
 总结:
%~dp0 获取当前路径可使用 System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase 方法,结尾包含\
例如C:\xxxxx\xxxxxx\

%~dp0 获取当前路径可使用 System.Environment.CurrentDirectory 方法,结尾不包含\
例如C:\xxxxx\xxxxxx

包含此运行文件.exe使用 System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName 方法
例如C:\xxxxx\xxxxxx\此文件.exe
 */




//包含运行(本文件.exe)
string 当前路径 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
//当前应用完整路径=%~dp0 结尾不包含\
string 当前路径1 = System.Environment.CurrentDirectory;
//当前应用完整路径=%~dp0 结尾不包含\
string 当前路径2 = System.IO.Directory.GetCurrentDirectory();
//当前应用完整路径=%~dp0 结尾包含\ 完全等于%~dp0
string 当前路径3 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
//string 当前路径4 = System.Windows.Forms.Application.StartupPath;



Console.WriteLine(当前路径);
Console.WriteLine(当前路径1);
Console.WriteLine(当前路径2);
Console.WriteLine(当前路径3);
//Console.WriteLine(当前路径4);