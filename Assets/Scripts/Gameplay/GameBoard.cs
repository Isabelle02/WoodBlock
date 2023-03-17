using TransformExtensions;
using UnityEngine;

namespace Gameplay
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        
        private void Awake()
        {
            DragDropMechanics.Dropped += OnObjectDropped;
        }

        private void OnObjectDropped(Collider2D obj, Vector3 defaultPos)
        {
            if (!obj.bounds.IsInBounds(_rectTransform.Bounds()))
                obj.transform.position = defaultPos;
            else
                obj.enabled = false;
        }
    }
}