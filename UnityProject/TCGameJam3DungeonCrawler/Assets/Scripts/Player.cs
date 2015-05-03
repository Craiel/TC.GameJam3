namespace Assets.Scripts
{
    using Assets.Scripts.Enemy;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : Actor
    {
        private const int ENERGY_STREAM_MAX = 100;

        [SerializeField]
        private Weapon crossbow;

        [SerializeField]
        private Weapon whip;

        [SerializeField]
        private Weapon axe;

        private Weapon[] weaponLoadout = new Weapon[2];
        private int activeWeaponIndex;

        private bool hasWeaponLoadout;

        private Dictionary<PowerColor, int> energyStreams = new Dictionary<PowerColor,int>();

        public void SetWeaponLoadout(Weapon primaryWeapon, Weapon secondaryWeapon)
        {
            System.Diagnostics.Trace.Assert(primaryWeapon != secondaryWeapon);

            weaponLoadout[0] = primaryWeapon;
            weaponLoadout[1] = secondaryWeapon;
            activeWeaponIndex = 0;
            hasWeaponLoadout = true;
        }

        public void ChargeEnergy(PowerColor powerColor, int quantity)
        {
            this.energyStreams[powerColor] += quantity;

            if (this.energyStreams[powerColor] > ENERGY_STREAM_MAX)
            {
                this.energyStreams[powerColor] = ENERGY_STREAM_MAX;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            this.energyStreams.Add(PowerColor.Red, 0);
            this.energyStreams.Add(PowerColor.Green, 0);
            this.energyStreams.Add(PowerColor.Blue, 0);
        }

        private void Update()
        {
            if(!hasWeaponLoadout)
            {
                return;
            }

            Weapon activeWeapon = weaponLoadout[activeWeaponIndex];
            activeWeapon.PointWeapon();

            if(Input.GetMouseButtonDown(1))
            {
                activeWeaponIndex = 1 - activeWeaponIndex;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                activeWeapon.Attack();
            }
        }
    }
}