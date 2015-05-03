using UnityEngine;
using System.Collections;

public class Crossbow : Weapon
{
    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private Transform projectileLaunchPosition;

    protected override void AttackImpl(int baseDamage, int redDamage, int greenDamage, int blueDamage, Vector3 direction)
    {
        GameObject currentProjectile = Instantiate(this.projectile, this.projectileLaunchPosition.position, Quaternion.identity) as GameObject;
        currentProjectile.GetComponent<Projectile>().Init(baseDamage, redDamage, greenDamage, blueDamage, direction);
    }

    protected override void PointWeaponImpl(Vector3 direction)
    {
        this.transform.LookAt(this.transform.position + direction);
    }
}