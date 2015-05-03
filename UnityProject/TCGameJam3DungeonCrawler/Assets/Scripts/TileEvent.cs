namespace Assets.Scripts
{
    using UnityEngine;

    using Assets.Scripts.Contracts;

    public abstract class TileEvent : MonoBehaviour
    {
        public virtual void OnLoad(ILevelSegment segmentData)
        {
            this.Segment = segmentData;
        }

        public virtual void OnUnload() {}

        public virtual void OnEnter() {}

        public virtual void OnExit() {}

        public virtual void OnActivate() {}

        public virtual void OnDeactivate() {}

        public virtual void OnMoveInside(Vector2 position) { }

        protected ILevelSegment Segment { get; private set; }
    }
}
