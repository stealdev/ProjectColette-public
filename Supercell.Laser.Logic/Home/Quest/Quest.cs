namespace Supercell.Laser.Logic.Home.Quest
{
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Logic.Helper;

    public class Quest
    {
        public int MissionType { get; set; }
        public int CurrentGoal { get; set; }
        public int QuestGoal { get; set; }
        public int GameModeVariation { get; set; }
        public bool QuestSeen { get; set; }
        public int CharacterId { get; set; }
        public int Reward { get; set; }
        public int Progress { get; set; }

        public Quest Clone()
        {
            return new Quest()
            {
                MissionType = MissionType,
                CurrentGoal = CurrentGoal,
                QuestGoal = QuestGoal,
                GameModeVariation = GameModeVariation,
                QuestSeen = QuestSeen,
                CharacterId = CharacterId,
                Reward = Reward,
                Progress = Progress
            };
        }

        public void EncodeBattleEnd(ChecksumEncoder encoder)
        {
            encoder.WriteVInt(85);
            encoder.WriteVInt(24); // brawl pass season
            encoder.WriteVInt(1);// type
            encoder.WriteVInt(CurrentGoal); // goal
            encoder.WriteVInt(QuestGoal); // выполнено из
            encoder.WriteVInt(10); // reward
            ByteStreamHelper.WriteDataReference(encoder, 16000000); // brawler
            ByteStreamHelper.WriteDataReference(encoder, 0); // idk
            ByteStreamHelper.WriteDataReference(encoder, 0); // idk
            encoder.WriteVInt(1); // reward type
            encoder.WriteVInt(1);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(true);
            encoder.WriteBoolean(true);
            encoder.WriteVInt(123); //  reward

            encoder.WriteVInt(200);
            ByteStreamHelper.WriteDataReference(encoder, 0);
            encoder.WriteVInt(0);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(Progress); // то что за батл крч получил
            encoder.WriteBoolean(false);
            encoder.WriteBoolean(true);
            encoder.WriteBoolean(false);
            encoder.WriteVInt(24);
            encoder.WriteVInt(Reward); // reward count
            encoder.WriteVInt(1719737999);
        }

        public void EncodeHome(ChecksumEncoder encoder)
        {
            encoder.WriteVInt(111);
            encoder.WriteVInt(22); // Brawl Pass seasn
            encoder.WriteVInt(MissionType); // Mission Type
            encoder.WriteVInt(CurrentGoal); // Achieved Goal
            encoder.WriteVInt(QuestGoal); // Quest Goal
            encoder.WriteVInt(-1);
            encoder.WriteVInt(1);
            encoder.WriteVInt(GameModeVariation); //Game Mode
            encoder.WriteVInt(0); // Refresh
            encoder.WriteVInt(-1); //EventSlot idk what this
            encoder.WriteVInt(1); //Reward Type
            encoder.WriteVInt(2);
            encoder.WriteVInt(Reward); // Reward count
            encoder.WriteVInt(0);
            encoder.WriteVInt(0);
            encoder.WriteVInt(1);
            encoder.WriteVInt(1);
            encoder.WriteVInt(1);
            encoder.WriteVInt(3);
            encoder.WriteVInt(0);
            encoder.WriteVInt(2);
            encoder.WriteVInt(1);
            encoder.WriteVInt(0); // current lvl
            encoder.WriteVInt(0); // max lvl
            encoder.WriteVInt(10000); // Timer
            encoder.WriteVInt(0); // Max LVL
            encoder.WriteVInt(0); //Timer
            encoder.WriteVInt(0);//Brawl Pass Need(1- Need);
            encoder.WriteVInt(0);
        }
    }
}
