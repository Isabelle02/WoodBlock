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
    }
}