using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace SQLite
{
    internal class SQLite类
    {
        /// <summary>
        /// 新建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns>新建成功，返回db路径，否则返回null</returns>
        static public string? NewDbFile(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                try
                {
                    SQLiteConnection.CreateFile(dbPath);
                    return dbPath;
                }
                catch
                {
                    return null;
                }
            }
            return dbPath;
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="dbDataTable">表字段</param>
        static public void NewTable(string dbPath, string TableName, List<dbDataTable>? dbDataTable = null)
        {
            string CmdData = "";
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            if (sqliteConn.State != System.Data.ConnectionState.Open)
            {
                sqliteConn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                if (dbDataTable != null)
                {
                    foreach (var InData in dbDataTable)
                        CmdData += string.Join(",", InData.ToStringCmd());
                    cmd.CommandText = "CREATE TABLE " + TableName + "(" + CmdData + ")";
                }
                else
                {
                    cmd.CommandText = "CREATE TABLE " + TableName;
                }
                cmd.ExecuteNonQuery();
            }
            sqliteConn.Close();
        }
        public class dbDataTable 
        {
            public string? TableName { get; set; }
            public string TableType { get; set; } = "text";
            public string ToStringCmd()
            {
                return TableName + " " + TableType;
            }
        }
    }
}
