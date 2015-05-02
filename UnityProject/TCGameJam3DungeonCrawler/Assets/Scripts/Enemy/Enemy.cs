using UnityEngine;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(CharacterController))]
public class Enemy : MonoBehaviour
{
    private Transform _transform;

    private Vector3 velocity;

    private EnemySpawn.enemyTypes type { get; set; }
    private EnemySpawn.enemyColors color { get; set; }

    public int health;
    public bool colliding;
    public float speed;
    private Vector3 _pos;
    private int _getMoving;

    public CharacterController enemyController;

    // Update is called once per frame
    void Start()
    {
        _pos = this.transform.position;
        
    }
    void Update()
    {

        colliding = this.enemyController.SimpleMove(new Vector3(speed, 0, 0));
        if (!colliding)
        {
            speed = speed * -1;
        }
        if(this.transform.position.x == _pos.x)
        {
            Debug.Log("Stayed still");
            _getMoving++;
        }
        if(_getMoving >=50)
        {
            speed = speed * -1;
        }
    }

    public void onHit(DamageType dmg, DamageColor col)
    {
        //dmg = base * nvl((% * Max),1) 
    }
}
