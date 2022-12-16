using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG记录
{
    internal class LOG
    {
        public class ConsoleCopy : IDisposable
        {
            private FileStream m_FileStream;
            private StreamWriter m_FileWriter;

            private readonly TextWriter m_DoubleWriter;
            private readonly TextWriter m_OldOut;

            private class DoubleWriter : TextWriter
            {
                private TextWriter m_One;
                private TextWriter m_Two;

                public DoubleWriter(TextWriter one, TextWriter two)
                {
                    m_One = one;
                    m_Two = two;
                }

                public override Encoding Encoding
                {
                    get { return m_One.Encoding; }
                }

                public override void Flush()
                {
                    m_One.Flush();
                    m_Two.Flush();
                }

                public override void Write(char value)
                {
                    m_One.Write(value);
                    m_Two.Write(value);
                }
            }

            public ConsoleCopy(string path)
            {
                m_OldOut = Console.Out;

                try
                {
                    m_FileStream = File.Create(path);

                    m_FileWriter = new StreamWriter(m_FileStream)
                    {
                        AutoFlush = true
                    };

                    m_DoubleWriter = new DoubleWriter(m_FileWriter, m_OldOut);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot open file for writing");
                    Console.WriteLine(e.Message);
                    return;
                }
                Console.SetOut(m_DoubleWriter);
            }

            public void Dispose()
            {
                Console.SetOut(m_OldOut);

                if (m_FileWriter != null)
                {
                    m_FileWriter.Flush();
                    m_FileWriter.Close();
                    m_FileWriter = null;
                }
                if (m_FileStream != null)
                {
                    m_FileStream.Close();
                    m_FileStream = null;
                }
            }
        }
    }
}
