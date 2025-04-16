using Supercell.Laser.Logic.Notification;
using Supercell.Laser.Titan.DataStream;

namespace Supercell.Laser.Logic.Home.Items
{
    public class Notification
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public bool IsViewed { get; set; }
        public int TimePassed { get; set; }
        public string MessageEntry { get; set; }
        public string PrimaryMessageEntry { get; set; }
        public string SecondaryMessageEntry { get; set; }
        public string ButtonMessageEntry { get; set; }
        public string FileLocation { get; set; }
        public string FileSha { get; set; }
        public string ExtLint { get; set; }
        public List<int> HeroesIds { get; set; }
        public List<int> HeroesTrophies { get; set; }
        public List<int> HeroesTrophiesReseted { get; set; }
        public List<int> StarpointsAwarded { get; set; }
        public int DonationCount;

        public int SkinID;

        public int BrawlerID;

        public int ResourceID;

        public int ResourceCount;

        public int RevokePersonHighID;
        public int RevokePersonLowID;
        public int RevokeCount;
        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(Id);
            stream.WriteInt(Index);
            stream.WriteBoolean(IsViewed);
            stream.WriteInt(TimePassed);
            stream.WriteString(MessageEntry);// base ||  FloaterTextNotification
            switch (Id)
            {
                case 2: // DonateNotification
                    stream.WriteString(null); // idk why i put here str, a2 + 28 
                    break;
                case 63: // ChallengeRewardNotification
                    // LogicReward::Encode start
                    stream.WriteVInt(0); // maybe count
                    // LogicGemOffer::encode start
                    stream.WriteVInt(1); // id
                    stream.WriteDataReference(0, 0); // character id
                    stream.WriteVInt(1); // skin id
                    stream.WriteVInt(1); // global id
                    // LogicGemOffer::encode end
                    // LogicReward::Encode end
                    stream.WriteVInt(0);
                    stream.WriteVInt(0);
                    stream.WriteString(null);
                    break;
                case 64: // BoxRewardNotification
                    stream.WriteVInt(0);
                    stream.WriteVInt(0);
                    stream.WriteVInt(0);
                    break;
                case 66: // FloaterTextNotification
                    break;
                case 67: // RankedMidSeasonRewardNotification
                    break; // todo
                case 68: // RankedSeasonEndNotification
                    /*
                     *   (*(void (__fastcall **)(ChecksumEncoder *, _DWORD))(*(_DWORD *)a2 + 64))(a2, *((_DWORD *)this + 10));
                         (*(void (__fastcall **)(ChecksumEncoder *, _DWORD))(*(_DWORD *)a2 + 64))(a2, *((_DWORD *)this + 11));
                         (*(void (__fastcall **)(ChecksumEncoder *, _DWORD))(*(_DWORD *)a2 + 64))(a2, *((_DWORD *)this + 12));
                         v4 = *(int (__fastcall **)(ChecksumEncoder *, int))(*(_DWORD *)a2 + 32);
                         if ( !*((_DWORD *)this + 13) )
                          return v4(a2, 0);
                            v4(a2, 1);
                         return LogicGemOffer::encode(*((LogicGemOffer **)this + 13), a2);
                    */
                    stream.WriteVInt(0);
                    stream.WriteVInt(0);
                    stream.WriteVInt(0);
                    if(false)
                        stream.WriteBoolean(false);
                    stream.WriteBoolean(true);
                    // LogicGemOffer::encode start
                    stream.WriteVInt(1); // id
                    stream.WriteDataReference(0, 0); // character id
                    stream.WriteVInt(1); // skin id
                    stream.WriteVInt(1); // global id
                    // LogicGemOffer::encode end
                    break;
                case 69: // BrawlPassAutoCollectSeasonNotification
                    break; // мне лень делать
                case 70: // ChallengeRewardNotification
                    break;
                case 71: // BrawlPassPointRewardNotification
                    break;
                case 72: // VanityItemRewardNotification
                    break;
                case 73: // BrawlPassRewardNotification
                    stream.WriteVInt(0);
                    break;
                case 75: // ChallengeSkinRewardNotification
                    stream.WriteVInt(0);
                    break;
                case 76: // QualifyNotification
                    break;
                case 77: // ProLeagueSeasonEndNotification
                    break;
                case 78: // RankRewardNotification
                    break;
                case 79: // StarPointsNotification
                    stream.WriteVInt(HeroesIds.Count);
                    for (int i = 0; i < HeroesIds.Count; i++)
                    {
                        stream.WriteVInt(HeroesIds[i]);
                        stream.WriteVInt(HeroesTrophies[i]);
                        stream.WriteVInt(HeroesTrophiesReseted[i]);
                        stream.WriteVInt(StarpointsAwarded[i]);
                    }
                    break;
                case 82: // BandNotification
                    break;
                case 83: // PromoPopupNotification
                    stream.WriteInt(0);
                    stream.WriteStringReference(PrimaryMessageEntry);
                    stream.WriteInt(0);
                    stream.WriteStringReference(SecondaryMessageEntry);
                    stream.WriteInt(0);
                    stream.WriteStringReference(ButtonMessageEntry);
                    stream.WriteStringReference(FileLocation);
                    stream.WriteStringReference(FileSha);
                    stream.WriteStringReference(ExtLint);
                    break;
                case 84: // StarPowerRewardNotification
                    break;
                case 85: // RevokeNotification
                    stream.WriteVInt(0); // type revork
                    stream.WriteVInt(RevokeCount);
                    stream.WriteInt(RevokePersonHighID);
                    stream.WriteInt(RevokePersonLowID);
                    stream.WriteVInt(0);
                    stream.WriteString("");
                    break;
                case 86: // IAPDeliveryNotification
                    break;
                case 88: // CoinDoublerRewardNotification
                    break;
                case 89: // GemRewardNotification
                    stream.WriteVInt(DonationCount);
                    break;
                case 90: // ResourceRewardNotification
                    stream.WriteVInt(0);
                    stream.WriteVInt(5000000 + ResourceID);
                    stream.WriteVInt(ResourceCount);
                    break;
                case 93: // HeroRewardNotification
                    stream.WriteVInt(0); // mb lvl
                    stream.WriteVInt(16000000 + BrawlerID);
                    break;
                case 94: // SkinRewardNotification
                    stream.WriteVInt(29000000 + SkinID);
                    break;
                default: // FreeTextNotification
                    stream.WriteVInt(0); 
                    break;
            }

        }
    }
}