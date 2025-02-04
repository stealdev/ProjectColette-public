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
            Stream.WriteString(@"
.__                                  ,          __          .          ,       ,      
[__)._._ *  _    _.-+-   /       `  _  |  _-+--+-_  
|      [  (_)|(/,(_.  |      \__.(_)|(/,  |     |(/,
                 ._|                                                          
(Shei's Brawl on v46)

" + "内存：" +megabytesUsed+
"\n在线人数："+playercount.ToString()+"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
//"\n Brawlstars Foreverrrrrr!!!!!!" +
//"\n(Not Supercell)" +
//"\nTODO:" +
//"\n修复装备 50%" +
//"\n修复战队 ✅" +
//"\n攻击逻辑 80%" +
//"\n选择星辉和选择妙具 ✅ 请及时至QQ更新幽暗车站版本！    （如果你看到了这个，大概是已经更新了）" +

//"\n移动逻辑 ✅     " +
//"\n退出键 ✅" +
//"\n对战结束 ✅" +
//"\n" +
//"\n" +
//"\n点对战可以匹配！" +
//"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            Stream.WriteVInt(0);
            //Debugger.Print("E");
            //Stream.WriteVInt(0);
            //Stream.WriteVInt(0);
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
