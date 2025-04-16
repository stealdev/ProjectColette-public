﻿namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class Item : GameObject
    {


        public ItemData ItemData;


        public int SpawningTick;
        public int PickUpTimerDefault;
        public int PickUpTimer;
        public int PickUpStartX;
        public int PickUpStartY;
        public int PickUpZ;
        public int PickUpStartZ;
        public Character Picker;
        public bool PickedUp;
        public int MoveTimer;
        public int MoveTimerDefault;
        public int TargetX;
        public int TargetY;
        public int SpawnedTick;
        public int Damage;

        public Item(ItemData itemData) : base(itemData)
        {
            ItemData = itemData;
            SpawningTick = 1;
            PickUpTimerDefault = 250;
            MoveTimer = 750;
            MoveTimerDefault = 750;
        }

        public override void Tick()
        {
            if (ItemData.Name == "ButtonHold" || ItemData.Name == "ButtonPress")
            {
                foreach(Character character in GameObjectManager.GetCharacters())
                {
                    if (character.GetPosition().GetDistance(Position) <= 150)
                    {
                        GetBattle().StoryMode.DoorTest = true;
                        break;
                    }
                }
            }
            if ((ItemData.Name == "DoorHorizontal" || ItemData.Name == "DoorVertical")&& !GetBattle().StoryMode.DoorTest)
            {
                foreach (Character character in GameObjectManager.GetCharacters())
                {
                    if (character.GetPosition().GetDistance(Position) <= 150&&character==GetBattle().StoryMode.PlayerCharacter)
                    {
                        GetBattle().StoryMode.N("实际上，这门根本没有碰撞体积。\n不过看那两个<cFF0000>鍌婚€�</c>演得那么像，\n最好还是别打扰他们。");
                        break;
                    }
                }
            }
            if (ItemData.Name == "OrbSpawner")
            {
                if (SpawningTick == 1) SpawningTick = 40;
                if (TicksGone < SpawningTick) goto LABEL_235;
                Item v269 = GameObjectFactory.CreateGameObjectByData(DataTables.GetItemByName("Point"));
                v269.SetPosition(GetX(), GetY(), GetZ());
                v269.PickUpStartX = GetX();
                v269.PickUpStartY = GetY();
                v269.PickUpStartZ = 1000 * GetZ();
                int angle = GetBattle().GetRandomInt(360);
                v269.TargetX = GetX() + LogicMath.GetRotatedX(600, 0, angle);
                v269.TargetY = GetY() + LogicMath.GetRotatedY(600, 0, angle);
                GameObjectManager.AddGameObject(v269);
                SpawningTick = TicksGone + 140;
                SpawnedTick = TicksGone;
            }
            if (PickedUp)
            {
                if (PickUpTimer < 1) goto LABEL_235;
                if (Picker != null)
                {
                    if (!Picker.IsAlive())
                    {
                        PickedUp = false;
                        goto LABEL_235;
                    }
                }
                PickUpTimer -= 50;
                int v196 = LogicMath.Min(1000, 1000 - 1000 * PickUpTimer / PickUpTimerDefault);
                int v200 = PickUpStartZ + (v196 - 1000) * v196 * (-1) + (PickUpZ - PickUpStartZ) * v196 / 1000;
                SetPosition((Picker.GetX() * v196 + PickUpStartX * (1000 - v196)) / 1000,
                        (Picker.GetY() * v196 + PickUpStartY * (1000 - v196)) / 1000,
                        v200 / 1000);
                if (PickUpTimer <= 0)
                {
                    //if (ItemData.Name == "Point")
                    //{
                    if (Picker.IsAlive())
                    {
                        Picker.ApplyItem(this);
                    }
                    //}
                }
                goto LABEL_235;
            }
            MoveTimer -= 50;
            if (MoveTimer < 0) MoveTimer = 0;
            if (TargetX == 0) goto LABEL_263;
            int v187 = LogicMath.Min(1000, 1000 - 1000 * MoveTimer / MoveTimerDefault);
            if (ItemData.Name == "Point")
            {
                if (v187 > 700)
                {
                    int v186 = 700;
                    int v189 = 300;
                    int v206 = v187 - v186;
                    int v211 = TargetX - PickUpStartX;
                    int v212 = TargetY - PickUpStartY;
                    int v213 = (TargetX - PickUpStartX) * v186;
                    int v214 = (PickUpStartX * v189 + TargetX * v186);
                    int v215 = (PickUpZ + v206 * (-6) * (v206 - v189));
                    int v216 = (PickUpStartY * v189 + TargetY * v186);
                    int v217 = ((v214 / 1000) * (v189 - v206) + (v213 / 1000 + PickUpStartX + v211 * v189 / 2000) * v206) / v189;
                    int v218 = ((v216 / 1000) * (v189 - v206) + (v212 * v186 / 1000 + PickUpStartY + v212 * v189 / 2000) * v206) / v189;
                    int v219 = v215 / 1000;
                    SetPosition(v217, v218, LogicMath.Clamp(v219, 0, 3000));
                }
                else
                {
                    int v186 = 700;
                    int v206 = v187 - 700;
                    int v220 = (PickUpStartY * (1000 - v187) + TargetY * v187);
                    int v221 = (PickUpStartZ + (-6) * v187 * v206 + (PickUpZ - PickUpStartZ) * v187 / v186);
                    int v218 = v220 / 1000;
                    int v217 = (PickUpStartX * (1000 - v187) + TargetX * v187) / 1000;
                    int v219 = v221 / 1000;
                    SetPosition(v217, v218, LogicMath.Clamp(v219, 0, 3000));
                }
            }
        LABEL_263:
            ;
        LABEL_235:
            ;
        }

        public void PickUp(Character character)
        {
            PickedUp = true;
            Picker = character;

            PickUpTimer = PickUpTimerDefault;
            PickUpStartX = GetX();
            PickUpStartY = GetY();
            PickUpZ = 20000;
        }

        public void SetAngle(int angle)
        {
            ;
        }

        public void DisableAppearAnimation()
        {
            ;
        }
        public override bool ShouldDestruct()
        {
            return PickedUp && PickUpTimer <= 0;
        }

        public bool CanBePickedUp(Character a1)
        {
            if (ItemData.CanBePickedUp && !PickedUp)
            {
                switch (ItemData.Name)
                {
                    case "Point":
                        return true;
                    case "Money":
                        return true;
                    case "BattleRoyaleBuff":
                        return true;
                    case "SoulCollectorSoul":
                        return a1.GetIndex() / 16 == GetIndex() / 16 && a1.GetHitpointPercentage() < 100;
                    case "WeaponThrowerWeapon":
                        return a1.GetIndex() == GetIndex();
                }
            }
            return false;
        }

        public override void Encode(BitStream bitStream, bool isOwnObject, int OwnObjectIndex, int visionTeam)
        {
            base.Encode(bitStream, isOwnObject, visionTeam);
            bitStream.WritePositiveInt(10, 4);

            if (ItemData.Name == "OrbSpawner")
            {
                bitStream.WritePositiveIntMax16383(SpawningTick);
                bitStream.WritePositiveIntMax16383(SpawnedTick);
                //bitStream.WritePositiveIntMax16383(0);
            }
            if (ItemData.Name == "Spray")
            {
                bitStream.WriteIntMax511(10);
                if (bitStream.WriteBoolean(true))
                {
                    bitStream.WritePositiveIntMax3(0);
                    bitStream.WritePositiveIntMax63(0);
                }
            }
            if(ItemData.Name=="DoorHorizontal"||ItemData.Name== "DoorVertical")
            {
                bitStream.WriteBoolean(GetBattle().StoryMode.DoorTest);
                bitStream.WritePositiveIntMax127(1);
            }
            if (ItemData.Name== "ButtonHold"||ItemData.Name== "ButtonPress")
            {
                bitStream.WritePositiveIntMax127(1);
            }
        }
        public void SetFromToPosition(int X, int Y, int Z, int TX, int TY, int TZ)
        {
            SetPosition(X, Y, Z);
            PickUpStartX = X;
            PickUpStartY = Y;
            PickUpStartZ = Z;
            TargetX = TX;
            TargetY = TY;
            PickUpZ = TZ;
        }
        public override int GetObjectType()
        {
            return 3;
        }
    }
}
