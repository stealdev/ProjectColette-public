namespace Supercell.Laser.Logic.Message.Friends
{
    using Supercell.Laser.Logic.Friends;

    public class AllianceOnlineStatusUpdatedMessage : GameMessage
    {
        public int Members;
        public long AvatarId;
        public int PlayerStatus;

        public override void Encode()
        {
            Stream.WriteVInt(100);
            Stream.WriteVInt(1);
            Stream.WriteLong(AvatarId);
            Stream.WriteVInt(PlayerStatus);
        }

        public override int GetMessageType()
        {
            return 20207;
        }

        public override int GetServiceNodeType()
        {
            return 3;
        }
    }
}
