using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifetime;

    private int damage;
    private Vector3 direction;

    private float currentLifetime;

    private bool isInitialized;

    public void Init(int damage, Vector3 direction)
    {
        this.damage = damage;
        this.direction = direction;

        this.transform.LookAt(this.transform.position + this.direction);

        this.isInitialized = true;
    }

    private void Update()
    {
        if (this.isInitialized)
        {
            this.transform.position += this.direction * this.speed * Time.deltaTime;
            this.currentLifetime += Time.deltaTime;

            if(this.currentLifetime > this.lifetime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}