using Microsoft.Win32;
/*
bool IsRegeditExit(string 路径, string 项名) //查找注册表(项)bool
{
    bool _exit = false;
    string[] subkeyNames;
    RegistryKey key = Registry.CurrentUser; //根HKEY_CURRENT_USER
    RegistryKey software = key.OpenSubKey(路径);
    subkeyNames = software.GetSubKeyNames();
    foreach (string keyName in subkeyNames)
    {
        if (keyName == 项名)
        {
            _exit = true;
            return _exit;
        }
    }
    return _exit;
}

Console.WriteLine(IsRegeditExit("Software\\FinalWire\\AIDA64", "SensorValues"));


bool IsRegeditKeyExit(string 路径, string 值名)//查找注册表(值)bool
{
    string[] subkeyNames;
    RegistryKey hkml = Registry.CurrentUser;
    RegistryKey software = hkml.OpenSubKey(路径, true);
    subkeyNames = software.GetValueNames();
    //取得该项下所有键值的名称的序列，并传递给预定的数组中
    foreach (string keyName in subkeyNames)
    {
        if (keyName == 值名) //判断键值的名称
    {
            hkml.Close();
            return true;
        }
    }
    hkml.Close();
    return false;
}

Console.WriteLine(IsRegeditKeyExit("SOFTWARE\\FinalWire\\AIDA64\\SensorValues", "Value.SCPUCLK"));


void WTRegedit(string 路径, string 值名, string 设定值) //建立注册表值
{
    RegistryKey hklm = Registry.CurrentUser; //根HKEY_CURRENT_USER
    RegistryKey software = hklm.OpenSubKey("", true);
    RegistryKey aimdir = software.CreateSubKey(路径);
    aimdir.SetValue(值名, 设定值);
}

WTRegedit("Software\\XXX", "NAME1", "tovalu1e");


string GetRegistData(string 路径 , string 值名) //读取册表值返回键值
{
{
    string registData;
    RegistryKey hkml = Registry.CurrentUser; //根HKEY_CURRENT_USER
    RegistryKey software = hkml.OpenSubKey("", true);
    RegistryKey aimdir = software.OpenSubKey(路径, true);
    registData = aimdir.GetValue(值名).ToString();
    return registData;
}

Console.WriteLine(GetRegistData("SOFTWARE\\FinalWire\\AIDA64\\SensorValues", "Value.SCPUCLK"));












*/

Console.WriteLine("Hello, World!");




//RegistryKey rkLocalMachine = Registry.LocalMachine;
//RegistryKey rkChild = rkLocalMachine.OpenSubKey("HKEY_CURRENT_USER\\SOFTWARE\\FinalWire\\AIDA64\\SensorValues", true);
//RegistryKey key = Registry.LocalMachine;
//RegistryKey software = key.OpenSubKey("HKEY_CURRENT_USER\\SOFTWARE\\FinalWire\\AIDA64\\SensorValues", true);

/*string[] names = rkChild.GetValueNames();
foreach (string name in names)
{
    string str = rkChild.GetValue(name).ToString();
    Console.WriteLine(str);
}*/

/*bool IsRegeditItemExist()
{
    string[] subkeyNames;
    RegistryKey key = Registry.LocalMachine;
    RegistryKey software = key.OpenSubKey("Software\FinalWire\AIDA64\SensorValues");
    subkeyNames = software.GetSubKeyNames();
    //在这里我是判断test表项是否存在
    foreach (string keyName in subkeyNames)
    {
        if (keyName == "123")
        {
            key.Close();
            return true;
        }
    }
    key.Close();
    return false;
}*/

//IsRegeditItemExist();
/*string GetRegistData(string name)
{
    string registData;
    RegistryKey hkml = Registry.LocalMachine;
    RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
    RegistryKey aimdir = software.OpenSubKey("FinalWire", true);
    registData = aimdir.GetValue(name).ToString();
    return registData;
}*/
//Console.WriteLine(GetRegistData("AIDA64"));

/*void WTRegedit(string 路径, string 值名, string 设定值)
{
    RegistryKey hklm = Registry.CurrentUser;
    RegistryKey software = hklm.OpenSubKey("", true);
    RegistryKey aimdir = software.CreateSubKey(路径);
    aimdir.SetValue(值名, 设定值);
}
WTRegedit("Software\\XXX", "NAME1", "tovalu1e");*/


/*bool IsRegeditExit(string 路径, string 项名)
{
    bool _exit = false;
    string[] subkeyNames;
    RegistryKey key = Registry.CurrentUser;
    RegistryKey software = key.OpenSubKey(路径);
    subkeyNames = software.GetSubKeyNames();
    foreach (string keyName in subkeyNames)
    {
        if (keyName == 项名)
        {
            _exit = true;
            return _exit;
        }
    }
    return _exit;
}*/

//Console.WriteLine(IsRegeditExit("Software\\FinalWire\\AIDA64", "SensorValues"));

/*string GetRegistData(string 路径 , string 值名)
{
    string registData;
    RegistryKey hkml = Registry.CurrentUser;
    RegistryKey software = hkml.OpenSubKey("", true);
    RegistryKey aimdir = software.OpenSubKey(路径, true);
    registData = aimdir.GetValue(值名).ToString();
    return registData;
}

Console.WriteLine(GetRegistData("SOFTWARE\\FinalWire\\AIDA64\\SensorValues", "Value.SCPUCLK"));*/


bool IsRegeditKeyExit(string 路径, string 值名)
{
    string[] subkeyNames;
    RegistryKey hkml = Registry.CurrentUser;
    RegistryKey software = hkml.OpenSubKey(路径, true);
    subkeyNames = software.GetValueNames();
    //取得该项下所有键值的名称的序列，并传递给预定的数组中
    foreach (string keyName in subkeyNames)
    {
        if (keyName == 值名) //判断键值的名称
    {
            hkml.Close();
            return true;
        }
    }
    hkml.Close();
    return false;
}

Console.WriteLine(IsRegeditKeyExit("SOFTWARE\\FinalWire\\AIDA64\\SensorValues", "Value.SCPUCLK"));