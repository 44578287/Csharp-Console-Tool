/*
总结
创建料夹调用
System.IO.Directory.CreateDirectory(新建完整路径);
例如:System.IO.Directory.CreateDirectory(C:\xxxxx\xxxxx\这是指令所创建资料夹);
在C:\xxxxx\xxxxx 下就会一个名为"这是指令所创建资料夹",的资料夹

复制文件调用
System.IO.File.Copy(当前路径+"/原文件位置/文件名.xxx", 当前路径+"/目标位置/文件名.xxx", true); true定义覆盖文件
移动文件调用
System.IO.File.Move(当前路径+"/原文件位置/文件名.xxx", 当前路径+"/目标位置/文件名.xxx");


移动文件夹调用
System.IO.Directory.Move(当前路径+"/原文件位置/XXXX文件夹",当前路径+"/原文件位置/XXXX文件夹");

删除文件调用
System.IO.File.Delete(目标路径/文件名.xxx");
例如:System.IO.File.Delete(C:\xxxxx\xxxx.xxx);
这样会删除C:\xxxx\xxxx.xxx文件

删除文件夹调用
System.IO.Directory.Delete(当前路径 + "/测试文件夹", true); true定义强制删除即使里面有文件
例如:System.IO.Directory.Delete(C:\xxxxx\xxxxxx);
这样会删除C:\xxxx\xxxxxxx文件夹

 
测试文件夹是否存在
Directory.Exists(要检测的文件夹路径);
例如:Directory.Exists(C:\xxxxx\123);
如果xxxx下有123文件夹则回传True
如果xxxx下没有123文件夹则回传False
 
测试文件是否存在
File.Exists(要检测的文件夹路径\文件名);
例如:File.Exists(C:\xxxxx\123.txt);
如果xxxx下有123.txt文件则回传True
如果xxxx下没有123.txt文件则回传False


foreach 语句作用遍历循环
foreach (int x in myArray)
foreach (目标新值 in myArray)
具体:https://zh.wikipedia.org/wiki/Foreach%E5%BE%AA%E7%8E%AF

搜索文件并使用*作为万用字符
DirectoryInfo di = new DirectoryInfo(当前路径);
foreach (var fi in di.GetFiles())
{
    Console.WriteLine(fi.Name);
}
di为搜索路径目录
di.GetFiles() ()中可填搜索指示,例如:*.txt则会输出指定路径文件夹下的所有.txt文件
具体:https://docs.microsoft.com/zh-cn/dotnet/api/system.io.directoryinfo.getfiles?view=net-6.0


搜索文件夹并使用*作为万用字符
public static bool 搜索文件夹(string 目标路径, string 文件夹名)
        {
            string[] dirs = Directory.GetDirectories(目标路径, 文件夹名, SearchOption.TopDirectoryOnly);
            Console.WriteLine("The number of directories starting with p is {0}.", dirs.Length);
            foreach (string dir in dirs)
            {
                Console.WriteLine(dir);
                return true;
            }
            return false;
        }

 */




string 当前路径 = System.IO.Directory.GetCurrentDirectory();


/*string pathString = System.IO.Path.Combine(当前路径, "测试文件夹");

Console.WriteLine(pathString);*/

//System.IO.Directory.CreateDirectory(pathString);
//System.IO.Directory.CreateDirectory(当前路径, "测试文件夹");




/*string sourcePath = 当前路径 + "/原文件位置";
string targetPath = 当前路径 + "/目标位置";

string sourceFile = System.IO.Path.Combine(sourcePath, "1.txt");
string destFile = System.IO.Path.Combine(targetPath, "1.txt");

System.IO.File.Copy(sourceFile, destFile, true);*/


//System.IO.File.Copy(当前路径+"/原文件位置/1.txt", 当前路径+"/目标位置/1.txt", true);

/*if (Directory.Exists(当前路径 + "//测试文件夹"))
{
    Console.WriteLine("文件夹存在");


}
else
{
    Console.WriteLine("文件夹不存在");
}*/


/*if (File.Exists(当前路径 + "/测试文件夹/"+"*"+".txt"))
{
    Console.WriteLine("文件夹存在");


}
else
{
    Console.WriteLine("文件夹不存在");
}
*/

/*string path = 当前路径, fileName = "文件夹";
string[] files = Directory.GetDirectories(path, "*" + fileName, System.IO.SearchOption.AllDirectories);
foreach (var item in files)
    Console.WriteLine(item + "\r\t");

DirectoryInfo[] flies = new DirectoryInfo(path).GetDirectories("*" + fileName);
foreach (var item in flies)
    Console.WriteLine(item.FullName + " 创建时间:" + item.CreationTime + "\r\t");



//System.IO.FileInfo[] GetFiles(string searchPattern, System.IO.SearchOption searchOption);

DirectoryInfo di = new DirectoryInfo(当前路径+"/原文件位置");
//string di = 当前路径 + "/原文件位置";
Console.WriteLine(di);
foreach (var fi in di.GetFiles("*.txt"))
{
    Console.WriteLine(fi.Name);
}

//System.IO.Directory.Delete(当前路径 + "/测试文件夹", true);

Console.WriteLine(System.IO.Directory.Exists(当前路径 + "/测试文件夹"));*/


//System.IO.Directory.Move("C:/Users/g9964/Desktop/vs2022/测试1_控制台/资料处理/bin/Debug/net6.0/123321", "C:/Users/g9964/Desktop/vs2022/测试1_控制台/资料处理/bin/Debug/net6.0/测试文件夹");//移动解压文件夹"


bool MoveFolder(string sourcePath, string destPath)
{
    bool result = false;
    if (Directory.Exists(sourcePath))
    {
        DirectoryInfo folder = new DirectoryInfo(sourcePath);

        if (!Directory.Exists(destPath))
        {
            //目标目录不存在则创建
            try
            {
                Directory.CreateDirectory(destPath);
            }
            catch (Exception ex)
            {
                throw new Exception("创建目标目录失败：" + ex.Message);
            }
        }

        #region 移动文件夹内所有文件
        //获得源文件下所有文件
        List<string> files = new List<string>(Directory.GetFiles(sourcePath));
        files.ForEach(c =>
        {
            string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
            //覆盖模式
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            File.Move(c, destFile);
        });
        #endregion

        #region 移动获得源文件下所有目录文件

        List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
        folders.ForEach(c =>
        {
            string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
            //Directory.Move必须要在同一个根目录下移动才有效，不能在不同卷中移动。
            //Directory.Move(c, destDir);

            //采用递归的方法实现
            MoveFolder(c, destDir);
        });
        #endregion

        Directory.Delete(sourcePath);

        result = true;
    }
    else
    {
        result = false;
        throw new DirectoryNotFoundException("源目录不存在！");
    }
    return result;
}


MoveFolder(当前路径+"/123321", 当前路径+"/测试文件夹");


