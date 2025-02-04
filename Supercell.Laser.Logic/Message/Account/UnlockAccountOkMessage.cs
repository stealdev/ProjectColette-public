using Supercell.Laser.Titan.Math;

namespace Supercell.Laser.Logic.Message.Account
{
    public class UnlockAccountOkMessage : GameMessage
    {
        public LogicLong AccountId;
        public string PassToken;

        public override void Encode()
        {
            Stream.WriteLong(AccountId);
            Stream.WriteString(PassToken);
        }

        public override int GetMessageType()
        {
            return 20132;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
