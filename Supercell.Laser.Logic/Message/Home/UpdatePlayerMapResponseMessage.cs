namespace Supercell.Laser.Logic.Message.Home
{
    using Newtonsoft.Json.Linq;
    using System.Text;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Util;
    using Supercell.Laser.Titan.Debug;

    public class UpdatePlayerMapResponseMessage : GameMessage
    {
        public int MapId;
        public int ErrorCode;
        public override void Encode()
        {
            Stream.WriteVInt(ErrorCode);
            ByteStreamHelper.EncodeLogicLong(Stream, MapId);
        }

        public override int GetMessageType()
        {
            return 22103;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
