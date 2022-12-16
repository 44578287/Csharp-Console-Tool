using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;


string 当前路径 = System.IO.Directory.GetCurrentDirectory();


/*string text = System.IO.File.ReadAllText(当前路径+ "\\原文件位置\\1.txt");
System.Console.WriteLine("Contents of WriteText.txt = {0}", text);*/

/*string[] lines = System.IO.File.ReadAllLines(当前路径+"\\1.txt");
System.Console.WriteLine("Contents of WriteLines2.txt = ");
foreach (string line in lines)
{
    Console.WriteLine("\t" + line);
}*/



/*var path = @"C:\Users\Jano\Documents\thermopylae.txt";

string content = File.ReadAllText(path, Encoding.UTF8);
Console.WriteLine(content);*/



 void loadTXT(string filePath)
{
    StreamReader sr = new StreamReader(File.Open(filePath, FileMode.Open), Encoding.GetEncoding("GB2312"));
    ArrayList mydata = new ArrayList();
    while (true)
    {
        // 从上到下读取每一行数据
        string line = sr.ReadLine();
        if (line == string.Empty || line == null) break;
        char[] separator = { '\t' };
        string[] data = line.Split(separator);

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].Trim() != string.Empty)
            {
                // 保存当前行的每一列数据
                mydata.Add(data[i].Trim());
            }
        }
    }
}