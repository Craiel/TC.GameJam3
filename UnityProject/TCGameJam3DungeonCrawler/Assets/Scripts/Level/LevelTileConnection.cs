namespace Assets.Scripts.Level
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class LevelTileConnection : ILevelTileConnection
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Id { get; set; }

        public LevelSegmentDirection Direction { get; set; }

        public Vector2 Position { get; set; }
    }
}
