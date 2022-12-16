using MySql.Data.MySqlClient;
using MySql控制;
using System.Data;
/*
连接MYSQL资料库
MySqlConnection conn = new MySqlConnection("server=xxxxx;port=xxxx;user=xxxxx;password=xxxxxx; database=xxxxxxx;");//新建一个连接名为conn ()内server填入服务器IP port填入端口号 user填入账号 password填入密码 database填入数据库名称
conn.Open(); //打开通道
conn.Close();//关闭通道





*/



// server=127.0.0.1/localhost 代表本机，端口号port默认是3306可以不写
String connetStr = "server=10.10.10.7;port=3306;user=44578287;password=SUao44578287; database=c;";
    MySqlConnection conn = new MySqlConnection(connetStr);
    try
    {
        conn.Open();//打开通道，建立连接，可能出现异常,使用try catch语句
        Console.WriteLine("已经建立连接"); //在这里使用代码对数据库进行增删查改 业务代码可以在这一行下面编写
                                     // 业务代码写这里！  就是这句话下面的一行   建立好连接就可以把下面的业务代码镐里头了！

    string cmdStr = string.Format("select * from {0};", "user");
    MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
    MySqlDataReader m = cmd.ExecuteReader();//获取读取器返回的是表中所有数据
    while (m.Read())
    {
        /*string value = (string)m["ADM"];
        //输出当前列的值
        Console.WriteLine(value);*/
//Console.WriteLine(m["ID"]);
Console.WriteLine("编号"+ m["ID"]+" 账户名:"+ m["ADM"]+" 密码:"+ m["PW"]+" 名称:"+ m["Name"]);
}




}
catch (MySqlException ex)
{
    Console.WriteLine("错误:"+ex.Message);//有错则报出错误
}
finally
{
    conn.Close();//关闭通道
}



Console.ReadKey();

//MYSQLTEST.AlterTableExample();



