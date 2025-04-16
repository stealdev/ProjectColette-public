using System.Threading.Tasks;

namespace Supercell.Laser.Logic.Battle
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Battle.Component;
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Battle.Input;
    using Supercell.Laser.Logic.Battle.Level;
    using Supercell.Laser.Logic.Battle.Level.Factory;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Logic.Time;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Titan.DataStream;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;
    using System;
    using System.Threading;
    using System.Security.Cryptography.X509Certificates;
    using System.Numerics;
    using Supercell.Laser.Titan.Json;
    using System.Security.Cryptography;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Notification;
    using Supercell.Laser.Logic.Message.Account;
    using Masuda.Net.HelpMessage;

    public struct DiedEntry
    {
        public int DeathTick;
        public BattlePlayer Player;
    }

    public class BattleMode
    {
        public const int ORBS_TO_COLLECT_NORMAL = 0xA;
        public const int INTRO_TICKS = 180;
        public const int NO_TIME_TICKS = 16000;
        public const int NORMAL_TICKS = 3180;
        public StartLoadingMessage Dummy;

        public Timer m_updateTimer;

        public int m_locationId;
        private int m_gameModeVariation;
        private int m_playersCountWithGameModeVariation;

        public bool BattleWithTrophies;

        private Queue<ClientInput> m_inputQueue;

        List<GameObject> printobjects = new List<GameObject>();

        public List<BattlePlayer> m_players;
        private Dictionary<long, BattlePlayer> m_playersBySessionId;
        private Dictionary<long, LogicGameListener> m_spectators;
        private GameObjectManager m_gameObjectManager;

        private Rect m_playArea;
        private TileMap m_tileMap;
        public GameTime m_time;
        private LogicRandom m_random;
        private int m_randomSeed;
        public List<GameObject> StoryModeStuffs;
        private int m_winnerTeam;

        public BattlePlayerMap BattlePlayerMap;
        private int m_playersAlive;

        private int m_gemGrabCountdown;
        public bool IsStoryMode;
        public bool IsGameOver { get; set; }

        public List<int> EventModifiers;

        public StoryMode StoryMode;

        public BattleMode(int locationId)
        {
            m_winnerTeam = -1;
            m_locationId = locationId;
            m_gameModeVariation = GameModeUtil.GetGameModeVariation(Location.GameModeVariation);
            m_playersCountWithGameModeVariation = GamePlayUtil.GetPlayerCountWithGameModeVariation(m_gameModeVariation);

            m_inputQueue = new Queue<ClientInput>();

            m_randomSeed = 0;
            m_random = new LogicRandom(m_randomSeed);

            m_players = new List<BattlePlayer>();
            m_playersBySessionId = new Dictionary<long, BattlePlayer>();

            m_time = new GameTime();
            //for (int i = 0; i < 120; i++) m_time.IncreaseTick();
            m_tileMap = TileMapFactory.CreateTileMap(Location.Map);
            m_playArea = new Rect(0, 0, m_tileMap.LogicWidth, m_tileMap.LogicHeight);
            m_gameObjectManager = new GameObjectManager(this);

            m_spectators = new Dictionary<long, LogicGameListener>();

            EventModifiers = new List<int>();

            if (m_gameModeVariation == 30) StoryMode = new(this);
        }
        public void SetPlayerMap(BattlePlayerMap map)
        {
            BattlePlayerMap = map;
            m_tileMap = TileMapFactory.CreatePlayerMap(map.MapData);
            m_gameModeVariation = map.GMV;
            m_playersCountWithGameModeVariation = GamePlayUtil.GetPlayerCountWithGameModeVariation(map.GMV);
        }
        public bool HasEventModifier(int m) => EventModifiers.Contains(m);
        public void SetEventModifiers(List<int> ms)
        {
            EventModifiers = ms.ToList();
        }
        public int GetGemGrabCountdown()
        {
            return m_gemGrabCountdown;
        }
        public LogicRandom GetLogicRandom()
        {
            return m_random;
        }
        public GameObjectManager GetGameObjectManager()
        {
            return m_gameObjectManager;
        }
        public int GetPlayersAliveCountForBattleRoyale()
        {
            return m_playersAlive;
        }
        public Petrol AddPetrol(int a2, int a3, int a4, int a5, int a6, int a7)
        {
            Petrol v32 = new Petrol(a2, a3, a4, a5, a6, a7);
            m_gameObjectManager.Petrols.Add(v32);
            return v32;
        }
        private void TickSpawnHeroes()
        {
            //return;
            foreach (BattlePlayer player in m_players)
            {
                if (player.IsAlive) continue;
                if (m_gameModeVariation == 6) return;
                LogicVector2 spawnPoint = player.GetSpawnPoint();

                if (GetTicksGone() == player.DeathTick + 20 * GameModeUtil.GetRespawnSeconds(m_gameModeVariation))
                {
                    if (player.DeathTick != 1)
                    {
                        AreaEffect v90 = GameObjectFactory.CreateGameObjectByData(DataTables.GetAreaEffectByName("HeroSpawn"));
                        v90.SetPosition(spawnPoint.X, spawnPoint.Y, 0);
                        v90.SetIndex(player.TeamIndex * 16 + player.PlayerIndex);
                        m_gameObjectManager.AddGameObject(v90);
                        v90.Trigger();
                    }
                }
                if (GetTicksGone() == player.DeathTick + 20 * GameModeUtil.GetRespawnSeconds(m_gameModeVariation) + 40)
                {
                    Character character = SpawnHero(player.CharacterData, player.HeroPowerLevel, player.TeamIndex * 16 + player.PlayerIndex, 0, false);
                    character.SetPosition(spawnPoint.X, spawnPoint.Y, 0);
                    character.SetBot(player.IsBot());
                    if (player.Gear1 != null) character.Gear1 = null;
                    if (player.Gear2 != null) character.Gear2 = null;
                    character.SpawnTick = GetTicksGone();
                    character.IsInvincible = true;
                    player.OwnObjectId = character.GetGlobalID();
                    player.IsAlive = true;
                    //if (character.CharacterData.AreaEffect != null)
                    //{
                    //    AreaEffectData effectData = DataTables.Get(17).GetData<AreaEffectData>(character.CharacterData.AreaEffect);
                    //    character.CharacterAreaEffect = new AreaEffect(17, effectData.GetInstanceId());
                    //    character.CharacterAreaEffect.SetPosition(character.GetX(), character.GetY(), 0);
                    //    character.CharacterAreaEffect.SetSource(character);
                    //    character.CharacterAreaEffect.SetIndex(character.GetIndex());
                    //    character.CharacterAreaEffect.SetDamage(0);
                    //    m_gameObjectManager.AddGameObject(character.CharacterAreaEffect);
                    //}
                }
            }
        }
        public Character SpawnHero(CharacterData a2, int a3, int a4, int a5, bool a6)
        {
            Character v9 = new Character(a2);
            m_gameObjectManager.AddGameObject(v9);
            if (a2.AreaEffect != null)
            {
                v9.AddAreaEffect(0, 0, null, 1, false);
            }
            v9.SetIndex(a4);
            v9.SetUpgrades(a3);
            return v9;
        }
        public void PlayerDied(BattlePlayer player)
        {
            if (m_gameModeVariation == 6)
            {
                int rank = m_playersAlive;
                m_playersAlive--;
                player.BattleRoyaleRank = rank;

                BattleEndMessage message = new BattleEndMessage();
                message.GameMode = 2;
                message.IsPvP = BattleWithTrophies;
                message.Players = new List<BattlePlayer>();
                message.Players.Add(player);
                message.OwnPlayer = player;

                if (player.IsBot() == 0)
                {
                    HomeMode homeMode = LogicServerListener.Instance.GetHomeMode(player.AccountId);
                    if (homeMode != null)
                    {
                        if (homeMode.Home.Quests != null)
                        {
                            message.ProgressiveQuests = homeMode.Home.Quests.UpdateQuestsProgress(m_gameModeVariation, player.CharacterId, player.Kills, player.Damage, player.Heals, homeMode.Home);
                        }
                    }
                }

                if (player.Avatar == null) return;
                player.Avatar.BattleId = -1;

                if (player.GameListener == null) return;

                Hero hero = player.Avatar.GetHero(player.CharacterId);

                message.Result = rank;
                int tokensReward = 40 / rank;
                message.TokensReward = tokensReward;
                if (rank > 5)
                {
                    int trophiesReward = -(rank - 5);
                    if (hero.Trophies < -trophiesReward) trophiesReward = -hero.Trophies;
                    message.TrophiesReward = trophiesReward;

                    player.Avatar.AddTokens(tokensReward);
                    player.Home.TokenReward += tokensReward;
                    hero.AddTrophies(message.TrophiesReward);
                }
                else
                {
                    int trophiesReward = (5 - rank) * 2;
                    message.TrophiesReward = trophiesReward;

                    player.Avatar.AddTokens(tokensReward);
                    player.Home.TokenReward += tokensReward;
                    player.Home.TrophiesReward += trophiesReward;

                    if (rank <= 4)
                    {
                        player.Avatar.AddStarTokens(1);
                        message.StarToken = true;
                        player.Home.StarTokenReward += 1;
                    }
                    hero.AddTrophies(message.TrophiesReward);
                }


                player.GameListener.SendTCPMessage(message);

            }

            if (m_gameModeVariation == 0)
            {
                player.ResetScore();
            }

            //player.OwnObjectId = 0;

        }

        public BattlePlayer GetPlayerWithObject(int globalId)
        {
            GameObject gameObject = m_gameObjectManager.GetGameObjectByID(globalId);
            if (gameObject == null) return null;
            return gameObject.GetPlayer();
        }

        public TileMap GetTileMap()
        {
            return m_tileMap;
        }

        public void Start()
        {
            m_updateTimer = new Timer(new TimerCallback(Update), null, 0, 1000 / 20);
        }

        public void Update(object stateInfo)
        {
            this.ExecuteOneTick();
            //Debugger.Print("shid");
        }

        public BattlePlayer GetPlayer(int globalId)
        {
            return m_gameObjectManager.GetGameObjectByID(globalId).GetPlayer();
        }

        public void AddSpectator(long sessionId, LogicGameListener gameListener)
        {
            m_spectators.Add(sessionId, gameListener);
        }

        public void ChangePlayerSessionId(long old, long newId)
        {
            if (m_playersBySessionId.ContainsKey(old))
            {
                BattlePlayer player = m_playersBySessionId[old];
                player.LastHandledInput = 0;
                m_playersBySessionId.Remove(old);
                m_playersBySessionId.Add(newId, player);
                //this.m_updateTimer.Dispose();
                //this.GameOver();
                //this.IsGameOver = true;
                //return;
            }
        }

        public BattlePlayer GetPlayerBySessionId(long sessionId)
        {
            if (m_playersBySessionId.ContainsKey(sessionId))
            {
                return m_playersBySessionId[sessionId];
            }
            return null;
        }

        public void AddPlayer(BattlePlayer player, long sessionId)
        {
            if (Debugger.DoAssert(player != null, "LogicBattle::AddPlayer - player is NULL!"))
            {
                player.SessionId = sessionId;
                m_players.Add(player);
                if (m_players.Count == 1 && m_gameModeVariation == 30) StoryMode.Player = player;
                if (sessionId > 0)
                {
                    m_playersBySessionId.Add(sessionId, player);
                }
                if (player.Avatar != null)
                {
                    player.Avatar.BattleId = Id;
                    player.Avatar.TeamIndex = player.TeamIndex;
                    player.Avatar.OwnIndex = player.PlayerIndex;
                }
            }
        }

        public void RemovePlayer(BattlePlayer player)
        {
            if (Debugger.DoAssert(player != null, "LogicBattle::AddPlayer - player is NULL!"))
            {
                player.GameListener = null;
                m_playersBySessionId.Remove(player.SessionId);
                if (player.Avatar != null)
                {
                    player.Avatar.BattleId = -1;
                }
            }
            /*if (!m_playersBySessionId.Any())
            {
                this.m_updateTimer.Dispose();
                this.GameOver();
                this.IsGameOver = true;
                Debugger.Print("Battle #" + Id + " has ended!");
                return;
            }*/
        }

        public void AddGameObjects()
        {
            //return;
            //CharacterData b1 = DataTables.GetCharacterByName("Safe");
            //Character bas1 = new Character(b1);
            //bas1.SetPosition(3000, 5000, 0);
            //bas1.SetIndex(32);
            //m_gameObjectManager.AddGameObject(bas1);
            //return;
            //Item b=new Item()
            //b.SetPosition(2950, 4950, 0);
            //b.SetIndex(16);
            //m_gameObjectManager.AddGameObject(b);
            m_playersAlive = m_players.Count;

            int team1Indexer = 0;
            int team2Indexer = 0;
            if (IsStoryMode)
            {
                StoryModeStuffs = new List<GameObject>();
                foreach (BattlePlayer player in m_players)
                {
                    Character character = new Character(player.CharacterData);
                    character.SetIndex(player.PlayerIndex + (16 * player.TeamIndex));
                    character.SetHeroLevel(player.HeroPowerLevel);
                    //character.SetImmunity(60, 100);
                    character.Gear1 = null;
                    character.Gear2 = null;
                    character.SetPosition(3150, 4950, 0);

                    m_gameObjectManager.AddGameObject(character);
                    player.OwnObjectId = character.GetGlobalID();
                    //if (player.SessionId == -1)
                    //{
                    StoryModeStuffs.Add(character);
                    //}
                }

                //Character character1 = new Character(16, 0);
                //character1.SetIndex(-1);
                //character1.SetHeroLevel(11);
                ////character1.SetImmunity(60, 100);
                //character1.SetPosition(2950, 4950, 0);

                //m_gameObjectManager.AddGameObject(character1);

                return;
            }
            foreach (BattlePlayer player in m_players)
            {
                Character character = SpawnHero(player.CharacterData, player.HeroPowerLevel, player.TeamIndex * 16 + player.PlayerIndex, 0, false);
                player.IsAlive = true;
                character.SetBot(player.IsBot());
                //if (player.IsBot() && GetGameModeVariation() == 24) character.m_isBot = 2;
                //character.SetImmunity(60, 100);
                if (player.Gear1 != null) character.Gear1 = null;
                if (player.Gear2 != null) character.Gear2 = null;


                if (GameModeUtil.HasTwoTeams(m_gameModeVariation))
                {
                    if (player.TeamIndex == 0)
                    {
                        Tile tile = m_tileMap.SpawnPointsTeam1[team1Indexer++ % m_tileMap.SpawnPointsTeam1.Count];
                        character.SetPosition(tile.X + 150, tile.Y + 150, 0);
                        player.SetSpawnPoint(tile.X + 150, tile.Y + 150);
                    }
                    else
                    {
                        Tile tile = m_tileMap.SpawnPointsTeam2[team2Indexer++ % m_tileMap.SpawnPointsTeam2.Count];
                        character.SetPosition(tile.X + 150, tile.Y + 150, 0);
                        player.SetSpawnPoint(tile.X + 150, tile.Y + 150);
                    }
                }
                else
                {
                    Tile tile = m_tileMap.SpawnPointsTeam1[team1Indexer++ % m_tileMap.SpawnPointsTeam1.Count];
                    character.SetPosition(tile.X + 150, tile.Y + 150, 0);
                    player.SetSpawnPoint(tile.X + 150, tile.Y + 150);
                }

                //m_gameObjectManager.AddGameObject(character);
                player.OwnObjectId = character.GetGlobalID();

                //if (character.CharacterData.AreaEffect != null)
                //{
                //    AreaEffectData effectData = DataTables.Get(17).GetData<AreaEffectData>(character.CharacterData.AreaEffect);
                //    character.CharacterAreaEffect = new AreaEffect(17, effectData.GetInstanceId());
                //    character.CharacterAreaEffect.SetPosition(character.GetX(), character.GetY(), 0);
                //    character.CharacterAreaEffect.SetSource(character);
                //    character.CharacterAreaEffect.SetIndex(character.GetIndex());
                //    character.CharacterAreaEffect.SetDamage(0);
                //    m_gameObjectManager.AddGameObject(character.CharacterAreaEffect);
                //}
                //if (m_gameModeVariation == 3) character.SetPosition(2950, 4950, 0);
                //foreach (CardData carddata in DataTables.Get(DataType.Card).GetDatas())
                //{
                //    if (carddata.Target == character.CharacterData.Name && carddata.MetaType == 5)
                //    {
                //        player.AccessoryCardData = carddata;
                //        break;
                //    }
                //}

                //character.tms = new AreaEffect(17, 17);
                //character.tms.SetPosition(0, 0, 0);
                //character.tms.SetIndex(character.GetIndex());
                //m_gameObjectManager.AddGameObject(character.tms);
            }

            if (m_gameModeVariation == 0)
            {
                Item item = new Item(DataTables.GetItemByName("OrbSpawner"));
                item.SetPosition(3150, 4950, 0);
                item.DisableAppearAnimation();
                m_gameObjectManager.AddGameObject(item);
            }

            if (m_gameModeVariation == 3)
            {
                ItemData data = DataTables.Get(18).GetData<ItemData>("Money");
                Item item = new Item(data);
                item.SetPosition(3150, 4950, 0);
                item.DisableAppearAnimation();
                m_gameObjectManager.AddGameObject(item);
            } 

            if (m_gameModeVariation == 6)
            {
                CharacterData data = DataTables.Get(16).GetData<CharacterData>("LootBox");
                for (int i = 0; i < m_tileMap.Height; i++)
                {
                    for (int j = 0; j < m_tileMap.Width; j++)
                    {
                        Tile tile = m_tileMap.GetTile(i, j, true);
                        if (tile.Code == '4')
                        {
                            bool shouldSpawnBox = GetRandomInt(0, 120) < 60;

                            if (shouldSpawnBox)
                            {
                                Character box = new Character(data);
                                box.SetPosition(tile.X + 150, tile.Y + 150, 0);
                                box.SetIndex(165);
                                m_gameObjectManager.AddGameObject(box);
                            }
                        }
                    }
                }
            }
            if (m_gameModeVariation == 7)
            {
                //CharacterData characterData = DataTables.Get(16).GetData<CharacterData>("FleaPet");
                //Character boss = new Character(characterData);
                //boss.SetPosition(10000, 10000, 0);
                //boss.SetIndex(32);
                //m_gameObjectManager.AddGameObject(boss);
                //CharacterData characterData1 = DataTables.Get(16).GetData<CharacterData>("ShamanPet");
                //Character boss1 = new Character(16, characterData1.GetInstanceId());
                //boss1.SetPosition(10000, 9000, 0);
                //boss1.SetIndex(16);
                //m_gameObjectManager.AddGameObject(boss1);
                //CharacterData characterData2 = DataTables.Get(16).GetData<CharacterData>("RaidBoss");
                //Character boss2 = new Character(16, characterData2.GetInstanceId());
                //boss2.SetPosition(10000, 8000, 0);
                //boss2.SetIndex(16);
                //m_gameObjectManager.AddGameObject(boss2);
                //CharacterData characterData3 = DataTables.Get(16).GetData<CharacterData>("ShieldTank");
                //Character boss3 = new Character(16, characterData3.GetInstanceId());
                //boss3.SetPosition(8000, 8000, 0);
                //m_gameObjectManager.AddGameObject(boss3);
            }
            if (m_gameModeVariation == 2)
            {
                CharacterData b = DataTables.GetCharacterByName("Safe");
                Character base1 = new Character(b);
                base1.SetPosition(m_tileMap.SpawnPointsBases[0].X + 150, m_tileMap.SpawnPointsBases[0].Y + 150, 0);
                base1.SetIndex(22);
                base1.TeamIndex = 1;
                m_gameObjectManager.AddGameObject(base1);
                Character base2 = new Character(b);
                base2.SetPosition(m_tileMap.SpawnPointsBases[1].X + 150, m_tileMap.SpawnPointsBases[1].Y + 150, 0);
                base2.SetIndex(6);
                base2.TeamIndex = 0;
                m_gameObjectManager.AddGameObject(base2);
            }
            if (m_gameModeVariation == 13)
            {

                Character TrainingDummyBig = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyBig"));

                Character TrainingDummyMedium = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyMedium"));
                Character TrainingDummyMedium1 = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyMedium"));
                Character TrainingDummyMedium2 = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyMedium"));
                Character TrainingDummyMedium3 = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyMedium"));

                Character TrainingDummySmall = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummySmall"));
                Character TrainingDummyShooting = new Character(DataTables.Get(16).GetData<CharacterData>("TrainingDummyShooting"));


                TrainingDummyBig.SetPosition(m_tileMap.TrainingDummyBigSpawners[0].X + 150, m_tileMap.TrainingDummyBigSpawners[0].Y + 150, 0);
                TrainingDummyBig.SetIndex(-16);
                m_gameObjectManager.AddGameObject(TrainingDummyBig);




                for (int i = 0; i < m_tileMap.TrainingDummySmallSpawners.Count; i++)
                {
                    Tile tile = m_tileMap.TrainingDummySmallSpawners[i++ % m_tileMap.TrainingDummySmallSpawners.Count];
                    Tile spawner = m_tileMap.TrainingDummySmallSpawners[i];
                    Console.WriteLine(i);
                    TrainingDummySmall.SetPosition(tile.X + 150, tile.Y + 150, 0);
                    TrainingDummySmall.SetIndex(-16);
                    // m_gameObjectManager.AddGameObject(TrainingDummySmall);
                }

                for (int i = 0; i < m_tileMap.TrainingDummyShooting.Count; i++)
                {
                    Tile spawner = m_tileMap.TrainingDummyShooting[i];
                    TrainingDummyShooting.SetPosition(spawner.X + 150, spawner.Y + 150, 0);
                    TrainingDummyShooting.SetIndex(-16);
                    m_gameObjectManager.AddGameObject(TrainingDummyShooting);
                }


                TrainingDummyMedium.SetPosition(m_tileMap.TrainingDummyMediumSpawners[0].X + 150, m_tileMap.TrainingDummyMediumSpawners[0].Y + 150, 0);
                TrainingDummyMedium.SetIndex(-16);
                m_gameObjectManager.AddGameObject(TrainingDummyMedium);


                TrainingDummyMedium1.SetPosition(m_tileMap.TrainingDummyMediumSpawners[1].X + 150, m_tileMap.TrainingDummyMediumSpawners[1].Y + 150, 0);
                TrainingDummyMedium1.SetIndex(-16);
                m_gameObjectManager.AddGameObject(TrainingDummyMedium1);

                TrainingDummyMedium2.SetPosition(m_tileMap.TrainingDummyMediumSpawners[2].X + 150, m_tileMap.TrainingDummyMediumSpawners[2].Y + 150, 0);
                TrainingDummyMedium2.SetIndex(-16);
                m_gameObjectManager.AddGameObject(TrainingDummyMedium2);

                TrainingDummyMedium3.SetPosition(m_tileMap.TrainingDummyMediumSpawners[3].X + 150, m_tileMap.TrainingDummyMediumSpawners[3].Y + 150, 0);
                TrainingDummyMedium3.SetIndex(-16);
                m_gameObjectManager.AddGameObject(TrainingDummyMedium3);
            }
            if (m_gameModeVariation == 30)
            {
                StoryMode.AddGameObjects();

            }

        }

        public void RemoveSpectator(long id)
        {
            if (m_spectators.ContainsKey(id))
            {
                m_spectators.Remove(id);
            }
        }

        public bool IsInPlayArea(int x, int y)
        {
            return m_playArea.IsInside(x + 150, y + 150);
        }

        public int GetTeamPlayersCount(int teamIndex)
        {
            int result = 0;
            foreach (BattlePlayer player in GetPlayers())
            {
                if (player.TeamIndex == teamIndex) result++;
            }
            return result;
        }

        public void AddClientInput(ClientInput input, long sessionId)
        {
            if (!m_playersBySessionId.ContainsKey(sessionId)) return;

            input.OwnerSessionId = sessionId;
            m_inputQueue.Enqueue(input);
        }

        public void HandleSpectatorInput(ClientInput input, long sessionId)
        {
            if (input == null) return;

            if (!m_spectators.ContainsKey(sessionId)) return;
            m_spectators[sessionId].HandledInputs = input.Index;
        }

        private void HandleClientInput(ClientInput input)
        {
            if (input == null) return;


            BattlePlayer player = GetPlayerBySessionId(input.OwnerSessionId);

            if (player == null) return;
            if (player.LastHandledInput >= input.Index) return;

            player.LastHandledInput = input.Index;
            Character character1 = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
            if ((character1 == null || !character1.IsAlive()) && input.Type != 4 && input.Type != 9) return;
            if (character1 != null && character1.ViusalChargeType > 0 && input.Type >= 2 && input.Type <= 8) return;
            if (input.Type < 8 && input.Type != 4 && StoryMode != null && StoryMode.Intangible) return;
            if (input.Type != 0 && StoryMode != null && StoryMode.Choosing) return;
            //Debugger.Print(input.Type.ToString());
            switch (input.Type)//0普攻 1大招 2移动 5捏大 6松大 8妙具 9发倒指
            {
                case 0:
                    {
                        if (StoryMode != null && StoryMode.Choosing)
                        {
                            StoryMode.ChoiceChosen(input.X, input.Y);
                            return;
                        }
                        //Debugger.Print("TG:" + GetTicksGone());

                        //Debugger.Print("Active:"+GetTicksGone().ToString());
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
                        if (character == null) return;
                        //character.MoveTo(0, character.GetX(), character.GetY()-2000, 0, 0, 0, 0);
                        //return;
                        Skill skill = character.GetWeaponSkill();
                        if (skill == null) return;

                        character.UltiDisabled();
                        if (input.AutoAttack)
                        {
                            Character target = (Character)m_gameObjectManager.GetGameObjectByID(input.AutoAttackTarget);
                            if (target != null)
                            {
                                input.X = target.GetX();
                                input.Y = target.GetY();
                            }
                            else
                            {
                                target = character.GetClosestVisibleEnemy();
                                if (target != null)
                                {
                                    input.X = target.GetX();
                                    input.Y = target.GetY();
                                }
                            }
                            character.ActivateSkill(0, input.X, input.Y);
                            break;
                        }
                        if (!skill.SkillData.IsPositionalTargeted())
                        {
                            character.ActivateSkill(0, input.X + character.GetX(), input.Y + character.GetY());
                        }
                        else character.ActivateSkill(0, input.X, input.Y);

                        var counter = character1.GetFadeCounter();
                        character1.IncrementFadeCounter();
                        character.SetFadeCounter(counter);

                        character1.ResetAFKTicks();
                        break;
                    }
                case 1:
                    {
                        //return;
                        if (character1.IsControlled) return;
                        //Debugger.Print("ulti");
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);

                        if (character == null) return;
                        character.m_skills[1].SeemToBeActive = false;

                        if (character.DisplayUltiTmp) return;
                        Skill skill = character.GetUltimateSkill();
                        if (skill == null) return;
                        character?.UltiDisabled();
                        if (!player.HasUlti()) return;
                        //if (character.CharacterData.Name == "Mummy")
                        //{
                        //    if (character.TransformationState == 0)
                        //    {
                        //        character.TransformationState = 1;
                        //        int charges = character.m_skills[0].Charge;
                        //        character.IsRage = true;
                        //        character.m_skills[0] = new Skill(20000133, false);
                        //        character.m_skills[0].Charge = charges;
                        //    }
                        //    else
                        //    {
                        //        int charges = character.m_skills[0].Charge;
                        //        character.TransformationState = 0;
                        //        character.IsRage = false;
                        //        character.m_skills[0] = new Skill(20000132, false);
                        //        character.m_skills[0].Charge = charges;
                        //    }
                        //    return;
                        //}

                        character.UltiEnabled();
                        skill.IsFromAutoAttack = input.AutoAttack;
                        if (input.AutoAttack && !skill.SkillData.MovementBasedAutoshoot)
                        {
                            Character target = (Character)m_gameObjectManager.GetGameObjectByID(input.AutoAttackTarget);
                            if (target != null)
                            {
                                input.X = target.GetX();
                                input.Y = target.GetY();
                            }
                            else
                            {
                                target = character.GetClosestVisibleEnemy();
                                if (target != null)
                                {
                                    input.X = target.GetX();
                                    input.Y = target.GetY();
                                }
                            }
                            character.ActivateSkill(1, input.X, input.Y);
                            break;
                        }
                        if (input.AutoAttack && skill.SkillData.MovementBasedAutoshoot)
                        {
                            character.ActivateSkill(1, input.X, input.Y);
                            break;
                        }
                        if (!skill.SkillData.IsPositionalTargeted())
                        {
                            character.ActivateSkill(1, input.X + character.GetX(), input.Y + character.GetY());
                        }
                        else character.ActivateSkill(1, input.X, input.Y);
                        var counter = character1.GetFadeCounter();
                        character1.IncrementFadeCounter();
                        character.SetFadeCounter(counter);

                        character1.ResetAFKTicks();
                        break;
                    }
                case 2:
                    {
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);

                        //List<Character> clonecharacters = (Character)m_gameObjectManager.GetGameObjectByCloneOfObjectGlobalId(player.OwnObjectId);

                        if (character == null) return;

                        LogicVector2 old = new LogicVector2(character.GetX(), character.GetY());
                        LogicVector2 destin = new LogicVector2(input.X, input.Y);
                        int v15 = old.GetDistanceSquaredTo(input.X, input.Y);
                        //Debugger.Print(v15.ToString());
                        ////if (v15 >= 810001)
                        //{
                        //    int v16 = LogicMath.Sqrt(v15);
                        //    int v18 = 900 * (input.X - character.GetX()) / v16;
                        //    int dx= v18 + character.GetX();
                        //    int v14 = 900 * (input.Y - character.GetY()) / v16 + character.GetY();
                        character.MoveTo(0, input.X, input.Y, 0, 0, 0, 0);
                        //AreaEffectData effectData = DataTables.Get(17).GetData<AreaEffectData>(skill.SkillData.AreaEffectObject);
                        //if (character.m_movementDestination != null)
                        //{
                        //    AreaEffect effect = new AreaEffect(17, 51);
                        //    effect.SetPosition(character.m_movementDestination.X, character.m_movementDestination.Y, 0);
                        //    effect.SetSource(character);
                        //    effect.SetIndex(character.GetIndex());
                        //    m_gameObjectManager.AddGameObject(effect);
                        //}

                        //}

                        character1.ResetAFKTicks();
                        break;
                    }
                case 4:
                    {
                        SendBattleEndToPlayer(player);
                        RemovePlayer(player);
                        character1.ResetAFKTicks();
                        return;
                    }
                case 5:
                    {
                        if (character1.IsControlled) return;
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
                        character?.UltiEnabled();
                        character1.ResetAFKTicks();
                        //if (character?.Ultiing ?? false) character.UltiButtonPressedTimes = 1;
                        break;
                    }
                case 6:
                    {
                        if (character1.IsControlled) return;
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
                        character?.UltiDisabled();
                        character1.ResetAFKTicks();
                        //if (character != null) character.m_skills[1].SeemToBeActive = false;
                        //if (character?.UltiButtonPressedTimes == 1) character.UltiButtonPressedTwice();
                        break;
                    }
                case 8:
                    {
                        if (StoryMode != null && StoryMode.StartWaitingTick > GetTicksGone())
                        {
                            StoryMode.DoorTest ^= true;
                            StoryMode.WaitSkipped = true;
                            return;
                            //StoryMode.N("sshdbuvuyeuywguegywueyhwue");
                        }
                        if (character1.IsControlled) return;
                        Accessory accessory = player.Accessory;
                        Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
                        if (accessory != null) accessory.TriggerAccessory(character, input.X, input.Y);
                        //if (player.AccessoryCoolDown > 0) return;
                        //player.AccessoryCoolDown = player.AccessoryData.Cooldown;
                        //player.AccessoryUsesLeft--;
                        //}
                        character1.ResetAFKTicks();
                        break;
                    }
                case 9:
                    {
                        if (character1.IsControlled) return;
                        player.UsePin(input.EmoteIndex, GetTicksGone());
                        character1.ResetAFKTicks();
                        break;
                    }
                case 10:
                    {
                        if (character1.IsSteerMovementActive())
                        {
                            if ((input.X | input.Y) < 0) character1.SteerAngle = -1;
                            else
                                character1.SteerAngle = LogicMath.GetAngle(-input.X + character1.GetX(), -input.Y + character1.GetY());
                            return;
                        }
                        Projectile projectile = character1.GetControlledProjectile();
                        if (projectile == null) return;
                        int v26;
                        if ((input.X | input.Y) < 0) v26 = -1;
                        else
                        {
                            v26 = LogicMath.GetAngle(input.X - projectile.GetX(), input.Y - projectile.GetY());
                        }
                        projectile.SteerAngle = v26;
                        character1.ResetAFKTicks();
                        break;
                    }
                case 13:
                    input.X = input.X * 150 / 100;
                    if (character1.GetSkillHoldedTicks() <= 0)
                    {
                        character1.HoldSkillStarted();
                        if (character1.m_skills[0].SkillData.AttackPattern == 13) break;
                        AreaEffect areaEffect = GameObjectFactory.CreateGameObjectByData(DataTables.GetAreaEffectByName("Tara005UltiSuck"));
                        areaEffect.SetIndex(character1.GetIndex());
                        areaEffect.SetPosition(character1.GetX(), character1.GetY(), 0);
                        m_gameObjectManager.AddGameObject(areaEffect);
                        //areaEffect.Trigger
                    }
                    if (character1.m_skills[0].SkillData.AttackPattern == 13) break;
                    character1.SetForcedAngle(LogicMath.NormalizeAngle360(LogicMath.GetAngle(input.X, input.Y) - character1.GetIndex() / 16 * 180));
                    character1.SetVisionOverride(input.X + character1.GetX(), input.Y + character1.GetY(), 2);
                    character1.StopMovement();
                    character1.SetForcedVisible();
                    character1.BlockHealthRegen();
                    character1.m_skills[1].SeemToBeActive = true;
                    //character1.m_state = 2;
                    //character1.ActivateSkill(1, 0, 0);
                    //character1.m_skills[1].OnActivate = true;
                    //character1.m_skills[1].TicksActive = 100;
                    character1.ResetAFKTicks();
                    break;
                case 14:
                    character1.SkillHoldTicks = -1;
                    character1.m_skills[1].SeemToBeActive = false;
                    character1.ResetAFKTicks();
                    break;
                case 15:
                    break;
                case 17:
                    if (!player.HasOverCharge()) return;
                    player.OverCharging = true;
                    character1.ResetAFKTicks();
                    break;
                default:
                    Debugger.Warning("Input is unhandled: " + input.Type);
                    break;
            }
        }

        public bool IsTileOnPoisonArea(int xTile, int yTile)
        {
            if (m_gameModeVariation != 6) return false;

            int tick = GetTicksGone();

            if (tick > 500)
            {
                int poisons = 0;
                poisons += (tick - 500) / 100;

                if (xTile <= poisons || xTile >= 59 - poisons || yTile <= poisons || yTile >= 59 - poisons)
                {
                    return true;
                }
            }
            return false;
        }

        private void HandleIncomingInputMessages()
        {
            while (m_inputQueue.Count > 0)
            {
                this.HandleClientInput(m_inputQueue.Dequeue());
            }
        }

        public void ExecuteOneTick()
        {
            try
            {
                this.HandleIncomingInputMessages();


                if (this.CalculateIsGameOver())
                {
                    this.m_updateTimer.Dispose();
                    this.GameOver();
                    this.IsGameOver = true;
                    return;
                }
                this.SendVisionUpdateToPlayers();
                foreach (BattlePlayer player in GetPlayers())
                {
                    player.KillList.Clear();
                }
                if (StoryMode != null) StoryMode.Tick();
                this.m_gameObjectManager.PreTick();

                this.Tick();
                this.m_time.IncreaseTick();

            }
            catch (Exception e)
            {
                Parallel.ForEach(m_players, player =>
                {
                    if (player.GameListener != null)
                    {
                        //ServerErrorMessage serverErrorMessage = new ServerErrorMessage();
                        try
                        {
                            //player.GameListener.SendMessage(serverErrorMessage);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Battle stopped with exception! Message: " + e.Message + " Trace: " + e.StackTrace);
                        }
                    }
                });
                
                m_updateTimer.Dispose();
                IsGameOver = true;

            }

        }

        private void TickSpawnEventStuffDelayed()
        {
            ;
        }

        public void GameOver()
        {
            SendBattleEndToPlayers();
        }

        public void SendBattleEndToPlayers()
        {
            Random rand = new Random();

            foreach (BattlePlayer player in m_players)
            {
                if (player.SessionId < 0) continue;
                //if (!player.IsAlive) continue;
                if (player.BattleRoyaleRank == -1) player.BattleRoyaleRank = 1;
                if (player.Avatar == null) continue;
                int rank = player.BattleRoyaleRank;
                player.Avatar.BattleId = -1;

                bool isVictory = m_winnerTeam == player.TeamIndex;

                BattleEndMessage message = new BattleEndMessage();
                Hero hero = player.Avatar.GetHero(player.CharacterId);

                if (player.IsBot() == 0)
                {
                    HomeMode homeMode = LogicServerListener.Instance.GetHomeMode(player.AccountId);
                    if (homeMode != null)
                    {
                        if (homeMode.Home.Quests != null)
                        {
                            message.ProgressiveQuests = homeMode.Home.Quests.UpdateQuestsProgress(m_gameModeVariation, player.CharacterId, player.Kills, player.Damage, player.Heals, homeMode.Home);
                        }
                    }
                }

                if (m_gameModeVariation != 6)
                {
                    message.GameMode = 1;
                    message.IsPvP = BattleWithTrophies;
                    message.Players = m_players;
                    message.OwnPlayer = player;

                    if (m_winnerTeam == -1 && BattleWithTrophies) // Draw
                    {
                        message.Result = 2;
                        message.TokensReward = 15;

                        message.TrophiesReward = 0;

                        player.Avatar.AddTokens(15);

                        player.Home.TokenReward += 15;
                    }

                    int brawlerTrophies = hero.Trophies;
                    int winTrophies = 0;
                    int loseTrophies = 0;
                    if (!BattleWithTrophies) {
                        message.BattleWithoutTrophies = true;
                    }
                    if (0 <= brawlerTrophies && brawlerTrophies <= 49)
                    {
                        winTrophies = 8;
                        loseTrophies = 0;
                    }
                    else 
                    {
                        if (50 <= brawlerTrophies && brawlerTrophies <= 99) 
                        {
                            winTrophies = 8;
                            loseTrophies = -1;
                        }
                        if (100 <= brawlerTrophies && brawlerTrophies <= 199) 
                        {
                            winTrophies = 8;
                            loseTrophies = -2;
                        }
                        if (200 <= brawlerTrophies && brawlerTrophies <= 299) 
                        {
                            winTrophies = 8;
                            loseTrophies = -3;
                        }
                        if (300 <= brawlerTrophies && brawlerTrophies <= 399) 
                        {
                            winTrophies = 8;
                            loseTrophies = -4;
                        }
                        if (400 <= brawlerTrophies && brawlerTrophies <= 499) 
                        {
                            winTrophies = 8;
                            loseTrophies = -5;
                        }
                        if (500 <= brawlerTrophies && brawlerTrophies <= 599) 
                        {
                            winTrophies = 8;
                            loseTrophies = -6;
                        }
                        if (600 <= brawlerTrophies && brawlerTrophies <= 699) 
                        {
                            winTrophies = 8;
                            loseTrophies = -7;
                        }
                        if (700 <= brawlerTrophies && brawlerTrophies <= 799) 
                        {
                            winTrophies = 8;
                            loseTrophies = -8;
                        }
                        if (800 <= brawlerTrophies && brawlerTrophies <= 899) 
                        {
                            winTrophies = 7;
                            loseTrophies = -9;
                        }
                        if (900 <= brawlerTrophies && brawlerTrophies <= 999) 
                        {
                            winTrophies = 6;
                            loseTrophies = -10;
                        }
                        if (1000 <= brawlerTrophies && brawlerTrophies <= 1099) 
                        {
                            winTrophies = 5;
                            loseTrophies = -11;
                        }
                        if (1100 <= brawlerTrophies && brawlerTrophies <= 1199) 
                        {
                            winTrophies = 4;
                            loseTrophies = -12;
                        }
                        if (brawlerTrophies >= 1200) 
                        {
                            winTrophies = 3;
                            loseTrophies = -12;
                        }
                    }

                    if (isVictory && BattleWithTrophies)
                    {
                        message.Result = 0;
                        message.TokensReward = 20;

                        int trophiesReward = winTrophies;
                        message.TrophiesReward = trophiesReward;

                        hero.AddTrophies(trophiesReward);
                        player.Avatar.AddTokens(20);
                        player.Avatar.TrioWins++;

                        player.Home.TokenReward += 20;
                        player.Home.TrophiesReward = LogicMath.Max(player.Home.TrophiesReward + trophiesReward, 0);
                    }
                    else if (m_winnerTeam != -1 && BattleWithTrophies)
                    {
                        message.Result = 1;
                        message.TokensReward = 10;

                        int trophiesReward = loseTrophies;
                        if (hero.Trophies < -trophiesReward) trophiesReward = -hero.Trophies;
                        message.TrophiesReward = trophiesReward;

                        hero.AddTrophies(trophiesReward);
                        player.Avatar.AddTokens(10);

                        player.Home.TokenReward += 10;
                        player.Home.TrophiesReward = LogicMath.Max(player.Home.TrophiesReward + trophiesReward, 0);
                    }
                }
                else
                {
                    message.IsPvP = BattleWithTrophies;
                    message.GameMode = 2;
                    message.Result = player.BattleRoyaleRank;
                    message.Players = new List<BattlePlayer>();
                    message.Players.Add(player);
                    message.OwnPlayer = player;
                    if (!BattleWithTrophies) {
                        message.BattleWithoutTrophies = true;
                    }

                    int tokensReward = 40 / rank;
                    message.TokensReward = tokensReward;
                    if (rank > 5 && BattleWithTrophies)
                    {
                        int trophiesReward = -(rank - 5);
                        if (hero.Trophies < -trophiesReward) trophiesReward = -hero.Trophies;
                        message.TrophiesReward = trophiesReward;

                        player.Avatar.AddTokens(tokensReward);
                        player.Home.TokenReward += tokensReward;
                        if (m_playersBySessionId.Count > 1) hero.AddTrophies(message.TrophiesReward);
                    }
                    else if (BattleWithTrophies)
                    {
                        int trophiesReward = (5 - rank) * 2;
                        message.TrophiesReward = trophiesReward;

                        player.Avatar.AddTokens(tokensReward);
                        player.Home.TokenReward += tokensReward;
                        player.Home.TrophiesReward += trophiesReward;

                        if (rank <= 4)
                        {
                            player.Avatar.AddStarTokens(1);
                            message.StarToken = true;
                            player.Home.StarTokenReward += 1;
                        }
                        if (m_playersBySessionId.Count > 1) hero.AddTrophies(message.TrophiesReward);
                    }
                }

                if (player.Avatar == null) continue;
                if (player.GameListener == null) continue;
                player.GameListener.SendTCPMessage(message);
            }
        }

        public void SendBattleEndToPlayer(BattlePlayer player)
        {
            Random rand = new Random();

            if (player.SessionId < 0) return;
            //if (!player.IsAlive) return;
            if (player.BattleRoyaleRank == -1) player.BattleRoyaleRank = 1;
            if (player.Avatar == null) return;
            int rank = player.BattleRoyaleRank;
            player.Avatar.BattleId = -1;

            bool isVictory = m_winnerTeam == player.TeamIndex;

            BattleEndMessage message = new BattleEndMessage();
            Hero hero = player.Avatar.GetHero(player.CharacterId);

            if (player.IsBot() == 0)
            {
                HomeMode homeMode = LogicServerListener.Instance.GetHomeMode(player.AccountId);
                if (homeMode != null)
                {
                    if (homeMode.Home.Quests != null)
                    {
                        message.ProgressiveQuests = homeMode.Home.Quests.UpdateQuestsProgress(m_gameModeVariation, player.CharacterId, player.Kills, player.Damage, player.Heals, homeMode.Home);
                    }
                }
            }

            if (m_gameModeVariation != 6)
            {
                message.GameMode = 1;
                message.IsPvP = BattleWithTrophies;
                message.Players = m_players;
                message.OwnPlayer = player;

                if (m_winnerTeam == -1) // Draw
                {
                    message.Result = 2;
                    message.TokensReward = 10;

                    message.TrophiesReward = 0;
                }

                if (isVictory)
                {
                    message.Result = 0;
                    message.TokensReward = 20;

                    int trophiesReward = 8;
                    message.TrophiesReward = trophiesReward;
                }
                else if (m_winnerTeam != -1)
                {
                    message.Result = 1;
                    message.TokensReward = 10;

                    int trophiesReward = -6;
                    if (hero.Trophies < -trophiesReward) trophiesReward = -hero.Trophies;
                    message.TrophiesReward = trophiesReward;
                }
            }
            else
            {
                message.IsPvP = BattleWithTrophies;
                message.GameMode = 2;
                message.Result = player.BattleRoyaleRank;
                message.Players = new List<BattlePlayer>();
                message.Players.Add(player);
                message.OwnPlayer = player;
                int tokensReward = 40 / rank;
                message.TokensReward = tokensReward;
                if (rank > 5)
                {
                    int trophiesReward = -(rank - 5);
                    if (hero.Trophies < -trophiesReward) trophiesReward = -hero.Trophies;
                    message.TrophiesReward = trophiesReward;
                }
                else
                {
                    int trophiesReward = (5 - rank) * 2;
                    message.TrophiesReward = trophiesReward;

                    if (rank <= 4)
                    {
                        message.StarToken = true;
                    }
                }
            }

            if (player.Avatar == null) return;
            if (player.GameListener == null) return;
            player.GameListener.SendTCPMessage(message);
        }


        public int GetTeamScore(int team)
        {
            int score = 0;
            foreach (BattlePlayer player in m_players)
            {
                if (player.TeamIndex == team) score += player.GetScore();
            }
            return score;
        }

        private bool CalculateIsGameOver()
        {
            if (m_gameModeVariation == 0)
            {
                if (GetTeamScore(0) > GetTeamScore(1) && GetTeamScore(0) >= 10)
                {
                    if (m_gemGrabCountdown == 0)
                    {
                        m_gemGrabCountdown = GetTicksGone() + 20 * 17;
                    }
                    else if (GetTicksGone() > m_gemGrabCountdown)
                    {
                        m_winnerTeam = 0;
                        return true;
                    }
                }
                else if (GetTeamScore(0) < GetTeamScore(1) && GetTeamScore(1) >= 10)
                {
                    if (m_gemGrabCountdown == 0)
                    {
                        m_gemGrabCountdown = GetTicksGone() + 20 * 17;
                    }
                    else if (GetTicksGone() > m_gemGrabCountdown)
                    {
                        m_winnerTeam = 1;
                        return true;
                    }
                }
                else
                {
                    m_gemGrabCountdown = 0;
                }
            }
            else if (m_gameModeVariation == 6)
            {
                if (m_playersAlive <= 1)
                {
                    return true;
                }
            }

            else if (GameModeUtil.HasTwoBases(m_gameModeVariation))
            {
                bool Base1Alive = false;
                bool Base2Alive = false;
                foreach (Character character in m_gameObjectManager.GetCharacters())
                {
                    if (character.CharacterData.IsBase())
                    {
                        if (character.GetIndex() / 16 == 0) Base1Alive = true;
                        else Base2Alive = true;
                    }
                }
                if (!Base1Alive || !Base2Alive)
                {
                    m_winnerTeam = Base1Alive ? 0 : 1;
                    if (!Base1Alive && !Base2Alive) m_winnerTeam = -1;
                    return true;
                }
            }
            if (GetTicksGone() >= GameModeUtil.GetBattleTicks(m_gameModeVariation) - 1)
            {
                if (m_gameModeVariation == 3)
                {
                    if (GetTeamScore(0) > GetTeamScore(1))
                    {
                        m_winnerTeam = 0;
                    }
                    else if (GetTeamScore(0) < GetTeamScore(1))
                    {
                        m_winnerTeam = 1;
                    }
                    else
                    {
                        m_winnerTeam = -1;
                    }
                }
                if (GameModeUtil.HasTwoBases(m_gameModeVariation))
                {
                    int Base1Alive = 0;
                    int Base2Alive = 0;
                    foreach (Character character in m_gameObjectManager.GetCharacters())
                    {
                        if (character.CharacterData.IsBase())
                        {
                            if (character.GetIndex() / 16 == 0) Base1Alive = character.GetHitpointPercentage();
                            else Base2Alive = character.GetHitpointPercentage();
                        }
                    }
                    if (Base1Alive > Base2Alive) m_winnerTeam = 0;
                    else if (Base1Alive < Base2Alive) m_winnerTeam = 1;
                    else m_winnerTeam = -1;
                }
                return true;
            }
            return false;
        }

        private void Tick()
        {
            m_gameObjectManager.Tick();
            m_tileMap.Tick(m_gameObjectManager);
            TickSpawnEventStuffDelayed();
            TickSpawnHeroes();
            TickPetrols();
            UpdatePlayerStatus();
        }
        private void TickPetrols()
        {
            List<Petrol> ToRemove = new List<Petrol>();
            foreach (Petrol v5 in m_gameObjectManager.Petrols)
            {
                if (v5.Tick(m_gameObjectManager))
                {
                    ToRemove.Add(v5);
                    foreach (Petrol v9 in m_gameObjectManager.Petrols)
                    {
                        if (v9.TeamIndex == v5.TeamIndex && LogicMath.Abs(v9.X - v5.X) <= 1 && LogicMath.Abs(v9.Y - v5.Y) <= 1) v9.Ignite(m_gameObjectManager, v5.Damage);
                    }
                }
            }
            foreach (Petrol v1 in ToRemove)
            {
                m_gameObjectManager.Petrols.Remove(v1);
            }
            //Debugger.Print(m_gameObjectManager.Petrols.Count);

        }
        
        public void UpdatePlayerStatus()
        {
            foreach (BattlePlayer player in m_players)
            {
                //v6 = *(a1[18] + 4 * i);
                if (m_gameObjectManager.GetGameObjectByID(player.OwnObjectId) == null && player.IsAlive && m_gameObjectManager.AddObjects.ToList().Find(obj => obj.GetGlobalID() == player.OwnObjectId) == null)
                {
                    player.IsAlive = false;
                    player.DeathTick = GetTicksGone();
                }

                Character character = (Character)m_gameObjectManager.GetGameObjectByID(player.OwnObjectId);
                player.UpdateOverCharge();
                if (character != null)
                {
                    //v8 = v7;
                    //ZN11LogicPlayer9coinsHeldEi(v6, *(v7 + 480));
                    Accessory v9 = player.Accessory;
                    if (v9 != null) v9.UpdateAccessory(character);
                    //if (*(v8 + 672))
                    //    ZN11LogicPlayer9coinsHeldEi(v6, 2);
                    //if (!ZN17LogicGameModeUtil10isSoloModeEi(a1[41]))
                    //{
                    //    if (*(v6 + 49) || a1[3] < DEPLOY_TICKS_FIRST_SPAWN)
                    //    {
                    //        *(v6 + 120) = 0;
                    //    }
                    //    else
                    //    {
                    //        v4 = a1[41];
                    //        v5 = v4 == 5;
                    //        if (v4 == 5)
                    //            v5 = *(a1 + 195) == 0;
                    //        if (!v5)
                    //            ++*(v6 + 120);
                    //    }
                    //}
                }
            }
        }
        private void SendVisionUpdateToPlayers()
        {
            Parallel.ForEach(m_players, player =>
            {
                if (player.GameListener != null)
                {
                    BitStream visionBitStream = new BitStream(64);
                    m_gameObjectManager.Encode(visionBitStream, m_tileMap, player.OwnObjectId, player.PlayerIndex, player.TeamIndex);

                    VisionUpdateMessage visionUpdate = new VisionUpdateMessage();
                    visionUpdate.Tick = GetTicksGone();
                    visionUpdate.HandledInputs = player.LastHandledInput;
                    visionUpdate.Viewers = m_spectators.Count;
                    visionUpdate.VisionBitStream = visionBitStream;

                    try
                    {
                        player.GameListener.SendMessage(visionUpdate);
                    }
                    catch (Exception)
                    {
                        RemovePlayer(player);
                    }
                    //bugger.Print("Send!");
                }
            });

            BitStream spectateStream = new BitStream(64);
            m_gameObjectManager.Encode(spectateStream, m_tileMap, 0, -1, -1);

            Task.Run(() =>
            {
                foreach (LogicGameListener gameListener in m_spectators.Values.ToArray())
                {
                    VisionUpdateMessage visionUpdate = new VisionUpdateMessage();
                    visionUpdate.Tick = GetTicksGone();
                    visionUpdate.HandledInputs = gameListener.HandledInputs;
                    visionUpdate.Viewers = m_spectators.Count;
                    visionUpdate.VisionBitStream = spectateStream;

                    gameListener.SendMessage(visionUpdate);
                }
            });
        }

        public BattlePlayer[] GetPlayers()
        {
            return m_players.ToArray();
        }

        public int GetRandomInt(int min, int max)
        {
            return m_random.Rand(max - min) + min;
        }

        public int GetRandomInt(int max)
        {
            return m_random.Rand(max);
        }

        public int GetTicksGone()
        {
            return m_time.GetTick();
        }

        public int GetGameModeVariation()
        {
            return m_gameModeVariation;
        }

        public int GetPlayersCountWithGameModeVariation()
        {
            return m_playersCountWithGameModeVariation;
        }

        public int GetRandomSeed()
        {
            return m_randomSeed;
        }

        public LocationData Location
        {
            get
            {
                return DataTables.Get(DataType.Location).GetDataByGlobalId<LocationData>(m_locationId);
            }
        }

        public long Id { get; set; }
    }
}
/*
 * update 12/21:
 * added Colette super logic
 * added charging logic
 * battle will end when server errors
 * update 12/28:
 * done emote logic and encode
 * (tried) to fix auto attack
 * decoded speed and slow CharacterServer
 * 
 * 1/23:
 * fix logic of speacial brawlers
 * 1/25:
 * fixed club
 * 2/2:
 * fixing attack logic
 * 2/5
 * ⬆️
 * 2/6
*/

/*
 * ChargeSpeed:(logic/20 ticks)
 * CastingRange:(tile)
 * tile to logic:*100
*/
