using Supercell.Laser.Logic.Helper;

namespace Supercell.Laser.Logic.Message.Ranking
{
    public class GetLeaderboardMessage : GameMessage
    {
        public bool IsRegional { get; set; }
        public int LeaderboardType { get; set; }
        public int HeroDataId;
        public override void Decode()
        {
            base.Decode();

            IsRegional = Stream.ReadBoolean();
            LeaderboardType = Stream.ReadVInt();
            HeroDataId = ByteStreamHelper.ReadDataReference(Stream);
        }

        public override int GetMessageType()
        {
            return 14403;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
