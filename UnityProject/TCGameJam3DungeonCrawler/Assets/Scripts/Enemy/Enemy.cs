using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(CharacterController))]
public abstract class Enemy : Actor
{
    public enum State
    {
        Idle,
        Attacking
    }
    private State currentState = State.Idle;

    protected Player player;

    public virtual void Init(Player player)
    {
        this.player = player;
    }

    protected abstract bool CanDetectPlayer();
    protected abstract void Attack();
    protected abstract void Idle();

    protected virtual void Update()
    {
        UpdateState();
        ResolveAction();
    }

    private void UpdateState()
    {
        bool canDetectPlayer = CanDetectPlayer();
        if (this.currentState == State.Idle && canDetectPlayer)
        {
            this.currentState = State.Attacking;
        }
        else if (this.currentState == State.Attacking && !canDetectPlayer)
        {
            this.currentState = State.Idle;
        }   
    }

    private void ResolveAction()
    {
        if(this.currentState == State.Idle)
        {
            Idle();
        }
        else if(this.currentState == State.Attacking)
        {
            Attack();
        }
    }
}