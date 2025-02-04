namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSelectSkinCommand : Command
    {
        int skinId;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            skinId = stream.ReadVInt();
            stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            SkinData skinData = DataTables.Get(DataType.Skin).GetData<SkinData>(skinId);
            SkinConfData skinConfData= DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(skinData.Conf);
            int characterid=DataTables.Get(16).GetData<CharacterData>(skinConfData.Character).GetInstanceId() + 16000000;
            Hero hero=homeMode.Avatar.GetHero(characterid);
            hero.SelectedSkinId = skinId;
            homeMode.CharacterChanged.Invoke(0);
            return 0;
        }


        public override int GetCommandType()
        {
            return 506;
        }
    }
}
