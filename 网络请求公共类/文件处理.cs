using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace 网络请求公共类
{
    internal class FileProcess
    {
        /// <summary>
        /// 文件分片读取类
        /// </summary>
        public class FileSliceReader
        {
            /// <summary>
            /// 文件路径
            /// </summary>
            private readonly string _filePath;

            /// <summary>
            /// 单个分片大小(字节)
            /// </summary>
            private int _sliceSize;
            /// <summary>
            /// 文件读取记录
            /// </summary>
            public Dictionary<long, long> SliceDataSize = new();
            public long ReadSize = 0;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="filePath">文件路径</param>
            /// <param name="sliceSize">单个分片大小(字节)</param>
            public FileSliceReader(string filePath, int sliceSize)
            {
                if (File.Exists(filePath) == false)
                {
                    throw new FileNotFoundException("文件未找到");
                }
                _filePath = filePath;
                _sliceSize = sliceSize;
            }

            /// <summary>
            /// 读取文件分片，并将每个分片的内容存储在一个List中，返回该List，进行显示进度
            /// </summary>
            /// <returns>每个分片的内容组成的List</returns>
            public List<byte[]> ReadFile()
            {
                var result = new List<byte[]>();
                using (var fs = new FileStream(_filePath, FileMode.Open))
                {
                    if (_sliceSize <= 0)
                    {
                        _sliceSize = (int)fs.Length;
                    }
                    //var i = 0;
                    var remainLength = (int)fs.Length;
                    while (remainLength > 0)
                    {
                        var buffer = new byte[remainLength];
                        if (remainLength > _sliceSize)
                        {
                            buffer = new byte[_sliceSize];
                        }
                        var readLength = fs.Read(buffer, 0, buffer.Length);
                        result.Add(buffer);

                        remainLength -= readLength;
                        //Console.WriteLine($"进度：{100 * ++i / ((fs.Length + _sliceSize - 1) / _sliceSize)}%");
                        ReadSize += readLength;
                        //Console.WriteLine($"{ReadSize}-{0}/{fs.Length}");
                        SliceDataSize.Add(ReadSize, readLength);
                    }
                    fs.Close();
                }
                return result;
            }
            /// <summary>
            /// 分片读取Byte[]
            /// </summary>
            /// <param name="InputData">目标Byte[]</param>
            /// <param name="_sliceSize">分片大小</param>
            /// <returns></returns>
            public static List<byte[]> ReadBytes(byte[] InputData ,long _sliceSize) 
            {
                var result = new List<byte[]>();
                if (_sliceSize <= 0)
                {
                    _sliceSize = InputData.Length;
                }
                int i = 0;
                result = InputData.GroupBy(s => i++ / _sliceSize).Select(s => s.ToArray()).ToList();
                return result;
            }
            /// <summary>
            /// 分片读取文件流
            /// </summary>
            /// <param name="InputData">目标FileStream</param>
            /// <param name="_sliceSize">分片大小</param>
            /// <param name="CloseStream">关闭流</param>
            /// <returns></returns>
            public static List<byte[]> ReadFileStream(FileStream InputData , long _sliceSize,bool CloseStream = true)
            {
                var result = new List<byte[]>();
                if (_sliceSize <= 0)
                {
                    _sliceSize = (int)InputData.Length;
                }
                var remainLength = (int)InputData.Length;
                while (remainLength > 0)
                {
                    var buffer = new byte[remainLength];
                    if (remainLength > _sliceSize)
                    {
                        buffer = new byte[_sliceSize];
                    }
                    var readLength = InputData.Read(buffer, 0, buffer.Length);
                    result.Add(buffer);

                    remainLength -= readLength;
                }
                if(CloseStream)//默认读取完关闭文件流
                    InputData.Close();
                return result;
            }
            /// <summary>
            /// 分片读取内存流
            /// </summary>
            /// <param name="InputData">目标MemoryStream</param>
            /// <param name="_sliceSize">分片大小</param>
            /// <param name="CloseStream">关闭流</param>
            /// <returns></returns>
            public static List<byte[]> ReadMemoryStream(MemoryStream InputData, long _sliceSize, bool CloseStream = true)
            {
                var result = new List<byte[]>();
                if (_sliceSize <= 0)
                {
                    _sliceSize = (int)InputData.Length;
                }
                //var i = 0;
                var remainLength = (int)InputData.Length;
                while (remainLength > 0)
                {
                    var buffer = new byte[remainLength];
                    if (remainLength > _sliceSize)
                    {
                        buffer = new byte[_sliceSize];
                    }
                    var readLength = InputData.Read(buffer, 0, buffer.Length);
                    result.Add(buffer);

                    remainLength -= readLength;
                }
                if (CloseStream)//默认读取完关闭流
                    InputData.Close();
                return result;
            }
            /// <summary>
            /// 分片读取网络流
            /// </summary>
            /// <param name="InputData">目标NetworkStream</param>
            /// <param name="_sliceSize">分片大小</param>
            /// <param name="CloseStream">关闭流</param>
            /// <returns></returns>
            public static List<byte[]> ReadNetworkStream(NetworkStream InputData, long _sliceSize, bool CloseStream = true)
            {
                var result = new List<byte[]>();
                if (_sliceSize <= 0)
                {
                    _sliceSize = (int)InputData.Length;
                }
                //var i = 0;
                var remainLength = (int)InputData.Length;
                while (remainLength > 0)
                {
                    var buffer = new byte[remainLength];
                    if (remainLength > _sliceSize)
                    {
                        buffer = new byte[_sliceSize];
                    }
                    var readLength = InputData.Read(buffer, 0, buffer.Length);
                    result.Add(buffer);

                    remainLength -= readLength;
                }
                if (CloseStream)//默认读取完关闭流
                    InputData.Close();
                return result;
            }
            /// <summary>
            /// 分片读取流
            /// </summary>
            /// <param name="InputData">目标NetworkStream</param>
            /// <param name="_sliceSize">分片大小</param>
            /// <param name="CloseStream">关闭流</param>
            /// <returns></returns>
            public static List<byte[]> ReadStream(Stream InputData, long _sliceSize, bool CloseStream = true)
            {
                var result = new List<byte[]>();
                if (_sliceSize <= 0)
                {
                    _sliceSize = (int)InputData.Length;
                }
                //var i = 0;
                var remainLength = (int)InputData.Length;
                while (remainLength > 0)
                {
                    var buffer = new byte[remainLength];
                    if (remainLength > _sliceSize)
                    {
                        buffer = new byte[_sliceSize];
                    }
                    var readLength = InputData.Read(buffer, 0, buffer.Length);
                    result.Add(buffer);

                    remainLength -= readLength;
                }
                if (CloseStream)//默认读取完关闭流
                    InputData.Close();
                return result;
            }
        }
    }
}
