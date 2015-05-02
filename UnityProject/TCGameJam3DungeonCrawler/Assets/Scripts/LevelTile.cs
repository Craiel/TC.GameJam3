namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class LevelTile : ILevelTile
    {
        private readonly GameObject prefab;

        private readonly List<ILevelTileConnection> connections;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public LevelTile(GameObject prefab)
        {
            System.Diagnostics.Trace.Assert(prefab != null);

            this.prefab = prefab;

            this.connections = new List<ILevelTileConnection>();

            this.UpdateProperties();

            System.Diagnostics.Trace.Assert(this.TileData != null, "Tile was not defined after property update!");
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public Tile TileData { get; private set; }
        
        public float Width { get; private set; }
        public float Height { get; private set; }

        public Vector2? Rotation { get; set; }
        public float RotationAngle { get; set; }

        public Bounds Bounds { get; private set; }
        
        public override bool Equals(object obj)
        {
            var typed = obj as LevelTile;
            if (typed == null)
            {
                return false;
            }

            return typed.TileData.id == this.TileData.id;
        }

        public override int GetHashCode()
        {
            return this.TileData.id;
        }

        public ReadOnlyCollection<ILevelTileConnection> Connections
        {
            get
            {
                return this.connections.AsReadOnly();
            }
        }

        public GameObject GetInstance()
        {
            var instance = Object.Instantiate(this.prefab);
            System.Diagnostics.Trace.Assert(instance != null);

            if (this.Rotation != null)
            {
                instance.transform.Rotate(this.Rotation.Value, this.RotationAngle);
            }

            return instance;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateProperties()
        {
            // Create a temporary instance to update the properties
            //  Maybe we can get around this? The prefab has no bounds since it's not visible
            var tempInstance = this.GetInstance();
            try
            {
                // Update the rest of the shebang
                this.Bounds = this.GetMaxBounds(tempInstance);
                this.Width = this.Bounds.size.x;
                this.Height = this.Bounds.size.y;

                this.TileData = this.prefab.GetComponent<Tile>();

                ConnectionPoint[] connectionPoints = tempInstance.GetComponentsInChildren<ConnectionPoint>();
                this.ProcessConnectionPoints(connectionPoints);
            }
            finally 
            {
                Object.Destroy(tempInstance);
            }
        }

        private void ProcessConnectionPoints(ConnectionPoint[] points)
        {
            if (points == null || points.Length <= 0)
            {
                return;
            }

            foreach (ConnectionPoint point in points)
            {
                var connection = new LevelTileConnection(point)
                                     {
                                         Position =
                                             new Vector2(
                                             point.transform.position.x,
                                             point.transform.position.y)
                                     };

                if (point.IsVertical)
                {
                    connection.Direction = connection.Position.y > this.Bounds.center.y
                                               ? LevelSegmentDirection.Up
                                               : LevelSegmentDirection.Down;
                }
                else
                {
                    connection.Direction = connection.Position.x > this.Bounds.center.x
                                               ? LevelSegmentDirection.Right
                                               : LevelSegmentDirection.Left;
                }
                
                this.connections.Add(connection);
            }
        }

        private Bounds GetMaxBounds(GameObject source)
        {
            var origin = new Vector3(source.transform.position.x, source.transform.position.y, 0);
            var mutedBounds = new Bounds(origin, Vector3.zero);
            foreach (Renderer r in source.GetComponentsInChildren<Renderer>())
            {
                var mutedChildBounds = new Bounds(
                    new Vector3(r.bounds.center.x, r.bounds.center.y, 0),
                    new Vector3(r.bounds.size.x, r.bounds.size.y));
                mutedBounds.Encapsulate(mutedChildBounds);
            }

            return mutedBounds;
        }
    }
}
