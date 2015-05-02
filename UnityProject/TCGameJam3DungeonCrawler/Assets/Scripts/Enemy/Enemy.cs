﻿using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(CharacterController))]
public abstract class Enemy : Actor
{
    private const float RELEVANT_ACTION_RANGE = 50f;

    [SerializeField]
    protected int attackDamage;

    [SerializeField]
    protected float attackCooldown;

    [SerializeField]
    private float detectionRange;
    
    [SerializeField]
    private float attackRange;

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

        float distanceToPlayer = (this.player.transform.position - this.transform.position).magnitude;
        if(distanceToPlayer <= RELEVANT_ACTION_RANGE)
        {
            if(distanceToPlayer <= this.detectionRange)
            {
                if (distanceToPlayer <= this.attackRange && this.currentAttackCooldown < 0)
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
}