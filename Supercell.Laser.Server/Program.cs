namespace Supercell.Laser.Server
{
    using Masuda.Net;
    using Masuda.Net.HelpMessage;
    using Masuda.Net.Models;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Handler;
    using Supercell.Laser.Server.Settings;
    using Supercell.Laser.Titan.Debug;
    using System.Drawing;
    using Supercell.Laser.Server.Networking.Session;

    static class Program
    {
        public const string SERVER_VERSION = "1.2";
        public const string BUILD_TYPE = "Beta";

        private static void Main(string[] args)
        {
            Console.Title = "Server";
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);


            Colorful.Console.WriteWithGradient(
                @"    _____                          )   ___                     
   (, /   )        ,              (__/_____)    /)             
    _/__ / __  ___    _  _ _/_      /       ___//  _ _/__/_  _ 
    /     / (_(_) /__(/_(__(__     /       (_)(/__(/_(__(___(/_
 ) /           .-/                (______)                     
(_/           (_/                                              " + "\n\n\n", Color.DarkRed, Color.Red, 10);

            Logger.Print("Server is now starting...");

            Logger.Init();
            Configuration.Instance = Configuration.LoadFromFile("config.json");

            Resources.InitDatabase();
            Resources.InitLogic();
            Resources.InitNetwork();

            Logger.Print("Server started!");
            ExitHandler.Init();
            CmdHandler.Start();

        }
    }
}