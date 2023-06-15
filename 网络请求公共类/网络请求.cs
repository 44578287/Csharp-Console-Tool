using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using LoongEgg.LoongLogger;
using static 网络请求公共类.FileProcess;
using static 网络请求公共类.NetworkRequest;


namespace 网络请求公共类
{
    public class NetworkRequest
    {
        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="data">发送资料</param>
        /// <param name="dataByte">发送资料 Byte</param>
        /// <param name="httpMod">GET POST PUT DELETE 请求模式</param>
        /// <param name="contentType">"默认自动选择 可手动指定 请求头" 请求头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>返回响应内容String</returns>
        public static string? HttpRequestToString(string url, string? cookie = null, string? data = null, byte[]? dataByte = null, HttpMods httpMod = HttpMods.GET, Dictionary<string, string>? Headers = null, string? contentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? encoding = null, bool LogOut = true)
        {
            DateTime beforDT = System.DateTime.Now;
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            HttpWebResponse? response = null;
            HttpMods httpModBak = httpMod;
            if (contentType == null)//自动指定contentType
            {
                switch (httpMod.ToString())
                {
                    case "GET":
                        contentType = "text/html;charset=UTF-8";
                        break;
                    case "POST":
                        contentType = "application/x-www-form-urlencoded";
                        break;
                    case "PATCH":
                        contentType = "application/json";
                        break;
                    case "POST_UPDATA":
                        contentType = "application/octet-stream";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.POST;
                        break;
                    case "PUT":
                        contentType = "application/ json";
                        break;
                    case "PUT_UPDATA":
                        contentType = "application/octet-stream";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.PUT;
                        httpModBak = HttpMods.POST_UPDATA;
                        break;
                    case "DELETE":
                        contentType = null;
                        break;
                }
            }
            if (encoding == null)//未指定编码则自动指定ASCII
            {
                encoding = Encoding.ASCII;
            }
            string method = httpMod.ToString();
            request.Method = method;
            request.ContentType = contentType;
            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            if (cookie != null)
                request.Headers.Add("Cookie", cookie);
            if (Headers != null)
                foreach (var DataIn in Headers)
                {
                    request.Headers.Add(DataIn.Key, DataIn.Value);
                }
            if (data != null && method != "GET" && httpModBak != HttpMods.POST_UPDATA && dataByte == null)//string 类型发送
            {
                request.ContentLength = data.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), encoding);
                writer.Write(data);
                writer.Flush();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data != null && dataByte == null)// 文件路径 发送
            {
                FileStream fs = new FileStream(data, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                Stream writer = request.GetRequestStream();
                writer.Write(bArr, 0, bArr.Length);
                writer.Close();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data == null && dataByte != null)//Byte 发送
            {
                Stream writer = request.GetRequestStream();
                writer.Write(dataByte, 0, dataByte.Length);
                writer.Close();
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException Message)
            {
                Logger.WriteError("(" + url + ")(" + httpModBak.ToString() + ")" + Message.Message);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("使用:" + httpModBak.ToString() + " 访问:" + url + " 时出现错误:" + Message.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            if (LogOut)
                Logger.WriteDebug(retString + $" 耗时:{System.DateTime.Now.Subtract(beforDT).TotalMilliseconds}");
            return retString;
        }
        /// <summary>
        /// 通用请求方法
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="data">发送资料 string 或 文件路径</param>
        /// <param name="dataByte">发送资料 Byte</param>
        /// <param name="httpMod">GET POST PUT DELETE 请求模式</param>
        /// <param name="contentType">"默认自动选择 可手动指定 请求头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>返回响应内容</returns>
        public static HttpWebResponse? HttpRequest(string url, string? cookie = null, string? data = null, byte[]? dataByte = null, HttpMods httpMod = HttpMods.GET, Dictionary<string, string>? Headers = null, string? contentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? encoding = null, bool LogOut = true)
        {
            DateTime beforDT = System.DateTime.Now;
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            HttpWebResponse? response = null;
            HttpMods httpModBak = httpMod;
            if (contentType == null)//自动指定contentType
            {
                switch (httpMod.ToString())
                {
                    case "GET":
                        contentType = "text/html;charset=UTF-8";
                        break;
                    case "POST":
                        contentType = "application/x-www-form-urlencoded";
                        break;
                    case "PATCH":
                        contentType = "application/json";
                        break;
                    case "POST_UPDATA":
                        contentType = "application/octet-stream";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.POST;
                        break;
                    case "PUT":
                        contentType = "application/ json";
                        break;
                    case "PUT_UPDATA":
                        contentType = "application/octet-stream";
                        request.AllowWriteStreamBuffering = false;
                        httpMod = HttpMods.PUT;
                        httpModBak = HttpMods.POST_UPDATA;
                        break;
                    case "DELETE":
                        contentType = null;
                        break;
                }
            }
            if (encoding == null)//未指定编码则自动指定ASCII
            {
                encoding = Encoding.ASCII;
            }
            string method = httpMod.ToString();
            request.Method = method;
            request.ContentType = contentType;
            request.UserAgent = UserAgent;
            request.Timeout = Timeout;
            if (cookie != null)
                request.Headers.Add("Cookie", cookie);
            if (Headers != null)
                foreach (var DataIn in Headers)
                {
                    request.Headers.Add(DataIn.Key, DataIn.Value);
                }
            if (data != null && method != "GET" && httpModBak != HttpMods.POST_UPDATA && dataByte == null)//string 类型发送
            {
                request.ContentLength = data.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), encoding);
                writer.Write(data);
                writer.Flush();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data != null && dataByte == null)// 文件路径 发送
            {
                FileStream fs = new FileStream(data, FileMode.Open, FileAccess.Read);
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                fs.Close();
                Stream writer = request.GetRequestStream();
                writer.Write(bArr, 0, bArr.Length);
                writer.Close();
            }
            if (httpModBak == HttpMods.POST_UPDATA && data == null && dataByte != null)//Byte 发送
            {
                Stream writer = request.GetRequestStream();
                writer.Write(dataByte, 0, dataByte.Length);
                writer.Close();
            }
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException Message)
            {
                Logger.WriteError("(" + url + ")(" + httpModBak.ToString() + ")" + Message.Message);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("使用:" + httpModBak.ToString() + " 访问:" + url + " 时出现错误:" + Message.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return null;
            }
            if (LogOut)
                Logger.WriteDebug(HttpReturnData.HttpWebDataToString(response, false) + $" 耗时:{System.DateTime.Now.Subtract(beforDT).TotalMilliseconds}");
            return response;
        }
        /// <summary>
        /// 通用上传请求方法
        /// </summary>
        /// <param name="UploadPtahs">请求地址</param>
        /// <param name="cookie">cookie</param>
        /// <param name="FilesPath">发送资料文件路径</param>
        /// <param name="dataString">发送资料string</param>
        /// <param name="dataByte">发送资料Byte</param>
        /// <param name="SliceSize">文件分片大小</param>
        /// <param name="httpMod">GET POST PUT DELETE 请求模式</param>
        /// <param name="contentType">"默认自动选择 可手动指定 请求头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>返回响应内容</returns>
        public static string? UploadFile(List<string> UploadPtahs, string? cookie = null, string? FilesPath = null, string? dataString = null, byte[]? dataByte = null, int SliceSize = 0, HttpMods httpMod = HttpMods.POST_UPDATA, List<Dictionary<string, string>>? HeadersList = null, string? contentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? encoding = null, bool LogOut = true)
        {
            string? HttpRequest = null;
            List<byte[]> SliceCache = new();//文件分片缓存集合
            if (FilesPath != null && dataString == null && dataByte == null)
            {
                HeadersList = new();
                //request.Headers.Add("content-range", $"bytes 0-{fs.Length - 1}/{fs.Length}");
                FileSliceReader FileSliceReader = new(FilesPath, SliceSize);//文件分片宣告
                SliceCache = FileSliceReader.ReadFile();//分片并保存
                long ReadSize = 0;
                for (int i = 0; i < SliceCache.Count; i++)
                {
                    ReadSize += SliceCache[i].Length;
                    HeadersList.Add(new()
                    {
                        {"content-range",$"bytes {ReadSize - SliceCache[i].Length}-{ReadSize - 1}/{FileSliceReader.ReadSize}" }
                    });
                }
            }//仅传入文件地址(文件自动分片上传)
            else if (FilesPath == null && dataString != null && dataByte == null)
            {
                SliceCache.Add(System.Text.Encoding.Default.GetBytes(dataString));
            }//传文本
            else if (FilesPath == null && dataString == null && dataByte != null)
            {
                SliceCache.Add(dataByte);
            }//传Byte
            for (int i = 0; i < SliceCache.Count; i++)
            {
                Console.WriteLine($"分片{i}上传中...");
                int Slice = i;
                if (UploadPtahs.Count == 1)
                {
                    Slice = 0;
                }
                HttpRequest = HttpRequestToString(UploadPtahs[Slice], cookie: cookie, httpMod: httpMod, Headers: HeadersList![i], dataByte: SliceCache[i], contentType: contentType, UserAgent: UserAgent, Timeout: Timeout, encoding: encoding, LogOut: LogOut);
                //Logger.WriteInfor("上传成功!(POST) 任务ID:" + sessionID + " 分片:" + i + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
            }//上传
            return HttpRequest;
        }
        /// <summary>
        /// 通用请求方法V2(异步)
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="Cookie">Cookie</param>
        /// <param name="Data">要发送内容(String,Byte[],Stream,文件(文件名))</param>
        /// <param name="HttpMod">请求模式</param>
        /// <param name="HttpZip">压缩</param>
        /// <param name="HttpCache">是否缓存</param>
        /// <param name="Headers">自定请求头</param>
        /// <param name="contentType">"请求标头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>String 返回响应体内容</returns>
        public static async Task<string?> HttpRequestToString_V2Async(string Url, string? Cookie = null, object? Data = null, HttpMods HttpMod = HttpMods.GET, HttpZip HttpZip = HttpZip.OFF, HttpCache HttpCache = HttpCache.No_choice, Dictionary<string, string>? Headers = null, string? contentType = null, int Timeout = 15000, string? UserAgent = null, Encoding? encoding = null, bool LogOut = true)
        {
            DateTime beforDT = System.DateTime.Now;
            HttpClient HttpClient = new();
            HttpClient.Timeout = TimeSpan.FromMinutes(Timeout);//设置超时时间
            if (Cookie != null)//设置Cookie
                HttpClient.DefaultRequestHeaders.Add("Cookie", Cookie);
            if (contentType != null)//设置标头
                HttpClient.DefaultRequestHeaders.Accept.Add(new(contentType));
            if (HttpZip != HttpZip.OFF)//设置压缩类型
                HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", GetEnumDescription(HttpZip));
            if (HttpCache != HttpCache.No_choice)
                HttpClient.DefaultRequestHeaders.Add("Cache-Control", GetEnumDescription(HttpCache));
            if (encoding == null)//未设置编码时自动设置为ASCII
                encoding = Encoding.UTF8;
            if (UserAgent != null)//设置用户UA,诺是没有指定则自动指定
                HttpClient.DefaultRequestHeaders.Add("UserAgent", UserAgent);
            else
                HttpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.70");
            if (Headers != null)//设置额外自定Headers
                foreach (var DataIn in Headers)//遍历添加Header
                {
                    HttpClient.DefaultRequestHeaders.Add(DataIn.Key, DataIn.Value);
                }//应该能覆盖上面内容??


            HttpContent? httpConten = null;//待发送数据准备
            if (Data != null)//判断传入数据类型
                switch (Data?.GetType().Name)//按类型处里Data
                {
                    case "Char":
                    case "String"://文本信息
                        if (File.Exists((string?)Data))//如果文件存在则将文件转换成文件流并直接发送
                        {
                            FileStream fs = new((string)Data, FileMode.Open, FileAccess.Read);//打开文件并附予读取权限
                            HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置
                            httpConten = new StreamContent(fs);//写入文件流至待发送数据
                            fs.Close();//关闭文件流
                            break;
                        }
                        httpConten = new StringContent((string)Data, encoding, contentType);//写入文本至待发送数据
                        break;
                    case "Int32":
                    case "Int64":
                    case "Double"://数字信息
                        httpConten = new StringContent((string)Data, encoding, contentType);//写入文本至待发送数据
                        break;
                    case "Byte[]"://字节流
                        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置(二进制流)
                        httpConten = new ByteArrayContent((Byte[])Data);//写入字节流至待发送数据
                        break;
                    case "FileStream"://文件流
                    case "MemoryStream"://内存流
                    case "NetworkStream"://网络流
                        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置(二进制流)
                        httpConten = new StreamContent((Stream)Data);//写入文件流至待发送数据
                        break;
                    default:
                        throw new Exception("不支持的Data类型");
                }


            HttpResponseMessage response = new();//返回信息
            try
            {
                switch (HttpMod)//选择模式
                {
                    case HttpMods.GET:
                        response = await HttpClient.GetAsync(Url);
                        break;
                    case HttpMods.POST:
                        response = await HttpClient.PostAsync(Url, httpConten);
                        break;
                    case HttpMods.PUT:
                        response = await HttpClient.PutAsync(Url, httpConten);
                        break;
                    case HttpMods.DELETE:
                        response = await HttpClient.DeleteAsync(Url);
                        break;
                }
            }
            catch(Exception ex) 
            {
                Logger.WriteError($"Http请求错误!({GetEnumDescription(HttpMod)}) 请求地址:{Url} 因为:{ex.Message})", false);
                return null;
            }
            string ReturnData =  await response.Content.ReadAsStringAsync();//返回信息
            if (LogOut)
                Logger.WriteDebug(ReturnData + $" 耗时:{System.DateTime.Now.Subtract(beforDT).TotalMilliseconds}");
            //response.EnsureSuccessStatusCode();//获取错误
            return ReturnData;//返回信息
        }
        /// <summary>
        /// 通用请求方法V2(异步)
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="Cookie">Cookie</param>
        /// <param name="Data">要发送内容(String,Byte[],Stream,文件(文件名))</param>
        /// <param name="HttpMod">请求模式</param>
        /// <param name="HttpZip">压缩</param>
        /// <param name="HttpCache">是否缓存</param>
        /// <param name="Headers">自定请求头</param>
        /// <param name="contentType">"请求标头</param>
        /// <param name="UserAgent">设置用户UA</param>
        /// <param name="Timeout">设置超时时间</param>
        /// <param name="encoding">编码</param>
        /// <param name="LogOut">设置是否打印日志</param>
        /// <returns>HttpResponseMessage 返回响应所有内容</returns>
        public static async Task<HttpResponseMessage?> HttpRequest_V2Async(string Url, string? Cookie = null, object? Data = null, HttpMods HttpMod = HttpMods.GET, HttpZip HttpZip = HttpZip.OFF, HttpCache HttpCache = HttpCache.No_choice, Dictionary<string, string>? Headers = null, string? contentType = null, int Timeout = 15000, string? UserAgent = null, Encoding? encoding = null, bool LogOut = true)
        {
            DateTime beforDT = System.DateTime.Now;
            HttpClient HttpClient = new();
            HttpClient.Timeout = TimeSpan.FromMinutes(Timeout);//设置超时时间
            if (Cookie != null)//设置Cookie
                HttpClient.DefaultRequestHeaders.Add("Cookie", Cookie);
            if (contentType != null)//设置标头
                HttpClient.DefaultRequestHeaders.Accept.Add(new(contentType));
            if (HttpZip != HttpZip.OFF)//设置压缩类型
                HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", GetEnumDescription(HttpZip));
            if (HttpCache != HttpCache.No_choice)
                HttpClient.DefaultRequestHeaders.Add("Cache-Control", GetEnumDescription(HttpCache));
            if (encoding == null)//未设置编码时自动设置为ASCII
                encoding = Encoding.UTF8;
            if (UserAgent != null)//设置用户UA,诺是没有指定则自动指定
                HttpClient.DefaultRequestHeaders.Add("UserAgent", UserAgent);
            else
                HttpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.70");
            if (Headers != null)//设置额外自定Headers
                foreach (var DataIn in Headers)//遍历添加Header
                {
                    HttpClient.DefaultRequestHeaders.Add(DataIn.Key, DataIn.Value);
                }//应该能覆盖上面内容??

            HttpContent? httpConten = null;//待发送数据准备
            if (Data != null)//判断传入数据类型
                switch (Data?.GetType().Name)//按类型处里Data
                {
                    case "Char":
                    case "String"://文本信息
                        if (File.Exists((string?)Data))//如果文件存在则将文件转换成文件流并直接发送
                        {
                            FileStream fs = new((string)Data, FileMode.Open, FileAccess.Read);//打开文件并附予读取权限
                            HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置
                            httpConten = new StreamContent(fs);//写入文件流至待发送数据
                            fs.Close();//关闭文件流
                            break;
                        }
                        httpConten = new StringContent((string)Data, encoding, contentType);//写入文本至待发送数据
                        break;
                    case "Int32":
                    case "Int64":
                    case "Double"://数字信息
                        httpConten = new StringContent((string)Data, encoding, contentType);//写入文本至待发送数据
                        break;
                    case "Byte[]"://字节流
                        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置(二进制流)
                        httpConten = new ByteArrayContent((Byte[])Data);//写入字节流至待发送数据
                        break;
                    case "FileStream"://文件流
                    case "MemoryStream"://内存流
                    case "NetworkStream"://网络流
                        HttpClient.DefaultRequestHeaders.Accept.Add(new("application/octet-stream"));//覆盖用户设置(二进制流)
                        httpConten = new StreamContent((Stream)Data);//写入文件流至待发送数据
                        break;
                    default:
                        throw new Exception("不支持的Data类型");
                }

            HttpResponseMessage response = new();//返回信息
            try
            {
                switch (HttpMod)//选择模式
                {
                    case HttpMods.GET:
                        response = await HttpClient.GetAsync(Url);
                        break;
                    case HttpMods.POST:
                        response = await HttpClient.PostAsync(Url, httpConten);
                        break;
                    case HttpMods.PUT:
                        response = await HttpClient.PutAsync(Url, httpConten);
                        break;
                    case HttpMods.DELETE:
                        response = await HttpClient.DeleteAsync(Url);
                        break;
                }
            }
            catch(Exception ex) 
            {
                Logger.WriteError($"Http请求错误!({GetEnumDescription(HttpMod)}) 请求地址:{Url} 因为:{ex.Message})",false);
                return null;
            }
            if (LogOut)
                Logger.WriteDebug(await response.Content.ReadAsStringAsync() + $" 耗时:{System.DateTime.Now.Subtract(beforDT).TotalMilliseconds}");
            //response.EnsureSuccessStatusCode();//获取错误
            return response;//返回信息
        }

        public static async Task<string?> Upload_V2Asyne(List<string> UploadPtahs, object? Data = null, string? Cookie = null, int SliceSize = 0, bool ParallelUploads = false, HttpMods HttpMod = HttpMods.POST_UPDATA, List<Dictionary<string, string>>? HeadersList = null, string? ContentType = null, string? UserAgent = null, int Timeout = 15000, Encoding? Encoding = null, bool LogOut = true)
        {
            List<byte[]> SliceCache = new();//文件分片缓存集合

            if (Data != null)//判断传入数据类型
                switch (Data?.GetType().Name)//按类型处里Data
                {
                    case "Int32":
                    case "Int64":
                    case "Double":
                    case "Char":
                    case "String"://文本信息
                        if (File.Exists((string?)Data))//如果是文件则将文件转换成文件流并直接发送
                        {
                            HeadersList = new();
                            //request.Headers.Add("content-range", $"bytes 0-{fs.Length - 1}/{fs.Length}");
                            FileSliceReader FileSliceReader = new((string)Data, SliceSize);//文件分片宣告
                            SliceCache = FileSliceReader.ReadFile();//分片并保存
                            long ReadSize = 0;
                            for (int i = 0; i < SliceCache.Count; i++)
                            {
                                ReadSize += SliceCache[i].Length;
                                HeadersList.Add(new()//添加文件分片大小信息至头部
                                {
                                    {"content-range",$"bytes {ReadSize - SliceCache[i].Length}-{ReadSize - 1}/{FileSliceReader.ReadSize}" }
                                });
                            }
                            break;
                        }
                        SliceCache.Add(Encoding.UTF8.GetBytes((string)Data));//文本转Byte[]
                        break;
                    case "Byte[]"://字节流
                        SliceCache = FileSliceReader.ReadBytes((byte[])Data, SliceSize);//分片
                        break;
                    case "FileStream"://文件流
                    case "MemoryStream"://内存流
                    case "NetworkStream"://网络流
                    case "Stream":
                        SliceCache = FileSliceReader.ReadStream((Stream)Data, SliceSize);//分片
                        break;
                    default:
                        throw new Exception("不支持的Data类型");
                }

            List<Task<HttpResponseMessage?>> UploadTaskMessages = new();
            List< HttpResponseMessage?> HttpResponseMessagesList= new();

            for (int i = 0; i < SliceCache.Count; i++)
            {
                Console.WriteLine($"分片{i}上传中...");
                int Slice = i;
                if (UploadPtahs.Count == 1)
                {
                    Slice = 0;
                }
                if (!ParallelUploads)//同步上传
                    UploadTaskMessages.Add(HttpRequest_V2Async(UploadPtahs[Slice], Cookie: Cookie, HttpMod: HttpMod, Headers: HeadersList![i], Data: SliceCache[i], contentType: ContentType, UserAgent: UserAgent, Timeout: Timeout, encoding: Encoding, LogOut: LogOut));
                else//等待顺序上传
                {
                    HttpResponseMessagesList.Add(await HttpRequest_V2Async(UploadPtahs[Slice], Cookie: Cookie, HttpMod: HttpMod, Headers: HeadersList![i], Data: SliceCache[i], contentType: ContentType, UserAgent: UserAgent, Timeout: Timeout, encoding: Encoding, LogOut: LogOut));
                }
                //Logger.WriteInfor("上传成功!(POST) 任务ID:" + sessionID + " 分片:" + i + " #文件名:" + Path.GetFileName(FilesPath) + " 本地路径" + Path.GetDirectoryName(FilesPath) + " 云盘路径:" + CloudFilesPath);
            }//上传

            if(!ParallelUploads)
                return UploadTaskMessages.Last().Result?.Content.ToString();
            else
                return HttpResponseMessagesList.Last()?.Content.ToString();
        }






        /// <summary>
        /// 接收web回传内容
        /// </summary>
        public class HttpReturnData
        {
            /// <summary>
            /// web接收回传内容
            /// </summary>
            public HttpWebResponse HttpWebResponse { get; set; }

            /// <summary>
            /// 设置接收web回传内容
            /// </summary>
            public HttpReturnData(HttpWebResponse httpWebResponse)
            {
                HttpWebResponse = httpWebResponse;
            }
            /// <summary>
            /// 以string类型回传接收资料
            /// </summary>
            /// <param name="LogOut">设置是否打印日志</param> 
            public string HttpWebDataToString(bool LogOut = true)
            {
                Stream myResponseStream = HttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传接收资料
            /// </summary>
            /// <param name="HttpWebResponse">Http请求回传值</param> 
            /// <param name="LogOut">设置是否打印日志</param> 
            public static string HttpWebDataToString(HttpWebResponse HttpWebResponse, bool LogOut = true)
            {
                Stream myResponseStream = HttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传Cookie
            /// </summary>
            /// <param name="LogOut">设置是否打印日志</param> 
            public string? HttpWebCookie(bool LogOut = true)
            {
                string? retString = HttpWebResponse.Headers["Set-Cookie"];
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
            /// <summary>
            /// 以string类型回传Cookie
            /// </summary>
            /// <param name="HttpWebResponse">Http请求回传值</param> 
            /// <param name="LogOut">设置是否打印日志</param> 
            public static string? HttpWebCookie(HttpWebResponse HttpWebResponse, bool LogOut = true)
            {
                string? retString = HttpWebResponse.Headers["Set-Cookie"];
                if (LogOut)
                    Logger.WriteDebug(retString);
                return retString;
            }
        }
        /// <summary>
        /// 接收web回传内容_V2Async
        /// </summary>
        public class HttpReturnData_V2Async
        {
            /// <summary>
            /// web回传内容
            /// </summary>
            Task<HttpResponseMessage?>? HttpResponseMessage;
            /// <summary>
            /// 设置接收web回传内容
            /// </summary>
            public HttpReturnData_V2Async(Task<HttpResponseMessage?> _HttpResponseMessage)
            {
                //_HttpResponseMessage.Wait();//等待线程完成
                HttpResponseMessage = _HttpResponseMessage;
            }
            /// <summary>
            /// 以string类型回传接收资料
            /// </summary>
            /// <param name="LogOut">打印日志</param>
            /// <returns>接收内容</returns>
            public string? GetDataToString(bool LogOut = true)
            {
                return HttpResponseMessage?.Result?.Content.ToString();
            }
            /// <summary>
            /// 获取Cookie
            /// </summary>
            /// <param name="LogOut">打印日志</param>
            /// <returns>string类型Cookie</returns>
            public List<string>? GetCookieToString(bool LogOut = true)
            {
                List<string> cookies = new List<string>();
                IEnumerable<string>? values;
                if (HttpResponseMessage?.Result != null)
                {
                    HttpResponseMessage.Result.Headers.TryGetValues("Set-Cookie", out values);
                    if (values != null)
                        foreach (var DataIn in values)
                        {
                            cookies.Add(DataIn);
                        }
                }
                return cookies;
            }
            /*public async Task<string?> GetFile(string FilePath, bool LogOut = false)
            {
                if (HttpResponseMessage != null)
                    try
                    {
                        if(HttpResponseMessage.Result != null)
                        using (Stream stream = await HttpResponseMessage.Result.Content.ReadAsStreamAsync())
                        {
                            //string extension = Path.GetExtension(HttpResponseMessage.RequestMessage.RequestUri.ToString());
                            using (FileStream fs = new(FilePath, FileMode.CreateNew))
                            {
                                long Dada = 0;
                                HttpResponseMessage.Result.Content.Headers.ContentLength = Dada;
                                byte[] buffer = new byte[Dada];
                                int readLength = 0;
                                int length;
                                while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    readLength += length;
                                    // 写入到文件
                                    fs.Write(buffer, 0, length);
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                return FilePath;
            }*/
        }

        /// <summary>
        /// Http请求实例的类型 
        /// </summary>
        /// <remarks>
        /// 用来设置指定的请求类型
        /// </remarks>
        public enum HttpMods
        {
            /// <summary>
            /// GET请求
            /// </summary>
            [Description("GET")]
            GET,
            /// <summary>
            /// POST请求
            /// </summary>
            [Description("POST")]
            POST,
            /// <summary>
            /// POST请求
            /// </summary>
            [Description("POST_UPDATA")]
            POST_UPDATA,
            /// <summary>
            /// PUT请求
            /// </summary>
            [Description("PUT")]
            PUT,
            /// <summary>
            /// PUT请求
            /// </summary>
            [Description("PUT_UPDATA")]
            PUT_UPDATA,
            /// <summary>
            /// DELTE请求
            /// </summary>
            [Description("DELTE")]
            DELETE,
            /// <summary>
            /// PATCH请求
            /// </summary>
            [Description("PATCH")]
            PATCH
        }
        /// <summary>
        /// 压缩类型 
        /// </summary>
        /// <remarks>
        /// 用来设置指定的压缩类型
        /// </remarks>
        public enum HttpZip
        {
            /// <summary>
            /// gzip
            /// </summary>
            [Description("GET")]
            gzip = 0,
            /// <summary>
            /// deflate
            /// </summary>
            [Description("deflate")]
            deflate = 1,
            /// <summary>
            /// br
            /// </summary>
            [Description("br")]
            br = 2,
            /// <summary>
            /// 全部
            /// </summary>
            [Description("gzip, deflate, br")]
            ALL = 3,
            /// <summary>
            /// 不使用
            /// </summary>
            [Description("")]
            OFF = 4
        }
        /// <summary>
        /// 快取类型 
        /// </summary>
        /// <remarks>
        /// 用来设置指定快取
        /// </remarks>
        public enum HttpCache
        {
            /// <summary>
            /// no-store(永远不使用快取)
            /// </summary>
            [Description("no-store")]
            no_store = 0,
            /// <summary>
            /// no-cache(全部快取)
            /// </summary>
            [Description("no-cache")]
            no_cache = 1,
            /// <summary>
            /// public(固定URL快取)
            /// </summary>
            [Description("public")]
            public_ = 2,
            /// <summary>
            /// 不选择
            /// </summary>
            [Description("")]
            No_choice = 3,
        }
        /// <summary>
        /// 错误响应
        /// </summary>
        public enum ErrorCode
        {

            // 1xx 信息性
            /// <summary>
            /// 继续
            /// </summary>
            Continue = 100,
            /// <summary>
            /// 切换协议 
            /// </summary>
            SwitchingProtocols = 101,

            // 2xx 成功
            /// <summary>
            /// 确定
            /// </summary>
            OK = 200,
            /// <summary>
            /// 已创建
            /// </summary>
            Created = 201,
            /// <summary>
            /// 已接受
            /// </summary>
            Accepted = 202,
            /// <summary>
            ///  非权威性信息
            /// </summary>
            NonAuthoritativeInformation = 203,
            /// <summary>
            /// 无内容
            /// </summary>
            NoContent = 204,
            /// <summary>
            /// 重置内容
            /// </summary>
            ResetContent = 205,
            /// <summary>
            /// 部分内容
            /// </summary>
            PartialContent = 206,
            /// <summary>
            /// 多状态
            /// </summary>
            MultiStatus = 207,
            /// <summary>
            /// 已报告
            /// </summary>
            AlreadyReported = 208,
            /// <summary>
            /// IM使用
            /// </summary>
            IMUsed = 226,

            // 3xx 重定向
            /// <summary>
            /// 多重选择
            /// </summary>
            MultipleChoices = 300,
            /// <summary>
            /// 永久移动
            /// </summary>
            MovedPermanently = 301,
            /// <summary>
            /// 发现
            /// </summary>
            Found = 302,
            /// <summary>
            /// 参见其他 
            /// </summary>
            SeeOther = 303,
            /// <summary>
            /// 未修改
            /// </summary>
            NotModified = 304,
            /// <summary>
            /// 使用代理
            /// </summary>
            UseProxy = 305,
            /// <summary>
            /// 切换代理
            /// </summary>
            SwitchProxy = 306,
            /// <summary>
            /// 临时重定向
            /// </summary>
            TemporaryRedirect = 307,
            /// <summary>
            /// 永久重定向
            /// </summary>
            PermanentRedirect = 308,

            // 4xx 客户端错误
            /// <summary>
            /// 错误请求
            /// </summary>
            BadRequest = 400,
            /// <summary>
            /// 未授权
            /// </summary>
            Unauthorized = 401,
            /// <summary>
            /// 需要付款 
            /// </summary>
            PaymentRequired = 402,
            /// <summary>
            /// 禁止
            /// </summary>
            Forbidden = 403,
            /// <summary>
            /// 未找到
            /// </summary>
            NotFound = 404,
            /// <summary>
            /// 不允许的方法
            /// </summary>
            MethodNotAllowed = 405,
            /// <summary>
            /// 不可接受
            /// </summary>
            NotAcceptable = 406,
            /// <summary>
            /// 需要代理身份验证
            /// </summary>
            ProxyAuthenticationRequired = 407,
            /// <summary>
            /// 请求超时
            /// </summary>
            RequestTimeout = 408,
            /// <summary>
            /// 冲突
            /// </summary>
            Conflict = 409,
            /// <summary>
            /// 已经不存在
            /// </summary>
            Gone = 410,
            /// <summary>
            /// 需要长度
            /// </summary>
            LengthRequired = 411,
            /// <summary>
            /// 先决条件失败
            /// </summary>
            PreconditionFailed = 412,
            /// <summary>
            /// 请求实体太大
            /// </summary>
            RequestEntityTooLarge = 413,
            /// <summary>
            /// 请求URI太长
            /// </summary>
            RequestURITooLong = 414,
            /// <summary>
            /// 不支持的媒体类型
            /// </summary>
            UnsupportedMediaType = 415,
            /// <summary>
            /// 请求的范围不可满足
            /// </summary>
            RequestedRangeNotSatisfiable = 416,
            /// <summary>
            /// 期望失败
            /// </summary>
            ExpectationFailed = 417,
            /// <summary>
            /// 我是茶壶
            /// </summary>
            ImATeapot = 418,
            /// <summary>
            /// 认证超时
            /// </summary>
            AuthenticationTimeout = 419,
            /// <summary>
            /// 放松你的英昂
            /// </summary>
            EnhanceYourCalm = 420,
            /// <summary>
            /// 方法失败春季框架
            /// </summary>
            MethodFailureSpringFramework = 421,
            /// <summary>
            /// 无法处理的实体
            /// </summary>
            UnprocessableEntity = 422,
            /// <summary>
            /// 锁定
            /// </summary>
            Locked = 423,
            /// <summary>
            /// 失败的依赖
            /// </summary>
            FailedDependency = 424,
            /// <summary>
            /// 需要升级
            /// </summary>
            UpgradeRequired = 426,
            /// <summary>
            /// 需要先决条件
            /// </summary>
            PreconditionRequired = 428,
            /// <summary>
            /// 太多请求
            /// </summary>
            TooManyRequests = 429,
            /// <summary>
            /// 请求头字段太大
            /// </summary>
            RequestHeaderFieldsTooLarge = 431,
            /// <summary>
            /// 无响应nginx
            /// </summary>
            NoResponseNginx = 444,
            /// <summary>
            /// 重试与微软
            /// </summary>
            RetryWithMicrosoft = 449,
            /// <summary>
            /// 被Windows家长控制阻止
            /// </summary>
            BlockedbyWindowsParentalControls = 450,
            /// <summary>
            /// 出于法律原因不可用
            /// </summary>
            UnavailableForLegalReasons = 451,
            /// <summary>
            /// 客户端关闭请求
            /// </summary>
            ClientClosedRequest = 499,

            // 5xx 服务器错误
            /// <summary>
            /// 内部服务器错误
            /// </summary>
            InternalServerError = 500,
            /// <summary>
            /// 未实施
            /// </summary>
            NotImplemented = 501,
            /// <summary>
            /// 错误网关
            /// </summary>
            BadGateway = 502,
            /// <summary>
            /// 服务不可用
            /// </summary>
            ServiceUnavailable = 503,
            /// <summary>
            /// 网关超时
            /// </summary>
            GatewayTimeout = 504,
            /// <summary>
            /// 不支持的HTTP版本
            /// </summary>
            HTTPVersionNotSupported = 505,
            /// <summary>
            /// 变体也协商
            /// </summary>
            VariantAlsoNegotiates = 506,
            /// <summary>
            /// 存储空间不足
            /// </summary>
            InsufficientStorage = 507,
            /// <summary>
            /// 检测到循环
            /// </summary>
            LoopDetected = 508,
            /// <summary>
            /// 超出带宽限制
            /// </summary>
            BandwidthLimitExceeded = 509,
            /// <summary>
            /// 未扩展
            /// </summary>
            NotExtended = 510,
            /// <summary>
            ///  需要网络认证
            /// </summary>
            NetworkAuthenticationRequired = 511,
            /// <summary>
            /// 网络读取超时错误
            /// </summary>
            NetworkReadTimeoutError = 598,
            /// <summary>
            /// 网络连接超时错误
            /// </summary>
            NetworkConnectTimeoutError = 599
        }

        /// <summary>
        /// 获取enum描述名
        /// </summary>
        public static string? GetEnumDescription(object enumValue)
        {
            string? value = enumValue.ToString();
            FieldInfo? field = enumValue.GetType().GetField(value!);
            object[] objs = field!.GetCustomAttributes(typeof(DescriptionAttribute), false);  //获取描述属性
            if (objs == null || objs.Length == 0)  //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }
    }
}
