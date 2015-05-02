﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class EnemySpawn : MonoBehaviour
{
	public List<Assets.Scripts.Enemy.Range> range;
	public List<Assets.Scripts.Enemy.Color> color;
    public List<float> spawnChance;
    public float respawnTime;

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(this.transform.position,new Vector3(1,2,1));
        Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
    }
}
