using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

    static void Main(string[] args)
    {
        /*Stopwatch stopwatch = new Stopwatch();
        int frameCount = 0;

        while (true)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            string windowTitle = GetActiveWindowTitle(foregroundWindow);

            if (windowTitle == "我记得 - 赵雷")
            {
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                }

                frameCount++;
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    float fps = (float)frameCount / (stopwatch.ElapsedMilliseconds / 1000);
                    Console.WriteLine("FPS: " + fps);

                    stopwatch.Reset();
                    frameCount = 0;
                }
            }
            else
            {
                stopwatch.Stop();
                stopwatch.Reset();
                frameCount = 0;
            }

            Thread.Sleep(10);
        }*/
        while (true)
        {
            GameWindowHelper gameWindowHelper = new GameWindowHelper();
            string windowTitle = gameWindowHelper.GetActiveWindowTitle();
            Console.WriteLine("Window title: " + windowTitle);
            Thread.Sleep(1000);
        }

    }

    static string GetActiveWindowTitle(IntPtr handle)
    {
        const int nChars = 256;
        System.Text.StringBuilder buffer = new System.Text.StringBuilder(nChars);
        if (GetWindowText(handle, buffer, nChars) > 0)
        {
            return buffer.ToString();
        }
        return null;
    }

    public class GameWindowHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

        public string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buffer = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, nChars) > 0)
            {
                return buffer.ToString();
            }
            return null;
        }
    }
}


