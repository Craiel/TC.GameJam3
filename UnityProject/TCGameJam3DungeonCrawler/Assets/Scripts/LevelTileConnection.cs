namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class LevelTileConnection : ILevelTileConnection
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public LevelTileConnection(ConnectionPoint data)
        {
            this.ConnectionData = data;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public ConnectionPoint ConnectionData { get; private set; }

        public LevelSegmentDirection Direction { get; set; }

        public Vector2 Position { get; set; }
    }
}
