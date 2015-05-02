namespace Assets.Scripts
{
    using System;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class LevelTile : ILevelTile
    {
        private readonly Object resource;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public LevelTile(Object resource)
        {
            System.Diagnostics.Trace.Assert(resource != null);

            this.resource = resource;

            this.UpdateProperties();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int Id { get; private set; }
        public int Biome { get; private set; }

        public bool CanTileWithItself { get; private set; }
        public float Rarity { get; private set; }

        public Vector3? Rotation { get; set; }
        public float RotationAngle { get; set; }

        public Bounds Bounds { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public override bool Equals(object obj)
        {
            var typed = obj as LevelTile;
            if (typed == null)
            {
                return false;
            }

            return typed.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public GameObject GetInstance()
        {
            var instance = Object.Instantiate(this.resource) as GameObject;
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
            //  Maybe we can get around this?
            var tempInstance = this.GetInstance();
            try
            {
                // Update the rest of the shebang
                this.Bounds = this.GetMaxBounds(tempInstance);

                // Only care about the x axis for now but we save both
                this.Width = this.Bounds.max.x - this.Bounds.min.x;
                this.Height = this.Bounds.max.y - this.Bounds.min.y;
            }
            finally 
            {
                Object.Destroy(tempInstance);
            }
        }

        private Bounds GetMaxBounds(GameObject source)
        {
            var bounds = new Bounds(source.transform.position, Vector3.zero);
            foreach (Renderer r in source.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(r.bounds);
            }

            return bounds;
        }
    }
}
