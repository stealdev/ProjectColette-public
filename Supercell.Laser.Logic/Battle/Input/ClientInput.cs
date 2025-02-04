namespace Supercell.Laser.Logic.Battle.Input
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Titan.Debug;
    public class ClientInput
    {
        public ClientInput()
        {
            ;
        }

        public int Index;
        public int Type;
        public int EmoteIndex;

        public int X, Y;

        public bool AutoAttack;
        public int AutoAttackTarget; // global id

        public long OwnerSessionId;

        public void Decode(BitStream Stream)
        {
            Index = Stream.ReadPositiveInt(15);
            //Debugger.Print(Index.ToString());
            Type = Stream.ReadPositiveInt(5);

            X = Stream.ReadInt(15);
            Y = Stream.ReadInt(15);
            Stream.ReadBoolean();
            AutoAttack = Stream.ReadBoolean();
            Stream.ReadBoolean();
            if (Type == 9) // use emote
            {
                EmoteIndex=Stream.ReadPositiveInt(3); // emote index
            }
            if (Type == 11) // use emote
            {
                Stream.ReadPositiveVIntMax255(); // emote index
            }
            if (Type == 15) // use emote
            {
                EmoteIndex = Stream.ReadPositiveIntMax511()-6; // emote index
                Stream.ReadPositiveIntMax511(); // emote index
            }
            if (Type == 16) // use emote
            {
                Stream.ReadPositiveIntMax511(); 
                //Stream.ReadPositiveIntMax511(); 
            }
            if (AutoAttack)
            {
                if (Stream.ReadBoolean())
                {
                    int v7 = Stream.ReadPositiveInt(14); // global id
                    AutoAttackTarget = GlobalId.CreateGlobalId(1, v7);
                }
            }
        }
    }
}
