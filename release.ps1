dotnet publish Flow.Launcher.Plugin.Ipconfig -c Release -r win-x64 --no-self-contained
Copy-Item -Path Flow.Launcher.Plugin.Ipconfig/ipconfig-icon.png -Destination Flow.Launcher.Plugin.Ipconfig/bin/Release/win-x64/publish/ipconfig-icon.png
Compress-Archive -LiteralPath Flow.Launcher.Plugin.Ipconfig/bin/Release/win-x64/publish -DestinationPath Flow.Launcher.Plugin.Ipconfig/bin/Ipconfig.zip -Force