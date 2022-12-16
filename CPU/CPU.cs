// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Management;

Console.WriteLine("Hello, World!");

PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");

/*for (int i=0; i<=10;i++)
{
    //Console.WriteLine("CPU: {0:n1}%", cpu.NextValue());
    Console.WriteLine(Math.Round(cpu.NextValue(), 1) + "%");
    Thread.Sleep(100);
}*/