namespace Supercell.Laser.Logic.Home.Gatcha
{
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Titan.DataStream;

    public class LogicReward
    {
        public readonly int Type;
        private List<Offer> GemOffers;

        public LogicReward(int type)
        {
            Type = type;
            GemOffers = new List<Offer>();
        }
        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(0); // maybe count
            foreach (Offer offer in GemOffers)
            {
                offer.Encode(stream);
            } // todo
        }
    }
}
