using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class KeyEvent : TileEvent
{

    private long id;

    public override void OnLoad(long segmentId)
    {
        this.id = segmentId;
        Debug.Log("TileEventLOAD: " + this.name + " in " + this.id);
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
