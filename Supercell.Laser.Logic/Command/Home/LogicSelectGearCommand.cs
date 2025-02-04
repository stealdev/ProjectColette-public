namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicSelectGearCommand : Command
    {
        public int characterId;
        public int gearid;
        public int gearslot;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            characterId = stream.ReadVInt();
            stream.ReadVInt();
            gearid = stream.ReadVInt();
            gearslot = stream.ReadVInt();
            //int g = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            //List<GearData> gearDatas = new List<GearData>();
            //foreach (GearData gearData in DataTables.Get(DataType.Gear).GetDatas())
            //{
            //    if (gearData.Rarity == "RareGear" || gearData.ExtraHerosAvailableTo.Contains(DataTables.Get(DataType.Character).GetDataByGlobalId<CharacterData>(characterId).Name)) gearDatas.Add(gearData);
            //}
            //if(!gearDatas.Contains(DataTables.))
            int globalId = GlobalId.CreateGlobalId(16, characterId);
            if (homeMode.Avatar.HasHero(globalId))
            {
                Hero hero = homeMode.Avatar.GetHero(globalId);
                if (gearslot == 0)
                {
                    hero.SelectedGearId1 = gearid;
                }
                else
                {
                    hero.SelectedGearId2 = gearid;
                }
                homeMode.CharacterChanged.Invoke(0);
                return 0;
            }
            return 0;
        }

        public override int GetCommandType()
        {
            return 543;
        }
    }
}
