using UnityEngine;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
	//public List<Assets.Scripts.Enemy.Range> range;
	//public List<Assets.Scripts.Enemy.Color> color;
    public List<float> spawnChance;
    public List<float> respawnTime;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
    }
}
