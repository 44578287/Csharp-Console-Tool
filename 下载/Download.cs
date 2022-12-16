using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 下载
{
    internal class Download
    {
        public class MultiDownload
        {
            #region 变量
            // 线程数量
            private int _threadNum;
            //文件大小
            private long _fileSize;
            //文件地址
            private string _fileUrl;
            //保存路径
            private string _savePath;
            //线程完成数量
            private short _threadCompleteNum;
            //是否完成
            private bool _isComplete;
            //当前总文件下载大小(实时的)
            private volatile int _downloadSize;
            //线程数组
            private Thread[] _threads;
            //未下载完全分片，最多尝试次数
            private int _maxTryTimes;
            // 临时文件列表
            private List<string> _tempFiles = new List<string>();
            // 线程锁
            private object locker = new object();
            #endregion

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="threahNum">线程数量</param>
            /// <param name="fileUrl">文件Url路径</param>
            /// <param name="savePath">本地保存路径</param>
            public MultiDownload(int threahNum, string fileUrl, string savePath, int maxTryTimes = 20)
            {
                this._threadNum = threahNum;
                this._threads = new Thread[threahNum];
                this._fileUrl = fileUrl;
                this._savePath = savePath;
                this._maxTryTimes = maxTryTimes;
            }
            /// <summary>
            /// 开始下载文件
            /// </summary>
            /// <param name="wait">是否为同步，true为同步会等待下载完成，false为异步继续执行后面的代码</param>
            public void Start(bool wait = false)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_fileUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // 获取文件总大小
                _fileSize = response.ContentLength;
                // 平均分配
                long singelNum = (_fileSize / _threadNum);
                // 获取剩余的
                long remainder = (_fileSize % _threadNum);
                request.Abort();
                response.Close();
                for (int i = 0; i < _threadNum; i++)
                {
                    List<long> range = new List<long>
                {
                    i * singelNum
                };
                    if (remainder != 0 && (_threadNum - 1) == i)
                    {
                        //剩余的交给最后一个线程
                        range.Add(i * singelNum + singelNum + remainder - 1);
                    }
                    else
                    {
                        range.Add(i * singelNum + singelNum - 1);
                    }
                    // 下载文件指定位置的数据
                    long[] ran = new long[] { range[0], range[1] };
                    _threads[i] = new Thread(new ParameterizedThreadStart(Download))
                    {
                        Name = Path.GetFileNameWithoutExtension(_fileUrl) + "_{0}".Replace("{0}", Convert.ToString(i + 1))
                    };
                    _threads[i].Start(ran);
                }

                if (wait)
                {
                    foreach (Thread item in _threads)
                    {
                        //数组中的所有子线程都对主线程进行阻塞，只是阻塞了启动
                        item.Join();
                    }
                }
            }
            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="obj">参数为分片起始位置，range[0], range[1] </param>
            private void Download(object obj)
            {
                long[] rans = obj as long[];
                string tmpFileBlock = Path.GetTempPath() + Thread.CurrentThread.Name + ".tmp";
                _tempFiles.Add(tmpFileBlock);
                // 当前需要下载分片大小
                long shardFileSize = rans[1] - rans[0] + 1;
                // 尝试重新下载次数
                int tryTimes = 0;
#if DEBUG
                Console.WriteLine($"开始下载分片文件：{tmpFileBlock}，长度： {shardFileSize}");
#endif
                Download(tmpFileBlock, true, rans[0], rans[1], out long completedSize);
                while (completedSize < shardFileSize)
                {
                    // 未下载完成 继续下载
#if DEBUG
                    Console.WriteLine($"未下载完的文件：{tmpFileBlock}，未完成部分大小： {shardFileSize - completedSize}");
#endif
                    Download(tmpFileBlock, false, (rans[0] + completedSize), rans[1], out completedSize);
                    tryTimes++;
                    if (tryTimes > _maxTryTimes)
                    {
                        break;
                    }
                }

                if (completedSize == shardFileSize)
                {
                    lock (locker) _threadCompleteNum++;
                }

                if (_threadCompleteNum == _threadNum)
                {
                    // 所有分片都已下载完成
                    Complete();
                    _isComplete = true;
                }
            }
            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="tmpFileBlock">临时文件存放路径</param>
            /// <param name="isFirst">是否第一次下载，若为第一次下载则创建新文件，若不是第一次下载则在原文件上追加</param>
            /// <param name="fromRange">分片开始位置</param>
            /// <param name="toRange">分片结束位置</param>
            /// <param name="completedSize">返回已下载长度，用于判断分片文件是否下载完成</param>
            private void Download(string tmpFileBlock, bool isFirst, long fromRange, long toRange, out long completedSize)
            {
                Stream httpFileStream = null, localFileStram = null; long shardFileSize = 0L;
                try
                {
                    HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(_fileUrl);
                    httprequest.AddRange(fromRange, toRange);
                    HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();
                    httpFileStream = httpresponse.GetResponseStream();
                    localFileStram = new FileStream(tmpFileBlock, isFirst ? FileMode.Create : FileMode.Append);
                    // 获取当前分片大小
                    shardFileSize = httpresponse.ContentLength + localFileStram.Length;
#if DEBUG
                    Console.WriteLine($"开始下载分片文件：{tmpFileBlock}，下载片段位置： {fromRange}-{toRange}，下载长度：{toRange - fromRange + 1}-{shardFileSize}");
#endif
                    byte[] by = new byte[4096];
                    // Read方法将返回读入by变量中的总字节数
                    int getByteSize = httpFileStream.Read(by, 0, (int)by.Length);
                    while (getByteSize > 0)
                    {
                        lock (locker) _downloadSize += getByteSize;
                        localFileStram.Write(by, 0, getByteSize);
                        getByteSize = httpFileStream.Read(by, 0, (int)by.Length);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine("下载异常： " + ex.Message);
                    throw new WebException(ex.Message.ToString());
                }
                finally
                {
#if DEBUG
                    Console.WriteLine($"{(shardFileSize == localFileStram.Length)} 结束下载分片文件：{tmpFileBlock}，下载片段位置： {fromRange}-{toRange}，本次需完成长度：{toRange - fromRange + 1}，未完成长度：{shardFileSize - localFileStram.Length}");
#endif
                    // 计算未下载文件长度
                    completedSize = localFileStram.Length;
                    if (httpFileStream != null) httpFileStream.Dispose();
                    if (localFileStram != null) localFileStram.Dispose();
                }
            }

            /// <summary>
            /// 下载完成后合并文件块
            /// </summary>
            private void Complete()
            {
                Stream mergeFile = new FileStream(@_savePath, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(mergeFile);
                foreach (string file in _tempFiles)
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        BinaryReader tempReader = new BinaryReader(fs);
                        writer.Write(tempReader.ReadBytes((int)fs.Length));
                        tempReader.Close();
                    }
                    File.Delete(file);
                }
                writer.Close();
            }
        }
    }
}
