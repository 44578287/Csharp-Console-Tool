using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql控制
{
    internal class MYSQLTEST
    {
        public static void CreateMysqlDB()
        {
            MySqlConnection conn = new MySqlConnection("server=10.10.10.7;port=3306;user=44578287;password=SUao44578287; database=c;");
            MySqlCommand cmd = new MySqlCommand("CREATE DATABASE yiersan;", conn);

            conn.Open();

            //防止第二次启动时再次新建数据库
            try
            {
                cmd.ExecuteNonQuery();
                conn.Close();
                Console.WriteLine("建立数据库成功");
            }
            catch (Exception)
            {
                conn.Close();
                Console.WriteLine("建立数据库失败，已存在了");
                //throw;
            }
            //防止第二次启动时再次新建数据库
        }

        /// <summary>
        /// 建表
        /// </summary>
        public static void AlterTableExample()
        {
            string connStr = "server=10.10.10.7;port=3306;user=44578287;password=SUao44578287; database=c;";
            string createStatement = "CREATE TABLE People (Name VarChar(50), Age Integer)";
            string alterStatement = "ALTER TABLE People ADD Sex Boolean";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                //防止第二次启动时再次新建数据表
                try
                {
                    // 建表  
                    using (MySqlCommand cmd = new MySqlCommand(createStatement, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 改表或者增加行  
                    using (MySqlCommand cmd = new MySqlCommand(alterStatement, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("建表成功");
                }
                catch (Exception)
                {
                    Console.WriteLine("建表失败，已存在");
                    //throw;
                }
                //防止第二次启动时再次新建数据表

            }

        }
    }
}
