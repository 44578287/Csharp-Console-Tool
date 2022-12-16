using System.Net.NetworkInformation;

static string GetMacByNetworkInterface()
{
    try
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface ni in interfaces)
        {
            return BitConverter.ToString(ni.GetPhysicalAddress().GetAddressBytes());
        }
    }
    catch (Exception)
    {
    }
    return "00-00-00-00-00-00";
}

string mac地址 = GetMacByNetworkInterface();
Console.WriteLine(mac地址);