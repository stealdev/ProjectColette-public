namespace Supercell.Laser.Logic.Message.Ranking
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Helper;
    using System.Reflection.Emit;

    public class LeaderboardMessage : GameMessage
    {
        public int LeaderboardType { get; set; }

        public List<KeyValuePair<ClientHome, ClientAvatar>> Avatars;
        public List<Alliance> AllianceList;
        public long OwnAvatarId;
        public string Region { get; set; }
        public int HeroDataId;

        public LeaderboardMessage() : base()
        {
            Avatars = new List<KeyValuePair<ClientHome, ClientAvatar>>();
            AllianceList = new List<Alliance>();
            LeaderboardType = 1;
        }

        public override void Encode()
        {
            int playerIndex = 0;

            Stream.WriteVInt(LeaderboardType);
            Stream.WriteVInt(0);
            ByteStreamHelper.WriteDataReference(Stream,HeroDataId);
            Stream.WriteString(Region); // Region
            //goto LABEL_1;
            if (LeaderboardType == 1)
            {
                Stream.WriteVInt(Avatars.Count);
                foreach (var pair in Avatars)
                {
                    var home = pair.Key;
                    var avatar = pair.Value;
                    if (avatar.AccountId == OwnAvatarId)
                    {
                        playerIndex = Avatars.IndexOf(pair) + 1;
                    }

                    Stream.WriteVLong(avatar.AccountId);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(Avatars.IndexOf(pair));
                    //Stream.WriteVInt(avatar.Trophies);

                    Stream.WriteBoolean(true);
                    Stream.WriteString(null);
                    Stream.WriteString(avatar.Name ?? "NoName");
                    Stream.WriteVInt(100);
                    Stream.WriteVInt(home.ThumbnailId);
                    Stream.WriteVInt(home.NameColorId);
                    Stream.WriteVInt(0);
                    Stream.WriteBoolean(false);
                }
            }
            else if (LeaderboardType == 0)
            {
                Stream.WriteVInt(Avatars.Count);
                foreach (var pair in Avatars)
                {
                    var home = pair.Key;
                    var avatar = pair.Value;
                    if (avatar.AccountId == OwnAvatarId)
                    {
                        playerIndex = Avatars.IndexOf(pair) + 1;
                    }

                    Stream.WriteVLong(avatar.AccountId);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(avatar.GetHero(HeroDataId).Trophies);

                    Stream.WriteBoolean(true);
                    Stream.WriteString(null);
                    Stream.WriteString(avatar.Name ?? "NoName");
                    Stream.WriteVInt(100);
                    Stream.WriteVInt(home.ThumbnailId);
                    Stream.WriteVInt(43000000);
                    Stream.WriteVInt(0);
                    Stream.WriteBoolean(false);
                }
            }
            Stream.WriteVInt(0);
            Stream.WriteVInt(playerIndex);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteString("BS");
        }

        public override int GetMessageType()
        {
            return 24403;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
