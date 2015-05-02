using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class Actor : MonoBehaviour
{
    [SerializeField]
    private int totalHitPoints;

    [SerializeField]
    Assets.Scripts.Enemy.Color color = Assets.Scripts.Enemy.Color.None;

    protected CharacterController characterController;

    public int HitPoints { get; private set; }

    public delegate void ActorEvent(Actor actor);
    public event ActorEvent OnActorKilled = delegate { };

    protected virtual void Awake()
    {
        this.characterController = GetComponent<CharacterController>();
        HitPoints = this.totalHitPoints;
    }

    public void TakeDamage(int baseDamage, int redDamage = 0, int greenDamage = 0, int blueDamage = 0)
    {
        int totalDamage = baseDamage;
        if (color == Assets.Scripts.Enemy.Color.Red)
            totalDamage += redDamage;
        if (color == Assets.Scripts.Enemy.Color.Green)
            totalDamage += greenDamage;
        if (color == Assets.Scripts.Enemy.Color.Blue)
            totalDamage += blueDamage;

        HitPoints -= totalDamage;
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
