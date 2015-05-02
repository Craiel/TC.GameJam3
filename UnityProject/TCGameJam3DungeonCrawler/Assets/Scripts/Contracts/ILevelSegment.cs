namespace Assets.Scripts.Contracts
{
    using System.Collections.Generic;

    using UnityEngine;

    public interface ILevelSegment
    {
        ILevelTile Tile { get; }

        Vector2 Position { get; set; }

        float Width { get; }
        float Height { get; }
        
        void Show();
        void Hide();

        bool GetCanExtend(LevelSegmentDirection direction);
        void SetCanExtend(LevelSegmentDirection direction, bool value);

        ILevelSegment GetNeighbor(LevelSegmentDirection direction);
        void SetNeighbor(LevelSegmentDirection direction, ILevelSegment segment);

        bool Contains(Vector2 value);

        IList<ILevelTileConnection> GetConnections(LevelSegmentDirection direction);
    }
}
