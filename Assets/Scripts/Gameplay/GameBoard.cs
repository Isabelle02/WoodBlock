using System.Collections.Generic;
using System.Linq;
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

        private BlockObject _currentBlock;
        private readonly Dictionary<Vector3, BlockObject> _blocks = new();

        private void Awake()
        {
            DragDropMechanics.Dropped += OnObjectDropped;
        }

        private void Start()
        {
            _currentBlock = Pool.Get<BlockObject>(transform);
            _currentBlock.transform.SetPositionXY(0, -25);
        }

        private void OnObjectDropped(Collider2D obj, Vector3 defaultPos)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cell = _tilemap.WorldToCell(pos);
            var newPos = _tilemap.CellToWorld(cell) + _tilemap.cellSize / 2;
            
            if (!obj.bounds.IsInBounds(_rectTransform.Bounds()) || _blocks.Keys.ToList().Exists(key => key == newPos))
                obj.transform.position = defaultPos;
            else
            {
                obj.enabled = false;

                var block = obj.gameObject.GetComponent<BlockObject>();
                block.transform.SetPositionXY(newPos.x, newPos.y);
                _blocks.Add(newPos, block);

                _currentBlock = Pool.Get<BlockObject>(transform);
                _currentBlock.transform.SetPositionXY(0, -25);

                var breaking = GetToBreak();
                var breakingBlocks = new Dictionary<Vector3, BlockObject>(_blocks);
                foreach (var breakingBlock in breakingBlocks)
                {
                    var breakingPos = breakingBlock.Key;
                    if (breaking.ContainsKey(breakingPos.x) || breaking.ContainsKey(breakingPos.y))
                    {
                        breakingBlock.Value.Collider.enabled = true;
                        _blocks.Remove(breakingBlock.Key);
                        Pool.Release(breakingBlock.Value);
                    }
                }
            }
        }

        private Dictionary<float, int> GetToBreak()
        {
            var breaking = new Dictionary<float, int>();

            var positions = _blocks.Keys;
            
            var breakingX = positions.GroupBy(item => item.x).Where(group => group.Count() == _size.y)
                .ToDictionary(g => g.Key, x => x.Count());
            var breakingY = positions.GroupBy(item => item.y).Where(group => group.Count() == _size.x)
                .ToDictionary(g => g.Key, x => x.Count());

            breaking.AddRange(breakingX);
            breaking.AddRange(breakingY);
            
            return breaking;
        }
    }
}