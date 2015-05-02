namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class LevelTile : ILevelTile
    {
        private readonly GameObject prefab;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public LevelTile(GameObject prefab)
        {
            System.Diagnostics.Trace.Assert(prefab != null);

            this.prefab = prefab;

            this.UpdateProperties();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int Id { get; private set; }
        public int Biome { get; private set; }

        public bool CanTileWithItself { get; private set; }
        public float Rarity { get; private set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public Vector2? Rotation { get; set; }
        public float RotationAngle { get; set; }
        
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
                Bounds bounds = this.GetMaxBounds(tempInstance);
                this.Width = bounds.size.x;
                this.Height = bounds.size.y;
            }
            finally 
            {
                Object.Destroy(tempInstance);
            }
        }

        private Bounds GetMaxBounds(GameObject source)
        {
            var origin = new Vector3(source.transform.position.x, source.transform.position.y, 0);
            var bounds = new Bounds(origin, Vector3.zero);
            foreach (Renderer r in source.GetComponentsInChildren<Renderer>())
            {
                var mutedBounds = new Bounds(
                    new Vector3(r.bounds.center.x, r.bounds.center.y, 0),
                    new Vector3(r.bounds.size.x, r.bounds.size.y));
                bounds.Encapsulate(mutedBounds);
            }

            return bounds;
        }
    }
}
