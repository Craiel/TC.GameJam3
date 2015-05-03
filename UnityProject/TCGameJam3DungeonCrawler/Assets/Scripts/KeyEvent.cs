using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Contracts;

public class KeyEvent : TileEvent
{
    public const string KeyString = "key_";
    private bool isLocked;
    private bool isLockRequested;
    private bool wasUnlocked;

    public override void OnLoad(ILevelSegment segment)
    {
        base.OnLoad(segment);

        EventAggregate.Instance.Subscribe(KeyString + this.Segment.InternalId, this.OnUnlock);
        Debug.Log("TileEventLOAD: " + this.name + " in " + this.Segment.InternalId);

        this.wasUnlocked = GameState.Instance.GetState(KeyString + this.Segment.InternalId);
        this.isLockRequested = !this.wasUnlocked;
    }

    private void OnUnlock(object source)
    {
        var typed = source as KeyUnlockEvent;
        if (typed == null)
            return;

        Debug.Log("UnLocking Room " + this.Segment.InternalId);
        GetComponentInChildren<Animator>().SetTrigger("exitTile");
        this.wasUnlocked = true;
        this.isLocked = false;
        this.isLockRequested = false;
    }

    public override void OnUnload()
    {
        EventAggregate.Instance.Unsubscribe(KeyString + this.Segment.InternalId, this.OnUnlock);
        Debug.Log("TileEventUNLOAD: " + this.name + " in " + this.Segment.InternalId);
    }

    public override void OnEnter()
    {
        Debug.Log("TileEventENTER: " + this.name + " in " + this.Segment.InternalId);
        this.isLockRequested = true;
    }

    public override void OnMoveInside(Vector2 position)
    {
        //Debug.Log("Move: " + position + this.isLockRequested + this.isLocked);
        if(this.isLockRequested && !this.wasUnlocked)
        {
            if (this.isLocked)
            {
                return;
            }

            var testBounds = this.Segment.GetAbsoluteBounds();
            testBounds.Expand(-4.0f);
            testBounds = new Bounds((Vector2)testBounds.center, (Vector2)testBounds.size);
            if (testBounds.Contains(position))
            {
                Debug.Log("Locking Room " + this.Segment.InternalId);
                GetComponentInChildren<Animator>().SetTrigger("enterTile");
                this.isLocked = true;
            }            
        }

        base.OnMoveInside(position);
    }
}
