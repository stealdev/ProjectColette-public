namespace Supercell.Laser.Logic.Message.Home
{
    using Newtonsoft.Json.Linq;
    using System.Text;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Util;
    using Supercell.Laser.Titan.Debug;

    public class DeletePlayerMapMessage : GameMessage
    {
        public int MapId;
        public override void Decode()
        {
            MapId=(int) ByteStreamHelper.DecodeLogicLong(Stream);
        }

        public override int GetMessageType()
        {
            return 12101;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
