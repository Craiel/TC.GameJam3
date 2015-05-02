using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseEnemy : Actor
{
    private const float RELEVANT_ACTION_RANGE = 20f;

    [SerializeField]
    protected int attackDamage;

    [SerializeField]
    protected float attackCooldown;

    [SerializeField]
    private float detectionRange;
    
    [SerializeField]
    private float attackRange;
    
    [SerializeField]
    Assets.Scripts.Enemy.PowerColor color = Assets.Scripts.Enemy.PowerColor.None;
    
    private float currentAttackCooldown;

    protected Player player;
    
    public virtual void Init(Player player)
    {
        this.player = player;
    }

    protected abstract void Idle();
    protected abstract void Patrol();
    protected abstract void Chase();
    protected abstract void Attack();
    
    protected virtual void Update()
    {
        if (this.currentAttackCooldown > 0)
        {
            this.currentAttackCooldown -= Time.deltaTime;
        }

        if (this.player == null)
        {
            return;
        }

        float distanceToPlayer = (this.player.transform.position - this.transform.position).magnitude;
        if(distanceToPlayer <= RELEVANT_ACTION_RANGE)
        {
            if(distanceToPlayer <= this.detectionRange)
            {
                RaycastHit hitInfo;
               
                if (distanceToPlayer <= this.attackRange && this.currentAttackCooldown <= 0 &&
                    Physics.Raycast(new Ray(this.transform.position, (this.player.transform.position - this.transform.position).normalized), out hitInfo) &&
                    hitInfo.collider.gameObject == this.player.gameObject)
                {
                    Attack();
                    this.currentAttackCooldown = this.attackCooldown;
                }
                else
                {
                    Chase();
                }
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            Idle();
        }
    }

    public override void TakeDamage(int baseDamage, int redDamage = 0, int greenDamage = 0, int blueDamage = 0)
    {
        int totalDamage = baseDamage;
        if (color == Assets.Scripts.Enemy.PowerColor.Red)
            totalDamage += redDamage;
        if (color == Assets.Scripts.Enemy.PowerColor.Green)
            totalDamage += greenDamage;
        if (color == Assets.Scripts.Enemy.PowerColor.Blue)
            totalDamage += blueDamage;

        base.TakeDamage(totalDamage, redDamage, greenDamage, blueDamage);
    }
}