using LoongEgg.LoongLogger;
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
            Logger.WriteDebug("执行创建数据库 位置:" + Path.GetFullPath(dbPath));
            if (!File.Exists(dbPath))
            {
                Logger.WriteInfor("未在:" + Path.GetFullPath(dbPath) + " 找到数据库即将执行创建");
                try
                {
                    SQLiteConnection.CreateFile(dbPath);
                    Logger.WriteInfor("创建成功!位置:" + Path.GetFullPath(dbPath));
                    return dbPath;
                }
                catch (Exception Message)
                {
                    Logger.WriteError("错误!" + Message);
                    return null;
                }
            }
            Logger.WriteInfor("数据库存在!位置:" + Path.GetFullPath(dbPath));
            return dbPath;
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="TableStructure">表内容</param>
        /// <returns>返回是否成功</returns>
        public static bool NewTable(string dbPath, string TableName, List<TableStructure> TableStructure)
        {
            Logger.WriteDebug("执行创建表 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    string CmdData = "";
                    foreach (var InData in TableStructure)
                        CmdData += string.Join(",", InData.ToStringCmd());
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TableName + "(" + CmdData + ")";
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("创建表成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
                }
            }
            catch (Exception Message)
            {
                Logger.WriteError("错误!" + Message);
                return false;
            }
            finally
            {
                sqliteConn.Close();
            }
            return true;
        }
        /// <summary>
        /// 表内容
        /// </summary>
        public class TableStructure
        {
            /// <summary>
            /// 表对象ID
            /// </summary>
            public int? TableCid { get; set; }
            /// <summary>
            /// 表对象名
            /// </summary>
            public string? TableName { get; set; }
            /// <summary>
            /// 表对象类型
            /// </summary>
            public string? TableType { get; set; }
            /// <summary>
            /// 表对象是否可为NULL
            /// </summary>
            public bool TableDataNull { get; set; } = true;
            /// <summary>
            /// 表对象默认值
            /// </summary>
            public string? TableDefaultValue { get; set; }
            // <summary>
            /// 表对象主键
            /// </summary>
            public bool TableKey { get; set; }
            /// <summary>
            /// 表对象是自动增值
            /// </summary>
            public bool TableDataAUTOINCREMENT { get; set; } = false;
            /// <summary>
            /// 合成表对象语句
            /// </summary>
            public string ToStringCmd()
            {
                if (TableDataNull && !TableDataAUTOINCREMENT)
                    return TableName + " " + TableType + " NOT NULL";
                if (TableDataAUTOINCREMENT)
                    return TableName + " " + "INTEGER" + " PRIMARY KEY AUTOINCREMENT";
                return TableName + " " + TableType;
            }
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <returns>返回是否成功</returns>
        public static bool DeleteTable(string dbPath, string TableName)
        {
            Logger.WriteDebug("执行删除表 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = "DROP TABLE IF EXISTS " + TableName;
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("删除表成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
                }
            }
            catch (Exception Message)
            {
                Logger.WriteError("错误!" + Message);
                return false;
            }
            finally
            {
                sqliteConn.Close();
            }
            return true;
        }
        /// <summary>
        /// 更改表名
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">原表名称</param>
        /// <param name="NewTableName">新表名称</param>
        /// <returns>返回是否成功</returns>
        public static bool ChangeTable(string dbPath, string TableName, string NewTableName)
        {
            Logger.WriteDebug("执行更改表名 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " -> " + NewTableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = "ALTER TABLE " + TableName + " RENAME TO " + NewTableName;
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("更改表名成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " -> " + NewTableName);
                }
            }
            catch (Exception Message)
            {
                Logger.WriteError("错误!" + Message);
                return false;
            }
            finally
            {
                sqliteConn.Close();
            }
            return true;
        }
        /// <summary>
        /// 查询表结构
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <returns>返回表结构</returns>
        public static List<TableStructure>? CheckStructureTable(string dbPath, string TableName)
        {
            List<TableStructure>? ReturnData = new();
            Logger.WriteDebug("执行查询表结构 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = sqliteConn.CreateCommand();
                    cmd.CommandText = $"PRAGMA table_info('{TableName}')";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        List<object> StructureData = new();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            StructureData.Add(reader[i]);
                        }
                        ReturnData.Add(
                            new()
                            {
                                TableCid = Convert.ToInt32(StructureData[0]),
                                TableName = StructureData[1].ToString(),
                                TableType = StructureData[2].ToString(),
                                TableDataNull = Convert.ToBoolean(StructureData[3]),
                                TableDefaultValue = StructureData[4].ToString(),
                                TableKey = Convert.ToBoolean(StructureData[5])
                            });
                    }
                    
                    Logger.WriteInfor("查询表结构成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 结构:");
                    foreach (var DataIn in ReturnData)
                        Logger.WriteInfor($"序号-{DataIn.TableCid} 名字-{DataIn.TableName} 数据类型-{DataIn.TableType} 能否null值-{DataIn.TableDataNull} 预设值-{DataIn.TableDefaultValue} 主键-{DataIn.TableKey}");
                }
            }
            catch (Exception Message)
            {
                Logger.WriteError("错误!" + Message);
                return null;
            }
            finally
            {
                sqliteConn.Close();
            }
            return ReturnData;
        }
        /// <summary>
        /// 对表增添列
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="TableStructure">表内容</param>
        /// <returns>返回是否成功</returns>
        public static bool AddColumnTable(string dbPath, string TableName, TableStructure TableStructure)
        {
            Logger.WriteDebug("执行增添列 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 新列名:" + TableStructure.TableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    string CmdData = TableStructure.ToStringCmd();
                    cmd.CommandText = "ALTER TABLE " + TableName + " ADD COLUMN " + CmdData;
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("增添列成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 新列名:" + TableStructure.TableName);
                }
            }
            catch (Exception Message)
            {
                Logger.WriteError("错误!" + Message);
                return false;
            }
            finally
            {
                sqliteConn.Close();
            }
            return true;
        }














    }
}
