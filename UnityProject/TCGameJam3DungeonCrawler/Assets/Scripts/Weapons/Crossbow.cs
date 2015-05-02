using UnityEngine;
using System.Collections;

public class Crossbow : Weapon
{
    protected override void AttackImpl(int totalDamage, Vector3 pointingDirection)
    {
        Debug.Log("Pew PEW!!!!!");
    }
}