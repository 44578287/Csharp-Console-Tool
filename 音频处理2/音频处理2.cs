using LibMusicVisualizer;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Text.Json;
using static 服务端.MODS.SerialPort;

Console.WriteLine("Hello, World!");

WasapiCapture capture;             // 音频捕获
Visualizer visualizer;             // 可视化
double[]? spectrumData = null;            // 频谱数据
AudioData AudioData = new();


capture = new WasapiLoopbackCapture();          // 捕获电脑发出的声音
visualizer = new Visualizer(256);               // 新建一个可视化器, 并使用 256 个采样进行傅里叶变换
capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);      // 指定捕获的格式, 单声道, 32位深度, IeeeFloat 编码, 8192采样率
capture.DataAvailable += (sender, e) => 
{
    int length = e.BytesRecorded / 4;           // 采样的数量 (每一个采样是 4 字节)
    double[] result = new double[length];       // 声明结果

    for (int i = 0; i < length; i++)
        result[i] = BitConverter.ToSingle(e.Buffer, i * 4);      // 取出采样值

    visualizer.PushSampleData(result);          // 将新的采样存储到 可视化器 中
    
};
capture.StartRecording();
Console.WriteLine("开始:" + capture.CaptureState);



//SerialPortUtils.OpenClosePort("COM3", 1500000);//开启串口
//SerialPortUtils.OpenClosePort("COM3", 2187500);//开启串口
int AAAA = 0;
for (; ; )
{
    //AAAA++;
    DataTimer_Tick();
    AudioData.DataList?.Clear();
    for (int i = 0; i< spectrumData?.Length;i++)
    {
        AudioData.DataList?.Add(spectrumData[i] * 100000 > 64 ? 64 : (int)(spectrumData[i] * 100000));
    }
    //AudioData.Speakers = GetCurrentSpeakerName();
    //AudioData.Volume = GetCurrentSpeakerVolume();
    //SerialPortUtils.SendData(JsonSerializer.Serialize(AudioData));
    Console.WriteLine(JsonSerializer.Serialize(AudioData));
    //Console.WriteLine(AAAA);
    /*if (AAAA > 100)
    {
        if (capture.CaptureState.ToString() == "Capturing")
        {
            capture.StopRecording();
            Console.WriteLine("暂停:" + capture.CaptureState);
        }
        else 
        {
            capture.StartRecording();
            Console.WriteLine("开始:" + capture.CaptureState);
        }
        AAAA = 0;
    }*/


    Thread.Sleep(25);
}



















void DataTimer_Tick()
{
    double[] newSpectrumData = visualizer.GetSpectrumData();         // 从可视化器中获取频谱数据
    newSpectrumData = Visualizer.MakeSmooth(newSpectrumData, 2);                // 平滑频谱数据

    if (spectrumData == null)                                        // 如果已经存储的频谱数据为空, 则把新的频谱数据直接赋值上去
    {
        spectrumData = newSpectrumData;
        return;
    }

    for (int i = 0; i < newSpectrumData.Length; i++)                 // 计算旧频谱数据和新频谱数据之间的 "中间值"
    {
        double oldData = spectrumData[i];
        double newData = newSpectrumData[i];
        double lerpData = oldData + (newData - oldData) * .2f;            // 每一次执行, 频谱值会向目标值移动 20% (如果太大, 缓动效果不明显, 如果太小, 频谱会有延迟的感觉)
        spectrumData[i] = lerpData;
    }
}



int GetCurrentSpeakerVolume()
{
    int volume = 0;
    var enumerator = new MMDeviceEnumerator();

    //获取音频输出设备
    IEnumerable<MMDevice> speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToArray();
    /*foreach (var DataIn in speakDevices)
    {
        Console.WriteLine("{0} ({1})", DataIn.FriendlyName, DataIn.ID);
    }*/
    if (speakDevices.Count() > 0)
    {
        MMDevice mMDevice = speakDevices.ToList()[0];
        volume = Convert.ToInt16(mMDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
    }
    return volume;
}
string? GetCurrentSpeakerName()
{
    var enumerator = new MMDeviceEnumerator();

    //获取音频输出设备
    IEnumerable<MMDevice> speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToArray();
    foreach (var DataIn in speakDevices)
    {
        if (speakDevices.Count() > 0)
        {
            return DataIn.FriendlyName;
        }
        //Console.WriteLine("{0} ({1})", DataIn.FriendlyName, DataIn.ID);
    }
    return null;
}




class AudioData
{
    /// <summary>
    /// 
    /// </summary>
    public string? Speakers { get; set; } = null;
    /// <summary>
    /// 
    /// </summary>
    public int? Volume { get; set; } = 0;
    /// <summary>
    /// 
    /// </summary>
    public List<int>? DataList { get; set; } = new();
}