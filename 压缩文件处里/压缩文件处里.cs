/*
权限说明
ZipArchiveMode.Create	1	只允许创建新的存档项。
ZipArchiveMode.Read	    0	只允许读取存档项。
ZipArchiveMode.Update	2	允许对存档项执行读取和写入操作。

压缩级别说明
CompressionLevel.Fastest	    1	即使结果文件未可选择性地压缩，压缩操作也应尽快完成。
CompressionLevel.NoCompression	2	该文件不应执行压缩。
CompressionLevel.Optimal	    0	即使操作要用更长的时间来完成，也应该可选性地压缩压缩操作。
CompressionLevel.SmallestSize	3	压缩操作应尽可能小地创建输出，即使该操作需要更长的时间才能完成。

打开压缩包
ZipFile.Open(压缩文件位置,权限);

压缩文件
ZipFile.CreateFromDirectory(压缩目标文件目录,存档位置包刮文件名);
ZipFile.CreateFromDirectory(压缩目标文件目录,存档位置包刮文件名,压缩级别,是否包含压缩目标目录);

解压文件
ZipFile.ExtractToDirectory(压缩文件位置, 解压位置目录);
ZipFile.ExtractToDirectory(压缩文件位置, 解压位置目录,是否覆盖文件);

创建空压缩包
ZipArchive.CreateEntry(存档位置包刮文件名);
ZipArchive.CreateEntry(存档位置包刮文件名,压缩级别);

检索 zip 存档中的文件
ZipArchive.GetEntry(压缩文件位置);

检索 zip 存档中的所有文件
ZipArchive.Entries(压缩文件位置);


public static void 万能解压(string 文件位置,string 解压位置)
        {
            Stream stream = File.OpenRead(文件位置);
            var reader = ReaderFactory.Open(stream);
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    //Console.WriteLine(reader.Entry.Key);
                    reader.WriteEntryToDirectory(解压位置, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
        }
        public static string [] ? 万能解压返回接压内容(string 文件位置, string 解压位置)
        {
            Stream stream = File.OpenRead(文件位置);
            var reader = ReaderFactory.Open(stream);
            List<string> ? 解压内容 = null;
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    //Console.WriteLine(reader.Entry.Key);
                    解压内容.Add(reader.Entry.Key);
                    reader.WriteEntryToDirectory(解压位置, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                }
            }
            return 解压内容.ToArray();
        }
*/
using SharpCompress.Common;
using SharpCompress.Readers;
using System.IO.Compression;

string 当前路径 = System.Environment.CurrentDirectory;

Console.WriteLine("加解压测试");

string 文件位置 = 当前路径 + "/Data" + "/LOGO.zip";
//string 文件位置 = 当前路径 + "/Data"+ "/光影-2.zip";
//string 文件位置 = 当前路径 + "/Data" + "/1.rar";
string 解压位置 = 当前路径 + "/Data";

//ZipFile.ExtractToDirectory(文件位置, 解压位置);


//ZipFile.CreateFromDirectory(解压位置, 当前路径 + "/2.zip");
//ZipFile.CreateFromDirectory(当前路径+ "/待压缩", 当前路径 + "/已压缩/1.zip", CompressionLevel.Fastest, true);

//ZipFile.ExtractToDirectory(文件位置, 解压位置);

/*using (Stream stream = File.OpenRead(文件位置))
{
	var reader = ReaderFactory.Open(stream);
	while (reader.MoveToNextEntry())
	{
		if (!reader.Entry.IsDirectory)
		{
			Console.WriteLine(reader.Entry.Key);
			reader.WriteEntryToDirectory(解压位置, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
		}
	}
}*/



Console.WriteLine("完成");