using System.Linq;

namespace Supercell.Laser.Logic.Home
{
    using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using System.Numerics;
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
    using System.Text;
    using System.Reflection.Metadata;
    using System.Formats.Asn1;
    using System.Globalization;

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
        [JsonProperty] public int NameColorId;
        [JsonProperty] public int[] CharacterIds;
        [JsonProperty] public int FavouriteCharacter;
        public int CharacterId => CharacterIds[0];

        [JsonIgnore] public List<OfferBundle> OfferBundles;

        [JsonProperty] public int TrophiesReward;
        [JsonProperty] public int TokenReward;
        [JsonProperty] public int StarTokenReward;

        [JsonProperty] public BigInteger BrawlPassProgress;
        [JsonProperty] public BigInteger PremiumPassProgress;
        [JsonProperty] public BigInteger BrawlPassPlusProgress;
        [JsonProperty] public int BrawlPassTokens;
        [JsonProperty] public bool HasPremiumPass;
        [JsonProperty] public List<int> UnlockedEmotes;
        [JsonProperty] public List<int> UnlockedThumbnails;
        [JsonProperty] public List<int> UnlockedTituls;
        [JsonProperty] public NotificationFactory NotificationFactory;
        [JsonProperty] public List<int> UnlockedSkins;

        [JsonProperty] public int TrophyRoadProgress;
        [JsonIgnore] public Quests Quests;
        [JsonProperty] public int EventId;
        [JsonProperty] public List<PlayerMap> PlayerMaps = new List<PlayerMap>();

        [JsonProperty] public BattleCard DefaultBattleCard;
        [JsonIgnore] public EventData[] Events;

        [JsonProperty] public int PreferredThemeId;

        [JsonProperty] public int RecruitTokens;
        [JsonProperty] public int RecruitBrawler;
        [JsonProperty] public int RecruitBrawlerCard;
        [JsonProperty] public int RecruitGemsCost;
        [JsonProperty] public int RecruitCost;
        [JsonProperty] public int ChromaticCoins; // after v52 - shit bcs not uset yet

        [JsonProperty] public List<string> OffersClaimed;

        [JsonProperty] public Dictionary<int, int> PlayerSelectedEmotes = new Dictionary<int, int>();


        [JsonProperty] public List<int> Brawlers;

        public PlayerThumbnailData Thumbnail => DataTables.Get(DataType.PlayerThumbnail).GetDataByGlobalId<PlayerThumbnailData>(ThumbnailId);
        public NameColorData NameColor => DataTables.Get(DataType.NameColor).GetDataByGlobalId<NameColorData>(NameColorId);

        public HomeMode HomeMode;

        [JsonProperty] public DateTime LastVisitHomeTime;

        public ClientHome()
        {
            Brawlers = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 34, 36, 37, 40, 42, 43, 45, 47, 48, 50, 52, 58, 61, 63, 64, 67, 69, 71, 73 };
            PlayerSelectedEmotes.Add(1, 28);
            PlayerSelectedEmotes.Add(2, 28);
            PlayerSelectedEmotes.Add(3, 28);
            PlayerSelectedEmotes.Add(4, 28);
            PlayerSelectedEmotes.Add(5, 94 -3);
            RecruitBrawler = 1;
            RecruitBrawlerCard = 4;
            RecruitCost = 160;
            RecruitGemsCost = 29;
            RecruitTokens = 0;
            OffersClaimed = new List<string>();

            ThumbnailId = GlobalId.CreateGlobalId(28, 0);
            NameColorId = GlobalId.CreateGlobalId(43, 0);
            CharacterIds = new int[] { GlobalId.CreateGlobalId(16, 0), GlobalId.CreateGlobalId(16, 1), GlobalId.CreateGlobalId(16, 2) };
            FavouriteCharacter = GlobalId.CreateGlobalId(16, 0);

            OfferBundles = new List<OfferBundle>();
            UnlockedSkins = new List<int>();
            UnlockedEmotes = new List<int>();
            UnlockedTituls = new List<int>();
            UnlockedThumbnails = new List<int>();
            LastVisitHomeTime = DateTime.UnixEpoch;

            TrophyRoadProgress = 1;

            BrawlPassProgress = 1;
            PremiumPassProgress = 1;
            EventId = 1;
            UnlockedEmotes = new List<int>();
            DefaultBattleCard = new BattleCard();

            if (NotificationFactory == null) NotificationFactory=new NotificationFactory();

            PreferredThemeId = -1;
        }

        public int TimerMath(DateTime timer_start, DateTime timer_end)
        {
            {
                DateTime timer_now = DateTime.Now;
                if (timer_now > timer_start)
                {
                    if (timer_now < timer_end)
                    {
                        int time_sec = (int)(timer_end - timer_now).TotalSeconds;
                        return time_sec;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public void HomeVisited()
        {

            //OfferBundles.RemoveAll(bundle => bundle.IsTrue);
            GenerateOffer(
                new DateTime(2024, 5, 9, 12, 0, 0), new DateTime(2025, 5, 10, 11, 0, 0),
                10000, 999, 130, ShopItem.Gems,
                0, 0, 0,
                "уцкйцукйцу", "Подарок", "offer_generic",
                0, 0, false,
                false, false, 0,
                0, 0, 0, 0, 0
            );
            GenerateOffer(
            new DateTime(2024, 5, 9, 12, 0, 0), new DateTime(2025, 5, 10, 11, 0, 0),
            1, 999, 754 - 3, ShopItem.Skin,
            0, 0, 0,
            "уцкйцукйцу", "Подарок", "offer_generic",
            0, 0, false,
            false, false, 0,
            0, 0, 0, 0, 0
            );
            GenerateOffer(
            new DateTime(2024, 5, 9, 12, 0, 0), new DateTime(2025, 5, 10, 11, 0, 0),
            1, 999, 756 - 3, ShopItem.Skin,
            0, 0, 0,
            "уцкйцукйцу", "КОЛЯ ДОЛБАЕБ", "offer_generic",
            0, 0, false,
            false, false, 0,
            0, 0, 0, 0, 0
            );

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

        public void PurchaseOfferWithCatalog(int DataGlobalId, int Currency)
        {
            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            if (DataGlobalId > 29000000 && DataGlobalId < 30000000)
            {
                SkinData skinData = DataTables.Get(DataType.Skin).GetDataWithId<SkinData>(DataGlobalId);
                if (Currency == 1) // Gems
                {
                    if (!HomeMode.Avatar.UseDiamonds(skinData.PriceGems)) return;
                }
                if (Currency == 2) // Blings
                {
                    if (!HomeMode.Avatar.UseDiamonds(skinData.PriceBling)) return;
                }

                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(9);
                reward.SkinGlobalId = DataGlobalId;
                reward.Count = 1;
                unit.AddDrop(reward);
               
                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
                NewCommand(skinData);

            } else if (DataGlobalId > 52000000 && DataGlobalId < 53000000)
                {
                    EmoteData skinData = DataTables.Get(DataType.Emote).GetDataWithId<EmoteData>(DataGlobalId);
                    if (Currency == 1) // Gems
                    {
                        if (!HomeMode.Avatar.UseDiamonds(skinData.PriceGems)) return;
                    }
                    if (Currency == 2) // Blings
                    {
                        if (!HomeMode.Avatar.UseDiamonds(skinData.PriceBling)) return;
                    }

                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(11);
                    reward.DataGlobalId = DataGlobalId;
                    reward.Count = 1;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                    command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
            }
            else if (DataGlobalId > 28000000 && DataGlobalId < 29000000)
                {
                    PlayerThumbnailData skinData = DataTables.Get(DataType.PlayerThumbnail).GetDataWithId<PlayerThumbnailData>(DataGlobalId);
                    if (Currency == 1) // Gems
                    {
                        if (!HomeMode.Avatar.UseDiamonds(skinData.PriceGems)) return;
                    }
                    if (Currency == 2) // Blings
                    {
                        if (!HomeMode.Avatar.UseDiamonds(skinData.PriceBling)) return;
                    }

                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(11);
                    reward.DataGlobalId = DataGlobalId;
                    reward.Count = 1;
                    unit.AddDrop(reward);



                    command.DeliveryUnits.Add(unit);
                    command.Execute(HomeMode);
                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);

            }



            void NewCommand(SkinData skinData)
            {
                Quests = new Quests();
                Quests.AddRandomQuests(HomeMode.Avatar.Heroes, 8);

                LogicHeroWinQuestsChangedCommand cmd = new LogicHeroWinQuestsChangedCommand();
                cmd.Quests = Quests;

                AvailableServerCommandMessage message12 = new AvailableServerCommandMessage();
                message12.Command = cmd;
                HomeMode.GameListener.SendMessage(message12);

                DeliveryUnit unit = new DeliveryUnit(100);
                GatchaDrop reward = new GatchaDrop(9);
                foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
                {

                    if (emoteData.Skin == skinData.Name)
                    {
                        GatchaDrop reward1 = new GatchaDrop(11);
                        reward1.DataGlobalId = DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId();
                        reward1.Count = 1;
                        unit.AddDrop(reward1);
                    }
                }
                foreach (PlayerThumbnailData playerThumbnailData in DataTables.Get(DataType.PlayerThumbnail).GetDatas())
                {
                    if (playerThumbnailData.CatalogPreRequirementSkin == skinData.Name )
                    {
                        GatchaDrop reward1 = new GatchaDrop(11);
                        reward1.DataGlobalId = DataTables.Get(DataType.PlayerThumbnail).GetData<PlayerThumbnailData>(playerThumbnailData.Name).GetGlobalId();
                        reward1.Count = 1;
                        unit.AddDrop(reward1);
                    }
                }
                foreach (SprayData sprayData in DataTables.Get(DataType.Spray).GetDatas())
                {
                    if (sprayData.Skin == skinData.Name)
                    {
                        GatchaDrop reward1 = new GatchaDrop(11);
                        reward1.DataGlobalId = DataTables.Get(DataType.Spray).GetData<SprayData>(sprayData.Name).GetGlobalId();
                        reward1.Count = 1;
                        unit.AddDrop(reward1);
                    }
                }
                command.DeliveryUnits.Add(unit);
                command.Execute(HomeMode);

                AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                message.Command = command;
                HomeMode.GameListener.SendMessage(message);
            }


        }

        

        public void PurchaseOffer(int index)
        {
            if (index < 0 || index >= OfferBundles.Count) return;

            OfferBundle bundle = OfferBundles[index];
            if (bundle.Purchased) return;

            if (bundle.Currency == 0)
            {
                if (!HomeMode.Avatar.UseDiamonds(bundle.Cost)) return;
            }
            else if (bundle.Currency == 1)
            {
                if (!HomeMode.Avatar.UseGold(bundle.Cost)) return;
            }

            bundle.Purchased = true;

            if (bundle.Claim == "debug")
            {
                ;
            }
            else
            {
                OffersClaimed.Add(bundle.Claim);
            }



            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            Random rand = new Random();
            foreach (Offer offer in bundle.Items)
            {
                
                if (offer.Type == ShopItem.BrawlBox || offer.Type == ShopItem.FreeBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(10);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.HeroPower)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(6);
                    reward.DataGlobalId = offer.ItemDataId;
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.BigBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(12);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.MegaBox)
                {
                    for (int x = 0; x < offer.Count; x++)
                    {
                        DeliveryUnit unit = new DeliveryUnit(11);
                        HomeMode.SimulateGatcha(unit);
                        if (x + 1 != offer.Count)
                        {
                            command.Execute(HomeMode);
                        }
                        command.DeliveryUnits.Add(unit);
                    }
                }
                else if (offer.Type == ShopItem.Skin)
                {
                    
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(9);
                    reward.SkinGlobalId = GlobalId.CreateGlobalId(29, offer.SkinDataId);
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                    SkinData skinData = DataTables.Get(DataType.Skin).GetDataWithId<SkinData>(GlobalId.CreateGlobalId(29, offer.SkinDataId));
                    NewCommand(skinData);
                }
                else if (offer.Type == ShopItem.Gems)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(8);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.Coin)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(7);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else if (offer.Type == ShopItem.CoinDoubler)
                {
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(2);
                    reward.Count = offer.Count;
                    unit.AddDrop(reward);
                    command.DeliveryUnits.Add(unit);
                }
                else
                {
                    // todo...
                }

                command.Execute(HomeMode);


                void NewCommand(SkinData skinData)
                {
                    if (skinData == null) return;
                    DeliveryUnit unit = new DeliveryUnit(100);
                    GatchaDrop reward = new GatchaDrop(9);
                    foreach (EmoteData emoteData in DataTables.Get(DataType.Emote).GetDatas())
                    {

                        if (emoteData.Skin == skinData.Name  )
                        {
                            GatchaDrop reward1 = new GatchaDrop(11);
                            reward1.DataGlobalId = DataTables.Get(DataType.Emote).GetData<EmoteData>(emoteData.Name).GetGlobalId();
                            reward1.Count = 1;
                            unit.AddDrop(reward1);
                        }

                    }
                    foreach (PlayerThumbnailData playerThumbnailData in DataTables.Get(DataType.PlayerThumbnail).GetDatas())
                    {
                        if (playerThumbnailData.CatalogPreRequirementSkin == skinData.Name )
                        {
                            GatchaDrop reward1 = new GatchaDrop(11);
                            reward1.DataGlobalId = DataTables.Get(DataType.PlayerThumbnail).GetData<PlayerThumbnailData>(playerThumbnailData.Name).GetGlobalId();
                            reward1.Count = 1;
                            unit.AddDrop(reward1);
                        }
       
                    }
                    foreach (SprayData sprayData in DataTables.Get(DataType.Spray).GetDatas())
                    {
                        if (sprayData.Skin == skinData.Name )
                        {
                            GatchaDrop reward1 = new GatchaDrop(11);
                            reward1.DataGlobalId = DataTables.Get(DataType.Spray).GetData<SprayData>(sprayData.Name).GetGlobalId();
                            reward1.Count = 1;
                            unit.AddDrop(reward1);
                        }
                    }
                    command.DeliveryUnits.Add(unit);
                    command.Execute(HomeMode);

                    AvailableServerCommandMessage message = new AvailableServerCommandMessage();
                    message.Command = command;
                    HomeMode.GameListener.SendMessage(message);
                }
            }
            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            HomeMode.GameListener.SendMessage(message);
        }

        
        public void GenerateOffer(
            DateTime OfferStart,
            DateTime OfferEnd,
            int Count,
            int BrawlerID,
            int Extra,
            ShopItem Item,
            int Cost,
            int OldCost,
            int Currency,
            string Claim,
            string Title,
            string BGR,
            int IsTID,
            int DailyOfferType, 
            bool OneTimeOffer,
            bool LoadOnStartup,
            bool Processed,
            int TypeBenefit,
            int Benefit,

            int panelClass,
            int panelType,

            int styleClass,
            int styleType

            )
        {

            OfferBundle bundle = new OfferBundle();
            bundle.IsDailyDeals = false;
            bundle.IsTrue = true;
            bundle.EndTime = OfferEnd;
            bundle.Cost = Cost;
            bundle.OldCost = OldCost;
            bundle.Currency = Currency;
            bundle.Claim = Claim;
            bundle.Title = Title;
            bundle.BackgroundExportName = BGR;
            bundle.IsTID = IsTID;
            bundle.OfferType = DailyOfferType;
            bundle.OneTimeOffer = OneTimeOffer;
            bundle.LoadOnStartup = LoadOnStartup;
            bundle.Processed = Processed;
            bundle.TypeBenefit = TypeBenefit;
            bundle.Benefit = Benefit;
            bundle.ShopPanelLayoutClass = panelClass;
            bundle.ShopPanelLayoutType = panelType;
            bundle.ShopStyleSetClass = styleClass;
            bundle.ShopStyleSetType = styleType;




          //  if (OffersClaimed.Contains(bundle.Claim))
          //  {
          //      bundle.Purchased = true;
           /// }
          //  if (TimerMath(OfferStart, OfferEnd) == -1)
          //  {
           //     bundle.Purchased = true;
          //  }
           // if (HomeMode.HasHeroUnlocked(16000000 + BrawlerID))
           // {
           //     bundle.Purchased = true;
           // }

            Offer offer = new Offer(Item, Count, (16000000 + BrawlerID), Extra);
            bundle.Items.Add(offer);

            OfferBundles.Add(bundle);
        }

        private void RotateShopContent(DateTime time, bool isNewAcc)
        {
            //OfferBundles.RemoveAll(bundle => true);
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
                //OfferBundle uab = new OfferBundle();
                //uab.Title = "👴要全英雄";
                //uab.EndTime = DateTime.UtcNow.Date.AddDays(18141); // tomorrow at 8:00 utc (11:00 MSK)
                //uab.Cost = 0;
                //Offer offer = new Offer(ShopItem.BrawlBox, 1);
                //uab.Items.Add(offer);
               // OfferBundles.Add(uab);
            }

        }


        public void Encode(ByteStream stream)
        {//64 VInt 32 Boolean 28 String 16 String 36 Int


            stream.WriteVInt(2000000);
            stream.WriteVInt(0);

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(HomeMode.Avatar.Trophies); // Trophies
            stream.WriteVInt(HomeMode.Avatar.HighestTrophies); // Highest Trophies
            stream.WriteVInt(HomeMode.Avatar.HighestTrophies);
            stream.WriteVInt(TrophyRoadProgress + 100);
            stream.WriteVInt(50000); // Experience
            ByteStreamHelper.WriteDataReference(stream, ThumbnailId);
            ByteStreamHelper.WriteDataReference(stream, NameColorId);


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

            stream.WriteVInt(UnlockedSkins.Count); // Played game modes
            foreach (int s in UnlockedSkins)
            {
                ByteStreamHelper.WriteDataReference(stream, s);
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

            stream.WriteVInt(OfferBundles.Count); // Shop offers at 0x78e0c4
            foreach (OfferBundle offerBundle in OfferBundles)
            {
                offerBundle.Encode(stream);        // ес че эта хуйня крашит
            };

            /*
            stream.WriteVInt(2); // Offers count

            stream.WriteVInt(1); // GemOffer count

            stream.WriteVInt(19); // Item Type
            stream.WriteVInt(200); // Item Count
            stream.WriteDataReference(16, 76); // Item Data
            stream.WriteVInt(1309-4); // Item CSV Data

            stream.WriteVInt(0); // Currency Type
            stream.WriteVInt(0); // Price
            stream.WriteVInt(84600); // Timer
            stream.WriteVInt(1);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(3); // offer index
            stream.WriteVInt(0);
            stream.WriteBoolean(false); // daily 
            stream.WriteVInt(39); // old price 
            stream.WriteString("мистер аква");
            stream.WriteVInt(1);
            stream.WriteBoolean(false);
            stream.WriteString("offer_bgr_circus");
            stream.WriteVInt(-1);
            stream.WriteBoolean(false);
            stream.WriteVInt(1);
            stream.WriteVInt(1);
            stream.WriteString("");
            stream.WriteBoolean(false);
            stream.WriteBoolean(false); 
            stream.WriteDataReference(0); 
            stream.WriteDataReference(0);   	
            stream.WriteBoolean(false);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(16);
            stream.WriteVInt(77);
            stream.WriteBoolean(false);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(5);
            stream.WriteVInt(11);
            stream.WriteVInt(0);






            stream.WriteVInt(1); // GemOffer count

            stream.WriteVInt(3);
            stream.WriteVInt(11);
            stream.WriteDataReference(16, 76);
            stream.WriteVInt(0);

            stream.WriteVInt(0);
            stream.WriteVInt(1);
            stream.WriteVInt(10000);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(1488);
            stream.WriteString("ОСОБАЯ АКЦИЯ");
            stream.WriteVInt(1);
            stream.WriteBoolean(false);
            stream.WriteString("offer_bgr_kit");
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(1);
            stream.WriteVInt(5);
            stream.WriteString("offer_bgr_stv");
            stream.WriteBoolean(true);
            stream.WriteBoolean(false);
            stream.WriteDataReference(69, 8);
            stream.WriteDataReference(70, 0);
            stream.WriteBoolean(false);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(-1);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            */


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

            stream.WriteByte(1); // brawler selected count
            ByteStreamHelper.WriteDataReference(stream, CharacterIds[0]);
            //ByteStreamHelper.WriteDataReference(stream, CharacterIds[1]);
            //ByteStreamHelper.WriteDataReference(stream, CharacterIds[2]);

            stream.WriteString("RU");
            stream.WriteString("sprkdv и стилдев тоже ващето");

            stream.WriteVInt(2);
            stream.WriteVInt(2147483647); stream.WriteVInt(28);  // Have already watched starrdrop stupid animation
            stream.WriteVIntLong(1, 32); // blings gain
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

            stream.WriteVInt(1); // count brawl pass seasons
            for (int i = 0; i < 1; i++)
            {
                stream.WriteVInt(21);
                stream.WriteVInt(BrawlPassTokens);
                stream.WriteBoolean(HasPremiumPass);

                stream.WriteVInt(131);
                stream.WriteBoolean(false);

                if (stream.WriteBoolean(true)) // Track 9
                {
                    stream.WriteLongLong128(PremiumPassProgress);
                }
                if (stream.WriteBoolean(true)) // Track 10
                {
                    stream.WriteLongLong128(BrawlPassProgress);
                }
                //v53
                stream.WriteBoolean(true); // BrawlPassPlus
                if (stream.WriteBoolean(true)) // Track ?
                {
                    stream.WriteLongLong128(BrawlPassPlusProgress);
                }
            }
            stream.WriteVInt(0);


            if (Quests != null)
            {
                stream.WriteBoolean(true); ;
                Quests.Encode(stream); ;
            }
            else
            {
                stream.WriteBoolean(true); ;
                stream.WriteVInt(0); ;
            }






            stream.WriteVInt(1);
            stream.WriteVInt(2);
            stream.WriteVInt(0);

            stream.WriteBoolean(true);
            stream.WriteVInt(UnlockedEmotes.Count + UnlockedTituls.Count + UnlockedThumbnails.Count); // Played game modes
            foreach (int Emote in UnlockedEmotes)
            {
                ByteStreamHelper.WriteDataReference(stream, Emote);
                stream.WriteVInt(0);
            }
            foreach (int Thumbnail in UnlockedThumbnails)
            {
                ByteStreamHelper.WriteDataReference(stream, Thumbnail);
                stream.WriteVInt(0);
            }
            foreach (int Titul in UnlockedTituls)
            {
                ByteStreamHelper.WriteDataReference(stream, Titul);
                stream.WriteVInt(0);

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
                29, 6, // skin там чет
                63,1//dont show scid button
            };
            LogicConfData[1] = 41000000 + (PreferredThemeId == -1 ? 86 : PreferredThemeId);
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

            NotificationFactory.Encode(stream);

            stream.WriteVInt(-1);
            stream.WriteBoolean(false);
            stream.WriteVInt(0);
            stream.WriteVInt(0);//+108
            stream.WriteVInt(0);//+124



            stream.WriteBoolean(true); // Daily Login Calendar


            stream.WriteVInt(1);
            stream.WriteVInt(2); // Amount
                                 //1
            stream.WriteVInt(0);
            stream.WriteVInt(1);

            stream.WriteVInt(4);
            stream.WriteVInt(1);
            stream.WriteDataReference(0);
            stream.WriteVInt(52);
            stream.WriteBoolean(true);
            stream.WriteVInt(1);

            stream.WriteVInt(1);
            stream.WriteVInt(1); // Count

            stream.WriteVInt(3);
            stream.WriteVInt(1);
            stream.WriteDataReference(16, 0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);





            stream.WriteVInt(1);

            stream.WriteVInt(1); // ItemCycle
            stream.WriteVInt(13); // Count

            stream.WriteVInt(2); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(5); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(3); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(19); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(true); // Unk
            stream.WriteVInt(1);

            stream.WriteVInt(4); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(7); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(true); // Unk
            stream.WriteVInt(1);

            stream.WriteVInt(5); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(4); // ItemType
            stream.WriteVInt(7); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(6); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(8); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk


            stream.WriteVInt(7); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(19); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(8); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(9); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(9); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(19); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(10); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(10); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(11); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(19); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(12); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(34); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Un



            stream.WriteVInt(13); // Day
            stream.WriteVInt(1); // Count

            stream.WriteVInt(19); // ItemType
            stream.WriteVInt(1); // Count
            stream.WriteDataReference(16, 0); // BrawlerID
            stream.WriteVInt(0); // CsvID
            stream.WriteBoolean(false); // Unk

            stream.WriteVInt(14);
            stream.WriteVInt(2); // Count

            stream.WriteVInt(3);
            stream.WriteVInt(1);
            stream.WriteDataReference(16, 0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);

            stream.WriteVInt(3);
            stream.WriteVInt(1);
            stream.WriteDataReference(16, 0);
            stream.WriteVInt(0);
            stream.WriteBoolean(false);


            stream.WriteVInt(0); // DayArrayRange
            stream.WriteVInt(0); // Timer
            stream.WriteVInt(99999);
            stream.WriteVInt(0);
            stream.WriteVInt(0); // Road (if Poco Road - 1, if Brock Road - 2);
            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

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
            stream.WriteBoolean(true);// starroad

            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(0);

            stream.WriteVInt(1);

            List<int> rare = new List<int> { 1, 2, 3, 6, 8, 10, 13, 24 };
            List<int> super_rare = new List<int> { 7, 9, 18, 19, 22, 25, 27, 34, 61, 4 };
            List<int> epic = new List<int> { 14, 15, 16, 20, 26, 29, 30, 36, 43, 45, 48, 50, 58, 69 };
            List<int> mythic = new List<int> { 11, 17, 21, 35, 31, 32, 37, 42, 47, 64, 67, 71, 73 };
            List<int> legendary = new List<int> { 5, 12, 23, 28, 40, 52, 63 };


            stream.WriteDataReference(16, RecruitBrawler); // brawler id

            // brawler token cost \\
            if (rare.Contains(RecruitBrawler))
                stream.WriteVInt(160);
            else if (super_rare.Contains(RecruitBrawler))
                stream.WriteVInt(430);
            else if (epic.Contains(RecruitBrawler))
                stream.WriteVInt(925);
            else if (mythic.Contains(RecruitBrawler))
                stream.WriteVInt(1900);
            else if (legendary.Contains(RecruitBrawler))
                stream.WriteVInt(3800);
            else
                stream.WriteVInt(RecruitCost);


            if (rare.Contains(RecruitBrawler))
                stream.WriteVInt(29);
            else if (super_rare.Contains(RecruitBrawler))
                stream.WriteVInt(79);
            else if (epic.Contains(RecruitBrawler))
                stream.WriteVInt(169);
            else if (mythic.Contains(RecruitBrawler))
                stream.WriteVInt(359);
            else if (legendary.Contains(RecruitBrawler))
                stream.WriteVInt(699);
            else
                stream.WriteVInt(RecruitGemsCost);

            stream.WriteVInt(0);
            stream.WriteVInt(RecruitTokens); // player tokens
            stream.WriteVInt(0);
            stream.WriteVInt(0);


            
            
            stream.WriteVInt(1);

            stream.WriteDataReference(16, 2);

            stream.WriteVInt(1); // index

            stream.WriteVInt(1);



            stream.WriteVInt(0);
            stream.WriteVInt(0);
            stream.WriteVInt(2);
            stream.WriteVInt(0);

            stream.WriteVInt(0);

            stream.WriteVInt(0);
            

            stream.WriteVInt(1);// Mastery points and титулы вроде
            stream.WriteVInt(100000);
            stream.WriteVInt(0);
            stream.WriteDataReference(16, 0);

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
