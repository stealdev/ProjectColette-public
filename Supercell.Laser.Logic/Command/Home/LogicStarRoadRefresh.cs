namespace Supercell.Laser.Logic.Command.Avatar
{
    using System.Collections.Generic;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicStarRoadRefresh : Command
    {
        readonly List<int> rare = new List<int> { 1, 2, 3, 6, 8, 10, 13, 24 };
        readonly List<int> super_rare = new List<int> { 7, 9, 18, 19, 22, 25, 27, 34, 61, 4 };
        readonly List<int> epic = new List<int> { 14, 15, 16, 20, 26, 29, 30, 36, 43, 45, 48, 50, 58, 69 };
        readonly List<int> mythic = new List<int> { 11, 17, 21, 35, 31, 32, 37, 42, 47, 64, 67, 71, 73 };
        readonly List<int> legendary = new List<int> { 5, 12, 23, 28, 40, 52, 63 };

        private int RecruitBrawler;
        private int RecruitTokens;

        public override void Encode(ByteStream stream)
        {

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(1);
            stream.WriteVInt(1);
            stream.WriteVInt(1);
            stream.WriteVInt(1);


            stream.WriteBoolean(true);

            stream.WriteVInt(1);

            stream.WriteVInt(16);
            stream.WriteVInt(RecruitBrawler);

            if (rare.Contains(RecruitBrawler))
                stream.WriteVInt(160);
            else if (super_rare.Contains(RecruitBrawler))
                stream.WriteVInt(430);
            else if (epic.Contains(RecruitBrawler))
                stream.WriteVInt(925);
            else if (mythic.Contains(RecruitBrawler))
                stream.WriteVInt(1900);
            else if (legendary.Contains(RecruitBrawler))
                stream.WriteVInt(3800);
            else
                stream.WriteVInt(0);


            if (rare.Contains(RecruitBrawler))
                stream.WriteVInt(29);
            else if (super_rare.Contains(RecruitBrawler))
                stream.WriteVInt(79);
            else if (epic.Contains(RecruitBrawler))
                stream.WriteVInt(169);
            else if (mythic.Contains(RecruitBrawler))
                stream.WriteVInt(359);
            else if (legendary.Contains(RecruitBrawler))
                stream.WriteVInt(699);
            else
                stream.WriteVInt(0);


            stream.WriteVInt(0);
            stream.WriteVInt(RecruitTokens);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(1);

            stream.WriteDataReference(16, RecruitBrawler);

            stream.WriteVInt(1);

            stream.WriteVInt(1);
        }

        public override int Execute(HomeMode homeMode)
        {
            RecruitBrawler = homeMode.Home.RecruitBrawler;
            RecruitTokens = homeMode.Home.RecruitTokens;
            return 0;
        }

        public override int GetCommandType()
        {
            return 202;
        }
    }
}
