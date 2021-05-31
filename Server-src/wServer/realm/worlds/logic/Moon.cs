using common.resources;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking;
using wServer.realm.entities;
using wServer.realm.setpieces;
using wServer.realm.terrain;

namespace wServer.realm.worlds.logic
{
    public class MoonLord
    {
        public bool Closing;

        private static readonly ILog Log = LogManager.GetLogger(typeof(Oryx));

        private readonly World _world;
        public MoonLord(World world)
        {
            _world = world;
        }
        private void BroadcastMsg(string message)
        {
            _world.Manager.Chat.Oryx(_world, message);
        }
        public void InitCloseRealm()
        {
            Closing = true;
            _world.Manager.Chat.Announce("Moon closing in 1 minute.", true);
            _world.Timers.Add(new WorldTimer(60000, (w, t) => CloseRealm()));
        }

        private void CloseRealm()
        {
            _world.Closed = true;
            BroadcastMsg("I HAVE CLOSED THIS REALM!");
            BroadcastMsg("YOU WILL NOT LIVE TO SEE THE LIGHT OF DAY!");

            _world.Timers.Add(new WorldTimer(22000, (w, t) => SendToCastle()));
        }

        private void SendToCastle()
        {
            /*BroadcastMsg("MY MINIONS HAVE FAILED ME!");
            BroadcastMsg("BUT NOW YOU SHALL FEEL MY WRATH!");
            BroadcastMsg("COME MEET YOUR DOOM AT THE WALLS OF MY CASTLE!");*/

            if (_world.Players.Count <= 0)
                return;
            var world = _world.Manager.AddWorld(
                new MoonCastle(_world.Manager.Resources.Worlds.Data["Moon Castle"], playersEntering: _world.Players.Count));
            _world.QuakeToWorld(world);
            _world.Timers.Add(new WorldTimer(22000, (w, t) => _world.Closed = false));
        }
    }
    public class MoonEntityManager
    {
        struct MoonEvent
        {
            public string ObjectID { get; private set; }
            public ISetPiece SetPiece { get; private set; }
            public MoonEvent(string id, ISetPiece setPiece)
            {
                ObjectID = id;
                SetPiece = setPiece;
            }
        }

        struct RegionMob
        {
            public int SpawnLimit { get; private set; }
            public WmapTerrain Ground { get; private set; }
            public Tuple<string, double>[] SpawnData { get; private set; }

            public RegionMob(WmapTerrain ground, int limit, Tuple<string, double>[] spawnData)
            {
                Ground = ground;
                SpawnLimit = limit;
                SpawnData = spawnData;
            }

            public ushort GroundObjectType()
            {
                switch (Ground)
                {
                    case WmapTerrain.Moon3: return 0x8556;
                    case WmapTerrain.Moon2: return 0x8555;
                    case WmapTerrain.Moon1: return 0x8554;
                    default: return 0x8556;
                }
            }
        }

        static readonly ILog Log = LogManager.GetLogger(typeof(Moon));

        readonly List<RegionMob> _regionMobs = new List<RegionMob>()
        {
            //make sure all the decimals add up to 1

            new RegionMob(WmapTerrain.Moon3, 140, new[] //lowest 
            {
                Tuple.Create("Skeleton Astronaut", .5),
                Tuple.Create("Commander of the Cult", .3),//Skeleton Astronaut
                Tuple.Create("Lunar Protector", .2)
            }),
            new RegionMob(WmapTerrain.Moon2, 160, new [] //middle
            {
                Tuple.Create("Purple Moon Slime", .4),
                Tuple.Create("Mutated Venenum", .2),//Purple Moon Slime
                Tuple.Create("Actias Luna", .4)
            }),
            new RegionMob(WmapTerrain.Moon1, 40, new [] //highest
            {
                Tuple.Create("Skeleton Astronaut", .3),
                Tuple.Create("Spinning Tornado",   0.4),
                Tuple.Create("Moon Statue",   0.3)
            })
        };

        readonly List<MoonEvent> _eventMobs = new List<MoonEvent>()
        {
            new MoonEvent("Eternal, The Devourer", (ISetPiece) new MEternal()),
            new MoonEvent("The Jade Rabbit", (ISetPiece) new MJadeRabbit()),
            new MoonEvent("Lunar: Lord of the Lost Lands", (ISetPiece) new Mlotll()),
            new MoonEvent("Lunar: Grand Sphinx", (ISetPiece) new LSphinx()),
            new MoonEvent("Lunar: Cube God", (ISetPiece) new LCubeGod()),
            new MoonEvent("Celestial Observer", (ISetPiece) new MCelestial()),
            new MoonEvent("Eternal, The Devourer", (ISetPiece) new MEternal())


            //new MoonEvent("Dragon Head", (ISetPiece) new RockDragon()),
            //new MoonEvent("The Keyper", (ISetPiece) new Keyper())


        };

        readonly int REGION_OFFSET = 14;
        readonly int MAX_ACTIVE_EVENTS = 3;

        World _parent;
        int _worldWidth;
        int _worldHeight;

        int[] _mobCount;
        int _activeEvents;
        List<string> _spawnedEvents;

        public MoonLord _overseer;

        public MoonEntityManager(World world)
        {
            _parent = world;

            _worldWidth = _parent.Map.Width;
            _worldHeight = _parent.Map.Height;

            _mobCount = new int[3];
            _spawnedEvents = new List<string>();

            _activeEvents = 0;

            recalculatePopulation();
            ensurePopulation();
            checkEvents();
            _overseer = new MoonLord(world);
        }

        int _prevSec;
        public void Tick(RealmTime time)
        {
            var secs = (int)(time.TotalElapsedMs / 1000);

            if (_prevSec != secs)
            {
                if (secs % 30 == 0) //every 10 seconds
                    recalculatePopulation();
                if (secs % 60 == 0) //every one minute
                    ensurePopulation();
                if (secs % 120 == 0) //every three minutes
                    checkEvents();
                if (secs % 3000 == 0) //50mins
                    CloseRealm();

            }

            _prevSec = secs;
        }
        public bool CloseRealm()
        {
            if (_overseer == null)
                return false;

            _overseer.InitCloseRealm();
            return true;
        }

        public void onEnemyKilled(Entity enemy, Player killer)
        {
            if (_eventMobs.Select(_ => _.ObjectID).Contains(enemy.ObjectDesc.ObjectId))
            {
                _activeEvents--;

                _parent.Manager.Chat.Announce($"{enemy.ObjectDesc.ObjectId} has been killed by { ( (killer != null) ? killer.Name : string.Empty )}!");
            }
        }

        void checkEvents()
        {
            if (_activeEvents > 1)
                return;

            var toSpawn = MAX_ACTIVE_EVENTS - _activeEvents;
            var leftToSpawn = _eventMobs.Where(_ => !_spawnedEvents.Contains(_.ObjectID)).ToList();
            //var leftToSpawn = _eventMobs;

            if (leftToSpawn.Count == 0)
                spawnFinalBoss();
            else
                for (var i = 0; i < toSpawn; i++)
                {
                    var eventMob = leftToSpawn[RandomUtil.RandInt(0, leftToSpawn.Count() - 1)];

                    spawnEvent(eventMob);
                }

            Log.Info("Events respawned!");
        }

        void spawnFinalBoss()
        {
            //TODO
        }

        void spawnEvent(MoonEvent e)
        {
            int x, y;

            do
            {
                x = RandomUtil.RandInt(50, _worldWidth - 50);
                y = RandomUtil.RandInt(50, _worldHeight - 50);
            } while (!_parent.IsPassable(x, y) ||
            _parent.AnyPlayerNearby(x, y, 50) || 
            _parent.Map[x, y].TileId == 0x8556
            );

            var setPiece = e.SetPiece;
            x -= setPiece.Size / 2;
            y -= setPiece.Size / 2;

            setPiece.RenderSetPiece(_parent, new IntPoint(x, y));

            _activeEvents++;
        }

        void recalculatePopulation()
        {
            _mobCount[(int)WmapTerrain.Moon1 - REGION_OFFSET] = 0;
            _mobCount[(int)WmapTerrain.Moon2 - REGION_OFFSET] = 0;
            _mobCount[(int)WmapTerrain.Moon3 - REGION_OFFSET] = 0;

            foreach (var en in _parent.Enemies.Values)
            {
                if (en.Terrain == WmapTerrain.None ||
                    (en.Terrain != WmapTerrain.Moon1 && en.Terrain != WmapTerrain.Moon2 && en.Terrain != WmapTerrain.Moon3))
                    continue;

                _mobCount[(int)en.Terrain - REGION_OFFSET]++;
            }

            Log.Info("Population Recalculated!");
        }
        void ensurePopulation()
        {
            foreach (var i in _regionMobs)
            {
                var toSpawn = i.SpawnLimit - _mobCount[(int)i.Ground - REGION_OFFSET];

                if (toSpawn > 0)
                    respawnPopulation(i.Ground, toSpawn);
            }
            Log.Info("Population respawned! " + _parent.Enemies.Count);
        }

        void respawnPopulation(WmapTerrain region, int amount)
        {
            var toSpawn = new List<string>();
            var ret = 0;
            var regionType = _regionMobs[(int)region - REGION_OFFSET].GroundObjectType();

            do
            {
                var mobData = _regionMobs[(int)region - REGION_OFFSET].SpawnData;

                foreach(var data in mobData)
                {
                    var rand = RandomUtil.RandDouble();

                    if (rand < data.Item2)
                    {
                        toSpawn.Add(data.Item1);
                        ret++;
                    }
                }
            } while (amount > ret);

            foreach(var mob in toSpawn)
            {
                int x, y;

                do
                {
                    x = RandomUtil.RandInt(0, _worldWidth);
                    y = RandomUtil.RandInt(0, _worldHeight);

                } while (!_parent.IsPassable(x, y) || _parent.AnyPlayerNearby(x, y, 40) || _parent.Map[x, y].TileId != regionType);

                var entity = Entity.Resolve(_parent.Manager, mob) as Enemy;

                entity.Move(x, y);
                entity.Terrain = region;

                _parent.EnterWorld(entity);
            }
        }

    }

    public class Moon : World
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(Moon));

        MoonEntityManager _entityManager;

        public Moon(ProtoWorld proto, Client client = null) : base(proto)
        {
        }

        protected override void Init()
        {
            base.Init();

            _entityManager = new MoonEntityManager(this);
            Log.Info("Moon initialized");
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time);

            _entityManager.Tick(time);
        }

        public void OnEnemyKilled(Entity enemy, Player player)
        {
            _entityManager.onEnemyKilled(enemy, player);
        }
        
    }
}
