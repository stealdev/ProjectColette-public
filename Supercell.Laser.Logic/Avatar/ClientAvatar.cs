namespace Supercell.Laser.Logic.Avatar
{
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using System.IO;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Battle.Objects;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Security.Principal;

    public enum AllianceRole
    {
        None = 0,
        Member = 1,
        Leader = 2,
        Elder = 3,
        CoLeader = 4
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientAvatar
    {
        [JsonProperty] public long AccountId;
        [JsonProperty] public string PassToken;

        [JsonProperty] public string Name;
        [JsonProperty] public bool NameSetByUser;
        [JsonProperty] public int TutorialsCompletedCount = 2;

        [JsonIgnore] public HomeMode HomeMode;

        [JsonProperty] public int Gold;
        [JsonProperty] public int Diamonds;

        [JsonProperty] public List<Hero> Heroes;

        [JsonProperty] public int TrioWins;
        [JsonProperty] public int SoloWins;

        [JsonProperty] public int Tokens;
        [JsonProperty] public int StarTokens;

        [JsonProperty] public bool IsDev;
        [JsonProperty] public bool IsPremium;

        [JsonProperty] public long AllianceId;
        [JsonProperty] public AllianceRole AllianceRole;

        [JsonProperty] public DateTime LastOnline;

        [JsonProperty] public List<Friend> Friends;
        [JsonProperty] public bool Banned;

        [JsonIgnore] public int PlayerStatus;
        [JsonIgnore] public long TeamId;

        [JsonIgnore] public long BattleId;
        [JsonIgnore] public long UdpSessionId;
        [JsonIgnore] public int TeamIndex;
        [JsonIgnore] public int OwnIndex;

        [JsonProperty] public int RollsSinceGoodDrop;

        public int Trophies
        {
            get
            {
                int result = 0;
                foreach (Hero hero in Heroes.ToArray())
                {
                    result += hero.Trophies;
                }
                return result;
            }
        }

        public int HighestTrophies
        {
            get
            {
                int result = 0;
                foreach (Hero hero in Heroes.ToArray())
                {
                    result += hero.HighestTrophies;
                }
                return result;
            }
        }

        public int GetUnlockedBrawlersCountWithRarity(string rarity)
        {
            return Heroes.Count(x => x.CardData.Rarity == rarity);
        }

        public void ResetTrophies()
        {
            foreach (Hero hero in Heroes.ToArray())
            {
                hero.Trophies = 0;
                hero.HighestTrophies = 0;
            }
        }

        public int GetUnlockedHeroesCount()
        {
            return Heroes.Count;
        }

        public void UnlockHero(int characterId)
        {
            Hero heroEntry = new Hero(characterId);
            Heroes.Add(heroEntry);
        }

        public void UpgradeHero(int characterId)
        {
            Hero heroEntry = GetHero(characterId);
            if (heroEntry.SelectedOverChargeId == 0)
            {
                CardData o = heroEntry.GetDefaultMetaForHero(6);
                if (o != null) heroEntry.SelectedOverChargeId = o.GetInstanceId();
            }
        }

        public void RemoveHero(int characterId)
        {
            Heroes.RemoveAll(x => x.CharacterId == characterId);
        }

        public bool HasHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId) != null;
        }
        public Hero GetHero(int characterId)
        {
            return Heroes.Find(x => x.CharacterId == characterId);
        }
        public Hero GetHeroForCard(CardData cardData)
        {
            //Debugger.Print(DataTables.Get(16).GetData<CharacterData>(cardData.Target).GetInstanceId() + "");
            return GetHero(DataTables.Get(16).GetData<CharacterData>(cardData.Target).GetInstanceId() + 16000000);
        }
        public bool UseDiamonds(int count)
        {
            if (count > Diamonds) return false;

            Diamonds -= count;
            return true;
        }

        public bool UseGold(int count)
        {
            if (count > Gold) return false;

            Gold -= count;
            return true;
        }

        public void AddDiamonds(int count)
        {
            Diamonds += count;
        }

        public void AddGold(int count)
        {
            Gold += count;
        }

        public bool UseTokens(int count)
        {
            if (count > Tokens) return false;

            Tokens -= count;
            return true;
        }

        public void AddTokens(int count)
        {
            HomeMode.Home.BrawlPassTokens += count;
        }

        public bool UseStarTokens(int count)
        {
            if (count > StarTokens) return false;

            StarTokens -= count;
            return true;
        }

        public void AddStarTokens(int count)
        {
            StarTokens += count;
        }

        public ClientAvatar()
        {
            Name = "Brawler";

            Gold = 100;
            Diamonds = 0;

            Heroes = new List<Hero>();

            IsDev = false;
            IsPremium = false;

            AllianceRole = AllianceRole.None;
            AllianceId = -1;

            LastOnline = DateTime.UtcNow;
            Friends = new List<Friend>();
        }

        public void SkipTutorial()
        {
            TutorialsCompletedCount = 2;
        }

        public bool IsTutorialState()
        {
            return TutorialsCompletedCount < 2;
        }

        public Friend GetRequestFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState != 4);
        }

        public Friend GetAcceptedFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id && friend.FriendState == 4);
        }
        public EmoteData GetDefaultEmoteForCharacter(string Character, string Type)
        {
            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Character == Character && emoteData.EmoteType == Type) return emoteData;
            }
            return null;
        }
        public Friend GetFriendById(long id)
        {
            return Friends.Find(friend => friend.AccountId == id);
        }
        public void Refresh()
        {
            for (int i = 0; ; i++)
            {
                CharacterData character = DataTables.Get(16).GetDataWithId<CharacterData>(i);
                if (character.Disabled && !character.LockedForChronos)
                {
                    RemoveHero(character.GetGlobalId());
                    continue;
                }
                if (!HasHero(16000000 + i))
                {
                    if (!character.IsHero()) break;
                    UnlockHero(character.GetGlobalId());
                }
            }
        }
        public int Checksum
        {
            get
            {
                ChecksumEncoder encoder = new ChecksumEncoder();
                Encode(encoder);
                return encoder.GetCheckSum();
            }
        }
        //64 vint 36 int 32 boolean
        //124 VInt 108 Int 104 Boolean
        public void Encode(ChecksumEncoder stream)
        {

            stream.WriteVLong(AccountId);
            stream.WriteVLong(AccountId);
            stream.WriteVLong(AccountId);
            stream.WriteString(Name);
            stream.WriteBoolean(NameSetByUser);
            stream.WriteInt(-1);

            stream.WriteVInt(17);
            stream.WriteVInt(1 + Heroes.Count);//1
            {
                //ByteStreamHelper.WriteDataReference(stream, 5000001);
                //stream.WriteVInt(-1);
                //stream.WriteVInt(Tokens);

                ByteStreamHelper.WriteDataReference(stream, 5000008);
                stream.WriteVInt(-1);
                stream.WriteVInt(86420);
                //stream.WriteVInt(Gold);

                //ByteStreamHelper.WriteDataReference(stream, 5000009);
                //stream.WriteVInt(-1);
                //stream.WriteVInt(StarTokens);

                foreach (Hero hero in Heroes)
                {
                    ByteStreamHelper.WriteDataReference(stream, hero.CardData);
                    stream.WriteVInt(-1);
                    stream.WriteVInt(1);
                }
            }

            stream.WriteVInt(Heroes.Count);//2
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(hero.Trophies);
            }

            stream.WriteVInt(Heroes.Count);//3
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(hero.HighestTrophies);
            }

            stream.WriteVInt(Heroes.Count);//4
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(RandomNumberGenerator.GetInt32(4));
            }

            stream.WriteVInt(Heroes.Count);//5
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(hero.PowerPoints);
            }

            stream.WriteVInt(Heroes.Count);//6
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(10);
                //stream.WriteVInt(hero.PowerLevel);
            }
            //Get spgs
            {
                List<int> spgs = new List<int>();
                foreach (CardData carddata in DataTables.Get(DataType.Card).GetDatas())
                {
                    if (carddata.MetaType == 4 || carddata.MetaType == 5 || carddata.MetaType == 6)
                    {
                        spgs.Add(carddata.GetInstanceId());
                    }
                }
                List<int> spgsChosen = new List<int>();
                foreach (Hero hero in Heroes)
                {
                    spgsChosen.Add(hero.SelectedGadgetId);
                    spgsChosen.Add(hero.SelectedStarPowerId);
                    spgsChosen.Add(hero.SelectedOverChargeId);
                }
                stream.WriteVInt(spgs.Count);//7
                for (int i = 0; i < spgs.Count; i++)
                {
                    if (spgsChosen.Contains(spgs[i]))
                    {
                        ByteStreamHelper.WriteDataReference(stream, 23000000 + spgs[i]);
                        stream.WriteVInt(-1);
                        stream.WriteVInt(2);//0 lock 1 unlock 2 chosen
                    }
                    else
                    {
                        ByteStreamHelper.WriteDataReference(stream, 23000000 + spgs[i]);
                        stream.WriteVInt(-1);
                        stream.WriteVInt(1);//0 lock 1 unlock 2 chosen
                    }

                }

            }
            stream.WriteVInt(Heroes.Count);//8
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterData);
                stream.WriteVInt(-1);
                stream.WriteVInt(0);//“新”
                //stream.WriteVInt(hero.PowerLevel);
            }
            stream.WriteVInt(Heroes.Count);//9
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000000 + hero.SelectedGearId1);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);

            }
            stream.WriteVInt(Heroes.Count);//10
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000000 + hero.SelectedGearId2);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(Heroes.Count);//11
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(Heroes.Count);//12
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(Heroes.Count);//13
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(Heroes.Count);//14
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(Heroes.Count);//15
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(1);
                stream.WriteVInt(hero.CharacterId);
            }
            stream.WriteVInt(0);//16(PowerPoints)
            stream.WriteVInt(Heroes.Count);//17
            foreach (Hero hero in Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, 62000004);
                stream.WriteVInt(-1);
                stream.WriteVInt(2);
            }

            stream.WriteVInt(Diamonds); // Diamonds
            stream.WriteVInt(0); // CumulativePurchasedDiamonds

            stream.WriteVInt(0);//player level
            stream.WriteVInt(0);
            stream.WriteVInt(0);//Cumulative
            stream.WriteVInt(0);//battle count
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(2);//tutorial count
            stream.WriteVInt(19);//LogicDataTables::GetRankedRankDataByRank
            stream.WriteVInt(1000);//IsOldPlayer????
            stream.WriteVInt(15);//????
            stream.WriteString(null);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(1);





            //stream.WriteVLong(AccountId);
            //stream.WriteVLong(AccountId);
            //stream.WriteVLong(AccountId);


            //stream.WriteString("shit");
            //stream.WriteVInt(1);

            //stream.WriteInt(0);

            //stream.WriteVInt(8);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);


            //stream.WriteVInt(0);
            //stream.WriteVInt(0);

            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);

            //stream.WriteVInt(2);
        }
    }

}
