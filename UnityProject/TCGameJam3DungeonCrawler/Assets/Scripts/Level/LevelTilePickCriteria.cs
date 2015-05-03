namespace Assets.Scripts.Level
{
    using Assets.Scripts.Contracts;

    public class LevelTilePickCriteria
    {
        public bool IsStart { get; set; }
        public ILevelTile NextTo { get; set; }
    }
}
