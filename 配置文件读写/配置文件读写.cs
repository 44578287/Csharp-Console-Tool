/*
 配置文件读写
IniHelper.SetValue(分区名,值名,值,配置文件位置);    //储存配置文件
IniHelper.GetValue("我的数据", "MyID", "", file);   //读取配置文件
 
 
 
 
 
 
 
 
 
 
 
 
 
 */







using static 配置文件读写.iniHelper;

Console.WriteLine("Hello, World!");

string MyID = Console.ReadLine();//输入ID
//写入文件
string file = System.Environment.CurrentDirectory + "/config.ini";//指定位置
if (file != null)
{
    IniHelper.SetValue("我的数据", "MyID", MyID, file);
    //IniHelper.SetValue("我的数据测试", "嘿嘿嘿", "44578287", file);
}
Console.WriteLine("我输入的ID为{0}", MyID);
//读取文件
if (file != null)
{
    MyID = IniHelper.GetValue("我的数据", "MyID", "", file);
    Console.WriteLine("我读取到的ID为{0}" + MyID);
    Console.ReadLine();
}