namespace Assets.Scripts
{
    using UnityEngine;

    public abstract class TileEvent : MonoBehaviour
    {
        public abstract void OnLoad(long segmentId);

        public abstract void OnUnload();

        public abstract void OnEnter();

        public abstract void OnExit();

        public abstract void OnActivate();

        public abstract void OnDeactivate();
    }
}
