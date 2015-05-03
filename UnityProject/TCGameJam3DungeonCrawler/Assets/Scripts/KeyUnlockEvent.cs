using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class KeyUnlockEvent : TileEvent
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("in trigger collsion");
        var a = col.gameObject.GetComponent<Player>();
        if (a)
        {
            Debug.Log("in IF");
            GameState.Instance.SetState(KeyEvent.KeyString + this.SegmentId);
            EventAggregate.Instance.Notify(KeyEvent.KeyString + this.SegmentId, this);
            Destroy(this.gameObject);
        }
        else
        {
            //doing nothign but i really want to kill everything here
            //Destroy(col.gameObject);
        }

    }
}
