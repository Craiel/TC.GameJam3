namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using System.Linq;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class LevelSegment : ILevelSegment
    {
        private static long nextSegmentId;

        private readonly IDictionary<LevelSegmentDirection, bool> canExtend;
        private readonly IDictionary<LevelSegmentDirection, ILevelSegment> neighbors;

        private readonly ILevelTile tile;

        private GameObject activeObject;

        private readonly IList<GameObject> connectorDebugObjects;
        private readonly IList<GameObject> backgroundObjects; 

        private GameObject debugActiveSegmentIndicatorBL;
        private GameObject debugActiveSegmentIndicatorTR;

        private GameObject debugRoot;
        private GameObject backgroundRoot;

        private Vector2 position;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelSegment(ILevelTile tile)
        {
            this.InternalId = nextSegmentId++;

            this.tile = tile;

            this.Width = tile.Width;
            this.Height = tile.Height;

            this.canExtend = new Dictionary<LevelSegmentDirection, bool>();
            this.neighbors = new Dictionary<LevelSegmentDirection, ILevelSegment>();

            // Todo: take this from tile connector points
            this.SetCanExtend(LevelSegmentDirection.Right, true);

            this.connectorDebugObjects = new List<GameObject>();
            this.backgroundObjects = new List<GameObject>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long InternalId { get; private set; }

        public bool IsMirrored { get; set; }

        public bool IsActive
        {
            get
            {
                return this.activeObject != null;
            }

            set
            {
                if (value && this.activeObject == null)
                {
                    this.Activate();
                } else if (!value && this.activeObject != null)
                {
                    this.Deactivate();
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
                    this.position = value;
                    if(this.IsActive)
                    {
                        // Re-show to update all the debug markers etc
                        this.Deactivate();
                        this.Activate();
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
            var halfWidth = this.Width / 2;
            //var halfHeight = this.Height / 2;

            return value.x >= this.Position.x - halfWidth
                && value.x <= this.Position.x + halfWidth
                && value.y >= this.Position.y
                && value.y <= this.Position.y + this.Height;
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

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Activate()
        {
            if (this.activeObject != null)
            {
                return;
            }

            // Todo: Have to load the object's state
            this.activeObject = this.tile.GetInstance();
            this.activeObject.transform.position = new Vector3(this.Position.x, this.Position.y, 0);

            this.debugRoot = new GameObject(this.activeObject.name + "_DEBUG");
            this.backgroundRoot = new GameObject(this.activeObject.name + "_BG");

            if (this.IsMirrored)
            {
                var absoluteCenter = new Vector3(this.tile.Bounds.center.x + this.position.x, 0, 0);
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
                this.connectorDebugObjects.Add(debugObject);
            }

            // Background
            this.RebuildBackground();

            this.debugActiveSegmentIndicatorTR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugActiveSegmentIndicatorTR.name = this.activeObject.name + "_TR";
            this.debugActiveSegmentIndicatorTR.GetComponent<Renderer>().material.color = Color.magenta;
            this.debugActiveSegmentIndicatorTR.transform.position = new Vector3(this.tile.Bounds.min.x + this.Position.x, this.tile.Bounds.min.y + this.Position.y);
            this.debugActiveSegmentIndicatorTR.transform.SetParent(this.debugRoot.transform);

            this.debugActiveSegmentIndicatorBL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.debugActiveSegmentIndicatorBL.name = this.activeObject.name + "_BL";
            this.debugActiveSegmentIndicatorBL.GetComponent<Renderer>().material.color = Color.magenta;
            this.debugActiveSegmentIndicatorBL.transform.position = new Vector3(this.tile.Bounds.max.x + this.Position.x, this.tile.Bounds.max.y + this.Position.y);
            this.debugActiveSegmentIndicatorBL.transform.SetParent(this.debugRoot.transform);
        }

        private void RebuildBackground()
        {
            if (this.tile.TileData.background == null)
            {
                return;
            }

            float marginHorizontal = 40.0f;
            float marginVertical = 10.0f;
            GameObject instance = Object.Instantiate(this.tile.TileData.background);
            Bounds backGroundBounds = this.tile.Bounds;
            backGroundBounds.Expand(new Vector3(marginVertical * 2, marginHorizontal * 2));
            //var halfWidth = backGroundBounds.size.x / 2;

            Bounds instanceBounds = Utils.GetMaxBounds(instance);
            int tileX = 1 + (int)Mathf.Ceil(backGroundBounds.size.x / instanceBounds.size.x);
            int tileY = 1 + (int)Mathf.Ceil(backGroundBounds.size.y / instanceBounds.size.y);

            float xPos = this.position.x - marginVertical;
            for (var x = 0; x < tileX; x++)
            {
                float yPos = this.position.y - marginHorizontal;
                for (var y = 0; y < tileY; y++)
                {
                    var backTile = Object.Instantiate(this.tile.TileData.background);
                    backTile.name = string.Format("bg_{0}x{1}", xPos, yPos);
                    backTile.transform.position = new Vector3(xPos, yPos, 3);
                    backTile.transform.SetParent(this.backgroundRoot.transform);
                    this.backgroundObjects.Add(backTile);

                    yPos += instanceBounds.size.y;
                }

                 xPos += instanceBounds.size.x;
            }

            Object.Destroy(instance);
        }

        private void Deactivate()
        {
            if (this.activeObject == null)
            {
                return;
            }

            // Show the connectors for debugging
            foreach (GameObject debugObject in this.connectorDebugObjects)
            {
                Object.Destroy(debugObject);
            }
            this.connectorDebugObjects.Clear();

            foreach (GameObject backgroundObject in this.backgroundObjects)
            {
                Object.Destroy(backgroundObject);
            }
            this.backgroundObjects.Clear();

            // Todo: Have to save the object's state
            Object.Destroy(this.activeObject);
            this.activeObject = null;

            Object.Destroy(this.debugActiveSegmentIndicatorTR);
            Object.Destroy(this.debugActiveSegmentIndicatorBL);

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
                            break;
                        }
                    case LevelSegmentDirection.Right:
                        {
                            return LevelSegmentDirection.Left;
                            break;
                        }
                }
            }

            return desiredDirection;
        }
    }
}
