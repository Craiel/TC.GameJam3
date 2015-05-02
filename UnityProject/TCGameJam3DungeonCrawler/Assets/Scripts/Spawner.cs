namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    public class Spawner : MonoBehaviour
    {
        public List<GameObject> spawnList;

        public float interval;

        public int activeInstanceLimit;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
        }
    }
}
