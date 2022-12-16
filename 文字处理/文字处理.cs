


string text = System.IO.File.ReadAllText(@"C:\Users\g9964\Desktop\vs2022\测试1_控制台\文字处理\bin\Debug\net6.0\模组信息.txt");

//System.Console.WriteLine( text);
text.IndexOf("字");
//System.Console.WriteLine(text.IndexOf("版本"));

/*string[] lines = System.IO.File.ReadAllLines(@"C:\Users\g9964\Desktop\vs2022\测试1_控制台\文字处理\bin\Debug\net6.0\1.txt");
foreach (string line in lines)
{
    // Use a tab to indent each line of the file.
    Console.WriteLine("\t" + line);
}*/


string[] sArray = text.Split('"');//以字元!作為分隔符號
/*foreach (var item in sArray)
    Console.WriteLine(item);
Console.ReadKey();*/
/*Console.WriteLine(sArray[0]);
Console.WriteLine(sArray[1]);

Console.WriteLine(sArray[2]);
Console.WriteLine(sArray[3]);*/

Console.WriteLine(Array.IndexOf(sArray, "="));
//string[] sArray = CMD输出.Split(new string[] { "time=", "ms" }, StringSplitOptions.RemoveEmptyEntries);