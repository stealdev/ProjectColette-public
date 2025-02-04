namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class LogicSelectStarPowerCommand : Command
    {
        public int CardInstanceId;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            CardInstanceId=stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            //Debugger.Print("bruh");
            CardData spg = DataTables.Get(23).GetData<CardData>(CardInstanceId);
            //Debugger.Print(spg.Target.ToString());
            if (spg == null) return 0;
            Hero hero = homeMode.Avatar.GetHeroForCard(spg);
            if (spg.MetaType==4) hero.SelectedStarPowerId=spg.GetInstanceId();
            else hero.SelectedGadgetId=spg.GetInstanceId();
            //Debugger.Print(hero.ToString());
            homeMode.CharacterChanged.Invoke(0);

            return 0;
        }

        public override int GetCommandType()
        {
            return 529;
        }
    }
}
