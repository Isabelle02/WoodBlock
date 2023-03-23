using System.Collections.Generic;
using System.Linq;
using Extensions;
using TransformExtensions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gameplay
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
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

                var breaking = GetToBreak();
                foreach (var breakingPos in _map)
                {
                    if (breaking.ContainsKey(breakingPos.x) || breaking.ContainsKey(breakingPos.y))
                    {
                        var breakingBlock = breakingPos.GetObject<BlockObject>();
                        if (breakingBlock)
                            Pool.Release(breakingBlock);
                    }
                }
            }
        }

        private Dictionary<float, int> GetToBreak()
        {
            var breaking = new Dictionary<float, int>();
            var breakingX = _map.GroupBy(item => item.x).Where(group => group.Count() == _size.y)
                .ToDictionary(g => g.Key, x => x.Count());
            var breakingY = _map.GroupBy(item => item.y).Where(group => group.Count() == _size.x)
                .ToDictionary(g => g.Key, x => x.Count());

            breaking.AddRange(breakingX);
            breaking.AddRange(breakingY);
            
            return breaking;
        }
    }
}