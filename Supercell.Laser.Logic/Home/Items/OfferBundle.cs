namespace Supercell.Laser.Logic.Home.Items
{
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;

    public class OfferBundle
    {
        public List<Offer> Items;
        public int Currency;
        public int Cost;
        public bool IsDailyDeals;
        public bool Purchased;
        public DateTime EndTime;
        public int OldCost;
        public string Title;
        public string BackgroundExportName;
        public string Claim;
        public int State;

        public int IsTID;
        public bool IsTrue;
        public int OfferType; // 1 - daily offer 2 - jackpot
        public bool OneTimeOffer;
        public bool LoadOnStartup;
        public bool Processed;
        public int TypeBenefit;
        public int Benefit;

        public int ShopPanelLayoutClass;
        public int ShopPanelLayoutType;

        public int ShopStyleSetClass;
        public int ShopStyleSetType;


        public OfferBundle()
        {
            Items = new List<Offer>();
            State = 0;
        }

        public void Encode(ByteStream Stream)
        {
            Stream.WriteVInt(Items.Count);  // RewardCount
            foreach (Offer gemOffer in Items)
            {
                gemOffer.Encode(Stream);
            }

            Stream.WriteVInt(Currency); // currency
            Stream.WriteVInt(Cost); // cost
            Stream.WriteVInt((int)(EndTime - DateTime.UtcNow).TotalSeconds); // Seconds left
            Stream.WriteVInt(State); // State
            Stream.WriteVInt(0); // ??
            Stream.WriteBoolean(Purchased); // already bought

            Stream.WriteVInt(0); // ??
            Stream.WriteVInt(0); // ??

            Stream.WriteBoolean(false); // is daily deals
            Stream.WriteVInt(OldCost); // Old cost???
            Stream.WriteString(Title); // Name
            Stream.WriteVInt(IsTID); // is tid
            Stream.WriteBoolean(LoadOnStartup); // LoadOnStartup
            Stream.WriteString(BackgroundExportName ?? "offer_generic");  // background
            Stream.WriteVInt(-1);
            Stream.WriteBoolean(Processed); // processed
            Stream.WriteVInt(TypeBenefit); // type benefit
            Stream.WriteVInt(Benefit); // benefit
            Stream.WriteString("");
            Stream.WriteBoolean(OneTimeOffer); // one-time-offer text 
            Stream.WriteBoolean(false);
            Console.WriteLine(ShopPanelLayoutClass);
            Console.WriteLine(ShopPanelLayoutType);
            Console.WriteLine(ShopStyleSetClass);
            Console.WriteLine(ShopStyleSetType);

            if (ShopPanelLayoutClass == 0 && ShopPanelLayoutType == 0)
            {
                Stream.WriteDataReference(0); // shop panel layouts 
            }
            else
            {
                Stream.WriteDataReference(ShopPanelLayoutClass, ShopPanelLayoutType); // shop panel layouts 
            }

            if (ShopStyleSetClass == 0 && ShopStyleSetType == 0)
            {
                Stream.WriteDataReference(0); // shop panel layouts 
            }
            else
            {
                Stream.WriteDataReference(ShopStyleSetClass, ShopStyleSetType); // shop panel layouts 
            }

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(OfferType);
            Stream.WriteVInt(-1);
            Stream.WriteVInt(Cost);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(5);
            Stream.WriteVInt(11);
            Stream.WriteVInt(0);
        }
    }
}
