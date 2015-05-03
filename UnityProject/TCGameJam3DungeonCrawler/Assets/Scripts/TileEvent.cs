namespace Assets.Scripts
{
    using UnityEngine;

    using Assets.Scripts.Contracts;

    public abstract class TileEvent : MonoBehaviour
    {
        public virtual void OnLoad(ILevelSegment segment)
        {
            this.SegmentId = segment.InternalId;
            this.SegmentBounds = segment.Tile.Bounds;
            this.LocalBounds = new Bounds(segment.Position, segment.Tile.Bounds.size);
        }

        public virtual void OnUnload() {}

        public virtual void OnEnter() {}

        public virtual void OnExit() {}

        public virtual void OnActivate() {}

        public virtual void OnDeactivate() {}

        public virtual void OnMoveInside(Vector2 position) { }
        
        protected long SegmentId { get; private set; }
        protected Bounds SegmentBounds { get; private set; }
        protected Bounds LocalBounds { get; private set; }
    }
}
