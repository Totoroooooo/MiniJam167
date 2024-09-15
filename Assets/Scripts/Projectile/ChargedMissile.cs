using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class ChargedMissile : ProjectileController, IHitter
    {
        [SerializeField] private Collider _collider;
        [Space]
        [SerializeField] private float _damage;
        [SerializeField] private float _startUp = 3f;
        [SerializeField] private float _active = .5f;
        [SerializeField] private float _recovery = .5f;
        
        public float Damage => _damage;
        public float Lethality => 0;
        
        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            
        }
    }
}