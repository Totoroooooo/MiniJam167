using UnityEngine;

namespace MiniJam167.HitSystem
{
    public class Hitter : MonoBehaviour, IHitter
    {
        public float Damage => 10;
        public float Lethality => 0;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHittable hittable))
                return;
            
            hittable.OnHit(this);
        }
    }
}