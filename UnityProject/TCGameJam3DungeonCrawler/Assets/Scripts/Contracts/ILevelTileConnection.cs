namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface ILevelTileConnection
    {
        string Id { get; set; }

        LevelSegmentDirection Direction { get; set; }

        Vector2 Position { get; set; }
    }
}
