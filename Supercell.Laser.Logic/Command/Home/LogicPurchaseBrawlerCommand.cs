namespace Supercell.Laser.Logic.Command.Home
{
    using System.Reflection.Metadata;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Gatcha;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Titan.DataStream;

    public class LogicPurchaseBrawlerCommand : Command
    {
        private int Tick1;
        private int Unk1;
        private int Unk2;
        private int VInts1;
        private int BrawlerID;
        private int Unk3;
        private int Unk;
        private int Cost;

        public override void Decode(ByteStream stream)
        {
            base.Decode(stream);
            Tick1 = stream.ReadVInt();
            Unk1 = stream.ReadVInt();
            Unk2 = stream.ReadVInt();
            VInts1 = stream.ReadVInt();
            stream.ReadVInt();
            BrawlerID = stream.ReadVInt();
            Unk3 = stream.ReadVInt();
            Unk = stream.ReadVInt();
        }

        public override int Execute(HomeMode homeMode)
        {
            Console.WriteLine(Tick1);
            Console.WriteLine(Unk1);
            Console.WriteLine(Unk2);
            Console.WriteLine(VInts1);
            Console.WriteLine(BrawlerID);
            Console.WriteLine(Unk3);
            Console.WriteLine(Unk);
            
            //CharacterData character = DataTables.Get(DataType.Character).GetData<CharacterData>("BrawlerName");
            int globalId = GlobalId.CreateGlobalId(16, BrawlerID);

            LogicGiveDeliveryItemsCommand command = new LogicGiveDeliveryItemsCommand();
            DeliveryUnit unit = new DeliveryUnit(100);

            GatchaDrop drop = new GatchaDrop(1);
            drop.DataGlobalId = globalId;
            drop.Count = 1;
            unit.AddDrop(drop);

            command.DeliveryUnits.Add(unit);
            command.Execute(homeMode);

            AvailableServerCommandMessage message = new AvailableServerCommandMessage();
            message.Command = command;
            homeMode.GameListener.SendMessage(message);

            // if Unk == 20 - это хромосомы, не вижу смысла делать их так как они вырезаны.

            List<int> rare = new List<int> { 1, 2, 3, 6, 8, 10, 13, 24 };
            List<int> super_rare = new List<int> { 7, 9, 18, 19, 22, 25, 27, 34, 61, 4 };
            List<int> epic = new List<int> { 14, 15, 16, 20, 26, 29, 30, 36, 43, 45, 48, 50, 58, 69 };
            List<int> mythic = new List<int> { 11, 17, 21, 35, 31, 32, 37, 42, 47, 64, 67, 71, 73 };
            List<int> legendary = new List<int> { 5, 12, 23, 28, 40, 52, 63 };


            if (rare.Contains(BrawlerID))
                Cost = 29;
            else if (super_rare.Contains(BrawlerID))
                Cost = 79;
            else if (epic.Contains(BrawlerID))
                Cost = 169;
            else if (mythic.Contains(BrawlerID))
                Cost = 359;
            else if (legendary.Contains(BrawlerID))
                Cost = 699;
            else
                Cost = 999;

            homeMode.Avatar.UseDiamonds(Cost);

            return 0;
        }

        public override int GetCommandType()
        {
            return 560;
        }
    }
}
