namespace Supercell.Laser.Logic.Home.Items
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json.Converters;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;

    public class EventData
    {
        public int Slot;
        public int LocationId;
        public DateTime EndTime;
        public LocationData Location => DataTables.Get(DataType.Location).GetDataByGlobalId<LocationData>(LocationId);
        public BattlePlayerMap BattlePlayerMap;

        public void Encode(ByteStream encoder)
        {
            encoder.WriteVInt(-1);
            encoder.WriteVInt(Slot);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);//v53
            encoder.WriteVInt((int)(EndTime - DateTime.Now).TotalSeconds);
            encoder.WriteVInt(0);
            if (Slot == 12 || Slot == 13) ByteStreamHelper.WriteDataReference(encoder, null);
            //else if(LocationId==15000121)ByteStreamHelper.WriteDataReference(encoder, 15000122);
            else ByteStreamHelper.WriteDataReference(encoder, Location);



            encoder.WriteVInt(0); // GameModeVaridation
            encoder.WriteVInt(2);

            encoder.WriteString(null); // 0xacecac
            encoder.WriteVInt(0); // 0xacecc0
            encoder.WriteVInt(0); // 0xacecd4
            encoder.WriteVInt(0); // 0xacece8

            encoder.WriteVInt(0); // modifier

            encoder.WriteVInt(0); // 0xacee58
            encoder.WriteVInt(0); // 0xacee6c
            //encoder.WriteBoolean(false);
            //encoder.WriteVInt(0); // 0xacee6c

            ByteStreamHelper.WriteBattlePlayerMap(encoder, BattlePlayerMap);

            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);//LogicRankedSeason
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            //encoder.WriteBoolean(false);
            encoder.WriteVInt(-1);
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(-1);

            encoder.WriteVInt(0);//v51
            encoder.WriteVInt(0);//v51
            encoder.WriteVInt(0);//v51

            encoder.WriteBoolean(false);//v53
        }
    }
}
