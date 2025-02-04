namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Helper;

    public class TeamSetPlayerMapMessage : GameMessage
    {
        public long mapid;
        public override int GetMessageType()
        {
            return 12110;
        }
        public override void Decode()
        {
            mapid = Stream.ReadVLong();
        }
        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
