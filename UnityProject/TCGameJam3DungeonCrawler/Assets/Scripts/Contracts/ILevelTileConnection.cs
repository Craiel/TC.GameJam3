namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface ILevelTileConnection
    {
        ConnectionPoint ConnectionData { get; }

        LevelSegmentDirection Direction { get; set; }

        Vector2 Position { get; set; }
    }
}
