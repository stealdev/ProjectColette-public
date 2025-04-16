namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicSelectCharacterCommand : Command
    {
        public int CharacterInstanceId;
        public int Index;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            CharacterInstanceId = stream.ReadVInt();
            Index=stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            int globalId = GlobalId.CreateGlobalId(16, CharacterInstanceId);
            if (homeMode.Avatar.HasHero(globalId))
            {
                homeMode.Home.CharacterIds[Index] = globalId;
                homeMode.CharacterChanged.Invoke(globalId);
                Hero hero = homeMode.Avatar.GetHero(globalId);
                //homeMode.Home.CharacterId = GlobalId.CreateGlobalId(16, 38);
                //homeMode.CharacterChanged.Invoke(GlobalId.CreateGlobalId(16, 38));
                return 0;
            }


            return -1;
        }

        public override int GetCommandType()
        {
            return 525;
        }
    }
}
