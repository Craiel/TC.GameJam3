namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public class LevelTileCache
    {
        private static LevelTileCache instance;

        private readonly IList<ILevelTile> tiles;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelTileCache()
        {
            this.tiles = new List<ILevelTile>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static LevelTileCache Instance
        {
            get
            {
                return instance ?? (instance = new LevelTileCache());
            }
        }

        public bool PrefabsScanned { get; private set; }

        public void Register(ILevelTile tile)
        {
            System.Diagnostics.Trace.Assert(!this.tiles.Contains(tile));

            Debug.Log("Registering Tile " + tile.TileData.id);
            this.tiles.Add(tile);
        }

        public void Unregister(ILevelTile tile)
        {
            Debug.Log("Unregistering Tile " + tile.TileData.id);
            this.tiles.Remove(tile);
        }

        public ILevelTile PickTile(ILevelTile nextTo)
        {
            System.Diagnostics.Trace.Assert(this.tiles.Count > 0, "No tiles registered yet");

            ILevelTile pick = null;
            while (pick == null)
            {
                int pickIndex = Random.Range(0, this.tiles.Count);
                pick = this.tiles[pickIndex];
                if (nextTo != null && nextTo.TileData.id == pick.TileData.id && !pick.TileData.canTileWithItself)
                {
                    pick = null;
                }
            }
            
            return pick;
        }

        public void RescanPrefabs()
        {
            Object[] resources = Resources.LoadAll("Prefabs");
            foreach (Object resource in resources)
            {
                var typed = resource as GameObject;
                if (typed == null)
                {
                    continue;
                }

                var tileComponent = typed.GetComponent<Tile>();
                if (tileComponent == null)
                {
                    continue;
                }

                var tile = new LevelTile(typed);
                this.Register(tile);
            }

            this.PrefabsScanned = true;
        }
    }
}
