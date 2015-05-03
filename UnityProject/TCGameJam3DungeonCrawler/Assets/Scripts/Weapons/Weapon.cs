using UnityEngine;
using System.Collections.Generic;

using Assets.Scripts;
using Assets.Scripts.Enemy;

public abstract class Weapon : MonoBehaviour
{
    public const int ENERGY_STREAM_MAX = 100;

    [SerializeField]
    private int baseDamage;

    [SerializeField]
    private int energyConsumedOnAttack;

    [SerializeField]
    private bool hasPointingDirection;

    [SerializeField]
    private float cooldownTime;

    private Dictionary<PowerColor, int> energyStreams = new Dictionary<PowerColor, int>();

    private float currentCooldownTime;

    private Vector3 pointingDirection;

    public bool HasPointingDirection { get { return this.hasPointingDirection; } }
    public Dictionary<PowerColor, int> EnergyStreams { get { return this.energyStreams; } }

    protected abstract void PointWeaponImpl(Vector3 direction);
    protected abstract void AttackImpl(int baseDamage, int redDamage, int greenDamage, int blueDamage, Vector3 direction);

    protected virtual void Start()
    {
        energyStreams.Add(PowerColor.Red, 0);
        energyStreams.Add(PowerColor.Green, 0);
        energyStreams.Add(PowerColor.Blue, 0);
    }

    protected virtual void Update()
    {
        if(this.currentCooldownTime > 0)
        {
            this.currentCooldownTime -= Time.deltaTime;
        }
    }

    public void PointWeapon()
    {
        if (this.hasPointingDirection)
        {            
            Plane xyPlane = new Plane(Vector3.forward, new Vector3(0, 0, CharacterMovementController.zAxis));
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if (xyPlane.Raycast(cameraRay, out distance))
            {
                Vector3 position = cameraRay.GetPoint(distance);
                this.pointingDirection = (position - this.transform.position).normalized;
                PointWeaponImpl(this.pointingDirection);
            }
        }
    }

    public void Attack()
    {
        if (this.currentCooldownTime <= 0f)
        {
            AttackImpl(this.baseDamage, 
                        GetEnergyDamage(this.energyStreams[PowerColor.Red]),
                        GetEnergyDamage(this.energyStreams[PowerColor.Green]),
                        GetEnergyDamage(this.energyStreams[PowerColor.Blue]), 
                        pointingDirection);

            this.energyStreams[PowerColor.Red] = Mathf.Max(this.energyStreams[PowerColor.Red] - this.energyConsumedOnAttack, 0);
            this.energyStreams[PowerColor.Green] = Mathf.Max(this.energyStreams[PowerColor.Green] - this.energyConsumedOnAttack, 0);
            this.energyStreams[PowerColor.Blue] = Mathf.Max(this.energyStreams[PowerColor.Blue] - this.energyConsumedOnAttack, 0);
    
            this.currentCooldownTime = this.cooldownTime;
        }
    }

    public void ChargeEnergy(PowerColor powerColor, int quantity)
    {
        this.energyStreams[powerColor] += quantity;

        if(this.energyStreams[powerColor] > ENERGY_STREAM_MAX)
        {
            this.energyStreams[powerColor] = ENERGY_STREAM_MAX;
        }
    }
   
    private int GetEnergyDamage(int energy)
    {
        return energy;
    }
}