using UnityEngine;
using System.Collections;

public class Turret : BaseEnemy
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject neckBone;

    [SerializeField]
    private GameObject projectile;

    [SerializeField]
    private int projectileDamage;

    [SerializeField]
    private Transform projectileLaunch;

    protected override void Idle()
    {
        this.animator.SetBool("IsFiring", false);
    }

    protected override void Patrol()
    {
        this.animator.SetBool("IsFiring", false);
    }

    protected override void Chase()
    {
 
    }

    protected override void Attack()
    {
        this.animator.SetBool("IsFiring", true);

        Vector3 direction = (this.player.transform.position - this.projectileLaunch.position).normalized;

        this.neckBone.transform.LookAt(this.player.transform);
        this.neckBone.transform.Rotate(this.neckBone.transform.up, -90f);

        GameObject projectile = Instantiate(this.projectile) as GameObject;
        projectile.transform.position = this.projectileLaunch.position;
        projectile.GetComponent<Projectile>().Init(this.projectileDamage, 0, 0, 0, direction);
    }
}