using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 下载
{
    internal class exe
    {
        public static bool TransferExe(string runFilePath, params string[] args)
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
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
            return true;
        }
        public static string TransferExe2(string runFilePath, params string[] args)
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
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
            return output;
        }
        public static void Aria2(string 下载地址)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string[] 指令 = { "--split=12 --user-agent=Chrome ", 下载地址 };
            string 调用aria2地址 = 当前路径 + "/Data/Tool/aria2.exe";
            TransferExe(调用aria2地址, 指令);

        }
        public static void Aria2(string 下载地址, string 目标位置)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string[] 指令 = { "--split=12 --user-agent=Chrome", " --dir=" + 目标位置, 下载地址 };
            string 调用aria2地址 = 当前路径 + "/Data/Tool/aria2.exe";
            TransferExe(调用aria2地址, 指令);

        }
        public static void Aria2(string 下载地址, string 目标位置, string 重命名)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string[] 指令 = { "--split=12 --user-agent=Chrome", " --dir=" + 目标位置, 下载地址, "-o " + 重命名 };
            string 调用aria2地址 = 当前路径 + "/Data/Tool/aria2.exe";
            TransferExe(调用aria2地址, 指令);

        }
        public static void CmdDw(string 下载地址, string 目标位置, string 重命名)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string[] 指令 = { "/c " + "powershell -Command Invoke-WebRequest " + 下载地址 + " -OutFile ", 目标位置 + "\\" + 重命名 };
            Console.WriteLine(目标位置);
            //Console.WriteLine(指令[0]+ 指令[1]);
            TransferExe("cmd.exe", 指令);

        }
        public static void CurlDw(string 下载地址)
        {
            string[] 指令 = { "-L -O ", 下载地址};
            TransferExe("curl", 指令);
        }
        public static void CurlDw(string 下载地址, string 目标位置, string 重命名)
        {
            string[] 指令 = { "-L -o ",目标位置+"/"+ 重命名 ,下载地址,};
            TransferExe("curl", 指令);
        }
        public static void WinRAR(string 目标文件, string 解压目录)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string 调用Winrar地址 = 当前路径 + "\\tool\\WinRAR.exe";
            string[] 指令 = { " x -y -aos ", " " + "\"" + 目标文件 + "\"", " " + "\"" + 解压目录 + "\"" };
            TransferExe(调用Winrar地址, 指令);
        }
        public static void Ping(string 目标IP)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string 调用tcping地址 = 当前路径 + "\\tool\\tcping.exe";
            string[] 指令 = { 目标IP };
            TransferExe(调用tcping地址, 指令);
        }
        public static string Ping2(string 目标IP)
        {
            string 当前路径 = System.IO.Directory.GetCurrentDirectory();
            string 调用tcping地址 = 当前路径 + "\\tool\\tcping.exe";
            string[] 指令 = { 目标IP };
            return TransferExe2(调用tcping地址, 指令);
        }
    }
}
