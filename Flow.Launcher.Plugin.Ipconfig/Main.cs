using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Flow.Launcher.Plugin.Ipconfig
{
    public class Ipconfig : IPlugin, IContextMenu, IPluginI18n
    {
        public const string IconPath = "Images\\IpconfigIcon.png";

        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            var interfaces = IpConfigUtil.GetNetworkInterfaces();
            var search = query.Search;


            var resultList = new List<Result>();
            foreach (var attrs in interfaces)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    if (!IsMatch(search, attrs.Name))
                        continue;
                }

                var result = new Result
                {
                    IcoPath = IconPath,
                    Title = attrs.Name,
                    SubTitle = attrs.IpAddress.ToString(),
                    Action = _ => CopyAddress(attrs.IpAddress),
                    AutoCompleteText = $"{query.ActionKeyword} {attrs.Name}",
                    PreviewPanel = new Lazy<UserControl>(() =>
                    {
                        return new IpPreviewPanel(attrs, content => _context.API.CopyToClipboard(content));
                    }),
                    ContextData = attrs
                };

                resultList.Add(result);
            }

            return resultList;
        }

        private static bool IsMatch(string searchKey, string name)
        {
            return string.Equals(searchKey, name, StringComparison.OrdinalIgnoreCase) ||
                   name.StartsWith(searchKey, StringComparison.OrdinalIgnoreCase);
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
                IcoPath = IconPath,
                Title = attrs.Name,
                SubTitle = _context.API.GetTranslation("ipconfig_plugin_copy_ip"),
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
                    IcoPath = IconPath,
                    Title = _context.API.GetTranslation("ipconfig_plugin_gateway_ipv4_address"),
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
                    IcoPath = IconPath,
                    Title = _context.API.GetTranslation("ipconfig_plugin_ipv4_address"),
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
                IcoPath = IconPath,
                Title = _context.API.GetTranslation("ipconfig_plugin_physical_address"),
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
                    IcoPath = IconPath,
                    Title = _context.API.GetTranslation("ipconfig_plugin_gateway_ipv6_address"),
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
                    IcoPath = IconPath,
                    Title = _context.API.GetTranslation("ipconfig_plugin_ipv6_address"),
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

        public string GetTranslatedPluginTitle()
        {
            return _context.API.GetTranslation("ipconfig_plugin_title");
        }

        public string GetTranslatedPluginDescription()
        {
            return _context.API.GetTranslation("ipconfig_plugin_description");
        }
    }
}