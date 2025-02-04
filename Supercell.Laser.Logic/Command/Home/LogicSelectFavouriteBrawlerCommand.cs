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

    public class LogicSelectFavouriteBrawlerCommand : Command
    {
        public int CharacterId;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            CharacterId=ByteStreamHelper.ReadDataReference(stream);
        }

        public override int Execute(HomeMode homeMode)
        {
            if (homeMode.Avatar.HasHero(CharacterId))
            {
                homeMode.Home.FavouriteCharacter = CharacterId;
                return 0;
            }


            return -1;
        }

        public override int GetCommandType()
        {
            return 570;
        }
    }
}
