namespace Supercell.Laser.Logic.Data
{
    public class SkinData : LogicData
    {
        public SkinData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }

        public string Rarity { get; set; }
        public string Conf { get; set; }

        public bool Disabled { get; set; }

        public string Campaigns { get; set; }

        public string IconOverride { get; set; }

        public int SkinGroupId { get; set; }

        public int ObtainType { get; set; }

        public int ObtainTypeCN { get; set; }

        public string LastChance { get; set; }

        public string MasterSkin { get; set; }

        public string PetSkin { get; set; }

        public string PetSkin2 { get; set; }

        public int PriceClubCoins { get; set; }

        public int PriceStarPoints { get; set; }

        public int PriceBling { get; set; }

        public int PriceGems { get; set; }

        public int DiscountPriceGems { get; set; }

        //public string PetSkin2 { get; set; }

        //public int CostGems { get; set; }

        //public string TID { get; set; }

        //public string ShopTID { get; set; }

        //public string Features { get; set; }

        //public string CommunityCredit { get; set; }

        //public string MaterialsFile { get; set; }

        //public string BlueTexture { get; set; }

        //public string RedTexture { get; set; }

        //public string BlueSpecular { get; set; }

        //public string RedSpecular { get; set; }
    }
}
