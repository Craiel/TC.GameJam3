namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class LevelSegment : ILevelSegment
    {
        private readonly IDictionary<LevelSegmentDirection, bool> canExtend;
        private readonly IDictionary<LevelSegmentDirection, ILevelSegment> neighbors;

        private readonly ILevelTile tile;

        private GameObject activeObject;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelSegment(ILevelTile tile)
        {
            this.tile = tile;

            this.Width = tile.Width;
            this.Height = tile.Height;

            this.canExtend = new Dictionary<LevelSegmentDirection, bool>();
            this.neighbors = new Dictionary<LevelSegmentDirection, ILevelSegment>();

            // Todo: take this from tile connector points
            this.SetCanExtend(LevelSegmentDirection.Right, true);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public Vector2 Position { get; set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

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

        public bool GetCanExtend(LevelSegmentDirection direction)
        {
            if (this.canExtend.ContainsKey(direction))
            {
                return this.canExtend[direction];
            }

            return false;
        }

        public void SetCanExtend(LevelSegmentDirection direction, bool value)
        {
            if (this.canExtend.ContainsKey(direction))
            {
                this.canExtend[direction] = value;
            }
            else
            {
                this.canExtend.Add(direction, value);
            }
        }

        public ILevelSegment GetNeighbor(LevelSegmentDirection direction)
        {
            if (this.neighbors.ContainsKey(direction))
            {
                return this.neighbors[direction];
            }

            return null;
        }

        public void SetNeighbor(LevelSegmentDirection direction, ILevelSegment segment)
        {
            if (this.neighbors.ContainsKey(direction))
            {
                this.neighbors[direction] = segment;
            }
            else
            {
                this.neighbors.Add(direction, segment);
            }
        }

        public bool Contains(Vector2 value)
        {
            return value.x >= this.Position.x
                && value.x <= this.Position.x + this.Width
                && value.y >= this.Position.y
                && value.y <= this.Position.y + this.Height;
        }
    }
}
