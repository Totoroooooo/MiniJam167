using UnityEngine;

namespace MiniJam167.Utility
{
    public class PoolHolder<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private Pool<T> _pool;
        public Pool<T> Pool => _pool;
    }
}