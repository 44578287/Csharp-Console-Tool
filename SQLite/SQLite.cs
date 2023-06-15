// See https://aka.ms/new-console-template for more information
using LoongEgg.LoongLogger;
using SQLite;
using System.Data;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数
Console.WriteLine("Hello, World!");
DateTime beforDT = System.DateTime.Now;



//SQLite类.NewDbFile("SQLite.db");//创建SQL文件
//SQLite类.NewTable("SQLite.db", "USID", new() { new() { TableName = "ID", TableDataAUTOINCREMENT = true } });//创建表
//SQLite类.AddColumnTable("SQLite.db", "USID", new() { TableName = "Data", TableType = "string" });//创建表
//SQLite类.DeleteTable("SQLite.db", "USID");
//SQLite类.ChangeTable("SQLite.db", "USID","A");
/*SQLite类.CheckStructureTable("SQLite.db", "A");
//SQLite类.NewTable("SQLite.db", "User", new() { new() { TableName = "USID" } });//创建User表
//SQLite类.AddColumnTable("SQLite.db", "user", new() { TableName = "IADfwD", TableDataAUTOINCREMENT = false });
SQLite类.SimpleInsert("SQLite.db", "usid", "868" );*/
//SQLite类.Insert("SQLite.db", "usid", new() { new() { ObjectName = "ID", ObjectData = 11 }, new() { ObjectName = "Data", ObjectData = "AA" } });
//SQLite类.Replace("SQLite.db", "usid", new() { new() { ObjectName = "ID", ObjectData = 11 }, new() { ObjectName = "Data", ObjectData = "777" } });
//SQLite类.Delete("SQLite.db", "usid", new() { new() { ObjectName = "ID", ObjectData = 11 }, new() });
//List<int> Data
DataTable ?Data = SQLite类.Select("SQLite.db", "usid", new() { ObjectName = "ID", ObjectData = 1 });
foreach(var InData in Data)
    Console.WriteLine(InData);




Console.WriteLine("DateTime总共花费{0}ms.", System.DateTime.Now.Subtract(beforDT).TotalMilliseconds);