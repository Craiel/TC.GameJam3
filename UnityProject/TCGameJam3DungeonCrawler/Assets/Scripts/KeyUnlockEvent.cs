using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class KeyUnlockEvent : TileEvent
{
    private long id;

    public override void OnLoad(long segmentId)
    {
        this.id = segmentId;
    }

    void OnTriggerEnter(Collider col)
    {
        var a = col.gameObject.GetComponent<Player>();
        if (a)
        {
            Destroy(this);
        }
        else
        {
            //doing nothign but i really want to kill everything here
            //Destroy(col.gameObject);
        }

    }

    public override void OnUnload()
    {
        Debug.Log("TileEventUNLOAD: " + this.name + " in " + this.id);
    }

    public override void OnEnter()
    {
        Debug.Log("TileEventENTER: " + this.name + " in " + this.id);
        GetComponentInChildren<Animator>().SetTrigger("enterTile");
    }

    public override void OnExit()
    {
        Debug.Log("TileEventEXIT: " + this.name + " in " + this.id);
    }

    public override void OnActivate()
    {
        Debug.Log("TileEventDEACT: " + this.name + " in " + this.id);
    }

    public override void OnDeactivate()
    {
        Debug.Log("TileEventDEACT: " + this.name + " in " + this.id);
    }
}
