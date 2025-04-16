namespace Supercell.Laser.Logic.Home.Structures
{
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Util;
    using System.Runtime.CompilerServices;
    using System.Reflection;
    using System.Xml.Linq;

    [JsonObject(MemberSerialization.OptIn)]
    public class Hero
    {
        public static readonly int[] UpgradePowerPointsTable = new int[]
        {
            20, 50, 100, 180, 310, 520, 860, 1410, 2300, 2300 + 1440
        };

        public static readonly int[] UpgradeCostTable = new int[]
        {
            20, 35, 75, 140, 290, 480, 880, 1250, 1875, 2800
        };

        [JsonProperty] public int CharacterId;
        [JsonProperty] public int CardId;

        [JsonProperty] public int Trophies;
        [JsonProperty] public int HighestTrophies;

        [JsonProperty] public int PowerPoints;
        [JsonProperty] public int PowerLevel;

        [JsonProperty] public int SelectedStarPowerId;
        [JsonProperty] public int SelectedGadgetId;

        [JsonProperty] public int SelectedGearId1;
        [JsonProperty] public int SelectedGearId2;

        [JsonProperty] public int SelectedSkinId;

        [JsonProperty] public int SelectedOverChargeId;

        [JsonProperty] public Dictionary<int, int> emote;

        public CharacterData CharacterData => DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterId);
        public CardData CardData => DataTables.Get(DataType.Card).GetDataByGlobalId<CardData>(CardId);
        public EmoteData GetDefaultEmoteForCharacter(string Character, string Type)
        {
            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Character == Character && emoteData.EmoteType == Type) return emoteData;
            }
            return null;
        }
        public Hero(int characterId)
        {
            CharacterId = characterId;
            CharacterData characterData = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(characterId);
            CardData g = GetDefaultMetaForHero(5);
            if (g != null) SelectedGadgetId = g.GetInstanceId();
            CardData s = GetDefaultMetaForHero(4);
            if (s != null) SelectedStarPowerId = s.GetInstanceId();
            CardData o = GetDefaultMetaForHero(6);
            if (o != null) SelectedOverChargeId = o.GetInstanceId();
            CardId = DataTables.GetUnlockCardFor(CharacterData).GetGlobalId();
            PowerLevel = 11;
            SelectedGearId1 = 0;
            SelectedGearId2 = 1;
            SelectedSkinId = 0;
            emote = new Dictionary<int, int>();
            emote.Add(1, GetDefaultEmoteForCharacter(characterData.Name, "DEFAULT").GetInstanceId());
            emote.Add(2, 137 - 3);
            emote.Add(3, 148-3);
        }

        public void AddTrophies(int trophies)
        {
            Trophies += trophies;
            HighestTrophies = LogicMath.Max(HighestTrophies, Trophies);
        }

        public void SetTrophies(int trophies)
        {
            Trophies = trophies;
            HighestTrophies = LogicMath.Max(HighestTrophies, Trophies);
        }
        public CardData GetDefaultMetaForHero(int MetaType)
        {
            foreach (CardData carddata in DataTables.Get(DataType.Card).GetDatas())
            {
                if (carddata.Target == CharacterData.Name && carddata.MetaType == MetaType)
                {
                    return carddata;
                }
            }
            return null;
        }
        public void Encode(ByteStream stream)
        {
            ByteStreamHelper.WriteDataReference(stream, CharacterData);
            ByteStreamHelper.WriteDataReference(stream, null);
            stream.WriteVInt(Trophies);
            stream.WriteVInt(HighestTrophies);
            stream.WriteVInt(PowerLevel);
        }
    }
}
