using System.Collections.Generic;
using TransformExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace Makeup.UI
{
    public class GridLayout : MonoBehaviour
    {
        [SerializeField] private Vector2 _space;
        [SerializeField] private Vector2 _backgroundOffset;
        [SerializeField] private Vector2 _scaledBackgroundOffset;
        [SerializeField] private Image _background;
        [SerializeField] private Transform _container;

        [SerializeField] private float _widthMin;
        [SerializeField] private float _widthMax;
        [SerializeField] private float _columns;
        [SerializeField] private float _height;

        private readonly List<MonoBehaviour> _elements = new();
        private float _maxWidth;

        public Vector2 Size => _background.sprite.bounds.size;

        public void Add(MonoBehaviour element)
        {
            var index = _elements.Count;
            var currentLineIndex = (int) Mathf.Floor(index / _columns);
            var linesCount = currentLineIndex + 1;
            
            _elements.Add(element);

            element.transform.SetParent(_container);
            
            var items = new List<MonoBehaviour>();
            for (var i = 0; i < _elements.Count; i++)
            {
                var lineIndex = (int) Mathf.Floor(i / _columns);
                if (lineIndex == currentLineIndex)
                    items.Add(_elements[i]);
            }
            
            var width = items.ArrangeHorizontalByCenter(_space.x);
            
            var contentWidth = linesCount * _height;
            var remainingItemsWidth = contentWidth + _space.y * (linesCount + 1) / 2f;

            var lastIndex = 0;
            for (var i = 0; i < _elements.Count; i++)
            {
                var item = _elements[i];
                var lineIndex = (int) Mathf.Floor(i / _columns);
                
                if (lineIndex == lastIndex)
                {
                    remainingItemsWidth -= _height + _space.y;
                    lastIndex++;
                }
                
                item.transform.SetLocalPositionY(remainingItemsWidth - (contentWidth - _height) / 2f);
            }

            if (_background == null)
                return;
            
            var scale = 1.0f;
            var backgroundOffset = _backgroundOffset;
            if (width > _widthMax && _widthMax > 0.0f)
            {
                scale = _widthMax / width;
                backgroundOffset = _scaledBackgroundOffset;
            }

            if (width < _widthMin)
                width = _widthMin;

            _container.SetLocalScaleXY(scale);

            if (_maxWidth < width)
            {
                _background.transform.SetLocalScaleX(width * scale + backgroundOffset.x);
                _maxWidth = width;
            }

            _background.transform.SetLocalScaleY(scale * _height * linesCount + backgroundOffset.y);
        }

        public void Clear()
        {
            foreach (var element in _elements)
                Pool.Release(element);
            
            _elements.Clear();
            _maxWidth = 0;
        }

        private void Initialize()
        {
            _elements.Clear();
            
            foreach (Transform child in _container)
            {
                var element = child.GetComponent<MonoBehaviour>();
                if (element == null)
                    continue;
                
                Add(element);
            }
        }
    }
}