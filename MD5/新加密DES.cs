using System.Security.Cryptography;
using System.Text;

namespace MD5
{
    internal class 新加密DES
    {
        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="TextData">待加密内容</param>
        /// <param name="Key">KEY(原偏移量)</param>
        /// <returns>string 加密内容</returns>
        public static string? TextEncrypt(string TextData, string Key = "44578287")
        {
            Key = ToDES(Key);
            byte[] DES = Encoding.Default.GetBytes(Key);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.Default.GetBytes(TextData);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(DES, DES), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="TextData">待加密内容</param>
        /// <param name="Key">KEY(原偏移量)</param>
        /// <returns>string 加密内容</returns>
        public static string? TextDecrypt(string TextData, string Key = "44578287")
        {
            Key = ToDES(Key);
            byte[] DES = Encoding.Default.GetBytes(Key);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(TextData);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DES, DES), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }

                    return Encoding.Default.GetString(ms.ToArray());
                }
                catch
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inputFile">待加密文件位置</param>
        /// <param name="outputFile">输出加密文件位置</param>
        /// <param name="Key">KEY</param>
        /// <returns>输出文件路径</returns>
        public static string? FileEncrypt(string inputFile, string outputFile, string Key = "44578287")
        {
            if (!File.Exists(inputFile))
            {
                return null;
            }

            Key = ToDES(Key);
            byte[] DES = Encoding.Default.GetBytes(Key);;
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            byte[] btFile = File.ReadAllBytes(inputFile);

            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(DES, DES), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    fs.Close();
                }
                return outputFile;
            }
        }
        /// <summary>
        /// 加密文件并覆盖原文件
        /// </summary>
        /// <param name="inputFile">待加密文件位置</param>
        /// <param name="Key">KEY</param>
        /// <returns>输出文件路径</returns>
        public static string? FileEncrypt(string inputFile, string Key = "44578287")
        {
            string? returnData = FileEncrypt(inputFile, inputFile, Key);
            if (returnData == null)
                return null;
            return returnData;
        }


        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inputFile">待解密文件位置</param>
        /// <param name="outputFile">输出解密文件位置</param>
        /// <param name="Key">KEY</param>
        /// <returns>输出文件路径</returns>
        public static string? FileDecrypt(string inputFile, string outputFile, string Key = "44578287")
        {
            if (!File.Exists(inputFile))
            {
                return null;
            }
            Key = ToDES(Key);
            byte[] DES = Encoding.Default.GetBytes(Key);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            byte[] btFile = File.ReadAllBytes(inputFile);

            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(DES, DES), CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    fs.Close();
                }
                return outputFile;
            }
        }
        /// <summary>
        /// 解密文件并覆盖原文件
        /// </summary>
        /// <param name="inputFile">待解密的文件的绝对路径</param>
        /// <param name="Key">KEY</param>
        /// <returns>输出文件路径</returns>
        public static string? FileDecrypt(string inputFile, string Key = "44578287")
        {
            string? returnData = FileDecrypt(inputFile, inputFile, Key);
            if (returnData == null)
                return null;
            return returnData;
        }
        /// <summary>
        /// 解密文件内容
        /// </summary>
        /// <param name="inputFile">待解密文件位置</param>
        /// <param name="outputFile">输出解密文件位置</param>
        /// <param name="Key">KEY</param>
        /// <returns>输出文件路径</returns>
        public static string? FileDecryptData(string inputFile, string Key = "44578287")
        {
            if (!File.Exists(inputFile))
            {
                return null;
            }
            Key = ToDES(Key);
            byte[] DES = Encoding.Default.GetBytes(Key);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            byte[] btFile = File.ReadAllBytes(inputFile);
            MemoryStream fs = new();//文件写入改内存流
            string? returnData = null;
            try
            {
                using (CryptoStream cs = new (fs, des.CreateDecryptor(DES, DES), CryptoStreamMode.Write))
                {
                    cs.Write(btFile, 0, btFile.Length);
                    cs.FlushFinalBlock();
                    returnData = System.Text.Encoding.Default.GetString(fs.ToArray());
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                fs.Close();
            }
            return returnData;
        }

        /// <summary>
        /// 将一个不是8位字符串转成8位！！
        /// </summary>
        /// <param name="str"></param>
        public static string ToDES(string str)
        {
            if (str.Length <= 8)
            {
                str = str.ToLower().PadRight(8, 'a');//在字符串右边加a加满8位！！！
                return str;
            }
            else
            {
                str = str.Remove(8);//把第九位以后的字符删完！
                return str;
            }
        }
    }
}
