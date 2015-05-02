namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class Game : MonoBehaviour
    {
        private readonly IDictionary<long, SpawnerStateGroup> spawnerGroups;

        private readonly IDictionary<string, SpawnerState> activeSpawners; 

        private GameLevel level;

        public Player player;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Game()
        {
            this.activeSpawners = new Dictionary<string, SpawnerState>();
            this.spawnerGroups = new Dictionary<long, SpawnerStateGroup>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void Start()
        {
            this.level = new GameLevel(this);
            this.level.Start();
        }

        public void RegisterSpawner(ILevelSegment source, Spawner spawner)
        {
            System.Diagnostics.Trace.Assert(spawner != null);

            Debug.Log("Register Spawner!");

            SpawnerStateGroup group = this.GetGroup(spawner.Group, source);
            if (!this.activeSpawners.ContainsKey(spawner.AbsoluteId))
            {
                this.activeSpawners.Add(spawner.AbsoluteId, new SpawnerState(this, spawner, group));
            }
        }

        public void UnregisterSpawner(ILevelSegment source, Spawner spawner)
        {
            System.Diagnostics.Trace.Assert(spawner != null);

            Debug.Log("Unregister Spawner!");

            // Find and delete all entities still active by this spawner
            var group = this.GetGroup(spawner.Group, source);
            group.DestroyEntities(spawner.AbsoluteId);

            // Remove the spawner
            this.activeSpawners.Remove(spawner.AbsoluteId);
        }

        public void SpawnEnemy(SpawnerState state)
        {
            Spawner spawner = this.level.LocateSpawner(state.Id);

            IList<SpawnedEntity> spawnList = spawner.GetNextObject();
            foreach (SpawnedEntity entity in spawnList)
            {
                if (!(entity is BaseEnemy))
                {
                    continue;
                }

                BaseEnemy instance = (BaseEnemy)Instantiate(entity, spawner.transform.position, spawner.transform.rotation);
                instance.Init(this.player);

                state.Group.AddEnemyInstance(state, instance);
                state.PendingInterval = state.Interval;
            }
        }

        public void Spawn(SpawnerState state)
        {
            Spawner spawner = this.level.LocateSpawner(state.Id);

            IList<SpawnedEntity> spawnList = spawner.GetNextObject();
            foreach (SpawnedEntity entity in spawnList)
            {
                if (entity == null)
                {
                    continue;
                }

                SpawnedEntity instance;
                if (entity is BaseEnemy)
                {
                    instance = (BaseEnemy)Instantiate(entity, spawner.transform.position, spawner.transform.rotation);
                    ((BaseEnemy)instance).Init(this.player);
                }
                else
                {
                    Debug.Log("Spawning new Entity!");
                    instance = (SpawnedEntity)Instantiate(entity, spawner.transform.position, spawner.transform.rotation);
                }

                state.Group.AddInstance(state, instance);
                state.PendingInterval = state.Interval;
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private SpawnerStateGroup GetGroup(long groupId, ILevelSegment segment = null)
        {
            if (!this.spawnerGroups.ContainsKey(groupId))
            {
                System.Diagnostics.Trace.Assert(segment != null, "Group must be aquired with a valid segment!");
                this.spawnerGroups.Add(groupId, new SpawnerStateGroup(segment));
            }

            return this.spawnerGroups[groupId];
        }

        private void Update()
        {
            this.level.Update();

            float updateTime = Time.deltaTime;

            foreach (long key in this.spawnerGroups.Keys)
            {
                this.spawnerGroups[key].Update(updateTime);
            }

            foreach (string key in this.activeSpawners.Keys)
            {
                this.activeSpawners[key].Update(updateTime);
            }
        }

        
    }
}
