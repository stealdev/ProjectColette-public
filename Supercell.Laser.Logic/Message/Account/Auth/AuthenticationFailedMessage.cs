namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class AuthenticationFailedMessage : GameMessage
    {
        public int ErrorCode;
        public string FingerprintSha;
        public string Message;
        public string UpdateUrl;
        public DateTime dateTime;
        public string ContentUrl;
        public override void Encode()
        {
            Stream.WriteInt(ErrorCode);
            Stream.WriteString(FingerprintSha);
            Stream.WriteString(null); // Redirect
            Stream.WriteString(ContentUrl); // content url
            Stream.WriteString(UpdateUrl); // update url
            Stream.WriteString(Message);
            
            Stream.WriteInt((int)(dateTime-DateTime.Now).TotalSeconds);
            Stream.WriteBoolean(false);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(0);
            Stream.WriteInt(1);
            Stream.WriteString(null);
            Stream.WriteInt(0);
            Stream.WriteBoolean(true);
            Stream.WriteBoolean(true);
            Stream.WriteString(null);
            Stream.WriteVInt(0);
            Stream.WriteString(null);
            Stream.WriteBoolean(false);
            //Supercell.Laser.Titan.Debug.Debugger.Print("e");
        }

        public override int GetMessageType()
        {
            return 20103;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
}
