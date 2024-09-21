using System;
using UnityEngine;

namespace MiniJam167.HitSystem
{
    public interface ITargetable
    {
        public bool Targetable { get; }
        public Vector3 Position { get; }

        public Action<bool> TargetableChanged { get; set; }
    }
}