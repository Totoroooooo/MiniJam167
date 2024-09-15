using DG.Tweening;
using UnityEngine;

namespace MiniJam167.Projectile
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultMissile : Missile
    {
        [Header("Components")]
        [SerializeField] protected Rigidbody2D _rb;
        
        [Header("Settings")]
        [SerializeField] protected float _speed = 3;
        [SerializeField] private float _life = 3;
        
        private Tween _lifeTween;

        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            _rb.velocity = rotation * Vector2.up * _speed;
            _lifeTween = DOVirtual.DelayedCall(_life, Release).Play();
        }

        protected override void OnRelease()
        {
            _lifeTween?.Kill();
        }
    }
}
