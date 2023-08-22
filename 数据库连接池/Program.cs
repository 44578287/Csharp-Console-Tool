using System.Collections.Concurrent;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Reflection;
using LoongEgg.LoongLogger;

Logger.Enable(LoggerType.Console | LoggerType.Debug, LoggerLevel.Debug);//注册Log日志函数


//SQLite SQLite = new(new(), new() { MinPoolSize = 1, MaxPoolSize = 5 });
//var Conn = SQLite.Open();
//Console.WriteLine(SQLite.Command("SELECT * FROM main.user WHERE name = 'data_644658.csv'", Conn)?.ExecuteReader().StepCount);
//SQLite.Close(Conn);
//await Test2Async();
//while (true)
{
    //SQLite.InsertData("user", new[] { "name" }, new[] { "A" });
    //SQLite.InsertData("user", new() { {"name", "A"} });
    //SQLite.BulkInsertData("user", new[] {"name" }, new() { new[] {"A" } });
    //Console.WriteLine(SQLite.GetAllTableNames()[0]);
    //Console.WriteLine(SQLite.GetPoolSize());
    //Thread.Sleep(1000);
}

//SQLite.CreateTable("testdata", new[] { "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT", "name TEXT", "time INTEGER" });

/*var Temp_Data = FillDataClassListIgnoreCase<TestData>(SQLite.QueryData("testdata", null, "1"));

foreach (var data in Temp_Data)
{ 
    Console.WriteLine($"{data.Id} {data.Name} {data.Time}");
}
*/

// 使用 using 语句确保在使用后自动释放资源
using (var context = new ApplicationDbContext())
{
    // 确保数据库中的表已经创建，如果不存在则创建它
    context.Database.EnsureCreated();
}

// 继承 DbContext 类，用于管理数据库连接和操作
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } // DbSet 表示数据库中的表

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 配置数据库连接字符串，这里使用 SQLite 数据库
        optionsBuilder.UseSqlite("Data Source=mydatabase.db");
    }
}


// 数据库中的数据模型类
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}



/*stopwatch.Stop();
TimeSpan elapsedTime = stopwatch.Elapsed;
Console.WriteLine($"代码执行时长：{elapsedTime.TotalMilliseconds} 毫秒");*/

/*void Test()
{
    for (int i = 0; i < 10; i++)
    {
        try
        {
            //SQLiteConnection connection = new("Data Source=db.db;Version=3;");
            //connection.Open();
            SQLiteConnection connection = dbPool.GetConnection();
            // Use the connection to perform database operations
            using (SQLiteCommand Cmd = new("SELECT * FROM main.user WHERE name = 'data_644658.csv'", connection))
            {
                //Console.WriteLine(Cmd.ExecuteReader().StepCount);
            }
            // After finishing the operation, release the connection back to the pool
            Thread.Sleep(10);
            dbPool.ReleaseConnection(connection);
            //connection.Clone();
        }
        catch (TimeoutException ex)
        {
            Console.WriteLine("Failed to acquire a database connection: " + ex.Message);
        }
    }
}*/

/*async Task Test2Async()
{
    //string connectionString = "Data Source=db.db;Version=3;";
    Console.WriteLine("Initial pool size: " + dbPool.GetCurrentPoolSize());

    int numTasks = 100;
    Task[] tasks = new Task[numTasks];

    for (int i = 0; i < numTasks; i++)
    {
        tasks[i] = Task.Run(async () =>
        {
            SQLiteConnection connection = dbPool.GetConnection();
            Console.WriteLine("Acquired connection. Pool size: " + dbPool.GetCurrentPoolSize());
            // Simulate some database operation
            using (SQLiteCommand Cmd = new("SELECT * FROM main.user WHERE name = 'data_644658.csv'", connection))
            {
                Console.WriteLine(Cmd.ExecuteReader().StepCount);
            }
            await Task.Delay(1000);
            dbPool.ReleaseConnection(connection);
            Console.WriteLine("Released connection. Pool size: " + dbPool.GetCurrentPoolSize());
        });
    }

    await Task.WhenAll(tasks);

    while (true)
    {
        Thread.Sleep(500);
        Console.WriteLine("Final pool size: " + dbPool.GetCurrentPoolSize());
    }

    Console.WriteLine("Final pool size: " + dbPool.GetCurrentPoolSize());
}
*/
/*
/// <summary>
/// SQL相关
/// </summary>
public static class SQL1
{
    /// <summary>
    /// Sqlite连接池类
    /// </summary>
    public class SQLiteConnectionPool
    {
        private readonly string _connectionString;
        private readonly Stack<SQLiteConnection> _connectionPool;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="poolSize">连接池大小</param>
        public SQLiteConnectionPool(string connectionString, int poolSize)
        {
            Logger.WriteInfor($"初始化数据库连接池 连接池大小:{poolSize}");
            _connectionString = connectionString;
            _connectionPool = new Stack<SQLiteConnection>();

            for (int i = 0; i < poolSize; i++)
            {
                SQLiteConnection connection = new(_connectionString);
                _connectionPool.Push(connection);
            }
        }
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns>连接</returns>
        private SQLiteConnection CreateConnection()
        {
            Logger.WriteWarn("连接池已耗尽!创建新的连接!");
            return new SQLiteConnection(_connectionString);
        }

        /// <summary>
        /// 从连接池中获取一个连接
        /// </summary>
        /// <returns>SQLite 连接</returns>
        public SQLiteConnection AcquireConnection()
        {
            Logger.WriteDebug($"获取SQL连接 {_connectionPool.Count}");
            lock (_connectionPool)
            {
                if (_connectionPool.Count > 0)
                {
                    return _connectionPool.Pop();
                }
            }

            return CreateConnection();
            //return null;
        }

        /// <summary>
        /// 将连接放回连接池
        /// </summary>
        /// <param name="connection">要放回的连接</param>
        public void ReleaseConnection(SQLiteConnection connection)
        {
            Logger.WriteDebug($"释放SQL连接 {_connectionPool.Count}");
            lock (_connectionPool)
            {
                _connectionPool.Push(connection);
            }
        }
    }
    /// <summary>
    /// SQLit操作类
    /// </summary>
    public class SQLite
    {
        /// <summary>
        /// 连接池
        /// </summary>
        public readonly SQLiteConnectionPool _connectionPool;
        /// <summary>
        /// 连接信息
        /// </summary>
        public ConnData _ConnData { get; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string _ConnStr { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ConnData">连接信息</param>
        /// <param name="poolSize">连接池大小</param>
        /// <exception cref="InvalidOperationException"></exception>
        public SQLite(ConnData ConnData, int poolSize)
        {
            Logger.WriteDebug($"初始化数据库 位置:{ConnData.dbPath}");
            _ConnData = ConnData;
            _ConnStr = ConnData.dbPath;

            if (NewDbFile(_ConnData.dbPath) == null)
            {
                Logger.WriteError($"初始化(创建)数据库失败!");
                throw new InvalidOperationException("初始化(创建)数据库失败!");
            }
            _connectionPool = new SQLiteConnectionPool($"data source={ConnData.dbPath}", poolSize);
            Logger.WriteInfor($"初始化数据库完成 位置:{ConnData.dbPath}");
            ConneTest();
        }
        /// <summary>
        /// 新建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns>新建成功，返回db路径，否则返回null</returns>
        static public string? NewDbFile(string dbPath)
        {
            Logger.WriteDebug("执行创建数据库 位置:" + System.IO.Path.GetFullPath(dbPath));
            if (!System.IO.File.Exists(dbPath))
            {
                Logger.WriteInfor("未在:" + System.IO.Path.GetFullPath(dbPath) + " 找到数据库即将执行创建");
                try
                {
                    SQLiteConnection.CreateFile(dbPath);
                    Logger.WriteInfor("创建成功!位置:" + System.IO.Path.GetFullPath(dbPath));
                    return dbPath;
                }
                catch (Exception Message)
                {
                    Logger.WriteError("错误!" + Message);
                    return null;
                }
            }
            Logger.WriteInfor("数据库存在!位置:" + System.IO.Path.GetFullPath(dbPath));
            return dbPath;
        }
        /// <summary>
        /// 数据库连接测试
        /// </summary>
        /// <returns>初始化成功OR失败</returns>
        public bool ConneTest()
        {
            SQLiteConnection connection;
            try
            {
                connection = Open()!;
                Close(connection);
            }
            catch
            {
                Logger.WriteError("数据库连接失败!");
                return false;
            }
            Logger.WriteInfor("数据库连接成功!");
            return true;
        }

        /// <summary>
        /// Sql命令执行
        /// </summary>
        /// <param name="Sql">Sql命令</param>
        /// <param name="connection">连接</param>
        /// <returns>执行结果</returns>
        public SQLiteCommand? Command(string Sql, SQLiteConnection? connection)
        {
            SQLiteCommand Cmd;
            try
            {
                Cmd = new(Sql, connection);
                Logger.WriteDebug($"执行命令成功! 影响行数:{Cmd.ExecuteNonQuery()} 命令:{Sql}");
            }
            catch (Exception ex)
            {
                Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                return null;
            }
            return Cmd;
        }
        /// <summary>
        /// Sql命令执行(单次)
        /// </summary>
        /// <param name="Sql">Sql命令</param>
        /// <returns>执行结果</returns>
        public SQLiteDataReader? CommandSingle(string Sql)
        {
            SQLiteDataReader ReData;
            SQLiteConnection connection = Open()!;
            try
            {
                connection = Open()!;
                SQLiteCommand Cmd = new(Sql, connection);
                ReData = Cmd.ExecuteReader();
                Logger.WriteDebug($"执行命令成功! 影响行数:{Cmd.ExecuteNonQuery()} 命令:{Sql}");
            }
            catch (Exception ex)
            {
                Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                return null;
            }
            finally
            {
                Close(connection);
            }
            return ReData;
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public SQLiteConnection? Open()
        {
            SQLiteConnection connection = _connectionPool.AcquireConnection(); // Acquire a connection
            try
            {
                connection.Open();
            }
            catch (Exception ex)//报错处理
            {
                //Console.WriteLine(ex.Message);
                Logger.WriteError(ex.Message);
                return null;
            }
            return connection;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close(SQLiteConnection? Input_Data)
        {
            Input_Data?.Close();
            if (Input_Data != null)
                _connectionPool.ReleaseConnection(Input_Data);
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns>状态</returns>
        public ConnectionState Status()
        {
            return _connectionPool.AcquireConnection().State;
        }

        /// <summary>
        /// 连接信息
        /// </summary>
        public class ConnData
        {
            /// <summary>
            /// 数据库路径
            /// </summary>
            public string dbPath { get; set; } = "db.db";
        }
    }
}
*/

/*
/// <summary>
/// 表示一个 SQLite 数据库连接池。
/// </summary>
public class SQLiteDatabasePool
{
    private readonly string _connectionString;
    private readonly ConcurrentQueue<SQLiteConnection> _connectionPool = new ConcurrentQueue<SQLiteConnection>();
    private readonly SemaphoreSlim _connectionSemaphore;
    private readonly Timer _connectionCleanupTimer;
    private readonly object _lock = new object();

    private int _currentPoolSize = 0;

    private int MaxPoolSize = 200;
    private int MinPoolSize = 10;
    private int ConnectionTimeout = 30; // 秒
    private int IdleTimeout = 2; // 秒

    /// <summary>
    /// 初始化一个新的 SQLiteDatabasePool 实例。
    /// </summary>
    /// <param name="connectionString">数据库连接字符串。</param>
    /// <param name="config">连接池配置。</param>
    public SQLiteDatabasePool(string connectionString, SQLiteDatabasePool_Config config)
    {
        MaxPoolSize = config.MaxPoolSize;
        MinPoolSize = config.MinPoolSize;
        ConnectionTimeout = config.ConnectionTimeout;
        IdleTimeout = config.IdleTimeout;

        _connectionString = connectionString;
        _connectionSemaphore = new SemaphoreSlim(MaxPoolSize, MaxPoolSize);

        // 创建一个定时器，定期清理闲置连接
        _connectionCleanupTimer = new Timer(CleanupIdleConnections!, null, IdleTimeout * 1000, IdleTimeout * 1000);

        InitializePool();
    }

    // 初始化连接池，创建最小连接数的连接并添加到池中
    private void InitializePool()
    {
        lock (_lock)
        {
            for (int i = 0; i < MinPoolSize; i++)
            {
                SQLiteConnection connection = CreateNewConnection();
                _connectionPool.Enqueue(connection);
                _currentPoolSize++;
            }
            Console.WriteLine("连接池初始化大小：" + _currentPoolSize);
        }
    }

    // 获取一个数据库连接
    public SQLiteConnection GetConnection()
    {
        if (_connectionSemaphore.Wait(ConnectionTimeout * 1000))
        {
            if (_connectionPool.TryDequeue(out SQLiteConnection? connection))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                return connection;
            }
            else
            {
                SQLiteConnection newConnection = CreateNewConnection();
                _currentPoolSize++; // 增加连接池大小
                return newConnection;
            }
        }

        throw new TimeoutException("在超时时间内无法获取数据库连接。");
    }

    // 释放数据库连接回连接池
    public void ReleaseConnection(SQLiteConnection connection)
    {
        if (connection.State == ConnectionState.Open)
        {
            _connectionPool.Enqueue(connection);
        }
        _connectionSemaphore.Release();
    }

    // 创建新的数据库连接
    private SQLiteConnection CreateNewConnection()
    {
        SQLiteConnection connection = new SQLiteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    // 清理闲置连接，自动调整连接池大小
    private void CleanupIdleConnections(object state)
    {
        lock (_lock)
        {
            int targetPoolSize = Math.Max(MinPoolSize, _currentPoolSize - 5); // 自动减小到最小连接数
            int connectionsToClose = _currentPoolSize - targetPoolSize;

            while (connectionsToClose > 0)
            {
                if (_connectionPool.TryDequeue(out SQLiteConnection? connection))
                {
                    connection.Close();
                    _currentPoolSize--; // 减少连接池大小
                    connectionsToClose--;
                }
            }
        }
    }

    /// <summary>
    /// 获取当前连接池大小
    /// </summary>
    /// <returns>当前连接池大小</returns>
    public int GetCurrentPoolSize()
    {
        return _currentPoolSize;
    }
    /// <summary>
    /// 配置文件
    /// </summary>
    public class SQLiteDatabasePool_Config
    {
        /// <summary>
        /// 最大连接数。
        /// </summary>
        public int MaxPoolSize { get; set; } = 200;

        /// <summary>
        /// 最小连接数。
        /// </summary>
        public int MinPoolSize { get; set; } = 10;

        /// <summary>
        /// 连接超时时间（秒）。
        /// </summary>
        public int ConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// 闲置连接超时时间（秒）。
        /// </summary>
        public int IdleTimeout { get; set; } = 2;
    }
}
*/
/// <summary>
/// SQL相关
/// </summary>
//*
public static class SQL2
{
    /// <summary>
    /// 数据库连接池。
    /// </summary>
    public class SQLiteDatabasePool
    {
        private readonly string _connectionString;
        private readonly ConcurrentQueue<SQLiteConnection> _connectionPool = new ConcurrentQueue<SQLiteConnection>();
        private readonly SemaphoreSlim _connectionSemaphore;
        private readonly Timer _connectionCleanupTimer;
        private readonly object _lock = new object();

        private int _currentPoolSize = 0;

        private int MaxPoolSize = 200;
        private int MinPoolSize = 10;
        private int ConnectionTimeout = 30; // 秒
        private int IdleTimeout = 2; // 秒

        /// <summary>
        /// 初始化一个新的 SQLiteDatabasePool 实例。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="config">连接池配置。</param>
        public SQLiteDatabasePool(string connectionString, SQLiteDatabasePool_Config config)
        {
            MaxPoolSize = config.MaxPoolSize;
            MinPoolSize = config.MinPoolSize;
            ConnectionTimeout = config.ConnectionTimeout;
            IdleTimeout = config.IdleTimeout;

            _connectionString = connectionString;
            _connectionSemaphore = new SemaphoreSlim(MaxPoolSize, MaxPoolSize);

            // 创建一个定时器，定期清理闲置连接
            _connectionCleanupTimer = new Timer(CleanupIdleConnections!, null, IdleTimeout * 1000, IdleTimeout * 1000);

            InitializePool();
        }

        // 初始化连接池，创建最小连接数的连接并添加到池中
        private void InitializePool()
        {
            lock (_lock)
            {
                for (int i = 0; i < MinPoolSize; i++)
                {
                    SQLiteConnection connection = CreateNewConnection();
                    _connectionPool.Enqueue(connection);
                    _currentPoolSize++;
                }
                Logger.WriteInfor("连接池初始化大小：" + _currentPoolSize);
            }
        }

        /// <summary>
        /// 获取一个数据库连接（异步版本）
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public async Task<SQLiteConnection> GetConnectionAsync()
        {
            if (await _connectionSemaphore.WaitAsync(ConnectionTimeout * 1000))
            {
                if (_connectionPool.TryDequeue(out SQLiteConnection? connection))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        await connection.OpenAsync();
                    }
                    return connection;
                }
                else
                {
                    SQLiteConnection newConnection = CreateNewConnection();
                    _currentPoolSize++; // 增加连接池大小
                    return newConnection;
                }
            }

            throw new TimeoutException("在超时时间内无法获取数据库连接。");
        }
        /// <summary>
        /// 获取一个数据库连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public SQLiteConnection GetConnection()
        {
            if (_connectionSemaphore.Wait(ConnectionTimeout * 1000))
            {
                if (_connectionPool.TryDequeue(out SQLiteConnection? connection))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    return connection;
                }
                else
                {
                    SQLiteConnection newConnection = CreateNewConnection();
                    _currentPoolSize++; // 增加连接池大小
                    return newConnection;
                }
            }

            throw new TimeoutException("在超时时间内无法获取数据库连接。");
        }

        // 释放数据库连接回连接池
        public void ReleaseConnection(SQLiteConnection connection)
        {
            if (connection.State == ConnectionState.Open)
            {
                _connectionPool.Enqueue(connection);
            }
            _connectionSemaphore.Release();
        }

        // 创建新的数据库连接
        private SQLiteConnection CreateNewConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // 清理闲置连接，自动调整连接池大小（异步版本）
        private void CleanupIdleConnections(object state)
        {
            lock (_lock)
            {
                Logger.WriteDebug($"当前连接池大小:{_currentPoolSize}");
                int targetPoolSize = Math.Max(MinPoolSize, _currentPoolSize - 5); // 自动减小到最小连接数
                int connectionsToClose = _currentPoolSize - targetPoolSize;

                while (connectionsToClose > 0)
                {
                    if (_connectionPool.TryDequeue(out SQLiteConnection? connection))
                    {
                        connection.Close();
                        _currentPoolSize--; // 减少连接池大小
                        connectionsToClose--;
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前连接池大小
        /// </summary>
        /// <returns>当前连接池大小</returns>
        public int GetCurrentPoolSize()
        {
            return _currentPoolSize;
        }
    }

    /// <summary>
    /// 连接池配置类。
    /// </summary>
    public class SQLiteDatabasePool_Config
    {
        /// <summary>
        /// 最大连接数。
        /// </summary>
        public int MaxPoolSize { get; set; } = 200;

        /// <summary>
        /// 最小连接数。
        /// </summary>
        public int MinPoolSize { get; set; } = 10;

        /// <summary>
        /// 连接超时时间（秒）。
        /// </summary>
        public int ConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// 闲置连接超时时间（秒）。
        /// </summary>
        public int IdleTimeout { get; set; } = 2;
    }
    /// <summary>
    /// SQLit操作类
    /// </summary>
    public class SQLite
    {
        /// <summary>
        /// 连接池
        /// </summary>
        private readonly SQLiteDatabasePool _dbPool;
        /// <summary>
        /// 连接信息
        /// </summary>
        private ConnData _ConnData { get; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _ConnStr { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ConnData">连接信息</param>
        /// <param name="poolSize">连接池大小</param>
        /// <exception cref="InvalidOperationException"></exception>
        public SQLite(ConnData ConnData, SQLiteDatabasePool_Config Config)
        {
            Logger.WriteDebug($"初始化数据库 位置:{ConnData.dbPath}");
            _ConnData = ConnData;
            _ConnStr = ConnData.dbPath;

            if (NewDbFile(_ConnData.dbPath) == null)
            {
                Logger.WriteError($"初始化(创建)数据库失败!");
                throw new InvalidOperationException("初始化(创建)数据库失败!");
            }
            _dbPool = new SQLiteDatabasePool($"data source={ConnData.dbPath}", Config);
            Logger.WriteInfor($"初始化数据库完成 位置:{ConnData.dbPath}");
            ConneTest();
        }

        /// <summary>
        /// 新建数据库文件
        /// </summary>
        /// <param name="dbPath">数据库文件路径及名称</param>
        /// <returns>新建成功，返回db路径，否则返回null</returns>
        static public string? NewDbFile(string dbPath)
        {
            Logger.WriteDebug("执行创建数据库 位置:" + System.IO.Path.GetFullPath(dbPath));
            if (!System.IO.File.Exists(dbPath))
            {
                Logger.WriteInfor("未在:" + System.IO.Path.GetFullPath(dbPath) + " 找到数据库即将执行创建");
                try
                {
                    SQLiteConnection.CreateFile(dbPath);
                    Logger.WriteInfor("创建成功!位置:" + System.IO.Path.GetFullPath(dbPath));
                    return dbPath;
                }
                catch (Exception Message)
                {
                    Logger.WriteError("错误!" + Message);
                    return null;
                }
            }
            Logger.WriteInfor("数据库存在!位置:" + System.IO.Path.GetFullPath(dbPath));
            return dbPath;
        }
        /// <summary>
        /// 备份数据库到指定路径。
        /// </summary>
        /// <param name="backupPath">备份文件的路径。</param>
        public void BackupDb(string backupPath)
        {
            Logger.WriteInfor($"备份数据库至:{backupPath}");
            try
            {
                if (System.IO.File.Exists(_ConnStr))
                {
                    System.IO.File.Copy(_ConnStr, backupPath, true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"备份数据库时出错!因为:{ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// 还原数据库从指定的备份文件。
        /// </summary>
        /// <param name="backupPath">备份文件的路径。</param>
        public void RestoreDb(string backupPath)
        {
            Logger.WriteInfor($"还原数据库:{backupPath}");
            try
            {
                if (System.IO.File.Exists(backupPath))
                {
                    System.IO.File.Copy(backupPath, _ConnStr, true);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"还原数据库时出错!因为:{ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// 数据库连接测试
        /// </summary>
        /// <returns>初始化成功OR失败</returns>
        public bool ConneTest()
        {
            SQLiteConnection connection;
            try
            {
                connection = Open()!;
                Close(connection);
            }
            catch
            {
                Logger.WriteError("数据库连接失败!");
                return false;
            }
            Logger.WriteInfor("数据库连接成功!");
            return true;
        }

        [Obsolete("不好管理资源啊!")]
        /// <summary>
        /// Sql命令执行
        /// </summary>
        /// <param name="Sql">Sql命令</param>
        /// <param name="connection">连接</param>
        /// <returns>执行结果</returns>
        public SQLiteCommand? Command(string Sql, SQLiteConnection? connection)
        {
            SQLiteCommand Cmd;
            try
            {
                Cmd = new(Sql, connection);
                Logger.WriteDebug($"执行命令成功! 影响行数:{Cmd.ExecuteNonQuery()} 命令:{Sql}");
            }
            catch (Exception ex)
            {
                Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                return null;
            }
            return Cmd;
        }
        [Obsolete("不好管理资源啊!")]
        /// <summary>
        /// Sql命令执行(单次)
        /// </summary>
        /// <param name="Sql">Sql命令</param>
        /// <returns>执行结果</returns>
        public SQLiteDataReader? CommandSingle(string Sql)
        {
            SQLiteDataReader? ReData = null;
            SQLiteConnection? connection = null;
            try
            {
                connection = Open();
                SQLiteCommand Cmd = new(Sql, connection);
                ReData = Cmd.ExecuteReader();
                Logger.WriteDebug($"执行命令成功! 影响行数:{ReData.StepCount} 命令:{Sql}");
            }
            catch (Exception ex)
            {
                Logger.WriteError($"执行命令失败! 因为: {ex.Message} 命令:{Sql}");
                return null;
            }
            finally
            {
                //ReData?.Close();
                Close(connection);
            }
            return ReData;
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public SQLiteConnection? Open()
        {
            SQLiteConnection connection;
            try
            {
                connection = _dbPool.GetConnection();
            }
            catch (Exception ex)//报错处理
            {
                //Console.WriteLine(ex.Message);
                Logger.WriteError(ex.Message);
                return null;
            }
            return connection;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close(SQLiteConnection? Input_Data)
        {
            if (Input_Data != null)
                _dbPool.ReleaseConnection(Input_Data);
        }
        /// <summary>
        /// 获取当前连接池大小
        /// </summary>
        /// <returns>连接池数量</returns>
        public int GetPoolSize()
        {
            return _dbPool.GetCurrentPoolSize();
        }

        /// <summary>
        /// 向指定表中插入数据。
        /// </summary>
        /// <param name="tableName">要插入数据的表名。</param>
        /// <param name="columns">要插入的列名数组。</param>
        /// <param name="values">对应的值数组。</param>
        /// <exception cref="ArgumentException">当列数与值数不匹配时引发。</exception>
        public void InsertData(string tableName, string[] columns, object?[] values)
        {
            Logger.WriteDebug($"插入表'{tableName}' 插入项目{string.Join(",", columns)}");
            SQLiteConnection? connection = null;
            try
            {
                if (columns.Length != values.Length)
                {
                    throw new ArgumentException("列数与值数必须相等。");
                }
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateInsertQuery(tableName, columns);

                    for (int i = 0; i < columns.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@{columns[i]}", values[i]);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"插入数据时出错!因为:{ex.Message}");
                throw; // 将异常继续抛出
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 向指定表中插入数据。
        /// </summary>
        /// <param name="tableName">要插入数据的表名。</param>
        /// <param name="data">包含要插入的列名及其对应值的字典。</param>
        /// <exception cref="ArgumentException">当列数与值数不匹配时引发。</exception>
        public void InsertData(string tableName, Dictionary<string, object?>? data)
        {
            if (data == null)
                return;
            Logger.WriteDebug($"插入表'{tableName}' 插入项目{string.Join(",", data.Keys)}");
            SQLiteConnection? connection = null;
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    List<string> columns = new List<string>(data.Keys);
                    List<object?> values = new List<object?>(data.Values);

                    if (columns.Count != values.Count)
                    {
                        throw new ArgumentException("列数与值数必须相等。");
                    }

                    command.CommandText = GenerateInsertQuery(tableName, columns.ToArray());

                    for (int i = 0; i < columns.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@{columns[i]}", values[i]);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"插入数据时出错!因为:{ex.Message}");
                throw; // 将异常继续抛出
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 执行批量插入操作，向指定表中插入多行数据。
        /// </summary>
        /// <param name="tableName">要插入数据的表名。</param>
        /// <param name="columns">要插入的列名数组。</param>
        /// <param name="batchValues">包含多个数据行的批量值列表。</param>
        /// <exception cref="ArgumentException">当列数和批量值为空，或批量值的列数与指定列数不匹配时引发。</exception>
        public void BulkInsertData(string tableName, string[] columns, List<object?[]> batchValues)
        {
            Logger.WriteDebug($"批量插入表'{tableName}' 插入项目{string.Join(",", columns)} 插入数量{batchValues.Count}");
            SQLiteConnection? connection = null;
            try
            {
                if (columns.Length == 0 || batchValues.Count == 0)
                {
                    throw new ArgumentException("列数和批量值不能为空。");
                }
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    string insertQuery = GenerateInsertQuery(tableName, columns);

                    for (int rowIndex = 0; rowIndex < batchValues.Count; rowIndex++)
                    {
                        object?[] values = batchValues[rowIndex];

                        if (values.Length != columns.Length)
                        {
                            throw new ArgumentException($"批量插入时出错，第 {rowIndex + 1} 行的列数与值数不匹配。");
                        }

                        string parameterNames = string.Join(", ", columns.Select((col, index) => $"@{col}_{rowIndex}"));
                        string valuesQuery = string.Join(", ", parameterNames.Split(',').Select(_ => $"@{_.TrimStart('@')}"));

                        string fullInsertQuery = $"{insertQuery} VALUES ({valuesQuery})";

                        command.CommandText = fullInsertQuery;

                        for (int columnIndex = 0; columnIndex < columns.Length; columnIndex++)
                        {
                            command.Parameters.AddWithValue($"@{columns[columnIndex]}_{rowIndex}", values[columnIndex]);
                        }

                        command.ExecuteNonQuery();
                        command.Parameters.Clear(); // Clear parameters for the next iteration
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"批量插入数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于插入数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要插入数据的表名。</param>
        /// <param name="columns">要插入的列名数组。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateInsertQuery(string tableName, string[] columns)
        {
            string columnNames = string.Join(", ", columns);
            string parameterNames = string.Join(", ", columns.Select(c => $"@{c}"));
            return $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";
        }

        /// <summary>
        /// 更新指定表中的数据。
        /// </summary>
        /// <param name="tableName">要更新的表名。</param>
        /// <param name="columns">要更新的列名数组。</param>
        /// <param name="values">对应的新值数组。</param>
        /// <param name="condition">更新数据的条件。</param>
        /// <exception cref="ArgumentException">当列数与值数不匹配时引发。</exception>
        public void UpdateData(string tableName, string[] columns, object?[] values, string condition)
        {
            Logger.WriteDebug($"更新表'{tableName}' 更新项目{string.Join(",", columns)} 条件 {condition}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                if (columns.Length != values.Length)
                {
                    throw new ArgumentException("列数与值数必须相等。");
                }
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateUpdateQuery(tableName, columns, condition);

                    for (int i = 0; i < columns.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@{columns[i]}", values[i]);
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"更新数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 更新指定表中的数据。
        /// </summary>
        /// <param name="tableName">要更新的表名。</param>
        /// <param name="data">包含要更新的列名及对应的新值的字典。</param>
        /// <param name="condition">更新数据的条件。</param>
        /// <exception cref="ArgumentException">当列数与值数不匹配时引发。</exception>
        public void UpdateData(string tableName, Dictionary<string, object?>? data, string condition)
        {
            if (data == null)
                return;
            Logger.WriteDebug($"更新表'{tableName}' 更新项目{string.Join(",", data.Keys)} 条件 {condition}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    List<string> updateExpressions = new List<string>();
                    foreach (var kvp in data)
                    {
                        command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        updateExpressions.Add($"{kvp.Key} = @{kvp.Key}");
                    }

                    command.CommandText = GenerateUpdateQuery(tableName, updateExpressions, condition);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"更新数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于更新数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要更新的表名。</param>
        /// <param name="columns">要更新的列名数组。</param>
        /// <param name="condition">更新数据的条件。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateUpdateQuery(string tableName, string[] columns, string condition)
        {
            string setClause = string.Join(", ", columns.Select(c => c + " = @" + c));
            return $"UPDATE {tableName} SET {setClause} WHERE {condition}";
        }
        /// <summary>
        /// 生成用于更新数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要更新的表名。</param>
        /// <param name="updateExpressions">数据。</param>
        /// <param name="condition">更新数据的条件。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateUpdateQuery(string tableName, List<string> updateExpressions, string condition)
        {
            string setClause = string.Join(", ", updateExpressions);
            return $"UPDATE {tableName} SET {setClause} WHERE {condition}";
        }

        /// <summary>
        /// 查询指定表中满足条件的数据。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="columns">要查询的列名数组。为 null 或空数组时表示查询所有列。</param>
        /// <param name="condition">查询条件。</param>
        /// <param name="orderBy">排序条件。例如： "ColumnName ASC"。</param>
        /// <returns>满足条件的数据表。</returns>
        public DataTable QueryData(string tableName, string[]? columns, string condition, string? orderBy = null)
        {
            Logger.WriteDebug($"查询表'{tableName}' 查询项目{string.Join(",", columns ?? new string[] { "*" })} 条件 {condition} 排序 {orderBy}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateQueryQuery(tableName, columns, condition, orderBy);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"查询数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 分页查询指定表中的数据。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="columns">要查询的列名数组。</param>
        /// <param name="condition">查询条件。</param>
        /// <param name="orderBy">排序条件。例如： "ColumnName ASC"。</param>
        /// <param name="pageNumber">页码（从1开始）。</param>
        /// <param name="pageSize">每页的条目数。</param>
        /// <returns>指定页的数据表。</returns>
        public DataTable QueryDataWithPagination(string tableName, string[]? columns, string condition, int pageNumber, int pageSize, string? orderBy = null)
        {
            Logger.WriteDebug($"分页查询表'{tableName}' 查询项目{string.Join(",", columns ?? new string[] { "*" })} 条件 {condition} 排序 {orderBy} 第 {pageNumber} 页 每页 {pageSize} 条");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateQueryWithPaginationQuery(tableName, columns, condition, pageNumber, pageSize, orderBy);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"分页查询数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于查询数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="columns">要查询的列名数组。</param>
        /// <param name="condition">查询条件。</param>
        /// <param name="orderBy">排序条件。例如： "ColumnName ASC"。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateQueryQuery(string tableName, string[]? columns, string condition, string? orderBy = null)
        {
            string columnNames;

            if (columns == null || columns.Length == 0)
            {
                columnNames = "*"; // 查询所有列
            }
            else
            {
                columnNames = string.Join(", ", columns);
            }
            if (orderBy != null)
                return $"SELECT {columnNames} FROM {tableName} WHERE {condition} ORDER BY {orderBy}";
            else
                return $"SELECT {columnNames} FROM {tableName} WHERE {condition}";
        }
        /// <summary>
        /// 生成用于分页查询数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="columns">要查询的列名数组。</param>
        /// <param name="condition">查询条件。</param>
        /// <param name="pageNumber">页码（从1开始）。</param>
        /// <param name="pageSize">每页的条目数。</param>
        /// <param name="orderBy">排序条件。例如： "ColumnName ASC"。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateQueryWithPaginationQuery(string tableName, string[]? columns, string condition, int pageNumber, int pageSize, string? orderBy = null)
        {
            string columnNames;
            if (columns == null || columns.Length == 0)
            {
                columnNames = "*"; // 查询所有列
            }
            else
            {
                columnNames = string.Join(", ", columns);
            }
            int offset = (pageNumber - 1) * pageSize;
            if (orderBy != null)
                return $"SELECT {columnNames} FROM {tableName} WHERE {condition} ORDER BY {orderBy} LIMIT {pageSize} OFFSET {offset}";
            else
                return $"SELECT {columnNames} FROM {tableName} WHERE {condition} LIMIT {pageSize} OFFSET {offset}";
        }

        /// <summary>
        /// 获取指定查询条件下的数据总行数。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="condition">查询条件。</param>
        /// <returns>数据总行数。</returns>
        public int GetTotalRowCount(string tableName, string condition)
        {
            Logger.WriteDebug($"询表项目数量'{tableName}' 条件{condition}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateTotalRowCountQuery(tableName, condition);
                    int rowCount = Convert.ToInt32(command.ExecuteScalar());
                    return rowCount;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"获取数据总行数时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于查询数据总行数的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要查询的表名。</param>
        /// <param name="condition">查询条件。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateTotalRowCountQuery(string tableName, string condition)
        {
            return $"SELECT COUNT(*) FROM {tableName} WHERE {condition}";
        }

        /// <summary>
        /// 删除指定表中满足条件的数据。
        /// </summary>
        /// <param name="tableName">要删除数据的表名。</param>
        /// <param name="condition">删除数据的条件。</param>
        public void DeleteData(string tableName, string condition)
        {
            Logger.WriteDebug($"删除表'{tableName}' 条件 {condition}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateDeleteQuery(tableName, condition);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"删除数据时出错!因为:{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于删除数据的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要删除数据的表名。</param>
        /// <param name="condition">删除数据的条件。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateDeleteQuery(string tableName, string condition)
        {
            return $"DELETE FROM {tableName} WHERE {condition}";
        }

        /// <summary>
        /// 创建表。
        /// </summary>
        /// <param name="tableName">要创建的表名。</param>
        /// <param name="columns">要创建的列定义数组。</param>
        public void CreateTable(string tableName, string[] columns)
        {
            Logger.WriteDebug($"创建表'{tableName}' 创建项目{string.Join(",", columns)}");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateCreateTableQuery(tableName, columns);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("创建表时出错：" + ex.Message);
                throw; // 将异常继续抛出
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于创建表的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要创建的表名。</param>
        /// <param name="columns">要创建的列定义数组。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateCreateTableQuery(string tableName, string[] columns)
        {
            string columnDefinitions = string.Join(", ", columns);
            return $"CREATE TABLE {tableName} ({columnDefinitions})";
        }

        /// <summary>
        /// 删除表。
        /// </summary>
        /// <param name="tableName">要删除的表名。</param>
        public void DropTable(string tableName)
        {
            Logger.WriteDebug($"删除表'{tableName}'");
            SQLiteConnection? connection = null; // 获取连接
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateDropTableQuery(tableName);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("删除表时出错：" + ex.Message);
                throw; // 将异常继续抛出
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于删除表的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要删除的表名。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateDropTableQuery(string tableName)
        {
            return $"DROP TABLE IF EXISTS {tableName}";
        }

        /// <summary>
        /// 判断指定名称的表是否存在。
        /// </summary>
        /// <param name="tableName">要判断的表名。</param>
        /// <returns>如果表存在，返回true；否则返回false。</returns>
        public bool TableExists(string tableName)
        {
            Logger.WriteDebug($"判断表是否存在：'{tableName}'");
            SQLiteConnection? connection = null;
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateTableExistsQuery(tableName);
                    int tableCount = Convert.ToInt32(command.ExecuteScalar());
                    return tableCount > 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"判断表是否存在时出错：{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于判断指定表是否存在的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要判断的表名。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateTableExistsQuery(string tableName)
        {
            return $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'";
        }

        /// <summary>
        /// 获取数据库中所有表的名称。
        /// </summary>
        /// <returns>包含所有表名的字符串数组。</returns>
        public string[] GetAllTableNames()
        {
            Logger.WriteDebug("获取所有表名");
            SQLiteConnection? connection = null;
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateGetAllTableNamesQuery();
                    List<string> tableNames = new List<string>();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableNames.Add(reader.GetString(0));
                        }
                    }
                    return tableNames.ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"获取表名时出错：{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于获取数据库中所有表的名称的SQLite查询语句。
        /// </summary>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateGetAllTableNamesQuery()
        {
            return "SELECT name FROM sqlite_master WHERE type='table'";
        }
        /*
        /// <summary>
        /// 获取指定表的列信息。
        /// </summary>
        /// <param name="tableName">要获取列信息的表名。</param>
        /// <returns>包含列信息的List。</returns>
        public List<TableColumnInfo> GetTableColumns(string tableName)
        {
            Logger.WriteDebug($"获取表'{tableName}'的列信息");
            SQLiteConnection? connection = null;
            try
            {
                connection = Open();
                using (SQLiteCommand command = connection!.CreateCommand())
                {
                    command.CommandText = GenerateGetTableColumnsQuery(tableName);
                    List<TableColumnInfo> columnInfoList = new List<TableColumnInfo>();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TableColumnInfo columnInfo = new TableColumnInfo
                            {
                                ColumnId = reader.GetInt32(0),
                                ColumnName = reader.GetString(1),
                                DataType = reader.GetString(2),
                                IsPrimaryKey = reader.GetInt32(5) == 1,
                                ColumnConstraints = reader.GetString(4) // 添加约束信息的获取
                            };
                            columnInfoList.Add(columnInfo);
                        }
                    }
                    return columnInfoList;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError($"获取表列信息时出错：{ex.Message}");
                throw;
            }
            finally
            {
                Close(connection);
            }
        }
        /// <summary>
        /// 生成用于获取指定表的列信息的SQLite查询语句。
        /// </summary>
        /// <param name="tableName">要获取列信息的表名。</param>
        /// <returns>生成的SQLite查询语句。</returns>
        private string GenerateGetTableColumnsQuery(string tableName)
        {
            return $"PRAGMA table_info({tableName})";
        }
        /// <summary>
        /// 表列信息的数据结构。
        /// </summary>
        public class TableColumnInfo
        {
            /// <summary>
            /// 列的序号。
            /// </summary>
            public int ColumnId { get; set; }

            /// <summary>
            /// 列名。
            /// </summary>
            public string? ColumnName { get; set; }

            /// <summary>
            /// 数据类型。
            /// </summary>
            public string? DataType { get; set; }

            /// <summary>
            /// 是否为主键。
            /// </summary>
            public bool IsPrimaryKey { get; set; }

            /// <summary>
            /// 列的约束。
            /// </summary>
            public string? ColumnConstraints { get; set; }

            // 添加其他列信息的属性，如默认值等
        }
        */

        /// <summary>
        /// 将 DataTable 中的数据填充到指定的数据类对象列表中。
        /// </summary>
        /// <typeparam name="T">要填充的数据类类型。</typeparam>
        /// <param name="dataTable">包含数据的 DataTable。</param>
        /// <returns>填充了数据的数据类对象列表。</returns>
        public static List<T> FillDataClassList<T>(DataTable dataTable) where T : new()
        {
            List<T> dataList = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T dataItem = new T();

                foreach (DataColumn column in dataTable.Columns)
                {
                    PropertyInfo? property = typeof(T).GetProperty(column.ColumnName);
                    if (property != null)
                    {
                        object value = row[column];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(dataItem, value, null);
                        }
                    }
                }

                dataList.Add(dataItem);
            }

            return dataList;
        }

        /// <summary>
        /// 将 DataTable 中的数据填充到指定的数据类对象列表中（忽略大小写）。
        /// </summary>
        /// <typeparam name="T">要填充的数据类类型。</typeparam>
        /// <param name="dataTable">包含数据的 DataTable。</param>
        /// <returns>填充了数据的数据类对象列表。</returns>
        public static List<T> FillDataClassListIgnoreCase<T>(DataTable dataTable) where T : new()
        {
            List<T> dataList = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T dataItem = new T();

                foreach (DataColumn column in dataTable.Columns)
                {
                    PropertyInfo? property = typeof(T).GetProperty(column.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        object value = row[column];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(dataItem, value, null);
                        }
                    }
                }

                dataList.Add(dataItem);
            }

            return dataList;
        }

        /// <summary>
        /// 将数据类对象列表转换为字典。
        /// </summary>
        /// <typeparam name="T">数据类类型。</typeparam>
        /// <param name="dataList">数据类对象列表。</param>
        /// <param name="NoNull">不要Null值。</param>
        /// <returns>字典，键为数据类属性，值为数据类对象列表中相应属性的值列表。</returns>
        public static Dictionary<string, List<object?>?> ConvertToDictionary<T>(List<T> dataList, bool NoNull = false) where T : class
        {
            Dictionary<string, List<object?>?> dictionary = new();

            PropertyInfo[] properties = typeof(T).GetProperties();
            if (NoNull)
                properties = properties.Where(item => item != null).ToArray();
            foreach (PropertyInfo property in properties)
            {
                List<object?>? values = dataList.Select(item => property.GetValue(item)).ToList();
                if (values != null && NoNull || !NoNull)
                    dictionary[property.Name] = values;
            }

            return dictionary;
        }

        /// <summary>
        /// 将数据类对象转换为字典。
        /// </summary>
        /// <typeparam name="T">数据类类型。</typeparam>
        /// <param name="dataObject">数据类对象。</param>
        /// <param name="NoNull">不要Null值。</param>
        /// <returns>字典，键为数据类属性，值为数据类对象的属性值。</returns>
        public static Dictionary<string, object?>? ConvertToDictionary<T>(T dataObject, bool NoNull = false) where T : class
        {
            Dictionary<string, object?>? dictionary = new();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(dataObject);
                if (value != null && NoNull || !NoNull)
                    dictionary[property.Name] = value;
            }

            return dictionary;
        }

        /// <summary>
        /// 连接信息
        /// </summary>
        public class ConnData
        {
            /// <summary>
            /// 数据库路径
            /// </summary>
            public string dbPath { get; set; } = "db.db";
        }
    }


}

