namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class LevelSegment : ILevelSegment
    {
        private readonly ILevelTile tile;

        private GameObject activeObject;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelSegment(ILevelTile tile)
        {
            this.tile = tile;
            this.Bounds = tile.Bounds;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }

        public Vector3 Position { get; set; }

        public Bounds Bounds { get; private set; }

        public ILevelSegment Left { get; set; }
        public ILevelSegment Right { get; set; }
        public ILevelSegment Up { get; set; }
        public ILevelSegment Down { get; set; }

        public void Show()
        {
            System.Diagnostics.Trace.Assert(this.activeObject == null);

            // Todo: Have to load the object's state
            this.activeObject = this.tile.GetInstance();
            this.activeObject.transform.position = this.Position;
        }

        public void Hide()
        {
            System.Diagnostics.Trace.Assert(this.activeObject != null);
            
            // Todo: Have to save the object's state
            Object.Destroy(this.activeObject);
            this.activeObject = null;
        }
    }
}
