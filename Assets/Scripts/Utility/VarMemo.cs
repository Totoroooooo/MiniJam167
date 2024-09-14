using UnityEngine;

namespace MiniJam167.Utility
{
    public abstract class VarMemo<T> : ScriptableObject
    {
        [SerializeField] private T _value;
        public T Value => _value;
    }
}