using System.Diagnostics;
using Supercell.Laser.Titan.Debug;
namespace Supercell.Laser.Logic.Message.Home
{
    public class LobbyInfoMessage : GameMessage
    {
        public int playercount;
        public override void Encode()
        {
            long megabytesUsed = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
            Stream.WriteVInt(4480);
            Stream.WriteString(@"<c32ff00>A<c65ff00>r<c98ff00>c<ccbff00>h<cffff00>B<cffff00>r<cffcc00>a<cff9900>w<cff6600>l</c>"+ "\n<cff2400>T<cff4800>G<cff6d00>:<cfe9100> <cffb600>@<cffda00>a<cfffe00>r<cdaff00>c<cb6ff00>h<c91ff00>b<c6dfe00>r<c48ff00>a<c24ff00>w<c05ff00>l</c>\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            Stream.WriteVInt(0);
        }

        public override int GetMessageType()
        {
            return 23457;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
