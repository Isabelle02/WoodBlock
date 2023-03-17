using UnityEngine;

namespace Extensions
{
    public static class CameraExtensions
    {
        public static Bounds Bounds(this Camera camera)
        {
            var camHeight = camera.orthographicSize;
            var camWidth = camHeight * camera.aspect;
            var camPosition = camera.transform.position;
            return new Bounds(camPosition, new Vector3(camWidth, camHeight));
        }
        
        public static T GetObject<T>()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity,  LayerMask.GetMask("UI"));
            if (hit.collider != null)
                if (hit.collider.gameObject.TryGetComponent(out T o))
                    return o;

            return default;
        }
    }
}