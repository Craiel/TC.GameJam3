namespace Assets.Scripts
{
    using Assets.Scripts.Contracts;

    public class LevelConnectionMap
    {
        public ILevelTileConnection Source { get; set; }
        public ILevelTileConnection Target { get; set; }

        public float Distance { get; set; }
    }
}
