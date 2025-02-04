namespace Supercell.Laser.Logic.Team
{
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Home;
    public class TeamMember
    {
        public bool IsOwner;
        public long AccountId;

        public int CharacterId;
        public int SkinId;

        public int HeroTrophies;
        public int HeroHighestTrophies;
        public int HeroLevel;

        public int State;
        public bool IsReady;

        public PlayerDisplayData DisplayData;

        public HomeMode homeMode;

        public int TeamIndex;

        public void Encode(ByteStream stream)
        {
            stream.WriteBoolean(IsOwner);
            stream.WriteLong(AccountId);

            ByteStreamHelper.WriteDataReference(stream, CharacterId);
            ByteStreamHelper.WriteDataReference(stream, homeMode.Avatar.GetHero(CharacterId).SelectedSkinId>0?29000000 + homeMode.Avatar.GetHero(CharacterId).SelectedSkinId:0);

            stream.WriteVInt(1000);
            stream.WriteVInt(HeroTrophies);//tropies
            stream.WriteVInt(HeroHighestTrophies);

            stream.WriteVInt(11);//power
            stream.WriteVInt(State);

            stream.WriteBoolean(IsReady);

            stream.WriteVInt(TeamIndex); // team
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // unk
            stream.WriteVInt(0); // unk

            DisplayData.Encode(stream);
            
            ByteStreamHelper.WriteDataReference(stream, 23000000 + homeMode.Avatar.GetHero(CharacterId).SelectedStarPowerId); // star power
            ByteStreamHelper.WriteDataReference(stream, 23000000 + homeMode.Avatar.GetHero(CharacterId).SelectedGadgetId); // star power
            ByteStreamHelper.WriteDataReference(stream, 62000000 + homeMode.Avatar.GetHero(CharacterId).SelectedGearId1); // star power
            ByteStreamHelper.WriteDataReference(stream, 62000000 + homeMode.Avatar.GetHero(CharacterId).SelectedGearId2); // star power
            ByteStreamHelper.WriteDataReference(stream, null); // OVERCHARGE
            
            stream.WriteVInt(0);
        }
    }
}
