namespace Assets.Scripts
{
    using UnityEngine;

    public static class Utils
    {
        public static Bounds GetMaxBounds(GameObject source)
        {
            var origin = new Vector3(source.transform.position.x, source.transform.position.y, 0);
            var mutedBounds = new Bounds(origin, Vector3.zero);
            foreach (Renderer r in source.GetComponentsInChildren<Renderer>())
            {
                var mutedChildBounds = new Bounds(
                    new Vector3(r.bounds.center.x, r.bounds.center.y, 0),
                    new Vector3(r.bounds.size.x, r.bounds.size.y));
                mutedBounds.Encapsulate(mutedChildBounds);
            }

            return mutedBounds;
        }
    }
}
