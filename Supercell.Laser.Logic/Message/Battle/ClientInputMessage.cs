namespace Supercell.Laser.Logic.Message.Battle
{
    using Supercell.Laser.Logic.Battle.Input;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class ClientInputMessage : GameMessage
    {
        public Queue<ClientInput> Inputs;

        public ClientInputMessage() : base()
        {
            Inputs = new Queue<ClientInput>();
        }

        public override void Decode()
        {
            BitStream stream = new BitStream(Stream.GetByteArray(), Stream.GetLength());

            stream.ReadPositiveInt(14);//tick
            stream.ReadPositiveInt(10);
            stream.ReadPositiveInt(13);//index
            stream.ReadPositiveInt(10);
            stream.ReadPositiveInt(10);
            stream.ReadPositiveInt(10);//keep alices sent


            int count = stream.ReadPositiveInt(5);
            for (int i = 0; i < count; i++)
            {
                ClientInput input = new ClientInput();
                input.Decode(stream);
                Inputs.Enqueue(input);
            }
        }

        public override int GetMessageType()
        {
            return 10555;
        }

        public override int GetServiceNodeType()
        {
            return 27;
        }
    }
}
