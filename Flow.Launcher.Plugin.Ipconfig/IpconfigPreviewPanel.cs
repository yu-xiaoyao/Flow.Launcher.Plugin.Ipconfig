using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Flow.Launcher.Plugin.Ipconfig;

public delegate void OnItemTextClick(string content);

public class IpconfigPreviewPanel : UserControl
{
    private readonly NetAttrs _attrs;
    private readonly OnItemTextClick _clickFunc;

    public IpconfigPreviewPanel(NetAttrs attrs, OnItemTextClick clickFunc = null)
    {
        _attrs = attrs;
        _clickFunc = clickFunc;
        // Loaded += On_Panel_Loaded;
        AddIpconfigPanel();
    }

    private void On_Panel_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void AddIpconfigPanel()
    {
        var basePanel = BuildStackPanel();

        AddChild(basePanel);
    }

    private StackPanel BuildStackPanel()
    {
        var panel = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Center,
        };

        if (_attrs.IpAddress.Ipv4 != null)
            panel.Children.Add(BuildItemPanel("Ipv4: ", _attrs.IpAddress.Ipv4));

        if (_attrs.IpAddress.Ipv6 != null)
            panel.Children.Add(BuildItemPanel("Ipv6: ", _attrs.IpAddress.Ipv6));

        if (_attrs.GatewayAddress.Ipv4 != null)
            panel.Children.Add(BuildItemPanel("Gateway Ipv4: ", _attrs.GatewayAddress.Ipv4));

        if (_attrs.GatewayAddress.Ipv6 != null)
            panel.Children.Add(BuildItemPanel("Gateway Ipv6: ", _attrs.GatewayAddress.Ipv6));

        panel.Children.Add(BuildItemPanel("Physical Address: ", BitConverter.ToString(_attrs.PhysicalAddress)));

        return panel;
    }


    private Panel BuildItemPanel(string name, string value)
    {
        var panel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(8, 8, 8, 0),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        var nameLabel = new Label()
        {
            FontSize = 10,
            HorizontalAlignment = HorizontalAlignment.Left,
            Content = name
        };

        var valueLabel = new Label()
        {
            FontSize = 14,
            Margin = new Thickness(5, 0, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = new SolidColorBrush(Colors.Gray),
            Content = value,
            Cursor = Cursors.Hand,
            ToolTip = "click or double click to Copy"
        };
        valueLabel.MouseLeftButtonDown += (sender, args) => { _clickFunc?.Invoke(value); };
        valueLabel.MouseDoubleClick += (sender, args) => { _clickFunc?.Invoke(value); };

        panel.Children.Add(nameLabel);
        panel.Children.Add(valueLabel);
        return panel;
    }
}