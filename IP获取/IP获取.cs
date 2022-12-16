using System;
using System.Globalization;
using System.Linq;
using System.Net;
using Python.Runtime;



List<IPAddress> GetExternalIP()
{
    IPAddress[] publicIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
    return publicIP.ToList();
}



List<IPAddress> GetInternalIP()
{
    IPAddress[] localIP = Dns.GetHostByName(Dns.GetHostName()).AddressList;
    return localIP.ToList();
}

//Console.WriteLine(GetExternalIP().First()+" "+ GetInternalIP().First());


