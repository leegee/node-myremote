; Set up installer details
[Setup]
AppName=MyRemote
AppVersion=0.1
DefaultDirName={commonpf}\MyRemote
PrivilegesRequired=admin
OutputBaseFilename=Install-MyRemote
; SetupIconFile=systray-icon.ico ; This is the icon used for the installer
OutputDir=Output

; Files to be installed
[Files]
Source: "bin\Release\net4.8.1-windows\MyRemote.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "systray-icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "index.html"; DestDir: "{app}"; Flags: ignoreversion
Source: ".env"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net4.8.1-windows\*.dll"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{userprograms}\MyRemote"; Filename: "{app}\MyRemote.exe"; WorkingDir: "{app}"; IconFilename: "{app}\systray-icon.ico"
Name: "{userdesktop}\MyRemote"; Filename: "{app}\MyRemote.exe"; WorkingDir: "{app}"; IconFilename: "{app}\systray-icon.ico"

; Custom actions to run after installing files
[Run]
; Add URL ACLs
Filename: "netsh"; Parameters: "http add urlacl url=http://+:8223/ user=Everyone"; StatusMsg: "Configuring URL ACL for 8223..."; Flags: runhidden
Filename: "netsh"; Parameters: "http add urlacl url=http://+:8224/ user=Everyone"; StatusMsg: "Configuring URL ACL for 8224..."; Flags: runhidden

; Add Firewall Rules
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""MyRemote"" protocol=TCP dir=in localport=8223 action=allow"; StatusMsg: "Configuring firewall rule for port 8223..."; Flags: runhidden
Filename: "netsh"; Parameters: "advfirewall firewall add rule name=""MyRemote"" protocol=TCP dir=in localport=8224 action=allow"; StatusMsg: "Configuring firewall rule for port 8224..."; Flags: runhidden

