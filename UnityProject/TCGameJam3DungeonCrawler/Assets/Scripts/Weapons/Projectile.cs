using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float lifetime;

    private int baseDamage;
    private int redDamage;
    private int greenDamage;
    private int blueDamage;

    private Vector3 direction;

    private float currentLifetime;

    private bool isInitialized;

    public void Init(int baseDamage, int redDamage, int greenDamage, int blueDamage, Vector3 direction)
    {
        this.baseDamage = baseDamage;
        this.redDamage = redDamage;
        this.greenDamage = greenDamage;
        this.blueDamage = blueDamage;
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
        BaseEnemy enemy = collision.gameObject.GetComponent<BaseEnemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(this.baseDamage, this.redDamage, this.greenDamage, this.blueDamage);
        }
        Destroy(this.gameObject);
    }
}