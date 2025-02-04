namespace Supercell.Laser.Logic.Battle.Component
{
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;
    using System.Security.Cryptography;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Logic.Home.Items;
    using System.Reflection.Emit;
    using Supercell.Laser.Logic.Battle.Structures;
    using System.Collections.Specialized;

    public class Skill
    {
        public int SkillId;
        public SkillData SkillData => DataTables.Get(DataType.Skill).GetDataByGlobalId<SkillData>(SkillId);

        public bool OnActivate = false;
        public bool IsUltiSkill;
        public int stufff;
        public int MaxCharge;
        public int Charge;
        public int RechargeModifier;
        public int Level = 11;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int TicksActive;
        public int MaxTicksActive;
        public int CoolDown;

        public bool Infinity;

        public bool SeemToBeActive;

        public bool IsFromAutoAttack;

        public Skill(int globalId, bool isUltiSkill)
        {
            SkillId = globalId;
            IsUltiSkill = isUltiSkill;

            int maxChargeCount = SkillData.MaxCharge;
            MaxCharge = LogicMath.Max(1000, 1000 * maxChargeCount);
            Charge = MaxCharge;

            TicksActive = 0;
        }

        public bool IsActive
        {
            get
            {
                return TicksActive > 0;
            }
        }

        public bool ShouldEndThisTick
        {
            get
            {
                return TicksActive == 50;
            }
        }
        public void AddCharge(Character a2, int a3)
        {
            Charge += a3 * 10;
            Charge = LogicMath.Min(Charge, 1000 * (SkillData.MaxCharge + (a2.GetCardValueForPassive("extra_bullet", 1) == -1 ? 0 : a2.GetCardValueForPassive("extra_bullet", 1))));
            if (Charge < 0) Charge = 0;
        }
        public bool HasEnoughCharge()
        {
            return Charge >= 1000;
        }
        public bool HasFullCharge(Character owner = null)
        {
            return Charge == MaxCharge;
        }
        public void Interrupt(Character character)
        {
            if ((SkillData.BehaviorType == "Attack" || SkillData.BehaviorType == "Ww") && TicksActive >= 1)
            {
                TicksActive = 0;
                Skill weapon = character.GetWeaponSkill();
                if (weapon != null) weapon.CoolDown = weapon.SkillData.Cooldown;
                Skill ulti = character.GetUltimateSkill();
                if (ulti != null) ulti.CoolDown = ulti.SkillData.Cooldown;
                if (SkillData.BehaviorType == "Attack") character.EndRapidFire();
            }
        }

        public void Tick(Character Owner = null)
        {
            if (Infinity || (Owner.GetPlayer()?.IsAdmin ?? false))
            {
                CoolDown = 0;
                Charge = MaxCharge;
            }
            int v10 = 0;
            int v24;
            Skill v9;
            if (SkillData.ChargeType == 15)
            {
                X = Owner.GetX();
                Y = Owner.GetY();
                Owner.AttackTarget.Set(X, Y);
            }
            if (!OnActivate) goto LABEL_83;
            if (SkillData.ChargeType == 9)
            {
                Y += 50;
            }
            if (Owner.CastingTime > 0) return;
            if (Owner.GetUltimateSkill() == this)
            {
                v24 = 2;
                Owner.UltiSkill = true;
                Owner.UltiUsed();
                if (Owner.CharacterData.HasPowerLevels)
                {
                    BattlePlayer v17 = Owner.GetPlayer();
                    if (v17 != null)
                    {
                        if (v17.PowerLevel <= 2) v17.PowerLevel++;
                    }
                }
                if (!(SkillData.BehaviorType == "Attack" || SkillData.BehaviorType == "Charge" || SkillData.BehaviorType == "Ww")) goto LABEL_17;

                v10 = TicksActive + SkillData.Cooldown;
                Owner.GetWeaponSkill().CoolDown = TicksActive + Owner.GetWeaponSkill().SkillData.Cooldown;
                v9 = Owner.m_skills[1];


            }
            else
            {
                Owner.UltiSkill = false;
                v9 = Owner.m_skills[0];
                v10 = TicksActive + SkillData.Cooldown;
                v24 = 1;
            }
            v9.CoolDown = v10;
            if (v9.CoolDown >= 50) v9.CoolDown -= 50;
            LABEL_17:

            switch (SkillData.BehaviorType)
            {
                case "Attack":
                    Owner.SpawnedItem = null;
                    Owner.SummonedCharacter = null;
                    if (IsFromAutoAttack && DataTables.GetProjectileByName(SkillData.Projectile)?.ConsumableShield >= 1)
                    {
                        Owner.AddConsumableShield(DataTables.GetProjectileByName(SkillData.Projectile).ConsumableShield * (100 + (Level - 1) * 10) / 100, DataTables.GetProjectileByName(SkillData.Projectile).ConsumableShieldTicks);
                        OnActivate = false;
                        TicksActive = 0;
                        MaxTicksActive = 0;
                        break;
                    }
                    if (SkillData.SpawnedItem != null)
                    {
                        if (Owner.CharacterData.UniqueProperty != 10 || Owner.ChargedShotCount >= Owner.GetMaxChargedShots())
                            Owner.SetItemSummoningVariables(DataTables.GetItemByName(SkillData.SpawnedItem), SkillData.NumSpawns, SkillData.MaxSpawns, SkillData.Damage * (100 + (Level - 1) * 10) / 100, SkillData.Damage);
                    }
                    else if (SkillData.SummonedCharacter != null)
                    {
                        CharacterData characterData = DataTables.GetCharacterByName(SkillData.SummonedCharacter);
                        int dmg = 0;
                        if (characterData.WeaponSkill != null)
                        {

                        }
                        else
                        {
                            if(characterData.AutoAttackDamage == 0)
                            {
                                if (DataTables.GetAreaEffectByName(characterData.AreaEffect) != null)
                                    dmg = LogicMath.Abs(DataTables.GetAreaEffectByName(characterData.AreaEffect).Damage);
                            }
                            else
                            {
                                dmg = characterData.AutoAttackDamage;
                            }
                            dmg = dmg * ((Level - 1) * 10) / 100;
                            if (characterData.AreaEffect != null && DataTables.GetAreaEffectByName(characterData.AreaEffect).Type == "DamageBoost") dmg = 0;
                        }
                        Owner.SetCharacterSummoningVariables(characterData, SkillData.NumSpawns,
                            SkillData.MaxSpawns,
                            dmg,
                            characterData.Hitpoints * ((Level - 1) * 10) / 100 + Owner.GetGearBoost(7));
                    }
                    int range = SkillData.CastingRange + GamePlayUtil.GetRangeAddSubtiles(Owner, false, Owner.GetPlayer(), 0, IsUltiSkill);
                    int spread = GetSpreadFromHold(Owner);
                    int activeTime = SkillData.ActiveTime;
                    if (!IsUltiSkill && Owner.GetCardValueForPassive("decrease_unload_time", 1) >= 1) activeTime -= activeTime * Owner.GetCardValueForPassive("decrease_unload_time", 1) / 100;
                    int nbioa = SkillData.NumBulletsInOneAttack;
                    if (SkillData.AttackPattern == 14) nbioa = Owner.JesterSkillCount + 1;
                    Owner.TriggerRapidFire(X, Y, SkillData.AttackPattern, nbioa, SkillData.MsBetweenAttacks, DataTables.Get(DataType.Projectile).GetData<ProjectileData>(SkillData.Projectile), SkillData.Damage * (100 + (Level - 1) * 10) / 100, range, spread, activeTime, SkillData.Damage, SkillData.ExecuteFirstAttackImmediately);
                    if (SkillData.ExecuteFirstAttackImmediately)
                    {
                        Owner.TickRapidFire();
                        //Owner.LastAttckTick--;
                    }




                    if (SkillData.AreaEffectObject != null)
                    {
                        int v117 = SkillData.Damage;
                        if (v117 <= 0) v117 = DataTables.GetAreaEffectByName(SkillData.AreaEffectObject).Damage;
                        Owner.AddAreaEffect(v117 * ((Level - 1) * 10) / 100, 0, DataTables.GetAreaEffectByName(SkillData.AreaEffectObject), v24, false);
                    }
                    OnActivate = false;
                    break;
                case "Charge":
                    int v55 = SkillData.ChargeSpeed;
                    if (Owner.IsChargeUpReady())
                    {
                        if (Owner.CharacterData.UniqueProperty == 3)
                        {
                            v55 = (100 * Owner.CharacterData.UniquePropertyValue2 / SkillData.CastingRange + 100) * v55 / 100;
                        }
                    }
                    if (SkillData.ChargeType == 16)
                    {
                        Owner = Owner.TriggerTransforming(DataTables.GetCharacterByName(SkillData.SummonedCharacter));
                    }
                    Owner.TriggerCharge(X, Y, v55, SkillData.ChargeType, SkillData.Damage * (100 + (Level - 1) * 10) / 100, SkillData.ChargePushback, SkillData.ChargeType == 10, DataTables.GetAreaEffectByName(SkillData.AreaEffectObject));
                    if (SkillData.PercentDamage >= 1) Owner.ChargePercentDamage = SkillData.PercentDamage;
                    Owner.ChargeNormalDMG = SkillData.Damage;
                    OnActivate = false;
                    break;
                case "Ww":
                    Owner.TriggerWhirlwind(SkillData.Damage * (100 + (Level - 1) * 10) / 100, SkillData.Damage, SkillData.MsBetweenAttacks);
                    OnActivate = false;
                    break;
                case "AreaBuff":
                    Owner.AddAreaEffect(SkillData.Damage * ((Level - 1) * 10) / 100, 0, DataTables.GetAreaEffectByName(SkillData.AreaEffectObject), v24, false);
                    OnActivate = false;
                    break;
                case "ChangeCharacter":
                    Owner.GetPlayer().OwnObjectId =
                        Owner.TriggerTransforming(DataTables.GetCharacterByName(SkillData.SummonedCharacter)).GetGlobalID();
                    OnActivate = false;
                    break;
                case "ProjectileShield":
                    Owner.ProjectileShieldAngle = (int)GamePlayUtil.RadToDeg(GamePlayUtil.WeaponSpreadToAngleRad(SkillData.Spread));
                    Owner.ProjectileShieldScale = SkillData.CastingRange * 100;
                    OnActivate = false;
                    Owner.SetForcedAngle(LogicMath.GetAngle(X - Owner.GetX(), Y - Owner.GetY()));
                    //Owner.AttackAngle = LogicMath.GetAngle(X - Owner.GetX(), Y - Owner.GetY());
                    break;
                default:
                    OnActivate = false;
                    break;
            }
        LABEL_83:



            //    if (ZNK14LogicSkillData12getMaxChargeEv(a1->dword0) >= 1)
            //        a1->Charges -= 1000 * *(&a1[1].ActiveTime + 3);
            //    if (v34 <= 0xA)
            //        __asm { ADD PC, R0, R1 }
            //    a3 = v33;
            //    a1->unsigned___int81C = 0;
            //LABEL_22:
            //    v17 = *a1->gap10;
            //    v18 = a1->ActiveTime;
            //    v19 = v17 < 1;
            //    if (v17 >= 1)
            //        v17 -= a3;
            //    a1->ActiveTime = (v18 - a3) & ~((v18 - a3) >> 31);
            //    if (!v19)
            //        *a1->gap10 = v17 & ~(v17 >> 31);
            int v18 = TicksActive;
            TicksActive = (TicksActive - 50) & ~((TicksActive - 50) >> 31);

            if (CoolDown >= 1)
            {
                int v20 = CoolDown - 50;
                CoolDown = v20;
                if (v20 <= 0)
                {
                    //v21 = a1->dword0;
                    CoolDown = 0;
                    //if (*(v21 + 140))
                    //    *a1->gap10 = 500;
                }
            }
            //v22 = sub_3DCFA0(a1->dword0);
            //v23 = GetCardValueForPassive(a2, 58, 1);
            //unsigned___int81ED = a2->unsigned___int81ED;
            //if (v23 > 0)
            //    v22 -= v23;
            //v25 = *(ZNK21LogicGameObjectServer24getLogicBattleModeServerEv() + 164);
            //if (unsigned___int81ED)
            //    v22 /= 2;
            //result = sub_41EC8C(v25, a2);
            //if (result)
            //{
            //    v26 = sub_26DFCC();
            //    result = a1_divide_a2(10 * v22, *(v26 + 156));
            //    v22 = result;
            //}
            //if (v22 >= 1)
            //{
            //    result = a1->CoolDown;
            //    v27 = result <= 0;
            //    if (result <= 0)
            //    {
            //        result = *a1->gap10;
            //        v27 = result <= 0;
            //    }
            //    if (v27)
            //    {
            //        v28 = ZNK14LogicSkillData12getMaxChargeEv(a1->dword0);
            //        v29 = GetCardValueForPassive(a2, 48, 1);
            //        if (v29 > 0)
            //            v28 += v29;
            //        v30 = LogicMath.Max(1000, 1000 * v28);
            //        v31 = a1_divide_a2(1000 * a3, v22);
            //        a1->Charges += v31;
            //        v32 = *(ZNK21LogicGameObjectServer24getLogicBattleModeServerEv() + 582) == 0;
            //        result = a1->Charges;
            //        if (!v32)
            //        {
            //            result = (result + 15 * v31);
            //            a1->Charges = result;
            //        }
            //        if (result > v30)
            //            a1->Charges = v30;
            //    }
            //}
            if (Charge < MaxCharge && CoolDown <= 0 && SkillData.MaxCharge >= 1)
            {
                int v22 = 1000 / (SkillData.RechargeTime / 50);
                v22 += v22 * Owner.GetGearBoost(5) / 100;
                if (Owner.ReloadBuffTicks > 0) v22 *= 2;
                Charge = LogicMath.Min(MaxCharge, Charge + v22);
            }
            if (v18 >= 1)
            {
                //result = v34;
                //if (!v34)
                //{
                if (TicksActive <= 0)
                    Owner.EndRapidFire();
                //}

            }
            if (TicksActive <= 0) MaxTicksActive = 0;
            //Debugger.Print("TA:" + TicksActive);

        }


        public void Activate(Character character, int x, int y, TileMap tileMap)
        {
            //Debugger.Print(TicksActive);
            //Debugger.Print(CoolDown);
            if (Charge < 1000 && SkillData.MaxCharge != 0) return;
            if (TicksActive > 0 || CoolDown > 0) return;

            Charge -= 1000;

            character.InterruptAllSkills(true);
            TicksActive = SkillData.ActiveTime;
            if (character.Accessory != null)
            {
                if (!IsUltiSkill && character.Accessory.IsActive && character.Accessory.Type == "next_attack_change" && character.Accessory.AccessoryData.SubType == 2 && character.Accessory.AccessoryData.CustomValue2 >= 1)
                {
                    TicksActive = character.Accessory.AccessoryData.CustomValue2;
                }
            }
            if (IsUltiSkill && character.GetCardValueForPassive("more_bullets_ulti", 1) >= 1)
            {
                TicksActive = character.GetCardValueForPassive("more_bullets_ulti", 1);
            }
            if (!IsUltiSkill && character.GetCardValueForPassive("decrease_unload_time", 1) >= 1) TicksActive -= TicksActive * character.GetCardValueForPassive("decrease_unload_time", 1) / 100;
            if (IsUltiSkill && character.GetGearBoost(11) >= 1) TicksActive += character.GetGearBoost(11) * 20;
            int v32 = GamePlayUtil.GetRangeAddSubtiles(character, false, character.GetPlayer(), 0, IsUltiSkill);
            LogicVector2 v33 = new LogicVector2();
            GetSkillCastPos(character.GetX(), character.GetY(), x, y, tileMap, SkillData, false, false, v33, v32, false, 0, character.GetPlayer());
            X = v33.X;
            Y = v33.Y;
            OnActivate = true;
            MaxTicksActive = TicksActive;
            character.BlockHealthRegen();
            character.CastingTime = SkillData.CastingTime - character.SkillHoldTicks * 50;
            if (SkillData.ExecuteFirstAttackImmediately) Tick(character);

        }
        public static void GetSkillCastPos(int a1, int a2, int a3, int a4, TileMap a5, SkillData a6, bool a7, bool a8, LogicVector2 a9, int a10, bool a11, int a12, BattlePlayer a13)
        {
            int v20; // w22
            int v21; // w24
            int v22; // w19
            int v23; // w25
            int v24; // w0
            int v28; // w25
            int v29; // w21
            int v30; // w24
            int v31; // w19
            int v32; // w8
            int v33; // w20
            int v34; // w0
            int v35; // w8
            string v37; // w19
            int v38; // w8
            int v39; // w21 MAPDST
            int v40; // w0
            int v41; // w24 MAPDST
            int v42; // w0
            int v43; // w19 MAPDST
            int v44; // w0
            int v45; // w23 MAPDST
            int v46; // w0
            int v47; // w9 MAPDST
            int v48; // w24
            int v49; // w23
            int v50; // w8
            int v51; // w24
            int v52; // w13
            int v54; // w25
            int v55; // w26
            int v56; // w12
            int v57; // w10
            //_QWORD* v58; // x0
            //__int64 v59; // x19
            //_QWORD* v60; // x23
            int v61; // w28
            int v62; // w20
            //_BOOL4 v63; // w24
            //__int64 v64; // x19
            //__int64 v65; // x19
            int v66; // w21
            //_BOOL4 v67; // w27
            int v68; // w20
            //__int64 v69; // x19
            char v70; // w0
            //_BOOL4 v71; // w8
            char v72; // w0
            int v73; // w23
            int v74; // w19
            int v75; // w20
            int v76; // w0
            int v77; // w19
            int v78; // w0
            int v79; // w8
            int v81; // w0
            int v82; // w19
            int v83; // w22
            int v84; // w25
            int v85; // w23
            int v86; // w20
            int v87; // w19
            //signed int v88; // w22
            //_DWORD* v89; // x28
            int v90; // w0
            int v91; // w23
            int v92; // w22
            int v93; // w25
            int v94; // w26
            int v95; // w27
            int v96; // w24
            //signed int v97; // w8
            int v98; // w9
            bool v99; // cc
            int v100; // w8
            int v101; // w8
            int v102; // w24
            int v103; // w8
            int v104; // w8
            bool v105; // cc
            int v106; // w8
            int v107; // w8
            int v109; // w19
            //__int64 v110; // x0
            int v112; // [xsp+18h] [xbp-E8h]
            //signed int v113; // [xsp+1Ch] [xbp-E4h]
            int v114; // [xsp+20h] [xbp-E0h]
            int v115; // [xsp+20h] [xbp-E0h]
            //signed int v116; // [xsp+24h] [xbp-DCh]
            int v117; // [xsp+30h] [xbp-D0h]
            int v118; // [xsp+34h] [xbp-CCh]
            int v119; // [xsp+38h] [xbp-C8h]
            bool v123; // [xsp+4Ch] [xbp-B4h]
            bool v127; // [xsp+64h] [xbp-9Ch]
            char v129; // [xsp+6Ch] [xbp-94h]
            int v130; // [xsp+70h] [xbp-90h]
            int v132; // [xsp+74h] [xbp-8Ch]
            bool v134; // [xsp+78h] [xbp-88h]
            int v135; // [xsp+84h] [xbp-7Ch]
            int v136; // [xsp+84h] [xbp-7Ch]
            int v138; // [xsp+88h] [xbp-78h]
            int v139; // [xsp+8Ch] [xbp-74h]
            int v140; // [xsp+8Ch] [xbp-74h]
            int v141; // [xsp+90h] [xbp-70h] BYREF
            int v142; // [xsp+94h] [xbp-6Ch]
            LogicVector2 v143; // [xsp+98h] [xbp-68h] BYREF
            int v144; // [xsp+A0h] [xbp-60h]

            if (!a6.AllowAimOutsideMap)
            {
                a3 = LogicMath.Clamp(a3, 1, (a5.LogicWidth - 2));
                a4 = LogicMath.Clamp(a4, 1, (a5.LogicHeight - 2));
            }
            v20 = a6.CastingRange;
            if (v20 >= 1)
            {
                v21 = a3 - a1;
                v22 = a4 - a2;
                v23 = 100 * (v20 + a10);
                v24 = LogicMath.Sqrt((v22 * v22 + v21 * v21));
                if (v24 > v23)
                {
                    v21 = v23 * v21 / v24;
                    v22 = v23 * v22 / v24;
                }
                a3 = v21 + a1;
                a4 = v22 + a2;
            }
            //goto LABEL_160;

            if ((a6.AlwaysCastAtMaxRange && !a7) || a8)
            {
                v28 = a3 - a1;
                v29 = a1 - a3;
                v30 = a4 - a2;
                v31 = a2 - a4;
                if (a12 == 2)
                    v32 = 300;
                else
                    v32 = 200;
                if (a8)
                    v33 = 0;
                else
                    v33 = v32;
                v34 = LogicMath.Sqrt((v30 * v30 + v28 * v28));
                if (v34 > 0)
                {
                    v35 = 100 * (v20 + a10);
                    v29 = v33 * v29 / v34;
                    v28 = v28 * v35 / v34;
                    v30 = v30 * v35 / v34;
                    v31 = v33 * v31 / v34;
                }
                a3 = v28 + a1;
                a4 = v30 + a2;
                if (!a6.AllowAimOutsideMap)
                {
                    v143 = new LogicVector2();
                    if (GamePlayUtil.LineSegmentIntersectslineSegment(
                            a1,
                            a2,
                            a3,
                            a4,
                            1,
                            1,
                            (a5.LogicWidth - 2),
                            1,
                            v143)
                      || GamePlayUtil.LineSegmentIntersectslineSegment(
                            a1,
                            a2,
                            a3,
                            a4,
                            1,
                            1,
                            1,
                            (a5.LogicHeight - 2),
                            v143)
                      || GamePlayUtil.LineSegmentIntersectslineSegment(
                            a1,
                            a2,
                            a3,
                            a4,
                            (a5.LogicWidth - 2),
                            (a5.LogicHeight - 2),
                            1,
                            (a5.LogicHeight - 2),
                            v143)
                      || GamePlayUtil.LineSegmentIntersectslineSegment(
                            a1,
                            a2,
                            a3,
                            a4,
                            (a5.LogicWidth - 2),
                            (a5.LogicHeight - 2),
                            (a5.LogicWidth - 2),
                            1,
                            v143))
                    {
                        a3 = v143.X + v29;
                        a4 = v143.Y + v31;
                    }
                }
            }
            if (!a6.AllowAimOutsideMap)
            {
                a3 = LogicMath.Clamp(a3, 1, (a5.LogicWidth - 2));
                a4 = LogicMath.Clamp(a4, 1, (a5.LogicHeight - 2));
            }
            v37 = a6.BehaviorType;
            if (v37 == "Ww" || v37 == "Charge")
            {
                a3 = LogicMath.Clamp(a3, 101, (a5.LogicWidth - 102));
                a4 = LogicMath.Clamp(a4, 101, (a5.LogicHeight - 102));
            }
            else
            {
                if (a11) goto LABEL_160;
            }
            if (a6.ChargeType == 1
              || a6.ChargeType == 4
              || a6.ChargeType == 8)
            {
                v38 = 0;
                goto LABEL_42;
            }
            if (a6.ChargeType != 10)
            {
            LABEL_37:
                if (a11)
                    goto LABEL_160;
            }
            if (v37 != "Attack")
            {
            LABEL_41:
                v38 = 0;
                goto LABEL_42;
            }
            if (a6.SummonedCharacter != null)
                v38 = 1;
            else
                if (a6.SpawnedItem != null)
            {
                v38 = 1;
            }
            else v38 = 0;
            LABEL_42:
            v39 = a6.ForceValidTile;
            if (v39 > 0)
            {
                if (v39 < 1)
                    goto LABEL_160;
                goto LABEL_44;
            }
            {
                if (v37 == "Ww" || v37 == "Charge")
                {
                    v50 = a6.ChargeType - 1;
                    if (v50 > 0xB)
                    {
                        v39 = 2;
                        if (a13 == null)
                        {
                            if (v39 < 1)
                                goto LABEL_160;
                            goto LABEL_44;
                        }
                    }
                    else
                    {
                        //v39 = switch_table__ZN16LogicSkillServer15getSkillCastPosEiiiiPK12LogicTileMapPK14LogicSkillDatabbP12LogicVector2ibiP11LogicPlayer_44838203_a45f_46c9_9ec2_b0f70bb8a77f_11924[v50];
                        if (a13 == null)
                        {
                        LABEL_43:
                            if (v39 < 1)
                                goto LABEL_160;
                            goto LABEL_44;
                        }
                    }
                    //if (*(a13 + 80) == 2)
                    //    v39 = 7;
                    if (v39 < 1)
                        goto LABEL_160;
                    goto LABEL_44;
                }
                if (v38 == 0)
                    goto LABEL_160;
            }
        //    v39 = 1;
        LABEL_44:
            //    if (v39 == 99)
            goto LABEL_160;
        //    v130 = 999999999;
        //    v116 = ZN12LogicTileMap21logicToPathFinderTileEi(a1);
        //    v113 = ZN12LogicTileMap21logicToPathFinderTileEi(a2);
        //    v114 = ZN12LogicTileMap21logicToPathFinderTileEi(a3);
        //    v127 = v39 == 4;
        //    v123 = v39 == 6;
        //    v112 = ZN12LogicTileMap21logicToPathFinderTileEi(a4);
        //    if (v39 > 7 || ((1 << v39) & 0xD8) == 0)
        //        goto LABEL_51;
        //    ZN12LogicVector2C2Ev(&v141);
        //    v40 = LogicMath.Min((v116 / 3), (v114 / 3));
        //    v41 = LogicMath.Clamp((v40 - 1), 0LL, (a5[36] - 1));
        //    v42 = LogicMath.Max((v116 / 3), (v114 / 3));
        //    v43 = LogicMath.Clamp((v42 + 1), 0LL, (a5[36] - 1));
        //    v44 = LogicMath.Min((v113 / 3), (v112 / 3));
        //    v45 = LogicMath.Clamp((v44 - 1), 0LL, (a5[37] - 1));
        //    v46 = LogicMath.Max((v113 / 3), (v112 / 3));
        //    v135 = LogicMath.Clamp((v46 + 1), 0LL, (a5[37] - 1));
        //    v47 = a12 == 2 ? 200 : 100;
        //    if (v41 > v43)
        //    {
        //    LABEL_51:
        //        if (v39 != 3)
        //            goto LABEL_112;
        //        goto LABEL_52;
        //    }
        //    v119 = 2 * v47 + 300;
        //    v51 = -1;
        //    v52 = -1;
        //    v129 = 0;
        //    v117 = v47 + 300 * v45 + 300;
        //    v118 = 300 * v45 - v47;
        //    do
        //    {
        //        if (v45 <= v135)
        //        {
        //            v54 = v117;
        //            v55 = v118;
        //            while (1)
        //            {
        //                v139 = v52;
        //                v58 = ZNK12LogicTileMap7getTileEii(a5, v41, v45);
        //                v59 = *v58;
        //                v60 = v58;
        //                if ((ZNK13LogicTileData14blocksMovementEv(*v58) & 1) != 0
        //                  && (ZNK13LogicTileData17blocksProjectilesEv(v59) & 1) != 0)
        //                {
        //                    v61 = v51;
        //                    v62 = ZNK13LogicTileData26isDestructibleWithPiercingEv(v59) ^ 1;
        //                }
        //                else
        //                {
        //                    v61 = v51;
        //                    v62 = 0;
        //                }
        //                v63 = v39 == 3;
        //                if (v39 != 7)
        //                    break;
        //                v64 = *v60;
        //                if ((ZNK13LogicTileData14blocksMovementEv(*v60) & 1) != 0
        //                  && (ZNK13LogicTileData17blocksProjectilesEv(v64) & 1) != 0
        //                  && (ZNK13LogicTileData26isDestructibleWithPiercingEv(v64) & 1) == 0)
        //                {
        //                    v66 = 1;
        //                }
        //                else
        //                {
        //                    v65 = *v60;
        //                    if ((ZNK13LogicTileData14blocksMovementEv(*v60) & 1) == 0
        //                      || (ZNK13LogicTileData17blocksProjectilesEv(v65) & 1) != 0)
        //                    {
        //                        break;
        //                    }
        //                    v66 = ZNK13LogicTileData26isDestructibleWithPiercingEv(v65) ^ 1;
        //                }
        //            LABEL_85:
        //                if ((ZNK13LogicTileData14blocksMovementEv(*v60) & 1) == 0
        //                  || (v67 = v127, (ZNK13LogicTileData17isDestructibleAnyEv(*v60) & 1) != 0)
        //                  && ((ZNK13LogicTileData17isDestructibleAnyEv(*v60) & 1) == 0
        //                   || (ZNK13LogicTileData17blocksProjectilesEv(*v60) & 1) != 0
        //                   || (v67 = v127, (ZNK13LogicTileData14blocksMovementEv(*v60) & 1) != 0)))
        //                {
        //                    v67 = 0;
        //                }
        //                v68 = v63 & v62;
        //                if ((ZNK13LogicTileData14blocksMovementEv(*v60) & 1) != 0)
        //                {
        //                    v69 = *v60;
        //                    v51 = v61;
        //                    if ((ZNK13LogicTileData14blocksMovementEv(*v60) & 1) != 0
        //                      && (ZNK13LogicTileData17blocksProjectilesEv(v69) & 1) == 0
        //                      && (ZNK13LogicTileData26isDestructibleWithPiercingEv(v69) & 1) == 0
        //                      || (v70 = ZNK13LogicTileData17isDestructibleAnyEv(*v60), v71 = v123, (v70 & 1) != 0)
        //                      && ((ZNK13LogicTileData17isDestructibleAnyEv(*v60) & 1) == 0
        //                       || (ZNK13LogicTileData17blocksProjectilesEv(*v60) & 1) != 0
        //                       || (v72 = ZNK13LogicTileData14blocksMovementEv(*v60), v71 = v123, (v72 & 1) != 0)))
        //                    {
        //                        v71 = 0;
        //                    }
        //                }
        //                else
        //                {
        //                    v71 = 0;
        //                    v51 = v61;
        //                }
        //                if (((v66 | v68 | v67 | v71) & 1) != 0
        //                  && ZN17LogicGamePlayUtil30lineSegmentIntersectsRectangleEiiiiiiiiP12LogicVector2(
        //                       a1,
        //                       a2,
        //                       a3,
        //                       a4,
        //                       (300 * v41 - v47),
        //                       v55,
        //                       (v119 + 300 * v41 - v47),
        //                       v54,
        //                       &v141))
        //                {
        //                    v56 = v130;
        //                    v52 = v139;
        //                    v57 = (v141 - a1) * (v141 - a1) + (v142 - a2) * (v142 - a2);
        //                    if (v57 < v130)
        //                        v51 = v141;
        //                    if (v57 < v130)
        //                        v52 = v142;
        //                    if (v57 < v130)
        //                        v56 = (v141 - a1) * (v141 - a1) + (v142 - a2) * (v142 - a2);
        //                    v129 |= v57 < v130;
        //                    v130 = v56;
        //                }
        //                else
        //                {
        //                    v52 = v139;
        //                }
        //                v55 += 300;
        //                v54 += 300;
        //                v99 = v45++ < v135;
        //                if (!v99)
        //                    goto LABEL_60;
        //            }
        //            v66 = 0;
        //            goto LABEL_85;
        //        }
        //    LABEL_60:
        //        v99 = v41++ < v43;
        //    }
        //    while (v99);
        //    if ((v129 & 1) == 0)
        //    {
        //        if (v39 != 3)
        //            goto LABEL_112;
        //        goto LABEL_52;
        //    }
        //    v73 = v52;
        //    v74 = v52 - a2;
        //    v75 = v51 - a1;
        //    v76 = LogicMath.Sqrt(v74 * v74 + v75 * v75);
        //    if (v76)
        //    {
        //        a3 = ((v76 - 50) * v75) / v76 + a1;
        //        a4 = ((v76 - 50) * v74) / v76 + a2;
        //        if (v39 != 3)
        //            goto LABEL_112;
        //        LABEL_52:
        //        v48 = ZN12LogicTileMap21logicToPathFinderTileEi(a3);
        //        v49 = ZN12LogicTileMap21logicToPathFinderTileEi(a4);
        //        v134 = 0;
        //        v39 = 5;
        //    LABEL_114:
        //        if ((ZN16LogicSkillServer18blockedForCastTypeEiiiPK12LogicTileMapi(v48, v49, v39, a5, a12) & 1) != 0)
        //        {
        //            v77 = ZN12LogicTileMap21pathFinderTileToLogicEib(v48, 1LL) - a1;
        //            v78 = ZN12LogicTileMap21pathFinderTileToLogicEib(v49, 1LL);
        //            v79 = v77 * v77 + (v78 - a2) * (v78 - a2);
        //            if (v79 < v130)
        //            {
        //                v81 = LogicMath.Sqrt((v48 - v116) * (v48 - v116) + (v49 - v113) * (v49 - v113));
        //                v82 = v49 - (v81 + 4);
        //                v83 = v81 + 4 + v48;
        //                v84 = v49;
        //                v85 = v81 + 4 + v49;
        //                v86 = LogicMath.Clamp(v48 - (v81 + 4), 0LL, (a5[40] - 1));
        //                v87 = LogicMath.Clamp(v82, 0LL, (a5[41] - 1));
        //                v88 = LogicMath.Clamp(v83, 0LL, (a5[40] - 1));
        //                v89 = a5;
        //                v90 = LogicMath.Clamp(v85, 0LL, (a5[41] - 1));
        //                v132 = v88;
        //                v140 = -1;
        //                if (v86 > v88)
        //                {
        //                    v138 = -1;
        //                }
        //                else
        //                {
        //                    v91 = v90;
        //                    v138 = -1;
        //                    if (v87 <= v90)
        //                    {
        //                        v92 = 0x7FFFFFFF;
        //                        v115 = v48;
        //                        v136 = -v84;
        //                        v93 = 0x7FFFFFFF;
        //                        v138 = -1;
        //                        v140 = -1;
        //                        do
        //                        {
        //                            v94 = (v86 - v48) * (v86 - v48);
        //                            v95 = (v86 - v116) * (v86 - v116);
        //                            if (v134)
        //                            {
        //                                v96 = v87;
        //                                do
        //                                {
        //                                    if ((ZN16LogicSkillServer18blockedForCastTypeEiiiPK12LogicTileMapi(v86, v96, v39, v89, a12) & 1) == 0)
        //                                    {
        //                                        v97 = v95 + (v96 - v113) * (v96 - v113);
        //                                        if (v97 >= 9)
        //                                        {
        //                                            v98 = v94 + (v136 + v96) * (v136 + v96);
        //                                            if (v98 < v92)
        //                                            {
        //                                                v92 = v94 + (v136 + v96) * (v136 + v96);
        //                                                v93 = v95 + (v96 - v113) * (v96 - v113);
        //                                                v138 = v96;
        //                                                v140 = v86;
        //                                            }
        //                                            else if (v98 == v92)
        //                                            {
        //                                                v99 = v97 < v93;
        //                                                if (v97 < v93)
        //                                                    v93 = v95 + (v96 - v113) * (v96 - v113);
        //                                                v100 = v140;
        //                                                if (v99)
        //                                                    v100 = v86;
        //                                                v140 = v100;
        //                                                if (v99)
        //                                                    v101 = v96;
        //                                                else
        //                                                    v101 = v138;
        //                                                v138 = v101;
        //                                            }
        //                                        }
        //                                    }
        //                                    v99 = v96++ < v91;
        //                                }
        //                                while (v99);
        //                            }
        //                            else
        //                            {
        //                                v102 = v87;
        //                                do
        //                                {
        //                                    if ((ZN16LogicSkillServer18blockedForCastTypeEiiiPK12LogicTileMapi(v86, v102, v39, v89, a12) & 1) == 0)
        //                                    {
        //                                        v103 = v94 + (v136 + v102) * (v136 + v102);
        //                                        if (v103 < v92)
        //                                        {
        //                                            v93 = v95 + (v102 - v113) * (v102 - v113);
        //                                            v92 = v94 + (v136 + v102) * (v136 + v102);
        //                                            v138 = v102;
        //                                            v140 = v86;
        //                                        }
        //                                        else if (v103 == v92)
        //                                        {
        //                                            v104 = v95 + (v102 - v113) * (v102 - v113);
        //                                            v105 = v104 < v93;
        //                                            if (v104 < v93)
        //                                                v93 = v95 + (v102 - v113) * (v102 - v113);
        //                                            v106 = v140;
        //                                            if (v105)
        //                                                v106 = v86;
        //                                            v140 = v106;
        //                                            if (v105)
        //                                                v107 = v102;
        //                                            else
        //                                                v107 = v138;
        //                                            v138 = v107;
        //                                        }
        //                                    }
        //                                    v99 = v102++ < v91;
        //                                }
        //                                while (v99);
        //                            }
        //                            v48 = v115;
        //                            v99 = v86++ < v132;
        //                        }
        //                        while (v99);
        //                    }
        //                }
        //                if (v140 == -1)
        //                    v109 = v113;
        //                else
        //                    v109 = v138;
        //                if (v140 == -1)
        //                    v110 = v116;
        //                else
        //                    v110 = v140;
        //                a3 = ZN12LogicTileMap21pathFinderTileToLogicEib(v110, 1LL);
        //                a5 = v89;
        //                a4 = ZN12LogicTileMap21pathFinderTileToLogicEib(v109, 1LL);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        a4 = v73;
        //        a3 = v51;
        //        if (v39 == 3)
        //            goto LABEL_52;
        //        LABEL_112:
        //        v134 = v39 == 1;
        //        if ((v39 & 0xFFFFFFFB) - 1 <= 1)
        //        {
        //            v48 = v114;
        //            v49 = v112;
        //            goto LABEL_114;
        //        }
        //    }
        LABEL_160:
            if (!a6.AllowAimOutsideMap)
            {
                a3 = LogicMath.Clamp(a3, 1, (a5.LogicWidth - 2));
                a4 = LogicMath.Clamp(a4, 1, (a5.LogicHeight - 2));
            }
            a9.Set(a3, a4);
        }

        public void Encode(BitStream bitStream,bool IsOwn,Character Owner)
        {
            bitStream.WritePositiveVIntMax255OftenZero(TicksActive / 50); // 0x15a048
            bitStream.WritePositiveVIntMax255OftenZero(MaxTicksActive / 50); // 0x15a048
            bitStream.WriteBoolean(OnActivate); // 没用
            bitStream.WriteBoolean(false); // ?
            bitStream.WritePositiveVIntMax255OftenZero(CoolDown / 50); // 灰色普攻条（不能攻击）
            //Debugger.Print(CoolDown);
            if (SkillData.MaxCharge >= 1)
            {
                int v5 = 20;
                if (!SkillData.HoldToShoot) v5 = 1;
                bitStream.WritePositiveIntMax4095(Charge / v5); // 0x15a09c
            }

            if (SkillData.SkillChangeType !=0)
            {
                int instanceId = SkillData.GetInstanceId();
                bitStream.WritePositiveIntMax255(instanceId);
            }
            if (Owner.CharacterData.UniqueProperty == 18) bitStream.WriteBoolean(false);//帅otto
        }

        public int GetSpreadFromHold(Character a2)
        {
            if (SkillData.AttackPattern == 14)
            {
                return SkillData.Spread * a2.JesterSkillCount;
            }
            if (SkillData.AttackPattern != 13)
                return SkillData.Spread;
            return SkillData.Spread - a2.GetSkillHoldedTicks() * 130 / a2.GetMaxHoldTicks();
        }

        public int GetMaxCharge()
        {
            return MaxCharge;
        }

        public bool HasButton()
        {
            return true;
        }

        public bool IsOkToReduceCooldown()
        {
            return true;
        }

        public int MaxCooldown()
        {
            return SkillData.Cooldown;
        }
    }
}
