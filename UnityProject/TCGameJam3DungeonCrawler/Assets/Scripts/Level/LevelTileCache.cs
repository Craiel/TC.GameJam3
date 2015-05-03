namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    using Object = UnityEngine.Object;
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

        public ILevelTile PickTileById(int id)
        {
            return this.tiles.FirstOrDefault(x => x.TileData.id == id);
        }

        public ILevelTile PickTile(LevelTilePickCriteria criteria)
        {
            System.Diagnostics.Trace.Assert(this.tiles.Count > 0, "No tiles registered yet");
            System.Diagnostics.Trace.Assert(criteria != null, "Need to specify criteria");

            int pickCounter = 0;
            bool pickCountWarning = false;
            ILevelTile pick;
            while (true)
            {
                pickCounter++;
                if (pickCounter > Constants.TilePickWarnThreshold && !pickCountWarning)
                {
                    Debug.LogWarning("Failed to pick a tile in time!");
                    pickCountWarning = true;
                }

                if (pickCounter > Constants.TilePickErrorThreshold)
                {
                    throw new InvalidOperationException("Tile Pick count exceeded maximum allowed retries!");
                }

                int pickIndex = Random.Range(1, this.tiles.Count);
                ILevelTile potentialPick = this.tiles[pickIndex];
                if (!potentialPick.TileData.isEnabled)
                {
                    continue;
                }

                if (criteria.IsStart && !potentialPick.TileData.canStart)
                {
                    continue;
                }

                if (criteria.NextTo != null)
                {
                    if (criteria.NextTo.TileData.id == potentialPick.TileData.id
                        && !potentialPick.TileData.canTileWithItself)
                    {
                        continue;
                    }
                }

                pick = potentialPick;
                break;
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
