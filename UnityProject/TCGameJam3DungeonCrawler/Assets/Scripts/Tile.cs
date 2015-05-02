namespace Assets.Scripts
{
    using UnityEngine;

    public class Tile : MonoBehaviour
    {
        public int Id { get; set; }

        public int Biome { get; set; }

        public float Rarity { get; set; }

        public bool CanTileWithItself { get; set; }
    }
}