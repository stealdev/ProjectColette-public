﻿namespace Supercell.Laser.Logic.Battle.Objects
{
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Math;

    public class GameObject
    {
        public int TicksGone => GetBattle().GetTicksGone();

        protected int DataId;
        protected GameObjectManager GameObjectManager;

        private int Index;
        public int TeamIndex=-1;
        private int FadeCounter;
        public int ObjectGlobalId;
        private int CloneOfObjectGlobalId;

        public bool IsInRealm;

        protected LogicVector2 Position;
        protected int Z;
        public int RenderZ = -1;
        public int Scale;

        public GameObject(int classId, int instanceId)
        {
            DataId = GlobalId.CreateGlobalId(classId, instanceId);

            Position = new LogicVector2();
            Z = 0;
            IsInRealm = false;
            FadeCounter = 10;
            Scale = 100;
        }
        public GameObject(LogicData data)
        {
            DataId = GlobalId.CreateGlobalId(data.GetClassId(), data.GetInstanceId());

            Position = new LogicVector2();
            Z = 0;

            FadeCounter = 10;
            Scale = 100;
        }

        public virtual void Tick()
        {
            ;
        }

        public virtual void Encode(BitStream bitStream, bool isOwnObject, int OwnObjectIndex, int visionTeam = 0)
        {
            if (GetObjectType() == 3 && ((Item)this).ItemData.AlignToTiles)
            {
                bitStream.WritePositiveIntMax63(Position.X / 300);
                bitStream.WritePositiveIntMax63(Position.Y / 300);
                goto LABEL_15;
            }
            if (GetObjectType() == 1 && ((Projectile)this).ProjectileData.IgnoreLevelBoarder)
            {
                bitStream.WriteIntMax65535(Position.X);
                bitStream.WriteIntMax65535(Position.Y);
            }
            else
            {
                bitStream.WritePositiveVInt(Position.X, 4);
                bitStream.WritePositiveVInt(Position.Y, 4);
            }
            bitStream.WritePositiveVInt(RenderZ != -1 ? RenderZ : Z, 4);
        LABEL_15:
            var t = GameObjectManager.GetBattle().GetPlayersCountWithGameModeVariation();
            if (GameObjectManager.GetBattle().GetGameModeVariation() == 30) t = 3;
            if (Index == -16) bitStream.WritePositiveVInt(17 * t, 3);
            else bitStream.WritePositiveVInt(Index, 3);
            bitStream.WritePositiveIntMax1(0);//wtf???
        }

        public virtual void ResetEventsOnTick()
        {
            ;
        }

        public void SetForcedVisible()
        {
            FadeCounter = 10;
        }

        public void SetForcedInvisible()
        {
            FadeCounter = 0;
        }

        public GameObjectManager GetGameObjectManager()
        {
            return GameObjectManager;
        }

        public void IncrementFadeCounter()
        {
            if (FadeCounter < 10) FadeCounter++;
        }

        public void DecrementFadeCounter()
        {
            if (FadeCounter > 0) FadeCounter--;
        }

        public void SetFadeCounter(int counter)
        { FadeCounter = counter; }


        public int GetFadeCounter()
        {
            return FadeCounter;
        }

        public void SetPosition(int x, int y, int z)
        {
            Position.Set(x, y);
            Z = z;
        }

        public LogicVector2 GetPosition()
        {
            return Position.Clone();
        }

        public BattlePlayer GetPlayer()
        {
            BattleMode battle = GameObjectManager.GetBattle();
            int TeamIndex = Index / 16;
            int OwnIndex = Index % 16;
            foreach (BattlePlayer battlePlayer in battle.GetPlayers())
            {
                if (battlePlayer.TeamIndex == TeamIndex && battlePlayer.PlayerIndex == OwnIndex) return battlePlayer;
            }
            return null;
        }

        public int GetGlobalID()
        {
            return ObjectGlobalId;
        }

        public int GetCloneOfObjectGlobalId()
        {
            return CloneOfObjectGlobalId;
        }

        public void SetCloneOfObjectGlobalId(int i)
        {
            CloneOfObjectGlobalId = i;
        }

        public int GetDataId()
        {
            return DataId;
        }
        public int GetCardValueForPassiveFromPlayer(string a2, int a3)
        {
            if (GetIndex() == -16) return -1;
            BattlePlayer v1 = GetPlayer();
            if (v1 == null) return -1;
            return v1.GetCardValueForPassive(a2, a3);
        }
        public void AttachGameObjectManager(GameObjectManager gameObjectManager, int globalId)
        {
            GameObjectManager = gameObjectManager;
            ObjectGlobalId = globalId;
        }

        public virtual bool ShouldDestruct()
        {
            return false;
        }

        public virtual void OnDestruct()
        {
            ;
        }
        public BattleMode GetBattle()
        {
            return GameObjectManager.GetBattle();
        }
        public int GetX()
        {
            return Position.X;
        }

        public int GetY()
        {
            return Position.Y;
        }

        public int GetZ()
        {
            return Z;
        }

        public void SetIndex(int i)
        {
            Index = i;
        }

        public int GetIndex()
        {
            return Index;
        }

        public virtual bool IsAlive()
        {
            return true;
        }

        public virtual int GetRadius()
        {
            return Scale;
        }

        public virtual int GetSize()
        {
            return 100;
        }

        public virtual int GetObjectType()
        {
            return -1;
        }
    }
}
