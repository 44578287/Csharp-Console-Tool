using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 方法
{
    internal class Class1
    {
            
        public static void 方法1()
        {
            int 数字 = 10;

            if (数字 % 2 == 0)
            {
                Console.WriteLine("true");
            }
            else
            {
                Console.WriteLine("false");
            }
        }
        public static void 方法2()
        {
            int a = 10;
            int b = 20;
            if(a>b)
                Console.WriteLine("a大");
            else
                Console.WriteLine("b大");

        }

        public static void 方法3(int 数字)
        { 
            if(数字==0)
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
        }

        public static bool 方法4(int a, int b)
        {
            if (a > b)
            {
                Console.WriteLine("a大");
                return true;
            }
            else
            { 
                Console.WriteLine("b大");
                return false;
            }
        }
    }
}
