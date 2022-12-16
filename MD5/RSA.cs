using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;
using System.Management;
using System.Threading;
using System.Diagnostics;

namespace MD5
{
    internal class RSA
    {
        public static string GetCpuID()
        {
            try
            {
                string? cpuInfo = "";//cpu序列号
                ManagementClass? mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection? moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }

            finally { }
        }


        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="content">待加密内容</param>
        /// <param name="secretKey">KEY</param>
        /// <returns>Byte[] 加密内容</returns>
        public static byte[] TextEncrypt(string content, string secretKey = @"12345678")
        {
            byte[] data = Encoding.UTF8.GetBytes(content);
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[i % key.Length];
            }

            return data;
        }
        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="data">待解密内容</param>
        /// <param name="secretKey">KEY</param>
        /// <returns>Steing 解密内容</returns>
        public static string TextDecrypt(byte[] data, string secretKey = @"12345678")
        {
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= key[i % key.Length];
            }

            return Encoding.UTF8.GetString(data, 0, data.Length);
        }
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="inputFile">待加密文件位置</param>
        /// <param name="outputFile">输出加密文件位置</param>
        /// <param name="password">KEY</param>
        /// <param name="DeleteFiles">删除输入文件</param>
        /// <returns></returns>
        public static void FileEncrypt(string inputFile, string? outputFile = null, string password = @"12345678", bool DeleteFiles = false)
        {
            outputFile ??= inputFile;//如果未指定输出位置则覆盖原加密文件


            UnicodeEncoding UE = new();
            byte[] key = UE.GetBytes(password);

            string cryptFile = outputFile;
            FileStream fsCrypt = new(cryptFile, FileMode.Create);

#pragma warning disable SYSLIB0022 // 类型或成员已过时
            RijndaelManaged RMCrypto = new();
#pragma warning restore SYSLIB0022 // 类型或成员已过时

            CryptoStream cs = new(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);

            FileStream fsIn = new(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();

            if (DeleteFiles)
                File.Delete(inputFile);
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="inputFile">待解密文件位置</param>
        /// <param name="outputFile">输出解密文件位置</param>
        /// <param name="password">KEY</param>
        /// <param name="DeleteFiles">删除入出文件</param>
        /// <returns></returns>
        public static void FileDecrypt(string inputFile, string? outputFile = null, string password = @"12345678", bool DeleteFiles = false)
        {
            outputFile ??= inputFile;//如果未指定输出位置则覆盖原加密文件

            UnicodeEncoding UE = new();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new(inputFile, FileMode.Open);

#pragma warning disable SYSLIB0022 // 类型或成员已过时
            RijndaelManaged RMCrypto = new();
#pragma warning restore SYSLIB0022 // 类型或成员已过时

            CryptoStream cs = new(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

            FileStream fsOut = new(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

            if (DeleteFiles)
                File.Delete(inputFile);
        }
        /// <summary>
        /// 解密文件内容
        /// </summary>
        /// <param name="inputFile">待解密文件位置</param>
        /// <param name="password">KEY</param>
        /// <returns></returns>
        public static string FileDecryptData(string inputFile,string password = @"12345678")
        {

            UnicodeEncoding UE = new();
            byte[] key = UE.GetBytes(password);

            FileStream fsCrypt = new(inputFile, FileMode.Open);

#pragma warning disable SYSLIB0022 // 类型或成员已过时
            RijndaelManaged RMCrypto = new();
#pragma warning restore SYSLIB0022 // 类型或成员已过时

            CryptoStream cs = new(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);

            List<byte>? DataByte = new();
            int data;
            while ((data = cs.ReadByte()) != -1)
                DataByte.Add((byte)data);

            cs.Close();
            fsCrypt.Close();

            return System.Text.Encoding.UTF8.GetString(DataByte.ToArray());
        }



    }
}
