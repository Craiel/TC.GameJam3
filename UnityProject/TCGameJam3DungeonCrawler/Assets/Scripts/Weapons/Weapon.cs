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

    protected abstract void PointWeaponImpl(Vector3 direction);
    protected abstract void AttackImpl(int totalDamage, Vector3 direction);

    private void Update()
    {
        if(this.hasPointingDirection)
        {
            Plane xyPlane = new Plane(Vector3.forward, Vector3.zero);
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if(xyPlane.Raycast(cameraRay, out distance))
            {
                Vector3 position = cameraRay.GetPoint(distance);
                this.pointingDirection = (position - this.transform.position).normalized;
                PointWeaponImpl(this.pointingDirection);
            }
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
}