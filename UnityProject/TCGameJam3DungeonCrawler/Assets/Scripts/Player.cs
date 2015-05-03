namespace Assets.Scripts
{
    using Assets.Scripts.Enemy;
    using System;
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

        public bool HasWeaponLoadout { get { return this.hasWeaponLoadout; } }

        public Weapon PrimaryWeapon { get { return this.weaponLoadout[0]; } }
        public Weapon SecondaryWeapon { get { return this.weaponLoadout[1]; } }

        public Weapon ActiveWeapon { get { return this.weaponLoadout[this.activeWeaponIndex]; } }

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

        private void Start()
        {
            if(GameLoadout.Instance != null)
            {
                SetWeaponLoadout(GameLoadout.Instance.Weapons[0], GameLoadout.Instance.Weapons[1]);
            }
            else
            {
                SetWeaponLoadout(typeof(Axe), typeof(Crossbow));
            }
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
                this.weaponLoadout[activeWeaponIndex].gameObject.SetActive(false);
                this.weaponLoadout[activeWeaponIndex].IsActive = false;
                activeWeaponIndex = 1 - activeWeaponIndex;
                this.weaponLoadout[activeWeaponIndex].gameObject.SetActive(true);
                this.weaponLoadout[activeWeaponIndex].IsActive = true;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                activeWeapon.Attack();
            }
        }

        private void SetWeaponLoadout(Type primaryWeaponType, Type secondaryWeaponType)
        {
            this.weaponLoadout[0] = GetWeapon(primaryWeaponType);
            this.weaponLoadout[1] = GetWeapon(secondaryWeaponType);
            activeWeaponIndex = 0;

            this.weaponLoadout[0].gameObject.SetActive(true);
            this.weaponLoadout[1].gameObject.SetActive(false);
            this.weaponLoadout[0].IsActive = true;

            hasWeaponLoadout = true;
        }

        private Weapon GetWeapon(Type weaponType)
        {
            if(weaponType == typeof(Axe))
            {
                return this.axe;
            }
            else if (weaponType == typeof(Whip))
            {
                return this.whip;
            }
            else if (weaponType == typeof(Crossbow))
            {
                return this.crossbow;
            }
            return null;
        }
    }
}
