using System.Text;
using System.Security.Cryptography;
using MD5;
using System.Net;

/*string 字符串MD5(string input)
{
    // step 1, calculate MD5 hash from input

    MD5 md5 = System.Security.Cryptography.MD5.Create();

    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

    byte[] hash = md5.ComputeHash(inputBytes);

    // step 2, convert byte array to hex string

    StringBuilder sb = new StringBuilder();

    for (int i = 0; i < hash.Length; i++)
    {
        sb.Append(hash[i].ToString("X2"));
    }

    return sb.ToString().ToLower();
}
string 字符串MD5_2(string str)
{
    byte[] b = System.Text.Encoding.Default.GetBytes(str);

    b = new MD5CryptoServiceProvider().ComputeHash(b);
    string ret = "";
    for (int i = 0; i < b.Length; i++)
    {
        ret += b[i].ToString("x").PadLeft(2, '0');
    }
    return ret;
}

string 文件MD5(string fileName)
{
    try
    {
        FileStream file = new FileStream(fileName, FileMode.Open);
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(file);
        file.Close();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString().ToLower();
    }
    catch (Exception ex)
    {
        throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
    }
}
*/
//Console.WriteLine(字符串MD5("这是Python分开打包后的测试"));
//Console.WriteLine(字符串MD5_2("这是Python分开打包后的测试"));
//string KEY = GetCpuID();
//byte []Data = MD5.RSA.TextEncrypt("你好", KEY);
//Console.WriteLine(TextDecrypt(Data, KEY));
//Console.WriteLine(RSADecrypt("44578287", "你好!!"));

//string cookie = "你好哇";

DateTime beforDT = System.DateTime.Now;
/*
Guid uuid = System.Guid.NewGuid();
Console.WriteLine(uuid);
Console.WriteLine(cookie);

byte[] KeyCookie = MD5.RSA.TextEncrypt(cookie, uuid.ToString());//用GUID加密cookie
File.WriteAllText("KEY", uuid.ToString());//创建KEY并写入GUID

MD5.RSA.FileEncrypt("KEY");//加密文件
Console.WriteLine(File.ReadAllText("KEY", Encoding.UTF8));

string uuid2 = MD5.RSA.FileDecryptData("KEY");//解密文件
Console.WriteLine(MD5.RSA.TextDecrypt(System.Text.Encoding.UTF8.GetBytes(uuid2)));*/
//RijndaelManaged myRijndael = new();
//myRijndael.GenerateKey();
//Console.WriteLine(System.Text.Encoding.UTF8.GetString(myRijndael.Key));
//MD5.RSA.EncryptFile("KEY", "KEY1", "123");//解密文件
//MD5.RSA.DecryptFile("KEY1","YEK","123");//解密文件
//string? uuid = System.Guid.NewGuid().ToString();
//Console.WriteLine(cookie);
//Console.WriteLine(uuid);
/*string? cookieKey = 新加密DES.TextEncrypt(cookie, uuid);
Console.WriteLine(cookieKey);
File.WriteAllText("KEY", uuid);//生成KEY文件并保存UUID
新加密DES.FileEncrypt("KEY","44578287");//加密KEY的UUID文件
uuid = 新加密DES.FileDecryptData("KEY","44578287");//解密UUID
Console.WriteLine(uuid);
Console.WriteLine(新加密DES.TextDecrypt(cookieKey, uuid));//解密cookie*/
/*string? KEY = 加密.DecryptEncrypt("Cookie/KEY", "44578287");

if (KEY == null)//判断是否有密钥
{
    加密.InitializationEncrypt("Cookie/KEY", "44578287");//如果没有密钥就生成一个
    KEY = 加密.DecryptEncrypt("Cookie/KEY", "44578287");//解密刚生成密钥
}
Console.WriteLine(KEY);

Console.WriteLine(加密.InitializationUser("Cookie","admin", "我爱你", KEY!));
Console.WriteLine(加密.DecryptUserCookie("Cookie", "admin", KEY!));*/
string? KEY = null;
string? Cookie = null;
while (KEY == null || Cookie == null)
{
    KEY = 加密.DecryptEncrypt("Cookie/KEY", "44578287");//解密刚生成密钥
    if (KEY == null)//判断是否有密钥
    {
        加密.InitializationEncrypt("Cookie/KEY", "44578287");//如果没有密钥就生成一个
        KEY = 加密.DecryptEncrypt("Cookie/KEY", "44578287");//解密刚生成密钥
        Console.WriteLine(加密.InitializationUser("Cookie", "admin", "你姐不出来的啦", KEY!, true));
    }
    KEY = 加密.DecryptEncrypt("Cookie/KEY", "44578287");//解密刚生成密钥
    Console.WriteLine(KEY);
    Console.WriteLine(Cookie = 加密.DecryptUserCookie("Cookie", "admin", KEY!));
    if (Cookie == null)
    {
        Console.WriteLine(加密.InitializationUser("Cookie", "admin", "你姐不出来的啦", KEY!, true));
    }
}


//Console.WriteLine(MD5.DES.TextDecrypt("U2FsdGVkX1/CXPOU9FcWGVMxBzDtAO4IFIXjkH9KH20=", "p&s;wd$/"));


/*byte[] DES = Encoding.Default.GetBytes("p&s;wd$/");
#pragma warning disable SYSLIB0021 // 类型或成员已过时
DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时

using (MemoryStream ms = new MemoryStream())
{
    byte[] inData = Convert.FromBase64String("c9c1eb210d7bb451b0ae577876d9456d");

    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DES, Encoding.Default.GetBytes("we2?、6、/")), CryptoStreamMode.Write))
    {
        cs.Write(inData, 0, inData.Length);
        cs.FlushFinalBlock();
    }

    Console.WriteLine(Encoding.Default.GetString(ms.ToArray()));





}
*/


string path = @"C:\Projects\vs2022\cloudreve-API";
DirectoryInfo root = new DirectoryInfo(path);
DirectoryInfo[] files = root.GetDirectories();



foreach (var file in files)
Console.WriteLine(file.Name);


Console.WriteLine("DateTime总共花费{0}ms.", System.DateTime.Now.Subtract(beforDT).TotalMilliseconds);