using UnityEngine;

namespace MiniJam167.HitSystem
{
    public class HitCollision : MonoBehaviour, IHittable
    {
        public float Shield => 0;
        public float DamageMultiplier => 1;
        
        private IHittable _hittable;

        public void SetHittableParent(IHittable hittable)
        {
            _hittable = hittable;
        }
        
        public void OnHit(IHitter hitter)
        {
            _hittable.OnHit(hitter);
        }
    }
}