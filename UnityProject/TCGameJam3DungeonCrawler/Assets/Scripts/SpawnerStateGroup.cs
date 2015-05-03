namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Contracts;

    using Object = UnityEngine.Object;

    public class SpawnerStateGroup : IDisposable
    {
        private readonly IList<SpawnedEntity> entities; 
        private readonly IDictionary<string, IList<SpawnedEntity>> entitiesBySpawner; 

        private readonly IDictionary<SpawnedEntity, float> entityLifespan;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public SpawnerStateGroup(ILevelSegment segment)
        {
            this.entities = new List<SpawnedEntity>();
            this.entitiesBySpawner = new Dictionary<string, IList<SpawnedEntity>>();
            this.entityLifespan = new Dictionary<SpawnedEntity, float>();

            this.Id = segment.InternalId;

            this.InstanceLimitActive = segment.Tile.TileData.spawnInstanceLimitActive;

            this.EnemyLimitTotal = segment.Tile.TileData.spawnEnemyLimitTotal;
            this.InstanceLimitTotal = segment.Tile.TileData.spawnInstanceLimitTotal;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long Id { get; private set; }

        public int InstanceLimitActive { get; set; }

        public int EnemyLimitTotal { get; set; }
        public int InstanceLimitTotal { get; set; }

        public int SpawnedEnemyCount { get; set; }
        public int SpawnedInstanceCount { get; set; }

        public bool CanSpawnEnemy
        {
            get
            {
                // Check if we can spawn another instance
                if (this.entities.Count >= this.InstanceLimitActive
                    || this.SpawnedEnemyCount >= this.EnemyLimitTotal)
                {
                    return false;
                }

                return true;
            }
        }

        public bool CanSpawn
        {
            get
            {
                // Check if we can spawn another instance
                if (this.entities.Count >= this.InstanceLimitActive
                    || this.SpawnedInstanceCount > this.InstanceLimitTotal)
                {
                    return false;
                }

                return true;
            }
        }

        public void Update(float updateTime)
        {
            // Order matters, don't want the lifespan to get an extra tick
            this.UpdateSpawnedEntityLifeSpan(updateTime);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DestroyEntities(string spawnerId)
        {
            if (!this.entitiesBySpawner.ContainsKey(spawnerId))
            {
                return;
            }

            foreach (SpawnedEntity entity in this.entitiesBySpawner[spawnerId])
            {
                this.entities.Remove(entity);
                Object.Destroy(entity.gameObject);
            }

            this.entitiesBySpawner.Remove(spawnerId);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateSpawnedEntityLifeSpan(float time)
        {
            IList<SpawnedEntity> deleteList = new List<SpawnedEntity>();

            foreach (SpawnedEntity entity in this.entities)
            {
                if (this.entityLifespan.ContainsKey(entity))
                {
                    this.entityLifespan[entity] -= time;

                    if (this.entityLifespan[entity] <= 0)
                    {
                        deleteList.Add(entity);
                    }
                }
            }

            foreach (SpawnedEntity entity in deleteList)
            {
                if (this.entities.Contains(entity))
                {
                    this.entities.Remove(entity);
                }

                foreach (string key in this.entitiesBySpawner.Keys)
                {
                    if (this.entitiesBySpawner[key].Contains(entity))
                    {
                        this.entitiesBySpawner[key].Remove(entity);
                    }
                }

                this.entityLifespan.Remove(entity);
                Object.Destroy(entity.gameObject);
            }
        }

        private void Dispose(bool isDisposing)
        {
            if (!isDisposing)
            {
                return;
            }

            foreach (SpawnedEntity entity in this.entities)
            {
                Object.Destroy(entity.gameObject);
            }
        }

        public void AddInstance(SpawnerState source, SpawnedEntity instance)
        {
            this.entities.Add(instance);
            this.SpawnedInstanceCount++;

            if (!this.entitiesBySpawner.ContainsKey(source.Id))
            {
                this.entitiesBySpawner.Add(source.Id, new List<SpawnedEntity>());
            }

            this.entitiesBySpawner[source.Id].Add(instance);

            if (instance.lifeSpan > 0)
            {
                this.entityLifespan.Add(instance, instance.lifeSpan);
            }
        }

        public void AddEnemyInstance(SpawnerState source, BaseEnemy instance)
        {
            this.AddInstance(source, instance);

            this.SpawnedEnemyCount++;
        }
    }
}
