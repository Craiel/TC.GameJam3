using UnityEngine;
using System.Collections;

public class Crossbow : Weapon
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private Transform projectileLaunchPosition;

    protected override void AttackImpl(int totalDamage, Vector3 pointingDirection)
    {
        GameObject currentProjectile = Instantiate(this.projectile, this.projectileLaunchPosition.position, Quaternion.identity) as GameObject;
        currentProjectile.GetComponent<Projectile>().Init(totalDamage, pointingDirection);
    }

    protected override void PointWeaponImpl(Vector3 direction)
    {
        this.transform.LookAt(this.transform.position + direction);
    }
}