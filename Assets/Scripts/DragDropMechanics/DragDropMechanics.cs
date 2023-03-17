using System;
using Extensions;
using TransformExtensions;
using UnityEngine;

public class DragDropMechanics : MonoBehaviour
{
    private Collider2D _capturedObject;
    private Vector3 _startPosition;

    public static event Action<Collider2D, Vector3> Dropped;
    
    private static Bounds CameraBounds => Camera.main.Bounds();
    private Vector2 CapturedObjectSize => _capturedObject.bounds.size;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                _capturedObject = hit.collider;
                _startPosition = _capturedObject.transform.position;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (_capturedObject)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                var min = new Vector2(-CameraBounds.size.x + CapturedObjectSize.x / 2,
                    -CameraBounds.size.y + CapturedObjectSize.y / 2);
                var max = new Vector2(CameraBounds.size.x - CapturedObjectSize.x / 2,
                    CameraBounds.size.y - CapturedObjectSize.y / 2);
                
                var targetX = Mathf.Clamp(mousePosition.x, min.x, max.x);
                var targetY = Mathf.Clamp(mousePosition.y, min.y, max.y);
                _capturedObject.transform.SetPositionXY(targetX, targetY);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
            CallDropped();
    }

    private void CallDropped()
    {
        if (_capturedObject)
            Dropped?.Invoke(_capturedObject, _startPosition);

        _capturedObject = null;
    }
}
