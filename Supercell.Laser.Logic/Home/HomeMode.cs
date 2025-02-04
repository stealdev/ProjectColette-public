namespace Supercell.Laser.Logic.Home
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public class HomeMode
    {
        public readonly LogicGameListener GameListener;

        public ClientHome Home;
        public ClientAvatar Avatar;

        public Action<int> CharacterChanged;

        public HomeMode(ClientHome home, ClientAvatar avatar, LogicGameListener gameListener)
        {
            Home = home;
            Avatar = avatar;

            Home.HomeMode = this;
            Avatar.HomeMode = this;

            GameListener = gameListener;
        }

        public static HomeMode LoadHomeState(LogicGameListener gameListener, ClientHome home, ClientAvatar avatar, EventData[] events)
        {
            home.Events = events;

            HomeMode homeMode = new HomeMode(home, avatar, gameListener);
            homeMode.Enter(DateTime.UtcNow);

            return homeMode;
        }

        public void Enter(DateTime dateTime)
        {
            Home.HomeVisited();
        }

        public void ClientTurnReceived(int tick, int checksum, List<Command> commands)
        {
            foreach (Command command in commands)
            {
                if (command.Execute(this) != 0)
                {
                    OutOfSyncMessage outOfSync = new OutOfSyncMessage();
                    GameListener.SendMessage(outOfSync);
                }
            }
            Home.Tick();
        }
        
        public CardData GetDefaultMetaForHero(Hero character,int MetaType)
        {
            foreach (CardData carddata in DataTables.Get(DataType.Card).GetDatas())
            {
                if (carddata.Target == character.CharacterData.Name && carddata.MetaType == MetaType)
                {
                    return carddata;
                }
            }
            return null;
        }
    }
}
