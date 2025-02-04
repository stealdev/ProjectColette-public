namespace Supercell.Laser.Server.Logic.Game
{
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Titan.Json;

    public static class Events
    {
        public const int REFRESH_MINUTES = 3;

        private static Timer RefreshTimer;
        private static EventSlotConfig[] ConfigSlots;
        private static Dictionary<int, EventData> Slots;

        public static void Init()
        {
            LoadSettings();
            Slots = new Dictionary<int, EventData>();

            RefreshTimer = new Timer(new TimerCallback(RefreshTimerElapsed), null, 0, REFRESH_MINUTES * 60 * 1000);
        }

        private static void GenerateEvents()
        {
            for (int i = 0; i < ConfigSlots.Length; i++)
            {
                Slots[ConfigSlots[i].Slot] = GenerateEvent(ConfigSlots[i].AllowedModes, ConfigSlots[i].Slot, ConfigSlots[i].location);
            }
        }

        private static void RefreshTimerElapsed(object s)
        {
            GenerateEvents();
        }

        private static EventData GenerateEvent(string[] gameModes, int slot,int presetlocation)
        {
            if (presetlocation != 0)
            {
                LocationData location = DataTables.Get(DataType.Location).GetDataWithId<LocationData>(presetlocation);
                EventData ev = new EventData();
                ev.EndTime = DateTime.Now.AddMinutes(REFRESH_MINUTES);
                ev.LocationId = location.GetGlobalId();
                ev.Slot = slot;
                //if(slot==13) ev.BattlePlayerMap = new Laser.Logic.Battle.Structures.BattlePlayerMap(Accounts.Load(3).Home.PlayerMaps[0]);
                //if(slot==13) ev.BattlePlayerMap = new Laser.Logic.Battle.Structures.BattlePlayerMap(Accounts.Load(3).Home.PlayerMaps[0]);
                return ev;
            }
            int count = DataTables.Get(DataType.Location).Count;
            Random rand = new Random();
            int tries = 0;
            while (tries<1000)
            {
                LocationData location = DataTables.Get(DataType.Location).GetDataWithId<LocationData>(rand.Next(0, count));
                if (!location.Disabled && gameModes.Contains(location.GameModeVariation))
                {
                    EventData ev = new EventData();
                    ev.EndTime = DateTime.Now.AddMinutes(REFRESH_MINUTES);
                    ev.LocationId = location.GetGlobalId();
                    ev.Slot = slot;

                    return ev;
                }
                tries++;
            }
            tries = 0;
            while (tries < 1000)
            {
                LocationData location = DataTables.Get(DataType.Location).GetDataWithId<LocationData>(rand.Next(0, count));
                if (gameModes.Contains(location.GameModeVariation))
                {
                    EventData ev = new EventData();
                    ev.EndTime = DateTime.Now.AddMinutes(REFRESH_MINUTES);
                    ev.LocationId = location.GetGlobalId();
                    ev.Slot = slot;

                    return ev;
                }
                tries++;
            }
            return null;

            
        }

        private static void LoadSettings()
        {
            LogicJSONObject settings = LogicJSONParser.ParseObject(File.ReadAllText("gameplay.json"));
            LogicJSONArray slots = settings.GetJSONArray("slots");
            ConfigSlots = new EventSlotConfig[slots.Size()];

            for (int i = 0; i < slots.Size(); i++)
            {
                EventSlotConfig config = new EventSlotConfig();

                LogicJSONObject slot = slots.GetJSONObject(i);
                config.Slot = slot.GetJSONNumber("slot").GetIntValue();

                LogicJSONArray gameModes = slot.GetJSONArray("game_modes");
                config.AllowedModes = new string[gameModes.Size()];
                config.location= slot.GetJSONNumber("location").GetIntValue();
                for (int j = 0; j < gameModes.Size(); j++)
                {
                    config.AllowedModes[j] = gameModes.GetJSONString(j).GetStringValue();
                }

                ConfigSlots[i] = config;
            }
        }

        public static EventData GetEvent(int i)
        {
            if (HasSlot(i))
                return Slots[i];
            return null;
        }

        public static bool HasSlot(int slot)
        {
            return Slots.ContainsKey(slot);
        }

        public static EventData[] GetEvents()
        {
            return Slots.Values.ToArray();
        }

        private class EventSlotConfig
        {
            public int Slot { get; set; }
            public string[] AllowedModes { get; set; }
            public int location;
        }
    }
}
