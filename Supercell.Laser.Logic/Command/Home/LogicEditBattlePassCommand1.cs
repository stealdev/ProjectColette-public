namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicEditBattlePassCommand1 : Command
    {
        public bool unk1;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            unk1 = stream.ReadBoolean();
        }

        public override int Execute(HomeMode homeMode)
        {
            return 0;
        }

        public override int GetCommandType()
        {
            return 567;
        }
    }
}
