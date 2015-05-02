using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour
{
    public enum enemyTypes
    {
        Close,
        Medium,
        Long,
        Random
    }
    public enum enemyColors
    {
        Red,
        Blue,
        Green,
        Random
    }
    public List<enemyTypes> enemyType;
    public List<enemyColors> enemyColor;
    public List<float> enemySpawnChance;
    public float respawnTime;

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(this.transform.position,new Vector3(1,2,1));
        Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
    }
}
