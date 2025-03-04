## Linux

#### this guide is made for ubuntu, when using different distros adjust the service and apt commands.

install both mysql and dotnet via your package manager
```bash
sudo apt install mysql-server dotnet-sdk-9.0
```
clone the repository (install git via `sudo apt install git` if needed)
```bash
git clone https://github.com/allbrawl/ProjectColette-public
```
cd into the the correct directory
```bash
cd ProjectColette-public/
```
start mysql
```bash
sudo service mysql start
```
mysql shell
```bash
sudo mysql
```
set mysql root password
```bash
ALTER USER 'root'@'localhost' IDENTIFIED WITH caching_sha2_password BY 'YOUR_PASSWORD';
```
create a new mysql database
```bash
CREATE DATABASE databasename;
```
exit mysql
```bash
exit;
```
import database.sql
```bash
sudo mysql -u root -p databasename < database.sql
```
cd into the project
```bash
cd Supercell.Laser.Server
```
compile the project
```bash
dotnet publish
```
now cd into the path the the compiled dll
```bash
cd bin/Release/net9.0/
```
edit the config file with your prefered text editor, for example vim:
```bash
vim config.json
```
change the `mysql_password` to the password you set, and the `mysql_database` to the database you created. also edit `BotToken` and `ChannelId` if you want to use the discord bot

finally run the server
```bash
dotnet Supercell.Laser.Server.dll
```
