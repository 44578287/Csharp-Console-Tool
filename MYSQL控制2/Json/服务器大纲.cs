using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYSQL控制2.Json
{
    internal class Json服务器大纲
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public int ID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string 服务器版本 { get; set; }
            /// <summary>
            /// 模组
            /// </summary>
            public string 服务器类型 { get; set; }
            /// <summary>
            /// 启用
            /// </summary>
            public string 服务器状态 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string 服务器地址 { get; set; }
        }
    }
}
