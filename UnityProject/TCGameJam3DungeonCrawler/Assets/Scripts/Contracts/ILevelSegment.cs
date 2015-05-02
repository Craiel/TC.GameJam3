namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface ILevelSegment
    {
        Vector2 Position { get; }

        float Width { get; }
        float Height { get; }
        
        void Show();
        void Hide();

        bool GetCanExtend(LevelSegmentDirection direction);
        void SetCanExtend(LevelSegmentDirection direction, bool value);

        ILevelSegment GetNeighbor(LevelSegmentDirection direction);
        void SetNeighbor(LevelSegmentDirection direction, ILevelSegment segment);

        bool Contains(Vector2 value);
    }
}
