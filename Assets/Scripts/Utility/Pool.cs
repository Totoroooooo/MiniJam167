using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MiniJam167.Utility
{
    [Serializable] public class Pool<T> where T : Component
    {
        [SerializeField] private T _original;

        private Queue<T> _pool = new();
        
        public void Populate(Transform parent, int count)
        {
            for (int i = 0; i < count; i++)
                Release(Instantiate(parent, Vector2.zero, Quaternion.identity));
        }

        public T Get(Transform parent, Vector3 position, Quaternion rotation)
        {
            if (_pool.Count <= 0)
                return Instantiate(parent, position, rotation);
            
            T clone = _pool.Dequeue();
            clone.transform.SetParent(parent, false);
            clone.transform.position = position;
            clone.transform.rotation = rotation;
            clone.gameObject.SetActive(true);
            return clone;
        }

        public void Release(T clone)
        {
            clone.gameObject.SetActive(false);
            _pool.Enqueue(clone);
        }

        private T Instantiate(Transform parent, Vector2 position, Quaternion rotation)
        {
            T clone = Object.Instantiate(_original, position, rotation, parent);
            return clone;
        }
    }
}