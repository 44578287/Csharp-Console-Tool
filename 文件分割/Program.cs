// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using 文件分割;
using static 文件分割.Class1;

//Console.WriteLine("Hello, World!");


//string 测试文本 = "0123456789";
//byte[] Bytes = System.Text.Encoding.Default.GetBytes(测试文本);
//Encoding.Default.GetString(Bytes);
//Console.WriteLine($"Bytes长度:{Bytes.Length}");
//Console.WriteLine(Encoding.Default.GetString(Bytes));
/*FileStream 文件流 = new("C:\\Users\\g9964\\Downloads\\WriteText.txt", FileMode.Open, FileAccess.Read);
long 文件大小 = 文件流.Length;
int 分片大小 = 51200;
int 当前读取长度 = 0;
long 剩余文件长度 = 文件大小;
List<object> 文件分片列表= new ();
List<string> 文件分片列表2 = new();
byte[] bytes = new byte[分片大小];//存储读取结果  
for (int i = 0;i< 文件大小 / 分片大小;i++)
{
    文件流.Read(bytes, 0, 分片大小);
    文件分片列表.Add(bytes);
    文件分片列表2.Add(Encoding.Default.GetString(bytes));
    //Console.WriteLine(Encoding.Default.GetString(暂存));
    //Console.WriteLine($"bytes {当前读取长度}-{当前读取长度+ 分片大小}/{文件大小}");
    当前读取长度 += 分片大小;
}
byte[] 暂存 = new byte[(int)文件大小 % 分片大小];
文件流.Read(暂存,0, (int)文件大小 % 分片大小);
文件分片列表.Add(暂存);
文件分片列表2.Add(Encoding.Default.GetString(暂存));
//Console.WriteLine(Encoding.Default.GetString(暂存));
foreach (var DataIn in 文件分片列表)
    Console.WriteLine(Encoding.Default.GetString((byte[])DataIn));*/
//Console.WriteLine((DataIn));

/*int TPM = 0;
string text ="";
for (int i = 0; i < 1024*153; i++)
{
    if (i % 51200 == 0&&i!=1&&i!=0)
    {
        text += TPM++;
    }
    else
        text += "-";
}

Console.WriteLine("开始写入");
await File.WriteAllTextAsync("WriteText.txt", text);*/

/*FileStream WriteText1 = new("WriteText.txt", FileMode.Open, FileAccess.Read);
FileStream WriteText2 = new("WriteText (1).txt", FileMode.Open, FileAccess.Read);

byte[] WriteText1Byer = new byte[WriteText1.Length];
byte[] WriteText2Byer = new byte[WriteText2.Length];
Console.WriteLine($"1:{WriteText1Byer.Length} 2:{WriteText2Byer.Length}");


for (int i = 0; i < WriteText1Byer.Length; i++)
{
    if (WriteText1Byer[i] != WriteText2Byer[i])
        Console.WriteLine($"{i}不正常");


}*/
/*int SliceSize = 51200;
FileStream FileStream = new("C:\\Users\\g9964\\Downloads\\WriteText.txt", FileMode.Open, FileAccess.Read);
List<byte[]> DataList = new();
byte[] DataByteTpm = new byte[SliceSize];
//for (int i = 0; i< FileStream.Length / SliceSize;i++)
ByteClass ByteClass = new();
long 读取进度 = 0;
do
{
    FileStream.Read(ByteClass.BytesIn,0,SliceSize);
    DataList.Add(ByteClass.ByteArrayGet());
    读取进度 += SliceSize;
    FileStream.Seek(0, (SeekOrigin)读取进度);
    
}
while ((FileStream.Position / SliceSize) < 0);
foreach (var datain in DataList)
    Console.WriteLine(Encoding.Default.GetString(datain));
*/
/*data.Add(data2);
data2 = System.Text.Encoding.Default.GetBytes("2222");
data.Add(data2);
data2 = System.Text.Encoding.Default.GetBytes("1113333311");
data.Add(data2);
data2 = System.Text.Encoding.Default.GetBytes("111777711");
data.Add(data2);
foreach (var datain in data)
    Console.WriteLine(Encoding.Default.GetString(datain));*/

/*List<byte[]> ListByteArray = new();
FileStream fs = File.OpenRead("C:\\Users\\g9964\\Downloads\\WriteText.txt");
byte[] arr = new byte[51200];
//while (fs.Read(arr, 0, arr.Length) >= arr.Length)
for(int i=0; i<2;i++)
{
    fs.Read(arr, 0, arr.Length);
    ListByteArray.Add(arr);
    //Console.WriteLine(Encoding.Default.GetString(arr));
}
foreach (var DataIn in ListByteArray)
{
    Console.WriteLine(Encoding.Default.GetString(arr));
}*/



/*// 声明一个字节数组列表
List<byte[]> byteArrays = new List<byte[]>();
// 定义分片大小（这里为1024字节）
int chunkSize = 51200;
// 使用using语句打开FileStream以读取文件
using (FileStream fs = new FileStream("WriteText.txt", FileMode.Open))
{
    int bytesRead;
    // 创建一个字节数组以存储读取的分片数据
    byte[] buffer = new byte[chunkSize];
    // 循环读取数据，直到读取的字节数小于等于0
    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
    {
        // 创建一个大小为实际读取的字节数的字节数组
        byte[] data = new byte[bytesRead];
        // 从读取的分片数据中复制字节到新的字节数组中
        System.Array.Copy(buffer, data, bytesRead);
        // 将字节数组添加到列表中
        byteArrays.Add(data);
    }
    Console.WriteLine(fs.Length);
}*/

// 声明一个字节数组列表
/*List<byte[]> byteArrays = new List<byte[]>();

// 使用using语句打开FileStream以读取文件
using (FileStream fs = new FileStream("WriteText.txt", FileMode.Open))
{
    // 定义分片大小（这里为1024字节）
    int chunkSize = 51200;
    int bytesRead;
    int totalBytesRead = 0;
    // 获取文件的总字节数
    int fileLength = (int)fs.Length;
    // 创建一个字节数组以存储读取的分片数据
    byte[] buffer = new byte[chunkSize];
    // 循环读取数据，直到读取的字节数小于等于0
    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
    {
        // 增加读取的字节总数
        totalBytesRead += bytesRead;
        // 显示读取进度（以百分比形式）
        Console.WriteLine("Reading Progress: {0}%", (totalBytesRead * 100) / fileLength);
        // 创建一个大小为实际读取的字节数的字节数组
        byte[] data = new byte[bytesRead];
        // 从读取的分片数据中复制字节到新的字节数组中
        System.Array.Copy(buffer, data, bytesRead);
        // 将字节数组添加到列表中
        byteArrays.Add(data);
    }
}

foreach (var DataIn in byteArrays)
{
    Console.WriteLine(Encoding.Default.GetString(DataIn));
    Console.WriteLine(DataIn.Length);
}*/




// 要读取的文件路径
string file = "WriteText.txt";
// 设置缓冲区大小（字节数），可自行设置
int bufferSize = 51200;

/*List<string> list = Class1.ReadFileBySlice(file, bufferSize);
foreach (var item in list)
{
    Console.WriteLine($"{item}");
}
Console.ReadLine();*/

/*FilePartitionRead FilePartitionRead = new(file, bufferSize);
List<byte[]> ByteList = FilePartitionRead.Execute();

foreach (var item in ByteList)
{
    Console.WriteLine(Encoding.Default.GetString(item));
}*/
/*FileStream FileStream = new(file,FileMode.Open);
FileChunkReader FileChunkReader = new(FileStream, bufferSize);
List<byte[]> ByteList = FileChunkReader.ReadFile();
foreach (var item in ByteList)
{
    Console.WriteLine(item.Length);
    Console.WriteLine(Encoding.Default.GetString(item));
}
FileStream.Close();
*/
FileSliceReader FileSliceReader = new(file, bufferSize);
List<byte[]> ByteList = new List<byte[]>();
ByteList = FileSliceReader.ReadFile();
long ReadSize = 0;
foreach (var item in ByteList)
{
    //Console.WriteLine(item.Count());
    ReadSize += item.Length;
    Console.WriteLine($"bytes {ReadSize- item.Length}-{ReadSize-1}/{FileSliceReader.ReadSize}");
    
    //Console.WriteLine(Encoding.Default.GetString(item));
}
/*foreach (var item in FileSliceReader.SliceDataSize)
{
    Console.WriteLine($"{item.Key}-{item.Value}");
}*/