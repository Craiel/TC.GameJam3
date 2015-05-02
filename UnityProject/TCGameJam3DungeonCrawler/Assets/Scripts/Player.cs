namespace Assets.Scripts
{
    using UnityEngine;

    public class Player : Actor
    {
        [SerializeField]
        private Weapon crossbow;

        [SerializeField]
        private Weapon whip;

        [SerializeField]
        private Weapon axe;

        private Weapon[] weaponLoadout = new Weapon[2];
        private int activeWeaponIndex;

        private bool hasWeaponLoadout;

        public void SetWeaponLoadout(Weapon primaryWeapon, Weapon secondaryWeapon)
        {
            System.Diagnostics.Trace.Assert(primaryWeapon != secondaryWeapon);

            weaponLoadout[0] = primaryWeapon;
            weaponLoadout[1] = secondaryWeapon;
            activeWeaponIndex = 0;
            hasWeaponLoadout = true;
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