using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Extensions
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxCollider2DScaler : MonoBehaviour
    {
        [SerializeField] private PrimitiveBoundsHandle.Axes _axes = PrimitiveBoundsHandle.Axes.All;
        
        private BoxCollider2D _collider;

        private int _screenWidth;
        private int _screenHeight;

        private RectTransform _rectTransform;

        private BoxCollider2D Collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider2D>();
                return _collider;
            }
        }
        
        private RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private void UpdateSize()
        {
            var size = RectTransform.rect.size;

            var colliderSize = Collider.size;

            size.x = _axes is PrimitiveBoundsHandle.Axes.X or PrimitiveBoundsHandle.Axes.All ? size.x : colliderSize.x;
            size.y = _axes is PrimitiveBoundsHandle.Axes.Y or PrimitiveBoundsHandle.Axes.All ? size.y : colliderSize.y;

            Collider.size = size;
        }

        private void Update()
        {
            UpdateSize();
        }
    }
}