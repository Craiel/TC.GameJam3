namespace Assets.Scripts
{
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        public int id;

        public int biome;

        public int spawnInstanceLimitActive;

        public int spawnEnemyLimitTotal;

        public float rarity;

        public bool canTileWithItself;
    }
}