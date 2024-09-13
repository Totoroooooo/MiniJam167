using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniJam67.HitSystem
{
    public interface IHittable
    {
        public void OnHit(IHitter hitter);
    }
}
