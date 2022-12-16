using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 控制台排版
{
    internal class 控制台UI
    {
        public static void 切换字体颜色(string 文本内容, ConsoleColor 颜色)
        {
            Console.ForegroundColor = 颜色;
            Console.Write(文本内容);
        }
        public static void 切换字体颜色保留原色(string 文本内容, ConsoleColor 颜色)
        {
            ConsoleColor 原色 = Console.ForegroundColor;
            Console.ForegroundColor = 颜色;
            Console.Write(文本内容);
            Console.ForegroundColor = 原色;
        }
        public static void 切换字体颜色并换行(string 文本内容, ConsoleColor 颜色)
        {
            Console.ForegroundColor = 颜色;
            Console.WriteLine(文本内容);
        }
        public static void 切换字体颜色保留原色并换行(string 文本内容, ConsoleColor 颜色)
        {
            ConsoleColor 原色 = Console.ForegroundColor;
            Console.ForegroundColor = 颜色;
            Console.WriteLine(文本内容);
            Console.ForegroundColor = 原色;
        }
        public static void 输出红色(string str)
        {
            切换字体颜色保留原色(str, ConsoleColor.Red);
        }
        public static void 输出蓝色(string str)
        {
            切换字体颜色保留原色(str, ConsoleColor.Blue);
        }
        public static void 输出绿色(string str)
        {
            切换字体颜色保留原色(str, ConsoleColor.Green);
        }
        public static void 输出红色换行(string str)
        {
            切换字体颜色保留原色并换行(str, ConsoleColor.Red);
        }
        public static void 输出蓝色换行(string str)
        {
            切换字体颜色保留原色并换行(str, ConsoleColor.Blue);
        }
        public static void 输出绿色换行(string str)
        {
            切换字体颜色保留原色并换行(str, ConsoleColor.Green);
        }
        public static void 输出置中(string str)
        {
            //切换字体颜色保留原色并换行(str, ConsoleColor.Green);
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, 1);
            Console.Write(str);
        }
        public static void 输出置中(string str, ConsoleColor 颜色)
        {
            //切换字体颜色保留原色并换行(str, ConsoleColor.Green);
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, 1);
            切换字体颜色(str, 颜色);
        }
        public static void 输出置中并换行(string str)
        {
            //切换字体颜色保留原色并换行(str, ConsoleColor.Green);
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, 1);
            Console.WriteLine(str);
        }
        public static void 输出置中并换行(string str, ConsoleColor 颜色)
        {
            //切换字体颜色保留原色并换行(str, ConsoleColor.Green);
            Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, 1);
            切换字体颜色并换行(str, 颜色);
        }

        /*public static void 标题()
        {
            蓝色("=======================================================================================================================");
            蓝色("\n\t\t\t\t\t欢迎使用MC自动安装更新脚本C#重构版");
            蓝色("\n=======================================================================================================================");
        }*/
        public static void 显示标题(string 文本内容, ConsoleColor 文本颜色, ConsoleColor 分隔行颜色)
        {
            切换字体颜色保留原色("=======================================================================================================================", 分隔行颜色);
            切换字体颜色保留原色并换行("\n\t\t\t\t\t" + 文本内容, 文本颜色);//置中显示 
            切换字体颜色保留原色("=======================================================================================================================", 分隔行颜色);
        }
        public static void 显示小标题(string 文本内容, ConsoleColor 文本颜色, ConsoleColor 分隔行颜色)
        {
            切换字体颜色保留原色("=======================================================================================================================", 分隔行颜色);
            切换字体颜色保留原色并换行("\n\n\t\t\t\t\t\t[ "+ 文本内容 + " ]", 文本颜色);
        }
        public static void 分割栏(ConsoleColor 文本颜色)
        {
            切换字体颜色保留原色并换行("=======================================================================================================================", 文本颜色);
        }
        public static void 分割栏(int 换行数, ConsoleColor 文本颜色)
        {
            for (int i = 0; i < 换行数; i++)
            {
                Console.WriteLine("");
            }
            切换字体颜色保留原色并换行("=======================================================================================================================", 文本颜色);
        }
    }
}
