using UnityEngine;
using System.Collections;

public class MeleeFighter : BaseEnemy
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

        if (this.currentVelocity != default(Vector3) || this.currentAcceleration != default(Vector3))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
            this.currentAcceleration.z = 0f;
            this.currentAcceleration = ClampMaximum(this.currentAcceleration, this.maxAcceleration);
            this.currentVelocity += this.currentAcceleration * Time.deltaTime;
            this.currentVelocity.z = 0f;
            this.currentVelocity = ClampMaximum(this.currentVelocity, this.maxSpeed);
            this.characterController.SimpleMove(this.currentVelocity);
        }
    }

    protected override void Idle()
    {
        this.currentAcceleration = Vector3.zero;
        this.currentVelocity = Vector3.zero;
    }

    protected override void Patrol()
    {
        this.currentAcceleration += new Vector3(Random.Range(-this.maxAcceleration, this.maxAcceleration), 0, 0);
    }

    protected override void Chase()
    {
        this.currentAcceleration = new Vector3(Mathf.Sign(this.player.transform.position.x - this.transform.position.x) * this.maxAcceleration, 0, 0);
    }

    protected override void Attack()
    {
        this.player.TakeDamage(this.attackDamage);
    }

    private Vector3 ClampMaximum(Vector3 vector, float maximum)
    {
        if (vector.magnitude > maximum)
        {
            return vector.normalized * maximum;
        }
        return vector;
    }
}
