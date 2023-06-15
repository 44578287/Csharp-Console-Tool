using System.IO.Compression;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using Yitter.IdGenerator;

var Jsonoptions = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
    WriteIndented = true
};

var options = new IdGeneratorOptions();
options.WorkerIdBitLength = 1;
options.SeqBitLength = 10;
YitIdHelper.SetIdGenerator(options);

DateTime beforDT = System.DateTime.Now;

List<FileStructure> StructureList = new();
DirectoryData DirectoryData = new();
List<Task> TaskList = new();
//File File = new();
//StructureList = File.FindFileInDir("C:\\Users\\g9964\\Pictures");
FileTool.Director("C:\\Users\\g9964\\Pictures", StructureList, DirectoryData, TaskList: TaskList);
//FileTool.Director("C:\\Projects", StructureList, DirectoryData, TaskList: TaskList);
Task.WaitAll(TaskList.ToArray());

/*var obj = new JObject();
var ObjData = new JArray();*/
JsonObject obj = new();
JsonArray ObjData = new();
for (int i = 0; i < StructureList.Count; i++)
{
    var TempJsonObj = new JsonObject();
    var TempJsonObjData = new JsonObject();
    TempJsonObjData.Add("ID", StructureList[i].ID);
    TempJsonObjData.Add("Name", StructureList[i].Name);
    TempJsonObjData.Add("Size", StructureList[i].Size);
    TempJsonObjData.Add("Time", StructureList[i].Time);
    TempJsonObjData.Add("Depth", StructureList[i].Depth);
    TempJsonObjData.Add("UniqueIdentifier", StructureList[i].UniqueIdentifier);
    TempJsonObjData.Add("Previous", StructureList[i].Previous);
    TempJsonObjData.Add("FullPath", StructureList[i].FullPath);
    TempJsonObjData.Add("AbsolutePath", StructureList[i].AbsolutePath);
    TempJsonObj.Add(StructureList[i].Type, TempJsonObjData);
    ObjData.Add(TempJsonObj);
    //Console.WriteLine($"({StructureList[i].Type}){StructureList[i].Previous}-{StructureList[i].Name}-{StructureList[i].Time}-{StructureList[i].Size}-{StructureList[i].Depth}-{StructureList[i].UniqueIdentifier}-{StructureList[i].ID}");
}
obj.Add("Path", ObjData);
Console.WriteLine(DirectoryData.FilesNumber);
Console.WriteLine(DirectoryData.DirectoryNumber);
Console.WriteLine(DirectoryData.Size);
Console.WriteLine(DirectoryData.Depth);

//Console.WriteLine(File.GetFileMD5("C:/Projects/Enigmavb_10.20.20230201_Single.zip"));

//Console.WriteLine(obj.ToJsonString());

//string text = GZipUtil.CompressString(obj.ToJsonString(Jsonoptions));
string text = obj.ToString();
System.IO.File.WriteAllText("WriteText.Json", text);
//Console.WriteLine(obj.SelectTokens("$.Path[*].File").ToList()[0]);
/*int TempCount = obj.SelectTokens("$.Path[*].File.AbsolutePath").Count();
var Key = obj.SelectTokens("$.Path[*].File.AbsolutePath").ToList();
var Value = obj.SelectTokens("$.Path[*].File.UniqueIdentifier").ToList();
Dictionary<string, string> Dictionary = new();
for (int i = 0; i < TempCount; i++)
{
    Dictionary.Add((string)Key[i]!, (string)Value[i]!);
    //Console.WriteLine($"{(string)Key[i]!}---{(string)Value[i]!}");
}
Console.WriteLine(Dictionary.Count);*/

Console.WriteLine("DateTime总共花费{0}ms.", System.DateTime.Now.Subtract(beforDT).TotalMilliseconds);

/*IBloomFilter bf = FilterBuilder.Build(10000000, 0.0001);
bf.Add("1");
bf.Add("22");
bf.Add("333");
bf.Add("4444");
bf.Add("1");

Console.WriteLine(bf.Contains("11"));*/

//Console.WriteLine(File.GetFileSHA256("C:\\基础新(2022-12-11)\\基础新(2022-12-11)\\v2rayN-With-Core.zip"));
//Console.WriteLine(File.GetFileMD5("C:\\基础新(2022-12-11)\\基础新(2022-12-11)\\v2rayN-With-Core.zip"));

public class File
{
    #region 声明WIN32API函数以及结构 **************************************

    [Serializable,
    System.Runtime.InteropServices.StructLayout
      (System.Runtime.InteropServices.LayoutKind.Sequential,
      CharSet = System.Runtime.InteropServices.CharSet.Auto
      ),
    System.Runtime.InteropServices.BestFitMapping(false)]
    private struct WIN32_FIND_DATA
    {
        public int dwFileAttributes;
        public int ftCreationTime_dwLowDateTime;
        public int ftCreationTime_dwHighDateTime;
        public int ftLastAccessTime_dwLowDateTime;
        public int ftLastAccessTime_dwHighDateTime;
        public int ftLastWriteTime_dwLowDateTime;
        public int ftLastWriteTime_dwHighDateTime;
        public int nFileSizeHigh;
        public int nFileSizeLow;
        public int dwReserved0;
        public int dwReserved1;

        [System.Runtime.InteropServices.MarshalAs
          (System.Runtime.InteropServices.UnmanagedType.ByValTStr,
          SizeConst = 260)]
        public string cFileName;

        [System.Runtime.InteropServices.MarshalAs
          (System.Runtime.InteropServices.UnmanagedType.ByValTStr,
          SizeConst = 14)]
        public string cAlternateFileName;
    }

    [System.Runtime.InteropServices.DllImport
      ("kernel32.dll",
      CharSet = System.Runtime.InteropServices.CharSet.Auto,
      SetLastError = true)]
    private static extern IntPtr FindFirstFile(string pFileName, ref WIN32_FIND_DATA pFindFileData);

    [System.Runtime.InteropServices.DllImport
      ("kernel32.dll",
      CharSet = System.Runtime.InteropServices.CharSet.Auto,
      SetLastError = true)]
    private static extern bool FindNextFile(IntPtr hndFindFile, ref WIN32_FIND_DATA lpFindFileData);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FindClose(IntPtr hndFindFile);

    #endregion 声明WIN32API函数以及结构 **************************************

    //具体方法函数

    private Stack<string> m_scopes = new Stack<string>();
    private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
    private WIN32_FIND_DATA FindFileData;
    private System.IntPtr hFind = INVALID_HANDLE_VALUE;

    public List<FileStructure> FindFileInDir(string rootDir)
    {
        string path = rootDir;
        List<FileStructure> StructureList = new();
    start:
#pragma warning disable SYSLIB0003 // 类型或成员已过时
        new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Path.Combine(path, ".")).Demand();
#pragma warning restore SYSLIB0003 // 类型或成员已过时
        if (path[path.Length - 1] != '\\')
        {
            path = path + "\\";
        }
        //Console.WriteLine("文件夹为：" + path);
        StructureList.Add(new() { Name = FindFileData.cFileName, Previous = path });
        hFind = FindFirstFile(Path.Combine(path, "*"), ref FindFileData);
        if (hFind != INVALID_HANDLE_VALUE)
        {
            do
            {
                if (FindFileData.cFileName.Equals(@".") || FindFileData.cFileName.Equals(@".."))
                    continue;
                if ((FindFileData.dwFileAttributes & 0x10) != 0)
                {
                    m_scopes.Push(Path.Combine(path, FindFileData.cFileName));
                }
                else
                {
                    //GetFileMD5(path + FindFileData.cFileName);
                    StructureList.Add(new() { Previous = path, Name = FindFileData.cFileName });
                    //Console.WriteLine(FindFileData.cFileName + "-----------" + GetFileMD5(path + FindFileData.cFileName));
                }
            }
            while (FindNextFile(hFind, ref FindFileData));
        }
        FindClose(hFind);
        if (m_scopes.Count > 0)
        {
            path = m_scopes.Pop();
            goto start;
        }
        return StructureList;
    }

    public static void Director(string dir, List<FileStructure> StructureList, DirectoryData? DirectoryData = null, int Depth = 0)
    {
        DirectoryData ??= new();
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] files = d.GetFiles();//文件
        DirectoryInfo[] directs = d.GetDirectories();//文件夹
        foreach (FileInfo f in files)
        {
            DirectoryData.FilesNumber++;
            DirectoryData.Size += f.Length;
            StructureList.Add(new() { FullPath = f.DirectoryName, Previous = d.Name, Name = f.Name, Type = f.Extension, Size = f.Length, Time = f.LastWriteTime, Depth = Depth, /*UniqueIdentifier = GetFileMD5(f.FullName)*/ });
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo dd in directs)
        {
            DirectoryData.DirectoryNumber++;
            StructureList.Add(new() { FullPath = d.FullName, Previous = d.Name, Name = dd.Name, Time = d.LastWriteTime, Depth = Depth });
            Director(dd.FullName, StructureList, DirectoryData, ++Depth);
            if (Depth > DirectoryData.Depth) DirectoryData.Depth = Depth;
            --Depth;
        }
        DirectoryData.Time = d.LastWriteTime;
    }

    public static string? GetFileSHA256(string FilePath)
    {
        try
        {
            FileStream stream = new FileStream(FilePath, FileMode.Open);
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(stream);
            return BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        }
        catch
        {
            return null;
        }
    }

    public static string? GetFileMD5(string FilePath)
    {
        using var md5 = MD5.Create();
        using FileStream stream = new(FilePath, FileMode.Open);
        //计算文件的MD5值
        byte[] hashValue = md5.ComputeHash(stream);

        //将字节数组转换成十六进制格式的字符串
        return BitConverter.ToString(hashValue).Replace("-", "");
    }

    public static string? GetFileCRC32(string FilePath)
    {
        System.IO.Hashing.Crc32 crc32 = new();
        FileStream fileStream = new FileStream(FilePath, FileMode.Open);
        crc32.Append(fileStream);
        Console.WriteLine(System.Text.Encoding.UTF8.GetString(crc32.GetCurrentHash()));
        fileStream.Close();
        return null;
    }

    /// <summary>
    ///  计算指定文件的SHA1值
    /// </summary>
    /// <param name="fileName">指定文件的完全限定名称</param>
    /// <returns>返回值的字符串形式</returns>
    public static String GetFileSHA1(String fileName)
    {
        String hashSHA1 = String.Empty;
        //检查文件是否存在，如果文件存在则进行计算，否则返回空值
        if (System.IO.File.Exists(fileName))
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                //计算文件的SHA1值
                System.Security.Cryptography.SHA1 calculator = System.Security.Cryptography.SHA1.Create();
                Byte[] buffer = calculator.ComputeHash(fs);
                calculator.Clear();
                //将字节数组转换成十六进制的字符串形式
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    stringBuilder.Append(buffer[i].ToString("x2"));
                }
                hashSHA1 = stringBuilder.ToString();
            }//关闭文件流
        }
        return hashSHA1;
    }//ComputeSHA1
}

/// <summary>
/// 文件结构
/// </summary>
public class FileStructure
{
    /// <summary>
    /// ID
    /// </summary>
    public double? ID { get; set; }

    /// <summary>
    /// 绝对路径
    /// </summary>
    public string? AbsolutePath { get; set; }

    /// <summary>
    /// 完整路径
    /// </summary>
    public string? FullPath { get; set; }

    /// <summary>
    /// 上级目录
    /// </summary>
    public string Previous { get; set; } = "/";

    /// <summary>
    /// 类型 默认文件夹
    /// </summary>
    public string Type { get; set; } = "Directory";

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = "不应该没有名字!!";

    /// <summary>
    /// 大小
    /// </summary>
    public double Size { get; set; } = 0;

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// 深度
    /// </summary>
    public int Depth { get; set; } = 0;

    /// <summary>
    /// 唯一标识
    /// </summary>
    public string? UniqueIdentifier { get; set; }
}

/// <summary>
/// 目录结构
/// </summary>
public class DirStructure
{
    /// <summary>
    /// ID
    /// </summary>
    public double? ID { get; set; }

    /// <summary>
    /// 绝对路径
    /// </summary>
    public string? AbsolutePath { get; set; }

    /// <summary>
    /// 完整路径
    /// </summary>
    public string? FullPath { get; set; }

    /// <summary>
    /// 上级目录
    /// </summary>
    public string Previous { get; set; } = "/";

    /// <summary>
    /// 类型 默认文件夹
    /// </summary>
    public string Type { get; set; } = "Directory";

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = "不应该没有名字!!";

    /// <summary>
    /// 大小
    /// </summary>
    public double Size { get; set; } = 0;

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// 深度
    /// </summary>
    public int Depth { get; set; } = 0;

    /// <summary>
    /// 唯一标识
    /// </summary>
    public string? UniqueIdentifier { get; set; }
}

/// <summary>
/// 目录遍历返回
/// </summary>
public class DirectoryData
{
    /// <summary>
    /// 文件数量
    /// </summary>
    public double FilesNumber { get; set; } = 0;

    /// <summary>
    /// 目录数量
    /// </summary>
    public double? DirectoryNumber { get; set; } = 0;

    /// <summary>
    /// 总大小
    /// </summary>
    public double? Size { get; set; } = 0;

    /// <summary>
    /// 深度
    /// </summary>
    public int Depth { get; set; } = 0;

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }
}

public class FileTool
{
    public static string? GetFileMD5(string FilePath)
    {
        try
        {
            using var md5 = MD5.Create();
            using FileStream stream = new(FilePath, FileMode.Open);
            //计算文件的MD5值
            byte[] hashValue = md5.ComputeHash(stream);

            //将字节数组转换成十六进制格式的字符串
            return BitConverter.ToString(hashValue).Replace("-", "");
        }
        catch
        {
            return "计算错误";
        }
    }

    public static void Director(string dir, List<FileStructure> StructureList, DirectoryData? DirectoryData = null, int Depth = 0, List<Task>? TaskList = null, int TempPathLength = -1)
    {
        if (TempPathLength == -1)//设置删除字符串长度
            TempPathLength = dir.Length;
        TaskList ??= new();
        DirectoryData ??= new();
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] files = d.GetFiles();//文件
        DirectoryInfo[] directs = d.GetDirectories();//文件夹
        foreach (FileInfo f in files)
        {
            DirectoryData.FilesNumber++;
            DirectoryData.Size += f.Length;
            TaskList.Add(Task.Run(() =>
            {
                StructureList.Add(new() { AbsolutePath = f.FullName, FullPath = f.DirectoryName?.Remove(0, TempPathLength), Previous = d.Name, Name = f.Name, Type = "File", Size = f.Length, Time = f.LastWriteTime, Depth = Depth, UniqueIdentifier = GetFileMD5(f.FullName), ID = YitIdHelper.NextId() });
            }));
            //StructureList.Add(new() { FullPath = f.DirectoryName, Previous = d.Name, Name = f.Name, Type = f.Extension, Size = f.Length, Time = f.LastWriteTime, Depth = Depth, UniqueIdentifier = GetFileMD5(f.FullName) });
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo dd in directs)
        {
            DirectoryData.DirectoryNumber++;
            StructureList.Add(new() { AbsolutePath = d.FullName, FullPath = d.FullName.Remove(0, TempPathLength), Previous = d.Name, Name = dd.Name, Time = d.LastWriteTime, Depth = Depth, ID = YitIdHelper.NextId() });
            Director(dd.FullName, StructureList, DirectoryData, ++Depth, TempPathLength: TempPathLength);
            if (Depth > DirectoryData.Depth) DirectoryData.Depth = Depth;
            --Depth;
        }
        DirectoryData.Time = d.LastWriteTime;
    }
}

public class GZipUtil
{
    /// <summary>
    /// 字节数组压缩
    /// </summary>
    /// <param name="strSource"></param>
    /// <returns></returns>
    public static byte[] Compress(byte[] data)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(data, 0, data.Length);
            zip.Close();
            byte[] buffer = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(buffer, 0, buffer.Length);
            ms.Close();
            return buffer;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// 字节数组解压缩
    /// </summary>
    /// <param name="strSource"></param>
    /// <returns></returns>
    public static byte[] Decompress(byte[] data)
    {
        try
        {
            MemoryStream ms = new MemoryStream(data);
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
            MemoryStream msreader = new MemoryStream();
            byte[] buffer = new byte[0x1000];
            while (true)
            {
                int reader = zip.Read(buffer, 0, buffer.Length);
                if (reader <= 0)
                {
                    break;
                }
                msreader.Write(buffer, 0, reader);
            }
            zip.Close();
            ms.Close();
            msreader.Position = 0;
            buffer = msreader.ToArray();
            msreader.Close();
            return buffer;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// 字符串压缩
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="str">String.</param>
    public static string CompressString(string str)
    {
        string compressString = "";
        byte[] compressBeforeByte = Encoding.UTF8.GetBytes(str);
        byte[] compressAfterByte = Compress(compressBeforeByte);
        //compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
        compressString = Convert.ToBase64String(compressAfterByte);
        return compressString;
    }

    /// <summary>
    /// 字符串解压缩
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="str">String.</param>
    public static string DecompressString(string str)
    {
        string compressString = "";
        //byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
        byte[] compressBeforeByte = Convert.FromBase64String(str);
        byte[] compressAfterByte = Decompress(compressBeforeByte);
        compressString = Encoding.UTF8.GetString(compressAfterByte);
        return compressString;
    }
}