namespace Assets.Scripts
{
    using UnityEngine;

    public class SpawnerState
    {
        private readonly Game host;
        private readonly SpawnerStateGroup group;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public SpawnerState(Game host, Spawner spawner, SpawnerStateGroup group)
        {
            this.host = host;

            this.Id = spawner.AbsoluteId;
            this.Interval = spawner.interval;
            this.Mode = spawner.mode;
            this.group = group;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Id { get; private set; }

        public float Interval { get; private set; }
        public float PendingInterval { get; set; }
        
        public SpawnMode Mode { get; private set; }

        public SpawnerStateGroup Group
        {
            get
            {
                return this.group;
            }
        }
        
        public void Update(float updateTime)
        {
            // Update the spawn delays
            if (this.PendingInterval > 0)
            {
                this.PendingInterval -= updateTime;
            }

            // Check delay
            if (this.PendingInterval > 0)
            {
                return;
            }

            // Ensure 0
            this.PendingInterval = 0;

            if (this.group.CanSpawnEnemy)
            {
                this.host.SpawnEnemy(this);
                return;
            }

            this.host.Spawn(this);

            Debug.Log("Attempting SPAWN!");
            // Spawn a new instance
            /*IList<SpawnedEntity> spawnList = spawner.GetNextObject();
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
                    instance = (SpawnedEntity)Instantiate(entity, spawner.transform.position, spawner.transform.rotation);
                }

                Debug.Log("Spawning new Entity!");
                if (instance.LifeSpawn != null)
                {
                    this.spawnedEntityLifespan.Add(instance, instance.LifeSpawn.Value);
                }

                this.spawnedEntities[spawner.Group].Add(instance);
                this.spawnedInstanceCount[spawner.Group]++;
            }

            // Set the delay
            this.spawnDelay[spawner] = spawner.interval;*/
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        
    }
}
