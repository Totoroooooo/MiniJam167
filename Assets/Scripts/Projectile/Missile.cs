using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public abstract class Missile : ProjectileController, IHitter
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _lethality;
        
        public float Damage => _damage;
        public float Lethality => _lethality;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHittable hittable))
                return;
            
            hittable.OnHit(this);
            _pool.Release(this);
        }
    }
}