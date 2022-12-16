using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace exe类
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

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.CreateNoWindow = true;

            process.Start();
            //string strOuput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();  //等待程序执行完退出进程
            process.Close();
            return true;
        }
        public static string TransferExe2(string command, params string[] args)
        {
            string s = "";
            foreach (string arg in args)
            {
                s = s + arg + " ";
            }
            s = s.Trim();
            Process pro = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(command, s);

            pro.StartInfo = startInfo;
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.RedirectStandardError = true;
            pro.StartInfo.RedirectStandardInput = true;

            pro.StartInfo.RedirectStandardOutput = true;
            pro.StartInfo.CreateNoWindow = true;
            pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            pro.Start();
            pro.StandardInput.WriteLine(command);
            pro.StandardInput.WriteLine("exit");
            pro.StandardInput.AutoFlush = true;
            //获取cmd窗口的输出信息
            string output = pro.StandardOutput.ReadToEnd();
            pro.WaitForExit();//等待程序执行完退出进程
            pro.Close();
            return output;

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
