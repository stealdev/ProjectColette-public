namespace Supercell.Laser.Logic.Message.Home
{
    using Newtonsoft.Json.Linq;
    using System.Text;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Util;
    using Supercell.Laser.Titan.Debug;

    public class UpdatePlayerMapMessage : GameMessage
    {
        public int MapId;
        public byte[] MapData;
        public override void Decode()
        {
            MapId=(int) ByteStreamHelper.DecodeLogicLong(Stream);
            byte[] bytes = Stream.ReadBytes(Stream.ReadBytesLength(), 900000);
            MapData = bytes;
            ZLibHelper.DecompressInMySQLFormat(bytes, out byte[] output);
            Debugger.Print(Encoding.UTF8.GetString(output, 0, output.Length));
        }

        public override int GetMessageType()
        {
            return 12103;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
