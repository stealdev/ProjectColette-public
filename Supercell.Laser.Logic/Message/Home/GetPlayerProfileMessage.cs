using Supercell.Laser.Logic.Command.Home;
using Supercell.Laser.Logic.Helper;

namespace Supercell.Laser.Logic.Message.Home
{
    public class GetPlayerProfileMessage : GameMessage
    {
        public long AccountId;

        public GetPlayerProfileMessage() : base()
        {
            ;
        }

        public override void Decode()
        {
            if (Stream.ReadBoolean())
            {
                Stream.ReadVInt();
                Stream.ReadLong();
                for (int i = Stream.ReadVInt(); i > 0; i--)
                {
                    ByteStreamHelper.ReadDataReference(Stream);
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                    Stream.ReadVInt();
                }
                Stream.ReadVInt();
                Stream.ReadString();
                Stream.ReadVInt();
                Stream.ReadVInt();
                Stream.ReadVInt();
                Stream.ReadVInt();
            }
            Stream.ReadVInt();
            AccountId = Stream.ReadLong();
        }

        public override int GetMessageType()
        {
            return 15081;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
