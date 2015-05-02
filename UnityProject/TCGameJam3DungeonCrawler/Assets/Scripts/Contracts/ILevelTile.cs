namespace Assets.Scripts.Contracts
{
    using System.Collections.ObjectModel;

    using UnityEngine;

    public interface ILevelTile
    {
        Tile TileData { get; }

        Bounds Bounds { get; }
        
        float Width { get; }
        float Height { get; }

        Vector2? Rotation { get; set; }
        float RotationAngle { get; set; }

        ReadOnlyCollection<ILevelTileConnection> Connections { get; }

        GameObject GetInstance();
    }
}
