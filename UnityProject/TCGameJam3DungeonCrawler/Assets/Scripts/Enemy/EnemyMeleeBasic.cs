namespace Assets.Scripts.Enemy
{
    using UnityEngine;

    public class EnemyMeleeBasic : BaseEnemy
    {
        protected override bool CanDetectPlayer()
        {
            return true;
        }

        protected override void Attack()
        {
            return;
        }

        protected override void Idle()
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }
    }
}
