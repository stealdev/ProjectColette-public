namespace Supercell.Laser.Logic.Battle.Structures
{
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;
    using Supercell.Laser.Titan.Util;
    using Supercell.Laser.Logic.Data;
    using System.Diagnostics.SymbolStore;
    using System.Numerics;
    using Newtonsoft.Json.Bson;
    using System.Security.Cryptography;
    using Supercell.Laser.Logic.Battle.Objects;

    public class BattlePlayer
    {
        public long AccountId;
        public int PlayerIndex;
        public int TeamIndex;

        public long TeamId = -1;

        public int HeroIndex;
        public int HeroIndexMax;

        public PlayerDisplayData DisplayData;
        public int CharacterId => CharacterIds[HeroIndex];
        public int SkinId => SkinIds[HeroIndex];
        public long SessionId;
        public LogicGameListener GameListener;

        public int[] CharacterIds = new int[3];
        public int[] SkinIds = new int[3];
        public int OwnObjectId;
        public int PreviousObjectId;
        public int ControlTicksLeft;
        public int LastHandledInput;

        public int Trophies, HighestTrophies;

        public int HeroPowerLevel;

        private int Score;
        private LogicVector2 SpawnPoint;

        public int StartUsingPinTicks;
        public int PinIndex;
        public SkinConfData SkinConfData => SkinId == 0 ? null : DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(SkinId).Conf);
        public SkinData SkinData => SkinId == 0 ? null : DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(SkinId);

        public CardData AccessoryCardData => AccessoryCardDatas[HeroIndex];
        public CardData[] AccessoryCardDatas;
        public AccessoryData AccessoryData => AccessoryDatas[HeroIndex];
        public AccessoryData[] AccessoryDatas;
        public CardData StarPowerData => StarPowerDatas[HeroIndex];
        public CardData[] StarPowerDatas;
        public Accessory Accessory;
        public CharacterData CharacterData => CharacterDatas[HeroIndex];
        public CharacterData[] CharacterDatas;
        public GearData Gear1;
        public GearData Gear2;
        public CardData[] OverChargeDatas;
        public CardData OverChargeData => OverChargeDatas[HeroIndex];
        public int PowerLevel;

        public int Bot;
        public bool IsAdmin;

        private int UltiCharge;
        private int OverCharge;

        public bool OverCharging;
        public bool OverChargeActivated;
        public bool OverChargeStarted;
        public bool OverChargeEnded;

        public bool IsAlive;
        public int BattleRoyaleRank;

        public List<PlayerKillEntry> KillList;

        public int Kills;
        public int Damage;
        public int Heals;

        public int DeathTick;

        public int T1;
        public int T2;
        public int E;
        public int Ti;
        public BattlePlayer()
        {
            DisplayData = new PlayerDisplayData();
            SpawnPoint = new LogicVector2();

            StartUsingPinTicks = -9999;

            HeroPowerLevel = 11;
            BattleRoyaleRank = -1;

            KillList = new List<PlayerKillEntry>();

            UltiCharge = 0;
            if (IsAdmin) UltiCharge = 4000;
            OverCharge = 0;
            DeathTick = 1;

        }

        public void Healed(int heals)
        {
            Heals += heals;
        }

        public void DamageDealed(int damage)
        {
            Damage += damage;
        }

        public void KilledPlayer(int index, int bountyStars)
        {
            KillList.Add(new PlayerKillEntry()
            {
                PlayerIndex = index,
                BountyStarsEarned = bountyStars
            });

            Kills++;
        }

        public bool HasUlti()
        {
            return UltiCharge >= 4000;
        }

        public bool OverChargeReady()
        {
            return OverCharge >= 4000;
        }

        public int GetUltiCharge()
        {
            return UltiCharge;
        }

        public int GetOverCharge()
        {
            return OverCharge;
        }
        public int GetCardValueForPassive(string a2, int a3)
        {
            if (StarPowerData != null)
            {
                if (StarPowerData.Type == a2)
                {
                    if (a3 == 2) return StarPowerData.Value3;
                    return a3 > 0 ? StarPowerData.Value : StarPowerData.Value2;
                }
                else return -1;

            }
            else
                return -1;
        }
        public void AddUltiCharge(int amount)
        {
            UltiCharge = LogicMath.Clamp(UltiCharge + amount, 0, 4000);
        }
        public int GetGearBoost(int LogicType)
        {
            if (Gear1 != null)
            {
                if (Gear1.LogicType == LogicType)
                {
                    return Gear1.ModifierValue;
                }
            }
            if (Gear2 != null)
            {
                if (Gear2.LogicType == LogicType)
                {
                    return Gear2.ModifierValue;
                }
            }
            return 0;
        }
        public void ChargeUlti(int a2, bool a3, bool a4)
        {
            int v9 = 100;
            if (!a4)
            {
                if (a3)
                    v9 = CharacterData.UltiChargeUltiMul;
                else
                    v9 = CharacterData.UltiChargeMul;
            }
            int result = v9 * a2 / 100 + UltiCharge;
            if (result > 4000)
                result = 4000;
            if (UltiCharge < 4000) UltiCharge = result;
            int Div = 2;
            if (CharacterData.Name == "Roller") Div = 20;
            if (HasOverCharge()) OverCharge = LogicMath.Min(4000, OverCharge + v9 * a2 / 100 / Div);
        }

        public void UseUlti()
        {
            if (!IsAdmin) UltiCharge = 0;
            //UltiCharge = 0;
        }

        public void UseOverCharge(Character character)
        {
            if (OverChargeReady() && !OverCharging) OverCharging = true;
        }

        public void UpdateOverCharge()// 神经元计算机反混淆控制流
        {
            OverChargeStarted = OverCharging && !OverChargeActivated;
            OverChargeActivated = OverCharging;
            OverChargeEnded = false;
            if (OverCharging)
            {
                OverCharge -= 40;
                if (!IsAlive || OverCharge <= 0)
                {
                    OverChargeEnded = true;
                    OverCharge = 0;
                    OverCharging = false;
                }
            }
        }
        public int IsBot()
        {
            return Bot;
        }

        public BattlePlayer(ClientHome home, ClientAvatar avatar) : this()
        {
            Home = home;
            Avatar = avatar;
        }

        public void AddScore(int a)
        {
            Score += a;
        }

        public void ResetScore()
        {
            Score = 0;
        }
        public EmoteData GetDefaultEmoteForCharacter(string Character, string Type)
        {
            foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
            {
                if (emoteData.Character == Character && emoteData.EmoteType == Type) return emoteData;
            }
            return null;
        }
        public void UsePin(int index, int ticks)
        {
            StartUsingPinTicks = ticks;
            PinIndex = index;
        }

        public bool IsUsingPin(int ticks)
        {
            return ticks - StartUsingPinTicks < 80;
        }

        public void SetHeroIndex(int Index)
        {
            HeroIndex = Index;
            Accessory = AccessoryDatas[Index] == null ? null : new Accessory(AccessoryDatas[Index]);

        }
        public int GetScore()
        {
            return Score;
        }

        public void SetSpawnPoint(int x, int y)
        {
            SpawnPoint.Set(x, y);
        }

        public LogicVector2 GetSpawnPoint()
        {
            return SpawnPoint.Clone();
        }
        public bool HasOverCharge()
        {
            return OverChargeData != null;
        }
        public void Encode(ByteStream stream)
        {
            stream.WriteLong(AccountId);
            stream.WriteVInt(PlayerIndex);
            stream.WriteVInt(TeamIndex);
            stream.WriteVInt(0);
            stream.WriteInt(0);
            //68 44 VInt 40 Int 32 Boolean 28 string
            stream.WriteVInt(HeroIndexMax + 1);
            for (int i = 0; i <= HeroIndexMax; i++)
            {
                ByteStreamHelper.WriteDataReference(stream, CharacterIds[i] > 16000080 ? 16000000 : CharacterIds[i]);
                if (stream.WriteBoolean(true))
                {
                    //logic Hero Upgrades
                    stream.WriteVInt(HeroPowerLevel);
                    ByteStreamHelper.WriteDataReference(stream, StarPowerDatas[i]);
                    ByteStreamHelper.WriteDataReference(stream, AccessoryCardDatas[i]);
                    //ByteStreamHelper.WriteDataReference(stream, null);

                    ByteStreamHelper.WriteDataReference(stream, Gear1);
                    ByteStreamHelper.WriteDataReference(stream, Gear2);
                    //stream.WriteVInt(3);
                    //stream.WriteVInt(3);
                    ByteStreamHelper.WriteDataReference(stream, OverChargeDatas[i]); //OverCharge

                }
                if (stream.WriteBoolean(true))
                {
                    stream.WriteVInt(5);
                    ByteStreamHelper.WriteDataReference(stream, GetDefaultEmoteForCharacter(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterIds[i]).Name, "HAPPY"));
                    ByteStreamHelper.WriteDataReference(stream, GetDefaultEmoteForCharacter(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterIds[i]).Name, "SAD"));
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(52, 175));
                    ByteStreamHelper.WriteDataReference(stream, GetDefaultEmoteForCharacter(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterIds[i]).Name, "THANKS"));
                    ByteStreamHelper.WriteDataReference(stream, GetDefaultEmoteForCharacter(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterIds[i]).Name, "GG"));
                    //ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(52, 175));
                    stream.WriteVInt(5);


                }
                if (stream.WriteBoolean(true))
                {
                    stream.WriteVInt(5);
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(68, 0));
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(68, 1));
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(68, 2));
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(68, 3));
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(68, 4));

                }
                string tmp = DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(CharacterIds[i]).Name;
                if (tmp == "MechaDude" || tmp == "CannonGirl") ByteStreamHelper.WriteDataReference(stream, null);
                else ByteStreamHelper.WriteDataReference(stream, SkinIds[i]);
                ByteStreamHelper.WriteDataReference(stream, null);
                stream.WriteVInt(0);
            }

            DisplayData.Encode(stream);
            stream.WriteBoolean(false);
            if (stream.WriteBoolean(false))
            {
                stream.WriteVLong(AccountId);
                stream.WriteString("SB");
                ByteStreamHelper.WriteDataReference(stream, null);//8
            }
            //new
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            ByteStreamHelper.WriteDataReference(stream, T1);
            ByteStreamHelper.WriteDataReference(stream, T2);
            ByteStreamHelper.WriteDataReference(stream, E);
            ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(76, RandomNumberGenerator.GetInt32(0, 66)));//76
            stream.WriteVInt(0);
        }

        [JsonIgnore] public readonly ClientHome Home;
        [JsonIgnore] public readonly ClientAvatar Avatar;

        public static BattlePlayer Create(ClientHome home, ClientAvatar avatar, int playerIndex, int teamIndex)
        {
            BattlePlayer player = new BattlePlayer(home, avatar);
            player.DisplayData.Name = avatar.Name;
            player.AccessoryCardDatas = new CardData[3];
            player.AccessoryDatas = new AccessoryData[3];
            player.StarPowerDatas = new CardData[3];
            player.CharacterDatas = new CharacterData[3];
            player.OverChargeDatas = new CardData[3];

            player.DisplayData.ThumbnailId = home.ThumbnailId;
            player.AccountId = avatar.AccountId;
            for (int i = 0; i < home.CharacterIds.Length; i++)
            {
                player.CharacterIds[i] = (home.CharacterIds[i]);
                player.AccessoryCardDatas[i] = (avatar.GetHero(home.CharacterIds[i]).SelectedGadgetId == 0 ? null : DataTables.Get(23).GetData<CardData>(avatar.GetHero(home.CharacterIds[i]).SelectedGadgetId));
                player.AccessoryDatas[i] = (player.AccessoryCardDatas[i] == null ? null : DataTables.Get(DataType.Accessory).GetData<AccessoryData>(player.AccessoryCardDatas[i].Name));

                player.StarPowerDatas[i] = (DataTables.Get(23).GetData<CardData>(avatar.GetHero(home.CharacterIds[i]).SelectedStarPowerId));

                if (avatar.GetHero(home.CharacterIds[i]).SelectedSkinId > 0) player.SkinIds[i] = (GlobalId.CreateGlobalId(29, avatar.GetHero(home.CharacterIds[i]).SelectedSkinId));

                player.CharacterDatas[i] = (DataTables.Get(DataType.Character).GetDataWithId<CharacterData>(home.CharacterIds[i]));
                int OverChargeId = avatar.GetHero(home.CharacterIds[i]).SelectedOverChargeId;
                if (OverChargeId > 0) player.OverChargeDatas[i] = DataTables.Get(23).GetData<CardData>(avatar.GetHero(home.CharacterIds[i]).SelectedOverChargeId);
                else player.OverChargeDatas[i] = DataTables.Get(23).GetData<CardData>(0);
            }
            player.HeroIndexMax = home.CharacterIds.Length - 1;
            player.Gear1 = DataTables.Get(DataType.Gear).GetDataByGlobalId<GearData>(62000000 + avatar.GetHero(home.CharacterIds[0]).SelectedGearId1);
            player.Gear2 = DataTables.Get(DataType.Gear).GetDataByGlobalId<GearData>(62000000 + avatar.GetHero(home.CharacterIds[0]).SelectedGearId2);
            player.Accessory = player.AccessoryData == null ? null : new Accessory(player.AccessoryData);
            player.PlayerIndex = playerIndex;
            player.TeamIndex = teamIndex;
            Hero hero = avatar.GetHero(home.CharacterIds[0]);
            player.Trophies = hero.Trophies;
            player.HighestTrophies = hero.HighestTrophies;
            player.HeroPowerLevel = 11;

            player.T1 = home.DefaultBattleCard.Thumbnail1;
            player.T2 = home.DefaultBattleCard.Thumbnail2;
            player.E = home.DefaultBattleCard.Emote;
            player.Ti = home.DefaultBattleCard.Title;
            //player.GameListener = new LogicGameListener();
            //player.HeroIndex = 1;
            return player;
        }

        public static BattlePlayer CreateBotInfo(string name, int playerIndex, int teamIndex, int character = 16000000)
        {

            BattlePlayer player = new BattlePlayer();
            player.AccessoryCardDatas = new CardData[3];
            player.AccessoryDatas = new AccessoryData[3];
            player.StarPowerDatas = new CardData[3];
            player.CharacterDatas = new CharacterData[3];
            player.OverChargeDatas = new CardData[3];
            player.DisplayData.Name = name;
            player.DisplayData.ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            player.AccountId = 100000 + playerIndex;
            player.CharacterIds[0] = (character);
            player.AccessoryCardDatas[0] = DataTables.Get(23).GetData<CardData>(255);
            player.AccessoryDatas[0] = DataTables.Get(DataType.Accessory).GetData<AccessoryData>(player.AccessoryCardData.Name);
            //player.AccessoryUsesLeft = 3;
            player.Accessory = player.AccessoryData == null ? null : new Accessory(player.AccessoryData);
            player.Gear1 = DataTables.Get(DataType.Gear).GetDataByGlobalId<GearData>(62000000);
            player.Gear2 = DataTables.Get(DataType.Gear).GetDataByGlobalId<GearData>(62000001);
            player.PlayerIndex = playerIndex;
            player.TeamIndex = teamIndex;
            player.SessionId = -1;
            player.Bot = 1;
            player.CharacterDatas[0] = (DataTables.Get(DataType.Character).GetDataWithId<CharacterData>(character));
            player.HeroIndexMax = 0;
            return player;
        }
        public static BattlePlayer CreateStoryModeDummy(string name, int playerIndex, int teamIndex, int character = 0, int skinid = 0, int accessory = 0)
        {

            BattlePlayer player = new BattlePlayer();
            player.AccessoryCardDatas = new CardData[3];
            player.AccessoryDatas = new AccessoryData[3];
            player.StarPowerDatas = new CardData[3];
            player.CharacterDatas = new CharacterData[3];
            player.OverChargeDatas = new CardData[3];
            player.DisplayData.Name = name;
            player.DisplayData.ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            player.AccountId = 100000 + playerIndex;
            player.CharacterIds[0] = (GlobalId.CreateGlobalId(16, character));
            if (accessory != 0)
            {
                player.AccessoryCardDatas[0] = DataTables.Get(23).GetData<CardData>(accessory);
                player.AccessoryDatas[0] = DataTables.Get(DataType.Accessory).GetData<AccessoryData>(player.AccessoryCardDatas[0].Name);
            }
            player.HeroIndexMax = 0;
            player.Accessory = player.AccessoryData == null ? null : new Accessory(player.AccessoryData);
            player.Gear1 = null;
            player.Gear2 = null;
            player.PlayerIndex = playerIndex;
            player.TeamIndex = teamIndex;
            player.SessionId = -1;
            if (skinid != 0) player.SkinIds[0] = (GlobalId.CreateGlobalId(29, skinid));
            player.CharacterDatas[0] = (DataTables.Get(DataType.Character).GetDataWithId<CharacterData>(character));

            return player;
        }

    }
}
