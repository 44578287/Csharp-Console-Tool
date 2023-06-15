using LoongEgg.LoongLogger;
using System;
using System.Data;
using System.Data.SQLite;
using System.Reflection.PortableExecutable;

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
        /// <summary>
        /// 简单插入数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">插入内容(仅1)</param>
        /// <returns>返回是否成功</returns>
        public static bool SimpleInsert(string dbPath, string TableName, object InsertData)
        {
            Logger.WriteDebug("执行插入数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = "INSERT INTO " + TableName + " VALUES(" + InsertData + ")";
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("插入数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName);
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
        /// 插入数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">插入内容(对象名/内容)</param>
        /// <returns>返回是否成功</returns>
        public static bool Insert(string dbPath, string TableName, List<InsertData> InsertData)
        {
            List<string> CmdData = new();
            string CmdDataString = "";
            List<string> CmdData2 = new();
            string CmdDataString2 = "";
            foreach (var InData in InsertData)
            {
                CmdData.Add(InData.ObjectName!);
                CmdData2.Add("@" + InData.ObjectName!);
            }
            CmdDataString = string.Join(",", CmdData.ToArray());
            CmdDataString2 = string.Join(",", CmdData2.ToArray());
            Logger.WriteDebug("执行插入数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 插入对象名:" + CmdDataString);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"INSERT INTO {TableName}({CmdDataString}) VALUES({CmdDataString2})";
                    for (int i = 0; i < InsertData.Count; i++)
                    {
                        cmd.Parameters.Add(InsertData[i].ObjectName, DbAutoType(InsertData[i].ObjectData)).Value = InsertData[i].ObjectData;
                    }
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("插入数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 插入对象名:" + CmdDataString);
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
        /// 替换数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">替换内容(对象名/内容)</param>
        /// <returns>返回是否成功</returns>
        public static bool Replace(string dbPath, string TableName, List<InsertData> InsertData)
        {
            List<string> CmdData = new();
            string CmdDataString = "";
            List<string> CmdData2 = new();
            string CmdDataString2 = "";
            foreach (var InData in InsertData)
            {
                CmdData.Add(InData.ObjectName!);
                CmdData2.Add("@" + InData.ObjectName!);
            }
            CmdDataString = string.Join(",", CmdData.ToArray());
            CmdDataString2 = string.Join(",", CmdData2.ToArray());
            Logger.WriteDebug("执行替换数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 替换对象名:" + CmdDataString);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"REPLACE INTO {TableName}({CmdDataString}) VALUES({CmdDataString2})";
                    for (int i = 0; i < InsertData.Count; i++)
                    {
                        cmd.Parameters.Add(InsertData[i].ObjectName, DbAutoType(InsertData[i].ObjectData)).Value = InsertData[i].ObjectData;
                    }
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("替换数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 替换对象名:" + CmdDataString);
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
        /// 更新数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">更新内容(对象名/内容)</param>
        /// <returns>返回是否成功</returns>
        /*public static bool Update(string dbPath, string TableName, List<InsertData> InsertData)
        {
            List<string> CmdData = new();
            string CmdDataString = "";
            //List<string> CmdData2 = new();
            //string CmdDataString2 = "";
            foreach (var InData in InsertData)
            {
                CmdData.Add(InData.ObjectName!+"=@"+ InData.ObjectName!);
                //CmdData2.Add("@" + InData.ObjectName!);
            }
            CmdDataString = string.Join(",", CmdData.ToArray());
            //CmdDataString2 = string.Join(",", CmdData2.ToArray());
            Logger.WriteDebug("执行更新数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 更新对象名:" + CmdDataString);
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"UPDATE {TableName} SET {CmdDataString} WHERE {InsertData[0].ObjectName}={InsertData[0].ObjectData}";
                    for (int i = 0; i < InsertData.Count; i++)
                    {
                        cmd.Parameters.Add(InsertData[i].ObjectName, DbAutoType(InsertData[i].ObjectData)).Value = InsertData[i].ObjectData;
                    }
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("更新数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 更新对象名:" + CmdDataString);
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
        }*///....
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">替换内容(对象名/内容)</param>
        /// <returns>返回是否成功</returns>
        public static bool Delete(string dbPath, string TableName, List<InsertData> InsertData)
        {
            Logger.WriteDebug("执行删除数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 删除对象名:" + $"{ InsertData[0].ObjectName}={ InsertData[0].ObjectData}");
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"DELETE FROM {TableName} WHERE {InsertData[0].ObjectName}={InsertData[0].ObjectData}";
                    cmd.ExecuteNonQuery();
                    Logger.WriteInfor("删除数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 删除对象名:" + $"{InsertData[0].ObjectName}={InsertData[0].ObjectData}");
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
        /// 读取数据
        /// </summary>
        /// <param name="dbPath">指定数据库文件</param>
        /// <param name="TableName">表名称</param>
        /// <param name="InsertData">替换内容(对象名/内容)</param>
        /// <returns>返回是否成功</returns>
        public static DataTable? Select(string dbPath, string TableName, InsertData InsertData)
        {
            Logger.WriteDebug("执行读取数据 位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 读取对象名:" + $"{InsertData.ObjectName}={InsertData.ObjectData}");
            DataTable dTable = new DataTable();
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + dbPath);
            try
            {
                if (sqliteConn.State != System.Data.ConnectionState.Open)
                {
                    sqliteConn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = sqliteConn;
                    cmd.CommandText = $"SELECT * FROM {TableName}";
                    cmd.ExecuteNonQuery();
                    SQLiteDataReader myReader = cmd.ExecuteReader();
                    
                    dTable.Load(myReader);

                    Logger.WriteInfor("读取数据成功!位置:" + Path.GetFullPath(dbPath) + " 表名:" + TableName + " 读取对象名:" + $"{InsertData.ObjectName}={InsertData.ObjectData}");
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
            return dTable;
        }

        /// <summary>
        /// 输入对象类型
        /// </summary>
        public class InsertData
        {
            public string? ObjectName { get; set; }
            public object? ObjectData { get; set; }
        }
        /// <summary>
        /// 系统类型与Sqlite转换
        /// </summary>
        public static DbType DbAutoType(object? input)
        {
            switch (input!.GetType().ToString())
            {
                case "System.Int16":
                    return DbType.Int16;
                case "System.Int32":
                    return DbType.Int32;
                case "System.Int64":
                    return DbType.Int64;
                case "System.String":
                    return DbType.String;
                case "System.Boolean":
                    return DbType.Boolean;
                case "System.Object":
                    return DbType.Object;
            }
            return DbType.Object;
        }











    }
}
