/*Microsoft.Scripting.Hosting.ScriptEngine engine = IronPython.Hosting.Python.CreateEngine();
dynamic script = engine.ExecuteFile("tts语音合成.py");
//dynamic script = engine.ExecuteFile("Py接C#.py");

script.语音合成调用.SSML路径 = ("C:\\Users\\g9964\\Desktop\\vs2022\\测试1_控制台\\PyC杂交\\bin\\Debug\\net6.0\\SSML.xml");
script.语音合成调用.输出路径 = ("C:\\Users\\g9964\\Desktop\\vs2022\\测试1_控制台\\PyC杂交\\bin\\Debug\\net6.0\\1");
//script.语音合成调用.合成();*/

using PyC杂交;

string 当前路径 = System.Environment.CurrentDirectory;
long 当前Ticks = DateTime.Now.Ticks;


//exe.TransferExe2(当前路径 + "/tts语音合成.exe", "--text C#调用测试", "--name zh-CN-XiaoxiaoNeural", "--style General", "--rate 0", "--pitch 0");

语音文件参数 测试 = new 语音文件参数();
测试.传入(exe.TransferExe2(当前路径 + "/tts语音合成.exe", "--text C#调用测试", "--name zh-CN-XiaoxiaoNeural", "--style General", "--rate 0", "--pitch 0"));

Console.WriteLine(测试.文件全名);
Console.WriteLine(测试.声音);
Console.WriteLine(测试.风格);
Console.WriteLine(测试.语速);
Console.WriteLine(测试.音调);
Console.WriteLine(测试.文本内容);

Console.WriteLine("处理时长:"+(DateTime.Now.Ticks - 当前Ticks) / 10000+"ms");
public struct 语音文件参数
{
    public string 文件全名;
    public string 声音;
    public string 风格;
    public string 语速;
    public string 音调;
    public string 文本内容;
    public void 传入(string 文件名)
    {
        文件全名 = 文件名;
        string[] sArray = 文件全名.Split(new string[] {"_"}, StringSplitOptions.RemoveEmptyEntries);
        声音 = sArray[0];
        风格 = sArray[1];
        语速 = sArray[2];
        音调 = sArray[3];
        文本内容 = sArray[4];
    }
}