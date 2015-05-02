namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class Level : MonoBehaviour, ILevel
    {
        private const float GenerationRange = 4;
        private const int MaxInstances = 100;

        private readonly IList<ILevelSegment> segments;

        private ILevelSegment rootSegment;
        private ILevelSegment activeSegment;
        private Vector3? currentPosition;
        private Bounds generatedBounds = new Bounds();

        private GameObject debugIndicator;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Level()
        {
            this.segments = new List<ILevelSegment>();
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void Start()
        {
            var testTile = new LevelTile(Resources.Load("Prefabs/Room_1"));
            LevelTileCache.Instance.Register(testTile);

            // Initialize the root and make it the active segment for now
            this.rootSegment = new LevelSegment(testTile) { CanGoDown = false, CanGoLeft = false, CanGoUp = false };
            this.activeSegment = this.rootSegment;
            this.activeSegment.Show();

            this.debugIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Update()
        {
            if (Camera.current != null)
            {
                this.currentPosition = Camera.current.transform.position;
            }

            if (this.currentPosition == null)
            {
                return;
            }

            this.debugIndicator.transform.position = new Vector3(this.currentPosition.Value.x, this.currentPosition.Value.y, 0);

            this.UpdateActiveSegment();

            // Check if we need to generate tiles within range
            float generationDistance = this.currentPosition.Value.x + GenerationRange;
            /*while (this.generatedBounds.max.x < generationDistance)
            {
                if (this.segments.Count > MaxInstances)
                {
                    break;
                }

                // Keep piling up the room
                ILevelTile tile = LevelTileCache.Instance.PickTile();
                var instance = tile.GetInstance();
                instance.transform.position += new Vector3(this.generatedBounds.max.x, 0, 0);
                Vector3 tileMaxBounds = new Vector3(
                    instance.transform.position.x + tile.Width,
                    instance.transform.position.y + tile.Height);
                this.generatedBounds.Encapsulate(tileMaxBounds);
                this.segments.Add(instance);
            }*/
        }

        private void UpdateActiveSegment()
        {
            System.Diagnostics.Trace.Assert(this.currentPosition != null);

            if (this.activeSegment.Bounds.Contains(this.currentPosition.Value))
            {
                return;
            }

            if (this.activeSegment.Left != null && this.activeSegment.Left.Bounds.Contains(this.currentPosition.Value))
            {
                this.activeSegment = this.activeSegment.Left;
                return;
            }

            if (this.activeSegment.Right != null && this.activeSegment.Right.Bounds.Contains(this.currentPosition.Value))
            {
                this.activeSegment = this.activeSegment.Right;
            }

            if (this.activeSegment.Up != null && this.activeSegment.Up.Bounds.Contains(this.currentPosition.Value))
            {
                this.activeSegment = this.activeSegment.Up;
            }

            if (this.activeSegment.Down != null && this.activeSegment.Down.Bounds.Contains(this.currentPosition.Value))
            {
                this.activeSegment = this.activeSegment.Down;
            }

            // Todo: Snap back into the active segment for now
            Camera.current.transform.position = this.activeSegment.Position;
            this.currentPosition = this.activeSegment.Position;
        }
    }
}