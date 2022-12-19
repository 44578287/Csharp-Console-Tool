using System.Text;

namespace MD5
{
    internal class 加密
    {
        /// <summary>
        /// 初始化加密KEY
        /// </summary>
        /// <param name="KeyPath">KEY位置</param>
        /// <param name="Password">Key密码</param>
        /// <returns>返回是否初始化及加密UUID</returns>
        public static bool InitializationEncrypt(string KeyPath, string Password)
        {
            bool first = false;
            if (File.Exists(KeyPath))//判断KEY文件是否存在
            {
                first = true;
                string uuid = System.Guid.NewGuid().ToString();//生成UUID
                File.WriteAllText(KeyPath, uuid);//生成KEY文件并保存UUID
                DES.FileEncrypt(KeyPath, Password);//加密KEY文件
            }
            //Password = DES.FileDecryptData(KeyPath, Password);//解密KEY文件获取UUID
            return first;
        }
        /// <summary>
        /// 解密KEY
        /// </summary>
        /// <param name="KeyPath">KEY位置</param>
        /// <param name="Password">Key密码</param>
        /// <returns>返回UUID</returns>
        public static string? DecryptEncrypt(string KeyPath, string Password)
        {
            //string? first = null;
            if (!File.Exists(KeyPath))//判断KEY文件是否存在
            {
                //InitializationEncrypt(KeyPath, Password);//如果没有密钥就生成一个
                return null;
            }
            //Password = DES.FileDecryptData(KeyPath, Password);//解密KEY文件获取UUID
            return DES.FileDecryptData(KeyPath, Password);
        }
        /// <summary>
        /// 初始化使用者Cookie
        /// </summary>
        /// <param name="CookiePath">Cookie根目录</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserCookie">用户Cookie</param>
        /// <param name="UUID">UUID</param>
        /// <param name="Forced">强制生成新的加密Cookie</param>
        /// <returns>返回是否初始化用户Cookie</returns>
        public static bool InitializationUser(string CookiePath, string UserName, string UserCookie, string UUID, bool Forced = false)
        {
            bool first = false;
            if (!Directory.Exists(CookiePath))//判断Cookie文件是否存在
            {
                Directory.CreateDirectory(CookiePath);//创建Cookie文件夹
            }
            string UserCookiePath = CookiePath + "/" + UserName;
            if (!Directory.Exists(UserCookiePath) || Directory.GetFiles(UserCookiePath).Length == 0)//判断用户Cookie文件是否存在及是否有Cookie存在
            {
                Directory.CreateDirectory(UserCookiePath);//创建用户Cookie文件夹
                string? KeyCookie = DES.TextEncrypt(UserCookie, UUID);//加密用户Cookie
                File.WriteAllText(UserCookiePath + "/" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString() + ".cookie", KeyCookie);//以时间戳保存加密用户Cookie
                first = true;
            }

            List<string> KeyCookieList = new();
            foreach (var InData in Directory.GetFiles(UserCookiePath))
                KeyCookieList.Add(System.IO.Path.GetFileNameWithoutExtension(InData));//获取最新的Cookie

            if (new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() - long.Parse(KeyCookieList.Max()!) > 2592000 || Forced == true)//判断Cookie是否超过一个月以及是否强制生成
            {
                string? KeyCookie = DES.TextEncrypt(UserCookie, UUID);//加密用户Cookie
                File.WriteAllText(UserCookiePath + "/" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString() + ".cookie", KeyCookie);//以时间戳保存加密用户Cookie
                first = true;
            }
            return first;
        }
        /// <summary>
        /// 解密使用者Cookie
        /// </summary>
        /// <param name="CookiePath">Cookie根目录</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UUID">UUID</param>
        /// <returns>返回用户Cookie</returns>
        public static string? DecryptUserCookie(string CookiePath, string UserName, string UUID)
        {
            string? first = null;
            string UserCookiePath = CookiePath + "/" + UserName;
            if (!Directory.Exists(UserCookiePath) || Directory.GetFiles(UserCookiePath).Length == 0)//判断用户Cookie文件是否存在及是否有Cookie存在
            {
                return null;
            }

            List<string> KeyCookieList = new();
            foreach (var InData in Directory.GetFiles(UserCookiePath))
                KeyCookieList.Add(System.IO.Path.GetFileNameWithoutExtension(InData));//获取最新的Cookie

            if (new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() - long.Parse(KeyCookieList.Max()!) < 2592000)//判断Cookie是否超过一个月
            {
                string KeyCookie = File.ReadAllText(UserCookiePath + "/" + KeyCookieList.Max() + ".cookie", Encoding.UTF8);//读取加密Cookie
                return DES.TextDecrypt(KeyCookie, UUID);//解密Cookie
            }
            return null;
        }
    }
}
