namespace Supercell.Laser.Server.Networking
{
    using Masuda.Net;
    using Supercell.Laser.Logic.Message.Account;
    using Supercell.Laser.Logic.Message.Team.Stream;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Server.Networking.Session;

    public static class Connections
    {
        public static int Count => ActiveConnections.Count;
        public static MasudaBot bot;
        private static List<Connection> ActiveConnections;
        private static Thread Thread;
        private static long SecondsGone;

        public static void Init()
        {
            ActiveConnections = new List<Connection>();
            bot = new MasudaBot(102038674, "ElazeGW3722wbRMI9StXcSJbvsRLitBm", "ElazeGW3722wbRMI9StXcSJbvsRLitBm", BotType.Public).LogTo(null);
            Thread = new Thread(Update);
            Thread.Start();
            SecondsGone = 0;
        }

        private static void Update()
        {
            while (true)
            {
                foreach (Connection connection in ActiveConnections.ToArray())
                {
                    if (connection.Avatar != null && !Sessions.IsSessionActive(connection.Avatar.AccountId))
                    {
                        connection.Close();
                        ActiveConnections.Remove(connection);
                    }
                    if (!connection.MessageManager.IsAlive())
                    {
                        //DisconnectedMessage d = new DisconnectedMessage() { Reason = 0 };
                        //connection.Send(d);
                        if (connection.MessageManager.HomeMode != null)
                        {
                            Sessions.Remove(connection.Avatar.AccountId);
                        }
                        connection.Close();
                        ActiveConnections.Remove(connection);
                    }
                }
                if (SecondsGone % 30 == 0) bot.ModifyChannelAsync("216185176", "服务器在线人数：" + Count, 0, 4, "141954264");
                //if (SecondsGone % 5 == 0) Sessions.SendGlobalMessage(-1, "System", "test");
                SecondsGone++;
                Thread.Sleep(1000);
            }
        }

        public static void AddConnection(Connection connection)
        {
            ActiveConnections.Add(connection);
        }
    }
}
