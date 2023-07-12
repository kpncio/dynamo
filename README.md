# Dynamer

*(Cloudflare DNS, .NET Core, C#)*

KPNC Technology: Dynamer: A dynamic DNS solution for Linux and Windows machines with Cloudflare...

# Configuration

- Dynamer's Config File (Config.json):
	- Email: Your Cloudflare related email address
  - ApiKey: Your Cloudflare API key
  - Domain: The domain (www.example.com)
  - IPv4: Whether to force IPv4 or not
  - Rate: Amount of seconds between checks

"Config.json" Example:
```json
{
  "Email": "user@example.com",
  "ApiKey": "1234567890abcdefghijklmnop",
  "Domain": "www.example.com",
  "IPv4": true,
  "Rate": 300
}
```

__*You only have to edit the service file when using Linux...*__

- Dynamer's Service File (dynamer.service):
	- User: The user that will launch Dynamer (with their perms)
  - WorkingDirectory: The directory of the Dynamer executable
  - ExecStart: The path of your dotnet executable and Dynamer library

"dynamer.service" Example:
```ini
[Unit]
Description=Dynamer for Cloudflare (.NET)
After=network.target

[Service]
User=albie
TimeoutStartSec=0
KillMode=process
WorkingDirectory=/home/albie/dynamer/
ExecStart=/usr/bin/dotnet /home/albie/dynamer/Dynamer.dll
Restart=always
RestartSec=10
Environment=Production

[Install]
WantedBy=multi-user.target
```

# Installation

__*For installation on a Linux (Ubuntu based) servers...*__

- Make sure you have correctly configured Dynamer (above) for you system

- Install dependencies and services:
	- Run "sudo bash install.sh"

- Now you may run "sudo service dynamer start"

__*For installation on a Windows (10/2019+) servers...*__

- Make sure you have correctly configured Dynamer (above) for you system

- Make sure you have .NET version 7.0 found [here](https://dotnet.microsoft.com/en-us/download/dotnet); however, you probably already have it

- To start it manually, you can simply double-click on the Dynamer.exe file or create a shortcut of it elsewhere

- If you would like the program to automatically start:
	- Create shortcut of "Dynamer.exe" by right-clicking and selecting "Create Shortcut"
	- You can rename this shortcut to whatever you want
	- Move the shortcut to the "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp" directory
