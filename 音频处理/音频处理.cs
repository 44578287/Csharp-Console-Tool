// See https://aka.ms/new-console-template for more information
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Text.Json;
using VisioForge.Libs.NAudio.Dsp;

Console.WriteLine("Hello, World!");


AudioData AudioData = new();

/*var enumerator = new MMDeviceEnumerator();
foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
{
    Console.WriteLine("{0} ({1})", endpoint.FriendlyName, endpoint.ID);
}
Console.WriteLine(GetCurrentSpeakerVolume());
*/

int GetCurrentSpeakerVolume()
{
    int volume = 0;
    var enumerator = new MMDeviceEnumerator();

    //获取音频输出设备
    IEnumerable<MMDevice> speakDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToArray();
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


//SerialPortUtils.OpenClosePort("COM3", 921600);//开启串口
//SerialPortUtils.OpenClosePort("COM3", 1500000);//开启串口

List<int> finalData2 = new();
List<int> AA = new();

WasapiLoopbackCapture cap = new WasapiLoopbackCapture();
cap.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 2);      // 指定捕获的格式, 单声道, 32位深度, IeeeFloat 编码, 8192采样率
cap.DataAvailable += (sender, e) =>      // 录制数据可用时触发此事件, 参数中包含音频数据
{
    float[] AllSamples = Enumerable      // 提取数据中的采样
        .Range(0, e.BytesRecorded / 4)   // 除以四是因为, 缓冲区内每 4 个字节构成一个浮点数, 一个浮点数是一个采样
        .Select(i => BitConverter.ToSingle(e.Buffer, i * 4))  // 转换为 float
        .ToArray();    // 转换为数组
                       // 获取采样后, 在这里进行详细处理

    // 设定我们已经将刚刚的采样保存到了变量 AllSamples 中, 类型为 float[]
    int channelCount = cap.WaveFormat.Channels;   // WasapiLoopbackCapture 的 WaveFormat 指定了当前声音的波形格式, 其中包含就通道数
    float[][] channelSamples = Enumerable
        .Range(0, channelCount)
        .Select(channel => Enumerable
            .Range(0, AllSamples.Length / channelCount)
            .Select(i => AllSamples[channel + i * channelCount])
            .ToArray())
        .ToArray();

    // 设定我们已经将分开了的采样保存到了变量 ChannelSamples 中, 类型为 float[][]
    // 例如通道数为2, 那么左声道的采样为 ChannelSamples[0], 右声道为 ChannelSamples[1]
    float[] AverageSamples = Enumerable
        .Range(0, AllSamples.Length / channelCount)
        .Select(index => Enumerable
            .Range(0, channelCount)
            .Select(channel => channelSamples[channel][index])
            .Average())
        .ToArray();

    // 我们将对 AverageSamples 进行傅里叶变换, 得到一个复数数组


    // 因为对于快速傅里叶变换算法, 需要数据长度为 2 的 n 次方, 这里进行
    double log = Math.Ceiling(Math.Log(AverageSamples.Length, 2));   // 取对数并向上取整
    int newLen = (int)Math.Pow(2, log);                             // 计算新长度
    float[] filledSamples = new float[newLen];
    Array.Copy(AverageSamples, filledSamples, AverageSamples.Length);   // 拷贝到新数组
    Complex[] complexSrc = filledSamples
        .Select(v => new Complex() { X = v })        // 将采样转换为复数
        .ToArray();
    //FastFourierTransform(false, log, complexSrc); 


    Complex[] halfData = complexSrc
   .Take(complexSrc.Length / 2)
   .ToArray();    // 一半的数据
    double[] dftData = halfData
        .Select(v => Math.Sqrt(v.X * v.X + v.Y * v.Y))  // 取复数的模
        .ToArray();    // 将复数结果转换为我们所需要的频率幅度
    //Console.WriteLine(dftData.Length);

    // 其实, 到这里你完全可以把这些数据绘制到窗口上, 这已经算是频域图象了, 但是对于音乐可视化来讲, 某些频率的数据我们完全不需要
    // 例如 10000Hz 的频率, 我们完全没必要去绘制它, 取 最小频率 ~ 2500Hz 足矣
    // 对于变换结果, 每两个数据之间所差的频率计算公式为 采样率/采样数, 那么我们要取的个数也可以由 2500 / (采样率 / 采样数) 来得出
    //int count = 2500 / (cap.WaveFormat.SampleRate / filledSamples.Length);
    int count = 128;
    double[] finalData = dftData.Take(count).ToArray();
    //finalData = Visualizer.MakeSmooth(finalData, 2);
    //List<int> finalData2 = new();
    finalData2.Clear();
    for (int i = 0; i < finalData.Length; i++)
    {
        finalData2.Add((int)(finalData[i] * 1000));
    }
    //AA = AA + (((finalData.Max() * 1000) - AA)/1);
    //Console.WriteLine(AA);
    /*AudioData = new()
    {
        Speakers = GetCurrentSpeakerName(),
        Volume = GetCurrentSpeakerVolume(),
        DataList = finalData2
    };
    SerialPortUtils.SendData(JsonSerializer.Serialize(AudioData));
    Console.WriteLine(JsonSerializer.Serialize(AudioData));*/
};


cap.StartRecording();   // 开始录制

Thread.Sleep(2000);

for (; ; )
{
    if (AA.Count == 0)
    {
        for (int i = 0; i < finalData2.Count; i++)
            AA.Add(0);
    }

    for (int i = 0; i < finalData2.Count; i++)
    {
        //AA[i] = (int)(AA[i] + (((finalData2[i]) - AA[i]) / 8));
        //AA[i] = finalData2[i];
        AA[i] = (int)(AA[i] + (finalData2[i] - AA[i]) * .2f);
    }



    //AudioData.Speakers = GetCurrentSpeakerName();
    //AudioData.Volume = GetCurrentSpeakerVolume();
    AudioData.DataList = AA;
    AudioData.Speakers = GetCurrentSpeakerName();
    AudioData.Volume = GetCurrentSpeakerVolume();
    //SerialPortUtils.SendData(JsonSerializer.Serialize(AudioData));
    Console.WriteLine(JsonSerializer.Serialize(AudioData));
    Thread.Sleep(10);
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