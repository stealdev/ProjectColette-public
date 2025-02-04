using System.Linq;

namespace Supercell.Laser.Logic.Home
{
    using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientHome
    {
        public const int DAILYOFFERS_COUNT = 6;

        public static readonly int[] GoldPacksPrice = new int[]
        {
            20, 50, 140, 280
        };

        public static readonly int[] GoldPacksAmount = new int[]
        {
            150, 400, 1200, 2600
        };

        [JsonProperty] public long HomeId;
        [JsonProperty] public int ThumbnailId;
        [JsonProperty] public int[] CharacterIds;
        [JsonProperty] public int FavouriteCharacter;
        public int CharacterId => CharacterIds[0];

        [JsonIgnore] public List<OfferBundle> OfferBundles;

        [JsonProperty] public int TrophiesReward;
        [JsonProperty] public int TokenReward;
        [JsonProperty] public int StarTokenReward;

        [JsonProperty] public int BrawlPassProgress;
        [JsonProperty] public int PremiumPassProgress;
        [JsonProperty] public int BrawlPassTokens;
        [JsonProperty] public bool HasPremiumPass;
        [JsonProperty] public List<int> UnlockedEmotes;

        [JsonProperty] public int TrophyRoadProgress;
        [JsonIgnore] public Quests Quests;
        [JsonProperty] public int EventId;
        [JsonProperty] public List<PlayerMap> PlayerMaps = new List<PlayerMap>();

        [JsonProperty] public BattleCard DefaultBattleCard;
        [JsonIgnore] public EventData[] Events;

        [JsonProperty] public int PreferredThemeId;

        public PlayerThumbnailData Thumbnail => DataTables.Get(DataType.PlayerThumbnail).GetDataByGlobalId<PlayerThumbnailData>(ThumbnailId);

        public HomeMode HomeMode;

        [JsonProperty] public DateTime LastVisitHomeTime;

        public ClientHome()
        {
            ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            CharacterIds = new int[] { GlobalId.CreateGlobalId(16, 0), GlobalId.CreateGlobalId(16, 1), GlobalId.CreateGlobalId(16, 2) };
            FavouriteCharacter = GlobalId.CreateGlobalId(16, 0);
            OfferBundles = new List<OfferBundle>();
            LastVisitHomeTime = DateTime.UnixEpoch;

            TrophyRoadProgress = 10;

            BrawlPassProgress = 1;
            PremiumPassProgress = 1;
            EventId = 1;
            UnlockedEmotes = new List<int>();
            DefaultBattleCard = new BattleCard();

            PreferredThemeId = -1;
        }

        public void HomeVisited()
        {
            RotateShopContent(DateTime.UtcNow, OfferBundles.Count == 0);
            LastVisitHomeTime = DateTime.UtcNow;

            if (Quests == null && TrophyRoadProgress >= 11)
            {
                Quests = new Quests();
                Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 8);
            }
            else if (Quests != null)
            {
                if (Quests.QuestList.Count < 8) // New quests adds at 07:00 AM UTC
                {
                    Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 8 - Quests.QuestList.Count);
                }
            }
        }

        public void Tick()
        {
            LastVisitHomeTime = DateTime.UtcNow;
            TokenReward = 0;
            TrophiesReward = 0;
            StarTokenReward = 0;
        }

        public void PurchaseOffer(int index)
        {
            ;
        }

        private void RotateShopContent(DateTime time, bool isNewAcc)
        {
            OfferBundles.RemoveAll(bundle => true);
            //if (OfferBundles.Select(bundle => bundle.IsDailyDeals).ToArray().Length > 6)
            //{
            //    OfferBundles.RemoveAll(bundle => bundle.IsDailyDeals);
            //}
            //OfferBundles.RemoveAll(offer => offer.EndTime <= time);

            //if (isNewAcc || DateTime.UtcNow >= DateTime.UtcNow.Date.AddHours(8)) // Daily deals refresh at 08:00 AM UTC
            //{
            //    if (LastVisitHomeTime < DateTime.UtcNow.Date.AddHours(8)||true)
            //    {
            //        UpdateDailyOfferBundles();
            //    }
            //}
            if (true || isNewAcc)
            {
                OfferBundle uab = new OfferBundle();
                uab.Title = "👴要全英雄";
                uab.EndTime = DateTime.UtcNow.Date.AddDays(18141); // tomorrow at 8:00 utc (11:00 MSK)
                uab.Cost = 0;
                Offer offer = new Offer(ShopItem.BrawlBox, 1);
                uab.Items.Add(offer);
                OfferBundles.Add(uab);
            }

        }

        private void UpdateDailyOfferBundles()
        {
            OfferBundles.Add(GenerateDailyGift());

            bool shouldPowerPoints = false;
            for (int i = 1; i < DAILYOFFERS_COUNT; i++)
            {
                OfferBundle dailyOffer = GenerateDailyOffer(shouldPowerPoints);
                if (dailyOffer != null)
                {
                    if (!shouldPowerPoints) shouldPowerPoints = dailyOffer.Items[0].Type != ShopItem.HeroPower;
                    OfferBundles.Add(dailyOffer);
                }
            }
        }

        private OfferBundle GenerateDailyGift()
        {
            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = true;
            bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // tomorrow at 8:00 utc (11:00 MSK)
            bundle.Cost = 0;

            Offer offer = new Offer(ShopItem.FreeBox, 1);
            bundle.Items.Add(offer);

            return bundle;
        }

        private OfferBundle GenerateDailyOffer(bool shouldPowerPoints)
        {
            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = true;
            bundle.EndTime = DateTime.UtcNow.Date.AddDays(1).AddHours(8); // tomorrow at 8:00 utc (11:00 MSK)

            Random random = new Random();
            int type = shouldPowerPoints ? 0 : random.Next(0, 2); // getting a type

            switch (type)
            {
                case 0: // Power points
                    List<Hero> unlockedHeroes = HomeMode.Avatar.Heroes;
                    bool heroValid = false;
                    int generateAttempts = 0;
                    int index = -1;
                    while (!heroValid && generateAttempts < 10)
                    {
                        generateAttempts++;
                        index = random.Next(unlockedHeroes.Count);
                        heroValid = unlockedHeroes[index].PowerPoints < 2300 + 1440;
                        if (heroValid)
                        {
                            foreach (OfferBundle b in OfferBundles)
                            {
                                if (!b.IsDailyDeals) continue;

                                if (b.Items.Count > 0)
                                {
                                    if (b.Items[0].Type == ShopItem.HeroPower)
                                    {
                                        if (b.Items[0].ItemDataId == unlockedHeroes[index].CharacterId)
                                        {
                                            heroValid = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!heroValid) return null;

                    int count = random.Next(15, 100) + 1;
                    Offer offer = new Offer(ShopItem.HeroPower, count, unlockedHeroes[index].CharacterId);

                    bundle.Items.Add(offer);
                    bundle.Cost = count * 2;
                    bundle.Currency = 1;

                    break;
                case 1: // mega box
                    Offer megaBoxOffer = new Offer(ShopItem.MegaBox, 1);
                    bundle.Items.Add(megaBoxOffer);
                    bundle.Cost = 40;
                    bundle.OldCost = 80;
                    bundle.Currency = 0;
                    break;
            }

            return bundle;
        }

        public void Encode(ByteStream stream)
        {//64 VInt 32 Boolean 28 String 16 String 36 Int


            stream.WriteVInt(2000000);
            stream.WriteVInt(0);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(HomeMode.Avatar.Trophies); // Trophies
            stream.WriteVInt(HomeMode.Avatar.HighestTrophies); // Highest Trophies
            stream.WriteVInt(0);
            stream.WriteVInt(300);
            stream.WriteVInt(50000); // Experience
            ByteStreamHelper.WriteDataReference(stream, Thumbnail);
            ByteStreamHelper.WriteDataReference(stream, 43000000);


            stream.WriteVInt(27);//played gamemodes(dont shouw battle hint)
            for (int i = 0; i < 27; i++) stream.WriteVInt(i);
            int skins = 0;
            foreach (Hero hero in HomeMode.Avatar.Heroes)
            {
                if (hero.SelectedSkinId != 0) skins++;
            }
            stream.WriteVInt(skins); // Selected Skins
            foreach (Hero hero in HomeMode.Avatar.Heroes)
            {
                if (hero.SelectedSkinId != 0) ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(29, hero.SelectedSkinId));
            }
            stream.WriteVInt(0); // Randomizer Skin Selected

            stream.WriteVInt(0); // Current Random Skin

            stream.WriteVInt(DataTables.Get(DataType.Skin).Count);
            {
                for (int i = 0; i < DataTables.Get(DataType.Skin).Count; i++) ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(29, i));
            }

            stream.WriteVInt(0); // Unlocked Skin Purchase Option

            stream.WriteVInt(0); // New Item State

            stream.WriteVInt(0);
            stream.WriteVInt(0);//highest trophies
            stream.WriteVInt(0);
            stream.WriteVInt(2);//control mode
            stream.WriteBoolean(false);//battle hints
            stream.WriteVInt(0);//token doubler
            stream.WriteVInt(0);//maybe starr drop timer ? #v50 --risporce(bsds)
            stream.WriteVInt(0);//trophy league timer --risporce(bsds)
            stream.WriteVInt(0);//power play timer --risporce(bsds)
            stream.WriteVInt(0);//Brawl pass season time --risporce(bsds)

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteBoolean(true); // Token Doubler Enabled
            stream.WriteVInt(2);  // Token Doubler New Tag State
            stream.WriteVInt(2);  // Event Tickets New Tag State
            stream.WriteVInt(2);  // Coin Packs New Tag State
            stream.WriteVInt(0);  // Change Name Cost
            stream.WriteVInt(0);  // Timer For the Next Name Change

            stream.WriteVInt(0); // Offers count

            //v51
            //stream.WriteBoolean(false);
            //stream.WriteBoolean(false);
            //stream.WriteVInt(0);
            //stream.WriteVInt(0);
            //stream.WriteBoolean(false);

            stream.WriteVInt(200);
            stream.WriteVInt(-1);

            stream.WriteVInt(0);

            stream.WriteVInt(1);
            stream.WriteVInt(30);

            stream.WriteByte(1);
            ByteStreamHelper.WriteDataReference(stream, CharacterIds[0]);
            //ByteStreamHelper.WriteDataReference(stream, CharacterIds[1]);
            //ByteStreamHelper.WriteDataReference(stream, CharacterIds[2]);

            stream.WriteString("CN");
            stream.WriteString("SB");

            stream.WriteVInt(1);
            stream.WriteVInt(2147483647); stream.WriteVInt(28);  // Have already watched starrdrop stupid animation
            //stream.WriteLong(2, 1);  // Unknown
            //stream.WriteLong(3, 0);  // Tokens Gained
            //stream.WriteLong(4, 0);  // Trophies Gained
            //stream.WriteLong(6, 0);  // Demo Account
            //stream.WriteLong(7, 0);  // Invites Blocked
            //stream.WriteLong(8, 0);  // Star Points Gained
            //stream.WriteLong(9, 1);  // Show Star Points
            //stream.WriteLong(10, 0);  // Power Play Trophies Gained
            //stream.WriteLong(12, 1);  // Unknown
            //stream.WriteLong(14, 0);  // Coins Gained
            //stream.WriteLong(15, 0);  // AgeScreen | 3 = underage (disable social media); | 1 = age popup
            //stream.WriteLong(16, 1);
            //stream.WriteLong(17, 0);  // Team Chat Muted
            //stream.WriteLong(18, 1);  // Esport Button
            //stream.WriteLong(19, 0);  // Champion Ship Lives Buy Popup
            //stream.WriteLong(20, 0);  // Gems Gained
            //stream.WriteLong(21, 0);  // Looking For Team State
            //stream.WriteLong(22, 1);
            //stream.WriteLong(23, 0);  // Club Trophies Gained
            //stream.WriteLong(24, 1);  // Have already watched club league stupid animation

            stream.WriteVInt(0);

            stream.WriteVInt(20); // count brawl pass seasons
            for (int i = 0; i < 20; i++)
            {
                stream.WriteVInt(i);
                stream.WriteVInt(100499);
                stream.WriteBoolean(true);

                stream.WriteVInt(131);
                stream.WriteBoolean(false);

                stream.WriteBoolean(true);

                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteBoolean(true);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                //v53
                stream.WriteBoolean(true);
                stream.WriteBoolean(true);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
                stream.WriteInt(-1);
            }
            stream.WriteVInt(0);

            stream.WriteBoolean(true);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteBoolean(true);
            stream.WriteVInt(1000);  // Vanity Count
            {
                for (int i = 0; i < 1000; i++)
                {
                    ByteStreamHelper.WriteDataReference(stream, GlobalId.CreateGlobalId(52, i));
                    stream.WriteVInt(0);
                    {
                        //stream.WriteVInt(1);
                        //stream.WriteVInt(1);

                    }
                }
            }
            stream.WriteBoolean(false);

            stream.WriteInt(0);
            stream.WriteVInt(502052);
            ByteStreamHelper.WriteDataReference(stream, FavouriteCharacter);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            //v53
            stream.WriteVInt(0);
            stream.WriteVInt(2023189);


            stream.WriteVInt(25); // Count

            stream.WriteVInt(1);
            stream.WriteVInt(2);
            stream.WriteVInt(3);
            stream.WriteVInt(4);
            stream.WriteVInt(5);
            stream.WriteVInt(6);
            stream.WriteVInt(7);
            stream.WriteVInt(8);
            stream.WriteVInt(9);
            stream.WriteVInt(10);
            stream.WriteVInt(11);
            stream.WriteVInt(12);
            stream.WriteVInt(13);
            stream.WriteVInt(14);
            stream.WriteVInt(15);
            stream.WriteVInt(16);
            stream.WriteVInt(17);
            stream.WriteVInt(20);
            stream.WriteVInt(21);
            stream.WriteVInt(22);
            stream.WriteVInt(23);
            stream.WriteVInt(24);
            stream.WriteVInt(30);
            stream.WriteVInt(31);
            stream.WriteVInt(32);

            stream.WriteVInt(Events.Length);
            foreach (EventData e in Events)
            {
                e.Encode(stream);
            }
            //stream.WriteVInt(0);

            stream.WriteVInt(0); // Comming Events

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            //stream.WriteBoolean(true);  // Show Offers Packs

            stream.WriteVInt(0); // ReleaseEntry

            int[] LogicConfData = new int[]
            {
                1,41000000,
                16000070,1,
                16000071,1,
                16000072,1,
                16000073,1,
                16000074,1,
                16000075,1,
                16000076,1,
                16000077,1,
                10027,0,
                10029,20,
                10018,1,
                63,1//dont show scid button
            };
            LogicConfData[1] = 41000000 + (PreferredThemeId == -1 ? 88 : PreferredThemeId);
            stream.WriteVInt(LogicConfData.Length / 2);  // IntValueEntry
            for (int i = 0; i < LogicConfData.Length / 2; i++)
            {
                stream.WriteVInt(LogicConfData[i * 2 + 1]);
                stream.WriteVInt(LogicConfData[i * 2]);
            }

            stream.WriteVInt(0); // Timed Int Value Entry

            stream.WriteVInt(0);  // Custom Event

            stream.WriteVInt(0);  //v53
            stream.WriteVInt(0);  //v53

            stream.WriteVInt(2);
            stream.WriteVInt(1);
            stream.WriteVInt(2);
            stream.WriteVInt(2);
            stream.WriteVInt(1);
            stream.WriteVInt(-1);
            stream.WriteVInt(2);
            stream.WriteVInt(1);
            stream.WriteVInt(4);

            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteLong(HomeId);  // PlayerID

            stream.WriteVInt(0); // NotificationFactory

            stream.WriteVInt(-1);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);//+108
            stream.WriteVInt(0);//+124

            stream.WriteBoolean(false);
            stream.WriteVInt(HomeMode.Avatar.Heroes.Count);//Gears
            foreach (Hero hero in HomeMode.Avatar.Heroes)
            {
                ByteStreamHelper.WriteDataReference(stream, hero.CharacterId);
                List<GearData> gearDatas = new List<GearData>();
                foreach (GearData gearData in DataTables.Get(DataType.Gear).GetDatas())
                {
                    if (gearData.Rarity == "RareGear" || gearData.ExtraHerosAvailableTo.Contains(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(hero.CharacterId).Name)) gearDatas.Add(gearData);
                }
                stream.WriteVInt(gearDatas.Count);
                for (int i = 0; i < gearDatas.Count; i++)
                {
                    ByteStreamHelper.WriteDataReference(stream, gearDatas[i]);
                }
                stream.WriteVInt(2);
                stream.WriteDataReference(62000000 + hero.SelectedGearId1);
                stream.WriteDataReference(62000000 + hero.SelectedGearId2);

            }
            stream.WriteBoolean(false);//v46q

            stream.WriteVInt(0);//v48

            DefaultBattleCard.Encode(stream);

            stream.WriteVInt(0); //sub_D5C83C

            //starrdropthinggy
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteBoolean(false);//v53
        }
    }
}
