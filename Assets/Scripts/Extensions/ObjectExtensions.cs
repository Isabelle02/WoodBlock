using UnityEngine;

namespace TransformExtensions
{
    public static class ObjectExtensions
    {
        public static void SetPositionXY(this Transform transform, float x, float y)
        {
            var position = transform.position;
            position.x = x;
            position.y = y;
            transform.position = position;
        }

        public static bool IsInBounds(this Bounds target, Bounds bounds)
        {
            return target.min.x >= bounds.min.x && target.max.x <= bounds.max.x &&
                   target.min.y >= bounds.min.y && target.max.y <= bounds.max.y;
        }

        public static Bounds Bounds(this RectTransform transform)
        {
            var corners = new Vector3[4];
            transform.GetWorldCorners(corners);
            var size = new Vector2(corners[2].x - corners[0].x, corners[2].y - corners[0].y);
            return new Bounds(transform.position, size);
        }
    }
}