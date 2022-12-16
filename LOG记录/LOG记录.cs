using LOG记录;

Console.WriteLine("LOG记录测试");

using (var cc = new LOG.ConsoleCopy(@"LOG/mylogfile.txt"))
{
    Console.WriteLine("LOG记录内容");
    int i = 0;
    while (true)
    {
        Console.WriteLine(i++);
        Thread.Sleep(1000);
    }
}