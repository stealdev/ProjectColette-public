namespace Supercell.Laser.Logic.Message.Team
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Helper;

    public class TeamSetLocationMessage : GameMessage
    {
        public int locid;
        public List<int> CustomModifiers;
        public TeamSetLocationMessage()
        {
            CustomModifiers = new();
        }
        public override int GetMessageType()
        {
            return 14363;
        }
        public override void Decode()
        {
            locid = ByteStreamHelper.ReadDataReference(Stream);
            CustomModifiers = ByteStreamHelper.ReadIntList(Stream, 30);
        }
        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
