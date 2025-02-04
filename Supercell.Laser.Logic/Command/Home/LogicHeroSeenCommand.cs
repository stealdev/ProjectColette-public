namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Titan.DataStream;

    public class LogicHeroSeenCommand : Command
    {
        public override void Decode(ByteStream stream)
        {
            ByteStreamHelper.ReadDataReference(stream);
            stream.ReadInt();
        }
        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }
        public override int GetCommandType()
        {
            return 522;
        }
    }
}
