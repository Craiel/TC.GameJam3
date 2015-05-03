namespace Assets.Scripts
{
    using UnityEngine;

    public class LevelBoundary : MonoBehaviour
    {
        void OnTriggerEnter(Collider col)
        {
            var a = col.gameObject.GetComponent<Actor>();
            if(a)
                col.gameObject.GetComponent<Actor>().Die();
            else
            {
                //doing nothign but i really want to kill everything here
                //Destroy(col.gameObject);
            }
                
        }
    }
}
