namespace Assets.Scripts.Enemy
{
    using UnityEngine;

    public class EnemyMeleeBasic : BaseEnemy
    {
        protected override void Idle()
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }

        protected override void Patrol()
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }

        protected override void Chase()
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }

        protected override void Attack()
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 0.1f);
        }
    }
}
