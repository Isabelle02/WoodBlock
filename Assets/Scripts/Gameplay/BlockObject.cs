using UnityEngine;

namespace Gameplay
{
    public class BlockObject : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;

        public Collider2D Collider => _collider;
    }
}