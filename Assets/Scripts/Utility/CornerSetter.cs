using System;
using UnityEngine;

namespace MiniJam167.Utility
{
    public class CornerSetter : MonoBehaviour
    {
        [Serializable] public struct Corner
        {
            public VectorRadio Radio;
            public Transform Transform;
        }
        
        [SerializeField] private Corner[] _corners;
        
        private void Awake()
        {
            foreach (Corner corner in _corners)
                corner.Radio.Value = corner.Transform.position;
        }
    }
}