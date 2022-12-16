using System.Diagnostics;
using System.IO.Ports;
using System.Text;

namespace 服务端.MODS
{
    internal class SerialPort
    {
        public class SerialPortUtils
        {

            public static string[] GetPortNames()
            {
                return System.IO.Ports.SerialPort.GetPortNames();
            }

            public static System.IO.Ports.SerialPort? SerialPort = null;
            public static System.IO.Ports.SerialPort OpenClosePort(string comName, int baud)
            {
                //串口未打开
                if (SerialPort == null || !SerialPort.IsOpen)
                {
                    SerialPort = new System.IO.Ports.SerialPort();
                    //串口名称
                    SerialPort.PortName = comName;
                    //波特率
                    SerialPort.BaudRate = baud;
                    //数据位
                    SerialPort.DataBits = 8;
                    //停止位
                    SerialPort.StopBits = StopBits.One;
                    //校验位
                    SerialPort.Parity = Parity.Even;
                    //打开串口
                    SerialPort.Open();
                    //串口数据接收事件实现
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);

                    return SerialPort;
                }
                //串口已经打开
                else
                {
                    SerialPort.Close();
                    return SerialPort;
                }
            }

            public static void ReceiveData(object sender, SerialDataReceivedEventArgs e)
            {
                System.IO.Ports.SerialPort _SerialPort = (System.IO.Ports.SerialPort)sender;

                int _bytesToRead = _SerialPort.BytesToRead;
                byte[] recvData = new byte[_bytesToRead];

                _SerialPort.Read(recvData, 0, _bytesToRead);

                //向控制台打印数据
                Debug.WriteLine("收到数据：" + System.Text.Encoding.UTF8.GetString(recvData));
            }

            public static bool SendData(byte[] data)
            {
                if (SerialPort != null && SerialPort.IsOpen)
                {
                    SerialPort.Write(data, 0, data.Length);
                    Debug.WriteLine("发送数据：" + data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public static bool SendData(string Data)
            {
                byte[] data = Encoding.UTF8.GetBytes(Data);
                if (SerialPort != null && SerialPort.IsOpen)
                {
                    SerialPort.Write(data, 0, data.Length);
                    //Debug.WriteLine("发送数据：" + data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
