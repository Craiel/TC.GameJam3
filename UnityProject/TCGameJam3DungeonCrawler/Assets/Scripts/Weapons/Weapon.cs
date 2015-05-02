using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private int baseDamage;

    [SerializeField]
    private int energyConsumedOnAttack;

    [SerializeField]
    private bool hasPointingDirection;

    [SerializeField]
    private float cooldownTime;

    private int energy;

    private float currentCooldownTime;

    private Vector3 pointingDirection;

    private void Update()
    {
        if(this.hasPointingDirection)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            this.pointingDirection = (cursorWorldPosition - this.transform.position).normalized;
        }

        if(this.currentCooldownTime > 0)
        {
            this.currentCooldownTime -= Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0) && this.currentCooldownTime <= 0f)
        {
            Attack();
            this.currentCooldownTime = this.cooldownTime;
        }
    }

    private void Attack()
    {
        int damage = this.baseDamage;
        if (this.energy > 0)
        {
            damage += GetEnergyDamage(this.energy);
        }

        AttackImpl(damage, pointingDirection);

        this.energy -= this.energyConsumedOnAttack;
        if (this.energy < 0)
        {
            this.energy = 0;
        }
    }

    //TODO: Define actual Energy Damage curve
    private int GetEnergyDamage(int energy)
    {
        return energy;
    }

    protected abstract void AttackImpl(int totalDamage, Vector3 direction);
}