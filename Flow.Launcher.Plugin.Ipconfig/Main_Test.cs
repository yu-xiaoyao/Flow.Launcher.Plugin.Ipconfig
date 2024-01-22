using System;

namespace Flow.Launcher.Plugin.Ipconfig;

public class Main_Test
{
    /// <summary>
    /// FOR TEST
    /// </summary>
    public static void Main()
    {
        var result = IpConfigUtil.GetNetworkInterfaces();
        foreach (var netAttrs in result)
        {
            Console.WriteLine($"{netAttrs}");
        }
    }
}