namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface ILevelSegment
    {
        bool CanGoLeft { get; set; }
        bool CanGoRight { get; set; }
        bool CanGoUp { get; set; }
        bool CanGoDown { get; set; }

        Vector3 Position { get; }

        Bounds Bounds { get; }

        ILevelSegment Left { get; set; }
        ILevelSegment Right { get; set; }
        ILevelSegment Up { get; set; }
        ILevelSegment Down { get; set; }

        void Show();
        void Hide();
    }
}
