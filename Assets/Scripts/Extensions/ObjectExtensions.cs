using System.Collections.Generic;
using System.Linq;
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
        
        public static void SetLocalPositionXY(this Transform transform,float x, float y)
        {
            var position = transform.localPosition;
            position.x = x;
            position.y = y;
            transform.localPosition = position;
        }
        
        public static void SetLocalPositionY(this Transform transform, float y)
        {
            var position = transform.localPosition;
            position.y = y;
            transform.localPosition = position;
        }
        
        public static void SetLocalScaleXY(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.x = value;
            scale.y = value;
            transform.localScale = scale;
        }
        
        public static void SetLocalScaleX(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.x = value;
            transform.localScale = scale;
        }
        
        public static void SetLocalScaleY(this Transform transform, float value)
        {
            var scale = transform.localScale;
            scale.y = value;
            transform.localScale = scale;
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
        
        public static float ArrangeHorizontalByCenter<T>(this List<T> items, float spacing) where T : MonoBehaviour
        {
            var contentWidth = items.Sum(item => item.GetComponent<Collider2D>().bounds.size.x);
            var contentWidthWithSpacing = contentWidth + spacing * (items.Count - 1);
            var remainingItemsWidth = contentWidth + spacing * (items.Count + 1) / 2f;

            foreach (var item in items)
            {
                var sizeX = item.GetComponent<Collider2D>().bounds.size.x;
                remainingItemsWidth -= sizeX + spacing;
                item.transform.SetLocalPositionXY(-remainingItemsWidth + (contentWidth - sizeX) / 2f, 0.0f);
            }

            return contentWidthWithSpacing;
        }
    }
}