namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;
    using UnityEngine;

    public class TileEventDebug : TileEvent
    {
        public override void OnLoad(ILevelSegment segment)
        {
            base.OnLoad(segment);

            Debug.Log("TileEventLOAD: " + this.name + " in " + this.SegmentId);
        }

        public override void OnUnload()
        {
            Debug.Log("TileEventUNLOAD: " + this.name + " in " + this.SegmentId);
        }

        public override void OnEnter()
        {
            Debug.Log("TileEventENTER: " + this.name + " in " + this.SegmentId);
        }

        public override void OnExit()
        {
            Debug.Log("TileEventEXIT: " + this.name + " in " + this.SegmentId);
        }

        public override void OnActivate()
        {
            Debug.Log("TileEventACT: " + this.name + " in " + this.SegmentId);
        }

        public override void OnDeactivate()
        {
            Debug.Log("TileEventDEACT: " + this.name + " in " + this.SegmentId);
        }
    }
}
