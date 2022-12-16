using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyC杂交
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
            Process process = new();//创建进程对象    
            ProcessStartInfo startInfo = new(runFilePath, s); // 括号里是(程序名,参数)
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
    }
}
