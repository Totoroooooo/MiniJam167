using UnityEngine;

namespace MiniJam167.Utility
{
    public abstract class Radio<T> : ScriptableObject
    {
        public T Value { get; set; }
    }
}