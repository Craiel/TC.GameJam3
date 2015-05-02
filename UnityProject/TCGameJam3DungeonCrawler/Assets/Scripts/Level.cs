namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class Level : MonoBehaviour, ILevel
    {
        private const float GenerationRange = 4;

        private const float LevelMargin = 1.0f;

        private readonly IList<ILevelSegment> segments;

        private ILevelSegment rootSegment;
        private ILevelSegment activeSegment;
        private Vector2? currentPosition;

        private GameObject debugIndicator;

        private GameObject debugActiveSegmentIndicator;

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
            this.segments.Add(this.rootSegment);

            this.activeSegment = this.rootSegment;
            this.activeSegment.Show();

            this.debugIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugIndicator.GetComponent<Renderer>().material.color = Color.green;

            this.debugActiveSegmentIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugActiveSegmentIndicator.GetComponent<Renderer>().material.color = Color.blue;
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

            if (this.activeSegment == null)
            {
                this.debugActiveSegmentIndicator.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                this.debugActiveSegmentIndicator.GetComponent<Renderer>().enabled = true;
                this.debugActiveSegmentIndicator.transform.position = new Vector3(
                    this.activeSegment.Position.x,
                    this.activeSegment.Position.y,
                    10.0f);
                this.debugActiveSegmentIndicator.transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
                
                // For testing we only extend right for now...
                ILevelSegment center = this.activeSegment;
                for (var i = 0; i < GenerationRange; i++)
                {
                    this.ExtendSegment(LevelSegmentDirection.Right, center);
                    center = center.GetNeighbor(LevelSegmentDirection.Right);
                }
            }
        }

        /*private void CollapseSegmentLeft(ILevelSegment segment)
        {
            if(segment.Left)
        }*/

        private void ApplySegmentConnectionMap(LevelConnectionMap closestConnection, ILevelSegment segment, ILevelSegment newSegment)
        {
            if (closestConnection == null)
            {
                Debug.LogWarning(
                    string.Format(
                        "No Connection between room {0} and {1}",
                        segment.Tile.TileData.Id,
                        newSegment.Tile.TileData.Id));

                newSegment.Position = segment.Position + new Vector2(segment.Width + LevelMargin, 0);
            }
            else
            {
                var yOffset = closestConnection.Source.Position.y - closestConnection.Target.Position.y;
                newSegment.Position = segment.Position + new Vector2(segment.Width, yOffset);
            }
        }
        private void ExtendSegmentLeft(LevelSegmentDirection direction, ILevelSegment segment, ILevelSegment newSegment)
        {
            LevelConnectionMap closestConnection = this.LocateClosestConnection(segment, direction, newSegment, LevelSegmentDirection.Right);
            this.ApplySegmentConnectionMap(closestConnection, segment, newSegment);
            newSegment.SetNeighbor(LevelSegmentDirection.Right, segment);
        }

        private void ExtendSegmentRight(LevelSegmentDirection direction, ILevelSegment segment, ILevelSegment newSegment)
        {
            LevelConnectionMap closestConnection = this.LocateClosestConnection(segment, direction, newSegment, LevelSegmentDirection.Left);
            this.ApplySegmentConnectionMap(closestConnection, segment, newSegment);
            newSegment.SetNeighbor(LevelSegmentDirection.Left, segment);
        }

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
                            this.ExtendSegmentLeft(direction, segment, newSegment);
                            break;
                        }

                        case LevelSegmentDirection.Right:
                        {
                            this.ExtendSegmentRight(direction, segment, newSegment);
                            break;
                        }

                        case LevelSegmentDirection.Up:
                        {
                            throw new NotImplementedException();
                        }

                        case LevelSegmentDirection.Down:
                        {
                            throw new NotImplementedException();
                        }
                }
                newSegment.Show();
                segment.SetNeighbor(direction, newSegment);
                this.segments.Add(newSegment);
            }
        }

        private LevelConnectionMap LocateClosestConnection(
            ILevelSegment source,
            LevelSegmentDirection sourceDirection,
            ILevelSegment target,
            LevelSegmentDirection targetDirection)
        {
            LevelConnectionMap currentMap = null;
            float? currentDistance = null;
            
            IList<ILevelTileConnection> sourceConnections = source.GetConnections(sourceDirection);
            IList<ILevelTileConnection> targetConnections = target.GetConnections(targetDirection);

            foreach (ILevelTileConnection sourceConnection in sourceConnections)
            {
                foreach (ILevelTileConnection targetConnection in targetConnections)
                {
                    float distance = Vector2.Distance(sourceConnection.Position, targetConnection.Position);
                    if (currentDistance == null || currentDistance > distance)
                    {
                        currentDistance = distance;
                        currentMap = new LevelConnectionMap
                                  {
                                      Source = sourceConnection,
                                      Target = targetConnection,
                                      Distance = distance
                                  };
                    }
                }
            }

            return currentMap;
        }
        
        private void UpdateActiveSegment()
        {
            System.Diagnostics.Trace.Assert(this.currentPosition != null);

            // If we have no active segment we "fell" out of it, so search in all loaded ones for a match
            if (this.activeSegment == null)
            {
                foreach (ILevelSegment segment in this.segments)
                {
                    if (segment.Contains(this.currentPosition.Value))
                    {
                        this.activeSegment = segment;
                        return;
                    }
                }

                return;
            }

            // We still are in an active segment so we might just be transitioning
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

            this.activeSegment = null;
        }
    }
}