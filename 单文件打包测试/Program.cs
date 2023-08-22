// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


DocTreeHelper.PrintTree(AppDomain.CurrentDomain.BaseDirectory);

Console.ReadKey();

public class DocTreeHelper
{
    /// <summary>
    /// 输出目录结构树
    /// </summary>
    /// <param name="dirpath">被检查目录</param>
    public static void PrintTree(string dirpath)
    {
        if (!Directory.Exists(dirpath))
        {
            throw new Exception("文件夹不存在");
        }

        PrintDirectory(dirpath, 0, "");
    }

    /// <summary>
    /// 将目录结构树输出到指定文件
    /// </summary>
    /// <param name="dirpath">被检查目录</param>
    /// <param name="outputpath">输出到的文件</param>
    public static void PrintTree(string dirpath, string outputpath)
    {
        if (!Directory.Exists(dirpath))
        {
            throw new Exception("文件夹不存在");
        }

        //将输出流定向到文件 outputpath
        StringWriter swOutput = new StringWriter();
        Console.SetOut(swOutput);

        PrintDirectory(dirpath, 0, "");

        //将输出流输出到文件 outputpath
        File.WriteAllText(outputpath, swOutput.ToString());

        //将输出流重新定位回文件 outputpath
        StreamWriter swConsole = new StreamWriter(
            Console.OpenStandardOutput(), Console.OutputEncoding);
        swConsole.AutoFlush = true;
        Console.SetOut(swConsole);
    }

    /// <summary>
    /// 打印目录结构
    /// </summary>
    /// <param name="dirpath">目录</param>
    /// <param name="depth">深度</param>
    /// <param name="prefix">前缀</param>
    public static void PrintDirectory(string dirpath, int depth, string prefix)
    {
        DirectoryInfo dif = new DirectoryInfo(dirpath);

        //打印当前目录
        if (depth == 0)
        {
            Console.WriteLine(prefix + dif.Name);
        }
        else
        {
            Console.WriteLine(prefix.Substring(0, prefix.Length - 2) + "| ");
            Console.WriteLine(prefix.Substring(0, prefix.Length - 2) + "|-" + dif.Name);
        }

        //打印目录下的目录信息
        for (int counter = 0; counter < dif.GetDirectories().Length; counter++)
        {
            DirectoryInfo di = dif.GetDirectories()[counter];
            if (counter != dif.GetDirectories().Length - 1 ||
                dif.GetFiles().Length != 0)
            {
                PrintDirectory(di.FullName, depth + 1, prefix + "| ");
            }
            else
            {
                PrintDirectory(di.FullName, depth + 1, prefix + "  ");
            }
        }

        //打印目录下的文件信息
        for (int counter = 0; counter < dif.GetFiles().Length; counter++)
        {
            FileInfo f = dif.GetFiles()[counter];
            if (counter == 0)
            {
                Console.WriteLine(prefix + "|");
            }
            Console.WriteLine(prefix + "|-" + f.Name);
        }
    }
}