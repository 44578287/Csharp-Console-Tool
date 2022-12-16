using System.Data;
using System.Data.SqlClient;



SqlConnection conn = new SqlConnection();
String connStr = "Data Source=127.0.0.1; User ID=sa; Password=SUao44578287; Initial Catalog=MyFirstDb";
conn.ConnectionString = connStr;
if (conn.State == ConnectionState.Open)
{
    Console.WriteLine("数据库连接成功！");
}
else
{
    Console.WriteLine(conn.State.ToString());
}
conn.Close();