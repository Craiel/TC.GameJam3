namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class Level : MonoBehaviour, ILevel
    {
        private const float GenerationRange = 4;

        private readonly IList<ILevelSegment> segments;

        private ILevelSegment rootSegment;
        private ILevelSegment activeSegment;
        private Vector2? currentPosition;

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
            if (!LevelTileCache.Instance.PrefabsScanned)
            {
                LevelTileCache.Instance.RescanPrefabs();
            }

            // Initialize the root and make it the active segment for now
            this.rootSegment = new LevelSegment(LevelTileCache.Instance.PickTile())
                                   {
                                       Position = new Vector2(0, 0)
                                   };
            this.rootSegment.SetCanExtend(LevelSegmentDirection.Right, true);

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

            // For testing we only extend right for now...
            ILevelSegment center = this.activeSegment;
            for (var i = 0; i < GenerationRange; i++)
            {
                this.ExtendSegment(LevelSegmentDirection.Right, center);
                center = center.GetNeighbor(LevelSegmentDirection.Right);
            }
        }

        /*private void CollapseSegmentLeft(ILevelSegment segment)
        {
            if(segment.Left)
        }*/

        private void ExtendSegment(LevelSegmentDirection direction, ILevelSegment segment)
        {
            if (segment.GetNeighbor(direction) == null && segment.GetCanExtend(direction))
            {
                // Todo: do the shift for the connection points
                ILevelTile tile = LevelTileCache.Instance.PickTile();
                var newSegment = new LevelSegment(tile);

                switch (direction)
                {
                        case LevelSegmentDirection.Left:
                        {
                            newSegment.Position = segment.Position - new Vector2(segment.Width, 0);
                            newSegment.SetNeighbor(LevelSegmentDirection.Right, segment);
                            break;
                        }
                        case LevelSegmentDirection.Right:
                        {
                            newSegment.Position = segment.Position + new Vector2(segment.Width, 0);
                            newSegment.SetNeighbor(LevelSegmentDirection.Left, segment);
                            break;
                        }
                        case LevelSegmentDirection.Up:
                        {
                            newSegment.Position = segment.Position + new Vector2(0, segment.Height);
                            newSegment.SetNeighbor(LevelSegmentDirection.Down, segment);
                            break;
                        }
                        case LevelSegmentDirection.Down:
                        {
                            newSegment.Position = segment.Position - new Vector2(0, segment.Height);
                            newSegment.SetNeighbor(LevelSegmentDirection.Up, segment);
                            break;
                        }
                }
                newSegment.Show();
                segment.SetNeighbor(direction, newSegment);
                this.segments.Add(newSegment);
            }
        }
        
        private void UpdateActiveSegment()
        {
            System.Diagnostics.Trace.Assert(this.currentPosition != null);

            if (this.activeSegment.Contains(this.currentPosition.Value))
            {
                return;
            }

            foreach (LevelSegmentDirection direction in Enum.GetValues(typeof(LevelSegmentDirection)))
            {
                ILevelSegment segment = this.activeSegment.GetNeighbor(direction);
                if (segment != null && segment.Contains(this.currentPosition.Value))
                {
                    this.activeSegment = segment;
                    return;
                }
            }
            
            // Todo: Snap back into the active segment for now
            Camera.current.transform.position = new Vector3(
                this.activeSegment.Position.x,
                this.activeSegment.Position.y,
                Camera.current.transform.position.z);

            this.currentPosition = this.activeSegment.Position;
        }
    }
}