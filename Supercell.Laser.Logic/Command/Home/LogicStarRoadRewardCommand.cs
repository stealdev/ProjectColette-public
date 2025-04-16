namespace Supercell.Laser.Logic.Command.Home
{
    using Masuda.Net.Models;
    using Supercell.Laser.Logic.Command.Avatar;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicStarRoadRewardCommand : Command
    {
        readonly List<int> rare = new List<int> { 1, 2, 3, 6, 8, 10, 13, 24 };
        readonly List<int> super_rare = new List<int> { 7, 9, 18, 19, 22, 25, 27, 34, 61, 4 };
        readonly List<int> epic = new List<int> { 14, 15, 16, 20, 26, 29, 30, 36, 43, 45, 48, 50, 58, 69 };
        readonly List<int> mythic = new List<int> { 11, 17, 21, 35, 31, 32, 37, 42, 47, 64, 67, 71, 73 };
        readonly List<int> legendary = new List<int> { 5, 12, 23, 28, 40, 52, 63 };


        private int BrawlerID;

        public override void Decode(ByteStream stream)
        {
            Console.WriteLine(stream.ReadVInt());
            Console.WriteLine(stream.ReadVInt());
            Console.WriteLine(stream.ReadVInt());
            Console.WriteLine(stream.ReadVInt());
            Console.WriteLine(stream.ReadVInt());
            BrawlerID = stream.ReadVInt();
            //BrawlerID = stream.ReadVInt();

        }

        public override int Execute(HomeMode homeMode)
        {

            //Console.WriteLine(BrawlerID);

            switch (BrawlerID)
            {
                case 1:
                    homeMode.Home.RecruitBrawlerCard = 4;
                    homeMode.Home.RecruitBrawler = 2;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 2);
                    break;
                case 2:
                    homeMode.Home.RecruitBrawlerCard = 8;
                    homeMode.Home.RecruitBrawler = 3;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 3);
                    break;
                case 3:
                    homeMode.Home.RecruitBrawlerCard = 12;
                    homeMode.Home.RecruitBrawler = 4;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 4);
                    break;
                case 4:
                    homeMode.Home.RecruitBrawlerCard = 16;
                    homeMode.Home.RecruitBrawler = 5;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 5);
                    break;
                case 5:
                    homeMode.Home.RecruitBrawlerCard = 20;
                    homeMode.Home.RecruitBrawler = 6;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 6);
                    break;
                case 6:
                    homeMode.Home.RecruitBrawlerCard = 24;
                    homeMode.Home.RecruitBrawler = 7;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 7);
                    break;
                case 7:
                    homeMode.Home.RecruitBrawlerCard = 28;
                    homeMode.Home.RecruitBrawler = 8;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 8);
                    break;
                case 8:
                    homeMode.Home.RecruitBrawlerCard = 32;
                    homeMode.Home.RecruitBrawler = 9;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 9);
                    break;
                case 9:
                    homeMode.Home.RecruitBrawlerCard = 36;
                    homeMode.Home.RecruitBrawler = 10;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 10);
                    break;
                case 10:
                    homeMode.Home.RecruitBrawlerCard = 40;
                    homeMode.Home.RecruitBrawler = 11;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 11);
                    break;
                case 11:
                    homeMode.Home.RecruitBrawlerCard = 44;
                    homeMode.Home.RecruitBrawler = 12;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 12);
                    break;
                case 12:
                    homeMode.Home.RecruitBrawlerCard = 48;
                    homeMode.Home.RecruitBrawler = 13;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 13);
                    break;
                case 13:
                    homeMode.Home.RecruitBrawlerCard = 52;
                    homeMode.Home.RecruitBrawler = 14;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 14);
                    break;
                case 14:
                    homeMode.Home.RecruitBrawlerCard = 56;
                    homeMode.Home.RecruitBrawler = 15;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 15);
                    break;
                case 15:
                    homeMode.Home.RecruitBrawlerCard = 60;
                    homeMode.Home.RecruitBrawler = 16;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 16);
                    break;
                case 16:
                    homeMode.Home.RecruitBrawlerCard = 64;
                    homeMode.Home.RecruitBrawler = 17;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 17);
                    break;
                case 17:
                    homeMode.Home.RecruitBrawlerCard = 68;
                    homeMode.Home.RecruitBrawler = 18;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 18);
                    break;
                case 18:
                    homeMode.Home.RecruitBrawlerCard = 72;
                    homeMode.Home.RecruitBrawler = 19;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 19);
                    break;
                case 19:
                    homeMode.Home.RecruitBrawlerCard = 95;
                    homeMode.Home.RecruitBrawler = 20;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 20);
                    break;
                case 20:
                    homeMode.Home.RecruitBrawlerCard = 100;
                    homeMode.Home.RecruitBrawler = 21;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 21);
                    break;
                case 21:
                    homeMode.Home.RecruitBrawlerCard = 105;
                    homeMode.Home.RecruitBrawler = 22;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 22);
                    break;
                case 22:
                    homeMode.Home.RecruitBrawlerCard = 110;
                    homeMode.Home.RecruitBrawler = 23;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 23);
                    break;
                case 23:
                    homeMode.Home.RecruitBrawlerCard = 115;
                    homeMode.Home.RecruitBrawler = 24;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 24);
                    break;
                case 24:
                    homeMode.Home.RecruitBrawlerCard = 120;
                    homeMode.Home.RecruitBrawler = 25;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 25);
                    break;
                case 25:
                    homeMode.Home.RecruitBrawlerCard = 125;
                    homeMode.Home.RecruitBrawler = 26;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 26);
                    break;
                case 26:
                    homeMode.Home.RecruitBrawlerCard = 130;
                    homeMode.Home.RecruitBrawler = 27;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 27);
                    break;
                case 27:
                    homeMode.Home.RecruitBrawlerCard = 177;
                    homeMode.Home.RecruitBrawler = 28;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 28);
                    break;
                case 28:
                    homeMode.Home.RecruitBrawlerCard = 182;
                    homeMode.Home.RecruitBrawler = 29;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 29);
                    break;
                case 29:
                    homeMode.Home.RecruitBrawlerCard = 188;
                    homeMode.Home.RecruitBrawler = 30;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 30);
                    break;
                case 30:
                    homeMode.Home.RecruitBrawlerCard = 194;
                    homeMode.Home.RecruitBrawler = 31;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 31);
                    break;
                case 31:
                    homeMode.Home.RecruitBrawlerCard = 200;
                    homeMode.Home.RecruitBrawler = 32;
                    homeMode.Home.Brawlers.RemoveAll(x => x == 32);
                    break;
                default:
                    break;
            }

            if (rare.Contains(BrawlerID))
                homeMode.Home.RecruitTokens = homeMode.Home.RecruitTokens -= 160;
            else if (super_rare.Contains(BrawlerID))
                homeMode.Home.RecruitTokens = homeMode.Home.RecruitTokens -= 160;
            else if (epic.Contains(BrawlerID))
                homeMode.Home.RecruitTokens = homeMode.Home.RecruitTokens -= 160;
            else if (mythic.Contains(BrawlerID))
                homeMode.Home.RecruitTokens = homeMode.Home.RecruitTokens -= 160;
            else if (legendary.Contains(BrawlerID))
                homeMode.Home.RecruitTokens = homeMode.Home.RecruitTokens -= 160;
            else
                homeMode.Home.RecruitTokens = 0;

            if (homeMode.Home.RecruitTokens < 0) homeMode.Home.RecruitTokens = 0;

            LogicStarRoadRefresh command = new LogicStarRoadRefresh();
            command.Execute(homeMode);
            AvailableServerCommandMessage serverCommandMessage = new AvailableServerCommandMessage();
            serverCommandMessage.Command = command;
            homeMode.GameListener.SendMessage(serverCommandMessage);

            if (!homeMode.Avatar.HasHero(GlobalId.CreateGlobalId(16, BrawlerID)) ) { return -1; }
            else
            {
                homeMode.Avatar.UnlockHero(GlobalId.CreateGlobalId(16, BrawlerID));
                return 0;
            }

        }

        public override int GetCommandType()
        {
            return 560;
        }
    }
}
