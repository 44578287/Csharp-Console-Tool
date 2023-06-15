using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using LoongEgg.LoongLogger;
using Yitter.IdGenerator;
using 网络请求公共类;
using static 网络请求公共类.NetworkRequest;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数
DateTime beforDT = System.DateTime.Now;

// 创建 IdGeneratorOptions 对象，可在构造函数中输入 WorkerId：
//var options = new IdGeneratorOptions(1);
// options.WorkerIdBitLength = 10; // 默认值6，限定 WorkerId 最大值为2^6-1，即默认最多支持64个节点。
// options.SeqBitLength = 6; // 默认值6，限制每毫秒生成的ID个数。若生成速度超过5万个/秒，建议加大 SeqBitLength 到 10。
// options.BaseTime = Your_Base_Time; // 如果要兼容老系统的雪花算法，此处应设置为老系统的BaseTime。
// ...... 其它参数参考 IdGeneratorOptions 定义。

// 保存参数（务必调用，否则参数设置不生效）：
//YitIdHelper.SetIdGenerator(options);

// 以上过程只需全局一次，且应在生成ID之前完成。

// 初始化后，在任何需要生成ID的地方，调用以下方法：
//var newId = YitIdHelper.NextId();

//Console.WriteLine(ConvertToBase62(newId));

//string Url1 = "http://localhost:5055/API/Chartbed/GetUrl";
string Url1 = "http://s3.44578287.xyz/";
//string Url1 = "http://localhost:5055/API/Chartbed/Stream";
//NetworkRequest.HttpRequest_V2(Url1);
//FileStream FileStream = new("C:\\Users\\g9964\\Downloads\\WriteText.txt",FileMode.Open); 

//Console.WriteLine(await HttpRequestToString_V2Async(Url1,HttpMod:HttpMods.POST,Data: FileStream));

//Console.WriteLine(Path.GetFileName("C://1.txt") == "");
/*string Data = "";
Task task = HttpRequest_V2Async(Url1,httpMod:HttpMods.POST,Data: "[\"AAA\"]",contentType: "application/problem+json");
Task.Run(() => task);
task.Wait();*/
//List<Task<string?>> RetuinData = new(); /*HttpRequestToString_V2Async(Url1, HttpMod: HttpMods.POST,Data:"[\"AAAA\"]", contentType: "application/problem+json");*/
//string? RetuinData = "";
//ThreadPool.SetMaxThreads(1, 1);
/*for (int i = 0; i < 100; i++)
{
    //RetuinData = await HttpRequestToString_V2Async(Url1, HttpMod: HttpMods.POST, Data: "[\"AAAA\"]", contentType: "application/problem+json");
     RetuinData.Add(HttpRequestToString_V2Async(Url1, HttpMod: HttpMods.POST, Data: "[\"AAAA\"]", contentType: "application/problem+json"));
}



//Task.Run(() => RetuinData);

Task.WaitAll(RetuinData.ToArray());
Console.WriteLine("线程结束!");
//RetuinData.
Console.WriteLine(RetuinData[0].Result);
Console.WriteLine(RetuinData);
*/


/*Random random = new Random();
String code = "";
for (int i = 0; i < 12; i++)
{
    int r = random.Next(2) == 0 ? 48 : 65;
    code += (char)random.Next(r, r + 6);
}
Console.WriteLine("生成的code码是：" + code);*/
//ConvertToBase62(1);
//Console.WriteLine(ConvertToBase62(654846549841298));

//await HttpRequest_V2Async(Url1);

string ApiUrl2 = "https://cloud.445720.xyz/api/v3/user/session";
HttpReturnData_V2Async HttpReturnData_V2Async = new(HttpRequest_V2Async(ApiUrl2, HttpMod:HttpMods.POST,Data: "{\"UserName\":\"test@445720.xyz\",\"Password\":\"test@445720.xyz\",\"CaptchaCode\":\"\"}",contentType: "application/x-www-form-urlencoded"));
Console.WriteLine(HttpReturnData_V2Async.GetCookieToString()[0]);



Console.WriteLine("DateTime总共花费{0}ms.", System.DateTime.Now.Subtract(beforDT).TotalMilliseconds);











// Base62编码
static string ConvertToBase62(long num)
{
    const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    string result = string.Empty;
    int mod = 0;

    while (num > 0)
    {
        mod = (int)(num % 62);
        result = chars[mod] + result;
        num = num / 62;
    }

    return result;
}

static long ConvertFromBase62(string base62String)
{
    const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    long result = 0;
    int len = base62String.Length - 1;
    for (int i = 0; i <= len; i++)
    {
        result += chars.IndexOf(base62String[i]) * (long)Math.Pow(62, len - i);
    }

    return result;
}