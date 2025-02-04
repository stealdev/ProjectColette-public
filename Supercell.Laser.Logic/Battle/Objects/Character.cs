using System.Reflection.Metadata.Ecma335;

namespace Supercell.Laser.Logic.Battle.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.SymbolStore;
    using System.Globalization;
    using System.Net.Http.Headers;
    using System.Numerics;
    using System.Reflection.Metadata;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;
    using System.Security.Cryptography;
    using System.Xml.Linq;
    using DeenGames.Utils;
    using DeenGames.Utils.AStarPathFinder;
    using Masuda.Net.Models;
    using Supercell.Laser.Logic.Battle.Component;
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;
    using static System.Net.Mime.MediaTypeNames;
    public class DamageNumber
    {
        public int Delay;
        public int Value;
        public int Dealer;
        public DamageNumber(int value, int dealerIndex, int delay)
        {
            Dealer = dealerIndex;
            Value = value;
            Delay = delay;
        }
    }

    public class Character : GameObject
    {
        public const int MAX_HOLD_TICKS = 15;
        public const int INTRO_TICKS = 180;
        public const int DAMAGE_NUMBERS_DELAY_TICKS = 2;
        public const int DEPLOY_TICKS_FIRST_SPAWN = 4;
        public const int DEPLOY_TICKS = 19;
        public int ForcedAngleTicks;
        public CardData StarPowerData => GetPlayer() == null ? null : GetPlayer().StarPowerData;
        public int m_hitpoints;
        public int m_maxHitpoints;
        public int OrginalMaxHP;
        public int MapHeight => GameObjectManager.GetBattle().GetTileMap().LogicHeight;
        public int MapWidth => GameObjectManager.GetBattle().GetTileMap().LogicWidth;
        public int int344;
        public int dword348;
        public int State;
        public int MoveAngle;
        public int AttackAngle;
        public bool UltiSkill;
        public bool IsControlled;
        public int Shaking;
        public bool IsInvincible;
        public bool m_usingUltiCurrently;
        public bool ShouldExecuteColetteChargeOnReturn;
        public LogicVector2 WayPoint2 = new LogicVector2();
        public LogicVector2 WayPoint;
        public LogicVector2 WayPointStart;
        public LogicVector2 PosDelta;
        public LogicVector2 MovementVector = new LogicVector2();
        public int MoveStart;
        public int MoveEnd;
        public int PathLength;
        private int HealthRegenBlockedTick;
        public int m_lastSelfHealTick;
        public int BulletIndex;
        public int PrevY;
        public int PrevX;
        public bool YellowEye = false;
        public int VisibilityState = 0;
        public int InvisibleTicks = 0;
        public bool IsInvisible = false;
        public int SeeInvisibilityDistance = 0;
        public bool BreakInvisibilityOnAttack = false;
        public int SpawnTick;
        public AreaEffectData ChargeAreaEffectData;
        public bool KnockBacked;
        public bool Knockback1;
        public bool Knockback2;
        public bool Knockback3;
        public int ExtraRegenAmount;
        public int ExtraRegenTicks;
        public int ExtraRegenerIndex;
        public int HealthRegenedTick;
        public int HealthRegenedTickSP;
        public int ReloadBuffTicks;
        public AreaEffect AreaEffect;
        public int ChargeUpType;
        public int ChargeUp;
        public int ChargeUpMax;
        public bool MeleeAttackPushBacks;
        public int MeleeAttackPushBack;
        public int ConsumableShield;
        public int ConsumableShieldMax;
        public int ConsumableShieldTick;
        public int ConsumableShieldDuration;
        private int BlinkX;
        private int BlinkY;
        private GameObject DraggingObject;
        private int DraggingOffsetX;
        private int DraggingOffsetY;
        public bool StarPowerIC;
        public int AutoAttackDamangeBoost;
        public int DeployedTick;
        public int StarPowerProjectileTimer;
        public int ParentGID;
        public int HitEffectAngle;
        public int HitEffectProjectile;
        public int HitEffectSkin;
        public int StaticSpeedBuff;
        public int AoeRegenerate;
        public bool IsCrippled;
        public int CripplePercent;
        public int CrippleEndTick;
        public bool ChargeBreaksEnvironment;
        public int CastingTime;
        public bool TransformAnimation;
        public int SteerAngle;
        public int SteerStartTicks;
        public int AutoUltiChargeTimer;
        public int CcImmunityTicks;
        public int ChargedShotCount;
        public int ChargedShotMisses;
        public int RunningChargingUlti;
        public bool IsWeaklyStunned;
        public int JesterSkillCount;
        public int ItemPickedUpTick;
        public int UltiButtonPressedTimes;
        public bool DisplayUltiTmp;
        public int ViusalChargeType;
        public int VisionOverrideX;
        public int VisionOverrideY;
        public int VisionOverrideTicks;
        public int AshLastRageTick;
        public bool Attacking;
        public Character AutoAttackTarget;

        public int SkillHoldTicks;
        public int RapidFireTick;
        public List<Buff> m_buffs;
        public List<DamageNumber> DamageNumbers;
        public List<Ignoring> ChargeHitGIDs;
        public List<int> FangHitGIDs;
        public Accessory Accessory => GetPlayer() == null ? null : GetPlayer().Accessory;
        public bool IsTeleporting;
        public BattlePlayer PreviousPlayer;

        public bool Ultiing;
        public List<Skill> m_skills;
        public List<Immunity> Immunitys;

        private int m_itemCount;

        public AreaEffect RealmEffect;
        public CharacterData SummonedCharacter;
        public ItemData SpawnedItem;
        public int NumSpawns;
        public int NumSpawnsItem;
        public int MaxSpawns;
        public int MaxSpawnsItem;
        public int SpawnDamage;
        public int SpawnedItemDamage;
        public int SpawnedItemNormalDMG;
        public int SpawnHealth;

        private int m_heroLevel;
        public int LeftHand;
        public int AttackOffsetX;
        public int AttackOffsetY;

        public int AttackOffset2X;
        public int AttackOffset2Y;

        public LogicVector2 AttackVector;
        public LogicVector2 AttackTarget;
        public int ProjectileShieldAngle;
        public int ProjectileShieldScale;
        public int ProjectileShieldCoolDown;

        public int RapidFireX;
        public int RapidFireY;
        public int RapidFireDamage;
        public int RapidFireNormalDMG;
        public int RapidFireAttackPattern;
        public int RapidFireRange;
        public int RapidFireSpread;
        public ProjectileData RapidFireProjectile;
        public int RapidFireBulletsPerShoot;
        public int RapidFireMsBetweenAttacks;
        public int RapidFireShootTimes;
        public int ShootIndex;

        public int slowingModifier;
        public int AcceleratingModifier;
        public int DamageModifier;

        public int RealmTimer;

        public int m_isBot;
        public int m_ticksSinceBotEnemyCheck = 100;
        public int m_lastAIAttackTick;
        public int m_lastUltiTick;

        private Character m_closestEnemy;
        private LogicVector2 m_closestEnemyPosition;

        public int m_activeChargeType;
        public int ChargeHits;
        public int FangUltiCount;
        public bool IsCharging;
        public int ChargeSpeed;
        public int ChargePushback;
        public int ChargeAnimation;
        public int ChargeDamage;
        public int ChargeNormalDMG;
        public int ChargePercentDamage;

        private int m_stunTicks;

        private int m_destructafterticks;

        private int m_damageMultiplier;
        private int m_lastTileDamageTick;

        private int ShieldTicks;
        private int ShieldPercent;

        private int m_attackingTicks;
        public int LastAttckTick;


        public List<Poison> Poisons;
        public int PartialStunPromille;
        public int TicksSincePartialStun;

        public LogicVector2 m_chargestart;

        public AreaEffect CharacterAreaEffect;
        public AreaEffect tms;

        public int TransformationState;
        public bool IsRage;
        public int PassiveCounter;
        public int PassiveCounter2;

        public Gear Gear1;
        public Gear Gear2;
        public CharacterData CharacterData;

        public Character(CharacterData characterData) : base(characterData)
        {
            CharacterData = characterData;

            DamageNumbers = new List<DamageNumber>();
            m_skills = new List<Skill>();
            m_buffs = new List<Buff>();
            PosDelta = new LogicVector2();
            WayPoint = new LogicVector2();
            WayPointStart = new LogicVector2();
            AttackTarget = new LogicVector2();
            AttackVector = new LogicVector2();
            MoveStart = 0;
            MoveEnd = 0;
            m_maxHitpoints = CharacterData.Hitpoints;
            m_hitpoints = m_maxHitpoints;
            OrginalMaxHP = m_maxHitpoints;
            BlinkX = -1;
            Poisons = new List<Poison>();
            Immunitys = new List<Immunity>();
            IsControlled = false;
            RealmTimer = 0;
            State = 4;
            UltiSkill = false;
            m_destructafterticks = 5;
            SteerAngle = -1;
            SkillHoldTicks = -1;
            VisionOverrideX = -1;
            if (WeaponSkillData != null)
            {

                m_skills.Add(new Skill(WeaponSkillData.GetGlobalId(), false));
                if (m_isBot == 2) m_skills[0].Infinity = true;
                //m_skills[0].Level = m_heroLevel;
                //Debugger.Print(m_skills[0].SkillId.ToString());
            }
            if (UltimateSkillData != null)
            {
                m_skills.Add(new Skill(UltimateSkillData.GetGlobalId(), true));

                //m_skills[1].Level = m_heroLevel;

            }
            //Abi2

            if (CharacterData.Name == "Mummy") TransformationState = 0;



            m_activeChargeType = -1;
            PassiveCounter = 1;
            PassiveCounter2 = 4;
        }
        public int GetMaxHoldTicks()
        {
            int v2 = GetCardValueForPassive("faster_focus", 1);
            if (v2 >= 1) return 13 - 13 * v2 / 100;
            return 13;
        }
        public int GetMaxChargedShots()
        {
            if (WeaponSkillData?.ChargedShotCount >= 1) return WeaponSkillData.ChargedShotCount;
            if (CharacterData.UniqueProperty == 10) return CharacterData.UniquePropertyValue1;
            return -1;
        }
        public void PushOutOfWalls()
        {
            if (!IsSteerMovementActive() && DraggingObject == null && !Knockback3 && !Knockback1 && !Knockback2 && (!IsCharging || !GamePlayUtil.IsJumpCharge(m_activeChargeType)))
            {
                TileMap tileMap = GetBattle().GetTileMap();
                if (!tileMap.IsPassable(1, GetX() / 300, GetY() / 300, 0, 0))
                {
                    TriggerStun(1, true);
                    GamePlayUtil.GetClosestPassableSpot(1, GetX(), GetY(), 10, tileMap, PosDelta, true);
                    TriggerPushback(
                        GetX() - PosDelta.X + GetX(),
                        GetY() - PosDelta.Y + GetY(),
                        LogicMath.Max(GamePlayUtil.GetDistanceBetween(GetX(), GetY(), PosDelta.X, PosDelta.Y), 300) / 6,
                        true, 0, true, false, false, true, false, true, false, 0);

                }
            }
        }
        public void SetVisionOverride(int X, int Y, int ticks)
        {
            VisionOverrideX = X;
            VisionOverrideY = Y;
            VisionOverrideTicks = ticks;
            StopMovement();
        }
        public void AddCcImmunity(int ticks)
        {
            for (int i = 0; i < m_buffs.Count; i++)
            {
                Buff buff = m_buffs[i];
                if (buff.Type == 3)
                {
                    m_buffs.Remove(buff);
                    i--;
                }
            }
            Knockback3 = false;
            KnockBacked = false;
            CcImmunityTicks = ticks;
            StopMovement();
        }
        public static Character SummonMinion(
        CharacterData CharacterData,
        int X,
        int Y,
        int a4,
        int NumSpawn,
        int MaxSpawn,
        int ExtraDamage,
        int ExtraHealth,
        BattleMode a9,
        int a10,
        int a11,
        int a12,
        bool OverridePrevious,
        bool a14,
        int a15,
        bool StarPowerIC,
        int a17)
        {
            //_DWORD* v17; // r0
            int v18; // r4
            //_DWORD* v19; // r5
            int v20; // r10
            int v21; // r6
            int v22; // r8
            int v23; // r9
            int v24; // r0
            bool v25; // zf
            int v26; // r9 MAPDST
            bool v27; // cc
            int v29; // r10
            Character v30; // r6
            int v31; // r0
            int v33; // r8
            int v34; // r9
            int v35; // r0
            int HP; // r8
            int v37; // r9
            int v38; // r0
            int v39; // r2
            Character v40; // r6
            int v41; // r0 MAPDST
            int v42; // r10
            int v43; // r7
            //_DWORD* v47; // r8
            int v48; // r4
            int v49; // r6
            int v50; // r4
            TileMap v51; // r0
            int v52; // r5
            int v53; // r0
            int v54; // r6
            int v55; // r4
            int v56; // r6
            //_DWORD* v57; // r0
            char v58; // r4
            int v59; // r9
            int v60; // r6
            int v61; // r0
            int v62; // r8
            int v63; // r0
            int v64; // r2
            //unsigned int v65; // r0
            bool v66; // cf
            int v67; // r9
            int i; // r4
            int v69; // r0
            //_DWORD* v70; // r6
            int v71; // r0
            bool v72; // zf
            int v74; // r4
            int v75; // r0
            int v76; // r0
            int v77; // r2
            int v78; // r1
            int v79; // r0
            int v80; // r4
            int v81; // r8
            //__int64 v82; // r0
            int v83; // r0
            int v85; // r0
            bool v86; // zf
            Character v90; // [sp+34h] [bp-3Ch]
            int v95; // [sp+48h] [bp-28h]
            int v97; // [sp+50h] [bp-20h]
            int v98; // [sp+50h] [bp-20h]
            int v99; // [sp+50h] [bp-20h]
            int v100; // [sp+50h] [bp-20h]

            //ZNK28LogicGameObjectManagerServer14getGameObjectsEv();
            //v18 = v17[2];
            //v19 = v17;
            v26 = NumSpawn;
            if (a9.GetGameObjectManager().GetGameObjects().Length < 1)
            {
                v97 = 0;
            }
            else
            {
                v20 = 0;
                v97 = 0;
                foreach (Character character in a9.GetGameObjectManager().GetCharacters())
                {
                    if (character.GetIndex() == a11 * 16 + a10 && character.CharacterData == CharacterData) v97++;
                }
            }
            if (OverridePrevious)
            {
                if (v97 >= MaxSpawn)
                {
                    foreach (Character character in a9.GetGameObjectManager().GetCharacters())
                    {
                        if (character.GetIndex() == a11 * 16 + a10 && character.CharacterData == CharacterData)
                        {
                            character.CauseDamage(null, character.m_hitpoints, 0, false, null, false, true);
                            --v97;
                        }
                    }
                    while (v97 >= MaxSpawn) ;
                    v26 = NumSpawn;
                }
            }
            else if (MaxSpawn - v97 < NumSpawn)
            {
                v26 = MaxSpawn - v97;
            }
            if (v26 < 1)
                return null;
            v40 = null;
            v41 = 0;
            do
            {
                if (NumSpawn == 5)
                {
                    v48 = v41 * 72 - 18;
                    a4 = 600;
                    v49 = LogicMath.GetRotatedX(a4, 0, v48);
                    v50 = LogicMath.GetRotatedY(a4, 0, v48);
                    v51 = a9.GetTileMap();
                    v52 = LogicMath.Clamp(v49 + X, 101, v51.LogicWidth - 101);
                    v54 = LogicMath.Clamp(v50 + Y, 101, v51.LogicHeight - 101);
                    v74 = v54;
                    v67 = v52;
                }
                //    v42 = v19[2];
                //    v43 = 0;
                //    while (1)
                //    {
                //        v47 = v19;
                //        v48 = sub_1163C0(a9, 360);
                //        v49 = ZN9LogicMath11getRotatedXEiii(a4, 0, v48);
                //        v50 = ZN9LogicMath11getRotatedYEiii(a4, 0, v48);
                //        v51 = ZN21LogicBattleModeServer10getTileMapEv(a9);
                //        v52 = ZN9LogicMath5clampEiii(v49 + X, 101, *(v51 + 104) - 101);
                //        v53 = ZN21LogicBattleModeServer10getTileMapEv(a9);
                //        v54 = ZN9LogicMath5clampEiii(v50 + Y, 101, *(v53 + 108) - 101);
                //        v55 = ZN12LogicTileMap21logicToPathFinderTileEi(v52);
                //        v98 = v54;
                //        v56 = ZN12LogicTileMap21logicToPathFinderTileEi(v54);
                //        v57 = ZN21LogicBattleModeServer10getTileMapEv(a9);
                //        if (ZNK12LogicTileMap20isPassablePathFinderEiiibb(v57, 1, v55, v56, 0, 0))
                //            break;
                //        v19 = v47;
                //    LABEL_47:
                //        if (++v43 >= 0x14)
                //        {
                //            v64 = -1;
                //            v95 = -1;
                //            goto LABEL_50;
                //        }
                //    }
                //    v95 = v52;
                //    if (v42 >= 1)
                //    {
                //        v19 = v47;
                //        v58 = 1;
                //        v59 = 0;
                //        while (1)
                //        {
                //            v60 = *(*v19 + 4 * v59);
                //            if (!(*(*v60 + 32))(v60) && ZNK21LogicGameObjectServer7getDataEv(v60) == CharacterData)
                //            {
                //                if ((*(*v60 + 48))(v60))
                //                {
                //                    v61 = ZNK21LogicGameObjectServer4getXEv(v60);
                //                    v62 = (v95 - v61) * (v95 - v61);
                //                    v63 = ZNK21LogicGameObjectServer4getYEv(v60);
                //                    v64 = v98;
                //                    v65 = v62 + (v98 - v63) * (v98 - v63);
                //                    v58 &= v65 > 0x57E3;
                //                    v66 = v65 >= 0x57E4;
                //                    if (v65 < 0x57E4)
                //                        break;
                //                }
                //            }
                //            if (++v59 >= v42)
                //            {
                //                v64 = v98;
                //                goto LABEL_44;
                //            }
                //        }
                //        if (!v66)
                //            a4 = 151;
                //        LABEL_44:
                //        if ((v58 & 1) != 0)
                //            goto LABEL_50;
                //        goto LABEL_47;
                //    }
                //    v19 = v47;
                //    v64 = v98;
                //LABEL_50:
                //    v67 = v95;
                //    v99 = v64;
                //    if (!(~v95 | a4))
                //    {
                //        v67 = -1;
                //        if (v42 >= 1)
                //        {
                //            v67 = -1;
                //            for (i = 0; i != v42; ++i)
                //            {
                //                v70 = *(*v19 + 4 * i);
                //                v71 = (*(*v70 + 32))(v70);
                //                v72 = v71 == 0;
                //                if (!v71)
                //                    v72 = v70[10] == a10;
                //                if (v72)
                //                {
                //                    v69 = ZNK21LogicGameObjectServer7getDataEv(v70);
                //                    if (ZNK18LogicCharacterData6isHeroEv(v69))
                //                    {
                //                        v67 = ZNK21LogicGameObjectServer4getXEv(v70);
                //                        v99 = ZNK21LogicGameObjectServer4getYEv(v70);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //v74 = v99;
                //if (v67 == -1)
                //    v74 = Y;
                //if (v67 == -1)
                //    v67 = X;
                else
                {
                    v74 = Y;
                    v67 = X;
                }
                LogicVector2 tmp = new LogicVector2();
                GamePlayUtil.GetClosestPassableSpot(0, v67, v74, 10, a9.GetTileMap(), tmp, false);

                v40 = GameObjectFactory.CreateGameObjectByData(CharacterData);
                v75 = CharacterData.FlyingHeight;
                v100 = v74;
                v40.SetPosition(tmp.X, tmp.Y, v75);
                v40.SetIndex(a11 * 16 + a10);
                v76 = CharacterData.AutoAttackDamage;
                v77 = v40.AutoAttackDamangeBoost;
                v78 = v40.m_maxHitpoints + ExtraHealth;
                v40.m_hitpoints = v78;
                if (v76 > 0)
                    v77 += ExtraDamage;
                v40.m_maxHitpoints = v78;
                //v40->gap98 = v78;
                v40.AutoAttackDamangeBoost = v77;
                //v79 = ZNK28LogicGameObjectManagerServer17getGameObjectByIDEi(*a9);
                //if (v79)
                //{
                //    v80 = v79;
                //    v81 = v67 - ZNK21LogicGameObjectServer4getXEv(v79);
                //    HIDWORD(v82) = v100 - ZNK21LogicGameObjectServer4getYEv(v80);
                //    LODWORD(v82) = v81;
                //    v83 = ZN9LogicMath8getAngleEii(v82);
                //    v40->MoveAngle = v83;
                //    v40->AttackAngle = v83;
                //}
                //else
                //{
                //    v85 = *v40->TeamIndex;
                //    v86 = v85 == 1;
                //    v40->MoveAngle = 270;
                //    if (v85 == 1)
                //        v85 = 90;
                //    v40->AttackAngle = 270;
                //    if (v86)
                //    {
                //        v40->MoveAngle = v85;
                //        v40->AttackAngle = v85;
                //    }
                //}
                v40.DeployedTick = a9.GetTicksGone();
                v40.ParentGID = a12;
                a9.GetGameObjectManager().AddGameObject(v40);
                v40.AddAreaEffect(ExtraDamage, a17, null, 0, false);
                //*&v40->gap32E[18] = a15;
                //v40->gap328[4] = a14;
                //v40->UltiSkill = 1;
                v40.StarPowerIC = StarPowerIC;
                ++v41;
            }
            while (v41 != v26);
            return v40;
        }
        public void SetUpgrades(int a2)
        {
            SetHeroLevel(a2);
            CalculateChargeUp();
            if (StarPowerData != null)
            {
                if (GetCardValueForPassive("aoe_regenerate", 1) != -1)
                {
                    AoeRegenerate = GetCardValueForPassive("aoe_regenerate", 1);
                }
                if (GetCardValueForPassive("speed", 1) != -1)
                {
                    StaticSpeedBuff += GetCardValueForPassive("speed", 1);
                }
                if (GetCardValueForPassive("gain_health", 1) != -1)
                {
                    m_maxHitpoints += GetCardValueForPassive("gain_health", 1);
                    m_hitpoints = m_maxHitpoints;
                }
                if (GetCardValueForPassive("extra_bullet", 1) != -1)
                {
                    m_skills[0].MaxCharge += 1000 * GetCardValueForPassive("extra_bullet", 1);
                    m_skills[0].Charge = m_skills[0].MaxCharge;
                }
                StarPowerIC = true;
            }
        }
        public Character TriggerTransforming(CharacterData a2)
        {

            Character c = GetBattle().SpawnHero(a2, 11, GetIndex(), 0, false);
            //c.ObjectGlobalId = GetGlobalID();
            c.SetPosition(GetX(), GetY(), 0);
            c.Gear1 = new Gear(GetPlayer().Gear1);
            c.Gear2 = new Gear(GetPlayer().Gear2);
            if (CharacterData.UniqueProperty == 7) c.m_hitpoints = m_hitpoints;
            c.m_skills[0].Charge = m_skills[0].Charge;
            c.BlockHealthRegen();
            GetPlayer().OwnObjectId = c.GetGlobalID();
            if (CharacterData.UniqueProperty != 2)
                CauseDamage(this, 99999, 0, false, null, false, true);
            GameObjectManager.RemoveGameObject(this);
            TransformAnimation = true;
            return c;
        }
        public void UltiButtonPressedTwice()
        {
            if (IsCharging && m_activeChargeType == 1)
            {
                UltiButtonPressedTimes = 0;
                StopMovement();
                InterruptAllSkills(true);
            }
        }
        public void CalculateChargeUp()
        {
            int result;
            if (m_skills.Count >= 2)
            {
                if (m_skills[0].SkillData.ChargeSpeed > 0 && m_skills[0].SkillData.ChargeType == 0)
                {
                    ChargeUpType = 1;
                    ChargeUpMax = m_skills[0].SkillData.ChargeSpeed;
                    return;
                }
                if (m_skills[1].SkillData.ChargeType == 15)
                {
                    ChargeUpType = 7;
                    ChargeUpMax = m_skills[1].SkillData.ActiveTime;
                    ChargeUp = 0;
                }
            }
            if (GetCardValueForPassive("medikit", 1) >= 0)
            {
                ChargeUpType = 2;
                ChargeUpMax = GetCardValueForPassive("medikit", 2);
                return;
            }
            //if (GetCardValueForPassive("longer_dash", 1) >= 0)
            //{
            //    ChargeUpType = 1;
            //    ChargeUpMax = GetCardValueForPassive("longer_dash", 2);
            //    return;
            //}
            if (GetCardValueForPassive("grow_from_damage", 1) >= 0)
            {
                ChargeUpType = 3;
                ChargeUpMax = GetCardValueForPassive("grow_from_damage", 0);
                return;
            }
            if (GetCardValueForPassive("armor", 1) >= 0)
            {
                ChargeUpType = 4;
                ChargeUpMax = GetCardValueForPassive("armor", 1);
                ChargeUp = ChargeUpMax;
                return;
            }
            if (GetCardValueForPassive("increased_explosion", 1) >= 0)
            {
                ChargeUpType = 6;
                ChargeUpMax = GetCardValueForPassive("increased_explosion", 1);
                return;
            }
            if (GetCardValueForPassive("attack_pierce", 1) >= 0)
            {
                ChargeUpType = 6;
                ChargeUpMax = GetCardValueForPassive("attack_pierce", 1) * 50;
                return;
            }
            if (GetCardValueForPassive("explosion_damage_increase", 1) >= 0)
            {
                ChargeUpType = 6;
                ChargeUpMax = GetCardValueForPassive("explosion_damage_increase", 1);
                return;
            }
            if (GetCardValueForPassive("reduce_damage_periodically", 1) >= 0)
            {
                ChargeUpType = 2;
                ChargeUpMax = GetCardValueForPassive("reduce_damage_periodically", 1) * 50;
                return;
            }
            if (GetCardValueForPassive("spawn_pet_on_hit", 1) >= 0)
            {
                ChargeUpType = 6;
                ChargeUpMax = GetCardValueForPassive("spawn_pet_on_hit", 1) * 50;
                return;
            }
            switch (CharacterData.UniqueProperty)
            {
                case 1:
                    ChargeUpMax = 1000;
                    ChargeUpType = 8;
                    break;
                case 3:
                    ChargeUpType = 1;
                    ChargeUpMax = CharacterData.UniquePropertyValue1 * 50;
                    result = GetCardValueForPassive("faster_long_dash", 1);
                    if (result >= 1) ChargeUpMax -= 50 * result;
                    break;
                case 10:
                    ChargeUpType = 9;
                    ChargeUpMax = CharacterData.UniquePropertyValue1;
                    break;
                case 12:
                    ChargeUpType = 10;
                    ChargeUpMax = CharacterData.UniquePropertyValue1;
                    break;
                case 17:
                    ChargeUpType = 6;
                    ChargeUpMax = CharacterData.UniquePropertyValue1;
                    break;
            }
            if (GetPlayer() != null && GetPlayer().Accessory != null && GetPlayer().Accessory.AccessoryData.ShowCountdown)
            {
                ChargeUpType = 7;
                ChargeUpMax = GetPlayer().Accessory.AccessoryData.ActiveTicks * 50;
            }
        }
        public void AddExtraHealthRegen(
        int a2,
        int a3,
        int a4)
        {
            ApplyBuff(10, a3, a2, 0, a4);
            //if (a3 / 20 * a2 > ExtraRegenTicks / 20 * ExtraRegenAmount)
            //{
            //    ExtraRegenAmount = a2;
            //    ExtraRegenTicks = a3;
            //    ExtraRegenerIndex = a4;
            //}
        }
        public void SetStartRotation(int angle)
        {
            MoveAngle = angle;
            AttackAngle = angle;
        }
        public void ApplyPoison(int SourceIndex, int Damage, int NormalDMG, bool IsUlti, Character Source, int Type)//5 Electronic 6 Eve 8 IceAmber
        {
            if (Poisons.Count >= 1)
            {
                foreach (Poison poison in Poisons)
                {
                    if (poison.SourceIndex == SourceIndex && !poison.AllowStacking())
                    {
                        poison.RefreshPosion(Damage, NormalDMG, IsUlti, Type);
                        return;
                    }
                }
            }
            Poison poison1 = new Poison(Damage, NormalDMG, IsUlti, Source, SourceIndex, Type);
            Poisons.Add(poison1);
        }
        public void TriggerShake()
        {
            Shaking = 6;
        }
        public bool IsChargeUpReady()
        {
            return ChargeUp >= ChargeUpMax;
        }

        public void AddShield(int time, int percentage)
        {
            ShieldPercent = percentage;
            ShieldTicks = time;
        }

        public void SetBot(int isbot)
        {
            m_isBot = isbot;
        }


        public SkillData WeaponSkillData => DataTables.Get(DataType.Skill).GetData<SkillData>(CharacterData.WeaponSkill);
        public SkillData UltimateSkillData => DataTables.Get(DataType.Skill).GetData<SkillData>(CharacterData.UltimateSkill);


        public void ApplyItem(Item logicItem)
        {
            switch (logicItem.ItemData.Name)
            {
                case "SoulCollectorSoul":
                    Heal(logicItem.GetIndex(), logicItem.Damage, true, logicItem.ItemData);
                    break;
                case "BattleRoyaleBuff":
                    if (GameObjectManager.GetBattle().GetGameModeVariation() == 6)
                    {
                        m_itemCount++;
                    }
                    int delta = ((int)(((float)10 / 100) * (float)CharacterData.Hitpoints));
                    m_maxHitpoints += delta;
                    m_hitpoints = LogicMath.Min(m_hitpoints + delta, m_maxHitpoints);
                    m_damageMultiplier++;
                    break;
                case "Money":
                    BattlePlayer player = GameObjectManager.GetBattle().GetPlayer(GetGlobalID());
                    if (player != null)
                    {
                        player.AddScore(1);
                    }
                    break;
                case "Point":
                    player = GameObjectManager.GetBattle().GetPlayer(GetGlobalID());
                    if (player != null)
                    {
                        m_itemCount++;
                        player.AddScore(1);
                    }
                    break;
            }
        }
        public static int TransformPushBackStrengthToLength(int a1)
        {
            return 6 * LogicMath.Abs(a1);
        }
        public static int TransformPushBackLengthToStrength(int a1)
        {
            return a1 / 6;
        }
        public void GiveReloadBuff(int a2)
        {
            ReloadBuffTicks = a2;
        }
        public void SetDraggingObject(GameObject a2, int a3, int a4)
        {
            if (HasCcImmunity() || IsInvincible || IsCharging) return;
            DraggingObject = a2;
            DraggingOffsetX = a3;
            DraggingOffsetY = a4;
            MoveStart = TicksGone - 1;
            MoveEnd = TicksGone - 1;
            InterruptAllSkills(false);
        }
        private void TickSkills()
        {
            CastingTime = LogicMath.Max(0, CastingTime - 50);
            for (int i = 0; i < m_skills.Count; i++)
            {
                m_skills[i].Tick(this);
            }
        }
        private void TickAutoUltiCharge()
        {
            if (State != 4)
            {
                int v1 = CharacterData.ChargeUltiAutomatically / 4;
                if (v1 >= 1)
                {
                    AutoUltiChargeTimer--;
                    if (AutoUltiChargeTimer <= 0)
                    {
                        if (GetPlayer() != null)
                        {
                            if (Accessory != null && Accessory.Type == "auto_super_load_boost" && Accessory.IsActive)
                            {
                                v1 += v1 * Accessory.AccessoryData.CustomValue1 / 100;
                            }
                            ChargeUlti(v1, false, true, GetPlayer(), this);
                        }
                        AutoUltiChargeTimer = 5;
                    }
                }
            }
        }
        public override void Tick()
        {
            if (IsAlive())
            {
                TickTimers();
                TickStarPowers();
                TickGears();
                ExecuteBlink();
                if (GetBattle().StoryMode == null)
                {
                    PushOutOfWalls();
                }
                TickSkills();
                TickInvisibility();
                if (GetBattle().StoryMode == null) if (CharacterData.IsHero()) TickHeals();
                HandleMoveAndAttack();
                TickEffects();
                if (GetBattle().StoryMode == null)
                {
                    TickAutoUltiCharge();
                }
                TickTile();
                if (TicksGone > INTRO_TICKS) TickAI();

                if (AreaEffect != null)
                {
                    AreaEffect.SetPosition(GetX(), GetY(), 0);
                }
            }
            else m_destructafterticks--;

            //tms.SetPosition(Position.X, Position.Y, 0);
            //Debugger.Print(m_activeChargeType.ToString());
            //if (CharacterData.Name == "Mummy")
            //{
            //    if
            //}

        }
        public void SetImmunity(int TargetIndex, int Ticks, LogicData TargetData)
        {
            Immunity immunity = new Immunity(TargetIndex, TargetData, Ticks);
            Immunitys.Add(immunity);
        }
        public bool HasBuff(int Type)
        {
            foreach (Buff buff in m_buffs)
            {
                if (buff.Type == Type) return true;
            }
            return false;
        }

        public Buff GetBuff(int Type)
        {
            foreach (Buff buff in m_buffs)
            {
                if (buff.Type == Type) return buff;
            }
            return null;
        }

        public int GetBuffedSpeed()
        {
            int v1 = 0;
            foreach (Buff buff in m_buffs)
            {
                if (buff.Type <= 6 && ((1 << buff.Type) & 88) != 0) // 3 4 6
                    v1 += buff.Modifier;
            }
            return v1;
        }
        public int GetUnbuffedSpeed()
        {
            int v1 = 0;
            v1 += StaticSpeedBuff;
            if (CharacterData.HasPowerLevels && GetPowerLevel() > 0) v1 += 170;
            if (CharacterData.UniqueProperty == 1) v1 += 60 * GetPowerLevel();
            if (GetBattle().HasEventModifier(14)) v1 += 350;
            if (GetPlayer()?.OverCharging ?? false)
            {
                if (CharacterData.OverchargeSpeedPercent > 0)
                    v1 += CharacterData.OverchargeSpeedPercent * CharacterData.Speed / 100;
                else v1 += 20 * CharacterData.Speed / 100;
            }
            return v1;
        }
        public void TickEffects()
        {
            List<Immunity> ToRemove2 = new List<Immunity>();
            foreach (Immunity immunity in Immunitys)
            {
                if (immunity.Tick(50)) ToRemove2.Add(immunity);
            }
            foreach (Immunity immunity in ToRemove2)
            {
                Immunitys.Remove(immunity);
            }
            List<Poison> ToRemove1 = new List<Poison>();
            foreach (Poison poison in Poisons)
            {
                if (poison.Tick(this)) ToRemove1.Add(poison);
            }
            foreach (Poison poison1 in ToRemove1)
            {
                Poisons.Remove(poison1);
            }
            List<Buff> ToRemove = new List<Buff>();
            foreach (Buff buff in m_buffs)
            {
                if (buff.Tick(this)) ToRemove.Add(buff);
            }
            foreach (Buff buff in ToRemove)
            {
                m_buffs.Remove(buff);
            }
        }
        public void TickTimers()
        {
            //v2 = a1->gap354;
            //a1->gap354 = v2 - 1;
            //if (v2 <= 1)
            //{
            //    a1->BallStuff1 = 0;
            //    a1->BallStuff2 = 0;
            //}
            //v3 = *a1->gap37C;
            //if (v3 >= 1)
            //{
            //    *a1->gap37C = v3 - 1;
            //    if (v3 == 1)
            //        ZN20LogicCharacterServer11causeDamageEiiiPS_biiPK9LogicDatabbbbb(
            //          a1,
            //          -1,
            //          &stru_18698.r_info + 3,
            //          0,
            //          0,
            //          0,
            //          0,
            //          0,
            //          0,
            //          1,
            //          0,
            //          1,
            //          0,
            //          1);
            //}
            //v4 = a1->int3B0;
            //if (v4 >= 1)
            //{
            //    a1->int3B0 = v4 - 1;
            //    if (v4 == 1)
            //    {
            //        a1->dword3A8 = -1;
            //        a1->dword3AC = -1;
            //    }
            //}
            //v5 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
            //v6 = ZNK21LogicBattleModeServer12getTicksGoneEv(v5);
            //if (!*&a1->gapDD[11])
            //    *&a1->gapDD[11] = ZN21LogicBattleModeServer12getRandomIntEii(v5, -10, 10) + 110;
            //v7 = 0;
            if (CharacterData.UniqueProperty == 1 && TicksGone - AshLastRageTick >= 21) ChargeUp = LogicMath.Max(0, ChargeUp - 3);
            if (RealmTimer >= 1) RealmTimer--;
            if (RealmTimer == 0) IsInRealm = false;
            if (RapidFireTick != TicksGone) m_attackingTicks = 63;
            IsCrippled = TicksGone < CrippleEndTick;
            if (!IsCrippled) CripplePercent = 0;
            //a1->unsigned___int8200 = v7;
            //if (v8 >= 1)
            //    a1->int344 = v8 - 1;
            if (VisionOverrideTicks >= 1) VisionOverrideTicks--;
            if (ShieldTicks >= 1) ShieldTicks--;
            if (CcImmunityTicks >= 1) CcImmunityTicks--;
            if (Shaking >= 1) Shaking--;
            if (ReloadBuffTicks >= 1) ReloadBuffTicks--;
            if (SkillHoldTicks >= 0) SkillHoldTicks++;
            if (TicksGone <= SpawnTick + 60) IsInvincible = true;
            else IsInvincible = false;
            if (ChargeUpType == 2 || ChargeUpType == 6 || ChargeUpType == 1 && (m_skills[0].HasFullCharge() || CharacterData.Name == "CannonGirlSmall"))
            {
                if (State != 4) ChargeUp = LogicMath.Min(ChargeUp + 50, ChargeUpMax);
            }
            else if (ChargeUpType == 10)
            {
                if (State == 1 || State == 3) ChargeUp = 0;
                else if (State != 4) ChargeUp = LogicMath.Min(ChargeUp + 50, ChargeUpMax);
            }
            if (ConsumableShield >= 1 && TicksGone > ConsumableShieldTick && ((TicksGone - ConsumableShieldTick) % (ConsumableShieldDuration / 20) == 0)) ConsumableShield = LogicMath.Max(0, ConsumableShield - ConsumableShieldMax / 20);
            if (ChargeUpType == 7 && State != 4)
            {
                if (GetUltimateSkill().SkillData.ChargeType == 15)
                {
                    ChargeUp = GetUltimateSkill().TicksActive;
                    return;
                }
                BattlePlayer result = GetPlayer();
                if (result != null)
                {
                    Accessory v20 = result.Accessory;
                    if (v20 != null)
                    {
                        if (v20.AccessoryData.ShowCountdown)
                        {
                            if (v20.IsActive)
                            {
                                ChargeUp = ChargeUpMax - 50 * v20.TicksActive;
                                return;
                            }
                        }
                    }
                    ChargeUp = 0;
                }
            }
        }
        public void GiveSpeedSlowerBuff(int a2, int a3)
        {
            //LogicBuffServer* result; // r0
            //int v7; // r0
            //signed int v8; // r0
            //signed int v9; // kr00_4

            //result = ZNK20LogicCharacterServer13hasCcImmunityEv(a1);
            //if (!result)
            //{
            //    v7 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //    if (ZNK18LogicCharacterData15isTownCrushBossEv(v7))
            //    {
            //        v9 = a2;
            //        v8 = a2 + (a2 >> 31);
            //        a2 = 1;
            //        if (v9 / 2 > 1)
            //            a2 = v8 >> 1;
            //    }
            //    result = ZN20LogicCharacterServer9applyBuffEiiiiiii(a1, 3, a3, a2, 0);
            //    *(a1 + 232) = 0;
            //}
            //return result;
            ApplyBuff(3, a3, a2, 0);
        }
        public void GiveElectrocution(int Damage, int NormalDMG, int EffectType, int Bounces)
        {
            for (int i = 0; i < m_buffs.Count; i++)
            {
                if (m_buffs[i].Type == 14)
                {
                    m_buffs.RemoveAt(i);
                    i--;
                }
            }
            Buff b = ApplyBuff(14, 20, Damage, 0);
            b.EffectType = EffectType;
            b.NormalDMG = NormalDMG;
            b.BelleWeaponBounces = Bounces;
        }
        public void GiveSpeedFasterBuff(
        int a2,
        int a3,
        bool a4)
        {

            if (a4)
            {
                if (m_buffs.Count < 1)
                {
                    int344 = 10;
                    dword348 = 0;
                }
                else
                {
                    foreach (Buff buff in m_buffs)
                    {
                        if (buff.Type == 4)
                        {
                            ApplyBuff(4, a3, a2, 0);
                            return;
                        }
                    }
                    int344 = 10;
                    dword348 = 0;
                }
            }
            ApplyBuff(4, a3, a2, 0);
        }
        public void GiveDamageBuff(
        int a2,
        int a3)
        {
            ApplyBuff(1, a3, a2, 1);
            int344 = 10;
            dword348 = 2;
        }
        public void GiveDamageBuff2(
        int a2,
        int a3)
        {

            if (m_buffs.Count < 1)
            {
                int344 = 10;
                dword348 = 2;
            }
            else
            {
                foreach (Buff buff in m_buffs)
                {
                    if (buff.Type == 5)
                    {
                        ApplyBuff(5, a3, a2, 1);
                        return;
                    }
                }
                int344 = 10;
                dword348 = 2;
            }

            ApplyBuff(5, a3, a2, 1);
        }
        public void TickGears()
        {
            if (Gear1 != null) Gear1.Tick(this);
            if (Gear2 != null) Gear2.Tick(this);
        }
        public void TriggerBlink(int x, int y, AreaEffectData areaEffectData1, AreaEffectData areaEffectData2, int Damage, int NormalDMG)
        {
            IsTeleporting = true;
            AreaEffect areaEffect = new AreaEffect(areaEffectData1);
            areaEffect.SetPosition(x, y, 0);
            areaEffect.SetIndex(GetIndex());
            areaEffect.Damage = Damage;
            areaEffect.NormalDMG = NormalDMG;
            GameObjectManager.AddGameObject(areaEffect);
            areaEffect.Trigger();
            AreaEffect areaEffect2 = new AreaEffect(areaEffectData2);
            areaEffect2.SetPosition(GetX(), GetY(), 0);
            areaEffect2.SetIndex(GetIndex());
            GameObjectManager.AddGameObject(areaEffect2);
            areaEffect2.Trigger();
            MoveStart = TicksGone - 1;
            MoveEnd = TicksGone - 1;
            BlinkX = x;
            BlinkY = y;
        }
        public override void ResetEventsOnTick()
        {
            IsTeleporting = false;
            HitEffectProjectile = 0;
            HitEffectSkin = 0;
            List<DamageNumber> Tobe = new List<DamageNumber>();
            if (DamageNumbers.Count >= 1)
            {
                foreach (DamageNumber damageNumber in DamageNumbers)
                {
                    if (damageNumber.Delay <= 0)
                    {
                        Tobe.Add(damageNumber);
                    }
                    else
                    {
                        damageNumber.Delay--;
                    }
                }
            }
            if (Tobe.Count >= 1)
            {
                foreach (DamageNumber damageNumber in Tobe)
                {
                    DamageNumbers.Remove(damageNumber);
                }
            }
        }
        public Buff ApplyBuff(
        int Type,
        int Dura,
        int Modi,
        int SizeInc,
        int Index = -1)
        {
            if (m_buffs.Count < 1)
            {
                m_buffs.Add(new Buff(Type, Dura, Modi, SizeInc));
                return m_buffs.Last();
            }
            else
            {
                foreach (Buff buff in m_buffs)
                {
                    if (buff.Type == Type && !buff.CanStack())
                    {
                        bool v14 = buff.Duration < Dura;
                        if (buff.Duration >= Dura)
                            v14 = buff.Modifier < Modi;
                        if (v14)
                        {
                            buff.Modifier = Modi;
                            buff.SizeIncrease = SizeInc;
                            buff.Duration = Dura;
                        }
                        return buff;
                    }
                }

            }
            m_buffs.Add(new Buff(Type, Dura, Modi, SizeInc)
            {
                SourceIndex = Index
            });
            return m_buffs.Last();
        }
        private void ExecuteBlink()
        {
            if (BlinkX != -1)
            {
                MoveStart = TicksGone - 1;
                MoveEnd = TicksGone - 1;
                SetPosition(BlinkX, BlinkY, CharacterData.FlyingHeight);
                IsTeleporting = true;
                BlinkX = -1;
            }
        }
        private void TickStarPowers()
        {
            if (GetPlayer() == null) return;
            if (GetCardValueForPassive("shield_while_ulti", 1) >= 1)
            {
                if (GetControlledProjectile() != null) AddShield(2, GetCardValueForPassive("shield_while_ulti", 1));
                return;
            }
            if (GetCardValueForPassive("bush_shield", 1) >= 1)
            {
                if (IsInBush()) AddShield(GetCardValueForPassive("bush_shield", 0), GetCardValueForPassive("bush_shield", 1));
                return;
            }
            if (GetCardValueForPassive("speed_full_ammo", 1) >= 1)
            {
                if (ChargeUp >= ChargeUpMax || MeleeAttackPushBacks) GiveSpeedFasterBuff(GetCardValueForPassive("speed_full_ammo", 1), 10, true);
                return;
            }
            if (GetCardValueForPassive("speed_low_health", 0) >= 1)
            {
                if (GetHitpointPercentage() <= GetCardValueForPassive("speed_low_health", 0)) GiveSpeedFasterBuff(GetCardValueForPassive("speed_low_health", 1), 10, true);
                return;
            }
            if (GetCardValueForPassive("berserker", 1) >= 1)
            {
                if (GetHitpointPercentage() <= GetCardValueForPassive("berserker", 1)) GiveReloadBuff(10);
                return;
            }
            if (GetCardValueForPassive("low_health_shield", 1) >= 1)
            {
                if (GetHitpointPercentage() <= GetCardValueForPassive("low_health_shield", 1)) AddShield(4, GetCardValueForPassive("low_health_shield", 0));
                return;
            }
            if (GetCardValueForPassive("shield_homerun", 1) >= 1)
            {
                if (ChargeUp >= ChargeUpMax || MeleeAttackPushBacks) AddShield(4, GetCardValueForPassive("shield_homerun", 1));
                return;
            }
            if (GetCardValueForPassive("shield_while_charged_shots", 1) >= 1)
            {
                if (ChargedShotCount >= 1) AddShield(4, GetCardValueForPassive("shield_while_charged_shots", 1));
                return;
            }
            if (GetCardValueForPassive("speed_ulti", 1) >= 1)
            {
                if (HasUlti()) GiveSpeedFasterBuff(GetCardValueForPassive("speed_ulti", 1), 10, true);
                return;
            }
            //shield_ulti

            if (GetCardValueForPassive("medikit", 1) >= 1)
            {
                if (GetHitpointPercentage() <= GetCardValueForPassive("medikit", 1) && ChargeUp >= ChargeUpMax)
                {
                    Heal(GetIndex(), GetCardValueForPassive("medikit", 0), true, CharacterData);
                    ChargeUp = 0;
                }
                return;
            }
            BattlePlayer v51 = GetPlayer();
            if (v51 != null)
            {
                int v52 = v51.GetCardValueForPassive("plugged_in", 1);
                if (v52 >= 1 && !CharacterData.IsHero())
                {
                    StarPowerProjectileTimer--;
                    if (StarPowerProjectileTimer <= 0)
                    {
                        Character v55 = (Character)GameObjectManager.GetGameObjectByID(ParentGID);
                        int v56 = v51.GetCardValueForPassive("plugged_in", 2);
                        if (v55 != null && v55.IsAlive())
                        {
                            int v57 = 300 * v51.GetCardValueForPassive("plugged_in", 0);
                            if (GamePlayUtil.GetDistanceSquaredBetween(GetX(), GetY(), v55.GetX(), v55.GetY()) < v57 * v57)
                            {
                                Projectile v66 = Projectile.ShootProjectile(
                                        -1,
                                        -1,
                                        this,
                                        this,
                                        DataTables.GetProjectileByName("ArcadeStarPowerProjectile"),
                                        v55.GetX(),
                                        v55.GetY(),
                                        0,
                                        0,
                                        0,
                                        false,
                                        0,
                                        GetBattle(),
                                        0,
                                        3);
                                v66.HomingTarget = v55;
                                v66.SetIndex(GetIndex());
                                v66.SpeedBuffModifier = v52;
                                v66.SpeedBuffLength = 2 * v56;
                            }
                        }
                        StarPowerProjectileTimer = v56;
                    }

                }
                v52 = v51.GetCardValueForPassive("ulti_item_repeated_area", 1);
                if (v52 >= 1)
                {
                    StarPowerProjectileTimer--;
                    if (StarPowerProjectileTimer <= 0)
                    {
                        Item v55 = null;
                        foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                        {
                            if (gameObject.GetObjectType() != 3) continue;
                            if (gameObject.GetIndex() != GetIndex()) continue;
                            v55 = (Item)gameObject;
                            break;
                        }
                        if (v55 != null)
                        {
                            AreaEffect areaEffect = GameObjectFactory.CreateGameObjectByData(DataTables.GetAreaEffectByName("WeaponThrowerElectroArea"));
                            areaEffect.SetPosition(v55.GetX(), v55.GetY(), 0);
                            areaEffect.SetIndex(GetIndex());
                            areaEffect.SetSource(this, 3);
                            areaEffect.Damage = GetCardValueForPassive("ulti_item_repeated_area", 1);
                            areaEffect.NormalDMG = 0;
                            //areaEffect.DisplayScale = 500;
                            GameObjectManager.AddGameObject(areaEffect);
                            areaEffect.Trigger();
                            StarPowerProjectileTimer = GetCardValueForPassive("ulti_item_repeated_area", 0);
                        }
                    }

                }
            }
        }


        private void TickTile()
        {
            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();

            Tile tile = tileMap.GetTile(GetX(), GetY());
            if ((tile.Data.HidesHero && !tile.IsDestructed()) || IsInvisible)
            {
                DecrementFadeCounter();
            }
            else
            {
                IncrementFadeCounter();
            }

            int x = TileMap.LogicToTile(GetX());
            int y = TileMap.LogicToTile(GetY());
            if (TicksGone - m_lastTileDamageTick > 20)
            {
                if (GameObjectManager.GetBattle().IsTileOnPoisonArea(x, y))
                {
                    m_lastTileDamageTick = TicksGone;
                    CauseDamage(null, 1000, 0, false, null);
                }
            }
        }
        public bool IsInBush()
        {
            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();

            Tile tile = tileMap.GetTile(GetX(), GetY());
            if (tile.Data.HidesHero && !tile.IsDestructed())
            {
                return true;
            }
            return false;
        }
        public void ShowDamageNumber(int a2, int a3, Character a4)
        {
            bool v7 = false;
            int v11;
            if (a4 != null)
            {
                SkillData v8 = a4.GetWeaponSkill() == null ? null : a4.GetWeaponSkill().SkillData;
                if (v8 != null)
                    v7 = v8.NumBulletsInOneAttack > 1;
            }
            if (DamageNumbers.Count < 1)
            {
                if (v7)
                {
                    v11 = DAMAGE_NUMBERS_DELAY_TICKS;
                }
                else
                {
                    v11 = 0;
                }
                DamageNumber damageNumber = new DamageNumber(a3, a2, v11);
                DamageNumbers.Add(damageNumber);
            }
            else
            {
                foreach (DamageNumber DamageNumber in DamageNumbers)
                {
                    if (DamageNumber.Dealer == a2)
                    {
                        DamageNumber.Value += a3;
                        if (v7)
                        {
                            v11 = DAMAGE_NUMBERS_DELAY_TICKS;
                        }
                        else
                        {
                            v11 = 0;
                        }
                        DamageNumber.Delay = v11;
                        return;
                    }
                }
                if (v7)
                {
                    v11 = DAMAGE_NUMBERS_DELAY_TICKS;
                }
                else
                {
                    v11 = 0;
                }
                DamageNumber damageNumber = new DamageNumber(a3, a2, v11);
                DamageNumbers.Add(damageNumber);
            }
        }
        public void StopMovement()
        {
            MoveStart = TicksGone - 1;
            MoveEnd = TicksGone - 1;
        }

        //public Character GetClosestEnemy()
        //{
        //    int dis = 999999999;
        //    Character result = null;
        //    foreach(GameObject gameObject in GameObjectManager.GetGameObjects())
        //    {
        //        if (gameObject.GetObjectType() != 0) continue;
        //        Character character = (Character)gameObject;
        //        if (gameObject.GetIndex() / 16 == GetIndex() / 16) continue;
        //        if (character.IsImmuneAndBulletsGoThrough() || character.IsInvincible) continue;
        //        if (character.Position.GetDistance(Position) < dis)
        //        {
        //            result=character;
        //            dis = character.Position.GetDistance(Position);
        //        }
        //    }
        //    return result;
        //}

        private Character ShamanPetTarget;
        private void TickAI()
        {
            if (m_isBot != 0)
            {
                TickBot();
                return;
            }

            if (CharacterData.IsHero()) return;
        }
        public void SetInvisible(int ticks = 3)
        {
            if (InvisibleTicks <= 0) IsInvisible = true;
            InvisibleTicks = ticks;
        }
        private void TickBot()
        {
            if (IsControlled) return;
            m_ticksSinceBotEnemyCheck++;

            if (m_ticksSinceBotEnemyCheck > 20 || m_closestEnemy == null || m_isBot == 2)
            {
                m_ticksSinceBotEnemyCheck = 0;
                Character closestEnemy = GetClosestVisibleEnemy();

                if (closestEnemy == null) return;

                m_closestEnemy = closestEnemy;
                m_closestEnemyPosition = closestEnemy.GetPosition();
            }

            if (m_closestEnemy == null) return;
            if (m_isBot == 2)
            {
                if (TicksGone - m_lastUltiTick > 100)
                {
                    LogicVector2 enemyPosition1 = m_closestEnemy.GetPosition();
                    m_lastUltiTick = TicksGone;
                    m_lastAIAttackTick = TicksGone - 10;
                    ActivateSkill(1, enemyPosition1.X, enemyPosition1.Y);
                }
                //int deltaX = enemyPosition.X - Position.X;
                //int deltaY = enemyPosition.Y - Position.Y;

                Projectile projectile = null;
                int distance = 99999999;

                foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                {
                    if (gameObject.GetObjectType() != 1) continue; // not a character, ignore.

                    Projectile enemy = (Projectile)gameObject;
                    if (enemy == null) continue; // invalid object
                    if (enemy.GetIndex() / 16 == GetIndex() / 16 || enemy.State == 4) continue; // teammate, ignore.

                    int angle1 = LogicMath.GetAngle(GetX() - enemy.GetX(), GetY() - enemy.GetY());
                    int angle2 = enemy.Angle;

                    int distanceToEnemy = LogicMath.Sin(LogicMath.NormalizeAngle180(angle2 - angle1)) * Position.GetDistance(enemy.GetPosition()) >> 10;
                    if (distanceToEnemy < distance)
                    {
                        projectile = enemy;
                        distance = distanceToEnemy;
                    }
                }
                if (GameObjectManager.GetSpecificProjectiles(this).Count() >= 3)
                {
                    MoveAngle = LogicMath.GetAngle(m_closestEnemyPosition.X - Position.X, m_closestEnemyPosition.Y - Position.Y);
                    MoveAngle += GetBattle().GetRandomInt(2) == 1 ? 45 : -45;
                    GetPlayer().Accessory.TriggerAccessory(this, 0, 0);
                }
                else if (projectile != null && distance < projectile.ProjectileData.Radius + CharacterData.CollisionRadius && distance < 3000)
                {
                    //Debugger.Print(distance);
                    GetBattle().GetTileMap().DestroyEnvironment(GetX(), GetY(), 1000, false);
                    int angle2 = projectile.Angle;
                    //if (angle2 > angle1)
                    //{
                    //    int angle = LogicMath.NormalizeAngle360(90 - angle2);
                    //    MoveAngle = angle;
                    //    AttackAngle = MoveAngle;
                    //    MoveTo(0, GetX() + LogicMath.GetRotatedX(projectile.ProjectileData.Radius + CharacterData.CollisionRadius - distance, 0, angle), GetY() + LogicMath.GetRotatedY(0, projectile.ProjectileData.Radius + CharacterData.CollisionRadius - distance, angle), 0, 0, 0, 0);

                    //    //MoveTo(0, GetX() + LogicMath.GetRotatedX(100, 0, angle), GetY() + LogicMath.GetRotatedY(0, 100, angle), 0, 0, 0, 0);
                    //}
                    //else
                    //{
                    int angle = LogicMath.NormalizeAngle360(180 - angle2);
                    int S = 100;
                    //int S = projectile.ProjectileData.Radius + CharacterData.CollisionRadius - distance+10;
                    int DeltaX = LogicMath.Sin(angle) * S >> 10;
                    int DeltaY = LogicMath.Cos(angle) * S >> 10;
                    if (new LogicVector2(GetX() + DeltaX, GetY() + DeltaY).GetDistance(new LogicVector2(projectile.GetX(), projectile.GetY())) <= new LogicVector2(GetX(), GetY()).GetDistance(new LogicVector2(projectile.GetX(), projectile.GetY())))
                    {
                        DeltaX = -DeltaX;
                        DeltaY = -DeltaY;
                        angle = LogicMath.NormalizeAngle360(angle2 + 180);
                    }
                    //Debugger.Print(MoveAngle);
                    //MoveAngle = angle;
                    //AttackAngle = MoveAngle;
                    MoveTo(0, GetX() + DeltaX, GetY() + DeltaY, 0, 0, 0, 0);

                    //}
                }
                else if (Position.GetDistance(m_closestEnemyPosition) >= m_skills[0].SkillData.CastingRange * 50)
                {
                    MoveTo(0, m_closestEnemyPosition.X, m_closestEnemyPosition.Y, 0, 0, 0, 0);
                }
            }
            //if (m_ticksSinceBotEnemyCheck % 1 == 0)
            if (m_isBot == 1 && Position.GetDistance(m_closestEnemyPosition) >= m_skills[0].SkillData.CastingRange * 50)
            {
                TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();
                var Tiles = new byte[tileMap.LogicWidth / 300, tileMap.LogicHeight / 300];
                for (int i = 0; i < tileMap.LogicWidth / 300; i++)
                {
                    for (int j = 0; j < tileMap.LogicHeight / 300; j++)
                    {
                        Tiles[i, j] = (byte)(tileMap.GetTile(i, j, true).Data.BlocksMovement ? 0 : 1);
                    }
                }
                PathFinder pathFinder = new PathFinder(Tiles);
                pathFinder.Diagonals = false;
                //PathFinderOptions pathFinderOptions = new PathFinderOptions() { SearchLimit = 20000 };
                //Debugger.Print(Tiles.ToString());
                //var grid = new WorldGrid(Tiles);
                var start = new DeenGames.Utils.Point(LogicMath.Clamp(GetX(), 300, tileMap.LogicWidth - 300) / 300, LogicMath.Clamp(GetY(), 300, tileMap.LogicHeight - 300) / 300);
                //var start = new Position(1, 8);
                //var end = new Position(2, 8);
                var end = new DeenGames.Utils.Point(LogicMath.Clamp(m_closestEnemyPosition.X, 300, tileMap.LogicHeight - 300) / 300, LogicMath.Clamp(m_closestEnemyPosition.Y, 300, tileMap.LogicWidth - 300) / 300);
                //var end = new Point(8, 2);
                //pathFinder.DebugFoundPath = true;
                //var end = new Position(m_closestEnemyPosition.Y / 300, m_closestEnemyPosition.X / 300);
                var Points = pathFinder.FindPath(start, end);
                if (Points != null)
                {
                    //MoveStart = MoveEnd;
                    //WayPointStart = Position;
                    //WayPoint = Position;
                    PathFinderNode node = Points[0];
                    for (int i = 0; i < Points.Count; i++)
                    {
                        if (Points[i].X != node.X && Points[i].Y != node.Y)
                        {
                            node = Points[i - 1];
                            break;
                        }
                        if (i == Points.Count - 1)
                        {
                            node = Points[i];
                            break;
                        }
                    }
                    MoveTo(0, node.X * 300 + 150, node.Y * 300 + 150, 0, 0, 0, 0);
                }
            }

            if (TicksGone - m_lastAIAttackTick <= 20) return;
            Skill weapon = GetWeaponSkill();
            LogicVector2 enemyPosition = m_closestEnemy.GetPosition();
            if (Position.GetDistance(enemyPosition) >= m_skills[0].SkillData.CastingRange * 80) return;
            if (m_isBot == 2) m_skills[0].Charge = 4000;

            if (!weapon.HasEnoughCharge()) return;
            m_lastAIAttackTick = TicksGone;
            if (m_isBot == 2) m_lastAIAttackTick -= 15;
            //int deltaX = enemyPosition.X - Position.X;
            //int deltaY = enemyPosition.Y - Position.Y;

            ActivateSkill(0, enemyPosition.X, enemyPosition.Y);
        }
        public void TriggerCharge(int x, int y, int speed, int Type, int Damage, int Pushback, bool returning, AreaEffectData areaEffectdata = null)
        {
            int X = GetX();
            int Y = GetY();
            LogicVector2 t = new LogicVector2(x, y);
            GamePlayUtil.GetClosestLevelBorderCollision(X, Y, x, y, GetBattle().GetTileMap(), 0, t);
            if (t.X > x) t.X += 50;
            if (t.X < x) t.X -= 50;
            if (t.Y > y) t.Y += 50;
            if (t.Y < y) t.Y -= 50;
            x = t.X;
            y = t.Y;
            if (ChargeUpType == 1) ChargeUp = 0;
            if (GetCardValueForPassive("heal_on_charge", 1) >= 1) Heal(GetIndex(), GetCardValueForPassive("heal_on_charge", 1), true, null);
            Accessory accessory = GetPlayer()?.Accessory;
            if (accessory != null && accessory.IsActive && accessory.AccessoryData.Type == "ulti_change" && accessory.AccessoryData.SubType == 2)
            {
                ChargeBreaksEnvironment = true;
                accessory.EndAccessoryActivation();
            }
            else ChargeBreaksEnvironment = false;
            FangUltiCount = 0;
            //TeamIndex = a1->TeamIndex;
            //if (TeamIndex >= 0)
            //{
            //    v20 = (1431655765LL* a4) >> 32;
            //    v21 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //    a1 = ZN11LogicPlayer20increaseTeamingScoreEi(*(*(v21 + 72) + 4 * TeamIndex), ((v20 - a4) >> 1) + ((v20 - a4) >> 31));
            //}
            //v22 = a15 == 0;
            //if (!a15)
            //    a1 = a1->dword394;
            //if (!a15)
            //    v22 = a1 == (&dword_0 + 1);
            //if (v22)
            //    a1->dword38C = 0;
            if (Type != 10 || returning)
            {
                if (GetCardValueForPassive("shield_charge", 1) >= 1)
                {
                    AddShield(GetCardValueForPassive("shield_charge", 2), GetCardValueForPassive("shield_charge", 1));
                }
                //a1->UltiSkill = a15;
                m_stunTicks = 0;
                //a1->Rotate = 0;
                ChargeHits = 0;
                if (Type == 10) WayPoint2 = GetPosition();
            }
            ChargeHitGIDs = new List<Ignoring>();
            FangHitGIDs = new List<int>();
            //if (a9 != 7)
            //    JUMPOUT(0x324764);
            //v26 = GetX();
            //v157[0] = v26;
            //v27 = GetY();
            //v28 = x - v26;
            //v156 = v27;
            //v29 = y - v27;
            //v30 = LogicMath.Sqrt(v29 * v29 + v28 * v28);
            //v31 = v30;
            //v32 = 100 * a14;
            //v129 = 100 * a14;
            //if (v30)
            //{
            //    v28 = a1_divide_a2(v28 * v32, v30);
            //    v29 = a1_divide_a2(v29 * v32, v31);
            //}
            //v155 = v26 + v28;
            //v154 = v27 + v29;
            //v153 = 0;
            //v152[1] = 0;
            //v151 = 0;
            //p[1] = 0;
            //v152[0] = 0;
            //p[0] = 0;
            //sub_6F83B4(v152, v157);
            //sub_6F83B4(p, &v156);
            //v33 = a1;
            //v34 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v35 = sub_5F01A8(v34);
            //v36 = v156;
            //v37 = v157[0];
            //v124 = v35;
            //v131 = 0;
            //v126 = 0;
            //while (1)
            {
                if (Type == 4 || Type == 7 || Type == 8 || Type == 10 || (Type == 12 && !ChargeBreaksEnvironment))
                {
                    int v46;
                    int v47;
                    LogicVector2 v148 = new LogicVector2(-1, -1);
                    bool v38 = GamePlayUtil.GetClosestAnyCollision(
                            X,
                            Y,
                            x,
                            y,
                           GameObjectManager.GetBattle().GetTileMap(),
                            v148,
                            0,
                            0,
                            0,
                            0);
                    bool v127 = v38;
                    if (v148.X == -1)
                    {
                        v47 = y;
                        v46 = x;
                    }
                    else
                    {
                        int v39 = v148.X - X;
                        int v40 = Y;
                        int v41 = v148.Y - Y;
                        int v42 = LogicMath.Sqrt(v39 * v39 + v41 * v41);
                        if (v42 > 0)
                        {
                            int v43 = v42;
                            int v44 = 20;
                            if (v43 <= 20)
                                v44 = LogicMath.Max(1, v43 - 5);
                            int v45 = v43 - v44;
                            v39 = (v43 - v44) * v39 / v43;
                            v41 = v45 * v41 / v43;
                        }
                        v46 = v39 + X;
                        v47 = v41 + v40;
                        y = v41 + v40;
                        x = v46;
                    }
                    //int v48 = LogicMath.Sqrt((v46 - X) * (v46 - X) + (v47 - Y) * (v47 - Y));
                    x = LogicMath.Clamp(x, 101, GameObjectManager.GetBattle().GetTileMap().LogicWidth - 102);
                    y = LogicMath.Clamp(y, 101, GameObjectManager.GetBattle().GetTileMap().LogicHeight - 102);
                }
                if (Type == 2 || Type == 6 || Type == 9 || Type == 11 || Type == 16)
                {
                    LogicVector2 v = new();
                    GamePlayUtil.GetClosestPassableSpot(1, x, y, 10, GetBattle().GetTileMap(), v, true);
                    x = v.X;
                    y = v.Y;
                }
                //    sub_6F83B4(v152, &v155);
                //    sub_6F83B4(p, &v154);
                //    v55 = v129;
                //    v56 = v48 + v131;
                //    v131 += v48;
                //    if (!v38 || (v57 = 1, v56 >= v129 - 50))
                //    {
                //        v58 = ZN12LogicTileMap21logicToPathFinderTileEi(v155);
                //        v59 = ZN12LogicTileMap21logicToPathFinderTileEi(v154);
                //        v60 = sub_432FEC(v33);
                //        if (ZNK12LogicTileMap20isPassablePathFinderEiiibb(v124, v60, v58, v59, 0, 0))
                //            goto LABEL_36;
                //        v57 = 0;
                //        v55 = v129 + 300;
                //        ++v126;
                //    }
                //    v61 = v155;
                //    v62 = v155 - v37;
                //    v125 = v154;
                //    v63 = v154 - v36;
                //    v64 = LogicMath.Sqrt(v62 * v62 + v63 * v63);
                //    if (v57)
                //    {
                //        v65 = v127 == 4;
                //        if (v127 != 4)
                //            v65 = v127 == 1;
                //        if (v65)
                //            v63 = -v63;
                //        else
                //            v62 = -v62;
                //    }
                //    v129 = v55;
                //    if (v64)
                //    {
                //        v62 = a1_divide_a2(v62 * (v55 - v131), v64);
                //        v63 = a1_divide_a2(v63 * (v55 - v131), v64);
                //    }
                //    v155 += v62;
                //    v154 += v63;
                //    v37 = v61;
                //    v36 = v125;
                //    v33 = a1;
                //    if (v126 >= 10)
                //    {
                //    LABEL_36:
                //        if (v33->WayPoints >= 1)
                //        {
                //            do
                //                sub_2234E4(v33);
                //            while (v33->WayPoints > 0);
                //        }
                //        v66 = ZNK21LogicGameObjectServer4getXEv(v33);
                //        v33->field_54 = ZN12LogicTileMap21logicToPathFinderTileEi(v66);
                //        v67 = ZNK21LogicGameObjectServer4getYEv(v33);
                //        v33->field_58 = ZN12LogicTileMap21logicToPathFinderTileEi(v67);
                //        v33->field_5C = ZN12LogicTileMap21logicToPathFinderTileEi(*(v152[0] + 4 * v153 - 4));
                //        v33->field_60 = ZN12LogicTileMap21logicToPathFinderTileEi(*(p[0] + v151 - 1));
                //        v68 = v153;
                //        v132 = v153 - 1;
                //        if (v153 >= 1)
                //        {
                //            v69 = 4 * v153 - 4;
                //            v70 = 0;
                //            p_YwayPoints = &a1->YwayPoints;
                //            p_XwayPoints = &a1->XwayPoints;
                //            do
                //            {
                //                v73 = (v152[0] + v69);
                //                if (v70)
                //                {
                //                    sub_6F83B4(p_XwayPoints, v73);
                //                    v74 = (p[0] + v69);
                //                    v75 = p_YwayPoints;
                //                }
                //                else
                //                {
                //                    v76 = *v73;
                //                    v77 = v68;
                //                    v78 = *(p[0] + v132);
                //                    ZN12LogicVector2C2Ev(v146);
                //                    v79 = sub_432FEC(a1);
                //                    v80 = v78;
                //                    v68 = v77;
                //                    v81 = v76;
                //                    p_YwayPoints = &a1->YwayPoints;
                //                    sub_48C8C4(v79, v81, v80, 5, v124, v146, 0);
                //                    sub_6F83B4(p_XwayPoints, v146);
                //                    v75 = &a1->YwayPoints;
                //                    v74 = &v147;
                //                }
                //                sub_6F83B4(v75, v74);
                //                --v70;
                //                v69 -= 4;
                //            }
                //            while (v68 + v70 > 0);
                //        }
                //v82 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //    v83 = ZN21LogicBattleModeServer13getPathFinderEv(v82);
                //    ZN20LogicCharacterServer12ensurePathOkEP15LogicPathFinder(a1, v83);
                if (x == GetX()) y -= 100;
                PathfindTo(0, x, y, 0, 0, 0, 0, 0, 0);
                int v84 = GetPathLength();
                PathLength = v84;
                int pathlength = PathLength;
                if (v84 > 0)
                {
                    MoveStart = TicksGone;
                    if (Type == 2 || Type == 6 || Type == 9 || Type == 11 || Type == 16)
                    {
                        int range = m_skills[1].SkillData.CastingRange;
                        if (range == 0) range = 22;
                        //speed = speed * PathLength / range / 100;
                        //speed = speed * 7 / 10;
                        if (Type == 9) speed = (int)(speed * 0.3);
                        //if (Type == 11) speed = (int)(speed * 0.7);
                        if (Type == 16) speed = (int)(speed * 0.8);
                        //if (Type == 2) speed = (int)(speed * 0.95);
                        //if (Type == 6) speed = (int)(speed * 0.9);
                        speed = LogicMath.Max(speed, 1);
                        if (Type == 2 || Type == 6 || Type == 11) pathlength = 2300;
                    }
                    MoveEnd = LogicMath.Max(1, 20 * pathlength / speed) + TicksGone;
                    //if(Type==2||Type==6) MoveEnd = LogicMath.Max(1, 20 * m_skills[1].SkillData.CastingRange*100 / speed) + TicksGone;
                }
                //a1->field_158 = 0;
                IsCharging = true;
                if (CharacterData.UniqueProperty == 4)
                {
                    AddShield(CharacterData.UniquePropertyValue1, CharacterData.UniquePropertyValue2);
                }
                //v8 = *(v4 + 12);
                //v9 = *(v4 + 8);
                if (GetCardValueForPassive("super_reload", 1) >= 0)
                    ReloadBuffTicks = GetCardValueForPassive("super_reload", 1);
                //*&v1->gap1BE[2] = 0;
                //v1->field_14C = 0;
                ChargeSpeed = speed;
                //v1->FangUltiCount = 0;
                ChargeDamage = Damage;
                //v1->field_180 = v9;
                //v1->field_184 = 0;
                ChargePushback = Pushback;
                m_activeChargeType = Type;
                ShouldExecuteColetteChargeOnReturn = returning;
                //*v1->gap190 = v7;
                if (Type == 6)
                {
                    //v18 = *(v4 + 40);
                    //v19 = ZNK21LogicGameObjectServer4getXEv(v34);
                    //v20 = ZNK21LogicGameObjectServer4getYEv(v34);
                    //v21 = *(v34 + 40);
                    //v22 = *(v34 + 44);
                    //v23 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                    //v32 = v18;
                    //v1 = v34;
                    //sub_5EEF4C(*(v4 + 32), v19, v20, *(v4 + 36), v32, v33, v9, v21, v22, v34, v23, 2);
                }
                else if (Type == 2 || Type == 11)
                {
                    AreaEffect areaEffect = new AreaEffect(areaEffectdata);
                    areaEffect.SetPosition(GetX(), GetY(), 0);
                    areaEffect.SetIndex(GetIndex());
                    areaEffect.SetSource(this, 2);
                    areaEffect.Damage = Damage;
                    areaEffect.NormalDMG = ChargeNormalDMG;
                    //v14 = GetCardValueForPassive(v1, 21, 1);
                    //if (v14 >= 0)
                    //    v11[25] = v14;
                    int v15 = GetCardValueForPassive("prey_on_the_weak", 1);
                    if (v15 >= 0)
                    {
                        areaEffect.SetPreyOnTheWeak(v15, GetCardValueForPassive("prey_on_the_weak", 0));
                    }
                    GameObjectManager.AddGameObject(areaEffect);
                    areaEffect.Trigger();
                    ChargeAreaEffectData = areaEffectdata;
                }
                //dword2A0 = v1->dword2A0;
                //if (dword2A0)
                //{
                //    v25 = ZNK21LogicGameObjectServer4getXEv(v1);
                //    v26 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //    v27 = ZN21LogicBattleModeServer12getRandomIntEii(v26, -100, 100);
                //    v28 = ZNK21LogicGameObjectServer4getYEv(v1);
                //    v29 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //    v30 = ZN21LogicBattleModeServer12getRandomIntEii(v29, -100, 100);
                //    dword2A0 = sub_1F4FCC(v1, v27 + v25, v30 + v28);
                //}
                ////}
            }

            //v145 = GetX();
            //v89 = GetY();
            //v90 = x - v145;
            //v91 = v145;
            //v92 = y - v89;
            //v144 = v89;
            //v93 = LogicMath.Sqrt(v92 * v92 + v90 * v90);
            //if (v93)
            //{
            //    v94 = v93;
            //    v90 = a1_divide_a2(v90 * 100 * a14, v93);
            //    v92 = a1_divide_a2(v92 * 100 * a14, v94);
            //}
            //v143 = 0;
            //v142[1] = 0;
            //v141 = 0;
            //v140[1] = 0;
            //v142[0] = 0;
            //v140[0] = 0;
            //sub_6F83B4(v142, &v145);
            //sub_6F83B4(v140, &v144);
            //v95 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v96 = sub_5F01A8(v95);
            //ZN12LogicVector2C2Ev(&v138);
            //v139 = -1;
            //v138 = -1;
            //v133 = v96;
            //v97 = v91 + v90;
            //v98 = v89 + v92;
            //if (ZN17LogicGamePlayUtil22getClosestAnyCollisionEiiiiPK12LogicTileMapP12LogicVector2bbbb(
            //       v145,
            //       v144,
            //       v97,
            //       v89 + v92,
            //       v96,
            //       &v138,
            //       0,
            //       0,
            //       0,
            //       0))
            //{
            //    v99 = v138 - v145;
            //    v100 = v139 - v144;
            //    v101 = LogicMath.Sqrt(v99 * v99 + v100 * v100);
            //    if (v101)
            //    {
            //        v102 = v101;
            //        v103 = LogicMath.Max(1, v101 - 150);
            //        v99 = a1_divide_a2(v103 * v99, v102);
            //        v100 = a1_divide_a2(v103 * v100, v102);
            //    }
            //    v98 = v144 + v100;
            //    v97 = v145 + v99;
            //}
            //v137 = LogicMath.Clamp(v97, 1, v133[26] - 2);
            //sub_6F83B4(v142, &v137);
            //v136 = LogicMath.Clamp(v98, 1, v133[27] - 2);
            //sub_6F83B4(v140, &v136);
            //sub_6F83B4(v142, &v145);
            //sub_6F83B4(v140, &v144);
            //if (a1->WayPoints >= 1)
            //{
            //    do
            //        sub_2234E4(a1);
            //    while (a1->WayPoints > 0);
            //}
            //v104 = GetX();
            //a1->field_54 = ZN12LogicTileMap21logicToPathFinderTileEi(v104);
            //v105 = GetY();
            //a1->field_58 = ZN12LogicTileMap21logicToPathFinderTileEi(v105);
            //a1->field_5C = ZN12LogicTileMap21logicToPathFinderTileEi(*(v142[0] + 4 * v143 - 4));
            //a1->field_60 = ZN12LogicTileMap21logicToPathFinderTileEi(*(v140[0] + 4 * v141 - 4));
            //v106 = v143;
            //v130 = v143 - 1;
            //if (v143 >= 1)
            //{
            //    v107 = 4 * v143 - 4;
            //    v108 = 0;
            //    v109 = &a1->YwayPoints;
            //    v110 = &a1->XwayPoints;
            //    do
            //    {
            //        v111 = (v142[0] + v107);
            //        if (v108)
            //        {
            //            sub_6F83B4(v110, v111);
            //            v112 = (v140[0] + v107);
            //            v113 = v109;
            //        }
            //        else
            //        {
            //            v114 = v106;
            //            v115 = *v111;
            //            v116 = *(v140[0] + 4 * v130);
            //            ZN12LogicVector2C2Ev(v134);
            //            v117 = sub_432FEC(a1);
            //            v118 = v115;
            //            v106 = v114;
            //            v109 = &a1->YwayPoints;
            //            sub_48C8C4(v117, v118, v116, 5, v133, v134, 0);
            //            sub_6F83B4(v110, v134);
            //            v113 = &a1->YwayPoints;
            //            v112 = &v135;
            //        }
            //        sub_6F83B4(v113, v112);
            //        --v108;
            //        v107 -= 4;
            //    }
            //    while (v106 + v108 > 0);
            //}
            //v119 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v120 = ZN21LogicBattleModeServer13getPathFinderEv(v119);
            //ZN20LogicCharacterServer12ensurePathOkEP15LogicPathFinder(a1, v120);
            //v121 = ZNK20LogicCharacterServer13getPathLengthEv(a1);
            //*&a1->MoveLength = v121;
            //if (v121 <= 0)
            //    JUMPOUT(0x324724);
            //v122 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //ZNK21LogicBattleModeServer12getTicksGoneEv(v122);
            //return sub_3246FC(*&a1->MoveLength);
        }
        public int GetActiveSkillShield()
        {
            foreach (Skill skill in m_skills)
            {
                if (skill.SkillData.BehaviorType == "Shield" && skill.TicksActive > 0) return skill.SkillData.MsBetweenAttacks;
            }
            return 0;
        }
        public Projectile GetClosestProjectileFlyingAgainstYou()
        {
            Projectile closestEnemy = null;
            int distance = 99999999;

            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 1) continue; // not a character, ignore.

                Projectile enemy = (Projectile)gameObject;
                if (enemy == null) continue; // invalid object
                if (enemy.GetIndex() / 16 == GetIndex() / 16 || enemy.State == 4) continue; // teammate, ignore.

                int distanceToEnemy = Position.GetDistance(enemy.GetPosition());
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            return closestEnemy;
        }
        public Character GetClosestEnemy(int RangeSubTiles, int Type, bool IgnoreVisibility, bool IgnoreVisibility2, List<int> IgnoreTargets, bool RequireLOS, bool TargetTeammates, bool TargetFullHP)
        {
            Character closestEnemy = null;
            int distance = RangeSubTiles * 100;

            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 0) continue; // not a character, ignore.

                Character enemy = (Character)gameObject;
                if (enemy == null) continue; // invalid object
                if (enemy.GetIndex() / 16 == GetIndex() / 16 || enemy.IsImmuneAndBulletsGoThrough(IsInRealm) || enemy.IsInvincible) continue; // teammate, ignore.
                if (enemy.GetFadeCounter() < 1 && !IgnoreVisibility) continue;

                int distanceToEnemy = Position.GetDistance(enemy.GetPosition());
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            return closestEnemy;
        }
        public Character GetClosestEnemy()
        {
            Character closestEnemy = null;
            int distance = 99999999;

            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 0) continue; // not a character, ignore.

                Character enemy = (Character)gameObject;
                if (enemy == null) continue; // invalid object
                if (enemy.GetIndex() / 16 == GetIndex() / 16 || enemy.IsImmuneAndBulletsGoThrough(IsInRealm) || enemy.IsInvincible) continue; // teammate, ignore.

                int distanceToEnemy = Position.GetDistance(enemy.GetPosition());
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }

            return closestEnemy;
        }

        public Character GetClosestVisibleEnemy()
        {
            Character closestEnemy = null;
            int distance = 99999999;

            foreach (GameObject gameObject in GameObjectManager.GetVisibleGameObjects(GetIndex(), GetGlobalID()))
            {
                if (gameObject.GetObjectType() != 0) continue; // not a character, ignore.

                Character enemy = (Character)gameObject;
                if (enemy == null) continue; // invalid object
                if (enemy.GetIndex() / 16 == GetIndex() / 16 || enemy.IsInvisible) continue; // teammate, ignore.

                int distanceToEnemy = Position.GetDistance(enemy.GetPosition());
                if (distanceToEnemy < distance)
                {
                    closestEnemy = enemy;
                    distance = distanceToEnemy;
                }
            }
            //Debugger.Print(closestEnemy.GetDataId().ToString());
            return closestEnemy;
        }
        private int GetRegeneratePerSecond()
        {
            if (CharacterData.IsHero())
            {
                return 13 * m_maxHitpoints / 100;
            }
            return 0;
        }
        public int GetCardValueForPassive(string a2, int a3)
        {
            if (StarPowerData != null)
            {
                if (StarPowerData.Type == a2)
                {
                    if (a3 == 2) return StarPowerData.Value3;
                    return a3 > 0 ? StarPowerData.Value : StarPowerData.Value2;
                }
                else return -1;

            }
            else
                return -1;
        }
        private void TickHeals()
        {
            if (!CharacterData.IsHero()) return;
            if (IsInvincible)
            {
                HealthRegenBlockedTick = TicksGone;
            }
            if (ExtraRegenTicks >= 1)
            {
                ExtraRegenTicks--;
                if ((ExtraRegenTicks + 1) % 20 == 0) Heal(ExtraRegenerIndex, ExtraRegenAmount, true, null);
            }
            foreach (Buff buff in m_buffs)
            {
                if (buff.Type == 10 && buff.Duration % 20 == 0) Heal(buff.SourceIndex, buff.Modifier, true, null);
            }
            int v6 = TicksGone;

            int v8 = GetCardValueForPassive("repair_self", 1);
            int result;
            if (v8 < 0) result = 60;
            else
            {
                result = 60 - v8;
                if (result <= 21) result = 21;
            }
            if (TicksGone >= HealthRegenedTick + 20)
            {
                int v7 = GetRegeneratePerSecond();
                if ((v7 < 0 || v6 >= HealthRegenBlockedTick + result) && v7 > 0 && v6 >= DEPLOY_TICKS_FIRST_SPAWN)
                {
                    if (m_hitpoints < m_maxHitpoints) v7 += v7 * GetGearBoost(1) / 100;
                    if (v7 < 1)
                    {
                        //v13 = GetX();
                        //v14 = GetY();
                        //ZN20LogicCharacterServer11causeDamageEiiiPS_biiPK9LogicDatabbbbb(
                        //  a1,
                        //  -1,
                        //  -v7,
                        //  0,
                        //  0,
                        //  0,
                        //  v13,
                        //  v14,
                        //  0,
                        //  1,
                        //  0,
                        //  0,
                        //  0,
                        //  0);
                    }
                    else
                    {
                        Heal(GetIndex(), v7, false, CharacterData);
                        HealthRegenedTick = TicksGone;
                    }
                }
            }
            if (TicksGone >= HealthRegenedTickSP + 20)
            {
                bool v10 = false;
                int v15 = GetCardValueForPassive("heal_forest", 1);
                if (v15 >= 0)
                {
                    //v16 = v15;
                    //v17 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
                    //v18 = ZN21LogicBattleModeServer10getTileMapEv(v17);
                    //v19 = GetX();
                    //v20 = (1431655766LL* ZN12LogicTileMap21logicToPathFinderTileEi(v19)) >> 32;
                    //v21 = GetY();
                    //v22 = ZN12LogicTileMap21logicToPathFinderTileEi(v21);
                    //if (*(ZNK12LogicTileMap7getTileEii(v18, v20 + (v20 >> 31), v22 / 3) + 48))
                    //{
                    //    v23 = Index;
                    //    v24 = ZNK21LogicGameObjectServer7getDataEv(a1);
                    //    v10 = 1;
                    //    ZN20LogicCharacterServer4healEiibPK9LogicData(a1, v23, v16, 1, v24);
                    //}
                    if (IsInBush())
                    {
                        Heal(GetIndex(), v15, true, CharacterData);
                        v10 = true;
                    }
                }
                //bool active = ZNK20LogicCharacterServer14getActiveSkillEi(a1, 3);
                //v26 = active == 0;
                //v51 = v6;
                //if (active)
                //    v26 = Invisible == 0;
                if (GetActiveSkill("Invisibility") != null && IsInvisible)
                {
                    int v42 = GetCardValueForPassive("heal_invisible", 1);
                    if (v42 >= 0)
                    {
                        Heal(GetIndex(), v42, true, CharacterData);
                        v10 = true;
                    }
                }
                if (AoeRegenerate >= 1)
                {
                    foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                    {
                        if (gameObject.GetObjectType() != 0) continue;
                        Character v31 = (Character)gameObject;
                        if (v31.IsAlive() && v31.GetIndex() / 16 == GetIndex())
                        {
                            if (v31 != this && v31.CharacterData.IsHero() && v31.m_hitpoints < v31.m_maxHitpoints)
                            {
                                AreaEffectData v49 = DataTables.GetAreaEffectByName("AutoHeal");
                                int v48 = v31.GetX();
                                int v36 = GetX();
                                int v47 = v31.GetY();
                                int v46 = GetY();
                                int v37 = AreaEffect.GetRangeAgainst(v49, v31) - 100;
                                if (LogicMath.Abs(v48 - v36) <= v37
                                  && LogicMath.Abs(v47 - v46) <= v37
                                  && (v48 - v36) * (v48 - v36) + (v47 - v46) * (v47 - v46) <= (v37 * v37))
                                {
                                    AreaEffect v38 = GameObjectFactory.CreateGameObjectByData(v49);
                                    v38.SetPosition(GetX(), GetY(), 0);
                                    v38.SetIndex(GetIndex());
                                    v38.SetSource(this, 3);
                                    v38.Damage = -AoeRegenerate;
                                    v38.NormalDMG = 0;
                                    GameObjectManager.AddGameObject(v38);
                                    v38.Trigger();
                                    HealthRegenedTick = TicksGone;
                                    v10 = true;
                                    break;
                                }
                            }
                        }
                    }

                }
                if (GetCardValueForPassive("heal_continuous", 1) >= 1)
                {
                    if (TicksGone >= HealthRegenedTickSP + GetCardValueForPassive("heal_continuous", 0))
                    {
                        Heal(GetIndex(), (m_maxHitpoints - m_hitpoints) * GetCardValueForPassive("heal_continuous", 1) / 100, false, CharacterData);
                        v10 = true;
                    }
                }
                //result = v51;
                if (v10)
                    HealthRegenedTickSP = TicksGone;

            }
        }
        public bool IsObject()
        {
            return !CharacterData.HasAutoAttack() && CharacterData.Speed == 0;
        }
        public void TriggerStun(int a2, bool a3)
        {
            if (a2 >= 1)
            {
                if ((!IsInvincible && !HasCcImmunity()) || a3)
                {
                    m_stunTicks = a2;
                    InterruptAllSkills(false);
                    if (GetCardValueForPassive("ulti_defense", 1) >= 0)
                    {
                        ShieldTicks = 0;
                    }
                    if ((!IsCharging && !KnockBacked) || a3)
                    {
                        StopMovement();
                    }
                    if (GetControlledProjectile() != null)
                    {
                        GetControlledProjectile().TargetReached(5);
                    }
                }
                IsWeaklyStunned = false;
            }
        }
        public bool Heal(int DealerIndex, int healing, bool shouldShow, LogicData projectileData)
        {
            if (m_hitpoints >= 1)
            {
                if (Immunitys.Count >= 1)
                {
                    foreach (Immunity immunity in Immunitys)
                    {
                        if (immunity.TargetIndex == DealerIndex && immunity.TargetData == projectileData) return false;
                    }
                }
                int HP = m_hitpoints;
                int v23 = LogicMath.Min(m_maxHitpoints, HP + healing);
                m_hitpoints = v23;
                if (shouldShow && m_hitpoints > HP) DamageNumbers.Add(new DamageNumber(-healing, DealerIndex, 0));
                return true;
            }
            return false;
        }
        public void ChargeUlti(int a2, bool a3, bool a4, BattlePlayer a5, Character a6)
        {
            if (!a4) a2 += (a2 * a5.GetGearBoost(6) + 50) / 100;
            ((Character)GameObjectManager.GetGameObjectByID(a5.OwnObjectId))?.GetGearBoost(6);
            a5.ChargeUlti(a2, a3, a4);
        }
        public bool CauseDamage(Character damageDealer, int damage, int NormalDMG, bool SkillType, LogicData source, bool shouldShow = true, bool IgnoreInvincible = false, int X = -1, int Y = -1)
        {
            int index = damageDealer == null ? -1 : damageDealer.GetIndex();
            bool result = false;
            if (IgnoreInvincible) goto LABEL_1;
            if (IsInvincible)
            {
                if (shouldShow) ShowDamageNumber(index, 0, damageDealer);
                return false;
            }
            if (HasBuff(15)) damage += damage * 35 / 100;
            //if (IsImmuneAndBulletsGoThrough())
            //{
            //    return false;
            //}
            if (Immunitys.Count >= 1)
            {
                foreach (Immunity immunity in Immunitys)
                {
                    if (immunity.TargetData == source && immunity.TargetIndex == damageDealer?.GetIndex()) return false;
                }
            }
            if (m_hitpoints <= 0) return result;
            if (IsWeaklyStunned)
            {
                if (m_stunTicks > 0) m_stunTicks = 0;
                IsWeaklyStunned = false;
            }
            if (Accessory != null && (Accessory.Type == "return_damage" || Accessory.Type == "reload_shield") && Accessory.IsActive)
            {
                if (Accessory.Type == "return_damage")
                {
                    if (damageDealer != null)
                    {
                        Projectile nani = Projectile.ShootProjectile(-1, -1, this, this, DataTables.GetProjectileByName(Accessory.AccessoryData.CustomObject), damageDealer.GetX(), damageDealer.GetY(), Accessory.AccessoryData.CustomValue1 * damage / 100, 0, 0, false, 0, GetBattle(), 0, 4);
                        nani.HomingTarget = damageDealer;
                    }
                }
                else if (Accessory.Type == "reload_shield")
                {
                    GetWeaponSkill().AddCharge(this, 100 * Accessory.AccessoryData.CustomValue2);
                }
                damage -= damage * Accessory.AccessoryData.CustomValue1 / 100;
                Accessory.EndAccessoryActivation();
            }
            if (Accessory != null && Accessory.Type == "take_damage" && Accessory.AccessoryData.SubType == 3 && Accessory.IsActive)
            {
                Accessory.RewindHP += damage;
            }
            //if (Accessory != null && Accessory.IsActive)
            //{
            //    foreach (Projectile projectile in GameObjectManager.GetProjectiles())
            //    {
            //        if (projectile.GetIndex() == GetIndex() && projectile.State == 0) projectile.TriggerDelay = TicksGone + 2;
            //    }
            //}
            int shield = 0;
            if (ShieldTicks > 0)
            {
                shield += ShieldPercent;
            }
            else shield = GetActiveSkillShield();
            if (GetCardValueForPassive("reduce_all_damage", 1) >= 1) shield += GetCardValueForPassive("reduce_all_damage", 1);
            if (GetCardValueForPassive("running_reduces_damage", 1) >= 1 && X != -1)
            {
                int Angle = LogicMath.NormalizeAngle360(LogicMath.GetAngle(X - GetX(), Y - GetY()) - 180);
                if (Angle >= MoveAngle - GetCardValueForPassive("running_reduces_damage", 0) / 2 && Angle <= MoveAngle + GetCardValueForPassive("running_reduces_damage", 0) / 2) shield += GetCardValueForPassive("running_reduces_damage", 1);
            }
            if (GetPlayer()?.OverCharging ?? false)
            {
                if (CharacterData.OverchargeShieldPercent > 0)
                    shield += CharacterData.OverchargeShieldPercent;
                else shield += 20;
            }
            if (shield >= 100)
            {
                ShowDamageNumber(index, 0, damageDealer);
                return false;
            }

            damage = LogicMath.Max(1, damage - shield * damage / 100);

            if (shouldShow) ShowDamageNumber(index, damage, damageDealer);
            if (GetCardValueForPassive("reduce_damage_periodically", 1) >= 1 && ChargeUp >= ChargeUpMax)
            {
                int v = LogicMath.Min(damage * GetCardValueForPassive("reduce_damage_periodically", 2) / 100, GetCardValueForPassive("reduce_damage_periodically", 0));
                damage -= v;
                ChargeUp = 0;
            }
            if (ConsumableShield >= 1)
            {
                int v51 = LogicMath.Min(damage, ConsumableShield);
                damage -= v51;
                ConsumableShield -= v51;
            }
            if (Gear1 != null)
            {
                if (Gear1.ShieldAmount >= 1)
                {
                    int dr = LogicMath.Min(damage, Gear1.ShieldAmount);
                    damage -= dr;
                    Gear1.ShieldAmount -= dr;
                    GetGearBoost(4);
                }
            }
            if (Gear2 != null)
            {
                if (Gear2.ShieldAmount >= 1)
                {
                    int dr = LogicMath.Min(damage, Gear2.ShieldAmount);
                    damage -= dr;
                    Gear2.ShieldAmount -= dr;
                    GetGearBoost(4);
                }
            }
            if (source == null)
            {
                if (damageDealer != null)
                {
                    if (damageDealer.CharacterData.IsHero())
                    {
                        BattlePlayer v44 = damageDealer.GetPlayer();
                        if (v44 != null && v44.SkinData != null)
                        {
                            HitEffectSkin = v44.SkinData.GetInstanceId() + 1;
                        }
                        else HitEffectSkin = DataTables.Get(DataType.Skin).GetData<SkinData>(damageDealer.CharacterData.DefaultSkin).GetInstanceId() + 1;
                    }
                }
            }
            else if (source.GetDataType() == 6) HitEffectProjectile = source.GetInstanceId() + 1;

            HandleHealFromDamage(damage, SkillType, damageDealer);

            //Handle ChargeUltiFromDamage

            //Handle Bea Starpower

            //Handle Jacky Starpower
            int v55 = GetCardValueForPassive("reflect_damage_aoe", 1);
            if (v55 >= 1)
            {
                if (v55 * damage >= 100) TriggerAreaEffect(DataTables.GetAreaEffectByName("DrillerReflectArea"), GetX(), GetY(), v55 * damage / 100, 3);
            }

        LABEL_1:
            result = damage > 0;
            m_hitpoints -= damage;
            m_hitpoints = LogicMath.Max(m_hitpoints, 0);
            m_hitpoints = LogicMath.Min(m_hitpoints, m_maxHitpoints);

            if (damage > 0)
            {
                BlockHealthRegen();
                SetForcedVisible();
            }

            BattleMode battle = GameObjectManager.GetBattle();


            if (CharacterData.IsHero() || CharacterData.IsTrainingDummy())
            {
                if (damageDealer != null && damageDealer != this && !IsControlled)
                {
                    BattlePlayer enemy = battle.GetPlayerWithObject(damageDealer.GetGlobalID());
                    if (enemy != null)
                    {
                        if (damage > 0)
                        {
                            ChargeUlti(NormalDMG, SkillType, false, enemy, damageDealer);
                            if (damageDealer.CharacterData.UniqueProperty == 1)
                            {
                                damageDealer.AshLastRageTick = TicksGone;
                                if (SkillType) damageDealer.ChargeUp += NormalDMG / 2;
                                else damageDealer.ChargeUp += NormalDMG / 6 + NormalDMG / 6 * damageDealer.GetPowerLevel() / 4;//160 per mouse mouse damage:300
                                damageDealer.ChargeUp = LogicMath.Clamp(damageDealer.ChargeUp, 0, damageDealer.ChargeUpMax);
                            }
                            enemy.DamageDealed(damage);
                        }
                    }
                }

                if (m_hitpoints <= 0)
                {
                    Projectile v113 = GetControlledProjectile();
                    if (v113 != null) v113.TargetReached(5);

                    BattlePlayer player = battle.GetPlayerWithObject(GetGlobalID());
                    if (CharacterData.UniqueProperty == 2)
                    {
                        Character n = TriggerTransforming(DataTables.GetCharacterByName("MechaDude"));
                        n.m_hitpoints = n.m_maxHitpoints;
                        n.m_skills[0].Charge = 1000;
                        player.OwnObjectId = n.GetGlobalID();
                        if (GetCardValueForPassive("mecha_explosion", 1) >= 1)
                        {
                            TriggerAreaEffect(DataTables.GetAreaEffectByName("MechaDudeDestroyExplosion"), GetX(), GetY(), 0, 3);
                        }
                        if (GetCardValueForPassive("downgrade_shield", 1) >= 1) n.AddShield(GetCardValueForPassive("downgrade_shield", 0), GetCardValueForPassive("downgrade_shield", 1));
                        n.TriggerPushback(n.GetX(), n.GetY(), 50, true, 0, false, false, false, true, false, false, false, 0);
                        n.Knockback3 = true;
                        return result;
                    }

                    if (player != null)
                    {
                        battle.PlayerDied(player);
                        if (player.Accessory != null) player.Accessory.Interrupt(false, this);
                    }
                    if (damageDealer != null)
                    {
                        if (damageDealer.GetCardValueForPassive("steal_souls2", 1) >= 0)
                        {
                            Projectile v125 = Projectile.ShootProjectile(-1, -1, this, this, DataTables.GetProjectileByName("HammerDudeStarPowerProjectile"), damageDealer.GetX(), damageDealer.GetY(), damageDealer.GetCardValueForPassive("steal_souls2", 1), 0, 0, false, 0, GetBattle(), 0, 3);
                            v125.HomingTarget = damageDealer;
                            v125.SetStealSouls2BuffingData(damageDealer.GetCardValueForPassive("steal_souls2", 1), 20 * damageDealer.GetCardValueForPassive("steal_souls2", 0));
                            v125.SetIndex(damageDealer.GetIndex());
                        }
                        if (IsInRealm)
                        {
                            damageDealer.IsInRealm = false;
                            IsInRealm = false;
                            GameObjectManager.RemoveGameObject(RealmEffect);
                        }
                        BattlePlayer enemy = battle.GetPlayerWithObject(damageDealer.GetGlobalID());
                        if (GameObjectManager.GetBattle().GetGameModeVariation() == 3)
                        {
                            if (enemy != null)
                            {
                                damageDealer.AddItemsCollected(1);
                                enemy.AddScore(m_itemCount + 1);
                            }
                        }

                        if (enemy != null)
                        {
                            int bountyStars = GameObjectManager.GetBattle().GetGameModeVariation() == 3 ? m_itemCount + 1 : 0;
                            enemy.KilledPlayer(GetIndex() % 16, bountyStars);
                            if (HasBuff(15))
                            {
                                if (GetBuff(15).BelleWeaponBounces >= 0) enemy.AddUltiCharge(2000);
                            }
                        }
                    }

                    if (GameObjectManager.GetBattle().GetGameModeVariation() == 0)
                    {
                        ItemData data = DataTables.Get(18).GetData<ItemData>("Point");
                        for (int i = 0; i < m_itemCount; i++)
                        {
                            int v165 = GetBattle().GetRandomInt(300, 600);
                            int v167 = GetBattle().GetRandomInt(360);
                            int v168 = LogicMath.GetRotatedX(v165, 0, v167);
                            int v169 = LogicMath.GetRotatedY(v165, 0, v167);
                            int v160 = LogicMath.Clamp(GetX() + v168, 1, MapWidth - 2);
                            int v172 = LogicMath.Clamp(GetY() + v169, 1, MapHeight - 2);
                            LogicVector2 logicVector2 = new();
                            GamePlayUtil.GetClosestPassableSpot(0, v160, v172, 10, GetBattle().GetTileMap(), logicVector2, true);
                            v160 = logicVector2.X;
                            v172 = logicVector2.Y;
                            Item item = new Item(data);
                            item.SetFromToPosition(GetX(), GetY(), GetZ(), v160, v172, 0);
                            item.SetAngle(GameObjectManager.GetBattle().GetRandomInt(0, 360));
                            GameObjectManager.AddGameObject(item);
                        }
                    }

                }
            }

            if (m_hitpoints <= 0)
            {
                if (CharacterData.DeathAreaEffect != null)
                {
                    TriggerAreaEffect(DataTables.GetAreaEffectByName(CharacterData.DeathAreaEffect), GetX(), GetY(), 0, 2);
                }
                if (AreaEffect != null) GameObjectManager.RemoveGameObject(AreaEffect);

            }
            return result;
        }

        public void AddItemsCollected(int a)
        {
            m_itemCount += a;
            if (GameObjectManager.GetBattle().GetGameModeVariation() == 3)
            {
                m_itemCount = LogicMath.Min(6, m_itemCount);
            }
        }

        public void ResetItemsCollected()
        {
            m_itemCount = 0;
        }

        public void HoldSkillStarted()
        {
            SkillHoldTicks = 0;
        }
        public void EndRapidFire()
        {
            Attacking = false;
            if (!UltiSkill && Accessory != null && Accessory.IsActive && Accessory.Type == "next_attack_change" && Accessory.AccessoryData.ActiveTicks == 100000)
            {
                Accessory.EndAccessoryActivation();
            }
            if (CharacterData.UniqueProperty == 11 && UltiSkill)
            {
                string skill = null;
                SkillData skillData = m_skills[1].SkillData;
                switch (GetBattle().GetRandomInt(0, 4))
                {
                    case 0:
                        skill = skillData.SecondarySkill;
                        break;
                    case 1:
                        skill = skillData.SecondarySkill2;
                        break;
                    case 2:
                        skill = skillData.SecondarySkill3;
                        break;
                    case 3:
                        skill = skillData.SecondarySkill4;
                        break;
                }
                m_skills[1] = new Skill(DataTables.Get(DataType.Skill).GetData<SkillData>(skill).GetGlobalId(), true);
            }
        }

        public Skill GetWeaponSkill()
        {
            return m_skills.Count > 0 ? m_skills[0] : null;
        }

        public Skill GetUltimateSkill()
        {
            return m_skills.Count > 1 ? m_skills[1] : null;
        }
        public int GetPowerLevel()
        {
            if (CharacterData.UniqueProperty == 1) return LogicMath.Clamp(3 * ChargeUp / ChargeUpMax, 0, 2);
            if (GetPlayer() != null) return GetPlayer().PowerLevel;
            return 0;
        }
        public void InterruptAllSkills(bool IgnoreAccessory = true)
        {
            ChargeAnimation = 0;
            IsCharging = false;
            for (int i = 0; i < m_skills.Count; i++)
            {
                m_skills[i].Interrupt(this);
            }
            if (GetPlayer() == null || GetPlayer().Accessory == null) return;
            GetPlayer().Accessory.Interrupt(IgnoreAccessory, this);
        }
        public void AddAreaEffect(int a2, int a3, AreaEffectData a4, int a5, bool a6)
        {
            int v12; // w25
            int v13; // w0
            int v14; // x0
            int v15; // x8
            int v16; // w0 MAPDST
            int v17; // x22
            int v19; // w8
            int v20; // w20
            int v21; // w0
            BattlePlayer v22; // x0
            SkinData v23; // x24
            int v25; // x0

        LABEL_2:
            if (a4 != null)
            {
                AreaEffect = new AreaEffect(a4);
                AreaEffect.SetPosition(GetX(), GetY(), 0);
                AreaEffect.SetIndex(GetIndex());
                AreaEffect.SetSource(this, a5);
                //v15 = a1[92];
                AreaEffect.aoe_dot = a3;
                //*(v15 + 172) = a6 & 1;
                v16 = a4.Damage;
                if (v16 < 1)
                {
                    v21 = 0;
                    v20 = v16 - a2;
                }
                else
                {
                    v20 = v16 + a2;
                    v21 = a4.Damage;
                }
                AreaEffect.Damage = v20;
                AreaEffect.NormalDMG = v21;
                GameObjectManager.AddGameObject(AreaEffect);
                AreaEffect.Trigger();
                return;
            }
            v22 = GetPlayer();
            if (v22 != null)
                v23 = v22.SkinData;
            else
                v23 = null;
            if (GetCardValueForPassiveFromPlayer("larger_area_ulti", 1) >= 1)
            {
                if (v23 != null)
                {
                    a4 = DataTables.GetAreaEffectByName(DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(v23.Conf).AreaEffectStarPower);
                    if (a4 != null)
                        goto LABEL_2;
                }
                a4 = DataTables.GetAreaEffectByName("DamageBoost_larger");
                if (a4 == null)
                    return;
                goto LABEL_2;
            }
            else if (GetCardValueForPassiveFromPlayer("plugged_in", 1) >= 1)
            {
                if (v23 != null)
                {
                    a4 = DataTables.GetAreaEffectByName(DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(v23.Conf).AreaEffectStarPower2);
                    if (a4 != null)
                        goto LABEL_2;
                }
                a4 = DataTables.GetAreaEffectByName("DamageBoostPlugIn");
                if (a4 == null)
                    return;
                goto LABEL_2;
            }
            else if (GetCardValueForPassiveFromPlayer("super_charge_area", 1) >= 1)
            {
                a4 = DataTables.GetAreaEffectByName("RopeDudeSuperChargeAreaLarge");
                if (a4 == null)
                    return;
                goto LABEL_2;
            }
            else
            {
                if (v23 != null)
                {
                    a4 = DataTables.GetAreaEffectByName(DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(v23.Conf).AreaEffect);
                    if (a4 != null)
                        goto LABEL_2;
                }
            }
            //if (v23)
            //{
            //    a4 = *(*(v23 + 168) + 352LL);
            //    if (a4)
            //        goto LABEL_2;
            //}
            a4 = DataTables.GetAreaEffectByName(CharacterData.AreaEffect);
            if (a4 != null)
                goto LABEL_2;

        }
        public void BlockHealthRegen()
        {
            HealthRegenBlockedTick = TicksGone;
        }

        public void ActivateSkill(int type, int x, int y)
        {
            if (IsPlayerControlRemoved() || (Accessory != null && Accessory.AccessoryData.InterruptsAction && Accessory.IsActive)) return;
            //State = 3;

            Skill skill = m_skills[type];
            if (skill == null) return;
            if (skill.IsActive || skill.CoolDown > 0) return;
            //if (skill.SkillData.BehaviorType == "Charge") return;

            TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();
            AttackAngle = LogicMath.GetAngle(x - GetX(), y - GetY());
            skill.Activate(this, x, y, tileMap);
            //m_attackingTicks = 0;
            if (skill.SkillData.ChargeType == 1)
            {
                DisplayUltiTmp = true;
            }
            if (skill.SkillData.ChargeType == 15)
            {
                SteerAngle = LogicMath.GetAngle(x - GetX(), y - GetY());
                MoveAngle = SteerAngle;
                m_activeChargeType = 15;
                ChargeSpeed = skill.SkillData.ChargeSpeed;
                SteerStartTicks = TicksGone;
            }
            //if (!string.IsNullOrEmpty(skill.SkillData.AreaEffectObject))
            //{
            //    AreaEffectData effectData = DataTables.Get(17).GetData<AreaEffectData>(skill.SkillData.AreaEffectObject);
            //    AreaEffect effect = new AreaEffect(17, effectData.GetInstanceId());
            //    effect.SetPosition(GetX(), GetY(), 0);
            //    effect.SetSource(this);
            //    effect.SetIndex(GetIndex());
            //    effect.SetDamage(skill.SkillData.Damage);
            //    GameObjectManager.AddGameObject(effect);
            //}
        }
        public bool HasUlti()
        {
            int v2; // r0
            BattlePlayer v3; // r0
            int v5; // r0

            if (!CharacterData.IsHero())
            {
                return CharacterData.IsBoss();
            }
            v3 = GetPlayer();
            if (v3 != null)
                return v3.HasUlti();
            return false;
        }
        public AreaEffect TriggerAreaEffect(
        AreaEffectData a2,
        int a3,
        int a4,
        int a5,
        int a6)
        {
            AreaEffect v9; // r5
            int v10; // r6

            v9 = new AreaEffect(a2);
            v9.SetPosition(a3, a4, 0);
            v9.SetIndex(GetIndex());
            v9.SetSource(this, a6);
            v10 = 0;
            if (a6 == 1)
                v10 = v9.GetDamage(0);
            v9.Damage = v9.GetDamage(0) + a5;
            v9.NormalDMG = v10;
            GameObjectManager.AddGameObject(v9);
            v9.Trigger();
            return v9;
            //*(a1 + 448) = 0;
        }
        //private int SpreadIndex = 0;
        void GetShootPositionModifiers(
        int a1,
        int a2,
        int a3,
        int a4,
        int a5,
        int a6,
        int a7,
        int a8,
        int a9)
        {
            int v9; // r5
            int v11; // r4
            int v12; // r0
            int v13; // r10
            int v14; // r9
            int v15; // r8
            int v16; // r7
            int v17; // r5
            int v18; // r0
            int v19; // r3
            int result; // r0
            int v21; // r2
            int v22; // r2
            int v23; // r3

            v9 = a3 - a1;
            v11 = a4 - a2;
            //v9 = 1;
            //v11 = 1;
            v12 = LogicMath.Sqrt(v9 * v9 + v11 * v11);
            v13 = v9;
            v14 = v11;
            if (v12 != 0)
            {
                v13 = v9 * a6 / v12;
                v14 = v11 * a6 / v12;
            }
            v15 = a5;
            v16 = LogicMath.GetRotatedX(v9, v11, 90);
            v17 = LogicMath.GetRotatedY(v9, v11, 90);
            //Debugger.Print("X:" + (v16-GetX()) + " Y:" + (v17-GetY()));
            v18 = LogicMath.Sqrt(v16 * v16 + v17 * v17);
            if (v18 != 0)
            {
                v16 = v16 * a7 / v18;
                v15 = a5;
                v17 = v17 * a7 / v18;
            }
            //Debugger.Print("X:" + (v16 ) + " Y:" + (v17));

            v19 = v13 + a1;
            result = a9;
            v21 = v14 + a2;
            if (v15 != 0)
            {
                v22 = v21 - v17;
                v23 = v19 - v16;
                v17 = -v17;
                v16 = -v16;
            }
            else
            {
                v22 = v17 + v21;
                v23 = v16 + v19;
            }
            AttackOffsetX = v23;
            AttackOffsetY = v22;
            AttackOffset2X = v16;
            AttackOffset2Y = v17;
        }
        public void MeleeAttack(Character Target, int X, int Y, int Damage, int NormalDMG)
        {//Self Confident
            if (Target != null)
            {
                if (GamePlayUtil.GetDistanceBetween(GetX(), GetY(), Target.GetX(), Target.GetY()) <= CharacterData.AutoAttackRange * 100)
                {
                    if (Target.CauseDamage(this, Damage, NormalDMG, UltiSkill, null))
                    {
                        ;
                    }
                }
            }
        }
        public void RapidFireMeleeAttack(
        int X,
        int Y,
        int Damage,
        int NormalDMG,
        int Spread)
        {
            int v9; // r0
            int v10; // r5
            int v11; // r4
            int v15; // r5
            int v17; // r4
            int v18; // r8
            int v19; // r4
            int v20; // r4
            int v21; // r10
            int v22; // r0
            int v23; // r4
            float v24; // r0
            float v25; // r0
            int v26; // r1
            int v27; // r8
            int v28; // r10
            int v29; // r4
            int v30; // r7
            int v31; // r0
            int v32; // r3
            int v33; // r8
            int v34; // r7
            int v35; // r0
            bool v36; // r0
            bool v37; // zf
            int v38; // r4
            int v39; // r0
            int v40; // r0
            int v41; // r5
            int v42; // r0
            int v43; // r8
            int v44; // r7
            int v45; // r5
            int result; // r0
            int v47; // r10
            int v48; // r0
            //signed int v49; // r8
            //_DWORD* v50; // r0
            int v51; // r9
            int v52; // r6
            int v53; // r5
            //signed int v54; // r4
            int v55; // r5
            int v56; // r0
            float v57; // r0
            int v58; // r1
            int v59; // r0
            int v60; // r0
            int v61; // r4
            int v62; // r0
            int v64; // [sp+8h] [bp-70h]
            int v65; // [sp+34h] [bp-44h]
            int v66; // [sp+38h] [bp-40h]
            int v67; // [sp+3Ch] [bp-3Ch]
            int v68; // [sp+40h] [bp-38h]
            int v69; // [sp+44h] [bp-34h]
            int v71; // [sp+4Ch] [bp-2Ch]
            int v72; // [sp+50h] [bp-28h]
            int v73; // [sp+54h] [bp-24h]
            int v74; // [sp+54h] [bp-24h]
            //_DWORD* v75; // [sp+58h] [bp-20h]

            //v75 = ZN21LogicBattleModeServer10getTileMapEv(v9);
            v10 = X - GetX();
            v11 = Y - GetY();
            v71 = LogicMath.Sqrt(v10 * v10 + v11 * v11);
            v69 = LogicMath.GetAngle(v11, v10);
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 0) continue;
                Character v16 = (Character)gameObject;
                if (v16.IsAlive())
                {
                    if (v16.GetIndex() / 16 != GetIndex() / 16)
                    {
                        v17 = v16.GetX();
                        v18 = v17 - GetX();
                        v19 = v16.GetY();
                        v20 = v19 - GetY();
                        v21 = LogicMath.Sqrt(v18 * v18 + v20 * v20);
                        if (v21 <= v16.CharacterData.CollisionRadius + v71)
                        {
                            v22 = LogicMath.GetAngle(v20, v18);
                            v23 = LogicMath.GetAngleBetween(v69, v22);
                            v24 = GamePlayUtil.WeaponSpreadToAngleRad(Spread);
                            v25 = GamePlayUtil.RadToDeg(v24);
                            v26 = 0;
                            if (v21 > 300)
                                v26 = 10;
                            if (v21 <= 300 || v23 <= v26 + v25 / 2)
                            {
                                v27 = v16.CharacterData.CollisionRadius + 80;
                                v28 = GetX();
                                v29 = GetY();
                                v30 = v16.GetX();
                                v31 = v16.GetY();
                                v32 = v30;
                                //v16.SetPosition(v16.GetX() - 200, v16.GetY(),0);
                                //if (GameObjectManager.GetBattle().GetTileMap().IsPlayerLineOfSightAlmostClear(v28, v29, v32, v31, v27))
                                if (GameObjectManager.GetBattle().GetTileMap().CalculateLOSWithCollision(v28, v29, v32, v31, v27 * 2))
                                {
                                    v33 = GetIndex();
                                    v34 = GetX();
                                    v35 = GetY();
                                    v64 = v34;
                                    //v36 = ZN20LogicCharacterServer11causeDamageEiiiPS_biiPK9LogicDatabbbbb(
                                    //        v16,
                                    //        v33,
                                    //        Damage,
                                    //        NormalDMG,
                                    //        v72,
                                    //        1,
                                    //        v64,
                                    //        v35,
                                    //        0,
                                    //        0,
                                    //        *(v72 + 441),
                                    //        0,
                                    //        1,
                                    //        0);
                                    v36 = v16.CauseDamage(this, Damage, NormalDMG, UltiSkill, null);
                                    if (v36 && MeleeAttackPushBacks)
                                    {
                                        v16.TriggerPushback(GetX(), GetY(), MeleeAttackPushBack, false, 0, false, false, false, false, false, false, false, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MeleeAttackPushBacks = false;
            //v40 = ZNK21LogicGameObjectServer4getXEv(v72);
            //v41 = sub_21394C(v40);
            //v42 = ZNK21LogicGameObjectServer4getYEv(v72);
            //v43 = sub_21394C(v42);
            //v44 = LogicMath.Clamp(v41 - 4, 0, v75[24] - 1);
            //v66 = LogicMath.Clamp(v43 - 4, 0, v75[25] - 1);
            //v45 = LogicMath.Clamp(v41 + 4, 0, v75[24] - 1);
            //v74 = LogicMath.Clamp(v43 + 4, 0, v75[25] - 1);
            //result = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(v72);
            //v65 = v45;
            //if (v44 <= v45)
            //{
            //    do
            //    {
            //        if (v66 <= v74)
            //        {
            //            v47 = 300 * v66 + 150;
            //            v48 = v66;
            //            do
            //            {
            //                v49 = v48;
            //                v50 = ZNK12LogicTileMap7getTileEii(v75, v44, v48);
            //                if (v50)
            //                {
            //                    if (sub_7BA338(v50))
            //                    {
            //                        v51 = 300 * v44 + 150 - ZNK21LogicGameObjectServer4getXEv(v72);
            //                        v52 = v47 - ZNK21LogicGameObjectServer4getYEv(v72);
            //                        if (v51 * v51 + v52 * v52 <= (v71 * v71))
            //                        {
            //                            v53 = LogicMath.GetAngle(__SPAIR64__(v52, v51));
            //                            v54 = LogicMath.Sqrt(v51 * v51 + v52 * v52);
            //                            v55 = LogicMath.GetAngleBetween(v69, v53);
            //                            v56 = sub_F0D24(Spread);
            //                            v57 = COERCE_FLOAT(sub_36D218(v56));
            //                            if (v54 > 300)
            //                                v58 = 10;
            //                            if (v54 <= 300 || v55 <= v58 + v57 / 2)
            //                            {
            //                                if (v54)
            //                                {
            //                                    v59 = LogicMath.Max(1, v54 - 300);
            //                                    v51 = a1_divide_a2(v59 * v51, v54);
            //                                    v60 = LogicMath.Max(1, v54 - 300);
            //                                    v52 = a1_divide_a2(v60 * v52, v54);
            //                                }
            //                                v68 = ZNK21LogicGameObjectServer4getXEv(v72);
            //                                v67 = ZNK21LogicGameObjectServer4getYEv(v72);
            //                                v61 = ZNK21LogicGameObjectServer4getXEv(v72);
            //                                v62 = ZNK21LogicGameObjectServer4getYEv(v72);
            //                                if (sub_67008(v75, v68, v67, v61 + v51, v62 + v52))
            //                                    ZN12LogicTileMap11destroyTileEiibP21LogicBattleModeServer(v75, v44, v49, 1);
            //                            }
            //                        }
            //                    }
            //                }
            //                v47 += 300;
            //                v48 = v49 + 1;
            //            }
            //            while (v49 < v74);
            //        }
            //        result = v44 + 1;
            //    }
            //    while (v44++ < v65);
            //}
            //return result;
        }
        public void HandleHealFromDamage(int Damage, bool IsUlti, Character Target)
        {
            if (m_hitpoints < 1) return;
            if (Target == null) return;
            if (Target.CharacterData.IsHero())
            {
                if (Target.GetCardValueForPassive("ulti_heals_self", 1) >= 1 && IsUlti)
                {
                    if (GetIndex() / 16 != -1) Target.Heal(Target.GetIndex(), Target.GetCardValueForPassive("ulti_heals_self", 1), true, Target.CharacterData);
                }
            }
        }
        public int GetCurrentAttackSpeedTicks()
        {
            if (HasActiveSkill("Attack"))
            {
                if (RapidFireMsBetweenAttacks >= 1) return RapidFireMsBetweenAttacks / 50;
            }
            if (CastingTime >= 1)
            {
                foreach (Skill skill in m_skills)
                {
                    if (skill.SkillData.CastingTime >= 1) return skill.SkillData.CastingTime / 50;
                }
            }
            if (!CharacterData.IsBoss())
            {
                if (GetWeaponSkill() != null) return GetWeaponSkill().SkillData.MsBetweenAttacks / 50;
            }
            return CharacterData.AutoAttackSpeedMs / 50;
        }
        public void Cripple(int a2, int ticks = 80)
        {
            IsCrippled = true;
            CripplePercent = a2;
            CrippleEndTick = TicksGone + ticks;
        }
        public void SetCharacterSummoningVariables(
        CharacterData SummonedCharacter,
        int NumSpawns,
        int MaxSpawns,
        int Damage,
        int Health)
        {
            this.SummonedCharacter = SummonedCharacter;
            this.NumSpawns = NumSpawns;
            this.MaxSpawns = MaxSpawns;
            SpawnDamage = Damage;
            SpawnHealth = Health;
        }
        public void SetItemSummoningVariables(ItemData a2, int NumSpawns, int MaxSpawns, int Damage, int NormalDMG)
        {
            SpawnedItem = a2;
            NumSpawnsItem = NumSpawns;
            MaxSpawnsItem = MaxSpawns;
            SpawnedItemDamage = Damage;
            SpawnedItemNormalDMG = NormalDMG;
        }
        public void Attack(Character AutoAttackTarget, int x, int y, int range, ProjectileData projectileData, int damage, int normalDMG, int spread, int bulletsPerShot, int a11 = 0)
        {
            int v15; // r0
            int v17; // r5
            int v18; // r0
            int v20; // r0
            bool v21; // cc
            int v22; // r0
            bool v24; // zf
            int v25; // r0
            int v26; // r5
            int v27; // r0
            int v28; // r0
            int v29; // r5
            int v30; // r0
            bool v31; // zf
            int v32; // r0
            int v33; // r0
            int v34; // r0
            int v35; // r1
            int v36; // r0
            int v38; // r0
                     //int RapidFireRange; // r1
            int v40; // r5
            int v41; // r0
            int IsMinion; // r6
            int v43; // r5
            int v44; // r0
            int v45; // r8
            int v46; // r0
            int v48; // r0
            int v49; // r2
            int v51; // r0
            int v54; // r1
            int v55; // r0
            int v56; // r0
            int v57; // r0
            int v58; // r6
            bool v59; // zf
            int v60; // r9
            int v61; // r1
            int v62; // r10
            int v63; // r6
            int SecondHand; // r9
            int v65; // r7
            int v66; // r0
            int v67; // r0
                     //int AttackPattern; // r2 MAPDST
                     //int ShootIndex; // r5
            int v70; // r10
            int v71; // r6
            int v72; // r6
            int v73; // r8
            int v74; // r9
            int HP; // r8
            int v76; // r6
            int v77; // r0
            int v78; // r0
            int v79; // r5
            int v80; // r1
            int v81; // r0
            int v82; // r5
            int v83; // r5
            int v84; // r5
            int v85; // r0
            int v86; // r7
            int v87; // r8
            int v88; // r8
            int v89; // r1
            int active; // r0
            int v91; // r1
            int v92; // r0
            int v93; // r0
            int v94; // r1
            int v95; // r10
            int v96; // r1
            int v97; // r1
            int v98; // r6
            int v99; // r9
            int v100; // r0
            int v101; // r0
            int v102; // r10
            int v103; // r8
            int v104; // r0
            int v105; // r7
            int v106; // r7
            int v107; // r5
                      //int v108; // r7
            int v109; // r10
            int v111; // kr00_4
            int v112; // s16
            int v113; // r6
            int v114; // r9
            int v115; // r9
            int v116; // r0
            int v117; // r6
            int v118; // r7
            int v119; // r9
            int v120; // r0
            int v121; // r1
            int v122; // r8
            int v123; // r6
            int v124; // r0
            int v125; // r0
            int v126; // r6
            bool v128; // zf
            int v129; // r9
            int v130; // r7
            int v131; // r6
            int v132; // r8
            bool v134; // zf
            bool v135; // zf
            int v136; // r0
            int v137; // r0
            int v138; // r0
            int v139; // r0
            int v140; // r5
            int v141; // r6
            int v142; // r2
            int v143; // r0
            int v144; // r1
            int v145; // r0
            int v146; // r5
            int v147; // r6
            int v148; // r0
            int v149; // r5
            int v150; // r6
            int v151; // r0
            bool v153; // zf
            bool v154; // zf
            int v155; // r6
            int i; // r7
            int v157; // r5
            int v158; // r0
            int v159; // r0
            int v160; // r1
            int v161; // r0
            int v162; // r1
            int v163; // r0
            int v164; // r0
            bool v165; // zf
            int v166; // r8
            int v167; // r7
            int v168; // r6
            int v169; // r0
            int v170; // r0
            int v171; // r0
            int v178; // r0
            int v179; // r0
            int v180; // r0
            int v181; // r5
            int v182; // r10
            int j; // r5
            int v184; // r0
            int v185; // r0
            int v186; // r5
            int v188; // r0
            int v189; // r1
            int v190; // r2
            int v191; // r3
            int v193; // r0
            int v194; // r0
            int v196; // r0
            int v197; // r9
            int v198; // r7
            int v199; // r5
            int v201; // r5
            int v202; // r7
            int v203; // r9
            int v204; // r6
            int v205; // r9
            int v206; // r7
            int v207; // r5
            int v208; // r2
            int v209; // r0
            int v210; // r0
            int v211; // r5
            int BuffsCount; // r1
            int v216; // r5
            int v217; // r6
            int v218; // r5
            int v219; // r7
            int v220; // r0
            int v221; // r3
            int v222; // r0
            int v223; // r0
            int v224; // r0
            int v225; // r0
            int v226; // r8
            int v227; // r10
            int v228; // r6
            bool v232; // r1
            int v233; // r1
            bool v234; // zf
            int v235; // r5
            int v236; // r0
            int v237; // r1
            int v238; // r0
            int v239; // r5
            int v240; // r1
            int v241; // r0
            int v242; // r0
            int v243; // r1
            int v244; // r0
            int v245; // r0
            int v246; // r0
            bool v247; // zf
            int v248; // r0
            int v249; // r0
            int v250; // r0
            int v251; // r5
            int TeamIndex; // r6
            int v253; // r0
            int v254; // r0
            int v255; // r5
            bool v260; // zf
            int v261; // r1
            int v262; // r0
            int v263; // r0
            int v264; // r0
            int v265; // r5
            int v266; // r6
            int v267; // r0
            int v268; // [sp+0h] [bp-160h]
            int v269; // [sp+8h] [bp-158h]
            int v273;
            int v274;
            int v276; // [sp+40h] [bp-120h]
            int v277; // [sp+44h] [bp-11Ch]
            int v278; // [sp+4Ch] [bp-114h]
            int v279; // [sp+50h] [bp-110h]
            int v280; // [sp+54h] [bp-10Ch]
            int v281; // [sp+58h] [bp-108h]
            int v282; // [sp+5Ch] [bp-104h]
            int v284; // [sp+64h] [bp-FCh]
            int v285; // [sp+68h] [bp-F8h]
            int v286; // [sp+6Ch] [bp-F4h]
            int v287; // [sp+70h] [bp-F0h]
            int v288; // [sp+74h] [bp-ECh]
            int v289; // [sp+78h] [bp-E8h]
            int v290; // [sp+7Ch] [bp-E4h]
            int v291; // [sp+7Ch] [bp-E4h]
            int v292; // [sp+7Ch] [bp-E4h]
            int v293; // [sp+7Ch] [bp-E4h]
            int v294; // [sp+80h] [bp-E0h]
            int v295; // [sp+80h] [bp-E0h]
            int v296; // [sp+80h] [bp-E0h]
            int v297; // [sp+88h] [bp-D8h]
            int v298; // [sp+88h] [bp-D8h]
            int v299; // [sp+8Ch] [bp-D4h]
            int v300; // [sp+90h] [bp-D0h]
            int v301; // [sp+94h] [bp-CCh]
            int v302; // [sp+94h] [bp-CCh]
            int v303; // [sp+94h] [bp-CCh]
            int v304; // [sp+94h] [bp-CCh]
            int v305; // [sp+98h] [bp-C8h]
            int v307; // [sp+A0h] [bp-C0h]
            int v308; // [sp+A8h] [bp-B8h]
            int v309; // [sp+ACh] [bp-B4h]
            int v310; // [sp+B0h] [bp-B0h]
            int v315; // [sp+F4h] [bp-6Ch]
            List<Projectile> projectiles = new List<Projectile>();
            ProjectileData skinprojectileData = projectileData;
            BattlePlayer player = GetPlayer();
            if (player != null && !IsControlled)
            {
                if (!UltiSkill)
                {
                    int skinid = player.SkinId;
                    if (skinid > 0)
                    {
                        ProjectileData skinpro = DataTables.Get(DataType.Projectile).GetData<ProjectileData>(DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(skinid).Conf).MainAttackProjectile);
                        if (skinpro != null) skinprojectileData = skinpro;

                    }
                }
                else
                {
                    int skinid = player.SkinId;
                    if (skinid > 0)
                    {
                        ProjectileData skinpro = DataTables.Get(DataType.Projectile).GetData<ProjectileData>(DataTables.Get(DataType.SkinConf).GetData<SkinConfData>(DataTables.Get(DataType.Skin).GetDataByGlobalId<SkinData>(skinid).Conf).UltiProjectile);
                        if (skinpro != null) skinprojectileData = skinpro;
                    }
                }
            }

            int newDMG = damage;
            int SpeedMod = 0;
            Accessory accessory = player == null ? null : player.Accessory;
            //v294 = v17;
            //v20 = sub_1C6640(a1);
            //v21 = v20 < 1;
            //if (v20 >= 1)
            //    v21 = a1->dword368 < v20;
            //if (v21 || !*(ZNK21LogicGameObjectServer7getDataEv(a1) + 272))
            //{
            //    if (!a1->IsMinion)
            //        *a1->gap36C = 0;
            //}
            //else if (!a1->IsMinion)
            //{
            //    v28 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //    v29 = (1374389535LL* sub_4F82D0(*(v28 + 272)) *Damage) >> 32;
            //    v30 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
            //    v31 = v30 == 0;
            //    if (v30)
            //    {
            //        v30 = *(v30 + 20);
            //        v31 = v30 == 0;
            //    }
            //    if (v31 || (v196 = sub_62E790(*(v30 + 144)), (ProjectileData = v196) == 0))
            //    {
            //        v32 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //        ProjectileData = sub_4DEBD8(*(v32 + 272));
            //    }
            //    Damage = (v29 >> 5) + (v29 >> 31);
            //    v33 = sub_1C6640(a1);
            //    v34 = LogicMath.Clamp(0, 0, v33);
            //    v35 = *a1->gap36C;
            //    a1->dword368 = v34;
            //    *a1->gap36C = v35 + 1;
            //}
            //v22 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
            //v297 = v22;
            if (!UltiSkill && accessory != null)
            {
                if (accessory.IsActive && accessory.Type == "next_attack_change" && accessory.AccessoryData.SubType == 2)
                {
                    if (accessory.AccessoryData.CustomObject != null)
                    {
                        skinprojectileData = DataTables.GetProjectileByName(accessory.AccessoryData.CustomObject);
                    }
                    newDMG = newDMG * (100 + accessory.AccessoryData.CustomValue1) / 100;
                    if (accessory.AccessoryData.ActiveTicks == 100000) accessory.EndAccessoryActivation();
                }
            }
            //v25 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            v26 = TicksGone;
            //if (v26 < *&a1->gap398[12] && *&a1->gap398[4])
            //{
            //    if (*(ZNK21LogicGameObjectServer7getDataEv(a1) + 272)
            //      && (v27 = ZNK21LogicGameObjectServer7getDataEv(a1), ProjectileData == sub_81920(*(v27 + 272))))
            //    {
            //        ProjectileData = *&a1->gap398[4];
            //    }
            //    else
            //    {
            //        ProjectileData = *&a1->gap398[8];
            //    }
            //}
            v288 = 0;
            //v318[0] = 0;
            //v36 = 0;
            int v37 = LastAttckTick + GetCurrentAttackSpeedTicks();
            m_attackingTicks = LogicMath.Clamp(v37 - v26, 0, 63);
            //RapidFireRange = a1->RapidFireRange;
            //v284 = v26;
            if (projectileData != null && RapidFireRange >= 1)
            {
                //v40 = a1_divide_a2(150 * a1->field_D4, 100 * RapidFireRange);
                //v41 = sub_1ECCEC(ProjectileData) * v40;
                //v26 = v284;
                //v288 = v41 / 100;
            }
            if (v37 <= v26)
            {
                if (!UltiSkill && GetMaxChargedShots() >= 1 && CharacterData.UniqueProperty != 10)
                {
                    if (ChargedShotCount < GetMaxChargedShots()) ChargedShotMisses = 0;
                    else
                    {
                        damage = damage * WeaponSkillData.DamageModifier / 100;
                        string name = GetPlayer()?.SkinConfData?.SecondaryProjectile;
                        if (name != null) skinprojectileData = DataTables.GetProjectileByName(name);
                        else skinprojectileData = DataTables.GetProjectileByName(WeaponSkillData.SecondaryProjectile);
                        ChargedShotCount = 0;
                        ChargedShotMisses++;
                    }
                }
                bool se = false;
                //Debugger.Print("TG:" + v26);
                //IsMinion = a1->IsMinion;
                //v43 = GetCardValueForPassive(a1, 8, 1);
                //v279 = IsMinion;
                //v305 = a3;
                //v289 = a4;
                //v300 = ProjectileData;
                if (GetCardValueForPassive("ambush", 1) >= 1 && !UltiSkill)
                {
                    if (IsInBush())
                    {
                        newDMG += GetCardValueForPassive("ambush", 1);
                        se = true;
                    }
                }
                //v280 = v51;
                //v52 = a9;
                //if (a1->IsMinion)
                if (UltiSkill)//isultiskill
                {
                    newDMG += (GetCardValueForPassive("gain_damage_super", 1) != -1 ? GetCardValueForPassive("gain_damage_super", 1) : 0);
                }
                else
                {
                    newDMG += (GetCardValueForPassive("damage_main_attack", 1) != -1 ? GetCardValueForPassive("damage_main_attack", 1) : 0);

                    //v53 = a1;
                    //v54 = 70;
                }
                if (HasUlti())
                {
                    //v55 = GetCardValueForPassive(a1, 55, 1);
                    //if (v55 > 0)
                    //    Damage += v55;
                }
                //v56 = GetCardValueForPassive(v53, v54, 1);
                //if (v56 > 0)
                //    Damage += v56;
                //if (!a9)
                //    goto LABEL_61;
                //if (ProjectileData)
                //{
                //    v57 = ZNK19LogicProjectileData17getHealOwnPercentEv(ProjectileData);
                //    if (!Damage || v57)
                //        goto LABEL_61;
                //}
                //else if (!Damage)
                //{
                //    Damage = 0;
                //    goto LABEL_61;
                //}
                newDMG -= CripplePercent * newDMG / 100;
                if (newDMG <= 1)
                    newDMG = 1;
                //LABEL_61:
                //v58 = 0;
                //v59 = v297 == 0;
                //if (v297)
                //{
                //    v52 = *(v297 + 204);
                //    v59 = v52 == 0;
                //}
                bool DisablePercentageDamage = false;
                int StaticDMG = 0;
                if (accessory != null)
                {
                    if (accessory.IsActive)
                    {
                        if (!UltiSkill && accessory.Type == "next_attack_change")
                        {
                            if (accessory.AccessoryData.SubType == 4)
                            {
                                StaticDMG = accessory.AccessoryData.CustomValue1;
                                accessory.EndAccessoryActivation();
                            }
                        }
                    }
                }
                LastAttckTick = TicksGone;
                //a1->field_6C = v284 + 2;
                if (projectileData == null)
                {
                    if (a11 == 3)
                        RapidFireMeleeAttack(x, y, newDMG, normalDMG, spread);
                    else
                        MeleeAttack(AutoAttackTarget, 0, 0, newDMG, normalDMG);
                    goto LABEL_229;
                }
                //AttackOffsetX = -1;
                //v276 = a6;
                //v61 = a1->gap124[0];
                //AttackOffset2Y = 0;
                //AttackOffsetY = -1;
                //AttackOffset2X = 0;
                //v299 = Damage;
                if (true)
                {
                    //v53 = 0;
                    //v54 = 0;
                    v55 = LeftHand;
                    v58 = CharacterData.TwoWeaponAttackEffectOffset;
                    //v58 = 200;
                    //v51 = (a1 + 60);
                    //v59 = v54;
                    v246 = v55;
                    //v45 = v263;
                    //v43 = v280;
                    GetShootPositionModifiers(
                      GetX(),
                      GetY(),
                      x,
                      y,
                      v246,
                      50,
                      v58,
                      0,
                      0);
                    if (LeftHand == 0) LeftHand = 1;
                    else LeftHand = 0;
                }
                v277 = spread;
                //v278 = a1->field_B4;
                if (RapidFireAttackPattern <= 6 && ((1 << RapidFireAttackPattern) & 78) != 0)
                {
                    v70 = x - GetX();
                    v71 = y - GetY();
                    v72 = LogicMath.Sqrt(v70 * v70 + v71 * v71);
                    v73 = (v72 * spread) / 450;
                    if (ShootIndex % 3 == 2)
                    {
                        v73 = (v72 * spread) / -450;
                        v74 = -spread;
                    }
                    else
                    {
                        v74 = 0;
                        if (ShootIndex % 3 == 1)
                        {
                            v73 = (v72 * spread) / -450;
                            v74 = spread;
                        }
                    }
                    //if (*(v300 + 88) || (v24 = sub_51F080(v300) == 6, v78 = v289, v79 = v305, v24))
                    //{
                    //    if (v72)
                    //    {
                    //        v70 = a1_divide_a2((v73 + v72) * v70, v72);
                    //        v71 = a1_divide_a2((v73 + v72) * v71, v72);
                    //    }
                    //    v79 = ZNK21LogicGameObjectServer4getXEv(a1) + v70;
                    //    v78 = ZNK21LogicGameObjectServer4getYEv(a1) + v71;
                    //}
                    v79 = x;
                    v78 = y;
                    v80 = RapidFireShootTimes;
                    v298 = v78;
                    if (v80 >= 5)
                    {
                        v81 = 1080 * ShootIndex / (v80 - 1);
                        v74 = (LogicMath.Sin(v81) * spread) >> 10;
                    }
                    v302 = v79;
                    switch (RapidFireAttackPattern)
                    {
                        case 6:
                            v158 = ShootIndex;
                            if (v158 != 0)
                            {
                                v159 = (v158 - ((v158 & 1) != 0 ? 0 : 1)) / 2 + 1;
                                v160 = -1;
                                if ((ShootIndex & 1) == 0)
                                    v160 = 1;
                                v74 = v159 * v160 * spread / RapidFireShootTimes * 2;
                            }
                            else
                            {
                                v74 = 0;
                            }
                            break;
                        case 3:
                            v161 = (720 * ShootIndex) / (RapidFireShootTimes - 1);
                            v74 = (LogicMath.Sin(v161) * spread) >> 10;
                            break;
                        case 2:
                            v82 = ShootIndex;
                            v74 = v82 * (2 * spread) / (RapidFireShootTimes - 1) - spread;
                            break;
                    }
                    //Projectile projectile = new Projectile(skinprojectileData);
                    //projectile.ShootProjectile(LogicMath.GetAngle(x - GetX(), y - GetY()) + v74, this, newDMG, 0, false, RapidFireRange);
                    //GameObjectManager.AddGameObject(projectile);
                    //Debugger.Print("PRO:" + TicksGone);
                    //PosX = AttackOffsetX;
                    //PosY = AttackOffsetY;
                    //AttackX = AttackOffset2X;
                    //AttackY = AttackOffset2Y;
                    //v176 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                    Projectile v177 = Projectile.ShootProjectile(
                             AttackOffsetX,
                             AttackOffsetY,
                             this,
                             this,
                             skinprojectileData,
                             AttackOffset2X + v302,
                             AttackOffset2Y + v298,
                             newDMG,
                             RapidFireNormalDMG,
                             v74,
                             true,
                             -v288,
                             GameObjectManager.GetBattle(),
                             ShootIndex, UltiSkill ? 2 : 1);
                    //v318[0] = v177;
                    v178 = GetCardValueForPassive("heal_others_main_attack", 1);
                    if (v178 >= 0)
                        v177.HealOthersAreaEffect = v178;
                    v179 = GetCardValueForPassive("bounced_bullets_stronger", 1);
                    //v60 = v289;
                    if (v179 >= 0)
                        v177.DamageAddFromBounce = v179;
                    projectiles.Add(v177);
                    if (Accessory != null && Accessory.IsActive && Accessory.Type == "next_attack_change")
                    {
                        switch (Accessory.AccessoryData.SubType)
                        {
                            case 9:
                                v177.AttackSpecialParams_BounceHeal = Accessory.AccessoryData.CustomValue1;
                                break;
                            case 10:
                                v177.AttackSpecialParams_StealBulletPercent = Accessory.AccessoryData.CustomValue1;
                                v177.AttackSpecialParams_StealBulletPercentSelf = Accessory.AccessoryData.CustomValue2;
                                v177.SpecialEffect = true;
                                break;
                        }
                    }
                    //v180 = v177;
                    v177.RunEarlyTicks();
                    goto LABEL_218;
                }
                else if (RapidFireBulletsPerShoot >= 2)
                {
                    //v83 = (2454267027LL* sub_1ECCEC(v300)) >> 32;
                    v273 = RapidFireBulletsPerShoot - 1;
                    v295 = 2 * spread / (RapidFireBulletsPerShoot - 1);
                    //if (sub_228E44(v300))
                    //    a1->field_3D8 = 0;
                    //v285 = (v83 >> 2) + (v83 >> 31);
                    v285 = projectileData.Speed / 7;
                    v84 = RapidFireBulletsPerShoot;
                    v291 = -spread;
                    v85 = 3;
                    if (RapidFireBulletsPerShoot == 5)
                        v85 = 2;
                    v274 = v85;
                    //v275 = &a1->field_3D0;
                    v303 = 0;
                    for (int b = 0; b != RapidFireBulletsPerShoot; b++)
                    {
                        v88 = 0;
                        if (v84 >= 4)
                        {
                            v89 = (b + ShootIndex) % v274;
                            if (v89 == 2)
                            {
                                v88 = v285;
                            }
                            else
                            {
                                v88 = 0;
                                if (v89 == 1)
                                    v88 = v285 / 2;
                            }
                        }
                        if (RapidFireAttackPattern == 5)
                        {
                            v286 = 0;
                            v287 = 0;
                            if ((b & 1) == 0)
                                goto LABEL_117;
                            int vx = x - GetX();
                            int vy = y - GetY();
                            int v110 = LogicMath.Sqrt(vx * vx + vy * vy);
                            if (v110 >= 1)
                            {
                                int tmpp = projectileData.Speed;
                                v111 = tmpp * (b & 1);
                                v112 = v110 / tmpp;
                                v88 = v111 / 4;
                                v113 = (v112 * (tmpp - v111 / 4)) - v110;
                                v286 = v113 * vx / v110 / 2;
                                v287 = v113 * vy / v110 / 2;
                            }
                        }
                        else
                        {
                            v286 = 0;
                            v287 = 0;
                        }
                        if (RapidFireAttackPattern == 4)
                            v88 = v285 * b;
                        if (RapidFireAttackPattern == 13 || RapidFireAttackPattern == 10)
                        {
                            v286 = 0;
                            v287 = 0;
                            v88 = 0;
                        }
                    LABEL_117:
                        //v286 = 0;
                        //active = ZNK20LogicCharacterServer14hasActiveSkillEi(a1, 7);
                        v91 = newDMG;
                        //if (active)
                        //{
                        //    v92 = GetCardValueForPassive(a1, 45, 1);
                        //    v91 = v299;
                        //    if (v92 > 0)
                        //        v91 = v299 + v92;
                        //}
                        v282 = v91;
                        v94 = v291;
                        v95 = 0;
                        if (projectileData.TravelType == 3)
                            v94 = 0;
                        v291 = v94;
                        v96 = v295;
                        if (projectileData.TravelType == 3)
                            v96 = 0;
                        v295 = v96;
                        v97 = v285;
                        if (projectileData.TravelType == 3)
                            v97 = 0;
                        v285 = v97;
                        //if (true)
                        if (RapidFireAttackPattern != 7)
                        {
                            v106 = 0;
                            v107 = v88;
                        }
                        else
                        {
                            v98 = x - GetX();
                            v99 = y - GetY();
                            v100 = LogicMath.Sqrt(v98 * v98 + v99 * v99);
                            if (v100 < 1)
                            {
                                v106 = 0;
                                v107 = v88;
                                v95 = 0;
                            }
                            else
                            {
                                v101 = v100;
                                v102 = LogicMath.GetRotatedX(v98, v99, 90);
                                v103 = LogicMath.GetRotatedY(v98, v99, 90);
                                if (RapidFireAttackPattern == 7)
                                {
                                    v104 = 2 * projectileData.Radius;
                                    v105 = RapidFireBulletsPerShoot - 1;
                                }
                                else
                                {
                                    v114 = LogicMath.Sin(spread / 2);
                                    v115 = LogicMath.Cos(spread / 2);
                                    v116 = (v101 << 10) / v115;
                                    v105 = RapidFireBulletsPerShoot - 1;
                                    v104 = 2 * ((v116 * v114) >> 10) / v273;
                                }
                                v117 = v104 * b;
                                v118 = v104 * v105 / 2;
                                v119 = v104 * b * v103 / v101;
                                v120 = v118 * v103 / v101;
                                v121 = v117 * v102;
                                v122 = v102;
                                v95 = v119 - v120;
                                v296 = GetX();
                                v123 = v121 / v101;
                                v106 = v123 - v118 * v122 / v101;
                                //if (v296 <= -1)
                                //{
                                //    AttackOffsetX = GetX();
                                //    AttackOffsetY = GetY();
                                //}
                                v107 = 0;
                                v124 = v286;
                                if (RapidFireAttackPattern == 7)
                                    v124 = v106;
                                v286 = v124;
                                v125 = v287;
                                if (RapidFireAttackPattern == 7)
                                    v125 = v95;
                                v287 = v125;
                                //v295 = 0;
                                //v291 = 0;
                            }
                        }
                        v281 = v106;
                        if (!UltiSkill && accessory != null)
                        {
                            if (accessory.IsActive && accessory.Type == "next_attack_change" && accessory.AccessoryData.SubType == 1)
                            {
                                v295 = 2 * accessory.AccessoryData.CustomValue1 / v273 / 5;
                                v291 = -accessory.AccessoryData.CustomValue1 / 5;
                                v288 = accessory.AccessoryData.CustomValue3;
                                if (accessory.AccessoryData.ActiveTicks == 100000) accessory.EndAccessoryActivation();
                            }
                        }

                        //v129 = AttackOffsetX;
                        //v130 = AttackOffsetY;
                        //v131 = AttackOffset2X;
                        //v132 = AttackOffset2Y;
                        //Projectile projectile = new Projectile(skinprojectileData);
                        //projectile.ShootProjectile(AttackAngle + v291 + v295 * b, this, newDMG, 0, false, RapidFireRange, SpeedMod);
                        //GameObjectManager.AddGameObject(projectile);
                        //projectiles.Add(projectile);
                        //Debugger.Print((v291 + v295 * b).ToString());
                        //v270 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                        //v281 = 0;
                        //v95 = 0;
                        Projectile projectile = Projectile.ShootProjectile(
                                    AttackOffsetX + v281,
                                    AttackOffsetY + v95,
                                    this,
                                    this,
                                    skinprojectileData,
                                    v286 + x + AttackOffset2X,
                                    v287 + y + AttackOffset2Y,
                                    v282,
                                    RapidFireNormalDMG,
                                    v291 + v295 * b,
                                    true,
                                    v107 - v288,
                                    GameObjectManager.GetBattle(),
                                    b, UltiSkill ? 2 : 1);
                        projectiles.Add(projectile);
                        //if (sub_228E44(v300))
                        //    sub_7218E4(v275, v318);
                        if (!UltiSkill && Accessory != null)
                        {
                            if (Accessory.Type == "next_attack_change" && Accessory.IsActive)
                            {
                                switch (Accessory.AccessoryData.SubType)
                                {
                                    case 3:
                                        projectile.AttackSpecialParams_Stun = Accessory.AccessoryData.CustomValue1;
                                        break;
                                    case 11:
                                        projectile.AttackSpecialParams_WeaklyStun = Accessory.AccessoryData.CustomValue1;
                                        break;
                                    case 7:
                                        projectile.Damage += projectile.Damage * Accessory.AccessoryData.CustomValue2 / 100;
                                        projectile.AttackSpecialParams_Pushback = -Accessory.AccessoryData.CustomValue1;
                                        break;
                                }
                            }
                        }
                        if (projectileData.HealOwnPercent > 0)
                        {
                            v136 = GetCardValueForPassive("damage_super", 1);
                            if (v136 >= 0)
                                projectile.Healing = -v136;
                        }
                        else
                        {
                            v137 = GetCardValueForPassive("heal_main_attack", 1);
                            if (v137 >= 0)
                            {
                                projectile.Healing = v137;
                                projectile.SpecialEffect = true;
                            }
                        }
                        v138 = GetCardValueForPassive("cripple", 1);
                        if (v138 >= 0)
                            projectile.CripplePercent = v138;
                        if (UltiSkill)
                        {
                            v139 = GetCardValueForPassive("freeze", 1);
                            if (v139 >= 0)
                            {
                                v142 = GetCardValueForPassive("freeze", 0);
                                projectile.SetFreeze(v139, v142);
                            }
                        }
                        else
                        {
                            v145 = GetCardValueForPassive("basic_freeze", 0);
                            if (v145 >= 0)
                            {
                                v144 = GetCardValueForPassive("basic_freeze", 1);
                                projectile.SetFreeze(v144, v145);
                            }
                        }
                        v148 = GetCardValueForPassive("prey_on_the_weak", 1);
                        if (v148 >= 0)
                        {
                            v151 = GetCardValueForPassive("prey_on_the_weak", 0);
                            projectile.SetPreyOnTheWeak(v148, v151);
                        }
                        //if ((v280 & 1) != 0)
                        //    *(v318[0] + 284) = 1;
                        //v84 = RapidFireBulletsPerShoot;
                        if (!projectile.ProjectileData.DamageOnlyWithOneProj)
                            projectile.RunEarlyTicks();
                    }
                    if (!UltiSkill && Accessory != null && Accessory.IsActive && Accessory.Type == "next_attack_change" && Accessory.AccessoryData.ActiveTicks != 100000)
                    {
                        Accessory.EndAccessoryActivation();
                    }
                    if (projectiles.Count > 1 && projectileData.DamageOnlyWithOneProj)
                    {
                        foreach (Projectile projectile in projectiles)
                        {
                            foreach (Projectile projectile1 in projectiles)
                            {
                                if (projectile == projectile1) continue;
                                projectile.LinkedProjectileGIDs.Add(projectile1.GetGlobalID());
                            }

                            projectile.RunEarlyTicks();
                            //Debugger.Print(String.Join(",", projectiles[2].IgnoredTargets));
                        }
                    }
                    goto LABEL_218;
                    //v287 = 0;
                    //if (AttackPattern == 4)
                    //    v88 = v285 * v303;
                    //LABEL_117:
                    //v286 = 0;
                    //goto LABEL_118;
                }
                else
                {
                    if (!projectileData.Indirect)
                    {
                        v293 = v58;
                        if (projectileData.IsFriendlyHomingMissile)
                        {
                            //if (!v301)
                            //    goto LABEL_218;
                            //sub_4C793C(v311);
                            //v197 = sub_34860C(v311, 0);
                            //v198 = ZNK21LogicGameObjectServer4getXEv(v301);
                            //v199 = ZNK21LogicGameObjectServer4getYEv(v301);
                            //v200 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                            //v318[0] = ZN21LogicProjectileServer15shootProjectileEiiP20LogicCharacterServerPK21LogicGameObjectServerPK19LogicProjectileDataiiiiibiP21LogicBattleModeServerii(
                            //            -1,
                            //            -1,
                            //            a1,
                            //            a1,
                            //            v197,
                            //            v198,
                            //            v199,
                            //            Damage,
                            //            0,
                            //            0,
                            //            0,
                            //            0,
                            //            v200,
                            //            0);
                            //v60 = v289;
                            //sub_54FE64(v311);
                            //v180 = v318[0];
                            //*(v318[0] + 112) = v301;
                            //*(v180 + 44) = *(v301 + 44);
                            //*(v180 + 40) = *(v301 + 40);
                        }
                        else
                        {
                            //v304 = AttackOffsetX;
                            //v204 = v60;
                            //v205 = AttackOffsetY;
                            //v206 = AttackOffset2X;
                            //v207 = AttackOffset2Y;
                            //v272 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                            //v208 = a11 - 2;
                            //if (a11 != 2)
                            //    v208 = a8;
                            //Projectile projectile = new Projectile(skinprojectileData);
                            //if (!DisablePercentageDamage) projectile.PercentDamage = m_skills.Count > 1 ? m_skills[0].SkillData.PercentDamage : 0;
                            //if (StaticDMG >= 1) projectile.StaticDamage = StaticDMG;
                            //projectile.ShootProjectile(LogicMath.GetAngle(x - GetX(), y - GetY()), this, newDMG, 0, false, RapidFireRange);
                            //GameObjectManager.AddGameObject(projectile);
                            Projectile projectile = Projectile.ShootProjectile(
                                        AttackOffsetX,
                                        AttackOffsetY,
                                        this,
                                        this,
                                        skinprojectileData,
                                        AttackOffset2X + x,
                                        AttackOffset2Y + y,
                                        newDMG + GetGearBoost(16) * 2,
                                        RapidFireNormalDMG,
                                        spread,
                                        false,
                                        -v288,
                                        GameObjectManager.GetBattle(),
                                        0, UltiSkill ? 2 : 1);
                            projectiles.Add(projectile);

                            if (m_skills.Count > 0 && m_skills[0].SkillData.PercentDamage >= 1)
                            {
                                projectile.PercentDamage = m_skills[0].SkillData.PercentDamage;
                                projectile.AttackSpecialParams_MinPercentDamage = 750 + GetGearBoost(16);
                                projectile.AttackSpecialParams_DamageBuff = GetDamageBuffTemporary();
                                projectile.AttackSpecialParams_DamageDebuff = CripplePercent;
                                int dis = 600;
                                Character Guess = null;
                                foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                                {
                                    if (gameObject.GetObjectType() != 0) continue;
                                    if (gameObject.GetIndex() / 16 == GetIndex() / 16) continue;
                                    if (gameObject.GetPosition().GetDistance(new LogicVector2(x, y)) > dis) continue;
                                    dis = gameObject.GetPosition().GetDistance(new LogicVector2(x, y));
                                    Guess = (Character)gameObject;
                                }
                                if (Guess != null)
                                {
                                    projectile.DisplayScale = LogicMath.Clamp(Guess.m_hitpoints / 10 - 100, 100, 1000);
                                }
                                else projectile.DisplayScale = 250;
                            }
                            if (StaticDMG >= 1)
                            {
                                projectile.AttackSpecialParams_StaticPercentDamage = StaticDMG;
                            }
                            //v209 = v279;
                            //if (v279)
                            //    v209 = 1;
                            //if (!(v293 | v209))
                            //{
                            //    v210 = ZNK21LogicGameObjectServer7getDataEv(a1);
                            //    v211 = *(v210 + 272);
                            //    if (v211)
                            //    {
                            //        if (ZNK14LogicSkillData16getPercentDamageEv(*(v210 + 272)) >= 1)
                            //        {
                            //            sub_B4F64(v306);
                            //            v307 = ZNK14LogicSkillData16getPercentDamageEv(v211);
                            //            BuffsCount = a1->BuffsCount;
                            //            if (BuffsCount < 1)
                            //            {
                            //                v214 = 0;
                            //                v216 = v284;
                            //            }
                            //            else
                            //            {
                            //                Buffs = a1->Buffs;
                            //                v214 = 0;
                            //                do
                            //                {
                            //                    v215 = **Buffs;
                            //                    if (v215 <= 5 && ((1 << v215) & 38) != 0)
                            //                        v214 += (*Buffs)[3];
                            //                    v216 = v284;
                            //                    ++Buffs;
                            //                    --BuffsCount;
                            //                }
                            //                while (BuffsCount);
                            //            }
                            //            v309 = v214 + *&a1->gap124[4];
                            //            v259 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
                            //            v260 = v259 == 0;
                            //            v261 = 200;
                            //            if (v259)
                            //            {
                            //                v259 = v259[31];
                            //                v260 = v259 == 0;
                            //            }
                            //            if (!v260)
                            //                v261 = 10 * *v259 + 190;
                            //            v310 = v261;
                            //            if (v216 < *&a1->gap1EE[14])
                            //                v308 = *&a1->gap1EE[10];
                            //            sub_661928(v318[0] + 340, v306);
                            //            sub_111BC8(v306);
                            //        }
                            //    }
                            //}
                            //v262 = GetCardValueForPassive(a1, 6, 1);
                            //if (v262 >= 0)
                            //    *(v318[0] + 136) = v262;
                            //v263 = GetCardValueForPassive(a1, 4, 1);
                            //v60 = v289;
                            //if (v263 >= 0)
                            //    *(v318[0] + 140) = v263;
                            //if (a1->IsMinion)
                            //{
                            //    v264 = GetCardValueForPassive(a1, 23, 1);
                            //    if (v264 >= 0)
                            //    {
                            //        v265 = v264;
                            //        v266 = v318[0];
                            //        v267 = GetCardValueForPassive(a1, 23, 0);
                            //        sub_6AFA8(v266, v265, v267);
                            //    }
                            //}
                            //v180 = v318[0];
                            //if (v280)
                            //    *(v318[0] + 284) = 1;
                            if (GetCardValueForPassive("attack_pierce", 1) >= 0 && ChargeUp >= ChargeUpMax)
                            {
                                projectile.CanPierceCharacter = true;
                                ChargeUp = 0;
                            }
                            if (Accessory != null && Accessory.IsActive && Accessory.Type == "next_attack_change")
                            {
                                switch (Accessory.AccessoryData.SubType)
                                {
                                    case 5:
                                        SetDraggingObject(projectile, 0, 0);
                                        break;
                                    case 13:
                                        projectile.AttackSpecialParams_TrailArea = Accessory.AccessoryData.CustomObject;
                                        projectile.AttackSpecialParams_TrailAreaInterval = Accessory.AccessoryData.CustomValue1;
                                        break;
                                }
                            }
                            if (SpawnedItem != null)
                            {
                                projectile.SetItemSummonProjectileData(SpawnedItem, NumSpawnsItem, MaxSpawnsItem, SpawnedItemDamage, SpawnedItemNormalDMG);
                            }
                            projectile.SpecialEffect = se;
                            projectile.RunEarlyTicks();
                            goto LABEL_218;
                        }
                    }
                }

                //LABEL_217:
                //sub_6391F8(v180);
                //goto LABEL_218;
                v162 = AttackOffset2Y;
                AttackOffset2X += x;
                AttackOffset2Y = v162 + y;
                //if (!v297)
                //    goto LABEL_207;
                //v163 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //if (ZNK18LogicCharacterData6isHeroEv(v163))
                //    goto LABEL_207;
                //v164 = *(v297 + 204);
                //v165 = v164 == 0;
                //if (v164)
                //    v165 = *(v164 + 36) == 0;
                //if (v165 || *(v164 + 16) != 19)
                if (true)
                {
                    //LABEL_207:
                    if (RapidFireAttackPattern != 9)
                    //if (true)
                    {
                        v201 = AttackOffset2Y;
                        v202 = newDMG;
                        goto LABEL_265;
                    }
                    v167 = 0;
                    v292 = ShootIndex % 5;
                }
                else
                {
                    //v254 = sub_92E60(a1->field_C, *&a1->gapDD[3]);
                    //v166 = v300;
                    //if (v254)
                    //{
                    //    v255 = v254;
                    //    AttackOffset2X = ZNK21LogicGameObjectServer4getXEv(v254);
                    //    AttackOffset2Y = ZNK21LogicGameObjectServer4getYEv(v255);
                    //}
                    //v256 = *(v297 + 204);
                    //v257 = v256[8];
                    //v258 = sub_67DB14(*v256);
                    //v292 = a1_divide_a2(v257, v258);
                    //v276 = 0;
                    //v167 = 1;
                }
                v168 = GetX();
                v169 = GetY();
                v170 = GamePlayUtil.GetDistanceBetween(v168, v169, AttackOffset2X, AttackOffset2Y);
                if (false)
                    ;
                //v171 = sub_1EF8B8(**(v297 + 204));
                else
                    v171 = Projectile.GetSpreadMax(projectileData, spread, v170);
                int v229 = v171;
                v210 = v171;
                //v277 = v171/4;
                int v230, v231;

                switch (v292)
                {
                    case 1:
                        v230 = -v210;
                        v231 = -v210;
                        break;
                    case 2:
                        v231 = -v210;
                        v230 = v210;
                        break;
                    case 3:
                        v230 = -v210;
                        goto LABEL_246;
                    case 4:
                        v230 = v210;
                    LABEL_246:
                        v231 = v210;
                        break;
                    default:
                        v230 = 0;
                        v231 = 0;
                        break;
                }
                //if ((v292 - 1) <= 3)
                //    __asm { ADD PC, R1, R0 }
                //Debugger.Print(v217.ToString());
                LogicRandom v316 = new LogicRandom(ShootIndex + 12345);
                v218 = v230 - v229 / 4 + (v316.Rand(1000) * (v229 / 4)) / 500;
                v219 = v231 - v229 / 4 + (v316.Rand(1000) * (v229 / 4)) / 500;
                //v24 = sub_20A388(a1) == 0;
                v24 = true;
                v220 = 1;
                if (!v24)
                    v220 = -1;
                v221 = AttackOffset2Y;
                AttackOffset2X += v218 * v220;
                AttackOffset2Y += v219 * v220;
                AttackOffset2X = LogicMath.Clamp(AttackOffset2X, 1, MapWidth - 2);
                v201 = LogicMath.Clamp(AttackOffset2Y, 1, MapHeight - 2);
                v202 = newDMG;
                AttackOffset2Y = v201;
            LABEL_265:
                v226 = AttackOffsetX;
                v227 = AttackOffsetY;
                v228 = AttackOffset2X;
                //v230 = a11 - 2;
                //if (a11 != 2)
                //int v230 = 0;
                //int v230 = v277;
                Projectile v2311 = Projectile.ShootProjectile(
                         v226,
                         v227,
                         this,
                         this,
                         skinprojectileData,
                         v228,
                         v201,
                         v202,
                         RapidFireNormalDMG,
                         0,
                         false,
                         -v288,
                         GameObjectManager.GetBattle(),
                         ShootIndex,
                         UltiSkill ? 2 : 1);
                projectiles.Add(v2311);
                //v232 = *&a1->gap2F8[12];
                v232 = false;
                if (v232)
                {
                    //sub_4FB844(v231, v232, *&a1->gap2F8[20], *&a1->gap2F8[24], *&a1->gap2F8[28], *&a1->gap2F8[32]);
                }
                else
                {
                    //v233 = *&a1->gap2F8[16];
                    if (SummonedCharacter == null)
                    {
                        //    v60 = v289;
                        //    if (a1->gap1EE[0])
                        //        sub_24490C(v231, *&a1->gap2F8[20], *&a1->gap2F8[24], *&a1->gap2F8[28], *&a1->gap2F8[36]);
                        goto LABEL_272;
                    }
                    v2311.SetCharacterSummonProjectileData(SummonedCharacter, NumSpawns, MaxSpawns, SpawnDamage, SpawnHealth, StarPowerIC);
                }
            //v60 = v289;
            LABEL_272:
                //v181 = v284;
                if (GetCardValueForPassive("increased_explosion", 1) >= 0
                  && !UltiSkill
                  && ChargeUp >= ChargeUpMax)
                {
                    AreaEffectData areaEffectData = null;
                    if (player?.SkinConfData != null)
                    {
                        areaEffectData = DataTables.GetAreaEffectByName(player.SkinConfData.AreaEffectStarPower);
                    }
                    if (areaEffectData == null)
                    {
                        areaEffectData = DataTables.GetAreaEffectByName("WallyExplosionBig");
                    }
                    ChargeUp = 0;
                    if (areaEffectData != null) projectiles[0].AttackSpecialParams_AreaEffectData = areaEffectData;

                }
                //if (GetCardValueForPassive(a1, 12, 1) >= 0)
                //{
                //    v235 = v318[0];
                //    v236 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //    v237 = sub_48D9C0(v236);
                //    v238 = v235;
                //    v181 = v284;
                //    sub_23E4BC(v238, v237, 0, 0);
                //}
                //if (GetCardValueForPassive(a1, 57, 1) >= 0)
                //{
                //    v239 = v318[0];
                //    sub_4C793C(v312);
                //    v240 = sub_3A8DC0(v312, 0);
                //    v241 = v239;
                //    v181 = v284;
                //    sub_23E4BC(v241, v240, 0, 0);
                //    sub_54FE64(v312);
                //}
                //v242 = GetCardValueForPassive(a1, 15, 1);
                //if (v242 >= 0)
                //    sub_1A8FF4(v318[0], v242);
                v243 = GetCardValueForPassive("aoe_dot", 1);
                //v244 = v318[0];
                if (v243 != -1)
                    projectiles[0].aoe_dot = v243;
                //v245 = ZNK21LogicProjectileServer17getProjectileDataEv(v244);
                //v246 = sub_41CFF4(v245);
                //v247 = v301 == 0;
                //if (v301)
                //    v247 = v246 == 0;
                //if (!v247)
                //    *(v318[0] + 112) = v301;
                //v248 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //if (!ZNK18LogicCharacterData6isHeroEv(v248))
                //{
                //    v249 = sub_2402F4(a1, 42, 1);
                //    if (v249 >= 0)
                //        *(v318[0] + 140) = v249;
                //}
                //sub_6391F8(v318[0]);
                if (!UltiSkill)
                {
                    v250 = GetCardValueForPassive("heal_self_main_attack", 1);
                    if (v250 >= 0)
                    {
                        v251 = v250;
                        Heal(GetIndex(), v251, true, CharacterData);
                    }
                    //LABEL_218:
                    //v181 = v284;
                }
            LABEL_218:
                //v181 = v284;
                ++ShootIndex;
                if (RapidFireAttackPattern == 14)
                {
                    int m = GetCardValueForPassive("attack_sequence_extra_steps", 1);
                    JesterSkillCount++;
                    if (JesterSkillCount >= 3 + (m > 0 ? m : 0)) JesterSkillCount = 0;
                }
            //    if (a11 != 1)
            //    {
            //        a4 = v60;
            //        v182 = v300;
            //        if (a11 == 2 && a10 >= 2)
            //        {
            //            for (j = 1; j != a10; ++j)
            //            {
            //                v271 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //                v184 = v277;
            //                if ((j & 1) != 0)
            //                    v184 = -v277;
            //                v318[0] = ZN21LogicProjectileServer15shootProjectileEiiP20LogicCharacterServerPK21LogicGameObjectServerPK19LogicProjectileDataiiiiibiP21LogicBattleModeServerii(
            //                            -1,
            //                            -1,
            //                            a1,
            //                            a1,
            //                            v300,
            //                            v305,
            //                            v60,
            //                            v299,
            //                            v276,
            //                            v184,
            //                            1,
            //                            -v288,
            //                            v271,
            //                            0);
            //                sub_6391F8(v318[0]);
            //            }
            //        }
            //        goto LABEL_229;
            //    }
            //    *(&a1->field_1BA + 2) = v181 + 3;
            //    *&a1->gap1BE[2] = a10 - 1;
            //    *&a1->gap1BE[6] = v305;
            //    *&a1->gap1BE[10] = v60;
            //LABEL_228:
            //    v182 = v300;
            //    a4 = v60;
            LABEL_229:
                //    v185 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //    *&a1->gap26C[24] = ZNK21LogicBattleModeServer12getTicksGoneEv(v185);
                Attacking = false;
                BlockHealthRegen();
                SetForcedVisible();
                //    if (v182)
                //        *&a1->gap1EE[6] = sub_3D28FC(v182);
                //    sub_697088(a1);
                //    a3 = v305;
                goto LABEL_132;


            }
        LABEL_132:
            if (projectiles.Count >= 1 && IsInRealm)
            {
                foreach (Projectile projectile in projectiles)
                {
                    projectile.IsInRealm = true;
                }
            }
            AttackAngle = LogicMath.GetAngle(x - GetX(), y - GetY());
            bool v108 = false;
            SkillData skill = GetCurrentCastingSkill();
            if (skill != null)
            {
                v108 = !skill.CanMoveAtSameTime;
            }
            else
            {
                //LABEL_141:
                //    v109 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //    v108 = !ZNK18LogicCharacterData6isHeroEv(v109);
            }
            if (v108)
            {
                if (CharacterData.Speed > 0) MoveAngle = AttackAngle;
            }
        }

        public override bool ShouldDestruct()
        {
            return m_hitpoints <= 0 && m_destructafterticks < 1;
        }

        public bool IsChargeActive()
        {
            return IsCharging;
        }

        public int GetHitpointPercentage()
        {
            return (int)(((float)this.m_hitpoints / (float)this.m_maxHitpoints) * 100f);
        }

        public bool HasActiveSkill()
        {
            if (m_skills.Count == 0) return false;
            if (m_skills.Count == 1) return m_skills[0].IsActive;
            else return m_skills[0].IsActive || m_skills[1].IsActive;
        }
        public bool HasActiveSkill(string type)
        {

            if (m_skills.Count < 1) return false;
            foreach (Skill s in m_skills)
            {
                if (s.SkillData.BehaviorType == type && s.TicksActive > 0)
                    return true;
            }
            return false;
        }
        public SkillData GetActiveSkill(string type)
        {

            if (m_skills.Count < 1) return null;
            foreach (Skill s in m_skills)
            {
                if (s.SkillData.BehaviorType == type && s.TicksActive > 0)
                    return s.SkillData;
            }
            return null;
        }
        public SkillData GetCurrentActiveOrCastingSkill()
        {

            if (m_skills.Count < 1) return null;
            foreach (Skill s in m_skills)
            {
                if (s.TicksActive > 0)
                    return s.SkillData;
            }
            return null;
        }
        public Skill GetSkill(string type)
        {

            if (m_skills.Count < 1) return null;
            foreach (Skill s in m_skills)
            {
                if (s.SkillData.BehaviorType == type)
                    return s;
            }
            return null;
        }
        public bool IsSteerMovementActive()
        {
            foreach (Skill skill in m_skills)
            {
                if (skill.TicksActive > 0 && skill.SkillData.ChargeType == 15) return true;
            }
            return false;
        }
        public bool IsImmuneAndBulletsGoThrough(bool IsInRealm = false)
        {
            //bool result = true;
            if (Accessory != null && Accessory.Type == "dive" && Accessory.IsActive) return true;
            if (this.IsInRealm ^ IsInRealm) return true;
            int v2 = m_activeChargeType - 2;
            if ((!IsCharging || v2 >= 16 || ((0x4293 >> v2) & 1) == 0) && !IsSteerMovementActive())
            {
                if (Knockback3) return true;
                return false;
                //v5 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //v6 = ZNK18LogicCharacterData11isCarryableEv(v5);
                //result = 1;
                //if (!v6)
                //{
                //    result = a1->gap1EE[1];
                //    if (a1->gap1EE[1])
                //        return 1;
                //}
            }
            return true;
        }
        public Projectile GetControlledProjectile()
        {
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() == 1 && gameObject.GetIndex() == GetIndex())
                {
                    if (((Projectile)gameObject).ProjectileData.TravelType == 5) return (Projectile)gameObject;
                }
            }
            return null;
        }
        public void TriggerWhirlwind(int Damage, int NormalDMG, int SpeedIncrease)
        {
            ChargeDamage = Damage;
            ChargeNormalDMG = NormalDMG;
            UltiSkill = true;
            ChargeHitGIDs = new List<Ignoring>();
            ApplyBuff(4, 2, SpeedIncrease, 0);
            if (GetCardValueForPassive("ulti_defense", 1) > 0) AddShield(GetCardValueForPassive("ulti_defense", 1), GetCardValueForPassive("ulti_defense", 0));
        }
        public void TickWhirlwind()
        {
            SkillData skillData = GetActiveSkill("Ww");
            if (skillData == null) return;
            ApplyBuff(4, 2, skillData.MsBetweenAttacks, 0);
            int v11 = ChargeDamage;
            v11 += ChargeDamage * GetDamageBuffTemporary() / 100;
            v11 -= ChargeDamage * CripplePercent / 100;
            for (int i = 0; i < ChargeHitGIDs.Count; i++)
            {
                Ignoring ignoring = ChargeHitGIDs[i];
                ignoring.Ticks--;
                if (ignoring.Ticks <= 0)
                {
                    ChargeHitGIDs.Remove(ignoring);
                    i--;
                }
            }

            if (Knockback3) return;
            int v110 = 100 * skillData.CastingRange;
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 0) continue;
                Character character = (Character)gameObject;
                if (character.IsAlive() && character.GetIndex() / 16 != GetIndex() / 16 && !character.IsImmuneAndBulletsGoThrough(IsInRealm))
                {
                    int v32 = character.GetX();
                    int v34 = GetX();
                    int v35 = character.GetY();
                    int v36 = v32 - v34;
                    int v37 = GetY();
                    int v38 = v35 - v37;
                    if (LogicMath.Abs(v36) <= v110 && LogicMath.Abs(v38) <= v110)
                    {
                        int v39 = v36 * v36 + v38 * v38;
                        if (v39 <= v110 * v110)
                        {
                            if (ChargeHitGIDs.Find(x => x.GID == character.GetGlobalID()) != null) continue;
                            ChargeHitGIDs.Add(new Ignoring(character.GetGlobalID(), 5));
                            if (character.CauseDamage(this, v11, ChargeNormalDMG, UltiSkill, null, true))
                            {

                            }
                        }
                    }
                }
            }
            GetBattle().GetTileMap().DestroyEnvironment(GetX(), GetY(), v110, true);

            if (GetCardValueForPassive("super_destroy_walls", 1) <= 0) return;
            int v77 = GetX() / 300;
            int v79 = GetY() / 300;
            TileMap v80 = GetBattle().GetTileMap();
            int v81 = GetCardValueForPassive("super_destroy_walls", 1) - 100;
            int XMin = LogicMath.Clamp(v77 - 2, 0, v80.Width - 1);
            int YMin = LogicMath.Clamp(v79 - 2, 0, v80.Height - 1);
            int XMax = LogicMath.Clamp(v77 + 2, 0, v80.Width - 1);
            int YMax = LogicMath.Clamp(v79 + 2, 0, v80.Height - 1);
            for (int i = XMin; i <= XMax; i++)
            {
                for (int j = YMin; j <= YMax; j++)
                {
                    if (((i * 300 + 150 - GetX()) * (i * 300 + 150 - GetX()) + (j * 300 + 150 - GetY()) * (j * 300 + 150 - GetY())) <= v81 * v81)
                    {
                        Tile tile = v80.GetTile(i, j, true);
                        if (tile != null) tile.Destruct();
                    }
                }
            }
        }
        public void TickCharge()
        {
            int v110 = 0;
            int result = m_activeChargeType;
            if (result == 1 || result == 3 || result == 4 || (result == 5 && ChargeDamage >= 1) || result == 7 || result == 9 || result == 10 || result == 12 || result == 8 || result == 14 || result == 16)
            {
                //v2 = a1;
                //v3 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //v4 = ZNK18LogicCharacterData7isTrainEv(v3);
                //v5 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //v6 = ZNK18LogicCharacterData6isBossEv(v5);
                //ChargeType = a1->ChargeType;
                int v8 = 400;
                if (CharacterData.IsBoss())
                    v8 = 800;
                bool v9 = m_activeChargeType == 9;
                if (m_activeChargeType != 9)
                    v9 = m_activeChargeType == 3;
                v110 = v8;
                if (v9)
                    v110 = 800;
                int v101 = ChargeDamage;
                v101 += ChargeDamage * GetDamageBuffTemporary() / 100;
                v101 -= ChargeDamage * CripplePercent / 100;
                if (m_activeChargeType == 7 || m_activeChargeType == 10 || CharacterData.IsTrain())
                {
                    for (int i = 0; i < ChargeHitGIDs.Count; i++)
                    {
                        Ignoring ignoring = ChargeHitGIDs[i];
                        ignoring.Ticks--;
                        if (ignoring.Ticks <= 0)
                        {
                            ChargeHitGIDs.Remove(ignoring);
                            i--;
                        }
                    }
                }
                //    v108 = v4;
                foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
                {
                    if (gameObject.GetObjectType() != 0) continue;
                    Character character = (Character)gameObject;
                    if (character.IsAlive() && character.GetIndex() / 16 != GetIndex() / 16 && !character.IsImmuneAndBulletsGoThrough(IsInRealm))
                    {
                        int v32 = character.GetX();
                        int v34 = GetX();
                        int v35 = character.GetY();
                        int v36 = v32 - v34;
                        int v37 = GetY();
                        int v38 = v35 - v37;
                        if (LogicMath.Abs(v36) <= v110 && LogicMath.Abs(v38) <= v110)
                        {
                            //v29 = v104;
                            int v39 = v36 * v36 + v38 * v38;
                            //v28 = v106;
                            if (v39 <= v110 * v110)
                            {
                                if (ChargeHitGIDs.Find(x => x.GID == character.GetGlobalID()) != null) continue;
                                //v40 = ZNK21LogicGameObjectServer11getGlobalIDEv(v31);
                                //v41 = a1->field_14C - 1;
                                //while (v41 + 1 >= 1)
                                //{
                                //    v42 = *(*v99 + 4 * v41--);
                                //    if (v42 == v40)
                                //        goto LABEL_41;
                                //}
                                if (true)
                                {
                                    int ChargeType = m_activeChargeType;
                                    //v44 = a1->field_180;
                                    if (ChargeType == 10)
                                    {
                                        ChargeHitGIDs.Add(new Ignoring(character.GetGlobalID(), 100));
                                        //BuffsCount = a1->BuffsCount;
                                        //v46 = 0;
                                        //v47 = 0;
                                        //if (BuffsCount >= 1)
                                        //{
                                        //    Buffs = a1->Buffs;
                                        //    v47 = 0;
                                        //    do
                                        //    {
                                        //        v49 = **Buffs;
                                        //        if (v49 <= 5 && ((1 << v49) & 38) != 0)// damage
                                        //            v47 += (*Buffs)[3];
                                        //        ++Buffs;
                                        //        --BuffsCount;
                                        //    }
                                        //    while (BuffsCount);
                                        //}
                                        //v50 = v47 + *&a1->gap124[4];
                                        //v51 = a1->ChargeDamage;
                                        //if (v102 < *&a1->gap1EE[14])
                                        //    v46 = *&a1->gap1EE[10];
                                        //v52 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
                                        //v53 = v52 == 0;
                                        //if (v52)
                                        //{
                                        //    v52 = v52[31];
                                        //    v53 = v52 == 0;
                                        //}
                                        //if (!v53)
                                        //    v51 += (*v52 - 1) * (v51 / 20);
                                        //v54 = v50;
                                        //v55 = a1->field_184;
                                        //v56 = v51 * v50 / 100 + v51;
                                        //v57 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
                                        //v58 = 200;
                                        //if (v57)
                                        //{
                                        //    v59 = *(v57 + 124);
                                        //    v29 = v104;
                                        //    v60 = v46;
                                        //    if (v59)
                                        //        v58 = 10 * *v59 + 190;
                                        //}
                                        //else
                                        //{
                                        //    v29 = v104;
                                        //    v60 = v46;
                                        //}
                                        int v61 = GamePlayUtil.CalculatePercentDamage(ChargePercentDamage, ChargeDamage, true, GetDamageBuffTemporary(), CripplePercent, ChargeDamage / 4, character);
                                        //v28 = v106;
                                        ChargeNormalDMG = 1000;
                                        v101 = v61;
                                    }
                                    else if (!CharacterData.IsTrain())
                                    {
                                        ChargeHitGIDs.Add(new Ignoring(character.GetGlobalID(), 10));
                                    }
                                    //TeamIndex = a1->TeamIndex;
                                    //v62 = GetX();
                                    //v63 = GetY();
                                    //v101 = v101 + v101 * DamageModifier / 100;
                                    if (character.CauseDamage(this, v101, ChargeNormalDMG, UltiSkill, null, true))
                                    {
                                        ++ChargeHits;
                                        if (GetCardValueForPassive("fire_dot_ulti", 1) >= 0)
                                            character.ApplyPoison(GetIndex() % 16, GetCardValueForPassive("fire_dot_ulti", 1), 0, true, this, 2);
                                        if (GetCardValueForPassive("push_charge", 1) >= 1 && ShouldExecuteColetteChargeOnReturn)
                                        {
                                            if (character.IsAlive())
                                            {
                                                if (character.CharacterData.IsHero() || character.CharacterData.IsTrainingDummy())
                                                {
                                                    if (TicksGone + 2 < MoveEnd)
                                                    {
                                                        character.SetDraggingObject(this, character.GetX() - GetX(), character.GetY() - GetY());
                                                    }
                                                }
                                            }
                                        }
                                        int v70 = GetCardValueForPassive("shield_charge", 0);// 10
                                        if (v70 >= 1)
                                        {
                                            AddShield(GetCardValueForPassive("shield_charge", 2), GetCardValueForPassive("shield_charge", 1) + ChargeHits * v70);// 100(5秒)
                                        }
                                        if (GetCardValueForPassive("charge_ulti_on_kill", 1) >= 1)
                                        {
                                            if (!character.IsAlive()) GetPlayer().ChargeUlti(4000, false, true);
                                        }
                                    }
                                    character.TriggerPushback(
                                      GetX(),
                                      GetY(),
                                      ChargePushback,
                                      true,
                                      0,
                                      false,
                                      false,
                                      false,
                                      false,
                                      false,
                                      false,
                                      false,
                                      0);
                                    if (m_activeChargeType == 8)
                                    {
                                        IsCharging = false;
                                        StopMovement();
                                        FangHitGIDs.Add(character.GetGlobalID());
                                        if (FangUltiCount <= 2)
                                        {
                                            Character Target = null;
                                            int dis = GetUltimateSkill().SkillData.CastingRange * 50;
                                            foreach (GameObject gameObject1 in GameObjectManager.GetGameObjects())
                                            {
                                                if (gameObject1.GetObjectType() != 0) continue;
                                                Character character1 = (Character)gameObject1;
                                                if (!character1.IsAlive()) continue;
                                                if (character1.GetIndex() / 16 == GetIndex() / 16) continue;
                                                if (character1.IsImmuneAndBulletsGoThrough(IsInRealm)) continue;
                                                if (GamePlayUtil.GetDistanceBetween(GetX(), GetY(), character1.GetX(), character1.GetY()) >= dis) continue;
                                                if (FangHitGIDs.Contains(character1.GetGlobalID())) continue;
                                                dis = GamePlayUtil.GetDistanceBetween(GetX(), GetY(), character1.GetX(), character1.GetY());
                                                Target = character1;

                                            }
                                            if (Target != null)
                                            {
                                                List<Ignoring> tmp = ChargeHitGIDs;
                                                List<int> tmp2 = FangHitGIDs.ToList();
                                                int t = FangUltiCount;
                                                TriggerCharge(Target.GetX(), Target.GetY(), ChargeSpeed, m_activeChargeType, ChargeDamage, ChargePushback, false);
                                                ChargeHitGIDs = tmp;
                                                FangHitGIDs = tmp2;
                                                FangUltiCount = t + 1;
                                            }
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //v2 = a1;
                            //v29 = v104;
                            //v28 = v106;
                        }

                    }
                }
            }
            if ((m_activeChargeType | 2) != 3 && !ChargeBreaksEnvironment) return;
            int v77 = GetX() / 300;
            int v79 = GetY() / 300;
            TileMap v80 = GetBattle().GetTileMap();
            int v81 = v110;
            //if (!CharacterData.IsTrain())
            //    v81 = v110 - 100;
            int XMin = LogicMath.Clamp(v77 - 2, 0, v80.Width - 1);
            int YMin = LogicMath.Clamp(v79 - 2, 0, v80.Height - 1);
            int XMax = LogicMath.Clamp(v77 + 2, 0, v80.Width - 1);
            int YMax = LogicMath.Clamp(v79 + 2, 0, v80.Height - 1);
            for (int i = XMin; i <= XMax; i++)
            {
                for (int j = YMin; j <= YMax; j++)
                {
                    if (((i * 300 + 150 - GetX()) * (i * 300 + 150 - GetX()) + (j * 300 + 150 - GetY()) * (j * 300 + 150 - GetY())) <= v81 * v81)
                    {
                        Tile tile = v80.GetTile(i, j, true);
                        if (tile != null) tile.Destruct();
                    }
                }
            }
        }

        public SkillData GetCurrentCastingSkill()
        {
            foreach (Skill skill in m_skills)
            {
                if (skill.OnActivate && skill.SkillData.CastingTime > 0) return skill.SkillData;
                if (skill.TicksActive >= 1) return skill.SkillData;
            }
            return null;
        }
        public int GetDamageBuffTemporary()
        {
            int buff = 0;
            foreach (Buff buff1 in m_buffs)
            {
                if (buff1.Type == 1 || buff1.Type == 2 || buff1.Type == 5) buff += buff1.Modifier;
            }
            if (GetPlayer()?.OverCharging ?? false)
            {
                if (CharacterData.OverchargeDamagePercent > 0)
                    buff += CharacterData.OverchargeDamagePercent;
                else buff += 20;
            }
            return buff;
        }
        public int GetBuffedHealthRegen()
        {
            int buff = 0;
            foreach (Buff buff1 in m_buffs)
            {
                if (buff1.Type == 10) buff += 1;
            }
            return buff;
        }
        public void TickRapidFire()
        {
            SkillData v2; // r0
            int v3; // r1
            int v4; // r2
            int v5; // r3
            SkillData v6; // r5
            int v7; // r0
            int v8; // r6
            int v9; // r7
            int v10; // r0
            int v11; // r0
            bool v12; // zf
            int v13; // r8
            int v14; // r0
            int v15; // r0
            int BuffsCount; // r1
            int v18; // r2
            int v20; // r0
            int v21; // r0
            int v22; // r6
            int v23; // r5
            int v25; // r0
            int result; // r0

            State = 2;
            v2 = GetCurrentCastingSkill();
            v6 = v2;
            if (v2 != null /*|| v2.CanMoveAtSameTime*/)
            {
                if (v6.Projectile == null || !DataTables.GetProjectileByName(v6.Projectile).Indirect && DataTables.GetProjectileByName(v6.Projectile).TravelType != 6)
                {
                    AttackTarget.X = AttackVector.X + GetX();
                    AttackTarget.Y = AttackVector.Y + GetY();
                }
            }
            int Damage = RapidFireDamage;
            //v8 = a1->field_9C;
            //v9 = a1->field_AC;
            //v10 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v11 = sub_41EC8C(*(v10 + 164), a1);
            //v12 = v11 == 0;
            //v13 = v8 + v9;
            //if (v11)
            //{
            //    v11 = a1->field_B8;
            //    v12 = v11 == 0;
            //}
            //if (v12 || sub_21EF94(v11) <= 0 && sub_210EF8(a1->field_B8) <= 0)
            //{
            //    if (v6 && sub_81920(v6) && (v14 = sub_81920(v6), sub_21EF94(v14) >= 1))
            //    {
            //        v15 = *&a1->gap124[4];
            //    }
            //    else
            //    {
            //        BuffsCount = a1->BuffsCount;
            //        if (BuffsCount < 1)
            //        {
            //            v18 = 0;
            //        }
            //        else
            //        {
            //            Buffs = a1->Buffs;
            //            v18 = 0;
            //            do
            //            {
            //                v19 = **Buffs;
            //                if (v19 <= 5 && ((1 << v19) & 0x26) != 0)
            //                    v18 += (*Buffs)[3];
            //                ++Buffs;
            //                --BuffsCount;
            //            }
            //            while (BuffsCount);
            //        }
            //        v15 = v18 + *&a1->gap124[4];
            //    }
            //    v13 += v15 * v13 / 100;
            //}
            //v20 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v21 = ZNK21LogicBattleModeServer12getTicksGoneEv(v20);
            //v22 = a1->field_B8;
            v23 = 0;
            //v24 = 0;
            if (RapidFireProjectile == null)
                v23 = 3;
            //if (v21 < *&a1->gap1EE[14])
            //    v24 = &dword_0 + 1;
            //Attack(
            //  a1,
            //  a1->field_15C,
            //  a1->field_160,
            //  a1->field_164,
            //  v13,
            //  a1->field_AC,
            //  v22,
            //  a1->field_BC,
            //  v24,
            //  1,
            //  v23);
            //Debugger.Print("RF:" + TicksGone.ToString());
            Damage += GetDamageBuffTemporary() * Damage / 100;
            if (RapidFireAttackPattern == 12)
            {
                Attack(
                    null,
              GetX() + LogicMath.GetRotatedX(ChargeSpeed / 20 * RapidFireMsBetweenAttacks / 50, 0, MoveAngle),
              GetY() + LogicMath.GetRotatedY(ChargeSpeed / 20 * RapidFireMsBetweenAttacks / 50, 0, MoveAngle),
              RapidFireRange,
              RapidFireProjectile,
              Damage,
              RapidFireNormalDMG,
              RapidFireSpread,
              RapidFireBulletsPerShoot,
              v23);
            }
            else
                Attack(
                    null,
                  AttackTarget.X,
                  AttackTarget.Y,
                  RapidFireRange,
                  RapidFireProjectile,
                  Damage,
                  RapidFireNormalDMG,
                  RapidFireSpread,
                  RapidFireBulletsPerShoot,
                  v23);
            Attacking = true;
            //v25 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //result = ZNK21LogicBattleModeServer12getTicksGoneEv(v25);
            RapidFireTick = TicksGone;
            //Debugger.Print("RF:"+TicksGone.ToString());
            //return result;
        }
        public bool HasCcImmunity()
        {
            return CcImmunityTicks > 0;
        }
        public int UpdateMoveDirection(
        int a1,
        LogicVector2 a2,
        LogicVector2 a3,
        int a4,
        int a5)
        {
            int v9; // r6
            int v10; // r9
            int v11; // r1
            int v12; // r0
            int v13; // r2
            int v14; // r1
            int result; // r0
            int v16; // r1
            int v17; // r0
            bool v18; // zf

            v9 = a5;
            v10 = a2.GetDistanceSquared(a3);
            //v11 = a2.GetDistanceSquaredTo(a4, a5);
            v11 = 0;

            MovementVector = new LogicVector2();
            if (v10 >= v11)
            {
                v9 = a3.Y;
                v14 = a2.X - a3.X;
            }
            else
            {
                v14 = a2.X - a4;
            }
            MovementVector.X = v14;
            MovementVector.Y = a2.Y - v9;
            MovementVector.Normalize(256);
            result = LogicMath.GetAngle(MovementVector.X, MovementVector.Y);
            //if(m_isBot==0)Debugger.Print(MovementVector.ToString());
            if (MovementVector.X != 0 || MovementVector.Y != 0) MoveAngle = result;
            //MoveAngle = TicksGone*100 % 360;
            if (KnockBacked)
            {
                if (!(Knockback1 || Knockback3))
                {
                    //v17 = ZNK21LogicGameObjectServer7getDataEv(a1);
                    //result = ZNK18LogicCharacterData11isCarryableEv(v17);
                    //v18 = result == 0;
                    //if (!result)
                    //{
                    //    result = *(a1 + 576);
                    //    v18 = result == 0;
                    //}
                    //if (v18)
                    //{
                    MoveAngle = result + 180;
                    if (result + 180 >= 360)
                    {
                        result -= 180;
                        MoveAngle = result;
                    }
                    //}
                }
            }
            return result;
        }
        public void TriggerPushback(
        int TriggerPosX,
        int TriggerPosY,
        int PushbackLength,
        bool a5,
        int a6,
        bool IgnoreInvincible,
        bool a8,
        bool a9,
        bool IgnoreCcImmunity,
        bool a11,
        bool a12,
        bool a13,
        int a14,
            int MaxX = -1,
            int MaxY = -1)
        {
            // [COLLAPSED LOCAL DECLARATIONS. PRESS KEYPAD CTRL-"+" TO EXPAND]

            if ((!Knockback2 || a12) && (!IsInvincible || IgnoreInvincible))
            {
                bool v20 = !KnockBacked;
                if (KnockBacked)
                    v20 = !Knockback1;
                if (!v20 && !a9)
                {
                    if (!a13)
                        return;
                    //ZNK28LogicGameObjectManagerServer14getGameObjectsEv();
                    //v29 = v28[2];
                    //if (v29 >= 1)
                    //{
                    //    v30 = v28;
                    //    for (i = 0; i != v29; ++i)
                    //    {
                    //        v32 = *(*v30 + 4 * i);
                    //        if (((*v32)->field_20)(v32) == 1 && v32[28] == Target)
                    //        {
                    //            v33 = ZNK21LogicProjectileServer17getProjectileDataEv(v32);
                    //            if (sub_41FD9C(v33))
                    //                sub_430204(v32, 5);
                    //        }
                    //    }
                    //}
                    int v35 = TicksGone - 1;
                    Knockback1 = false;
                    MoveStart = v35;
                    MoveEnd = v35;
                }
                //v21 = ZNK21LogicGameObjectServer7getDataEv(Target);
                //bool v22 = CharacterData.IsBase
                bool v22 = false;
                if (!v22 || CharacterData.Hitpoints <= 2)
                {
                    if (IgnoreCcImmunity || !HasCcImmunity())
                    {
                        Knockback2 = a12;
                        bool IsCarryable = CharacterData.IsCarryable();
                        if (v22 || IsCarryable)
                        {
                            if (PushbackLength > 0)
                                PushbackLength <<= a6;
                            else
                                PushbackLength = 10;
                        }
                        //if (IsCarryable | v22 | CharacterData.Speed > 0)
                        //    goto LABEL_24;
                        bool v26 = !a8;
                        if (!a8)
                            v26 = !a12;
                        //if (!v26 || *(ZNK21LogicGameObjectServer7getDataEv(Target) + 260) == 7)
                        if (IsCarryable | v22 | CharacterData.Speed > 0)
                        {
                        LABEL_24:
                            if (IsCharging && (m_activeChargeType <= 0xA) && ((1 << m_activeChargeType) & 0x482) != 0)//1 7 (Bull&&Darrel)
                            {
                                if (PushbackLength == 0 || !a9)
                                    return;
                            }
                            else if (PushbackLength == 0)
                            {
                                return;
                            }
                            //if (sub_122624(v36))
                            //    return;
                            int v37 = PushbackLength;
                            if (!a12)
                                v37 = PushbackLength / 2;
                            if (a9)
                                v37 = PushbackLength;
                            if (a8)
                                v37 = PushbackLength;
                            if (CharacterData.CollisionRadius < 150)
                                v37 = PushbackLength;
                            if (PushbackLength < 1)
                                v37 = PushbackLength;
                            int v170 = v37;
                            if (GetX() == TriggerPosX
                              && GetY() == TriggerPosY)
                            {
                                int v40 = GameObjectManager.GetBattle().GetRandomInt(10, 100);
                                int v42 = GameObjectManager.GetBattle().GetRandomInt(10, 100);
                                if (GameObjectManager.GetBattle().GetRandomInt(0, 2) == 1)
                                    v42 = -v42;
                                if (GameObjectManager.GetBattle().GetRandomInt(0, 2) == 1)
                                    v40 = -v40;
                                TriggerPosX = LogicMath.Clamp(v40 + TriggerPosX, 1, GameObjectManager.GetBattle().GetTileMap().LogicWidth - 2);
                                TriggerPosY = LogicMath.Clamp(v42 + TriggerPosY, 1, GameObjectManager.GetBattle().GetTileMap().LogicHeight - 2);
                            }
                            LogicVector2 v200 = new LogicVector2(GetX() - TriggerPosX, GetY() - TriggerPosY);
                            int v52 = 6 * LogicMath.Abs(v170);
                            if (v170 <= -1)
                            {
                                int v53 = v200.GetLength();
                                if (v52 > v53)
                                    v52 = v53;
                            }
                            int v179 = v52;
                            v200.Normalize(v52);
                            int v199 = 0;
                            int v198 = 0;
                            int v54 = GetX();
                            int v56;
                            if (v170 < 1)
                            {
                                v199 = v54 - v200.X;
                                v56 = GetY() - v200.Y;
                            }
                            else
                            {
                                v199 = v200.X + v54;
                                v56 = v200.Y + GetY();
                            }
                            v198 = v56;
                            Knockback1 = false;
                            KnockBacked = true;
                            TileMap v59 = GameObjectManager.GetBattle().GetTileMap();
                            if (!IgnoreInvincible)
                            {
                                int v63 = GetX();
                                int v64 = GetY();
                                if (MaxX != -1)
                                {
                                    if (GetX() > MaxX) v199 = LogicMath.Max(MaxX, v199);
                                    else v199 = LogicMath.Min(MaxX, v199);
                                    if (GetY() > MaxY) v198 = LogicMath.Max(MaxY, v198);
                                    else v198 = LogicMath.Min(MaxY, v198);
                                }
                                bool v65;
                                int v177 = 0;
                                int v182 = 0;
                                LogicVector2 v188 = new LogicVector2(-1, -1);
                                if (IsCarryable)
                                {
                                    v65 = GamePlayUtil.GetClosestAnyCollision(
                                            v63,
                                            v64,
                                            v199,
                                            v198,
                                            v59,
                                            v188,
                                            0,
                                            1,
                                            0,
                                            1);
                                }
                                else
                                {
                                    v65 = GamePlayUtil.GetClosestAnyCollision(
                                            v63,
                                            v64,
                                            v199,
                                            v198,
                                            v59,
                                            v188,
                                            0,
                                            1,
                                            0,
                                            1);
                                    //v66 = sub_432FEC(Target);
                                    //int v66 = 1;
                                    //v67 = v59[12];
                                    //v68 = v199;
                                    //v69 = v198;
                                    //v70 = ZNK21LogicGameObjectServer7getDataEv(Target);
                                    //v71 = sub_3C4508(v70);
                                    //v72 = v68;
                                    //v59 = v185;
                                    //sub_3CF844(v66, v63, v64, v72, v69, v67, &v188, v71);
                                    v65 = false;
                                }
                                if (v188.X != -1)
                                {
                                    int v73 = v188.X - v63;
                                    bool v74 = v65;
                                    int v75 = v64;
                                    int v76 = v188.Y - v64;
                                    int v77 = LogicMath.Sqrt(v73 * v73 + v76 * v76);
                                    if (v77 > 0)
                                    {
                                        int v79 = 20;
                                        if (v77 <= 20)
                                            v79 = LogicMath.Max(1, v77 - 5);
                                        v73 = (v77 - v79) * v73 / v77;
                                        v76 = (v77 - v79) * v76 / v77;
                                    }
                                    v198 = v76 + v75;
                                    v64 = v75;
                                    v199 = v73 + v63;
                                    v65 = v74;
                                    //if (a14 >= 1)
                                    //    sub_279A68(Target, a14, 0);
                                }
                                v199 = LogicMath.Clamp(v199, 101, GameObjectManager.GetBattle().GetTileMap().LogicWidth - 102);
                                v198 = LogicMath.Clamp(v198, 101, GameObjectManager.GetBattle().GetTileMap().LogicHeight - 102);
                                //v87 = LogicMath.Sqrt((v198 - v64) * (v198 - v64) + (v199 - v63) * (v199 - v63));
                                //sub_6F83B4(v196, &v199);
                                //sub_6F83B4(p, &v198);
                                //v182 += v87;
                                //if (v182 >= v179 - 50 || v65 == 0 || !IsCarryable)
                                //{
                                //    if (!IsCarryable)
                                //        goto LABEL_119;
                                //    v95 = sub_21394C(v199);
                                //    v96 = sub_21394C(v198);
                                //    if (!ZNK12LogicTileMap7getTileEii(v59, v95, v96))
                                //        goto LABEL_119;
                                //    v97 = ZNK12LogicTileMap7getTileEii(v59, v95, v96);
                                //    if (!sub_7A334C(*v97))
                                //        goto LABEL_119;
                                //    v98 = v199;
                                //    v99 = v198;
                                //    v100 = v199 - v63;
                                //    v101 = v198 - v64;
                                //    v102 = LogicMath.Sqrt(v100 * v100 + v101 * v101);
                                //    if (v102)
                                //    {
                                //        v103 = v102;
                                //        v100 = a1_divide_a2(300 * v100, v102);
                                //        v101 = a1_divide_a2(300 * v101, v103);
                                //    }
                                //    v63 = v98;
                                //    v64 = v99;
                                //    v179 += 300;
                                //    v199 += v100;
                                //    v198 += v101;
                                //    ++v177;
                                //}
                                //else
                                //{
                                //    v88 = v63;
                                //    v63 = v199;
                                //    v89 = v198;
                                //    v90 = v199 - v88;
                                //    v91 = v198 - v64;
                                //    v92 = LogicMath.Sqrt(v90 * v90 + v91 * v91);
                                //    v93 = v65 == 4;
                                //    v94 = v92;
                                //    if (v65 != 4)
                                //        v93 = v65 == 1;
                                //    if (v93)
                                //        v91 = -v91;
                                //    else
                                //        v90 = -v90;
                                //    if (v92)
                                //    {
                                //        v90 = a1_divide_a2(v90 * (v179 - v182), v92);
                                //        v91 = a1_divide_a2(v91 * (v179 - v182), v94);
                                //    }
                                //    v199 += v90;
                                //    v104 = v198 + v91;
                                //    v64 = v89;
                                //    v198 = v104;
                                //}
                                //v59 = v185;
                                //if (v177 >= 10)
                                goto LABEL_119;

                            }
                            Knockback3 = true;
                            LogicVector2 v190 = new LogicVector2();

                            if (GamePlayUtil.GetClosestLevelBorderCollision(
                                   GetX(),
                                   GetY(),
                                   v199,
                                   v198,
                                   v59,
                                   1,
                                   v190))
                            {
                                GamePlayUtil.GetClosestPassableSpot(1, v199, v198, 10, GetBattle().GetTileMap(), v190, true);
                                v198 = v190.Y;
                                v199 = v190.X;
                            }
                            else
                            {
                                GamePlayUtil.GetClosestPassableSpot(1, v199, v198, 10, GetBattle().GetTileMap(), v190, true);
                                v198 = v190.Y;
                                v199 = v190.X;
                            }
                            v199 = LogicMath.Clamp(v199, 101, GameObjectManager.GetBattle().GetTileMap().LogicWidth - 102);
                            v198 = LogicMath.Clamp(v198, 101, GameObjectManager.GetBattle().GetTileMap().LogicHeight - 102);
                        //sub_6F83B4(p, v62);
                        //v105 = ZN12LogicTileMap21logicToPathFinderTileEi(*(v196[0] + 1));
                        //v106 = ZN12LogicTileMap21logicToPathFinderTileEi(*(p[0] + 1));
                        //v183 = sub_432FEC(Target);
                        //if (!ZNK12LogicTileMap20isPassablePathFinderEiiibb(v59, v183, v105, v106, 0, 0))
                        //{
                        //    v169 = ZN12LogicTileMap21logicToPathFinderTileEi(*v196[0]);
                        //    v167 = ZN12LogicTileMap21logicToPathFinderTileEi(*p[0]);
                        //    v107 = LogicMath.Sqrt((v105 - v169) * (v105 - v169) + (v106 - v167) * (v106 - v167)) + 2;
                        //    v108 = LogicMath.Clamp(v105 - v107, 0, v59[28] - 1);
                        //    v109 = v59;
                        //    v172 = LogicMath.Clamp(v106 - v107, 0, v59[29] - 1);
                        //    v168 = v105;
                        //    v110 = LogicMath.Clamp(v107 + v105, 0, v59[28] - 1);
                        //    v111 = v106;
                        //    v112 = LogicMath.Clamp(v107 + v106, 0, v109[29] - 1);
                        //    v180 = -1;
                        //    v171 = v110;
                        //    v178 = -1;
                        //    if (v108 <= v110)
                        //    {
                        //        v175 = -v111;
                        //        v113 = 0x7FFFFFFF;
                        //        v114 = 0x7FFFFFFF;
                        //        v178 = -1;
                        //        v180 = -1;
                        //        do
                        //        {
                        //            if (v172 <= v112)
                        //            {
                        //                v174 = (v108 - v169) * (v108 - v169);
                        //                v115 = (v108 - v168) * (v108 - v168);
                        //                v116 = v172;
                        //                do
                        //                {
                        //                    v117 = v116;
                        //                    if (ZNK12LogicTileMap20isPassablePathFinderEiiibb(v185, v183, v108, v116, 0, 0))
                        //                    {
                        //                        v118 = v115 + (v175 + v117) * (v175 + v117);
                        //                        if (v118 >= v113)
                        //                        {
                        //                            if (v118 == v113)
                        //                            {
                        //                                v119 = v174 + (v117 - v167) * (v117 - v167);
                        //                                v120 = v178;
                        //                                v121 = v119 < v114;
                        //                                if (v119 < v114)
                        //                                {
                        //                                    v120 = v108;
                        //                                    v114 = v174 + (v117 - v167) * (v117 - v167);
                        //                                }
                        //                                v178 = v120;
                        //                                v122 = v180;
                        //                                if (v121)
                        //                                    v122 = v117;
                        //                                v180 = v122;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            v113 = v115 + (v175 + v117) * (v175 + v117);
                        //                            v180 = v117;
                        //                            v178 = v108;
                        //                            v114 = v174 + (v117 - v167) * (v117 - v167);
                        //                        }
                        //                    }
                        //                    v116 = v117 + 1;
                        //                }
                        //                while (v117 < v112);
                        //            }
                        //            v121 = v108++ < v171;
                        //        }
                        //        while (v121);
                        //    }
                        //    v123 = v169;
                        //    if (v178 != -1)
                        //        v123 = v178;
                        //    v124 = v180;
                        //    v125 = (1431655766LL* v123) >> 32;
                        //    if (v178 == -1)
                        //        v124 = v167;
                        //    v126 = (1431655766LL* v124) >> 32;
                        //    *(v196[0] + 1) = ZN12LogicTileMap21pathFinderTileToLogicEib(3 * (v125 + (v125 >> 31)) + 1, 1);
                        //    *(p[0] + 1) = ZN12LogicTileMap21pathFinderTileToLogicEib(3 * (v126 + (v126 >> 31)) + 1, 1);
                        //}
                        LABEL_119:
                            InterruptAllSkills();
                            if (GetCardValueForPassive("ulti_defense", 1) >= 0)
                            {
                                ShieldTicks = 0;
                            }
                            //v130 = IgnoreInvincible;
                            //v131 = IgnoreCcImmunity;
                            //v132 = a12;
                            //if ((IsCarryable | IgnoreInvincible) == 1)
                            //{
                            //    if (WayPoints >= 1)
                            //    {
                            //        do
                            //            sub_2234E4(Target);
                            //        while (WayPoints > 0);
                            //    }
                            //    v176 = v129;
                            //    v133 = ZNK21LogicGameObjectServer4getXEv(Target);
                            //    field_54 = ZN12LogicTileMap21logicToPathFinderTileEi(v133);
                            //    v134 = ZNK21LogicGameObjectServer4getYEv(Target);
                            //    field_58 = ZN12LogicTileMap21logicToPathFinderTileEi(v134);
                            //    field_5C = ZN12LogicTileMap21logicToPathFinderTileEi(*(v196[0] + v197 - 1));
                            //    field_60 = ZN12LogicTileMap21logicToPathFinderTileEi(*(p[0] + v195 - 1));
                            //    v135 = v197;
                            //    if (IsCarryable)
                            //    {
                            //        v184 = v197 - 1;
                            //        if (v197 >= 1)
                            //        {
                            //            v136 = 4 * v197 - 4;
                            //            v137 = 0;
                            //            p_YwayPoints = &YwayPoints;
                            //            p_XwayPoints = &XwayPoints;
                            //            do
                            //            {
                            //                v140 = (v196[0] + v136);
                            //                if (v137)
                            //                {
                            //                    sub_6F83B4(p_XwayPoints, v140);
                            //                    v141 = (p[0] + v136);
                            //                    v142 = p_YwayPoints;
                            //                }
                            //                else
                            //                {
                            //                    v143 = *v140;
                            //                    v144 = *(p[0] + v184);
                            //                    ZN12LogicVector2C2Ev(&v186);
                            //                    v145 = sub_432FEC(Target);
                            //                    sub_48C8C4(v145, v143, v144, 5, v185, &v186, 0);
                            //                    p_YwayPoints = &YwayPoints;
                            //                    sub_6F83B4(p_XwayPoints, &v186);
                            //                    v142 = &YwayPoints;
                            //                    v141 = &v187;
                            //                }
                            //                sub_6F83B4(v142, v141);
                            //                --v137;
                            //                v136 -= 4;
                            //            }
                            //            while (v135 + v137 > 0);
                            //        }
                            //    }
                            //    else if (v197 >= 1)
                            //    {
                            //        v150 = 4 * v197 - 4;
                            //        do
                            //        {
                            //            sub_6F83B4(&XwayPoints, (v196[0] + v150));
                            //            sub_6F83B4(&YwayPoints, (p[0] + v150));
                            //            --v135;
                            //            v150 -= 4;
                            //        }
                            //        while (v135 > 0);
                            //    }
                            //    v127 = Target;
                            //    *&MoveLength = ZNK20LogicCharacterServer13getPathLengthEv(Target);
                            //    v151 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(Target);
                            //    v152 = ZN21LogicBattleModeServer13getPathFinderEv(v151);
                            //    ZN20LogicCharacterServer12ensurePathOkEP15LogicPathFinder(Target, v152);
                            //    v131 = IgnoreCcImmunity;
                            //    v130 = IgnoreInvincible;
                            //    v132 = a12;
                            //    v129 = v176;
                            //}
                            //else
                            {
                                //v146 = *(p[0] + v195 - 1);
                                //v147 = *(v196[0] + v197 - 1);
                                //v148 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(Target);
                                //v149 = ZN21LogicBattleModeServer13getPathFinderEv(v148);
                                //sub_56AE8(Target, v147, v146, &PosX, v149, 0);
                                PathfindTo(0, v199, v198, 0, 0, 0, 0, 0, 0);
                            }
                            int v153 = 12 * v170;
                            if (v170 < 0)
                                v153 = -8 * v170;
                            ChargeSpeed = v153;
                            int v154;
                            int v155;
                            if (a12)
                            {
                                v154 = 2 * v153;
                            }
                            else if (a11)
                            {
                                v154 = LogicMath.Max(1, v153 / 2);
                            }
                            else if (IsCarryable)
                            {
                                int v156 = (int)(14725602162 * v153) >> 32;
                                v155 = (v156 >> 3) + (v156 >> 31);
                                v154 = LogicMath.Max(1, v155 / 2);
                            }
                            else
                            {
                                if (!a8)
                                {
                                    if (!IgnoreInvincible)
                                        goto LABEL_152;
                                    v155 = v153 / 2;
                                    v154 = LogicMath.Max(1, v155);
                                    goto LABEL_151;
                                }
                                int v157 = 6;
                                if (v170 > 500)
                                    v157 = 4;
                                int v156 = (int)(2454267027 * v157 * v153) >> 32;
                                v155 = (v156 >> 3) + (v156 >> 31);
                                v154 = LogicMath.Max(1, v155 / 2);
                            }

                        LABEL_151:
                            ChargeSpeed = v154;
                        LABEL_152:
                            int v158 = GetPathLength();
                            PathLength = v158;
                            if (v158 >= 1)
                            {
                                MoveStart = TicksGone;
                                MoveEnd = LogicMath.Max(1, 20 * v158 / ChargeSpeed) + TicksGone;
                            }
                            if (IsCarryable)
                            {
                                ChargeSpeed = 9 * ChargeSpeed / 4;
                            }
                            //else if (v127->HoldingStuff)
                            //{
                            //    v160 = ZNK21LogicGameObjectServer4getXEv(v127);
                            //    v161 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(v127);
                            //    v162 = ZN21LogicBattleModeServer12getRandomIntEii(v161, -100, 100);
                            //    v163 = ZNK21LogicGameObjectServer4getYEv(v127);
                            //    v164 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(v127);
                            //    v165 = ZN21LogicBattleModeServer12getRandomIntEii(v164, -100, 100);
                            //    sub_1F4FCC(v127, v162 + v160, v165 + v163);
                            //}
                            if (a8)
                                ChargeSpeed = LogicMath.Max(1, 13 * ChargeSpeed / 14);
                            if (!a5)
                                ChargeSpeed = 0;
                            Projectile v166 = GetControlledProjectile();
                            if (v166 != null)
                                v166.TargetReached(5);
                            //v127->gap12E = v131;
                            //if (p[0])
                            //    j_j_j_free_0(p[0]);
                            //if (v196[0])
                            //    j_j_j_free_0(v196[0]);
                            return;
                        }
                    }
                }
            }
        }
        public int GetNextSteeredPos(LogicVector2 a2)
        {
            int v4; // r8
            int v5; // r0
            int v6; // r5
            int v8; // r6
            int v9; // r9 MAPDST
            int v10; // r5
            int v11; // r9
            Character v15; // r7
            int v16; // r10
            Character v17; // r9
            int v18; // r0
            int v19; // r0
            int v20; // r0
            int v21; // r1
            int v22; // r5
            int v23; // r5
            int v24; // r5
            int v25; // r6
            int v28; // r0
            int v29; // r6
            int v30; // r5
            int v31; // r0
            int v32; // r0
            int v33; // r5
            int v34; // r5
            int v35; // r0
            float v39; // [sp+Ch] [bp-3Ch]
            float v40; // [sp+10h] [bp-38h]
            float v41; // [sp+14h] [bp-34h]
            float v42; // [sp+18h] [bp-30h]

            v9 = ChargeSpeed;
            v22 = MoveAngle;
            v29 = -1;
            if (LogicMath.NormalizeAngle360(v22 - SteerAngle) < 180)
                v29 = 1;
            v30 = LogicMath.GetAngleBetween(SteerAngle, v22);
            v31 = 8;
            v32 = LogicMath.Min(v30, v31);
            MoveAngle = LogicMath.NormalizeAngle360(MoveAngle + v32 * v29);
            v33 = GetX();
            a2.X = v33 + LogicMath.Cos(MoveAngle) * v9 / 1024 / 20;
            v34 = GetY();
            a2.Y = v34 + LogicMath.Sin(MoveAngle) * v9 / 1024 / 20;
            a2.X = LogicMath.Clamp(a2.X, 101, GetBattle().GetTileMap().LogicWidth - 102);
            a2.Y = LogicMath.Clamp(a2.Y, 101, GetBattle().GetTileMap().LogicHeight - 102);
            AttackAngle = MoveAngle;
            float v310 = 0.1F;
            float v320 = 0.1F;
            float v330 = 8400;
            if ((ChargeSpeed * 0.1) > 0.1)
                v320 = ChargeSpeed * 0.1F;
            v39 = -0.015F;

            v41 = v39 * 300.0F;
            if ((v330 / v320) > 0.1)
                v310 = v330 / v320;
            v42 = (8400 * (TicksGone - SteerStartTicks) / 120) / v320;
            v40 = (v41 * v42) * (v42 - v310);
            //Debugger.Print(a2.ToString());
            //Debugger.Print(SteerAngle);
            return LogicMath.Clamp((int)v40, 0, 1500);
        }
        public int GetPosAtTick(int a1, int a2, LogicVector2 a3, int a4)
        {
            float v4; // d1
            float v5; // d2
            float v6; // d4
            int v9; // r0
            int v11; // r2
            int v12; // r0
            int v13; // r1
            int v14; // r3
            int v15; // r10
            int v16; // r4
            int v17; // r0
            int v18; // r7
            int v19; // r0
            int v20; // r1
            int v21; // r9
            int v22; // r5
            int v23; // r6
            int v24; // r8
            int v25; // r0
            int v26; // r2
            int v27; // r7
            int v28; // r0
            int v29; // r0
            int v30; // r0
            double v31; // s18
            double v32; // s16
            int v33; // r0
            int v34; // r5
            int v35;
            double v36; // s20
            double v37; // s0
            int v38; // r0
            double v39; // s4
            double v40; // s4
            double v41; // r0
            double v42; // [sp+Ch] [bp-4Ch]
            int v43; // [sp+10h] [bp-48h]
            int v44;
            int v45; // [sp+18h] [bp-40h]
                     //int v46; // [sp+1Ch] [bp-3Ch]

            v9 = MoveStart;
            v11 = MoveEnd;
            if (v11 == v9)
            {
                PosDelta.X = GetX();
                PosDelta.Y = GetY();

                //*a4 = GetX();
                //a4[1] = GetY();
                return 0;
            }
            v12 = 1000 * LogicMath.Min(a2 - v9 + (m_isBot == 0 ? 1 : 0), v11 - v9) / (v11 - v9);
            //Debugger.Print("Time:" + v12);
            v44 = v12;
            v13 = PathLength;
            //v14 = a1[32];
            v14 = 2;
            //v46 = a1;
            v42 = v12;
            //a1[215] = v12;
            //v15 = v14 - 1;
            //v16 = 4 * v14 - 4;
            v45 = v13 * v12 / 1000;
            //Debugger.Print(v13/(v11-v9));
            v17 = 0;
            if (TicksGone > MoveEnd)
            {
                PosDelta.X = GetX();
                PosDelta.Y = GetY();
                return 0;
            }
            //do
            //{
            //    if (v15 < 1)
            //    {
            //        PosDelta.X = GetX();
            //        PosDelta.Y = GetY();

            //        //*a4 = GetX();
            //        //a4[1] = GetY();
            //        return 0;
            //    }
            //    v18 = v17;
            //    v19 = v46[30];
            //    v20 = v46[33];
            //    v21 = *(v19 + v16);
            //    v22 = *(v20 + v16);
            //    v23 = *(v19 + v16 - 4) - v21;
            //    v24 = *(v20 + v16 - 4) - v22;
            //    v25 = LogicMath.Sqrt(v23 * v23 + v24 * v24);
            //    v26 = v18;
            //    v27 = v25;
            //    v17 = v25 + v26;
            //    v16 -= 4;
            //    --v15;
            //}
            //while (v17 < v45);
            v18 = v17;
            v21 = WayPoint.X;
            v22 = WayPoint.Y;
            v23 = WayPointStart.X - v21;
            v24 = WayPointStart.Y - v22;
            v25 = LogicMath.Sqrt(v23 * v23 + v24 * v24);//pathlen
            v26 = v18;
            v27 = v25;
            v17 = v25 + v26;
            //v16 -= 4;
            //--v15;

            v18 = v17;
            v26 = v18;//pathlen
                      //Debugger.Print("Delta:" + v26+"PL:"+v13+"PLA:"+v26);

            if (v27 < 1)
            {
                v28 = v24;
            }
            else
            {
                v23 = v23 * (v45 - v26) / v27;
                v28 = v24 * (v45 - v26) / v27;
            }
            //PosDelta.X = v23 + v21;
            //PosDelta.Y = v28 + v22;
            PosDelta.X = v21 - v23;
            PosDelta.Y = v22 - v28;
            //Debugger.Print("Delta:" + v28);
            //*a4 = v21;
            //a4[1] = v22;
            if (!IsCharging || !GamePlayUtil.IsJumpCharge(m_activeChargeType))
            {
                if (!KnockBacked)
                    return 0;
            }
            if (ChargeSpeed == 0)
                return 0;
            v31 = 0.1;
            v32 = 0.1;
            v33 = PathLength;
            if ((ChargeSpeed * 0.1) > 0.1)
                v32 = ChargeSpeed * 0.1;
            if (!IsCharging)
            {
                v35 = LogicMath.Sqrt(v33);
                v36 = -6.25;
                v39 = v36 / v35;
            }
            else switch (m_activeChargeType)
                {
                    case 2:
                        v39 = -0.35;
                        break;
                    case 6:
                        v39 = -0.1;
                        break;
                    case 3:
                        v35 = LogicMath.Sqrt(v33);
                        v36 = -5.0;
                    LABEL_25:
                        v39 = v36 / v35;
                        break;
                    case 11:
                        v39 = -0.2;
                        break;
                    case 16:
                        v39 = -0.25;
                        break;
                    default:
                        v39 = -8.75 / LogicMath.Sqrt(v33);
                        break;
                }
            double d = 0;
            if (IsCharging && m_activeChargeType == 9)
            {
                v40 = sub_518AB8(v44, v44 * 0.001);
            }
            else
            {
                //double a = Math.Max(0.1, ChargeSpeed * 0.1);
                //double b = PathLength / a;
                //double c = (PathLength * v44 / 1000) / a;
                //d = v39 / LogicMath.Sqrt(PathLength) * 300 * c * (c - Math.Max(b, 0.1));
                v41 = v39 * 300.0;
                if ((v33 / v32) > 0.1)
                    v31 = v33 / v32;
                v42 = (PathLength * v44 / 1000) / v32;
                v40 = (v41 * v42) * (v42 - v31);
            }
            v37 = LogicMath.Clamp((int)v40, 0, 3000);
            //v37 = LogicMath.Clamp((int)d, 0, 3000);
            //v43 = ZNK21LogicGameObjectServer7getDataEv(v48);
            //if (sub_4C125C(v43))
            //    return 0;
            return (int)v37;
            //v30 = v46[105];
            //if (!v30)
            //    return 0;
            //v31 = v30;
            //v32 = v46[55];
            //if (*(v46 + 300))
            //{
            //    v33 = v46[98] - 2;
            //    if (v33 <= 9)
            //        __asm { ADD PC, R1, R0 }
            //    v36 = -8.75;
            //}
            //else
            //{
            //    v36 = -6.25;
            //}
            //v37 = v36 / LogicMath.Sqrt(v32);
            //if (!*(v46 + 300) || (v38 = 0, v46[98] != 9))
            //{
            //    v4.n64_u32[0] = 1036831949;
            //    v5.n64_f32[0] = v31 * 0.1;
            //    v39 = vmax_f32(v5, v4).n64_f32[0];
            //    v6.n64_f32[0] = v32 / v39;
            //    v40 = (v46[55] * v42 / 1000) / v39;
            //    v38 = (((v37 * 300.0) * v40) * (v40 - vmax_f32(v6, v4).n64_f32[0]));
            //}
            //v34 = LogicMath.Clamp(v38, 0, 3000);
            //v41 = ZNK21LogicGameObjectServer7getDataEv(v46);
            //if (!ZNK18LogicCharacterData11isCarryableEv(v41))
            //    return v34;
            //return ZNK21LogicGameObjectServer4getZEv(v46);
        }
        int sub_518AB8(int a1, double a2)
        {
            int result; // r0

            if (a2 >= 0.5)
            {
                result = 2250;
                if (a2 < 0.7)
                    return result;
                return (int)sub_4771CC((a2 * 3.3333) + -2.0, 2250.0, 0.0);
            }
            else
            {
                return (int)sub_3A5150(a2 + a2, 0.0, 2250.0);
            }
        }
        double sub_4771CC(double a1, double a2, double a3)
        {
            return a2 + (a1 * a1 * a1 * (a3 - a2));
        }
        double sub_3A5150(double a1, double a2, double a3)
        {
            return a2 + (((((a1 + -1.0) * (a1 + -1.0)) * (a1 + -1.0)) + 1.0) * (a3 - a2));
        }
        public int GetSkillHoldedTicks()
        {
            //return 10;
            if (SkillHoldTicks < 0) return 0;
            return LogicMath.Min(SkillHoldTicks, GetMaxHoldTicks());
        }
        public void TickInvisibility()
        {
            SkillData result;
            VisibilityState = 0;
            if (InvisibleTicks < 1)
            {
                IsInvisible = false;
                YellowEye = false;
                goto LABEL_20;
            }
            else
            {
                InvisibleTicks--;
                if (InvisibleTicks < 1)
                {
                    VisibilityState = 2;
                    InvisibleTicks = 0;
                    IsInvisible = false;
                    YellowEye = false;
                    goto LABEL_20;
                }
            }
            //SetForcedInvisible();

            IsInvisible = true;
        //v4 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
        //v5 = sub_5F01A8(v4);
        //v6 = GetX();
        //v7 = sub_21394C(v6);
        //v8 = GetY();
        //v9 = sub_21394C(v8);
        //v32 = v5;
        //v33 = *(ZNK12LogicTileMap7getTileEii(v5, v7, v9) + 48);
        //v37 = a1->field_35C;
        //nullsub_4();
        //v11 = v10;
        //v12 = v10[2];
        //if (v12 >= 1)
        //{
        //    for (i = 0; i < v12; ++i)
        //    {
        //        v14 = *(*v11 + 4 * i);
        //        if ((*(*v14 + 32))(v14))
        //            break;
        //        if ((*(*v14 + 48))(v14))
        //        {
        //            if (!*(v14 + 864) && *(v14 + 44) != *a1->Index)
        //            {
        //                v15 = ZNK21LogicGameObjectServer4getXEv(v14);
        //                v16 = GetX();
        //                v35 = ZNK21LogicGameObjectServer4getYEv(v14);
        //                v17 = v15 - v16;
        //                v34 = GetY();
        //                if (LogicMath.Abs(v17) <= v37
        //                  && LogicMath.Abs(v35 - v34) <= v37
        //                  && v17 * v17 + (v35 - v34) * (v35 - v34) <= (v37 * v37))
        //                {
        //                    if (!v33
        //                      || (v36 = ZNK20LogicCharacterServer14getVisionRangeEv(v14),
        //                          v18 = ZNK21LogicGameObjectServer4getXEv(v14),
        //                          v19 = ZNK21LogicGameObjectServer4getYEv(v14),
        //                          v20 = ZNK21LogicGameObjectServer4getXEv(a1),
        //                          v21 = ZNK21LogicGameObjectServer4getYEv(a1),
        //                          ZNK12LogicTileMap23canPlayerSeeFromToNoLOSEiiiiib(v32, v18, v19, v20, v21, v36, 0)))
        //                    {
        //                        a1->YellowEye = 1;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //p_YellowEye = &a1->YellowEye;
        //if (a1->dword2A0)
        //    a1->YellowEye = 1;
        LABEL_20:
            //v23 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //v24 = *(v23 + 80);
            //if (v24 >= 1)
            //{
            //    v25 = *(v23 + 72);
            //    do
            //    {
            //        v26 = *v25;
            //        if (*v25)
            //        {
            //            v27 = *a1->Index;
            //            v28 = *(v26 + 12);
            //            v29 = v28 == v27;
            //            if (v28 != v27)
            //            {
            //                v26 = *(v26 + 204);
            //                v29 = v26 == 0;
            //            }
            //            if (!v29 && *(v26 + 16) == 12 && *(v26 + 36))
            //                *p_YellowEye = 1;
            //        }
            //        ++v25;
            //        --v24;
            //    }
            //    while (v24);
            //}
            result = GetActiveSkill("Invisibility");
            if (result != null)
            {
                if (InvisibleTicks <= 0) VisibilityState = 1;
                InvisibleTicks = 3;
                BreakInvisibilityOnAttack = result.BreakInvisibilityOnAttack;
                SeeInvisibilityDistance = result.SeeInvisibilityDistance;
                if (GetCardValueForPassive("speed_invisible", 1) >= 0)
                    ApplyBuff(4, 10, GetCardValueForPassive("speed_invisible", 1), 0);

            }
        }
        public CharacterData GetData() => CharacterData;
        public void TriggerRapidFire(int x, int y, int attp, int bps, int msb, ProjectileData projectileData, int damage, int range, int spread, int actime, int normalDMG, bool EFAI)
        {
            int v16; // r7
            int v21; // r0
            bool v22; // zf
            char v23; // r6
            int v24; // r0
            int v25; // r0
            int v26; // r0
            int v27; // r1
            int v28; // r8
            int v29; // r10
            int v30; // r7
            int v31; // r0
            int v32; // r9
            int result; // r0
            int v34; // r0
            int v35; // r1
            bool v36; // zf
            int i; // r7
            int v38; // r5

            MeleeAttackPushBacks = false;
            if (CharacterData.Name == "CannonGirlSmall")
            {
                if (ChargeUpType == 1)
                {
                    if (ChargeUp >= ChargeUpMax)
                    {
                        damage += 240;
                        AddShield(10, 40);
                        ChargeUp = 0;
                    }
                }
            }
            else
            if (ChargeUpType == 1)
            {
                if (ChargeUp >= ChargeUpMax)
                {
                    MeleeAttackPushBacks = true;
                    MeleeAttackPushBack = m_skills[0].SkillData.ChargePushback;
                }
                ChargeUp = 0;
            }
        //v21 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
        //v22 = v21 == 0;
        //if (v21)
        //{
        //    v16 = v21;
        //    v21 = *(v21 + 20);
        //    v22 = v21 == 0;
        //}
        //if (v22)
        //{
        //LABEL_11:
        //    v23 = a16;
        //    goto LABEL_12;
        //}
        //v23 = a16;
        //if (ZNK17LogicSkinConfData17getUltiProjectileEv(*(v21 + 144)))
        //    a12 = ZNK17LogicSkinConfData17getUltiProjectileEv(*(*(v16 + 20) + 144));
        LABEL_12:
            //v28 = a13;
            Attacking = true;
            //*(a1 + 521) = v23;
            //*(a1 + 344) = 0;
            //*(a1 + 864) = 0;
            //*(a1 + 868) = 0;
            //*(a1 + 574) = 0;
            if (projectileData != null && (projectileData.Indirect || projectileData.TravelType == 6))
            {
                //*(a1 + 348) = a2;
                //*(a1 + 352) = a3;
                AttackTarget = new LogicVector2(x, y);
                v30 = x - GetX();
                v29 = y - GetY();
            }
            else
            {
                v30 = x - GetX();
                v29 = y - GetY();
                v31 = LogicMath.Sqrt(v30 * v30 + v29 * v29);
                if (v31 > 0)
                {
                    v32 = 100 * range;
                    v30 = v30 * v32 / v31;
                    v29 = v29 * v32 / v31;
                }
                //*(a1 + 348) = ZNK21LogicGameObjectServer4getXEv(a1) + v30;
                //v28 = a13;
                //*(a1 + 352) = ZNK21LogicGameObjectServer4getYEv(a1) + v29;
                //*(a1 + 204) = v30;
            }
            AttackVector = new LogicVector2(v30, v29);
            //*(a1 + 292) = a14;
            //*(a1 + 196) = a5;
            SkillHoldTicks = -1;
            //*(a1 + 528) = 0;
            //*(a1 + 164) = 0;
            //*(a1 + 168) = 0;
            //*(a1 + 212) = a15;
            //*(a1 + 184) = a12;
            //result = -100;
            //*(a1 + 208) = v29;
            //*(a1 + 188) = a9;
            //*(a1 + 192) = a8;
            //*(a1 + 156) = a4;
            //*(a1 + 160) = a6;
            //*(a1 + 172) = a7;
            //*(a1 + 176) = a10;
            //*(a1 + 180) = a11;
            RapidFireAttackPattern = attp;
            RapidFireBulletsPerShoot = bps;
            RapidFireDamage = damage;
            RapidFireNormalDMG = normalDMG;
            RapidFireMsBetweenAttacks = msb;
            RapidFireProjectile = projectileData;
            RapidFireRange = range;
            RapidFireSpread = spread;
            RapidFireX = x;
            RapidFireY = y;
            ShootIndex = 0;
            LeftHand = 0;
            if (UltiSkill && GetCardValueForPassive("more_bullets_ulti", 1) >= 1)
            {
                RapidFireMsBetweenAttacks = GetCardValueForPassive("more_bullets_ulti", 0);
            }
            int unloadspeed = GetCardValueForPassive("decrease_unload_time", 1);
            if (!UltiSkill && unloadspeed >= 1)
            {
                RapidFireMsBetweenAttacks -= RapidFireMsBetweenAttacks * unloadspeed / 100;
            }
            if (GetData().UniqueProperty == 1)
            {
                RapidFireDamage += RapidFireDamage * GetPowerLevel() / 2;
                //RapidFireNormalDMG += RapidFireNormalDMG * GetPowerLevel() / 4;
            }
            if (Accessory != null && Accessory.Type == "take_damage" && Accessory.AccessoryData.SubType == 3 && Accessory.IsActive)
            {
                RapidFireDamage += Accessory.RewindHP / 2;
                Accessory.EndAccessoryActivation();
            }
            RapidFireShootTimes = actime / msb;
            //if (!v28)
            //{
            //    v34 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
            //    result = ZNK21LogicBattleModeServer12getTicksGoneEv(v34);
            //}
            //v35 = *(a1 + 512);
            //LastAttckTick = TicksGone;
            //v36 = v35 == 0;
            //if (v35)
            //{
            //    result = *(a1 + 948);
            //    v36 = result == 0;
            //}

            Accessory accessory = GetPlayer()?.Accessory;
            if (!UltiSkill && accessory != null)
            {
                if (accessory.IsActive && accessory.Type == "next_attack_change" && accessory.AccessoryData.SubType == 14)
                {
                    RapidFireBulletsPerShoot = accessory.AccessoryData.CustomValue1;
                    RapidFireSpread = accessory.AccessoryData.CustomValue2;
                    RapidFireDamage += RapidFireDamage * accessory.AccessoryData.CustomValue3 / 100;
                    if (accessory.AccessoryData.ActiveTicks == 100000) accessory.EndAccessoryActivation();
                }
            }
            if (EFAI)
            {
                LastAttckTick = -100;
            }
            else
            {
                LastAttckTick = TicksGone;
            }
            if (IsInvisible && BreakInvisibilityOnAttack)
            {
                foreach (Skill skill in m_skills)
                {
                    if (skill.SkillData.BehaviorType == "Invisibility") skill.TicksActive = 0;
                }
            }
            foreach (Skill skill in m_skills)
            {
                if (skill.SkillData.BehaviorType == "ProjectileShield") skill.TicksActive = 0;
            }
            //return result;
        }
        public void PauseMovement()
        {
            if (MoveEnd >= TicksGone)
            {
                MoveStart++;
                MoveEnd++;
            }
        }
        public int GetPathLength()
        {
            return LogicMath.Sqrt(
                         (WayPoint.X - WayPointStart.X) * (WayPoint.X - WayPointStart.X)
                       + (WayPoint.Y - WayPointStart.Y) * (WayPoint.Y - WayPointStart.Y));
        }
        public void SetForcedAngle(int a2)
        {
            MoveAngle = a2;
            AttackAngle = a2;
            ForcedAngleTicks = TicksGone;
        }
        public void TickProjectileShield()
        {
            ProjectileShieldCoolDown = LogicMath.Max(0, ProjectileShieldCoolDown - 1);
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 1) continue;
                if (gameObject.GetIndex() / 16 == GetIndex() / 16) continue;
                if (gameObject.GetPosition().GetDistance(Position) > (ProjectileShieldScale + 100) || gameObject.GetPosition().GetDistance(Position) < (ProjectileShieldScale - 100)) continue;
                int Angle = LogicMath.GetAngle(gameObject.GetX() - GetX(), gameObject.GetY() - GetY());
                if (Angle < AttackAngle - ProjectileShieldAngle / 2 || Angle > AttackAngle + ProjectileShieldAngle / 2) continue;
                Projectile projectile = (Projectile)gameObject;
                projectile.TargetReached(3);
                if (ProjectileShieldCoolDown == 0)
                {
                    //Projectile buster = new Projectile(6, 609);
                    //AttackOffsetX = projectile.GetX();
                    //AttackOffsetY = projectile.GetY();
                    //buster.ShootProjectile(Angle, this, 1200, 0, false, 20);
                    //GameObjectManager.AddGameObject(buster);
                    ProjectileShieldCoolDown = 8;
                }
            }
        }
        public bool IsPet()
        {
            if (TeamIndex != -1) return false;
            if (GetIndex() < 0) return false;
            return !CharacterData.IsHero();
        }
        private void HandleMoveAndAttack()
        {
            int v2; // r9
            int v3; // r6
                    //int TicksGone; // r10
            int MoveStartTime; // r5
            bool active; // r7
            int v7; // r0
            bool v8; // zf
            int v9; // r8
            int v10; // r2
            int v11; // r1
            int v12; // r0
            int v13; // r0
            bool v14; // zf
            int v15; // r0
            int v16; // r8
            int v17; // r0
            bool v18; // zf
            int v19; // r0
            int v20; // r6
            int v21; // r5
            int v22; // r0
            int v23; // r5
            int v24; // r6
            int v25; // r10
            int v26; // r9
            int v27; // r7
            int v28; // r0
            int v29; // r0
            int v30; // r6
            int v31; // r5
            int v32; // r5
            int v33; // r2
            int v34; // r1
            int v35; // r0
            int v36; // r3
            int v37; // r0
            int v38; // r0
            int v39; // r1
            int v40; // r2
            int v41; // r3
            int v42; // r0
            Character v43; // r5
            int v44; // r7
            int v45 = 0; // r0
            int ChargeType; // r0
            bool v47; // zf
            int v48; // r0
            int v49; // r0
            int v50; // r1
            int v51; // r1
            int v52; // r8
            int v53; // r6
            int v54; // r0
            int v55; // r10
            int v56; // r5
            int v57; // r0
            int v58; // r0
            bool v59; // zf
            int v60; // r5
            int v61; // r0
            int v62; // r0
            int v63; // r5
            int v64; // r6
            int v65; // r0
            int v66; // r8
            int v67; // r7
            int v68; // r6
            int v69; // r0
            int v70; // r0
            int v71; // r0
            int v72; // r9
            int v73; // r0
            int v74; // r6
            int v75; // r0
            int v76; // r0
            int v77; // r0
            int v78; // r6
            int v79; // r5
            int v80; // r9
            bool v81; // zf
            int v82; // r8
            int v83; // r5
            int v84; // r0
            int v85; // r0
            int v86; // r5
            int v87; // r6
            int v88; // r5
            int v89; // r8
            int v90; // r0
            int v91; // r6
            int v92; // r0
            int v93; // r5
            int v94; // r10
            int v95; // r0
            int v96; // r6
            int v97; // r7
            int v98; // r0
            int v99; // r0
            int v100; // r0
            int v101; // r1
            int v102; // r0
            int v103; // r0
            int v104; // r0
            int v105; // r0
            int v106; // r1
            int v107; // r2
            int v108; // r3
            int v109; // r0
            int v110; // r0
            int v111; // r1
            int v112; // r6
            int v113; // r7
            int v114; // r5
            int v115; // r6
            int v116; // r5
            int v117; // r0
            int v118; // r2
            int v119; // r0
            int v120; // r0
            int v121; // r0
            int v122; // r0
            int v123; // r6
            int v124; // r0
            int v125; // r0
            bool v126; // zf
            bool v127 = true; // zf
            bool Charging = false; // r7
            int v129; // r8
            int v130; // r0
            int v131; // r1
            int v132; // r0
            int v133; // r0
            int v134; // r8
            int v135; // r0
            int v136; // r8
            int v137; // r0
            int v138; // r0
            int v139; // r0
            int v140; // r0
            int v142; // r0
            int v143; // r1
            int v146; // r0
            int v147; // r0
            int v148; // r3
            int v149; // r2
            int v150; // r7
            int v151; // r6
            int v152; // r8
            int v153; // r5
            int v154; // r7
            int v155; // r6
            int v156; // r9
            int v157; // r0
            int v158; // r1
            int v159; // r0
            int v160; // r0
            int v161; // r1
            int v162; // r2
            int v163; // r3
            bool v164; // zf
            int dword3B8; // r5
            int v166; // r0
            int v167; // r0
            int v168; // r5
            int v169; // r0
            int v170; // r6
            int v171; // r0
            int v172; // r6
            int v173; // r5
            LogicVector2 v174; // r0
            LogicVector2 v175; // r7
            int v176; // r6
            int v177; // r9
            int v178; // r5
            int v179; // r6
            int v180; // r10
            int v181; // r6
            int v182; // r8
            int v183; // r8
            int v184; // r8
            int v185; // r10
            int v186; // r6
            int v187; // r7
            int v188; // r0
            int v189; // r1
            bool v190; // zf
            int v191; // r0
            int v192; // r0
            int v193; // r7
            int v194; // r10
            int TargetY; // r9
            int v196; // r6
            int v197; // r7
            int v198; // r0
            int TargetX; // r2
            int FangUltiCount; // r9
            int v201; // r0
            int v202; // r0
            int v203; // r0
            int v204; // r0
            int v205; // r9
            int BuffsCount; // r2
            int v207; // r8
            int v208; // r0
            int Buffs; // r3
            int v210; // r0
            int v211; // r6
            int v212; // r8
            int v213; // r6
            int v214; // r5
            int v215; // r6
            int v216; // r0
            bool v217; // zf
            int v218; // r2
            int v219; // r5
            int v220; // r0
            int v222; // r0
            bool v223; // zf
            int v224; // r10
            int v225; // r6
            int v226; // r9
            int v227; // r7
            int v228; // r8
            int v229; // r5
            int v230; // r0
            int v231; // r1
            int v232; // r7
            int v233; // r9
            int v234; // r0
            int v235; // r0
            int v236; // r9
            int v237; // r6
            int v238; // r0
            Character v239; // r5
            int v240; // r7
            int v241; // r6
            int v242; // r5
            int v243; // r7
            int v244; // r0
            int v245; // r8
            int v246; // r0
            int v247; // r9
            int v248; // r0
            int v249; // r6
            int v250; // r0
            int v251; // r0
            int v252; // r0
            int v253; // r0
            int v254; // r6
            int v255; // r0
            int v256; // r0
            int v257; // r1
            int v258; // r0
            int v259; // r7
            int v260; // r5
            int v262; // r8
            int v263; // r10
            int v264; // r7
            int v265; // r0
            int v266; // r1
            int v268; // r2
            int v269; // r5
            int v270; // [sp+0h] [bp-B0h]
            int v271; // [sp+10h] [bp-A0h]
            int v272; // [sp+38h] [bp-78h]
            int v273; // [sp+40h] [bp-70h]
            int v274; // [sp+40h] [bp-70h]
            int v275; // [sp+44h] [bp-6Ch]
            int v276; // [sp+44h] [bp-6Ch]
            int v277; // [sp+48h] [bp-68h]
            int v278; // [sp+48h] [bp-68h]
            int v279; // [sp+48h] [bp-68h]
            int v280; // [sp+48h] [bp-68h]
            bool v281; // [sp+4Ch] [bp-64h]
            int v282; // [sp+4Ch] [bp-64h]
            int MoveEndTime; // [sp+50h] [bp-60h]
            int v284; // [sp+50h] [bp-60h]
            int v285; // [sp+50h] [bp-60h]
            int v286; // [sp+50h] [bp-60h]
            int v287; // [sp+50h] [bp-60h]
            int v288; // [sp+50h] [bp-60h]
            int v289; // [sp+54h] [bp-5Ch]
            int v290; // [sp+54h] [bp-5Ch]
            int v291; // [sp+54h] [bp-5Ch]
            int v292; // [sp+54h] [bp-5Ch]
            int v293; // [sp+54h] [bp-5Ch]
            int unsigned___int81B0; // [sp+58h] [bp-58h]
            int v295; // [sp+58h] [bp-58h]
            int v296; // [sp+58h] [bp-58h]
            int v297; // [sp+58h] [bp-58h]
            int v298; // [sp+58h] [bp-58h]
            int v299; // [sp+58h] [bp-58h]
            int v300; // [sp+58h] [bp-58h]
            int v301; // [sp+5Ch] [bp-54h]
            int v302; // [sp+5Ch] [bp-54h]
            int v303; // [sp+60h] [bp-50h]
            int v304; // [sp+64h] [bp-4Ch]

            bool ShouldHandleMoveAndAttack = false;
            foreach (GameObject obj in GameObjectManager.GetGameObjects())
            {
                if (Position.GetDistance(obj.GetPosition()) <= 900 && obj.GetIndex() / 16 != GetIndex() / 16)
                {
                    obj.SetForcedVisible();
                }

                if (CharacterData.IsHero())
                {
                    if (obj.GetObjectType() == 3)
                    {
                        Item item = (Item)obj;
                        if (Position.GetDistance(item.GetPosition()) < 350 && item.CanBePickedUp(this) && !IsImmuneAndBulletsGoThrough(item.IsInRealm))
                        {
                            item.PickUp(this);
                        }
                    }
                }
            }
            //    v2 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //    v3 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
            //    TicksGone = ZNK21LogicBattleModeServer12getTicksGoneEv(v3);
            //    MoveEndTime = a1->MoveEndTime;
            //    unsigned___int81B0 = a1->unsigned___int81B0;
            //    MoveStartTime = a1->MoveStartTime;
            active = HasActiveSkill("Attack");
            //    v7 = ZNK20LogicCharacterServer14hasActiveSkillEi(a1, 8);
            //    v9 = v7;
            //if(m_isBot==0) Debugger.Print(IsCharging.ToString());
            if (!active && !IsCharging && !HasActiveSkill("ProjectileShield")) Ultiing = false;
            //if (m_skills.Count > 1 ? !m_skills[1].IsActive : false) Ultiing = false;
            //    v10 = *&a1->gap140[48];
            //    v11 = &DEPLOY_TICKS_FIRST_SPAWN;
            //    if (v10)
            //        v11 = &dword_954290;
            v281 = active || HasActiveSkill("ProjectileShield");
            //    v289 = MoveStartTime;
            if (TicksGone < DeployedTick + DEPLOY_TICKS)
            {
                if (!CharacterData.IsHero())
                {
                    State = 4;
                    BlockHealthRegen();
                    ShouldHandleMoveAndAttack = false;
                    PauseMovement();
                    goto LABEL_19;
                }
            }
            //if (ZNK21LogicBattleModeServer26isGameOverOrRoundResettingEv(v3))
            //{
            //    v16 = 0;
            //    a1->State = 0;
            //    goto LABEL_19;
            //}
            ShouldHandleMoveAndAttack = true;
            if (CastingTime >= 1)
            {
                State = 0;
                BlockHealthRegen();
                if (!(GetCurrentCastingSkill()?.CanMoveAtSameTime ?? true)) PauseMovement();
                //v38 = sub_5388E0(a1);
                v16 = 1;
                //if (!v38 || sub_68096C(v38, v39, v40, v41))
                //    goto LABEL_19;
                //LABEL_13:
                //sub_210910(a1);
                //v16 = 0;
                goto LABEL_19;
            }
            if (HasActiveSkill("Ww"))
            {
                State = 0;
                BlockHealthRegen();
                TickWhirlwind();
                v16 = 1;
                goto LABEL_19;
            }
            if (active)
            {
                State = 2;
                SetForcedVisible();
                BlockHealthRegen();
                if (RapidFireTick != TicksGone)
                    TickRapidFire();
                //v104 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //*&a1->gap26C[24] = ZNK21LogicBattleModeServer12getTicksGoneEv(v104);
                //v105 = sub_5388E0(a1);
                //v16 = 1;
                //if (v105 && !sub_68096C(v105, v106, v107, v108))
                //{
                //    sub_6562A0(a1);
                //    goto LABEL_13;
                //}
            }
            else
            {
                v16 = 1;
                if (m_stunTicks >= 1)
                {
                    State = 0;
                    HealthRegenBlockedTick = TicksGone;
                    m_stunTicks--;
                }
                else IsWeaklyStunned = false;
            }
            if (HasActiveSkill("ProjectileShield"))
            {
                State = 2;
                TickProjectileShield();
            }

        LABEL_19:
            if (DraggingObject != null)
            {
                if (DraggingObject.GetObjectType() == 0)
                {
                    Character DraggingObject = (Character)this.DraggingObject;
                    if (DraggingObject.IsAlive() && TicksGone < DraggingObject.MoveEnd - 2 && DraggingObject.IsCharging)
                    {
                        v277 = DraggingObject.GetX();
                        v24 = DraggingObject.GetY();
                        LogicVector2 v309 = new LogicVector2(-1, -1);
                        v26 = GetX();
                        v27 = GetY();
                        v30 = DraggingOffsetY + v24;
                        v31 = DraggingOffsetX + v277;
                        if (GamePlayUtil.GetClosestAnyCollision(
                               v26,
                               v27,
                               v31,
                               v30,
                               GameObjectManager.GetBattle().GetTileMap(),
                               v309,
                               0,
                               0,
                               0,
                               0))
                        {
                            v32 = DraggingObject.GetX();
                            v33 = DraggingObject.GetY();
                            v34 = v32 + 1;
                            v36 = 0;
                        }
                        else
                        {
                            v36 = GetZ();
                            v34 = v31;
                            v33 = v30;
                        }
                        SetPosition(v34, v33, v36);
                    }
                    else this.DraggingObject = null;
                }
                else
                {
                    Projectile DO = (Projectile)DraggingObject;
                    if (DO.IsAlive() && DO.State == 0) SetPosition(DraggingOffsetX + DO.GetX(), DraggingOffsetY + DO.GetY(), 0);
                    else DraggingObject = null;
                }
            }
            if (!ShouldHandleMoveAndAttack) return;
            //    if (!v16)
            //        JUMPOUT(0x4016A8);
            v43 = null;
            //    v44 = TicksGone - v289;
            //    v45 = 0;
            if (IsCharging)
            {
                SetForcedVisible();
                if (!GamePlayUtil.IsJumpCharge(m_activeChargeType) || TicksGone == MoveEnd) TickCharge();
                //v48 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //v49 = ZNK21LogicBattleModeServer12getTicksGoneEv(v48);
                v50 = MoveStart;
                //*&a1->gap26C[24] = v49;
                v45 = LogicMath.Clamp(TicksGone - v50, 0, 255);
            }
            ChargeAnimation = v45;
            //    v290 = MoveEndTime - v289;
            if (!IsPlayerControlRemoved() && CharacterData.HasAutoAttack() && CharacterData.AutoAttackRange >= 1 && !IsInvincible)
            {
                v52 = TicksGone;
                v53 = 0;
                //if (sub_3B5188(v2))
                //{
                //    v54 = sub_3B5188(v2);
                //    v53 = *(v54 + 88);
                //    if (*(v54 + 88))
                //        v53 = 1;
                //}
                //v55 = sub_5C9830(v2);
                //v56 = *(v2 + 260);
                //v57 = sub_533B98(v2);
                v43 = GetClosestEnemy(CharacterData.AutoAttackRange, 1, CharacterData.IsBase(), false, null, false, false, false);

                //v43 = sub_3F2F04(a1, v55, 1, v56 == 6, v53, 0, 0, v57, 0);
                //TicksGone = v52;
                if (Attacking)
                    v43 = null;
            }
            //    v51 = unsigned___int81B0;
            //LABEL_53:
            if (Attacking) Attacking = false;
            if (v43 != null && !Attacking)
            {
                Attacking = true;
                AutoAttackTarget = v43;
            }
            if (AutoAttackTarget == null)
                ;
            else
            {
                if (AutoAttackTarget.Position.GetDistance(Position) <= CharacterData.AutoAttackRange * 100 && AutoAttackTarget.IsAlive())
                {
                    if (!CharacterData.IsHero() && !IsPlayerControlRemoved()) StopMovement();
                }
                else
                {
                    //goto
                    ;
                }
            }
        //    if (v43 && !v58)
        //    {
        //        a1->gap140[40] = 1;
        //        *&a1->gap140[28] = v43;
        //    }
        //    v60 = a1_divide_a2(100 * a1->HP, a1->MaxHP);
        //    v61 = sub_1222F4(v2);
        //    if (v60 >= 76 && v61)
        //    {
        //        a1->gap140[40] = 0;
        //        *&a1->gap140[28] = 0;
        //    }
        //    v302 = TicksGone;
        //    if (*(v2 + 260) != 2 || (v62 = sub_92E60(a1->field_C, *&a1->gapDD[3])) == 0)
        //    {
        //        v295 = 0;
        //        goto LABEL_71;
        //    }
        //    v63 = v62;
        //    v64 = ZNK21LogicGameObjectServer4getXEv(v62);
        //    v65 = GetX();
        //    v66 = v44;
        //    v67 = (v64 - v65) * (v64 - v65);
        //    v68 = ZNK21LogicGameObjectServer4getYEv(v63);
        //    v69 = GetY();
        //    v2 = v304;
        //    v70 = v67 + (v68 - v69) * (v68 - v69);
        //    v295 = 1;
        //    v44 = v66;
        //    if (v70 > 0xC5C10)
        //        goto LABEL_71;
        //    v71 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
        //    v72 = sub_5F01A8(v71);
        //    v73 = ZNK21LogicGameObjectServer4getXEv(v63);
        //    v74 = (1431655766LL* ZN12LogicTileMap21logicToPathFinderTileEi(v73)) >> 32;
        //    v75 = ZNK21LogicGameObjectServer4getYEv(v63);
        //    v76 = ZN12LogicTileMap21logicToPathFinderTileEi(v75);
        //    if (!*(ZNK12LogicTileMap7getTileEii(v72, v74 + (v74 >> 31), v76 / 3) + 48))
        //    {
        //        v2 = v304;
        //        v295 = 0;
        //        v44 = v66;
        //        goto LABEL_71;
        //    }
        //    v77 = *(v63 + 644);
        //    v44 = v66;
        //    v2 = v304;
        //    if (TicksGone < v77 + 20)
        //    {
        //        v295 = 0;
        //        if (v77 > *&a1->gap140[48] + 10)
        //        {
        //        LABEL_71:
        //            v81 = a1->gap140[40] == 0;
        //            if (a1->gap140[40])
        //                v81 = *&a1->gap140[28] == 0;
        //            if (v81)
        //            {
        //                v82 = 0;
        //                v79 = 0;
        //                if (!sub_1222F4(v2))
        //                    goto LABEL_109;
        //                v82 = 0;
        //                v79 = 0;
        //                if (a1->field_3BC > 0)
        //                    goto LABEL_109;
        //                v83 = GetX();
        //                v84 = GetY();
        //                v85 = sub_471198(a1, v83, v84);
        //                if (!v85)
        //                {
        //                    v82 = 0;
        //                    goto LABEL_108;
        //                }
        //                v86 = v85;
        //                v87 = sub_11E368(*(v85 + 40));
        //                v88 = sub_11E368(*(v86 + 44));
        //                v89 = GetX();
        //                v90 = GetY();
        //                v284 = v87;
        //                v278 = v88;
        //                v91 = sub_319808(v89, v90, v87, v88) + 100;
        //                v92 = sub_5C9830(v2);
        //                v82 = 0;
        //                v79 = 0;
        //                if (v91 >= ZN12LogicTileMap21pathFinderTileToLogicEib(v92, 0))
        //                    goto LABEL_109;
        //                v82 = 1;
        //                *&a1->gap140[32] = v284;
        //                *&a1->gap140[36] = v278;
        //            }
        //            else
        //            {
        //                v285 = v44;
        //                v93 = sub_5C9830(v2) + 4;
        //                v82 = 0;
        //                v94 = 0;
        //                if (sub_3B5188(v2))
        //                {
        //                    v95 = sub_3B5188(v2);
        //                    v94 = *(v95 + 88);
        //                    if (*(v95 + 88))
        //                        v94 = 1;
        //                }
        //                v96 = *&a1->gap140[28];
        //                v97 = *(v304 + 260);
        //                v98 = sub_533B98(v304);
        //                v2 = v304;
        //                v99 = sub_104420(a1, v93, v96, 0, v97 == 6, v94, v98);
        //                TicksGone = v302;
        //                v44 = v285;
        //                v79 = 0;
        //                if (!v99)
        //                {
        //                LABEL_109:
        //                    if (sub_28A580(v2))
        //                    {
        //                        v123 = 1;
        //                        if (a1->gap140[40])
        //                        {
        //                        LABEL_117:
        //                            v80 = v303;
        //                            v78 = v123 != 0;
        //                            goto LABEL_118;
        //                        }
        //                        v124 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
        //                        if (v124)
        //                        {
        //                            v125 = *(v124 + 204);
        //                            v126 = v125 == 0;
        //                            if (v125)
        //                                v126 = *(v125 + 36) == 0;
        //                            if (!v126)
        //                            {
        //                                if (*(v125 + 16) != 19)
        //                                    v123 = v82;
        //                                goto LABEL_117;
        //                            }
        //                        }
        //                    }
        //                    v123 = v82;
        //                    goto LABEL_117;
        //                }
        //                if (sub_3B5188(v304) && !*(sub_3B5188(v304) + 88))
        //                {
        //                    v112 = sub_5C9830(v304);
        //                    ZN12LogicVector2C2Ev(v308);
        //                    v113 = 100 * v112;
        //                    v114 = ZNK21LogicGameObjectServer4getXEv(*&a1->gap140[28]);
        //                    v115 = v114 - GetX();
        //                    v116 = ZNK21LogicGameObjectServer4getYEv(*&a1->gap140[28]);
        //                    v117 = GetY();
        //                    v118 = v113;
        //                    v44 = v285;
        //                    sub_33E380(v115, v116 - v117, v118, v308);
        //                    v119 = GetX();
        //                    *&a1->gap140[32] = v308[0] + v119;
        //                    v120 = GetY();
        //                    v102 = v308[1] + v120;
        //                }
        //                else
        //                {
        //                    v100 = ZNK21LogicGameObjectServer4getXEv(*&a1->gap140[28]);
        //                    v101 = *&a1->gap140[28];
        //                    *&a1->gap140[32] = v100;
        //                    v102 = ZNK21LogicGameObjectServer4getYEv(v101);
        //                }
        //                *&a1->gap140[36] = v102;
        //                v82 = 0;
        //            }
        //            if (!ZNK18LogicCharacterData6isHeroEv(v2) || a1->gap124[1])
        //            {
        //                v79 = 0;
        //                if (!a1->gap124[2]
        //                  && !ZNK20LogicCharacterServer22isPlayerControlRemovedEv(a1)
        //                  && !sub_3192D4(v2)
        //                  && *(v2 + 260) != 2)
        //                {
        //                    v121 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
        //                    v79 = 1;
        //                    v122 = ZNK21LogicBattleModeServer12getTicksGoneEv(v121) - 1;
        //                    a1->MoveStartTime = v122;
        //                    a1->MoveEndTime = v122;
        //                }
        //                goto LABEL_109;
        //            }
        //        LABEL_108:
        //            v79 = 0;
        //            goto LABEL_109;
        //        }
        //    }
        //    v78 = 0;
        //    v79 = 0;
        //    v80 = v303;
        //    a1->gap140[40] = 0;
        //    v295 = 0;
        //    *&a1->gap140[28] = 0;
        LABEL_118:
            //if (TicksGone > MoveEnd)
            //    goto LABEL_122;
            //v127 = v79 == 0;
            //if (!v79)
            //    v127 = a1->unsigned___int8F9 == 0;
            if ((!v127 || (TicksGone > MoveEnd)) && !IsSteerMovementActive())
            {
                Charging = IsCharging;
                if (IsCharging && (m_activeChargeType == 2 || m_activeChargeType == 11))
                {
                    AreaEffect areaEffect = new AreaEffect(ChargeAreaEffectData);
                    areaEffect.SetPosition(GetX(), GetY(), 0);
                    areaEffect.SetIndex(GetIndex());
                    areaEffect.SetSource(this, 2);
                    areaEffect.Damage = ChargeDamage;
                    areaEffect.NormalDMG = ChargeNormalDMG;
                    //v14 = GetCardValueForPassive(v1, 21, 1);
                    //if (v14 >= 0)
                    //    v11[25] = v14;
                    v15 = GetCardValueForPassive("prey_on_the_weak", 1);
                    if (v15 >= 0)
                    {
                        areaEffect.SetPreyOnTheWeak(v15, GetCardValueForPassive("prey_on_the_weak", 0));
                    }
                    GameObjectManager.AddGameObject(areaEffect);
                    areaEffect.Trigger();
                    if (GetCardValueForPassive("charge_end_damage", 1) >= 0)
                    {
                        AreaEffect areaEffect1 = new AreaEffect(DataTables.GetAreaEffectByName("EnragerStarPowerDamage"));
                        areaEffect1.SetPosition(GetX(), GetY(), 0);
                        areaEffect1.SetIndex(GetIndex());
                        areaEffect1.SetSource(this, 2);
                        areaEffect1.NormalDMG = 0;
                        GameObjectManager.AddGameObject(areaEffect1);
                        areaEffect1.Trigger();
                    }
                }
                //v136 = v304;
                IsCharging = false;
                Knockback3 = false;
                ChargeAnimation = 0;
                Knockback2 = false;
                KnockBacked = false;
                ViusalChargeType = 0;
                if (Charging)
                {
                    if (ShouldExecuteColetteChargeOnReturn)
                    {
                        TriggerCharge(WayPoint2.X, WayPoint2.Y, ChargeSpeed, m_activeChargeType, ChargeDamage, ChargePushback, false);
                    }
                    v137 = GetCardValueForPassive("speed_after_ulti", 1);
                    if (v137 >= 0)
                    {
                        v79 = v137;
                        v138 = GetCardValueForPassive("speed_after_ulti", 0);
                        GiveSpeedFasterBuff(v79, v138, true);
                    }
                    if (GetCardValueForPassive("barrel_defense", 1) >= 0)
                        AddShield(GetCardValueForPassive("barrel_defense", 1), GetCardValueForPassive("barrel_defense", 0));
                }
                //if (!v78 || *(ZNK21LogicGameObjectServer24getLogicBattleModeServerEv() + 582))
                if (!CharacterData.HasAutoAttack() || AutoAttackTarget == null)
                {
                    //if (!ZNK18LogicCharacterData6isHeroEv(v136) && sub_1B7044(v136) >= 1 && !ZNK18LogicCharacterData6isBossEv(v136))
                    //    sub_265CE0(a1);
                    State = active || HasActiveSkill("ProjectileShield") ? 2 : 0;
                    //if (!sub_1B7044(v136) && (sub_28A580(v136) || ZNK18LogicCharacterData13getAreaEffectEv(v136)))
                    //{
                    //    if (sub_92F24(v136) < 1)
                    //    {
                    //        AttackAngle = a1->AttackAngle;
                    //        a1->AttackAngle = AttackAngle + 13;
                    //        if (AttackAngle >= 347)
                    //            a1->AttackAngle = AttackAngle - 347;
                    //    }
                    //    else
                    //    {
                    //        v203 = a1->field_74;
                    //        a1->field_74 = v203 + 50;
                    //        if (v203 >= 651)
                    //        {
                    //            if (!sub_1163C0(v80, 2))
                    //                a1->AttackAngle = ZN21LogicBattleModeServer12getRandomIntEii(v80, 0, 360);
                    //            a1->field_74 = 0;
                    //        }
                    //    }
                    //}
                    goto LABEL_241;
                }
                State = 2;
                //if (*(v136 + 260) == 2)
                //{
                //    a1->State = 3;
                //    sub_265CE0(a1);
                //}
                //    v204 = sub_92F24(v136);
                //    v205 = 0;
                //    BuffsCount = a1->BuffsCount;
                //    v207 = *&a1->gap1E4[4] + v204;
                //    v208 = 0;
                //    if (BuffsCount >= 1)
                //    {
                //        Buffs = a1->Buffs;
                //        v208 = 0;
                //        do
                //        {
                //            v79 = **Buffs;
                //            if (v79 <= 5 && ((1 << v79) & 38) != 0)
                //                v208 += (*Buffs)[3];
                //            ++Buffs;
                //            --BuffsCount;
                //        }
                //        while (BuffsCount);
                //    }
                //    v210 = (1374389535LL * (v208 + *&a1->gap124[4]) * v207) >> 32;
                //    v211 = (v210 >> 5) + (v210 >> 31);
                //    if (a1->gap328[4])
                //        v205 = sub_92F24(v304);
                //    v212 = v211 + v207;
                //    v213 = sub_3B5188(v304);
                //    v293 = v205;
                //    v288 = v212;
                //    if (sub_2402F4(a1, 47, 1) < 0)
                //    {
                //        v222 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
                //        v223 = v222 == 0;
                //        v300 = v213;
                //        if (v222)
                //        {
                //            v79 = v222;
                //            v223 = *(v222 + 20) == 0;
                //        }
                //        if (v223)
                //            goto LABEL_236;
                //        if (ZNK18LogicCharacterData6isHeroEv(v304))
                //            goto LABEL_236;
                //        v268 = 152;
                //        if (!*(v304 + 291))
                //            v268 = 148;
                //        v269 = *(*(v79 + 20) + v268);
                //        if (!v269 || !sub_102E68(*(v269 + 144)))
                //            goto LABEL_236;
                //        v220 = sub_102E68(*(v269 + 144));
                //    }
                //    else
                //    {
                //        v214 = v305;
                //        sub_4C793C(v305);
                //        v215 = sub_34860C(v305, 0);
                //        sub_54FE64(v305);
                //        v216 = ZNK21LogicGameObjectServer9getPlayerEv(a1);
                //        v217 = v216 == 0;
                //        v300 = v215;
                //        if (v216)
                //        {
                //            v214 = v216;
                //            v217 = *(v216 + 20) == 0;
                //        }
                //        if (v217 || ZNK18LogicCharacterData6isHeroEv(v304))
                //            goto LABEL_236;
                //        v218 = 152;
                //        if (!*(v304 + 291))
                //            v218 = 148;
                //        v219 = *(v214[5] + v218);
                //        if (!v219 || !sub_2A432C(*(v219 + 144)))
                //            goto LABEL_236;
                //        v220 = sub_2A432C(*(v219 + 144));
                //    }
                //    v300 = v220;
                //LABEL_236:
                //    v224 = *&a1->gap140[28];
                //    v225 = *&a1->gap140[32];
                //    v226 = *&a1->gap140[36];
                //    v227 = sub_58C4D0(v304);
                //    v228 = *&a1->gap1EE[14];
                //    v229 = sub_1EAA94(v304);
                //    v230 = sub_4ACA10(v304);
                //    v271 = v302 < v228;
                //    v231 = v224;
                //    TicksGone = v302;
                //    v136 = v304;
                //    tickstarpower(a1, v231, v225, v226, v288, v293, v300, v227, v271, v229, v230);
                Attack(AutoAttackTarget, AutoAttackTarget.GetX(), AutoAttackTarget.GetY(), CharacterData.AutoAttackRange, DataTables.GetProjectileByName(CharacterData.AutoAttackProjectile), CharacterData.AutoAttackDamage + AutoAttackDamangeBoost, CharacterData.AutoAttackDamage, CharacterData.AutoAttackProjectileSpread, CharacterData.AutoAttackBulletsPerShot);
                goto LABEL_241;
            }
            //    ZNK20LogicCharacterServer16getMovementSpeedEii(a1);
            //    X = GetX();
            //    v142 = GetY();
            //p_NextX = &a1->NextX;
            //Y = v142;
            if (IsSteerMovementActive()) v176 = GetNextSteeredPos(PosDelta);
            else v176 = GetPosAtTick(0, TicksGone, PosDelta, 0);
            LogicVector2 PrePos = Position.Clone();
            SetPosition(PosDelta.X, PosDelta.Y, v176);
            //v136 = v304;
            if (false)
            //if (v78 && *(v304 + 260) == 2)
            {
                //v282 = Y;
                //a1->State = 3;
                //if (v295)
                //{
                //    sub_265CE0(a1);
                //    a1->gap140[40] = 0;
                //}
                //v147 = sub_92F24(v304);
                //v148 = 0;
                //v149 = a1->BuffsCount;
                //v296 = 0;
                //if (v149 >= 1)
                //{
                //    v150 = a1->Buffs;
                //    v148 = 0;
                //    do
                //    {
                //        v151 = **v150;
                //        if (v151 <= 5 && ((1 << v151) & 38) != 0)
                //            v148 += *(*v150 + 12);
                //        v150 += 4;
                //        --v149;
                //    }
                //    while (v149);
                //}
                //v286 = X;
                //v152 = (v148 + *&a1->gap124[4]) * (*&a1->gap1E4[4] + v147) / 100 + *&a1->gap1E4[4] + v147;
                //if (a1->gap328[4])
                //    v296 = sub_92F24(v304);
                //v279 = *&a1->gap140[28];
                //v276 = *&a1->gap140[32];
                //v273 = *&a1->gap140[36];
                //v153 = sub_3B5188(v304);
                //v154 = sub_58C4D0(v304);
                //v155 = *&a1->gap1EE[14];
                //v156 = sub_1EAA94(v304);
                //v157 = sub_4ACA10(v304);
                //v158 = 0;
                //v270 = v152;
                //if (v302 < v155)
                //    v158 = &dword_0 + 1;
                //v136 = v304;
                //tickstarpower(a1, v279, v276, v273, v270, v296, v153, v154, v158, v156, v157);
                //X = v286;
                //Y = v282;
            }
            else
            {
                v159 = 1;
                if (v281)
                    v159 = 3;
                State = v159;
                //v160 = sub_5388E0(a1);
                //if (v160 && !sub_68096C(v160, v161, v162, v163))
                //    a1->gap140[40] = 0;
            }
        //v164 = *p_NextX == X;
        //if (*p_NextX == X)
        //    v164 = a1->NextY == Y;
        LABEL_262:
            //v221 = v333;
            //v222 = **(a1 + 1092);
            if (Position != PrePos)
            //if (PathLength > 200)
            {
                //if(m_isBot==0) Debugger.Print("DIS:" + (MoveEnd-MoveStart));
                //v223 = *(a1 + 1048);
                //v224 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
                //if (v223 + 1 < ZNK21LogicBattleModeServer7getTickEv(v224))
                if (ForcedAngleTicks + 1 < TicksGone && !IsSteerMovementActive()) UpdateMoveDirection(0, WayPoint, PosDelta, WayPoint.X, WayPoint.Y);
                //if(m_isBot==0)Debugger.Print("Angle:" + MoveAngle);
                if (State == 1) AttackAngle = MoveAngle;
                v167 = GetCardValueForPassive("running_charges_ulti", 0);
                if (v167 >= 0)
                {
                    if (GetPlayer() != null && !GetPlayer().HasUlti())
                    {
                        if (!IsPlayerControlRemoved())
                        {
                            v262 = GetCardValueForPassive("running_charges_ulti", 1);
                            v264 = RunningChargingUlti + Position.GetDistance(PrePos);
                            v265 = v264 / v262;
                            v266 = v264 - v265 * v262;
                            RunningChargingUlti = v266;
                            if (v265 >= 1)
                                ChargeUlti(v265 * v167, false, true, GetPlayer(), this);
                        }
                    }
                    else
                    {
                        RunningChargingUlti = 0;
                    }
                }
                v171 = GetCardValueForPassive("running_charges_weapon", 1);
                if (v171 >= 0)
                {
                    v172 = v171;
                    if (GetWeaponSkill().HasFullCharge(this))
                    {
                        RunningChargingUlti = 0;
                    }
                    else if (!IsPlayerControlRemoved())
                    {
                        v232 = RunningChargingUlti + Position.GetDistance(PrePos);
                        v233 = v232 / v172;
                        RunningChargingUlti = v232 - v233 * v172;
                        if (v233 >= 1)
                        {
                            v234 = GetCardValueForPassive("running_charges_weapon", 0);
                            GetWeaponSkill().AddCharge(this, v234 * v233);
                        }
                    }
                }
            }
        //else State = 0;
        //TicksGone = v302;
        //a1->field_70 = v302;
        LABEL_241:
            //v235 = *(v136 + 260);
            if (CharacterData.Type == "Minion_FindEnemies" && TicksGone > m_ticksSinceBotEnemyCheck + 10)
            {
                //v236 = 24;
                //v237 = 1;
                //if (*(v136 + 260) != 8)
                //{
                //    v237 = 1;
                //    v236 = 2 * *(sub_5F01A8(v303) + 116);
                //    if (*(v136 + 260) == 12)
                //        v237 = 3;
                //}
                //v238 = sub_533B98(v136);
                v239 = GetClosestEnemy(9999, 1, true, true, null, false, false, false);
                if (v239 == null) v239 = (Character)GameObjectManager.GetGameObjectByID(ParentGID);
                //v240 = v136;
                //if (v239)
                //    goto LABEL_249;
                //if (!sub_533B98(v136))
                //    goto LABEL_259;
                //v244 = sub_533B98(v136);
                //v245 = 0;
                //v246 = sub_3F2F04(a1, v236, v237, 1, 1, 0, 0, v244, 1);
                //if (!v246)
                //    goto LABEL_259;
                //v239 = v246;
                //v247 = sub_5C9830(v240) + 2;
                //if (sub_3B5188(v240))
                //{
                //    v248 = sub_3B5188(v240);
                //    v245 = *(v248 + 88);
                //    if (*(v248 + 88))
                //        v245 = 1;
                //}
                //v249 = *(v240 + 260);
                //v250 = sub_533B98(v240);
                //if (!sub_104420(a1, v247, v239, 0, v249 == 6, v245, v250))
                if (v239 != null)
                {
                LABEL_249:
                    v241 = v239.GetX();
                    v242 = v239.GetY();
                    //if (*(v240 + 260) == 12)
                    //{
                    //    v243 = v303;
                    //}
                    //else
                    //{
                    //    v243 = v303;
                    //    v241 += ZN21LogicBattleModeServer12getRandomIntEii(v303, -300, 300);
                    //    v242 += ZN21LogicBattleModeServer12getRandomIntEii(v303, -300, 300);
                    //}
                    //v253 = sub_5F01A8(v243);
                    //v254 = LogicMath.Clamp(v241, 1, *(v253 + 104) - 2);
                    //v255 = sub_5F01A8(v243);
                    //v256 = LogicMath.Clamp(v242, 1, *(v255 + 108) - 2);
                    //sub_13FFF0(a1, v254, v256, 0, 0);
                    //v136 = v304;
                    MoveTo(0, v241, v242, 0, 0, 0, 0);
                }
                //else
                //{
                //LABEL_259:
                //    v136 = v240;
                //    if (sub_533B98(v240) && !ZNK20LogicCharacterServer22isPlayerControlRemovedEv(a1))
                //    {
                //        v251 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv();
                //        v252 = ZNK21LogicBattleModeServer12getTicksGoneEv(v251) - 1;
                //        a1->MoveStartTime = v252;
                //        a1->MoveEndTime = v252;
                //    }
                //}
                //if (a1->TeamIndex == -1)
                //{
                //    sub_2B949C(a1);
                //    sub_1EBA94(a1);
                //}
                m_ticksSinceBotEnemyCheck = TicksGone;
            }
            //sub_6562A0(a1);
            //if (ZNK18LogicCharacterData6isBossEv(v136))
            //{
            //    if (sub_1222F4(v136))
            //    {
            //        sub_6604A0(a1);
            //    }
            //    else
            //    {
            //        sub_3F9640(a1);
            //        sub_1EBA94(a1);
            //    }
            //}
            //if (*&a1->gap1B8[8] < 1)
            //    JUMPOUT(0x401690);
            //if (TicksGone != *&a1->gap1B8[4])
            //    JUMPOUT(0x401640);
            //sub_92F24(v136);
            //v257 = a1->BuffsCount;
            //if (v257 >= 1)
            //{
            //    v258 = a1->Buffs;
            //    v259 = 0;
            //    do
            //    {
            //        v260 = **v258;
            //        if (v260 <= 5 && ((1 << v260) & 38) != 0)
            //            v259 += (*v258)[3];
            //        ++v258;
            //        --v257;
            //    }
            //    while (v257);
            //}
            //sub_3B5188(v304);
            if (DisplayUltiTmp)
            {
                if (!IsCharging) DisplayUltiTmp = false;
            }
            return;
        }



        private bool CheckObstacle(int nextTiles)
        {
            int movingSpeed = CharacterData.Speed / 20;
            movingSpeed *= (100 - slowingModifier) / 100;
            movingSpeed *= (100 + AcceleratingModifier) / 100;
            int deltaX;
            int deltaY;

            int newX = this.Position.X;
            int newY = this.Position.Y;

            for (int i = 0; i < nextTiles; i++)
            {
                if (PosDelta.X - Position.X > 0) deltaX = LogicMath.Min(movingSpeed, PosDelta.X - Position.X);
                else deltaX = LogicMath.Max(-movingSpeed, PosDelta.X - Position.X);

                if (PosDelta.Y - Position.Y > 0) deltaY = LogicMath.Min(movingSpeed, PosDelta.Y - Position.Y);
                else deltaY = LogicMath.Max(-movingSpeed, PosDelta.Y - Position.Y);

                newX += deltaX;
                newY += deltaY;

                if (!GameObjectManager.GetBattle().IsInPlayArea(newX, newY)) return true;

                Tile nextTile = GameObjectManager.GetBattle().GetTileMap().GetTile(newX, newY);
                if (nextTile == null) return true;
                if (nextTile.Data.BlocksMovement && !nextTile.IsDestructed()) return true;
            }

            return false;
        }

        public void UltiEnabled()
        {
            m_usingUltiCurrently = true;
        }

        public void UltiDisabled()
        {
            m_usingUltiCurrently = false;
        }

        public override bool IsAlive()
        {
            return m_hitpoints > 0;
        }

        public override int GetRadius()
        {
            return CharacterData.CollisionRadius;
        }

        public void SetHeroLevel(int level)
        {
            m_heroLevel = level;
            m_maxHitpoints = CharacterData.Hitpoints + ((int)(((float)5 / 100) * (float)CharacterData.Hitpoints)) * (level - 1);
            m_hitpoints = m_maxHitpoints;
            OrginalMaxHP = m_maxHitpoints;
            m_damageMultiplier = (level - 1) * 5;
        }

        public int GetHeroLevel()
        {
            return m_heroLevel;
        }

        public int GetNormalWeaponDamage()
        {
            return m_skills[0].SkillData.Damage + ((int)(((float)5 / 100) * (float)m_skills[0].SkillData.Damage)) * (m_heroLevel + m_damageMultiplier);
        }

        public int GetAbsoluteDamage(int damage)
        {
            return damage + ((int)(((float)5 / 100) * (float)damage)) * (m_heroLevel + m_damageMultiplier) + damage * (DamageModifier) / 100;
        }
        public int PathfindTo(
        int a1,
        int x,
        int y,
        int a4,
        int a5,
        int a6,
        int a7,
        int a8,
        int a9)
        {
            int v9; // r6
            int v10; // r2
            int v11; // r0
            int v12; // r7
            int v13; // r3
            int v14; // r1
            int v15; // r5
            int v16; // r3
            int v17; // r4
            int v18; // r2
            int v19; // r6
            int v20; // d16
            int v21; // d17
            int v22; // r2
            int v23; // r5
            int v24; // r4
            int v25; // r0
            int v26; // r4
            int v27; // r0
            int v28; // r1
            int v29; // r0
            int v30; // r5
            int valid; // r0
            int v32; // r4
            int v33; // r7
            int v34; // r0
            int v35; // r1
            int v36; // r0
            int v37; // r0
            int v38; // r6
            int v39; // r0
            int v40; // r0
            int v41; // r4
            int v42; // r0
            int v43; // r0
            int v44; // r1
            int v45; // r0
            int v46; // r9
            int v47; // r1
            int v48; // r10
            int v49; // r5
            int v50; // r9
            int v51; // r0
            int v52; // r8
            int v53; // r0
            int v54; // r1
            int v55; // r2
            int v56; // r3
            int v57; // t1
            int v58; // r0
            int v59; // r5
            int v60; // r9
            int v61; // r0
            int v62; // r8
            int v63; // r0
            int v64; // r1
            int v65; // r2
            int v66; // r3
            int v67; // t1
            int v68; // r0
            int v69; // r5
            int v70; // r9
            int v71; // r0
            int v72; // r8
            int v73; // r0
            int v74; // r1
            int v75; // r2
            int v76; // r3
            int v77; // t1
            int v78; // r0
            int v79; // r5
            int v80; // r9
            int v81; // r0
            int v82; // r8
            int v83; // r0
            int v84; // r1
            int v85; // r2
            int v86; // r3
            int v87; // t1
            int v88; // r0
            int v89; // r0
            int v90; // r8
            int v91; // r7
            int v92; // r0
            int v93; // r1
            int v94; // r2
            int v95; // r3
            int v96; // t1
            int v97; // r0
            int v98; // r8
            int v99; // r7
            int v100; // r0
            int v101; // r1
            int v102; // r2
            int v103; // r3
            int v104; // t1
            int v105; // r5
            int v106; // r7
            int v107; // r0
            int v108; // r5
            int v109; // r4
            int v110; // r7
            bool v111; // zf
            bool v112; // zf
            int v113; // r0
            int v114; // r4
            int v115; // r0
            int v116; // r0
            int v117; // r4
            int v118; // r6
            int v119; // r9
            int v120; // r7
            int v121; // r5
            int v122; // r0
            int v123; // r1
            int v124; // r7
            int v125; // r4
            int v126; // r9
            int v127; // r5
            int v128; // r8
            int v129; // r10
            int v130; // r0
            int v131; // r4
            int v132; // r0
            int v133; // r1
            int v134; // r2
            int v135; // r3
            int v136; // t1
            int v137; // r0
            int v138; // r9
            int v139; // r10
            int v140; // r0
            int v141; // r8
            int v142; // r0
            int v143; // r1
            int v144; // r2
            int v145; // r3
            int v146; // t1
            int v147; // r5
            int v148; // r9
            int v149; // r0
            int v150; // r8
            int v151; // r0
            int v152; // r1
            int v153; // r2
            int v154; // r3
            int v155; // t1
            int v156; // r0
            int v157; // r5
            int v158; // r9
            int v159; // r0
            int v160; // r8
            int v161; // r0
            int v162; // r1
            int v163; // r2
            int v164; // r3
            int v165; // t1
            int v166; // r0
            int v167; // r0
            int v168; // r4
            int v169; // r0
            int v170; // r8
            int v171; // r4
            int v172; // r6
            int v173; // r7
            int v174; // r5
            int v175; // r0
            int v176; // r8
            int v177; // r7
            int v178; // r10
            int v179; // r0
            int v180; // r9
            int v181; // r0
            int v182; // r1
            int v183; // r2
            int v184; // r3
            int v185; // t1
            int v186; // r0
            int v187; // r7
            int v188; // r10
            int v189; // r0
            int v190; // r9
            int v191; // r0
            int v192; // r1
            int v193; // r2
            int v194; // r3
            int v195; // t1
            int v196; // r0
            int v197; // r6
            int v198; // lr
            int v199; // r7
            int v200; // r4
            int v201; // d16
            int v202; // d17
            int v203; // r6
            int v204; // r4
            int v205; // r5
            int v206; // d16
            int v207; // d17
            int v208; // r7
            int v209; // r6
            int v210; // r4
            int v211; // d16
            int v212; // d17
            int v213; // r7
            int v214; // r6
            int v215; // r4
            int v216; // d16
            int v217; // d17
            int v218; // r4
            int v219; // r6
            int v220; // r5
            int v221; // d16
            int v222; // d17
            int v223; // r4
            int v224; // r6
            int v225; // r5
            int v226; // d16
            int v227; // d17
            int v228; // r5
            int v229; // r9
            int v230; // r0
            int v231; // r8
            int v232; // r0
            int v233; // r1
            int v234; // r2
            int v235; // r3
            int v236; // t1
            int v237; // r0
            int v238; // r5
            int v239; // r9
            int v240; // r0
            int v241; // r8
            int v242; // r0
            int v243; // r1
            int v244; // r2
            int v245; // r3
            int v246; // t1
            int v247; // r0
            int v249; // r4
            int v250; // r6
            int v251; // r5
            int v252; // d16
            int v253; // d17
            int v254; // r4
            int v255; // r6
            int v256; // r5
            int v257; // d16
            int v258; // d17
            int v259; // r7
            int v260; // r6
            int v261; // r4
            int v262; // d16
            int v263; // d17
            int v264; // r7
            int v265; // r6
            int v266; // r4
            int v267; // d16
            int v268; // d17
            int v269; // r7
            int v270; // r6
            int v271; // r4
            int v272; // d16
            int v273; // d17
            int v274; // r7
            int v275; // r6
            int v276; // r4
            int v277; // d16
            int v278; // d17
            int v279; // r7
            int v280; // r6
            int v281; // r4
            int v282; // d16
            int v283; // d17
            int v284; // r7
            int v285; // r6
            int v286; // r4
            int v287; // d16
            int v288; // d17
            char v289; // [sp+18h] [bp-60h]
            int v292; // [sp+24h] [bp-54h]
            int v293; // [sp+28h] [bp-50h]
            int v295; // [sp+30h] [bp-48h]
            int v296; // [sp+34h] [bp-44h]
            int v298; // [sp+3Ch] [bp-3Ch]
            int v299; // [sp+40h] [bp-38h]
            int v300; // [sp+44h] [bp-34h]
            int v301; // [sp+48h] [bp-30h]
            int v302; // [sp+48h] [bp-30h]
            int v303; // [sp+4Ch] [bp-2Ch]
            int v304; // [sp+58h] [bp-20h]
                      //TileMap tileMap = GameObjectManager.GetBattle().GetTileMap();
                      //var Tiles = new short[tileMap.LogicHeight/300, tileMap.LogicWidth/300];
                      //for (int i = 0; i < tileMap.LogicHeight/300; i++)
                      //{
                      //    for (int j = 0; j < tileMap.LogicWidth/300; j++)
                      //    {
                      //        Tiles[i, j] = (short)(tileMap.GetTiles()[i, j].Data.BlocksMovement ? 1 : 0);
                      //    }
                      //}
                      ////Debugger.Print(Tiles.ToString());
                      //PathFinder pathFinder = new PathFinder(new WorldGrid(Tiles));
                      //var start = new Position(GetY() / 300, GetX() / 300);
                      //var end = new Position(y / 300, x / 300);
                      //var Points= pathFinder.FindPath(start, end);
                      //if (Points.Any())
                      //{
                      //    x = Points[0].Row * 300;
                      //    y = Points[0].Column * 300;
                      //if (WayPoint.X != PosDelta.X) return 0;
            WayPoint.X = x;
            WayPoint.Y = y;
            WayPointStart.X = GetX();
            WayPointStart.Y = GetY();
            //}
            return 0;
            //v9 = a1;
            //v10 = a1[32];
            //if (v10 >= 1)
            //{
            //    v11 = a1[33];
            //    do
            //    {
            //        while (1)
            //        {
            //            while (1)
            //            {
            //                v12 = v9[35];
            //                v13 = v10 - 1;
            //                v9[32] = v10 - 1;
            //                v9[35] = v12 - 1;
            //                if (v12 > v10)
            //                    break;
            //                --v10;
            //                if (v13 <= 0)
            //                    goto LABEL_15;
            //            }
            //            v14 = v12 - v10;
            //            if (v12 - v10 >= 1)
            //                break;
            //            --v10;
            //            if (v13 <= 0)
            //                goto LABEL_15;
            //        }
            //        v15 = v12 - v10;
            //        v16 = v11 + 4 * v13;
            //        if ((v12 - v10) < 4)
            //            goto LABEL_10;
            //        v17 = (v11 + 4 * v10);
            //        v14 -= v15 & 0xFFFFFFFC;
            //        v16 += 4 * (v15 & 0xFFFFFFFC);
            //        v18 = v15 & 0xFFFFFFFC;
            //        do
            //        {
            //            v19 = (v17 - 4);
            //            v20 = *v17;
            //            v21 = v17[1];
            //            v17 += 2;
            //            v18 -= 4;
            //            *v19 = v20;
            //            v19[1] = v21;
            //        }
            //        while (v18);
            //        v9 = a1;
            //        if (v15 != (v15 & 0xFFFFFFFC))
            //        {
            //        LABEL_10:
            //            v22 = (v16 + 4);
            //            do
            //            {
            //                --v14;
            //                *(v22 - 1) = *v22;
            //                ++v22;
            //            }
            //            while (v14 > 0);
            //        }
            //        v10 = v9[32];
            //    }
            //    while (v10 > 0);
            //}
            //  LABEL_15:
            //      v23 = ZNK21LogicGameObjectServer4getXEv(v9);
            //      v24 = ZNK21LogicGameObjectServer4getYEv(v9);
            //      v292 = v23;
            //      v9[21] = ZN12LogicTileMap21logicToPathFinderTileEi(v23);
            //      v296 = v9 + 21;
            //      v293 = v24;
            //      v9[22] = ZN12LogicTileMap21logicToPathFinderTileEi(v24);
            //      v9[23] = ZN12LogicTileMap21logicToPathFinderTileEi(x);
            //      v295 = v9 + 23;
            //      v9[24] = ZN12LogicTileMap21logicToPathFinderTileEi(y);
            //      v25 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //      v26 = ZNK18LogicCharacterData11isCarryableEv(v25);
            //      v27 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //      if (v26)
            //      {
            //          v28 = 2 * (ZNK18LogicCharacterData18getCollisionRadiusEv(v27) > 199);
            //      }
            //      else
            //      {
            //          v29 = ZNK18LogicCharacterData6isBossEv(v27);
            //          v28 = 1;
            //          if (v29)
            //              v28 = 2;
            //      }
            //      v30 = v9 + 15;
            //      ZN15LogicPathFinder8findPathEiPK12LogicVector2S2_bPS0_bbbb(a5, v28, v296, v295, 1, v9 + 15, 0, a6, a7, a9);
            //      if (a8 && !ZNK15LogicPathFinder13getPathLengthEv(a5) && (*v296 != *v295 || v9[22] != v9[24]))
            //      {
            //          valid = ZNK15LogicPathFinder30getClosestValidTargetTileIndexEv(a5);
            //          if (valid >= 0)
            //          {
            //              v32 = valid;
            //              v33 = v9;
            //              v34 = valid / *(*(a5 + 20) + 120);
            //              v35 = v32 - v34 * *(*(a5 + 20) + 120);
            //              v9[24] = v34;
            //              v9[23] = v35;
            //              v36 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(v9);
            //              ZN21LogicBattleModeServer10getTileMapEv(v36);
            //              v37 = v9[23];
            //              v38 = 1;
            //              x = ZN12LogicTileMap21pathFinderTileToLogicEib(v37, 1);
            //              v39 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(v33);
            //              ZN21LogicBattleModeServer10getTileMapEv(v39);
            //              y = ZN12LogicTileMap21pathFinderTileToLogicEib(*(v33 + 96), 1);
            //              v40 = ZNK21LogicGameObjectServer7getDataEv(v33);
            //              v41 = ZNK18LogicCharacterData11isCarryableEv(v40);
            //              v42 = ZNK21LogicGameObjectServer7getDataEv(v33);
            //              if (v41)
            //              {
            //                  v38 = 2 * (ZNK18LogicCharacterData18getCollisionRadiusEv(v42) > 199);
            //              }
            //              else if (ZNK18LogicCharacterData6isBossEv(v42))
            //              {
            //                  v38 = 2;
            //              }
            //              ZN15LogicPathFinder8findPathEiPK12LogicVector2S2_bPS0_bbbb(a5, v38, v296, v295, 1, v30, 0, a6, a7, a9);
            //              v9 = a1;
            //          }
            //      }
            //      v43 = ZN12LogicTileMap21pathFinderTileToLogicEib(v9[15], 0);
            //      v44 = v9[16];
            //      *a4 = v43;
            //      v45 = ZN12LogicTileMap21pathFinderTileToLogicEib(v44, 0);
            //      v46 = a5;
            //      v47 = *a4;
            //      a4[1] = v45 + 50;
            //      *a4 = v47 + 50;
            //      v48 = ZNK15LogicPathFinder13getPathLengthEv(a5);
            //      if (!v48 && *v296 == *v295 && v9[22] == v9[24])
            //      {
            //          v49 = v9[32];
            //          if (v49 != v9[31])
            //              goto LABEL_48;
            //          v50 = 2 * v49;
            //          if (!(2 * v49))
            //              v50 = 5;
            //          if (v49 >= v50)
            //          {
            //          LABEL_47:
            //              v46 = a5;
            //          LABEL_48:
            //              v58 = v9[30];
            //              v9[32] = v49 + 1;
            //              *(v58 + 4 * v49) = x;
            //              v59 = v9[35];
            //              if (v59 != v9[34])
            //                  goto LABEL_64;
            //              v60 = 2 * v59;
            //              if (!(2 * v59))
            //                  v60 = 5;
            //              if (v59 >= v60)
            //              {
            //              LABEL_63:
            //                  v46 = a5;
            //              LABEL_64:
            //                  v68 = v9[33];
            //                  v9[35] = v59 + 1;
            //                  *(v68 + 4 * v59) = y;
            //                  v69 = v9[32];
            //                  if (v69 != v9[31])
            //                      goto LABEL_80;
            //                  v70 = 2 * v69;
            //                  if (!(2 * v69))
            //                      v70 = 5;
            //                  if (v69 >= v70)
            //                  {
            //                  LABEL_79:
            //                      v46 = a5;
            //                  LABEL_80:
            //                      v78 = v9[30];
            //                      v9[32] = v69 + 1;
            //                      *(v78 + 4 * v69) = v292;
            //                      v79 = v9[35];
            //                      if (v79 != v9[34])
            //                      {
            //                      LABEL_96:
            //                          v88 = v9[33];
            //                          v9[35] = v79 + 1;
            //                          *(v88 + 4 * v79) = v293;
            //                          *a4 = x;
            //                          a4[1] = y;
            //                          return ZN20LogicCharacterServer12ensurePathOkEP15LogicPathFinder(v9, v46);
            //                      }
            //                      v80 = 2 * v79;
            //                      if (!(2 * v79))
            //                          v80 = 5;
            //                      if (v79 >= v80)
            //                      {
            //                      LABEL_95:
            //                          v46 = a5;
            //                          goto LABEL_96;
            //                      }
            //                      v81 = 4 * v80;
            //                      if (v80 != (v80 & 0x3FFFFFFF))
            //                          v81 = -1;
            //                      v82 = operator new[](v81, v80 != (v80 & 0x3FFFFFFF));
            //                      v83 = v9[33];
            //                      if (v79 >= 1)
            //                      {
            //                          if (v79 >= 4 && (v82 >= v83 + 4 * v79 || v82 + 4 * v79 <= v83))
            //                          {
            //                              v284 = v9[33];
            //                              v84 = v79 - (v79 & 0xFFFFFFFC);
            //                              v285 = v82;
            //                              v86 = (v82 + 4 * (v79 & 0xFFFFFFFC));
            //                              v85 = (v83 + 4 * (v79 & 0xFFFFFFFC));
            //                              v286 = v79 & 0xFFFFFFFC;
            //                              do
            //                              {
            //                                  v287 = *v284;
            //                                  v288 = v284[1];
            //                                  v284 += 2;
            //                                  v286 -= 4;
            //                                  *v285 = v287;
            //                                  v285[1] = v288;
            //                                  v285 += 2;
            //                              }
            //                              while (v286);
            //                              v9 = a1;
            //                              if (v79 == (v79 & 0xFFFFFFFC))
            //                                  goto LABEL_92;
            //                          }
            //                          else
            //                          {
            //                              v84 = v79;
            //                              v85 = v9[33];
            //                              v86 = v82;
            //                          }
            //                          do
            //                          {
            //                              v87 = *v85++;
            //                              --v84;
            //                              *v86++ = v87;
            //                          }
            //                          while (v84 > 0);
            //                      }
            //                  LABEL_92:
            //                      if (v83)
            //      operator delete[](v83);
            //                      v79 = v9[35];
            //                      v9[33] = v82;
            //                      v9[34] = v80;
            //                      goto LABEL_95;
            //                  }
            //                  v71 = 4 * v70;
            //                  if (v70 != (v70 & 0x3FFFFFFF))
            //                      v71 = -1;
            //                  v72 = operator new[](v71, v70 != (v70 & 0x3FFFFFFF));
            //                  v73 = v9[30];
            //                  if (v69 >= 1)
            //                  {
            //                      if (v69 >= 4 && (v72 >= v73 + 4 * v69 || v72 + 4 * v69 <= v73))
            //                      {
            //                          v279 = v9[30];
            //                          v74 = v69 - (v69 & 0xFFFFFFFC);
            //                          v280 = v72;
            //                          v76 = (v72 + 4 * (v69 & 0xFFFFFFFC));
            //                          v75 = (v73 + 4 * (v69 & 0xFFFFFFFC));
            //                          v281 = v69 & 0xFFFFFFFC;
            //                          do
            //                          {
            //                              v282 = *v279;
            //                              v283 = v279[1];
            //                              v279 += 2;
            //                              v281 -= 4;
            //                              *v280 = v282;
            //                              v280[1] = v283;
            //                              v280 += 2;
            //                          }
            //                          while (v281);
            //                          v9 = a1;
            //                          if (v69 == (v69 & 0xFFFFFFFC))
            //                              goto LABEL_76;
            //                      }
            //                      else
            //                      {
            //                          v74 = v69;
            //                          v75 = v9[30];
            //                          v76 = v72;
            //                      }
            //                      do
            //                      {
            //                          v77 = *v75++;
            //                          --v74;
            //                          *v76++ = v77;
            //                      }
            //                      while (v74 > 0);
            //                  }
            //              LABEL_76:
            //                  if (v73)
            //    operator delete[](v73);
            //                  v69 = v9[32];
            //                  v9[30] = v72;
            //                  v9[31] = v70;
            //                  goto LABEL_79;
            //              }
            //              v61 = 4 * v60;
            //              if (v60 != (v60 & 0x3FFFFFFF))
            //                  v61 = -1;
            //              v62 = operator new[](v61, v60 != (v60 & 0x3FFFFFFF));
            //              v63 = v9[33];
            //              if (v59 >= 1)
            //              {
            //                  if (v59 >= 4 && (v62 >= v63 + 4 * v59 || v62 + 4 * v59 <= v63))
            //                  {
            //                      v274 = v9[33];
            //                      v64 = v59 - (v59 & 0xFFFFFFFC);
            //                      v275 = v62;
            //                      v66 = (v62 + 4 * (v59 & 0xFFFFFFFC));
            //                      v65 = (v63 + 4 * (v59 & 0xFFFFFFFC));
            //                      v276 = v59 & 0xFFFFFFFC;
            //                      do
            //                      {
            //                          v277 = *v274;
            //                          v278 = v274[1];
            //                          v274 += 2;
            //                          v276 -= 4;
            //                          *v275 = v277;
            //                          v275[1] = v278;
            //                          v275 += 2;
            //                      }
            //                      while (v276);
            //                      v9 = a1;
            //                      if (v59 == (v59 & 0xFFFFFFFC))
            //                          goto LABEL_60;
            //                  }
            //                  else
            //                  {
            //                      v64 = v59;
            //                      v65 = v9[33];
            //                      v66 = v62;
            //                  }
            //                  do
            //                  {
            //                      v67 = *v65++;
            //                      --v64;
            //                      *v66++ = v67;
            //                  }
            //                  while (v64 > 0);
            //              }
            //          LABEL_60:
            //              if (v63)
            //  operator delete[](v63);
            //              v59 = v9[35];
            //              v9[33] = v62;
            //              v9[34] = v60;
            //              goto LABEL_63;
            //          }
            //          v51 = 4 * v50;
            //          if (v50 != (v50 & 0x3FFFFFFF))
            //              v51 = -1;
            //          v52 = operator new[](v51, v50 != (v50 & 0x3FFFFFFF));
            //          v53 = v9[30];
            //          if (v49 >= 1)
            //          {
            //              if (v49 >= 4 && (v52 >= v53 + 4 * v49 || v52 + 4 * v49 <= v53))
            //              {
            //                  v269 = v9[30];
            //                  v54 = v49 - (v49 & 0xFFFFFFFC);
            //                  v270 = v52;
            //                  v56 = (v52 + 4 * (v49 & 0xFFFFFFFC));
            //                  v55 = (v53 + 4 * (v49 & 0xFFFFFFFC));
            //                  v271 = v49 & 0xFFFFFFFC;
            //                  do
            //                  {
            //                      v272 = *v269;
            //                      v273 = v269[1];
            //                      v269 += 2;
            //                      v271 -= 4;
            //                      *v270 = v272;
            //                      v270[1] = v273;
            //                      v270 += 2;
            //                  }
            //                  while (v271);
            //                  v9 = a1;
            //                  if (v49 == (v49 & 0xFFFFFFFC))
            //                      goto LABEL_44;
            //              }
            //              else
            //              {
            //                  v54 = v49;
            //                  v55 = v9[30];
            //                  v56 = v52;
            //              }
            //              do
            //              {
            //                  v57 = *v55++;
            //                  --v54;
            //                  *v56++ = v57;
            //              }
            //              while (v54 > 0);
            //          }
            //      LABEL_44:
            //          if (v53)
            //operator delete[](v53);
            //          v49 = v9[32];
            //          v9[30] = v52;
            //          v9[31] = v50;
            //          goto LABEL_47;
            //      }
            //      if (v9[31] < v48)
            //      {
            //          v89 = 4 * v48;
            //          if (v48 != (v48 & 0x3FFFFFFF))
            //              v89 = -1;
            //          v90 = operator new[](v89, v48 != (v48 & 0x3FFFFFFF));
            //          v91 = v9[32];
            //          v92 = v9[30];
            //          if (v91 < 1)
            //              goto LABEL_106;
            //          if (v91 >= 4 && (v90 >= v92 + 4 * v91 || v90 + 4 * v91 <= v92))
            //          {
            //              v249 = v9[30];
            //              v93 = v91 - (v91 & 0xFFFFFFFC);
            //              v250 = v90;
            //              v95 = (v90 + 4 * (v91 & 0xFFFFFFFC));
            //              v94 = (v92 + 4 * (v91 & 0xFFFFFFFC));
            //              v251 = v91 & 0xFFFFFFFC;
            //              do
            //              {
            //                  v252 = *v249;
            //                  v253 = v249[1];
            //                  v249 += 2;
            //                  v251 -= 4;
            //                  *v250 = v252;
            //                  v250[1] = v253;
            //                  v250 += 2;
            //              }
            //              while (v251);
            //              v9 = a1;
            //              if (v91 == (v91 & 0xFFFFFFFC))
            //                  goto LABEL_106;
            //          }
            //          else
            //          {
            //              v93 = v9[32];
            //              v94 = v9[30];
            //              v95 = v90;
            //          }
            //          do
            //          {
            //              v96 = *v94++;
            //              --v93;
            //              *v95++ = v96;
            //          }
            //          while (v93 > 0);
            //      LABEL_106:
            //          if (v92)
            //operator delete[](v92);
            //          v9[30] = v90;
            //          v9[31] = v48;
            //      }
            //      if (v9[34] < v48)
            //      {
            //          v97 = 4 * v48;
            //          if (v48 != (v48 & 0x3FFFFFFF))
            //              v97 = -1;
            //          v98 = operator new[](v97, v48 != (v48 & 0x3FFFFFFF));
            //          v99 = v9[35];
            //          v100 = v9[33];
            //          if (v99 < 1)
            //              goto LABEL_118;
            //          if (v99 >= 4 && (v98 >= v100 + 4 * v99 || v98 + 4 * v99 <= v100))
            //          {
            //              v254 = v9[33];
            //              v101 = v99 - (v99 & 0xFFFFFFFC);
            //              v255 = v98;
            //              v103 = (v98 + 4 * (v99 & 0xFFFFFFFC));
            //              v102 = (v100 + 4 * (v99 & 0xFFFFFFFC));
            //              v256 = v99 & 0xFFFFFFFC;
            //              do
            //              {
            //                  v257 = *v254;
            //                  v258 = v254[1];
            //                  v254 += 2;
            //                  v256 -= 4;
            //                  *v255 = v257;
            //                  v255[1] = v258;
            //                  v255 += 2;
            //              }
            //              while (v256);
            //              v9 = a1;
            //              if (v99 == (v99 & 0xFFFFFFFC))
            //                  goto LABEL_118;
            //          }
            //          else
            //          {
            //              v101 = v9[35];
            //              v102 = v9[33];
            //              v103 = v98;
            //          }
            //          do
            //          {
            //              v104 = *v102++;
            //              --v101;
            //              *v103++ = v104;
            //          }
            //          while (v101 > 0);
            //      LABEL_118:
            //          if (v100)
            //operator delete[](v100);
            //          v9[33] = v98;
            //          v9[34] = v48;
            //      }
            //      if (v48 < 1)
            //          goto LABEL_275;
            //      v298 = v48 - 1;
            //      v105 = 0;
            //      v289 = 0;
            //      v304 = v48;
            //      do
            //      {
            //          v109 = ZNK15LogicPathFinder13getPathPointXEi(v46, v105);
            //          v110 = ZNK15LogicPathFinder13getPathPointYEi(v46, v105);
            //          if (v105 == v298)
            //          {
            //              v111 = *v296 == v109;
            //              if (*v296 == v109)
            //                  v111 = v9[22] == v110;
            //              if (v111)
            //              {
            //                  v147 = v9[32];
            //                  if (v147 != v9[31])
            //                      goto LABEL_191;
            //                  v148 = 2 * v147;
            //                  if (!(2 * v147))
            //                      v148 = 5;
            //                  if (v147 >= v148)
            //                  {
            //                  LABEL_190:
            //                      v46 = a5;
            //                  LABEL_191:
            //                      v156 = v9[30];
            //                      v9[32] = v147 + 1;
            //                      *(v156 + 4 * v147) = v292;
            //                      v157 = v9[35];
            //                      if (v157 != v9[34])
            //                      {
            //                      LABEL_207:
            //                          v166 = v9[33];
            //                          v9[35] = v157 + 1;
            //                          *(v166 + 4 * v157) = v293;
            //                          v289 = 1;
            //                          v108 = v298;
            //                          goto LABEL_126;
            //                      }
            //                      v158 = 2 * v157;
            //                      if (!(2 * v157))
            //                          v158 = 5;
            //                      if (v157 >= v158)
            //                      {
            //                      LABEL_206:
            //                          v46 = a5;
            //                          goto LABEL_207;
            //                      }
            //                      v159 = 4 * v158;
            //                      if (v158 != (v158 & 0x3FFFFFFF))
            //                          v159 = -1;
            //                      v160 = operator new[](v159, v158 != (v158 & 0x3FFFFFFF));
            //                      v161 = v9[33];
            //                      if (v157 >= 1)
            //                      {
            //                          if (v157 >= 4 && (v160 >= v161 + 4 * v157 || v160 + 4 * v157 <= v161))
            //                          {
            //                              v213 = v9[33];
            //                              v162 = v157 - (v157 & 0xFFFFFFFC);
            //                              v214 = v160;
            //                              v164 = (v160 + 4 * (v157 & 0xFFFFFFFC));
            //                              v163 = (v161 + 4 * (v157 & 0xFFFFFFFC));
            //                              v215 = v157 & 0xFFFFFFFC;
            //                              do
            //                              {
            //                                  v216 = *v213;
            //                                  v217 = v213[1];
            //                                  v213 += 2;
            //                                  v215 -= 4;
            //                                  *v214 = v216;
            //                                  v214[1] = v217;
            //                                  v214 += 2;
            //                              }
            //                              while (v215);
            //                              v9 = a1;
            //                              if (v157 == (v157 & 0xFFFFFFFC))
            //                                  goto LABEL_203;
            //                          }
            //                          else
            //                          {
            //                              v162 = v157;
            //                              v163 = v9[33];
            //                              v164 = v160;
            //                          }
            //                          do
            //                          {
            //                              v165 = *v163++;
            //                              --v162;
            //                              *v164++ = v165;
            //                          }
            //                          while (v162 > 0);
            //                      }
            //                  LABEL_203:
            //                      if (v161)
            //      operator delete[](v161);
            //                      v157 = v9[35];
            //                      v9[33] = v160;
            //                      v9[34] = v158;
            //                      goto LABEL_206;
            //                  }
            //                  v149 = 4 * v148;
            //                  if (v148 != (v148 & 0x3FFFFFFF))
            //                      v149 = -1;
            //                  v150 = operator new[](v149, v148 != (v148 & 0x3FFFFFFF));
            //                  v151 = v9[30];
            //                  if (v147 >= 1)
            //                  {
            //                      if (v147 >= 4 && (v150 >= v151 + 4 * v147 || v150 + 4 * v147 <= v151))
            //                      {
            //                          v208 = v9[30];
            //                          v152 = v147 - (v147 & 0xFFFFFFFC);
            //                          v209 = v150;
            //                          v154 = (v150 + 4 * (v147 & 0xFFFFFFFC));
            //                          v153 = (v151 + 4 * (v147 & 0xFFFFFFFC));
            //                          v210 = v147 & 0xFFFFFFFC;
            //                          do
            //                          {
            //                              v211 = *v208;
            //                              v212 = v208[1];
            //                              v208 += 2;
            //                              v210 -= 4;
            //                              *v209 = v211;
            //                              v209[1] = v212;
            //                              v209 += 2;
            //                          }
            //                          while (v210);
            //                          v9 = a1;
            //                          if (v147 == (v147 & 0xFFFFFFFC))
            //                              goto LABEL_187;
            //                      }
            //                      else
            //                      {
            //                          v152 = v147;
            //                          v153 = v9[30];
            //                          v154 = v150;
            //                      }
            //                      do
            //                      {
            //                          v155 = *v153++;
            //                          --v152;
            //                          *v154++ = v155;
            //                      }
            //                      while (v152 > 0);
            //                  }
            //              LABEL_187:
            //                  if (v151)
            //    operator delete[](v151);
            //                  v147 = v9[32];
            //                  v9[30] = v150;
            //                  v9[31] = v148;
            //                  goto LABEL_190;
            //              }
            //          }
            //          if (v105)
            //              goto LABEL_135;
            //          v112 = *v295 == v109;
            //          if (*v295 == v109)
            //              v112 = v9[24] == v110;
            //          if (!v112)
            //          {
            //          LABEL_135:
            //              v299 = v109;
            //              v113 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //              v114 = ZNK18LogicCharacterData11isCarryableEv(v113);
            //              v115 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //              v300 = v110;
            //              if (v114)
            //              {
            //                  v116 = 2 * (ZNK18LogicCharacterData18getCollisionRadiusEv(v115) > 199);
            //              }
            //              else
            //              {
            //                  v111 = !ZNK18LogicCharacterData6isBossEv(v115);
            //                  v116 = 1;
            //                  if (!v111)
            //                      v116 = 2;
            //              }
            //              v303 = v116;
            //              v117 = ZNK15LogicPathFinder13getPathPointXEi(v46, v105);
            //              v118 = v105 + 1;
            //              v119 = ZNK15LogicPathFinder13getPathPointYEi(v46, v105);
            //              v120 = 0;
            //              v301 = v105;
            //              while (v118 + v120 < v304)
            //              {
            //                  v121 = ZNK15LogicPathFinder13getPathPointXEi(a5, v118 + v120);
            //                  v122 = ZNK15LogicPathFinder13getPathPointYEi(a5, v118 + v120++);
            //                  if (!ZN15LogicPathFinder11isClearPathEiiiiibbbb(a5, v303, v117, v119, v121, v122, 0, 1, a6, 0))
            //                  {
            //                      v120 -= 2;
            //                      break;
            //                  }
            //              }
            //              v123 = v301;
            //              if (v120 > 0)
            //                  v123 = v301 + v120;
            //              v9 = a1;
            //              v48 = v304;
            //              v124 = v123;
            //              v125 = ZN12LogicTileMap21pathFinderTileToLogicEib(v299, 0);
            //              v126 = ZN12LogicTileMap21pathFinderTileToLogicEib(v300, 0);
            //              v127 = a1[32];
            //              v128 = v125 + 50;
            //              v302 = v124;
            //              if (v127 == a1[31])
            //              {
            //                  v129 = 2 * v127;
            //                  if (!(2 * v127))
            //                      v129 = 5;
            //                  if (v127 < v129)
            //                  {
            //                      v130 = 4 * v129;
            //                      if (v129 != (v129 & 0x3FFFFFFF))
            //                          v130 = -1;
            //                      v131 = operator new[](v130, v129 != (v129 & 0x3FFFFFFF));
            //                      v132 = a1[30];
            //                      if (v127 >= 1)
            //                      {
            //                          if (v127 < 4 || v131 < v132 + 4 * v127 && v131 + 4 * v127 > v132)
            //                          {
            //                              v133 = v127;
            //                              v134 = a1[30];
            //                              v135 = v131;
            //                              goto LABEL_156;
            //                          }
            //                          v197 = a1[30];
            //                          v133 = v127 - (v127 & 0xFFFFFFFC);
            //                          v198 = v131;
            //                          v135 = (v131 + 4 * (v127 & 0xFFFFFFFC));
            //                          v134 = (v132 + 4 * (v127 & 0xFFFFFFFC));
            //                          v199 = v131;
            //                          v200 = v127 & 0xFFFFFFFC;
            //                          do
            //                          {
            //                              v201 = *v197;
            //                              v202 = v197[1];
            //                              v197 += 2;
            //                              v200 -= 4;
            //                              *v199 = v201;
            //                              v199[1] = v202;
            //                              v199 += 2;
            //                          }
            //                          while (v200);
            //                          v9 = a1;
            //                          v131 = v198;
            //                          if (v127 != (v127 & 0xFFFFFFFC))
            //                          {
            //                              do
            //                              {
            //                              LABEL_156:
            //                                  v136 = *v134++;
            //                                  --v133;
            //                                  *v135++ = v136;
            //                              }
            //                              while (v133 > 0);
            //                          }
            //                      }
            //                      if (v132)
            //      operator delete[](v132);
            //                      v127 = v9[32];
            //                      v9[30] = v131;
            //                      v9[31] = v129;
            //                  }
            //                  v48 = v304;
            //              }
            //              v137 = v9[30];
            //              v9[32] = v127 + 1;
            //              v138 = v126 + 50;
            //              *(v137 + 4 * v127) = v128;
            //              v106 = v9[35];
            //              if (v106 == v9[34])
            //              {
            //                  v139 = 2 * v106;
            //                  if (!(2 * v106))
            //                      v139 = 5;
            //                  if (v106 < v139)
            //                  {
            //                      v140 = 4 * v139;
            //                      if (v139 != (v139 & 0x3FFFFFFF))
            //                          v140 = -1;
            //                      v141 = operator new[](v140, v139 != (v139 & 0x3FFFFFFF));
            //                      v142 = v9[33];
            //                      if (v106 >= 1)
            //                      {
            //                          if (v106 < 4 || v141 < v142 + 4 * v106 && v141 + 4 * v106 > v142)
            //                          {
            //                              v143 = v106;
            //                              v144 = v9[33];
            //                              v145 = v141;
            //                              goto LABEL_172;
            //                          }
            //                          v203 = v9[33];
            //                          v143 = v106 - (v106 & 0xFFFFFFFC);
            //                          v204 = v141;
            //                          v145 = (v141 + 4 * (v106 & 0xFFFFFFFC));
            //                          v144 = (v142 + 4 * (v106 & 0xFFFFFFFC));
            //                          v205 = v106 & 0xFFFFFFFC;
            //                          do
            //                          {
            //                              v206 = *v203;
            //                              v207 = v203[1];
            //                              v203 += 2;
            //                              v205 -= 4;
            //                              *v204 = v206;
            //                              v204[1] = v207;
            //                              v204 += 2;
            //                          }
            //                          while (v205);
            //                          v9 = a1;
            //                          if (v106 != (v106 & 0xFFFFFFFC))
            //                          {
            //                              do
            //                              {
            //                              LABEL_172:
            //                                  v146 = *v144++;
            //                                  --v143;
            //                                  *v145++ = v146;
            //                              }
            //                              while (v143 > 0);
            //                          }
            //                      }
            //                      if (v142)
            //      operator delete[](v142);
            //                      v106 = v9[35];
            //                      v9[33] = v141;
            //                      v9[34] = v139;
            //                  }
            //                  v48 = v304;
            //              }
            //              v107 = v9[33];
            //              v9[35] = v106 + 1;
            //              v108 = v302;
            //              *(v107 + 4 * v106) = v138;
            //              v46 = a5;
            //              goto LABEL_126;
            //          }
            //          v167 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //          v168 = ZNK18LogicCharacterData11isCarryableEv(v167);
            //          v169 = ZNK21LogicGameObjectServer7getDataEv(v9);
            //          if (v168)
            //          {
            //              v170 = 2 * (ZNK18LogicCharacterData18getCollisionRadiusEv(v169) > 199);
            //          }
            //          else
            //          {
            //              v170 = 1;
            //              if (ZNK18LogicCharacterData6isBossEv(v169))
            //                  v170 = 2;
            //          }
            //          v171 = ZNK15LogicPathFinder13getPathPointXEi(v46, 0);
            //          v172 = ZNK15LogicPathFinder13getPathPointYEi(v46, 0);
            //          v173 = 1;
            //          while (v48 != v173)
            //          {
            //              v174 = ZNK15LogicPathFinder13getPathPointXEi(v46, v173);
            //              v175 = ZNK15LogicPathFinder13getPathPointYEi(v46, v173++);
            //              if (!ZN15LogicPathFinder11isClearPathEiiiiibbbb(v46, v170, v171, v172, v174, v175, 0, 1, a6, 0))
            //              {
            //                  v176 = v173 - 3;
            //                  goto LABEL_217;
            //              }
            //          }
            //          v176 = v298;
            //      LABEL_217:
            //          v9 = a1;
            //          v177 = a1[32];
            //          if (v177 == a1[31])
            //          {
            //              v178 = 2 * v177;
            //              if (!(2 * v177))
            //                  v178 = 5;
            //              if (v177 >= v178)
            //                  goto LABEL_232;
            //              v179 = 4 * v178;
            //              if (v178 != (v178 & 0x3FFFFFFF))
            //                  v179 = -1;
            //              v180 = operator new[](v179, v178 != (v178 & 0x3FFFFFFF));
            //              v181 = a1[30];
            //              if (v177 >= 1)
            //              {
            //                  if (v177 >= 4 && (v180 >= v181 + 4 * v177 || v180 + 4 * v177 <= v181))
            //                  {
            //                      v218 = a1[30];
            //                      v182 = v177 - (v177 & 0xFFFFFFFC);
            //                      v219 = v180;
            //                      v184 = (v180 + 4 * (v177 & 0xFFFFFFFC));
            //                      v183 = (v181 + 4 * (v177 & 0xFFFFFFFC));
            //                      v220 = v177 & 0xFFFFFFFC;
            //                      do
            //                      {
            //                          v221 = *v218;
            //                          v222 = v218[1];
            //                          v218 += 2;
            //                          v220 -= 4;
            //                          *v219 = v221;
            //                          v219[1] = v222;
            //                          v219 += 2;
            //                      }
            //                      while (v220);
            //                      v9 = a1;
            //                      if (v177 == (v177 & 0xFFFFFFFC))
            //                          goto LABEL_229;
            //                  }
            //                  else
            //                  {
            //                      v182 = v177;
            //                      v183 = a1[30];
            //                      v184 = v180;
            //                  }
            //                  do
            //                  {
            //                      v185 = *v183++;
            //                      --v182;
            //                      *v184++ = v185;
            //                  }
            //                  while (v182 > 0);
            //              }
            //          LABEL_229:
            //              if (v181)
            //  operator delete[](v181);
            //              v9[30] = v180;
            //              v177 = v9[32];
            //              v46 = a5;
            //              v9[31] = v178;
            //          LABEL_232:
            //              v48 = v304;
            //          }
            //          v186 = v9[30];
            //          v9[32] = v177 + 1;
            //          *(v186 + 4 * v177) = x;
            //          v187 = v9[35];
            //          if (v187 == v9[34])
            //          {
            //              v188 = 2 * v187;
            //              if (!(2 * v187))
            //                  v188 = 5;
            //              if (v187 >= v188)
            //                  goto LABEL_248;
            //              v189 = 4 * v188;
            //              if (v188 != (v188 & 0x3FFFFFFF))
            //                  v189 = -1;
            //              v190 = operator new[](v189, v188 != (v188 & 0x3FFFFFFF));
            //              v191 = v9[33];
            //              if (v187 >= 1)
            //              {
            //                  if (v187 >= 4 && (v190 >= v191 + 4 * v187 || v190 + 4 * v187 <= v191))
            //                  {
            //                      v223 = v9[33];
            //                      v192 = v187 - (v187 & 0xFFFFFFFC);
            //                      v224 = v190;
            //                      v194 = (v190 + 4 * (v187 & 0xFFFFFFFC));
            //                      v193 = (v191 + 4 * (v187 & 0xFFFFFFFC));
            //                      v225 = v187 & 0xFFFFFFFC;
            //                      do
            //                      {
            //                          v226 = *v223;
            //                          v227 = v223[1];
            //                          v223 += 2;
            //                          v225 -= 4;
            //                          *v224 = v226;
            //                          v224[1] = v227;
            //                          v224 += 2;
            //                      }
            //                      while (v225);
            //                      v9 = a1;
            //                      if (v187 == (v187 & 0xFFFFFFFC))
            //                          goto LABEL_245;
            //                  }
            //                  else
            //                  {
            //                      v192 = v187;
            //                      v193 = v9[33];
            //                      v194 = v190;
            //                  }
            //                  do
            //                  {
            //                      v195 = *v193++;
            //                      --v192;
            //                      *v194++ = v195;
            //                  }
            //                  while (v192 > 0);
            //              }
            //          LABEL_245:
            //              if (v191)
            //  operator delete[](v191);
            //              v9[33] = v190;
            //              v187 = v9[35];
            //              v46 = a5;
            //              v9[34] = v188;
            //          LABEL_248:
            //              v48 = v304;
            //          }
            //          v196 = v9[33];
            //          v108 = v176 & ~(v176 >> 31);
            //          v9[35] = v187 + 1;
            //          *(v196 + 4 * v187) = y;
            //          *a4 = x;
            //          a4[1] = y;
            //      LABEL_126:
            //          v105 = v108 + 1;
            //      }
            //      while (v105 < v48);
            //      if ((v289 & 1) == 0)
            //      {
            //      LABEL_275:
            //          v228 = v9[32];
            //          if (v228 == v9[31])
            //          {
            //              v229 = 2 * v228;
            //              if (!(2 * v228))
            //                  v229 = 5;
            //              if (v228 < v229)
            //              {
            //                  v230 = 4 * v229;
            //                  if (v229 != (v229 & 0x3FFFFFFF))
            //                      v230 = -1;
            //                  v231 = operator new[](v230, v229 != (v229 & 0x3FFFFFFF));
            //                  v232 = v9[30];
            //                  if (v228 >= 1)
            //                  {
            //                      if (v228 < 4 || v231 < v232 + 4 * v228 && v231 + 4 * v228 > v232)
            //                      {
            //                          v233 = v228;
            //                          v234 = v9[30];
            //                          v235 = v231;
            //                          goto LABEL_286;
            //                      }
            //                      v259 = v9[30];
            //                      v233 = v228 - (v228 & 0xFFFFFFFC);
            //                      v260 = v231;
            //                      v235 = (v231 + 4 * (v228 & 0xFFFFFFFC));
            //                      v234 = (v232 + 4 * (v228 & 0xFFFFFFFC));
            //                      v261 = v228 & 0xFFFFFFFC;
            //                      do
            //                      {
            //                          v262 = *v259;
            //                          v263 = v259[1];
            //                          v259 += 2;
            //                          v261 -= 4;
            //                          *v260 = v262;
            //                          v260[1] = v263;
            //                          v260 += 2;
            //                      }
            //                      while (v261);
            //                      v9 = a1;
            //                      if (v228 != (v228 & 0xFFFFFFFC))
            //                      {
            //                          do
            //                          {
            //                          LABEL_286:
            //                              v236 = *v234++;
            //                              --v233;
            //                              *v235++ = v236;
            //                          }
            //                          while (v233 > 0);
            //                      }
            //                  }
            //                  if (v232)
            //    operator delete[](v232);
            //                  v228 = v9[32];
            //                  v9[30] = v231;
            //                  v9[31] = v229;
            //              }
            //              v46 = a5;
            //          }
            //          v237 = v9[30];
            //          v9[32] = v228 + 1;
            //          *(v237 + 4 * v228) = v292;
            //          v238 = v9[35];
            //          if (v238 == v9[34])
            //          {
            //              v239 = 2 * v238;
            //              if (!(2 * v238))
            //                  v239 = 5;
            //              if (v238 < v239)
            //              {
            //                  v240 = 4 * v239;
            //                  if (v239 != (v239 & 0x3FFFFFFF))
            //                      v240 = -1;
            //                  v241 = operator new[](v240, v239 != (v239 & 0x3FFFFFFF));
            //                  v242 = v9[33];
            //                  if (v238 >= 1)
            //                  {
            //                      if (v238 < 4 || v241 < v242 + 4 * v238 && v241 + 4 * v238 > v242)
            //                      {
            //                          v243 = v238;
            //                          v244 = v9[33];
            //                          v245 = v241;
            //                          goto LABEL_302;
            //                      }
            //                      v264 = v9[33];
            //                      v243 = v238 - (v238 & 0xFFFFFFFC);
            //                      v265 = v241;
            //                      v245 = (v241 + 4 * (v238 & 0xFFFFFFFC));
            //                      v244 = (v242 + 4 * (v238 & 0xFFFFFFFC));
            //                      v266 = v238 & 0xFFFFFFFC;
            //                      do
            //                      {
            //                          v267 = *v264;
            //                          v268 = v264[1];
            //                          v264 += 2;
            //                          v266 -= 4;
            //                          *v265 = v267;
            //                          v265[1] = v268;
            //                          v265 += 2;
            //                      }
            //                      while (v266);
            //                      v9 = a1;
            //                      if (v238 != (v238 & 0xFFFFFFFC))
            //                      {
            //                          do
            //                          {
            //                          LABEL_302:
            //                              v246 = *v244++;
            //                              --v243;
            //                              *v245++ = v246;
            //                          }
            //                          while (v243 > 0);
            //                      }
            //                  }
            //                  if (v242)
            //    operator delete[](v242);
            //                  v238 = v9[35];
            //                  v9[33] = v241;
            //                  v9[34] = v239;
            //              }
            //              v46 = a5;
            //          }
            //          v247 = v9[33];
            //          v9[35] = v238 + 1;
            //          *(v247 + 4 * v238) = v293;
            //      }
            //      return ZN20LogicCharacterServer12ensurePathOkEP15LogicPathFinder(v9, v46);
        }
        public int GetGearBoost(int LogicType)
        {
            if (Gear1 != null)
            {
                if (Gear1.GearData.LogicType == LogicType)
                {
                    Gear1.OnBoost();
                    return Gear1.GearData.ModifierValue;
                }
            }
            if (Gear2 != null)
            {
                if (Gear2.GearData.LogicType == LogicType)
                {
                    Gear2.OnBoost();
                    return Gear2.GearData.ModifierValue;
                }
            }
            return 0;
        }
        public void Watch(Character Target)
        {
            SetStartRotation(LogicMath.GetAngle(Target.GetX() - GetX(), Target.GetY() - GetY()));
        }
        public bool IsPlayerControlRemoved()
        {
            BattlePlayer v2; // r0
            int v3; // r5
            bool v4; // zf
                     //_BOOL4 result; // r0
            bool v6; // zf
            int v7; // r0
                    //_BOOL4 v8; // r0
            bool v9; // zf
            int v10; // r0
            int v11; // r1
            int v12; // r1

            if (GetBattle().StoryMode != null && GetBattle().StoryMode.Intangible && GetBattle().StoryMode.PlayerCharacter == this) return true;
            //if (!(GetCurrentCastingSkill()?.CanMoveAtSameTime ?? true)) return true;
            if (ViusalChargeType > 0) return false;
            bool result = true;
            v2 = GetPlayer();
            if (v2 == null)
                goto LABEL_6;
            if (v2.Accessory != null)
            {
                if (v2.Accessory.IsActive && v2.Accessory.AccessoryData.StopMovement) return result;
            }
            //v3 = *(v2 + 204);
            //v4 = v3 == 0;
            //if (v3)
            //    v4 = *(v3 + 36) == 0;
            //if (v4)
            //    goto LABEL_6;
            //v7 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //v8 = ZNK18LogicCharacterData6isHeroEv(v7);
            //v9 = !v8;
            //if (v8)
            //    v9 = *(v3 + 36) == 0;
            //if (v9 || (v12 = ZNK18LogicAccessoryData15getStopMovementEv(*v3), result = 1, !v12))
            //{
            //    v10 = ZNK21LogicGameObjectServer7getDataEv(a1);
            //    if (ZNK18LogicCharacterData6isHeroEv(v10)
            //      || (v11 = ZNK18LogicAccessoryData18getStopPetForDelayEv(*v3), result = 1, !v11))
            //    {
            //        ;
            //    }
            //}
            if (true) ;
            else return result;
            LABEL_6:
            result = true;
            if (!IsCharging)
            {
                v6 = !KnockBacked;

                if (!KnockBacked)
                    v6 = m_stunTicks <= 0;
                if (v6)
                    return DraggingObject != null;
            }
            return result;
        }
        public void UltiUsed()
        {
            BattlePlayer player = GetPlayer();
            if (player != null)
            {
                if (!GetBattle().HasEventModifier(13)) player.UseUlti();
                m_usingUltiCurrently = false;
            }
            Ultiing = true;
        }
        public Character ReloadPlayer(CharacterData data)
        {
            Character n = GetBattle().SpawnHero(data, 11, GetIndex(), 0, false);
            n.SetPosition(GetX(), GetY(), 0);
            n.Gear1 = new Gear(GetPlayer().Gear1);
            n.Gear2 = new Gear(GetPlayer().Gear2);
            GetPlayer().OwnObjectId = n.GetGlobalID();
            n.m_hitpoints = m_hitpoints;
            n.m_skills[0].Charge = m_skills[0].Charge;
            CauseDamage(this, 99999, 0, false, null, false, true);
            GameObjectManager.RemoveGameObject(this);
            return n;

        }
        public int MoveTo(int X, int Y)
        {
            MoveTo(0, X, Y, 0, 0, 0, 0);
            return 0;
        }
        public int MoveTo(int a1, int deltax, int deltay, int a4, int a5, int a6, int a7)
        {
            int v10; // r6
            int v11; // r7
            int v12; // r0
            int v13; // r0
            int v14; // r0
            bool v15; // zf
            int v16; // r10
            int v17; // r5
            int x1; // r7 MAPDST
            int y1; // r6 MAPDST
            int v20; // r0
            int v21; // r8
            int v22; // r5
            int v23; // r9
            int v24; // r7
            int v25; // r5
            int i; // r10
            int v27; // r6
            int v28; // r9
            int v29; // r0
            int v30; // r0
            int v31; // r8
            int v32; // r2
            int v33; // r0
            int v34; // r6
            int v35; // r0
            int v36; // r0
            int v37; // r7
            int x; // r5
            int v39; // r6
            int y; // r9
            int v41; // r0
            int v42; // r1
            int v43; // r7
            int v44; // r5
            int v45; // r0
            int v46; // r8
            int v47; // r6
            int v48; // r0
            int v49; // r0
            int v50; // r0
            int v51; // r6
            int v52; // r9
            int v53; // r5
            int v54; // r1
            int v55; // r6
            int v56; // r0
            int v57; // r0
            int v58; // r0
            int v59; // r9
            int v60; // r6
            int v61; // r10
            int v62; // r7
            int v63; // r0
            char v64; // r8
            int v65; // r6
            int v66; // r0
            int v67; // r7
            int v68; // r0
            int v69; // r0
            int v70; // r0
            int v71; // r7
            int v72; // r5
            int v73; // r0
            int v74; // r6
            int v75; // r0
            int v76; // r0
            int v77; // r5
            int v78; // r0
            int v80; // r6
            int v81; // r5
            int v82; // r7
            int v83; // r0
            int v84; // [sp+0h] [bp-58h]
            int v85; // [sp+4h] [bp-54h]
            int v86; // [sp+18h] [bp-40h]
            int v87; // [sp+1Ch] [bp-3Ch]
            int v88; // [sp+20h] [bp-38h]
            int v91; // [sp+28h] [bp-30h]
            int v93; // [sp+30h] [bp-28h] BYREF

            if (!IsPlayerControlRemoved())
            {
                //if (IsSteerMovementActive())
                //{
                //    SteerAngle = LogicMath.GetAngle(deltax - GetX(), deltay - GetY());
                //    return 0; 
                //}
                //    if (*(a1 + 288) >= 1)
                //    {
                //        v10 = 0;
                //        while (1)
                //        {
                //            v11 = *(*(a1 + 280) + 4 * v10);
                //            if (*(v11 + 28))
                //            {
                //                v12 = ZNK16LogicSkillServer7getDataEv(*(*(a1 + 280) + 4 * v10));
                //                if (ZNK14LogicSkillData14getCastingTimeEv(v12) > 0)
                //                    break;
                //            }
                //            if (*(v11 + 4) >= 1)
                //                break;
                //            if (++v10 >= *(a1 + 288))
                //                goto LABEL_13;
                //        }
                //        v13 = ZNK16LogicSkillServer7getDataEv(v11);
                //        if (v13)
                //        {
                //            v14 = ZNK14LogicSkillData17canMoveAtSameTimeEv(v13);
                //            v15 = !v14;
                //            if (!v14)
                //                v15 = *(a1 + 300) == 0;
                //            if (v15 && *(a1 + 288) >= 1)
                //            {
                //                v80 = 0;
                //                v81 = 0;
                //                do
                //                {
                //                    v82 = *(*(a1 + 280) + 4 * v80);
                //                    v83 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
                //                    ZNK21LogicBattleModeServer12getTicksGoneEv(v83);
                //                    v81 |= ZN16LogicSkillServer9interruptEP20LogicCharacterServerbi(v82, a1, 0);
                //                    ++v80;
                //                }
                //                while (v80 < *(a1 + 288));
                //                if ((v81 & 1) != 0)
                //                {
                //                    *(a1 + 528) = 0;
                //                    *(a1 + 372) = 0;
                //                }
                //            }
                //        }
                //    }
                //LABEL_13:
                //    v16 = ZNK21LogicGameObjectServer24getLogicBattleModeServerEv(a1);
                //    v17 = ZN21LogicBattleModeServer10getTileMapEv(v16);
                //    x1 = LogicMath.Clamp(deltax, 1, *(v17 + 112) - 2);
                //    y1 = LogicMath.Clamp(deltay, 1, *(v17 + 116) - 2);
                //    if (*(a1 + 293) && *(v16 + 164) == 11)
                //    {
                //        v88 = v17;
                //        ZNK28LogicGameObjectManagerServer14getGameObjectsEv();
                //        v21 = v20[2];
                //        v22 = 999999999;
                //        if (v21 < 1)
                //        {
                //            v28 = 0;
                //            v32 = 0;
                //            v31 = 0;
                //        }
                //        else
                //        {
                //            v87 = v16;
                //            v23 = v20;
                //            v24 = 0;
                //            v25 = 0;
                //            for (i = 0; i != v21; ++i)
                //            {
                //                v27 = *(*v23 + 4 * i);
                //                if ((*(*v27 + 32))(v27))
                //                    break;
                //                if ((*(*v27 + 48))(v27) && v27[11] != *(a1 + 44) && *(ZNK21LogicGameObjectServer7getDataEv(v27) + 268) == 6)
                //                {
                //                    v24 = v27;
                //                }
                //                else if ((*(*v27 + 48))(v27)
                //                       && v27[11] == *(a1 + 44)
                //                       && *(ZNK21LogicGameObjectServer7getDataEv(v27) + 268) == 12)
                //                {
                //                    v25 = v27;
                //                }
                //            }
                //            if (v24)
                //            {
                //                v28 = ZNK21LogicGameObjectServer4getXEv(v24);
                //                v86 = ZNK21LogicGameObjectServer4getYEv(v24);
                //                v29 = ZNK21LogicGameObjectServer7getDataEv(v24);
                //                v30 = ZNK18LogicCharacterData12getTileRangeEv(v29);
                //                v31 = (100 * v30 + 300) * (100 * v30 + 300);
                //            }
                //            else
                //            {
                //                v31 = 0;
                //                v86 = 0;
                //                v28 = 0;
                //            }
                //            if (v25)
                //            {
                //                v33 = ZNK21LogicGameObjectServer4getXEv(v25);
                //                v34 = (v33 - v28) * (v33 - v28);
                //                v35 = ZNK21LogicGameObjectServer4getYEv(v25);
                //                v32 = v86;
                //                v16 = v87;
                //                v22 = v34 + (v35 - v86) * (v35 - v86);
                //            }
                //            else
                //            {
                //                v22 = 999999999;
                //                v16 = v87;
                //                v32 = v86;
                //            }
                //        }
                //        v36 = (y1 - v32) * (y1 - v32) + (x1 - v28) * (x1 - v28);
                //        if (v36 < v31 && v36 < v22)
                //        {
                //            v37 = v32;
                //            ZN12LogicVector2C2Ev(v93);
                //            if (v22 < v31)
                //                v31 = v22;
                //            x = GetX();
                //            v39 = v28;
                //            y = GetY();
                //            v41 = LogicMath.Sqrt(v31);
                //            v84 = v39;
                //            v85 = v37;
                //            if (ZN17LogicGamePlayUtil27lineSegmentIntersectsCircleEiiiiiiiP12LogicVector2(
                //                   x,
                //                   y,
                //                   x1,
                //                   y1,
                //                   v84,
                //                   v85,
                //                   *&v41,
                //                   v93))
                //            {
                //                x1 = v93[0];
                //                y1 = v93[1];
                //            }
                //        }
                //        v17 = v88;
                //    }
                //    v42 = *(v17 + 112);
                //    *(a1 + 808) = -1;
                //v43 = LogicMath.Clamp(x1, 1, v42 - 2);
                //    v44 = LogicMath.Clamp(y1, 1, *(v17 + 116) - 2);
                //    v45 = ZNK21LogicGameObjectServer7getDataEv(a1);
                LogicVector2 vector = new LogicVector2(deltax, deltay);
                GamePlayUtil.GetClosestLevelBorderCollision(GetX(), GetY(), deltax, deltay, GameObjectManager.GetBattle().GetTileMap(), 0, vector);
                if (vector.X > deltax) vector.X += 50;
                if (vector.X < deltax) vector.X -= 50;
                if (vector.Y > deltay) vector.Y += 50;
                if (vector.Y < deltay) vector.Y -= 50;
                v46 = CharacterData.Speed;
                //    v47 = ZN21LogicBattleModeServer13getPathFinderEv(v16);
                //    v48 = ZNK21LogicGameObjectServer7getDataEv(a1);
                //    v49 = ZNK18LogicCharacterData16canWalkOverWaterEv(v48);
                PathfindTo(
                      a1,
                      vector.X,
                      vector.Y,
                      0,
                      0,
                      0,
                      a6,
                      1,
                      a7);
                //v50 = *(a1 + 128);
                //v51 = v50 - 1;
                //if (v50 - 1 <= 0)
                //{
                //    *(a1 + 220) = 0;
                //}
                //else
                //{
                //    v52 = 4 * v50 - 4;
                //    v91 = v44;
                //    v53 = 0;
                //    do
                //    {
                //        v54 = *(*(a1 + 132) + v52 - 4) - *(*(a1 + 132) + v52);
                //        --v51;
                //        v53 += LogicMath.Sqrt(
                //                 (*(*(a1 + 120) + v52 - 4) - *(*(a1 + 120) + v52)) * (*(*(a1 + 120) + v52 - 4) - *(*(a1 + 120) + v52))
                //               + v54 * v54);
                //        v52 -= 4;
                //    }
                //    while (v51 > 0);
                PathLength = LogicMath.Sqrt(
                             (WayPoint.X - WayPointStart.X) * (WayPoint.X - WayPointStart.X)
                           + (WayPoint.Y - WayPointStart.Y) * (WayPoint.Y - WayPointStart.Y));
                //if(m_isBot==0) Debugger.Print(PathLength.ToString());
                //MoveStart = TicksGone-1;
                //MoveEnd = TicksGone-1;
                if (PathLength >= 1)
                {
                    v55 = TicksGone;
                    v56 = PathLength;
                    MoveStart = v55;
                    v57 = 5 * v56;
                    v46 += GetBuffedSpeed();
                    v46 += GetUnbuffedSpeed();
                    //if (a4)
                    //    v46 = a5;
                    MoveEnd = LogicMath.Max(1, 4 * v57 / v46) + v55;
                    //Debugger.Print("PL:"+PathLength+" :"+(MoveEnd - v55));
                    //MoveEnd += 1000;
                    //v58 = ZNK21LogicGameObjectServer7getDataEv(a1);
                    //    if (ZNK18LogicCharacterData6isHeroEv(v58) && *(a1 + 908) <= -1)
                    //    {
                    //        v59 = v16;
                    //        *(a1 + 908) = 10;
                    //        v60 = ZN21LogicBattleModeServer10getTileMapEv(v16);
                    //        v61 = v43;
                    //        v62 = ZN12LogicTileMap21logicToPathFinderTileEi(v43) / 3;
                    //        v63 = ZN12LogicTileMap21logicToPathFinderTileEi(v91);
                    //        v64 = *(ZNK12LogicTileMap7getTileEii(v60, v62, v63 / 3) + 48);
                    //        v65 = ZN21LogicBattleModeServer10getTileMapEv(v59);
                    //        v66 = GetX();
                    //        v67 = ZN12LogicTileMap21logicToPathFinderTileEi(v66) / 3;
                    //        v68 = GetY();
                    //        v69 = ZN12LogicTileMap21logicToPathFinderTileEi(v68);
                    //        v70 = ZNK12LogicTileMap7getTileEii(v65, v67, v69 / 3);
                    //        v71 = 150;
                    //        LOBYTE(v70) = *(v70 + 48) | v64;
                    //        *(a1 + 920) = v70;
                    //        if (!v70)
                    //            v71 = 450;
                    //        v72 = (2 * v71) | 1;
                    //        v73 = ZNK21LogicBattleModeServer14getLogicRandomEv(v59);
                    //        v74 = v61 - v71 + ZN11LogicRandom4randEi(v73, v72);
                    //        *(a1 + 912) = v74;
                    //        v75 = ZN21LogicBattleModeServer10getTileMapEv(v59);
                    //        *(a1 + 912) = LogicMath.Clamp(v74, 1, *(v75 + 112) - 2);
                    //        v76 = ZNK21LogicBattleModeServer14getLogicRandomEv(v59);
                    //        v77 = ZN11LogicRandom4randEi(v76, v72) + v91 - v71;
                    //        *(a1 + 916) = v77;
                    //        v78 = ZN21LogicBattleModeServer10getTileMapEv(v59);
                    //        *(a1 + 916) = LogicMath.Clamp(v77, 1, *(v78 + 116) - 2);
                    //    }
                    //}
                }
            }
            return 0;
        }
        public void AddConsumableShield(int a2, int a3)
        {
            if (ConsumableShield < a2)
            {
                ConsumableShield = a2;
                ConsumableShieldMax = a2;
                ConsumableShieldTick = TicksGone;
                ConsumableShieldDuration = a3;
            }
        }
        public Character GetActivePet(bool IgnoreBase)
        {
            foreach (GameObject gameObject in GameObjectManager.GetGameObjects())
            {
                if (gameObject.GetObjectType() != 0) continue;
                Character character = (Character)gameObject;
                if (character != this && character.GetIndex() == GetIndex() && character.IsAlive())
                {
                    if (!IgnoreBase || character.CharacterData.Speed > 0) return character;
                }
            }
            return null;
        }
        public override void Encode(BitStream bitStream, bool isOwnObject, int OwnObjectIndex, int visionTeam)
        {
            //if (m_isBot == 0) Debugger.Print(State);
            //if (m_isBot == 0) Debugger.Print("DIS:" + (MoveEnd - MoveStart));

            BattlePlayer player = GameObjectManager.GetBattle().GetPlayerWithObject(GetGlobalID());
            base.Encode(bitStream, isOwnObject, visionTeam);
            bitStream.WritePositiveInt(GetBattle().StoryMode == null && visionTeam == this.GetIndex() / 16 ? 10 : GetFadeCounter(), 4);
            if (CharacterData.HasAutoAttack() || CharacterData.Speed > 0 || CharacterData.IsTrainingDummy())
            {
                if (isOwnObject)
                {
                    bitStream.WriteBoolean(IsPlayerControlRemoved()); // 0xa1aff8
                    if (bitStream.WriteBoolean(ForcedAngleTicks + 1 >= TicksGone || IsSteerMovementActive()) || IsPlayerControlRemoved())
                    //bitStream.WriteBoolean(false); // 0xa1aff8
                    //if (bitStream.WriteBoolean(true) || true)
                    {
                        bitStream.WritePositiveIntMax511(AttackAngle);
                        bitStream.WritePositiveIntMax511(MoveAngle);
                    }
                }
                else
                {
                    bitStream.WritePositiveIntMax511(AttackAngle);
                    bitStream.WritePositiveIntMax511(MoveAngle);
                }
                bitStream.WritePositiveIntMax7(State); // State
                bitStream.WriteBoolean(GetDamageBuffTemporary() > 0 && !(GetPlayer()?.OverCharging ?? false)); // 紫气
                bitStream.WriteInt(m_attackingTicks, 6); // Animation Playing
                                                         //if (m_isBot == 0) Debugger.Print(m_attackingTicks);
                bitStream.WriteBoolean(KnockBacked); // reverse angle???
                if (bitStream.WriteBoolean(m_stunTicks >= 1)) bitStream.WriteBoolean(IsWeaklyStunned); // Stun
                bitStream.WriteBoolean(Shaking > 0);// unk
                bitStream.WriteBoolean(StarPowerIC); // star power indicator
            }
            else
            {
                bitStream.WritePositiveIntMax7(State); // State
                if (CharacterData.IsTrain() || CharacterData.ManualRotations)
                {
                    bitStream.WritePositiveIntMax511(AttackAngle);
                    bitStream.WritePositiveIntMax511(MoveAngle);
                }
                else if (CharacterData.AreaEffect != null) bitStream.WritePositiveIntMax511(MoveAngle);
            }

            if (CharacterData.LifeTimeTicks >= 1) bitStream.WritePositiveIntMax511(511);
            bitStream.WritePositiveVIntMax65535OftenZero(HitEffectProjectile); //hit by projectile data instance id -1 (for hit effect)
            bitStream.WritePositiveVIntMax65535OftenZero(HitEffectSkin); //hit by skin data instance id -1 (for hit effect)

            bitStream.WriteBoolean(false);//slippery debuff
            bitStream.WriteBoolean(HasBuff(4)); //fast
            bitStream.WriteBoolean(HasBuff(3));//slow
            bitStream.WriteBoolean(false); //surpress
            bitStream.WriteBoolean(true);//unknown
            if (bitStream.WriteBoolean(HasBuff(14))) bitStream.WritePositiveIntMax7(GetBuff(14).EffectType);
            bitStream.WriteBoolean(HasBuff(15));//Belle
            bitStream.WriteBoolean(false);//Otis
            if (bitStream.WriteBoolean(false)) bitStream.WriteIntMax15(0);
            if (bitStream.WriteBoolean(ReloadBuffTicks > 0)) bitStream.WriteIntMax15(0);//reload buff
            bitStream.WriteBoolean(false); //OVERCHARGE

            bitStream.WritePositiveVIntMax255OftenZero(0);
            bitStream.WritePositiveVIntMax255OftenZero(Poisons.Count);
            if (Poisons.Count >= 1)
            {
                foreach (Poison poison in Poisons)
                {
                    bitStream.WritePositiveIntMax15(poison.Type);
                    bitStream.WritePositiveIntMax15(poison.SourceIndex);
                }
            }
            bitStream.WriteBoolean(IsCrippled);
            bitStream.WritePositiveVIntMax255OftenZero(GetBuffedHealthRegen());//heal type

            bitStream.WritePositiveVIntMax16777215(m_hitpoints);
            bitStream.WritePositiveVIntMax16777215(m_maxHitpoints);

            if (m_hitpoints <= 0) bitStream.WriteIntMax15(0);
            if (CharacterData.IsMirrage()) bitStream.WriteBoolean(false);
            if (bitStream.WriteBoolean(false)) bitStream.WritePositiveIntMax511(511);
            if (CharacterData.IsHero() || CharacterData.IsMirrage())
            {
                bitStream.WritePositiveVIntMax255OftenZero(m_itemCount);
                bitStream.WritePositiveVIntMax255OftenZero(0);//v53
                if (bitStream.WriteBoolean(ConsumableShield > 0))
                {
                    bitStream.WritePositiveIntMax8191(ConsumableShield);
                    bitStream.WritePositiveIntMax8191(ConsumableShieldMax);
                }
                //if (bitStream.WriteBoolean(true))
                //{
                //    bitStream.WritePositiveIntMax8191(6666);
                //    bitStream.WritePositiveIntMax8191(6666);
                //}
                bitStream.WriteBoolean(false);//Eaten Ruff
                if (!IsAlive()) bitStream.WriteBoolean(TransformAnimation);
            }
            if (CharacterData.UniqueProperty == 24 || CharacterData.IsTwin()) bitStream.WriteBoolean(false);
            if (CharacterData.IsHero())
            {
                bitStream.WritePositiveVIntMax255OftenZero(0);

                //IsCharging=true;
                bitStream.WriteBoolean(false); // big brawler
                                               //bool stuff = TicksGone % 2==0;
                bitStream.WriteBoolean(true);
                {
                    bitStream.WriteBoolean(IsCharging || ViusalChargeType > 0);
                    bitStream.WriteBoolean(IsInvincible); // immunity
                    bitStream.WriteBoolean(m_usingUltiCurrently); // Yellow
                    bitStream.WriteBoolean(Ultiing); // using ulti
                    bitStream.WriteBoolean(false); // useless
                                                   //bitStream.WriteBoolean(false);
                }
                if (player != null && player.HasOverCharge())//Have OVERCHARGE
                {
                    bitStream.WriteBoolean(player.OverChargeReady());//overcharge ready
                    bitStream.WriteBoolean(player.OverCharging);//overcharging
                    bitStream.WriteBoolean(player.OverChargeStarted);//overcharge started
                    bitStream.WriteBoolean(player.OverChargeEnded);//overcharge ended
                }
                if (isOwnObject)
                {
                    if (bitStream.WriteBoolean(VisionOverrideTicks >= 1))
                    {
                        bitStream.WriteIntMax65535(VisionOverrideX);
                        bitStream.WriteIntMax65535(VisionOverrideY);
                    }

                }
                if (GetWeaponSkill() != null)
                {
                    if (GetWeaponSkill().SkillData.ChargedShotCount >= 1)
                    {
                        bitStream.WriteIntMax3(ChargedShotCount);
                    }
                    if (isOwnObject && GetWeaponSkill().SkillData.AttackPattern == 13)
                    {
                        bitStream.WritePositiveVIntMax255OftenZero(LogicMath.Clamp(SkillHoldTicks, 0, 255));
                    }
                    if (GetWeaponSkill().SkillData.AttackPattern == 15)
                    {
                        bitStream.WritePositiveVIntMax255OftenZero(100);
                        bitStream.WritePositiveIntMax511(MoveAngle);
                    }
                }
                if (CharacterData.ShouldEncodePetStatus || CharacterData.UniqueProperty == 13 && CharacterData.IsHero()) bitStream.WriteBoolean(false);
                if (CharacterData.HasPowerLevels || CharacterData.UniqueProperty == 1)
                {
                    bitStream.WritePositiveIntMax3(GetPowerLevel());
                }
                if (bitStream.WriteBoolean(false))
                {
                    bitStream.WritePositiveIntMax15(0);
                    bitStream.WritePositiveIntMax7(0);
                }
                //good till now
                //IsCharging=true;
                if (IsCharging || ViusalChargeType > 0)
                {
                    bitStream.WritePositiveIntMax255(ChargeAnimation);
                    bitStream.WritePositiveIntMax31(m_activeChargeType);
                    if (m_activeChargeType == 17) bitStream.WriteBoolean(false);//帅otto

                }
                switch (ChargeUpType)
                {
                    case 0:
                        break;
                    case 1:
                    case 7:
                        bitStream.WritePositiveVIntMax255OftenZero(ChargeUp / 50);
                        break;
                    case 5:
                        //sub_8839E8(a2, (*(a1 + 1232) >> 2));
                        break;
                    case 8:
                        bitStream.WritePositiveIntMax1023(LogicMath.Clamp(ChargeUp, 0, 1023));
                        break;
                    case 9:
                        bitStream.WritePositiveIntMax7(ChargedShotCount);
                        break;
                    default:
                        bitStream.WritePositiveIntMax1023(LogicMath.Clamp(ChargeUp / 50, 0, 1023));
                        break;
                }
                if (DataTables.GetProjectileByName(GetUltimateSkill()?.SkillData.Projectile)?.UniqueProperty == 5)
                {
                    bitStream.WriteBoolean(false);
                }

                if (true)
                {
                    if (Gear1 != null) Gear1.Encode(bitStream);
                    if (Gear2 != null) Gear2.Encode(bitStream);
                }
                if (CharacterData.PetAutoSpawnDelay > 0) bitStream.WritePositiveIntMax1023(0);
            }
            else if (CharacterData.SecondaryPet) bitStream.WritePositiveIntMax7(0);
            else
            {
                if ((GetWeaponSkill() != null && GetWeaponSkill().SkillData.BehaviorType == "Charge") || (GetUltimateSkill() != null && GetUltimateSkill().SkillData.BehaviorType == "Charge"))
                {
                    if (bitStream.WriteBoolean(IsCharging)) bitStream.WritePositiveIntMax31(m_activeChargeType);
                }
            }
            //bitStream.WritePositiveIntMax1023(0);
            if (CharacterData.Pet != null) bitStream.WriteBoolean(true);
            if (CharacterData.UniqueProperty == 9) bitStream.WriteBoolean(false);
            if (CharacterData.UniqueProperty == 20) bitStream.WriteBoolean(false);
            if (GamePlayUtil.CanUseFastTravel(this)) bitStream.WriteBoolean(IsTeleporting);
            if (CharacterData.UniqueProperty == 22) bitStream.WriteBoolean(false);
            if (bitStream.WriteBoolean(false))
            {
                ;
            }
            if (CharacterData.IsMinionLastStand()) bitStream.WriteBoolean(false);
            if (CharacterData.UniqueProperty == 11 && isOwnObject)
            {
                bitStream.WriteIntMax255(165);
                bitStream.WritePositiveIntMax7(JesterSkillCount);
            }

            if (CharacterData.IsBoss()) bitStream.WritePositiveIntMax7(0);
            bitStream.WriteBoolean(ShieldTicks > 0); // gold shield
            bitStream.WritePositiveInt(VisibilityState, 2);
            bitStream.WriteBoolean(IsInvisible); // invisible "effect"
            bitStream.WritePositiveInt(0, 9);// damage effect angle
            bitStream.WriteBoolean(false);//invisible
            bitStream.WriteBoolean(false);//?
            bitStream.WriteBoolean(false);//v53
            if (bitStream.WriteBoolean(false)) bitStream.WriteIntMax15(15);//willow
            bitStream.WriteBoolean(false);//?
            if (IsInRealm)
            {
                bitStream.WritePositiveIntMax255(0);
                bitStream.WritePositiveIntMax255(0);
            }
            if (isOwnObject)
            {
                if (GetUltimateSkill()?.SeemToBeActive ?? false)
                {
                    bitStream.WriteBoolean(true);
                    bitStream.WriteIntMax1023(1 - CharacterData.Speed);
                }
                else if (bitStream.WriteBoolean(GetBuffedSpeed() != 0)) bitStream.WriteIntMax1023(GetBuffedSpeed());
                bitStream.WriteBoolean(YellowEye);//Getting seen
            }
            if (DamageNumbers.Count <= 0)
            {
                bitStream.WritePositiveIntMax31(0);
            }
            else
            {
                int v71 = 0;
                foreach (DamageNumber damageNumber in DamageNumbers)
                {
                    if (damageNumber.Delay == 0 && (damageNumber.Dealer == OwnObjectIndex || GetIndex() == OwnObjectIndex)) v71++;
                }
                bitStream.WritePositiveIntMax31(v71);
                foreach (DamageNumber damageNumber in DamageNumbers)
                {
                    if (damageNumber.Delay == 0 && (damageNumber.Dealer == OwnObjectIndex || GetIndex() == OwnObjectIndex))
                    {
                        bitStream.WriteIntMax32767(LogicMath.Clamp(damageNumber.Value, -32767, 32767));
                    }
                }
            }

            for (int i = 0; i < LogicMath.Min(2, m_skills.Count); i++)
            {
                m_skills[i].Encode(bitStream, isOwnObject, this);
            }
            bitStream.WritePositiveIntMax3(0);
        }

        public override int GetObjectType()
        {
            return 0;
        }
    }
}
