## Windows

download mysql from [here](https://dev.mysql.com/downloads/installer/) (I suggest web-community, but it's just the installer type)

- no need for account! click "No thanks, just start my download."

run the installer and select "full", then just go through the installer by always pressing execute or next

download the  server from [here](https://github.com/allbrawl/ProjectColette-public/archive/refs/heads/main.zip)

download and install dotnet from [the microsoft website](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

unzip the file

open mysql workbench and click on "Local instance MySQL80"

click "data import/restore", then "import self-contained file", select the database.sql file. then click  "new..." give your database a name and click "start import"

open a terminal in the directory Supercell.Laser.Server

compile the project
```powershell
dotnet publish
```

in /bin/Release/net9.0 edit the config.json change the `mysql_password` to the password you set, and the `mysql_database` to the database you created. also edit `BotToken` and `ChannelId` if you want to use the discord bot

cd to the binary
cd bin\Release\net9.0

run  the server
```powershell
dotnet Supercell.Laser.Server.dll
```
