namespace Supercell.Laser.Logic.Message.Friends
{
    using System.IO;
    using System.Threading;
    using System.Xml.Linq;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Listener;

    public class FriendListMessage : GameMessage
    {
        public Friend[] Friends;

        public override void Encode()
        {
            Stream.WriteInt(0);
            Stream.WriteBoolean(true);
            Stream.WriteBoolean(false);//FriendRequestBlocked
            Stream.WriteInt(Friends.Length);

            foreach (Friend friend in Friends)
            {
                friend.Encode(Stream);
            }
            //Stream.WriteLong(1);

            //Stream.WriteString(null);
            //Stream.WriteString(null);
            //Stream.WriteString(null);
            //Stream.WriteString(null);
            //Stream.WriteString(null);
            //Stream.WriteString(null);

            //Stream.WriteInt(506);
            //Stream.WriteInt(4);
            //Stream.WriteInt(0);
            //Stream.WriteInt(0);
            //Stream.WriteInt(0);

            //Stream.WriteBoolean(false); // Alliance entry

            //Stream.WriteString(null);
            //Stream.WriteInt(0); // Last online time
            //Stream.WriteInt(0); // Last online time
            //if (Stream.WriteBoolean(true))
            //{
            //    Stream.WriteString("爱的家");
            //    Stream.WriteVInt(5000);
            //    Stream.WriteVInt(28000000);
            //    Stream.WriteVInt(43000000);
            //    Stream.WriteVInt(-1);
            //}
            //Stream.WriteInt(0); // Last online time
            //Stream.WriteInt(0); // Last online time
        }

        public override int GetMessageType()
        {
            return 20105;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
