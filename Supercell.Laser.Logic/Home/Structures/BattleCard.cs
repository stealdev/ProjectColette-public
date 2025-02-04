namespace Supercell.Laser.Logic.Home.Structures
{
    using Supercell.Laser.Logic.Avatar;
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    [JsonObject(MemberSerialization.OptOut)]
    public class BattleCard
    {
        public int Thumbnail1;
        public int Thumbnail2;
        public int Emote;
        public int Title;

        public BattleCard()
        {
            ;
        }

        public void Encode(ByteStream stream)
        {
            ByteStreamHelper.WriteDataReference(stream, null);
            ByteStreamHelper.WriteDataReference(stream, Thumbnail1);
            ByteStreamHelper.WriteDataReference(stream, Thumbnail2);
            ByteStreamHelper.WriteDataReference(stream, Emote);
            ByteStreamHelper.WriteDataReference(stream, Title);
            stream.WriteBoolean(Thumbnail1 == 0);
            stream.WriteBoolean(Thumbnail2 == 0);
            stream.WriteBoolean(Emote == 0);
            stream.WriteBoolean(Title == 0);
        }

    }
}
