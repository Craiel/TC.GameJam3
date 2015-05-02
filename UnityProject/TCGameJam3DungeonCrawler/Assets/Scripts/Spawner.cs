namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class Spawner : MonoBehaviour
    {
        private int nextSequence;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int id;

        public List<SpawnedEntity> spawnList;

        public float interval;

        public int instanceLimitActive;
        public int instanceLimitTotal;

        public SpawnMode mode;

        public string AbsoluteId { get; set; }
        public long Group { get; set; }

        public IList<SpawnedEntity> GetNextObject()
        {
            switch (this.mode)
            {
                    case SpawnMode.All:
                    {
                        return this.spawnList;
                    }

                    case SpawnMode.Random:
                    {
                        var index = Random.Range(0, this.spawnList.Count);
                        return new List<SpawnedEntity> { this.spawnList[index] };
                    }

                    case SpawnMode.Sequence:
                    {
                        var result = new List<SpawnedEntity>() { this.spawnList[this.nextSequence++] };
                        if (this.nextSequence >= this.spawnList.Count - 1)
                        {
                            this.nextSequence = 0;
                        }

                        return result;
                    }

                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
        }
    }
}
