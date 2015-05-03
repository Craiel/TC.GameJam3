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
            
            if (this.group.CanSpawn)
            {
                this.host.Spawn(this);
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        
    }
}
