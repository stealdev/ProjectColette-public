namespace Supercell.Laser.Logic.Command.Home
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Titan.DataStream;

    public class LogicSelectEmoteCommand : Command
    {
        int skinId;
        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            stream.ReadVInt();
            skinId = stream.ReadVInt();
            int slot =stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            //SkinData skinData = DataTables.Get(DataType.Skin).GetData<SkinData>(skinId);
            //SkinConfData skinConfData= DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(skinData.Conf);
            //int characterid=DataTables.Get(16).GetData<CharacterData>(skinConfData.Character).GetInstanceId() + 16000000;
            //Hero hero=homeMode.Avatar.GetHero(characterid);
            //hero.SelectedSkinId = skinId;
            return 0;
        }


        public override int GetCommandType()
        {
            return 538;
        }
    }
}
