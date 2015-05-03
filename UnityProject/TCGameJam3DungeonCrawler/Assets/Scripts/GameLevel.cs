namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Level;
    using Assets.Scripts.Spawning;

    using UnityEngine;

    public class GameLevel : IGameLevel
    {
        private readonly Game game;

        private readonly IList<ILevelSegment> segments;
        
        private ILevelSegment rootSegment;
        private ILevelSegment currentSegment;
        private Vector2? currentPosition;

        private GameObject debugIndicator;

        private GameObject debugActiveSegmentIndicator;

        private float lastExtensionTime = float.MinValue;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public GameLevel(Game game)
        {
            this.game = game;
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
            this.rootSegment = new LevelSegment(LevelTileCache.Instance.PickTileById(0))
                                   {
                                       Position = new Vector2(0, 0)
                                   };
            this.rootSegment.SetCanExtend(LevelSegmentDirection.Right, true);
            this.segments.Add(this.rootSegment);

            this.SetCurrentSegment(this.rootSegment);
            this.LoadSegment(this.rootSegment);

            this.debugIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugIndicator.GetComponent<Renderer>().material.color = Color.green;
            GameObject.Destroy(this.debugIndicator.GetComponent<Collider>());

            this.debugActiveSegmentIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugActiveSegmentIndicator.GetComponent<Renderer>().material.color = Color.blue;
            GameObject.Destroy(this.debugActiveSegmentIndicator.GetComponent<Collider>());
        }

        public void Update()
        {
            if (GameState.Instance.InErrorState)
            {
                return;
            }

            try
            {
                this.DoUpdate();
            }
            catch (Exception e)
            {
                Debug.LogError("Error in GameLevel.update: " + e);
                GameState.Instance.InErrorState = true;
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void DoUpdate()
        {
            if (this.game.player != null)
            {
                this.currentPosition = this.game.player.transform.position;
            }

            if (this.currentPosition == null)
            {
                return;
            }

            this.debugIndicator.transform.position = new Vector3(this.currentPosition.Value.x, this.currentPosition.Value.y, 0);

            this.UpdateCurrentSegment();

            ILevelSegment current = this.GetCurrentSegment();
            if (current == null)
            {
                this.debugActiveSegmentIndicator.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                this.debugActiveSegmentIndicator.GetComponent<Renderer>().enabled = true;
                this.debugActiveSegmentIndicator.transform.position = new Vector3(
                    current.GetAbsoluteBounds().center.x,
                    current.GetAbsoluteBounds().center.y,
                    10.0f);

                this.debugActiveSegmentIndicator.transform.localScale =
                    new Vector3(
                        current.GetAbsoluteBounds().size.x,
                        current.GetAbsoluteBounds().size.y,
                        1.0f);

                // Activate / Deactivate in both ways
                this.ActivateSegment(LevelSegmentDirection.Right, current, Constants.TileActivationRange);
                this.ActivateSegment(LevelSegmentDirection.Left, current, Constants.TileActivationRange);

                // We collapse / expand only every 2 seconds max, theres enough range for this not to matter
                float time = Time.time;
                if (time > this.lastExtensionTime + Constants.TileExtensionDelay)
                {
                    this.CollapseSegment(LevelSegmentDirection.Left, current, Constants.TileCollapseRange);
                    this.CollapseSegment(LevelSegmentDirection.Right, current, Constants.TileCollapseRange);

                    // Extend the active segment both ways
                    this.ExtendSegment(LevelSegmentDirection.Right, current, Constants.TileGenerationRange);
                    this.ExtendSegment(LevelSegmentDirection.Left, current, Constants.TileGenerationRange);

                    this.lastExtensionTime = time;
                }
            }
        }

        private ILevelSegment GetCurrentSegment()
        {
            return this.currentSegment;
        }

        private void ApplySegmentConnectionMap(LevelConnectionMap closestConnection, ILevelSegment segment, ILevelSegment newSegment)
        {
            newSegment.Position = segment.Position + new Vector2(segment.Width, 0);

            if (closestConnection == null)
            {
                Debug.LogWarning(
                    string.Format(
                        "No Connection between room {0} and {1}",
                        segment.Tile.TileData.id,
                        newSegment.Tile.TileData.id));

                newSegment.Position = segment.Position + new Vector2(segment.Width + Constants.InvalidConnectorLevelMargin, 0);
            }
            else
            {
                // Find the current absolute connection point positions
                Vector2? absoluteSource = segment.Tile.GetConnectionPointPosition(segment.GetObject(), closestConnection.Source.Id);
                Vector2? absoluteTarget = segment.Tile.GetConnectionPointPosition(newSegment.GetObject(), closestConnection.Target.Id);

                System.Diagnostics.Trace.Assert(absoluteSource != null);
                System.Diagnostics.Trace.Assert(absoluteTarget != null);

                Vector2 offset = new Vector2();
                if (absoluteSource.Value.x > absoluteTarget.Value.x)
                {
                    offset.x = -(absoluteSource.Value.x - absoluteTarget.Value.x);
                }
                else
                {
                    offset.x = absoluteTarget.Value.x - absoluteSource.Value.x;
                }

                if (absoluteSource.Value.y > absoluteTarget.Value.y)
                {
                    offset.y = -(absoluteSource.Value.y - absoluteTarget.Value.y);
                }
                else
                {
                    offset.y = absoluteTarget.Value.y - absoluteSource.Value.y;
                }

                newSegment.Position = segment.Position + new Vector2(segment.Width, 0) - offset;
            }
        }

        private void ExtendSegment(LevelSegmentDirection direction, ILevelSegment root, int range)
        {
            ILevelSegment center = root;
            for (var i = 0; i < Constants.TileGenerationRange; i++)
            {
                if (center == null)
                {
                    break;
                }

                this.DoExtendSegment(direction, center);
                center = center.GetNeighbor(direction);
            }    
        }

        private void CollapseSegment(LevelSegmentDirection direction, ILevelSegment segment, int minDepth, int currentDepth = 0)
        {
            ILevelSegment neighbor = segment.GetNeighbor(direction);
            if (neighbor != null)
            {
                currentDepth++;
                this.CollapseSegment(direction, neighbor, minDepth, currentDepth);
                currentDepth--;
            }

            if (currentDepth > minDepth)
            {
                this.UnloadSegment(segment);
            }
        }

        private void ActivateSegment(LevelSegmentDirection direction, ILevelSegment segment, int range, int currentDepth = 0)
        {
            ILevelSegment neighbor = segment.GetNeighbor(direction);
            if (neighbor != null)
            {
                currentDepth++;
                this.ActivateSegment(direction, neighbor, range, currentDepth);
                currentDepth--;
            }

            segment.IsActive = currentDepth <= range;
        }

        private void ExtendSegmentLeft(LevelSegmentDirection direction, ILevelSegment segment, ILevelSegment newSegment)
        {
            LevelConnectionMap closestConnection = this.LocateClosestConnection(segment, direction, newSegment, LevelSegmentDirection.Right);
            if (closestConnection == null)
            {
                throw new InvalidOperationException("No Connection found!");
            }

            this.ApplySegmentConnectionMap(closestConnection, segment, newSegment);
            newSegment.SetNeighbor(LevelSegmentDirection.Right, segment);
        }

        private void ExtendSegmentRight(LevelSegmentDirection direction, ILevelSegment segment, ILevelSegment newSegment)
        {
            LevelConnectionMap closestConnection = this.LocateClosestConnection(segment, direction, newSegment, LevelSegmentDirection.Left);
            if (closestConnection == null)
            {
                throw new InvalidOperationException("No Connection found!");
            }

            this.ApplySegmentConnectionMap(closestConnection, segment, newSegment);
            newSegment.SetNeighbor(LevelSegmentDirection.Left, segment);
        }

        private void DoExtendSegment(LevelSegmentDirection direction, ILevelSegment segment)
        {
            System.Diagnostics.Trace.Assert(segment != null);
            ILevelSegment neighbor = segment.GetNeighbor(direction);
            if (neighbor != null)
            {
                // Reactivate the neighbor instead of making a new one
                this.LoadSegment(neighbor);
                return;
            }

            if (segment.GetCanExtend(direction))
            {
                var pickCriteria = new LevelTilePickCriteria
                                       {
                                           NextTo = segment.Tile,
                                           IsStart = segment == this.rootSegment
                                       };

                ILevelTile tile = LevelTileCache.Instance.PickTile(pickCriteria);
                var newSegment = new LevelSegment(tile);
                if (tile.TileData.canMirror && UnityEngine.Random.Range(0, 2) == 1)
                {
                    newSegment.IsMirrored = true;
                }

                // Important to show the segment before we do positioning!
                this.LoadSegment(newSegment);

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
                    float distance = sourceConnection.Position.y - targetConnection.Position.y;
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

        private void SetCurrentSegment(ILevelSegment segment)
        {
            if (this.currentSegment == segment)
            {
                if (this.currentSegment != null)
                {
                    System.Diagnostics.Trace.Assert(this.currentPosition != null);
                    this.currentSegment.UpdateEvents(this.currentPosition.Value);
                }

                return;
            }

            // Deactivate the old segment first to get the events updated properly
            if (this.currentSegment != null)
            {
                this.currentSegment.IsCurrent = false;
            }

            this.currentSegment = segment;

            if (this.currentSegment != null)
            {
                this.currentSegment.IsCurrent = true;
            }
        }
        
        private void UpdateCurrentSegment()
        {
            System.Diagnostics.Trace.Assert(this.currentPosition != null);

            foreach (ILevelSegment segment in this.segments)
            {
                if (segment.Contains(this.currentPosition.Value))
                {
                    this.SetCurrentSegment(segment);
                    return;
                }
            }

            this.SetCurrentSegment(null);

            // If we have no active segment we "fell" out of it, so search in all loaded ones for a match
            /*ILevelSegment current = this.GetCurrentSegment();
            if (current == null)
            {
                foreach (ILevelSegment segment in this.segments)
                {
                    if (segment.Contains(this.currentPosition.Value))
                    {
                        this.SetCurrentSegment(segment);
                        return;
                    }
                }

                return;
            }

            // We still are in an active segment so we might just be transitioning
            if (current.Contains(this.currentPosition.Value))
            {
                return;
            }

            foreach (LevelSegmentDirection direction in Enum.GetValues(typeof(LevelSegmentDirection)))
            {
                ILevelSegment segment = current.GetNeighbor(direction);
                if (segment != null && segment.Contains(this.currentPosition.Value))
                {
                    this.SetCurrentSegment(segment);
                    return;
                }
            }

            this.SetCurrentSegment(null);*/
        }

        private void LoadSegment(ILevelSegment segment)
        {
            if (segment.IsLoaded)
            {
                return;
            }

            segment.IsLoaded = true;

            IList<Spawner> spawners = segment.GetSpawners();
            foreach (Spawner spawner in spawners)
            {
                this.game.RegisterSpawner(segment, spawner);
            }
        }

        private void UnloadSegment(ILevelSegment segment)
        {
            if (!segment.IsLoaded)
            {
                return;
            }

            IList<Spawner> spawners = segment.GetSpawners();
            foreach (Spawner spawner in spawners)
            {
                this.game.UnregisterSpawner(segment, spawner);
            }

            segment.IsLoaded = false;
        }

        public Spawner LocateSpawner(string id)
        {
            foreach (ILevelSegment segment in this.segments)
            {
                Spawner spawner = segment.GetSpawner(id);
                if (spawner != null)
                {
                    return spawner;
                }
            }

            return null;
        }
    }
}