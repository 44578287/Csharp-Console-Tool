using System.Threading.Tasks;
using Spectre.Console;




await AnsiConsole.Progress()
    //.AutoRefresh(false) // 关掉自动刷新
    .AutoClear(false)   // 完成后不删除任务列表
    .HideCompleted(false)   // 在任务完成后隐藏它们
    .Columns(new ProgressColumn[]
    {
        new TaskDescriptionColumn(),    // 任务描述
        new ProgressBarColumn(),        // 进度栏
        new PercentageColumn(),         // 百分比
        new RemainingTimeColumn(),      // 余下的时间
        new SpinnerColumn(),            // 旋转器
    })
    .StartAsync(async ctx =>
    {
        var task1 = ctx.AddTask("[green]进度条[/]");
        /*while (!ctx.IsFinished)
        {
            //Thread.Sleep(1000);
            await Task.Delay(250);
            task1.Increment(1.5);
        }*/
    });
List<string> AAA= new List<string>();
AAA.Add("avif");
AAA.Add("avif");
AAA.Add("avif");
AAA.Add("avif");
//int A101StudentCount = AAA.Count(t => t.ClassCode =“A101”&& t.StudentName.StartWith(“刘”))；
//int A101StudentCount = AAA.Count(t => t.StartsWith("avif"));
//Console.WriteLine(A101StudentCount);

List<Task<string>> ListFor = new List<Task<string>>();
ListFor.Count(t => t.IsCompleted);