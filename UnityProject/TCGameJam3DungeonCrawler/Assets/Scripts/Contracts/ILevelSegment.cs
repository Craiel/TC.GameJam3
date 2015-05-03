namespace Assets.Scripts.Contracts
{
    using System.Collections.Generic;

    using Assets.Scripts.Level;
    using Assets.Scripts.Spawning;

    using UnityEngine;

    public interface ILevelSegment
    {
        long InternalId { get; }

        bool IsMirrored { get; set; }

        // Current = segment the player is in
        // Active = segment that is within activation range
        // Loaded = segment is actually present in the world
        bool IsCurrent { get; set; }
        bool IsActive { get; set; }
        bool IsLoaded { get; set; }

        ILevelTile Tile { get; }

        Vector2 Position { get; set; }

        float Width { get; }
        float Height { get; }

        bool GetCanExtend(LevelSegmentDirection direction);
        void SetCanExtend(LevelSegmentDirection direction, bool value);

        ILevelSegment GetNeighbor(LevelSegmentDirection direction);
        void SetNeighbor(LevelSegmentDirection direction, ILevelSegment segment);

        bool Contains(Vector2 value);

        IList<ILevelTileConnection> GetConnections(LevelSegmentDirection direction);

        GameObject GetObject();

        IList<Spawner> GetSpawners();
        Spawner GetSpawner(string id);
    }
}
