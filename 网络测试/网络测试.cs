using exe类;

string 当前路径 = System.IO.Directory.GetCurrentDirectory();

//exe.Ping("445720.xyz");
/*string CMD输出 = exe.Ping2("445720.xyz 443");
//string[] sArray = CMD输出.Split('=');


//string[] sArray = CMD输出.Split(new char[2] { '=', 'm' });
string[] sArray = CMD输出.Split(new string[] { "=", "ms","(","fail)"}, StringSplitOptions.RemoveEmptyEntries);

foreach (string e in sArray)

{ Console.WriteLine(e); }*/

Console.WriteLine("开始测试直连节点:" + "mc.445720.xyz:25565");
string 直连节点延迟输出 = exe.Ping2("mc.445720.xyz 25565");
string [] sArray = 直连节点延迟输出.Split(new string[] { "=", "ms", "(", "fail)" }, StringSplitOptions.RemoveEmptyEntries);
string 直连节点平均延迟 = "";
string 直连节点丢包率 = "";
float 直连节点平均延迟比 = -1;
if (sArray.Length == 11)
{
    //直连节点平均延迟 = sArray[10];
    直连节点平均延迟 = "无法连接，无法提供行程统计 -1";
    直连节点丢包率 = sArray[9];
}
else
{
    直连节点平均延迟 = sArray[15];
    直连节点丢包率 = sArray[9];
    直连节点平均延迟比 = float.Parse(直连节点平均延迟);
}
Console.WriteLine("直连节点延迟:"+ 直连节点平均延迟+"ms");
Console.WriteLine("直连节点丢包:" + 直连节点丢包率);


Console.WriteLine("开始测试加速节点:" + "42.193.111.84:14457");
string 加速节点延迟输出 = exe.Ping2("42.193.111.84 14457");
          sArray = 加速节点延迟输出.Split(new string[] { "=", "ms", "(", "fail)" }, StringSplitOptions.RemoveEmptyEntries);
string 加速节点平均延迟 = "";
string 加速节点丢包率 = "";
float 加速节点平均延迟比 = -1;
if (sArray.Length == 11)
{
    //直连节点平均延迟 = sArray[10];
    加速节点平均延迟 = "无法连接，无法提供行程统计 -1";
    加速节点丢包率 = sArray[9];
}
else
{
    加速节点平均延迟 = sArray[15];
    加速节点丢包率 = sArray[9];
    加速节点平均延迟比 = float.Parse(加速节点平均延迟);
}
Console.WriteLine("加速节点延迟:" + 加速节点平均延迟 + "ms");
Console.WriteLine("加速节点丢包:" + 加速节点丢包率);

Console.WriteLine("测试完成!");
if (加速节点平均延迟比 > 直连节点平均延迟比)
{
    Console.WriteLine("推荐使用直连节点");
    Console.WriteLine("直连节点延迟:" + 直连节点平均延迟 + "ms");

}
else if (加速节点平均延迟比 < 直连节点平均延迟比)
{

}
else if(加速节点平均延迟比 == 直连节点平均延迟比)
{

}