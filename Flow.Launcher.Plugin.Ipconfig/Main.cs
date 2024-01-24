using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Flow.Launcher.Plugin.Ipconfig
{
    public class Ipconfig : IPlugin, IContextMenu
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            var resultList = new List<Result>();
            var interfaces = IpConfigUtil.GetNetworkInterfaces();

            foreach (var attrs in interfaces)
            {
                var result = new Result()
                {
                    IcoPath = "ipconfig-icon.png",
                    Title = attrs.Name,
                    SubTitle = attrs.IpAddress.ToString(),
                    Action = _ => CopyAddress(attrs.IpAddress),
                    PreviewPanel = new Lazy<UserControl>(() =>
                    {
                        // _context.API.LogInfo("IpConfig", "PreviewPanel<><>");
                        return new IpPreviewPanel(attrs, content => _context.API.CopyToClipboard(content));
                        //   new IpconfigPreviewPanel(attrs, content => _context.API.CopyToClipboard(content))
                    }),
                    ContextData = attrs
                };

                resultList.Add(result);
            }

            return resultList;
        }

        public List<Result> LoadContextMenus(Result selectedResult)
        {
            return BuildDetailResults(selectedResult.ContextData as NetAttrs, false);
        }


        private List<Result> BuildDetailResults(NetAttrs attrs, bool returnResult = true)
        {
            var result = new List<Result>();
            var gatewayAddress = attrs.GatewayAddress;
            var ipAddress = attrs.IpAddress;

            result.Add(new Result()
            {
                IcoPath = "ipconfig-icon.png",
                Title = attrs.Name,
                SubTitle = "Copy Ip Address",
                Action = _ =>
                {
                    _context.API.CopyToClipboard(attrs.CopyIpString());
                    return true;
                }
            });


            if (gatewayAddress.Ipv4 != null)
            {
                result.Add(new Result()
                {
                    IcoPath = "ipconfig-icon.png",
                    Title = "Gateway Ipv4 Address",
                    SubTitle = gatewayAddress.Ipv4,
                    Action = _ =>
                    {
                        _context.API.CopyToClipboard(gatewayAddress.Ipv4);
                        return returnResult;
                    }
                });
            }

            if (ipAddress.Ipv4 != null)
            {
                result.Add(new Result()
                {
                    IcoPath = "ipconfig-icon.png",
                    Title = "Ipv4 Address",
                    SubTitle = ipAddress.Ipv4,
                    Action = _ =>
                    {
                        _context.API.CopyToClipboard(ipAddress.Ipv4);
                        return returnResult;
                    }
                });
            }

            result.Add(new Result()
            {
                IcoPath = "ipconfig-icon.png",
                Title = "Physical Address",
                SubTitle = BitConverter.ToString(attrs.PhysicalAddress),
                Action = _ =>
                {
                    _context.API.CopyToClipboard(BitConverter.ToString(attrs.PhysicalAddress));
                    return returnResult;
                }
            });

            if (gatewayAddress.Ipv6 != null)
            {
                result.Add(new Result()
                {
                    IcoPath = "ipconfig-icon.png",
                    Title = "Gateway Ipv6 Address",
                    SubTitle = gatewayAddress.Ipv6,
                    Action = _ =>
                    {
                        _context.API.CopyToClipboard(gatewayAddress.Ipv6);
                        return returnResult;
                    }
                });
            }

            if (ipAddress.Ipv6 != null)
            {
                result.Add(new Result()
                {
                    IcoPath = "ipconfig-icon.png",
                    Title = "Ipv6 Address",
                    SubTitle = ipAddress.Ipv6,
                    Action = _ =>
                    {
                        _context.API.CopyToClipboard(ipAddress.Ipv6);
                        return returnResult;
                    }
                });
            }

            return result;
        }

        private bool CopyAddress(IpAddrs addr)
        {
            if (addr.Ipv4 != null)
            {
                _context.API.CopyToClipboard(addr.Ipv4);
            }
            else if (addr.Ipv6 != null)
            {
                _context.API.CopyToClipboard(addr.Ipv6);
            }

            return true;
        }
    }
}