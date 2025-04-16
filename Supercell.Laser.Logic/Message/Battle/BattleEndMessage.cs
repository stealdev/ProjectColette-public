namespace Supercell.Laser.Logic.Message.Battle
{
    using System.Numerics;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home.Quest;

    public class BattleEndMessage : GameMessage
    {
        public BattleEndMessage() : base()
        {
            ProgressiveQuests = new List<Quest>();
        }

        public int Result;
        public int TokensReward = 10000;
        public int TrophiesReward;
        public List<BattlePlayer> Players;
        public List<Quest> ProgressiveQuests;
        public BattlePlayer OwnPlayer;
        public bool StarToken;

        public int GameMode;
        public bool BattleWithoutTrophies;
        public bool IsPvP;

        public override void Encode()
        {
            Stream.WriteLong(OwnPlayer.AccountId);
            Stream.WriteLong(OwnPlayer.AccountId);

            Stream.WriteVInt(GameMode); // game mode
            Stream.WriteVInt(Result);
            Stream.WriteVInt(TokensReward); // tokens reward
            Stream.WriteVInt(TrophiesReward); // trophies reward
            Stream.WriteVInt(0);//Power Play Points Gained (Pro League Points)
            Stream.WriteVInt(0);//Doubled Tokens (Double Keys)
            Stream.WriteVInt(0);//Double Token Event (Double Event Keys)
            Stream.WriteVInt(0);//Token Doubler Remaining (Double Keys Remaining)
            Stream.WriteVInt(0);//game Lenght In Seconds
            Stream.WriteVInt(0);//Epic Win Power Play Points Gained (op Win Points)
            Stream.WriteVInt(0);//Championship Level Reached (CC Wins)

            Stream.WriteBoolean(false);//gen offer

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false);//164
            Stream.WriteBoolean(false);//165

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);//?
            Stream.WriteVInt(0);//v53
            Stream.WriteVInt(0);//v53

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false); // no experience
            Stream.WriteBoolean(false); // no tokens left
            Stream.WriteBoolean(BattleWithoutTrophies); // is PvP
            Stream.WriteBoolean(IsPvP);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);

            Stream.WriteVInt(-1);//ChallengeType
            Stream.WriteBoolean(false);

            Stream.WriteVInt(Players.Count);
            foreach (BattlePlayer player in Players)
            {
                Stream.WriteBoolean(player.AccountId == OwnPlayer.AccountId); // is own player
                Stream.WriteBoolean(player.TeamIndex != OwnPlayer.TeamIndex); // is enemy
                Stream.WriteBoolean(false); // Star player
                Stream.WriteVInt(1);
                {
                    ByteStreamHelper.WriteDataReference(Stream, player.CharacterId);
                }
                Stream.WriteVInt(1);
                {
                    ByteStreamHelper.WriteDataReference(Stream, player.SkinId);//skin
                }
                Stream.WriteVInt(1);
                {
                    Stream.WriteVInt(player.Trophies);
                }
                Stream.WriteVInt(1);
                {
                    Stream.WriteVInt(player.HeroPowerLevel);
                }
                Stream.WriteVInt(1);
                {
                    Stream.WriteVInt(0);
                }

                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                if (Stream.WriteBoolean(player.IsBot()==0))
                {
                    Stream.WriteLong(player.AccountId);
                }
                player.DisplayData.Encode(Stream);
                Stream.WriteBoolean(false);

                Stream.WriteVInt(0);
                Stream.WriteVInt(0);

                Stream.WriteInt16((short)player.Kills); // kills
                Stream.WriteInt16(0); // deaths
                Stream.WriteInt(player.Damage); // uron
                Stream.WriteInt(player.Heals); // heal
                // if uron < heal) write heal
                Stream.WriteDataReference(0);
            }

            Stream.WriteVInt(0);//xp
            Stream.WriteVInt(0);//dataref

            Stream.WriteVInt(2);
            {
                Stream.WriteVInt(1);
                Stream.WriteVInt(OwnPlayer.Trophies); // Trophies
                Stream.WriteVInt(OwnPlayer.HighestTrophies); // Highest Trophies

                Stream.WriteVInt(5);
                Stream.WriteVInt(100);
                Stream.WriteVInt(100);
            }

            ByteStreamHelper.WriteDataReference(Stream, OwnPlayer.Home.Thumbnail);

            if(Stream.WriteBoolean(true));//play again
            {
                Stream.WriteInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteInt(0);
                Stream.WriteInt(0);

            }
            if (Stream.WriteBoolean(true)) //quests
            {
                //if (Stream.WriteBoolean(ProgressiveQuests.Count > 0))
                //{
                //    Stream.WriteVInt(ProgressiveQuests.Count);
                //    foreach (Quest quest in ProgressiveQuests)
                //    {
                //        quest.EncodeBattleEnd(Stream);
                //    }
                //}
                Stream.WriteVInt(1);
                Stream.WriteVInt(85);
                Stream.WriteVInt(24); // brawl pass season
                Stream.WriteVInt(1);// type
                Stream.WriteVInt(1); // goal
                Stream.WriteVInt(2); // выполнено из
                Stream.WriteVInt(10); // reward
                ByteStreamHelper.WriteDataReference(Stream, 16000000); // brawler
                Stream.WriteVInt(6);
                Stream.WriteVInt(0);
                Stream.WriteVInt(-1); // reward type
                Stream.WriteVInt(1);
                Stream.WriteVInt(0);
                Stream.WriteBoolean(true);
                Stream.WriteBoolean(true);
                Stream.WriteVInt(123); //  reward

                Stream.WriteVInt(200);
                ByteStreamHelper.WriteDataReference(Stream, 0);
                Stream.WriteVInt(0);
                Stream.WriteBoolean(false);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(123); // то что за батл крч получил
                Stream.WriteBoolean(false);
                Stream.WriteBoolean(true);
                Stream.WriteBoolean(false);
                Stream.WriteVInt(24);
                Stream.WriteVInt(12312); // reward count
                Stream.WriteVInt(1719737999);

                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);


            }
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);//v53
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);//v53
            Stream.WriteBoolean(false);//ranked match state
            Stream.WriteVInt(-1);
            Stream.WriteBoolean(false);//chronosTextEntry
            //{
            //    Stream.WriteInt(0);
            //    Stream.WriteString("507");
            //}
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);//v53
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);//v53
            Stream.WriteBoolean(false);//v53
        }

        public override int GetMessageType()
        {
            return 23456;
        }

        public override int GetServiceNodeType()
        {
            return 27;
        }
    }
}
