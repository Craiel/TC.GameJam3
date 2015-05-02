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

        public void Register(ILevelTile tile)
        {
            System.Diagnostics.Trace.Assert(!this.tiles.Contains(tile));

            Debug.Log("Registering Tile " + tile.Id);
            this.tiles.Add(tile);
        }

        public void Unregister(ILevelTile tile)
        {
            Debug.Log("Unregistering Tile " + tile.Id);
            this.tiles.Remove(tile);
        }

        public ILevelTile PickTile()
        {
            System.Diagnostics.Trace.Assert(this.tiles.Count > 0, "No tiles registered yet");

            var pick = Random.Range(0, this.tiles.Count);
            return this.tiles[pick];
        }
    }
}
