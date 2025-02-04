namespace Supercell.Laser.Logic.Message.Club
{
    using System.IO;
    using Supercell.Laser.Logic.Stream.Entry;

    public class AllianceStreamMessage : GameMessage
    {
        public AllianceStreamEntry[] Entries;

        public override void Encode()
        {
            if (Entries == null)
            {
                Stream.WriteVInt(-1);
                return;
            }

            Stream.WriteVInt(Entries.Length);
            foreach (var entry in Entries)
            {
                entry.Encode(Stream);
            }

            //Stream.WriteVInt(1);
            //Stream.WriteVInt(2);
            //Stream.WriteVLong(1);
            //Stream.WriteVLong(2);
            //Stream.WriteString("e");
            //Stream.WriteVInt((int)0);
            //Stream.WriteVInt((int)0);
            //Stream.WriteVInt(0);

            //{
            //    Stream.WriteString("sb");
            //}
        }

        public override int GetMessageType()
        {
            return 24311;
        }

        public override int GetServiceNodeType()
        {
            return 11;
        }
    }
}
