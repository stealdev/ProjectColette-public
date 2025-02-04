namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicGatchaCommand : Command
    {
        public int BoxIndex;

        public LogicGatchaCommand() : base()
        {
            ;
        }

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            BoxIndex = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 500;
        }
    }
}
