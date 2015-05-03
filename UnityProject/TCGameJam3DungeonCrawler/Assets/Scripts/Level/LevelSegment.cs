namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Spawning;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class LevelSegment : ILevelSegment
    {
        private static long nextSegmentId;

        private readonly IDictionary<LevelSegmentDirection, bool> canExtend;
        private readonly IDictionary<LevelSegmentDirection, ILevelSegment> neighbors;

        private readonly ILevelTile tile;

        private readonly GameObject boundaryObject;

        private readonly IList<GameObject> visualChildren; 

        private readonly IList<TileEvent> eventObjects;

        private GameObject activeObject;
        
        private GameObject debugRoot;
        private GameObject backgroundRoot;

        private Vector2 position;

        private bool isCurrent;

        private bool isActive;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelSegment(ILevelTile tile)
        {
            this.InternalId = nextSegmentId++;

            this.tile = tile;

            this.Width = tile.Width;
            this.Height = tile.Height;

            this.boundaryObject = Resources.Load("Prefabs/LevelBoundaryBlock") as GameObject;
            
            this.canExtend = new Dictionary<LevelSegmentDirection, bool>();
            this.neighbors = new Dictionary<LevelSegmentDirection, ILevelSegment>();

            this.eventObjects = new List<TileEvent>();

            // Todo: take this from tile connector points
            this.SetCanExtend(LevelSegmentDirection.Right, true);

            this.visualChildren = new Collection<GameObject>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long InternalId { get; private set; }

        public bool IsMirrored { get; set; }

        public bool IsCurrent
        {
            get
            {
                return this.isCurrent;
            }

            set
            {
                if (this.isCurrent != value)
                {
                    if (value)
                    {
                        foreach (TileEvent eventObject in this.eventObjects)
                        {
                            eventObject.OnEnter();
                        }
                    }
                    else
                    {
                        foreach (TileEvent eventObject in this.eventObjects)
                        {
                            eventObject.OnExit();
                        }
                    }

                    this.isCurrent = value;
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (this.isActive != value)
                {
                    if (value)
                    {
                        foreach (TileEvent eventObject in this.eventObjects)
                        {
                            eventObject.OnActivate();
                        }
                    }
                    else
                    {
                        foreach (TileEvent eventObject in this.eventObjects)
                        {
                            eventObject.OnDeactivate();
                        }
                    }

                    this.isActive = value;
                }
            }
        }

        public bool IsLoaded
        {
            get
            {
                return this.activeObject != null;
            }

            set
            {
                if (value && this.activeObject == null)
                {
                    this.Load();
                } else if (!value && this.activeObject != null)
                {
                    this.Unload();
                }
            }
        }

        public ILevelTile Tile
        {
            get
            {
                return this.tile;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (this.position != value)
                {
                    Vector2 oldPosition = this.position;
                    this.position = value;
                    if(this.IsLoaded)
                    {
                        // Re-show to update all the debug markers etc
                        this.Move(this.position - oldPosition);
                    }
                }
            }
        }

        public float Width { get; private set; }
        public float Height { get; private set; }

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
            var actualDirection = this.GetActualDirection(direction);
            if (this.neighbors.ContainsKey(actualDirection))
            {
                return this.neighbors[actualDirection];
            }

            return null;
        }

        public void SetNeighbor(LevelSegmentDirection direction, ILevelSegment segment)
        {
            var actualDirection = this.GetActualDirection(direction);
            if (this.neighbors.ContainsKey(actualDirection))
            {
                this.neighbors[actualDirection] = segment;
            }
            else
            {
                this.neighbors.Add(actualDirection, segment);
            }
        }

        public bool Contains(Vector2 value)
        {
            return this.GetAbsoluteBounds().Contains(value);
        }

        public IList<ILevelTileConnection> GetConnections(LevelSegmentDirection direction)
        {
            var actualDirection = this.GetActualDirection(direction);
            return this.tile.Connections.Where(x => x.Direction == actualDirection).ToList();
        }

        public GameObject GetObject()
        {
            System.Diagnostics.Trace.Assert(this.activeObject != null);

            return this.activeObject;
        }

        public IList<Spawner> GetSpawners()
        {
            System.Diagnostics.Trace.Assert(this.activeObject != null);

            IList<Spawner> spawners = this.activeObject.GetComponentsInChildren<Spawner>();
            foreach (Spawner spawner in spawners)
            {
                // Force the spawner group to this tile
                spawner.AbsoluteId = this.InternalId + "_" + spawner.id;
                spawner.Group = this.InternalId;
            }

            return spawners;
        }

        public Spawner GetSpawner(string id)
        {
            if (this.activeObject == null)
            {
                return null;
            }

            IList<Spawner> spawners = this.activeObject.GetComponentsInChildren<Spawner>();
            foreach (Spawner spawner in spawners)
            {
                spawner.AbsoluteId = this.InternalId + "_" + spawner.id;
                spawner.Group = this.InternalId;

                if (spawner.AbsoluteId == id)
                {
                    // Force the spawner group to this tile
                    return spawner;
                }
            }

            return null;
        }

        public void UpdateEvents(Vector2 currentPosition)
        {
            foreach(var eventObject in this.eventObjects) 
            {
                eventObject.OnMoveInside(currentPosition);
            }
        }

        public Bounds GetAbsoluteBounds()
        {
            Vector3 center = new Vector2(
                this.tile.Bounds.center.x + this.Position.x,
                this.tile.Bounds.center.y + this.Position.y);
            return new Bounds(center, this.tile.Bounds.size);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Load()
        {
            if (this.activeObject != null)
            {
                return;
            }

            var absoluteBounds = this.GetAbsoluteBounds();

            // Todo: Have to load the object's state
            this.activeObject = this.tile.GetInstance();
            this.activeObject.transform.position = new Vector3(this.Position.x, this.Position.y, 0);

            this.debugRoot = new GameObject(this.activeObject.name + "_DEBUG");
            this.backgroundRoot = new GameObject(this.activeObject.name + "_BG");

            if (this.IsMirrored)
            {
                var absoluteCenter = new Vector3(absoluteBounds.center.x, 0, 0);
                this.activeObject.transform.localScale = new Vector3(1, 1, -1);
                this.activeObject.transform.RotateAround(absoluteCenter, new Vector3(0, 1, 0), 180);
                //this.activeObject.transform.Rotate(new Vector3(0, 1, 0), 180);
            }

            foreach (ILevelTileConnection connection in this.tile.Connections)
            {
                var debugObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                debugObject.name = this.activeObject.name + "_CON_" + connection.Id;
                debugObject.transform.position = this.Position + connection.Position;
                debugObject.GetComponent<Renderer>().material.color = Color.red;
                debugObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                debugObject.transform.SetParent(this.debugRoot.transform);
                this.visualChildren.Add(debugObject);
            }

            if (this.boundaryObject != null)
            {
                GameObject boundary = Object.Instantiate(this.boundaryObject);
                boundary.transform.position = new Vector2(absoluteBounds.min.x, absoluteBounds.min.y - Constants.LevelBoundaryDistance);
                boundary.transform.localScale = new Vector3(this.tile.Bounds.size.x + 10, 1, 10);
                boundary.name = this.activeObject.name + "_BOUNDS";
                this.visualChildren.Add(boundary);
                //this.boundary.transform.SetParent(this.backgroundRoot.transform);
            }

            // Background
            this.RebuildBackground();

            GameObject debugActiveSegmentIndicatorTR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugActiveSegmentIndicatorTR.name = this.activeObject.name + "_TR";
            debugActiveSegmentIndicatorTR.GetComponent<Renderer>().material.color = Color.magenta;
            debugActiveSegmentIndicatorTR.transform.position = absoluteBounds.max;
            debugActiveSegmentIndicatorTR.transform.SetParent(this.debugRoot.transform);
            this.visualChildren.Add(debugActiveSegmentIndicatorTR);

            GameObject debugActiveSegmentIndicatorBL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugActiveSegmentIndicatorBL.name = this.activeObject.name + "_BL";
            debugActiveSegmentIndicatorBL.GetComponent<Renderer>().material.color = Color.magenta;
            debugActiveSegmentIndicatorBL.transform.position = absoluteBounds.min;
            debugActiveSegmentIndicatorBL.transform.SetParent(this.debugRoot.transform);
            this.visualChildren.Add(debugActiveSegmentIndicatorBL);

            this.eventObjects.Clear();

            IList<TileEvent> events = this.activeObject.GetComponentsInChildren<TileEvent>();
            foreach (TileEvent eventObject in events)
            {
                this.eventObjects.Add(eventObject);
                eventObject.OnLoad(this);
            }
        }

        private void Move(Vector3 translation)
        {
            this.activeObject.transform.position += translation;

            foreach (GameObject child in this.visualChildren)
            {
                child.transform.position += translation;
            }
        }

        private void RebuildBackground()
        {
            if (this.tile.TileData.background == null)
            {
                return;
            }

            GameObject instance = Object.Instantiate(this.tile.TileData.background);
            Bounds backGroundBounds = this.tile.Bounds;
            backGroundBounds.Expand(new Vector3(Constants.LevelBackgroundMarginVertical * 2, Constants.LevelBackgroundMarginHorizontal * 2));
            //var halfWidth = backGroundBounds.size.x / 2;

            Bounds instanceBounds = Utils.GetMaxBounds(instance);
            int tileX = 1 + (int)Mathf.Ceil(backGroundBounds.size.x / instanceBounds.size.x);
            int tileY = 1 + (int)Mathf.Ceil(backGroundBounds.size.y / instanceBounds.size.y);

            float xPos = this.position.x - Constants.LevelBackgroundMarginHorizontal;
            for (var x = 0; x < tileX; x++)
            {
                float yPos = this.position.y - Constants.LevelBackgroundMarginHorizontal;
                for (var y = 0; y < tileY; y++)
                {
                    var backTile = Object.Instantiate(this.tile.TileData.background);
                    backTile.name = string.Format("bg_{0}x{1}", xPos, yPos);
                    backTile.transform.position = new Vector3(xPos, yPos, 3);
                    backTile.transform.SetParent(this.backgroundRoot.transform);
                    this.visualChildren.Add(backTile);

                    yPos += instanceBounds.size.y;
                }

                 xPos += instanceBounds.size.x;
            }

            Object.Destroy(instance);
        }

        private void Unload()
        {
            if (this.activeObject == null)
            {
                return;
            }

            // Notify that we are unloading this segment
            foreach (TileEvent eventObject in this.eventObjects)
            {
                eventObject.OnUnload();
            }

            this.eventObjects.Clear();

            // Show the connectors for debugging
            foreach (GameObject child in this.visualChildren)
            {
                Object.Destroy(child);
            }

            this.visualChildren.Clear();
            
            // Todo: Have to save the object's state
            Object.Destroy(this.activeObject);
            this.activeObject = null;
            
            Object.Destroy(this.debugRoot);
            Object.Destroy(this.backgroundRoot);
        }

        private LevelSegmentDirection GetActualDirection(LevelSegmentDirection desiredDirection)
        {
            var actualDirection = desiredDirection;
            if (this.IsMirrored)
            {
                switch (actualDirection)
                {
                    case LevelSegmentDirection.Left:
                        {
                            return LevelSegmentDirection.Right;
                        }

                    case LevelSegmentDirection.Right:
                        {
                            return LevelSegmentDirection.Left;
                        }
                }
            }

            return desiredDirection;
        }
    }
}
