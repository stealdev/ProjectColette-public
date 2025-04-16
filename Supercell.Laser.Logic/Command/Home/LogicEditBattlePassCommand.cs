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

    public class LogicEditBattlePassCommand : Command
    {
        public int CharacterId;
        public int VanityId;
        public bool unk1;
        public int Index;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            CharacterId=ByteStreamHelper.ReadDataReference(stream);
            VanityId=ByteStreamHelper.ReadDataReference(stream);
            unk1 = stream.ReadBoolean();
            Index = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            switch (Index)
            {
                case 0:
                    homeMode.Home.DefaultBattleCard.Thumbnail1 = VanityId;
                    break;
                case 1:
                    homeMode.Home.DefaultBattleCard.Thumbnail2 = VanityId;
                    break;
                case 5:
                    homeMode.Home.DefaultBattleCard.Emote = VanityId;
                    break;
                case 10:
                    homeMode.Home.DefaultBattleCard.Title = VanityId;
                    break;    
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 568;
        }
    }
}
