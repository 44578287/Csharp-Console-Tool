

using System;
using System.Runtime.InteropServices;

public class WindowRefreshMonitor
{
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static void Main()
    {
        // 设置要监测的窗口的标题
        string windowTitle = "任务管理器";

        // 找到窗口的句柄
        IntPtr hWnd = FindWindow(null, windowTitle);
        if (hWnd == IntPtr.Zero)
        {
            Console.WriteLine("无法找到指定窗口");
            return;
        }

        // 监测窗口的刷新
        while (true)
        {
            RECT windowRect;
            if (GetWindowRect(hWnd, out windowRect))
            {
                int windowWidth = windowRect.Right - windowRect.Left;
                int windowHeight = windowRect.Bottom - windowRect.Top;

                Console.WriteLine("窗口刷新，宽度: " + windowWidth + ", 高度: " + windowHeight);
            }

            // 添加适当的延迟，以避免过多的CPU使用
            System.Threading.Thread.Sleep(1000);
        }
    }
}





/*public class FpsCounter
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    private const int LOGPIXELSX = 88;
    private const int LOGPIXELSY = 90;

    private const int DESKTOPVERTRES = 117;
    private const int DESKTOPHORZRES = 118;

    private struct POINT
    {
        public int X;
        public int Y;
    }

    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    private IntPtr _hWnd;
    private Process _process;
    private IntPtr _hdc;

    private int _frameCount;
    private double _prevTime;

    private int _currentClientWidth;
    private int _currentClientHeight;

    public double Fps { get; private set; }

    public FpsCounter(Process process)
    {
        _process = process;
    }

    public void Start()
    {
        _hWnd = FindWindow(null, "YourGame"); // 替换为你的游戏窗口标题

        if (_hWnd == IntPtr.Zero)
        {
            throw new ArgumentException("Unable to find the game window.");
        }

        RECT rect;
        GetClientRect(_hWnd, out rect);

        _hdc = CreateDC("DISPLAY", null, null, IntPtr.Zero);

        int screenWidth = GetDeviceCaps(_hdc, DESKTOPHORZRES);
        int screenHeight = GetDeviceCaps(_hdc, DESKTOPVERTRES);

        DeleteDC(_hdc);

        int dpiX;
        int dpiY;

        using (var managementObject = new ManagementObject("Win32_DesktopMonitor.DisplayName='Display'"))
        {
            dpiX = int.Parse(managementObject["PixelsPerXLogicalInch"].ToString());
            dpiY = int.Parse(managementObject["PixelsPerYLogicalInch"].ToString());
        }

        _currentClientWidth = ((rect.Right - rect.Left) * dpiX) / 96;
        _currentClientHeight = ((rect.Bottom - rect.Top) * dpiY) / 96;

        _prevTime = GetTimeInSeconds();
        _frameCount = 0;

        // Start the frame counting loop in a separate thread
        new System.Threading.Thread(CountFrames).Start();
    }

    private double GetTimeInSeconds()
    {
        return (double)Stopwatch.GetTimestamp() / Stopwatch.Frequency;
    }

    private void CountFrames()
    {
        while (true)
        {
            IntPtr hdc = CreateDC("DISPLAY", null, null, IntPtr.Zero);

            int currentScreenWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
            int currentScreenHeight = GetDeviceCaps(hdc, DESKTOPVERTRES);

            DeleteDC(hdc);

            RECT rect;
            GetClientRect(_hWnd, out rect);

            int dpiX;
            int dpiY;

            using (var managementObject = new ManagementObject("Win32_DesktopMonitor.DisplayName='Display'"))
            {
                dpiX = int.Parse(managementObject["PixelsPerXLogicalInch"].ToString());
                dpiY = int.Parse(managementObject["PixelsPerYLogicalInch"].ToString());
            }

            int currentClientWidth = ((rect.Right - rect.Left) * dpiX) / 96;
            int currentClientHeight = ((rect.Bottom - rect.Top) * dpiY) / 96;

            int changedPixels = Math.Abs(currentClientWidth - _currentClientWidth) * Math.Abs(currentClientHeight - _currentClientHeight);

            if (changedPixels > 0)
            {
                _frameCount++;
            }

            double currentTime = GetTimeInSeconds();
            double elapsedTime = currentTime - _prevTime;

            if (elapsedTime >= 1)
            {
                Fps = _frameCount / elapsedTime;

                Console.WriteLine($"FPS: {Fps}");

                _frameCount = 0;
                _prevTime = currentTime;
            }

            _currentClientWidth = currentClientWidth;
            _currentClientHeight = currentClientHeight;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Process process = Process.GetProcessesByName("YourGame")[0]; // 替换为你的游戏进程名称

        FpsCounter fpsCounter = new FpsCounter(process);
        fpsCounter.Start();

        Console.ReadLine();
    }

    private static AutomationElement FindGameWindowElement(string gameWindowTitle, string gameWindowClassName)
    {
        AutomationElement desktop = AutomationElement.RootElement;
        Condition condition = new AndCondition(
            new PropertyCondition(AutomationElement.NameProperty, gameWindowTitle),
            new PropertyCondition(AutomationElement.ClassNameProperty, gameWindowClassName));

        AutomationElement gameWindowElement = desktop.FindFirst(TreeScope.Children, condition);
        return gameWindowElement;
    }
}*/