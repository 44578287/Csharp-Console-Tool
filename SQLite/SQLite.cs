// See https://aka.ms/new-console-template for more information
using LoongEgg.LoongLogger;
using SQLite;
using System.Data.SQLite;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数
Console.WriteLine("Hello, World!");
SQLite类.NewDbFile("SQLite.db");//创建SQL文件
SQLite类.NewTable("SQLite.db", "USID", new() { new() { TableName ="ID" , TableDataAUTOINCREMENT = true} });//创建表
//SQLite类.DeleteTable("SQLite.db", "USID");
//SQLite类.ChangeTable("SQLite.db", "USID","A");
SQLite类.CheckStructureTable("SQLite.db", "A");
//SQLite类.NewTable("SQLite.db", "User", new() { new() { TableName = "USID" } });//创建User表
SQLite类.AddColumnTable("SQLite.db", "A", new() { TableName = "IADfwD", TableDataAUTOINCREMENT = false });