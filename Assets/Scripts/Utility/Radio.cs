using UnityEngine;

namespace MiniJam167.Utility
{
    public abstract class Radio<T> : ScriptableObject where T : Component
    {
        public T Value { get; set; }
    }
}