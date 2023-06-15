using System.Collections.Concurrent;
using System.Collections.Generic;

namespace 异步
{
    internal class 多线程
    {
        public class ThreadManagement
        {
            /// <summary>
            /// 最大线程数
            /// </summary>
            public int MaxThread;
            /// <summary>
            /// 多线程列队
            /// </summary>
            public List<Thread> ThreadList;
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="MaxThread"></param>
            public ThreadManagement(List<Thread> ThreadList, int MaxThread = 4)
            {
                this.ThreadList = ThreadList;
                this.MaxThread = MaxThread;
            }

            public void Start()
            {
                foreach (var DataIn in ThreadList) 
                {
                    DataIn.Start();
                }
            }
        }
    }
}
