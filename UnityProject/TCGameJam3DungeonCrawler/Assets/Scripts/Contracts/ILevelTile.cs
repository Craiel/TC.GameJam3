﻿namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface ILevelTile
    {
        int Id { get; }
        int Biome { get; }

        bool CanTileWithItself { get; }

        float Rarity { get; }
        float Width { get; }
        float Height { get; }

        Vector3? Rotation { get; set; }
        float RotationAngle { get; set; }

        Bounds Bounds { get; }

        GameObject GetInstance();
    }
}
