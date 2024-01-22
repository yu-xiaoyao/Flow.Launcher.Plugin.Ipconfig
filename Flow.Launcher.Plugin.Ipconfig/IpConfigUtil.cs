using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace Flow.Launcher.Plugin.Ipconfig;

public class IpAddrs
{
    public string Ipv4 { set; get; }
    public string Ipv6 { set; get; }

    public override string ToString()
    {
        if (Ipv4 != null && Ipv6 != null)
        {
            return $"ipv4 = {Ipv4}, ipv6 = {Ipv6}";
        }

        return Ipv4 ?? Ipv6;
    }
}

public class NetAttrs
{
    public string Id { set; get; }
    public string Name { set; get; }
    public NetworkInterfaceType NetworkInterfaceType { set; get; }
    public byte[] PhysicalAddress { set; get; }
    public IpAddrs IpAddress { set; get; }
    public IpAddrs GatewayAddress { set; get; }

    public override string ToString()
    {
        return $"Id = {Id} , Name = {Name} , IpAddress: {IpAddress} , GatewayAddress: {GatewayAddress}";
    }

    public string CopyIpString()
    {
        return $"IpAddress : {IpAddress}\nGatewayAddress : {GatewayAddress}";
    }
}

public class IpConfigUtil
{
    public static List<NetAttrs> GetNetworkInterfaces()
    {
        var result = new List<NetAttrs>();

        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var adapter in networkInterfaces)
        {
            // only get the Up
            if (OperationalStatus.Up != adapter.OperationalStatus) continue;

            var netType = adapter.NetworkInterfaceType;
            if (netType == NetworkInterfaceType.Loopback)
            {
                continue;
            }

            Console.WriteLine($"UP: {adapter.Id} - {adapter.Name}");

            // if (netType == NetworkInterfaceType.Ethernet
            //     || netType == NetworkInterfaceType.Wireless80211
            //     || netType == NetworkInterfaceType.Ppp)
            // {
            // }

            var ipProperties = adapter.GetIPProperties();

            var gatewayAddresses = ipProperties.GatewayAddresses;

            string gatewayIpv4 = null;
            string gatewayIpv6 = null;
            foreach (var ipaddr in gatewayAddresses)
            {
                if (ipaddr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    gatewayIpv4 = ipaddr.Address.ToString();
                }
                else if (ipaddr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    gatewayIpv6 = ipaddr.Address.ToString();
                }
            }


            // 没有网关地址不要. 
            if (gatewayIpv4 == null && gatewayIpv6 == null) continue;

            var unicastAddresses = ipProperties.UnicastAddresses;
            string ipv4 = null;
            string ipv6 = null;
            foreach (var ipaddr in unicastAddresses)
            {
                if (ipaddr.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4 = ipaddr.Address.ToString();
                }
                else if (ipaddr.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (ipaddr.Address.IsIPv6LinkLocal) continue;
                    if (SuffixOrigin.Random == ipaddr.SuffixOrigin) continue;

                    ipv6 ??= ipaddr.Address.ToString();
                }
            }

            if (ipv4 == null && ipv6 == null) continue;

            var netAttr = new NetAttrs()
            {
                Id = adapter.Id,
                Name = adapter.Name,
                NetworkInterfaceType = netType,
                PhysicalAddress = adapter.GetPhysicalAddress().GetAddressBytes(),
                GatewayAddress = new IpAddrs()
                {
                    Ipv4 = gatewayIpv4,
                    Ipv6 = gatewayIpv6
                },
                IpAddress = new IpAddrs()
                {
                    Ipv4 = ipv4,
                    Ipv6 = ipv6
                }
            };

            adapter.GetPhysicalAddress().GetAddressBytes();

            result.Add(netAttr);

            // Console.WriteLine($"ip: {ipv4} - {ipv6}");
            // Console.WriteLine("---");
        }

        return result;
    }
}