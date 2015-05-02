using UnityEngine;
using Assets.Scripts;

[RequireComponent(typeof(CharacterController))]
public abstract class BaseEnemy : Actor
{
    public enum State
    {
        Idle,
        Attacking
    }

    private State currentState = State.Idle;

    [SerializeField]
    Assets.Scripts.Enemy.PowerColor color = Assets.Scripts.Enemy.PowerColor.None;

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