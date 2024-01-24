using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Flow.Launcher.Plugin.Ipconfig;

public delegate void OnAddressTextClick(string content);

public partial class IpPreviewPanel : UserControl
{
    private readonly OnAddressTextClick _clickFunc;

    public IpPreviewPanel(NetAttrs attrs, OnAddressTextClick clickFunc = null)
    {
        _clickFunc = clickFunc;

        InitializeComponent();
        Width = 360;
        Height = 360;

        if (attrs.IpAddress.Ipv4 != null)
            BindTextBlock(Ipv4, attrs.IpAddress.Ipv4);

        if (attrs.IpAddress.Ipv6 != null)
            BindTextBlock(Ipv6, attrs.IpAddress.Ipv6);

        if (attrs.PhysicalAddress != null)
            BindTextBlock(PhysicalAddress, BitConverter.ToString(attrs.PhysicalAddress));

        if (attrs.GatewayAddress.Ipv4 != null)
            BindTextBlock(GatewayIpv4, attrs.GatewayAddress.Ipv4);

        if (attrs.GatewayAddress.Ipv6 != null)
            BindTextBlock(GatewayIpv6, attrs.GatewayAddress.Ipv6);
    }


    private void BindTextBlock(TextBlock tb, string text)
    {
        tb.Cursor = Cursors.Hand;
        tb.Text = text;
        tb.ToolTip = "Click or Double Click to Copy";
        tb.MouseLeftButtonDown += (sender, args) =>
        {
            if (sender is not TextBlock textBlock) return;
            _clickFunc?.Invoke(textBlock.Text);
        };
        tb.Background = new SolidColorBrush(Colors.Gray);
        // tb.Width = 100;
    }
}