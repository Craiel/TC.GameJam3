using UnityEngine;
using System.Collections;

public class Flier : BaseEnemy
{
    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float maxAcceleration;

    private Vector3 currentVelocity;
    private Vector3 currentAcceleration;

    protected override void Update()
    {
        base.Update();

        if(this.currentVelocity != default(Vector3) || this.currentAcceleration != default(Vector3))
        {
            this.currentAcceleration = ClampMaximum(this.currentAcceleration, this.maxAcceleration);
            this.currentVelocity += this.currentAcceleration * Time.deltaTime;
            this.currentVelocity = ClampMaximum(this.currentVelocity, this.maxSpeed);
            this.characterController.Move(this.currentVelocity * Time.deltaTime);
        }
    }

    protected override void Idle()
    {
        this.currentAcceleration = Vector3.zero;
        this.currentVelocity = Vector3.zero;
    }

    protected override void Patrol()
    {
        this.currentAcceleration += new Vector3(Random.Range(-this.maxAcceleration, this.maxAcceleration),
                                                Random.Range(-this.maxAcceleration, this.maxAcceleration),
                                                Random.Range(-this.maxAcceleration, this.maxAcceleration));
    }

    protected override void Chase()
    {
        this.currentAcceleration = (this.player.transform.position - this.transform.position).normalized * this.maxAcceleration;
    }

    protected override void Attack()
    {
        this.player.TakeDamage(this.attackDamage);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /*
        if((this.characterController.collisionFlags & CollisionFlags.Above) != 0 ||
            (this.characterController.collisionFlags & CollisionFlags.Below) != 0)
        {
            this.curre
        }
        //this.currentAcceleration = Vector3.zero;
        //this.currentVelocity = -this.currentVelocity;
         * */
    }

    private Vector3 ClampMaximum(Vector3 vector, float maximum)
    {
        if(vector.magnitude > maximum)
        {
            return vector.normalized * maximum;
        }
        return vector;
    }
}
