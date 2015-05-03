namespace Assets.Scripts
{
    using UnityEngine;

    public abstract class TileEvent : MonoBehaviour
    {
        public virtual void OnLoad(long segmentId)
        {
            this.SegmentId = segmentId;
        }

        public virtual void OnUnload() {}

        public virtual void OnEnter() {}

        public virtual void OnExit() {}

        public virtual void OnActivate() {}

        public virtual void OnDeactivate() {}

        protected long SegmentId { get; private set; }
    }
}
