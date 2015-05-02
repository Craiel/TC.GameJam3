namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class LevelBuilder : MonoBehaviour, ILevelBuilder
    {
        private const float GenerationRange = 90.0f;
        private const int MaxInstances = 100;

        private readonly IList<GameObject> activeTiles;

        private Vector3? currentPosition;
        private Bounds generatedBounds = new Bounds();

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelBuilder()
        {
            this.activeTiles = new List<GameObject>();
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void Start()
        {
            var testTile = new LevelTile(Resources.Load("Prefabs/Room_1"));
            LevelTileCache.Instance.Register(testTile);

            //Resources.LoadAll("Prefabs");
            //Debug.Log("Starting LevelBuilder");
            //float root = Camera.current.transform.position.x;
            //this.testRoom = Resources.Load("Prefabs/Room_1");
            
           /*GameObject instance = Instantiate(this.testRoom) as GameObject;
            var bounds = this.GetMaxBounds(instance);
            Debug.Log(bounds.min);
            Debug.Log(bounds.max);
            instance.transform.Rotate(new Vector3(0, 1, 0), -90);*/
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

            if (this.activeTiles.Count > MaxInstances)
            {
                return;
            }

            // Check if we need to generate tiles within range
            float generationDistance = this.currentPosition.Value.x + GenerationRange;
            while (this.generatedBounds.max.x < generationDistance)
            {
                if (this.activeTiles.Count > MaxInstances)
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
                this.activeTiles.Add(instance);
            }
        }
    }
}