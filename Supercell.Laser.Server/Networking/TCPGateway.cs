namespace Supercell.Laser.Server.Networking
{
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking.Session;
    using System.Net;
    using System.Net.Sockets;

    public static class ContentServer
    {
        private static HttpListener HttpListener;
        public static void Init(string host, int port)
        {
            HttpListener = new HttpListener();
            //HttpListener.
        }
    }
    public static class TCPGateway
    {
        private static List<Connection> ActiveConnections;

        private static Socket Socket;
        private static Thread Thread;

        private static ManualResetEvent AcceptEvent;
        public static void Init(string host, int port)
        {
            ActiveConnections = new List<Connection>();

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            Socket.Listen(100);

            AcceptEvent = new ManualResetEvent(false);

            Thread = new Thread(TCPGateway.Update);
            Thread.Start();

            Logger.Print($"TCP Server started at {host}:{port}");
        }

        private static void Update()
        {
            while (true)
            {
                AcceptEvent.Reset();
                Socket.BeginAccept(new AsyncCallback(OnAccept), null);
                AcceptEvent.WaitOne();
            }
        }

        private static void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket client = Socket.EndAccept(ar);
                Connection connection = new Connection(client);
                ActiveConnections.Add(connection);
                Logger.Print("New connection!");
                Connections.AddConnection(connection);
                client.BeginReceive(connection.ReadBuffer, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), connection);
            } 
            catch (Exception)
            {
                ;
            }

            AcceptEvent.Set();
        }

        private static void OnReceive(IAsyncResult ar)
        {
            try
            {
                Connection connection = (Connection)ar.AsyncState;
                if (connection == null) return;

                try
                {
                    int r = connection.Socket.EndReceive(ar);
                    if (r <= 0)
                    {
                        Logger.Print("client disconnected.");
                        ActiveConnections.Remove(connection);
                        //TeamEntry team = Teams.Get(connection.MessageManager.HomeMode.Avatar.TeamId);

                        //if (team == null)
                        //{
                        //    Logger.Print("TeamLeave - Team is NULL!");
                        //    connection.MessageManager.HomeMode.Avatar.TeamId = -1;
                        //    connection.Send(new TeamLeftMessage());
                        //    return;
                        //}

                        //TeamMember entry = team.GetMember(connection.MessageManager.HomeMode.Avatar.AccountId);

                        //if (entry == null) return;
                        //connection.MessageManager.HomeMode.Avatar.TeamId = -1;

                        //team.Members.Remove(entry);

                        //connection.Send(new TeamLeftMessage());
                        //team.TeamUpdated();

                        //if (team.Members.Count == 0)
                        //{
                        //    Teams.Remove(team.Id);
                        //}
                        if (connection.MessageManager.HomeMode != null)
                        {
                            Sessions.Remove(connection.Avatar.AccountId);
                            BattleMode battle = Battles.Get(connection.Home.HomeMode.Avatar.BattleId);
                            if (battle != null)
                            {
                                BattlePlayer player = battle.GetPlayerBySessionId(connection.UdpSessionId);
                                if (player != null) battle.RemovePlayer(player);
                            }
                        }

                        connection.Close();
                        return;
                    }

                    connection.Memory.Write(connection.ReadBuffer, 0, r);
                    if (connection.Messaging.OnReceive() != 0)
                    {
                        ActiveConnections.Remove(connection);
                        if (connection.MessageManager.HomeMode != null)
                        {
                            Sessions.Remove(connection.Avatar.AccountId);
                        }
                        if (connection.Home != null)
                        {
                            BattleMode battle = Battles.Get(connection.Home.HomeMode.Avatar.BattleId);
                            if (battle != null)
                            {
                                BattlePlayer player = battle.GetPlayerBySessionId(connection.UdpSessionId);
                                if (player != null) battle.RemovePlayer(player);
                            }
                        }
                        connection.Close();
                        Logger.Print("client disconnected.");
                        return;
                    }
                    connection.Socket.BeginReceive(connection.ReadBuffer, 0, 1024, SocketFlags.None, new AsyncCallback(OnReceive), connection);
                }
                catch (SocketException)
                {
                    ActiveConnections.Remove(connection);
                    if (connection.MessageManager.HomeMode != null)
                    {
                        Sessions.Remove(connection.Avatar.AccountId);
                    }
                    BattleMode battle = Battles.Get(connection.Home.HomeMode.Avatar.BattleId);
                    if (battle != null)
                    {
                        BattlePlayer player = battle.GetPlayerBySessionId(connection.UdpSessionId);
                        if (player != null) battle.RemovePlayer(player);
                    }
                    connection.Close();
                    Logger.Print("client disconnected.");
                }
                catch (Exception exception)
                {
                    connection.Close();
                    //Logger.Print("Unhandled exception: " + exception + ", trace: " + exception.StackTrace);
                }
            }
            catch (Exception exception)
            {
                Logger.Print("Unhandled exception: " + exception + ", trace: " + exception.StackTrace);
            }
        }

        public static void OnSend(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndSend(ar);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
