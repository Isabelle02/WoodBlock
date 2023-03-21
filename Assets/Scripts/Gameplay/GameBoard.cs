using System.Collections.Generic;
using Makeup.UI;
using TransformExtensions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Tilemap _tilemap;

        private readonly List<Vector3> _map = new();

        private void Awake()
        {
            DragDropMechanics.Dropped += OnObjectDropped;
        }

        private void OnObjectDropped(Collider2D obj, Vector3 defaultPos)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cell = _tilemap.WorldToCell(pos);
            var newPos = _tilemap.CellToWorld(cell) + _tilemap.cellSize / 2;
            
            if (!obj.bounds.IsInBounds(_rectTransform.Bounds()) || _map.Contains(newPos))
                obj.transform.position = defaultPos;
            else
            {
                obj.enabled = false;

                var block = obj.gameObject.GetComponent<BlockObject>();
                block.transform.SetPositionXY(newPos.x, newPos.y);
                _map.Add(newPos);

                var newBlock = Pool.Get<BlockObject>(transform);
                newBlock.transform.SetPositionXY(0, -25);
            }
        }
    }
}