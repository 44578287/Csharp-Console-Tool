// See https://aka.ms/new-console-template for more information
using SQLite;

Console.WriteLine("Hello, World!");
Console.WriteLine(SQLite类.NewDbFile("SQLite.db"));//创建SQL文件
SQLite类.NewTable("SQLite.db", "User", new() { new() { TableName = "USID" } });//创建User表