using System;
namespace Supercell.Laser.Logic.Data
{
    public class SprayData : LogicData
    {
        public SprayData(Row row, DataTable datatable) : base(row, datatable)
        {
            LoadData(this, GetType(), row);
        }

        public string Name { get; set; }
        public string Skin { get; set; }
        public bool GiveOnSkinUnlock { get; set; }
    }
}
