﻿namespace Supercell.Laser.Logic.Util
{
    using System.Reflection.Metadata.Ecma335;
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Titan.Debug;

    public static class GameModeUtil
    {
        public static bool PlayersCollectPowerCubes(int variation)
        {
            int v1 = variation - 6;
            if (v1 <= 8)
                return ((0x119 >> v1) & 1) != 0;
            else
                return false;
        }

        public static int GetBattleTicks(int v)
        {
            int v2 = 2400;
            switch (v)
            {
                case 0:
                case 5:
                case 16:
                case 22:
                case 23:
                    v2 = 4200;
                    goto LABEL_9;
                case 3:
                case 7:
                case 8:
                    goto LABEL_9;
                case 6:
                case 9:
                case 10:
                case 12:
                case 13:
                case 18:
                case 20:
                    return BattleMode.NO_TIME_TICKS;
                case 14:
                    v2 = 9600;
                    goto LABEL_9;
                case 17:
                case 21:
                    v2 = 3600;
                    goto LABEL_9;
                default:
                    v2 = 20 * ((BattleMode.NORMAL_TICKS - BattleMode.INTRO_TICKS) / 20);
                LABEL_9:
                    return BattleMode.INTRO_TICKS + v2;
                    break;
            }
        }


        public static int GetRespawnSeconds(int variation)
        {
            switch (variation)
            {
                case 0:
                case 2:
                    return 3;
                case 3:
                    return 1;
                case 7:
                    return 3;
                default:
                    return 5;
            }
        }

        public static bool PlayersCollectBountyStars(int variation)
        {
            return variation == 3 || variation == 15;
        }

        public static bool HasTwoTeams(int variation)
        {
            return variation != 6 || variation != 15 || variation != 9;
        }

        public static bool HasTwoBases(int variation)
        {
            return variation == 2 || variation == 11;
        }

        public static int GetGameModeVariation(string mode)
        {
            switch (mode)
            {
                case "CoinRush":
                    return 0;
                case "GemGrab":
                    return 0;
                case "Heist":
                    return 2;
                case "BossFight":
                    return 7;
                case "Bounty":
                    return 3;
                case "Artifact":
                    return 4;
                case "LaserBall":
                    return 5;
                case "Showdown":
                    return 6;
                case "BigGame":
                    return 7;
                case "BattleRoyaleTeam":
                    return 9;
                case "Survival":
                    return 8;
                case "Raid":
                    return 10;
                case "RoboWars":
                    return 11;
                case "Tutorial":
                    return 12;
                case "Training":
                    return 13;
                case "LoneStar":
                    return 15;
                case "CTF":
                    return 16;
                case "TagTeam":
                    return 24;
                case "Invasion":
                    return 27;
                case "DeathmatchFFA":
                    return 28;
                case "LastStand":
                    return 29;
                case "ReachExit":
                    return 30;
                case "MapPrint":
                    return 99;
                default:
                    Debugger.Error("Wrong game mode!");
                    return -1;
            }
        }
    }
}
