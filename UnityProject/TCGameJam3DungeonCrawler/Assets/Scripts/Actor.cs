using Assets.Scripts;

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Actor : SpawnedEntity
{
    [SerializeField]
    private int totalHitPoints;

    protected CharacterController characterController;

    public int HitPoints { get; private set; }

    public delegate void ActorEvent(Actor actor);
    public event ActorEvent OnActorKilled = delegate { };

    protected virtual void Awake()
    {
        this.characterController = GetComponent<CharacterController>();
        HitPoints = this.totalHitPoints;
    }

    public virtual void TakeDamage(int baseDamage, int redDamage = 0, int greenDamage = 0, int blueDamage = 0)
    {
        HitPoints -= baseDamage;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnActorKilled(this);
        Destroy(this.gameObject);
    }
}
