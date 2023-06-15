using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 文件分割
{
    internal class Class1
    {
        /// <summary>
        /// 使用文件分片读取文件，并将每一片存入List中
        /// </summary>
        /// <param name="filePath">要读取的文件路径</param>
        /// <param name="bufferSize">缓冲区大小（字节数）</param>
        /// <returns></returns>
        public static List<string> ReadFileBySlice(string filePath, int bufferSize)
        {
            // 异步读取文件
            // 返回一个Task<string>对象
            Task<string> task = AsyncReadFile(filePath, bufferSize);
            // 等待异步读取结束
            Thread.Sleep(1000);

            // 创建List集合
            List<string> list = new List<string>();

            // 获取Task<string>的返回值
            string result = task.Result;
            Console.WriteLine("AsyncReadFile End...");
            Console.WriteLine("Now is Reading By Slice...");
            int count = 0;
            while (true)
            {
                // 根据缓冲区大小截取指定字节数
                var slice = result.Substring(0, bufferSize);
                // 将每一部分存入List集合
                list.Add(slice);
                // 剩余的部分
                var remaining = result.Remove(0, bufferSize);

                if (remaining == string.Empty)
                {
                    Console.WriteLine("read finished...");
                    break;
                }

                result = remaining;
                Console.WriteLine("progress: {0}", ++count);
            }

            return list;
        }

        /// <summary>
        /// 异步读取文件，返回一个Task<string>对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static Task<string> AsyncReadFile(string filePath, int bufferSize)
        {
            return Task.Run<string>(() =>
            {
                return ReadFile(filePath, bufferSize);
            });
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, int bufferSize)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                // 创建缓冲区
                byte[] buffer = new byte[bufferSize];
                // 用于存储每次读取的字节
                List<byte> bytes = new List<byte>();
                while (true)
                {
                    // 将文件中的数据读取到缓冲区
                    var read = fs.Read(buffer, 0, bufferSize);
                    for (int i = 0; i < read; i++)
                    {
                        bytes.Add(buffer[i]);
                    }
                    // 每次读取后判断当前缓冲区是否已经读取完毕
                    if (read < bufferSize)
                    {
                        // 拼接缓冲区里的数据，此处是追加
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in bytes)
                        {
                            sb.Append((char)item);
                        }
                        return sb.ToString();
                    }
                }
            }
        }


        /// <summary>
        /// 文件分片读取类
        /// </summary>
        public class FilePartitionRead
        {
            //文件分片大小，可自定义
            private int chunkSize;
            //文件路径
            private string filePath;

            private volatile int alreadyReadSize;
            //线程数
            int threadCount = 4;

            /// <summary>
            /// 实例化一个FilePartitionRead
            /// </summary>
            /// <param name="filePath">文件路径</param>
            /// <param name="chunkSize">每次分片大小</param>
            public FilePartitionRead(string filePath, int chunkSize, int threadCount = 4)
            {
                this.filePath = filePath;
                this.chunkSize = chunkSize;
                this.alreadyReadSize = 0;
                this.threadCount = threadCount;
            }

            /// <summary>
            /// 执行分片读取操作
            /// </summary>
            /// <returns>分片数据集合</returns>
            public List<byte[]> Execute()
            {
                List<byte[]> byteList = new List<byte[]>();
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    //文件大小
                    long fileLength = fs.Length;
                    //本次分片读取的长度
                    int readSize = 0;

                    //int threadCount = 4;   //线程数
                    Task[] tasks = new Task[threadCount];

                    int taskIndex = 0;
                    while (alreadyReadSize < fileLength)
                    {
                        if (alreadyReadSize + chunkSize > fileLength)
                        {
                            readSize = Convert.ToInt32(fileLength - alreadyReadSize);
                        }
                        else
                        {
                            readSize = chunkSize;
                        }
                        int finalReadSize = readSize;

                        tasks[taskIndex] = new Task(() =>
                        {
                            byte[] buffer = new byte[finalReadSize];
                            fs.Seek(alreadyReadSize, SeekOrigin.Begin);//定位流中当前指针位置
                            fs.Read(buffer, 0, finalReadSize);
                            //Console.WriteLine($"load data from: {alreadyReadSize} to {alreadyReadSize + finalReadSize}");
                            //添加到集合
                            byteList.Add(buffer);
                            Interlocked.Add(ref alreadyReadSize, finalReadSize);
                        });
                        tasks[taskIndex].Start();
                        taskIndex++;
                        //如果任务数超过 threadCount 就开始等待已经创建的任务执行完毕  
                        if (taskIndex == threadCount)
                        {
                            Task.WaitAll(tasks);
                            taskIndex = 0;
                        }
                    }
                    //全部读取完毕，等待最后几个任务执行完毕  
                    if (taskIndex != 0)
                    {
                        Task.WaitAll(tasks);
                    }
                }
                return byteList;
            }

            /// <summary>
            /// 显示进度
            /// </summary>
            private void ShowProcess()
            {

            }
        }

        /// <summary>
        /// 文件分片读取类
        /// </summary>
        public class FileChunkReader
        {
            /// <summary>
            /// 文件流对象
            /// </summary>
            private Stream _inputStream;
            /// <summary>
            /// 分片大小
            /// </summary>
            private int _chunkSize;
            /// <summary>
            /// 当前进度
            /// </summary>
            private double _currentProgress;
            /// <summary>
            /// 当前已读取的字节数
            /// </summary>
            private int _currentIndex;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="inputStream">输入文件流</param>
            /// <param name="chunkSize">分片大小，单位：字节</param>
            public FileChunkReader(Stream inputStream, int chunkSize)
            {
                this._inputStream = inputStream ?? throw new ArgumentNullException("Input stream can not be null");
                this._chunkSize = chunkSize;
            }
            /// <summary>
            /// 异步读取文件 并存储到List中
            /// </summary>
            /// <returns>存储分片信息的List</returns>
            public List<byte[]> ReadFile()
            {
                List<byte[]> lstData = new List<byte[]>();
                int totalReadLength = 0;
                int readLength = 0;
                byte[] sourceData = new byte[_inputStream.Length];
                _inputStream.Read(sourceData, 0, sourceData.Length);
                while (totalReadLength < sourceData.Length)
                {
                    if (totalReadLength + _chunkSize >= sourceData.Length)
                    {
                        readLength = sourceData.Length - totalReadLength;
                    }
                    else
                    {
                        readLength = _chunkSize;
                    }
                    byte[] chunkData = new byte[readLength];
                    Buffer.BlockCopy(sourceData, totalReadLength, chunkData, 0, readLength);
                    lstData.Add(chunkData);
                    totalReadLength += readLength;
                    //更新进度
                    _currentIndex++;
                    _currentProgress = Math.Round(_currentIndex * 1.00 / lstData.Count, 4);
                }
                return lstData;
            }
            /// <summary>
            /// 获取进度
            /// </summary>
            /// <returns>当前进度</returns>
            public double GetProgress()
            {
                return _currentProgress;
            }
        }




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
                }
                return result;
            }
        }
    }
}
