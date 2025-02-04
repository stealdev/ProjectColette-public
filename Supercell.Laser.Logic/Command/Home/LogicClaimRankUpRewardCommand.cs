namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicClaimRankUpRewardCommand : Command
    {
        public int MilestoneId { get; set; }
        public int UnknownDataId { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            MilestoneId = stream.ReadVInt();
            UnknownDataId = ByteStreamHelper.ReadDataReference(stream);
            Unk2 = stream.ReadVInt();
            //Unk3 = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 517;
        }
    }
}
