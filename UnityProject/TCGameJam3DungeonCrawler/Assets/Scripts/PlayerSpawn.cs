using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public static bool spawned;

    // Use this for initialization
    void Start()
    {
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        if (!spawned)
        {
            //GameObject t = (GameObject) Instantiate(player, this.transform.position, this.transform.rotation);
            GameObject go = Instantiate(player, this.transform.position, this.transform.rotation) as GameObject;
            go.transform.parent = this.transform;
            //Transform t = ((GameObject)Instantiate(somePrefab, transform.position, transform.rotation)).transform;
            //t.parent = transform;
            spawned = true;
        }
            
    }

    private void Update()
    {
        if (!spawned)
        {
            Instantiate(player, this.transform.position, this.transform.rotation);
            spawned = true;
        }
    }

    void OnDestory()
    {
        Debug.Log("player distoryed");
        spawned = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(1, 2, 1));
    }

}
