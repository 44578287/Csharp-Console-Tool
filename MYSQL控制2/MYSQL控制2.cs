using MySql.Data.MySqlClient;
using MYSQL控制2.Json;
using System.Text.Json;
using static MYSQL控制2.MYSQL控制类2;


MYSQL服务器 MYSQL服务器 = new MYSQL服务器 
{ 
    server="10.10.10.7", 
    port="3306",
    user="MC",
    password="MC",
    database="mc_auto_servers"
};

if (!连接数据库(MYSQL服务器))//测试是否能连接上MySql
{
    Console.WriteLine("错误无法连接!");
    Console.WriteLine("按任意键退出....");
    Console.ReadKey(true);
    System.Environment.Exit(0);
}

/*Console.WriteLine("开始列出服务器!");

MySqlConnection conn = new MySqlConnection(MYSQL服务器.connetStr());
try
{
    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
    //Console.WriteLine("已经建立连接");
    //在这里使用代码对数据库进行增删查改
    string sql = "select * from 服务器大纲";
    MySqlCommand cmd = new MySqlCommand(sql, conn);
    MySqlDataReader reader = cmd.ExecuteReader();//执行ExecuteReader()返回一个MySqlDataReader对象
    int i = 0;
    Json服务器大纲.Root[] 服务器大纲列表;
    while (reader.Read())//初始索引是-1，执行读取下一行数据，返回值是bool
    {
        Console.WriteLine(reader["ID"] + " " + reader["服务器版本"] + " " + reader["服务器类型"] + " " + reader["服务器状态"] + " " + reader["服务器地址"]);//"userid"是数据库对应的列名，推荐这种方式
        var 服务器大纲 = new Json服务器大纲.Root
        {
            ID = (int)reader["ID"],
            服务器版本 = (string)reader["服务器版本"],
            服务器类型 = (string)reader["服务器类型"],
            服务器状态 = (string)reader["服务器状态"],
            服务器地址 = (string)reader["服务器地址"]
        };
        //Console.WriteLine(JsonSerializer.SerializeToUtf8Bytes(服务器大纲));
        /*{"ID":"1","服务器版本":"1.12.2","服务器类型":"模组","服务器状态":"启用","服务器地址":["mc.445720.xyz","10.10.10.8"]} 转换Json格式
        i++;
    }
}
catch (MySqlException ex)
{
    Console.WriteLine("错误:" + ex.Message);//报错
}
finally
{
    conn.Close();//关闭连接
}*/

Console.WriteLine("开始列出模组列表!");

MySqlConnection conn = new MySqlConnection(MYSQL服务器.connetStr());
try
{
    conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
    Console.WriteLine("已经建立连接");
    //在这里使用代码对数据库进行增删查改
    string sql = "select * from 模组列表";
    MySqlCommand cmd = new MySqlCommand(sql, conn);
    MySqlDataReader reader = cmd.ExecuteReader();//执行ExecuteReader()返回一个MySqlDataReader对象
    while (reader.Read())//初始索引是-1，执行读取下一行数据，返回值是bool
    {
        Console.WriteLine(reader["ID"] + " " + reader["适用服务器版本"] + " " + reader["模组包名"] + " " + reader["模组内容"] + " " + reader["打包日期"] + " " + reader["下载地址"]);//"userid"是数据库对应的列名，推荐这种方式
    }
}
catch (MySqlException ex)
{
    Console.WriteLine("错误:" + ex.Message);//报错
}
finally
{
    conn.Close();//关闭连接
}