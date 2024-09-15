using DG.Tweening;
using MiniJam167.HitSystem;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    public class HomingMissile : DefaultMissile, IHittable
    {
        [Header("Homing")]
        [Range(0, 1)]
        [SerializeField] private float _rotationSpeed = .1f;
        [SerializeField] private float _precisionDuration = 5f;
        [SerializeField] private float _imprecisionDuration = 3f;
        [SerializeField] private TransformRadio _target;


        public float Shield => 0;
        public float DamageMultiplier => 1;
        
        private float _currentRotationSpeed;
        private Tween _homingDecaySequence;
        
        public void OnHit(IHitter hitter)
        {
            Release();
        }

        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            base.OnSpawn(position, rotation);
            _currentRotationSpeed = _rotationSpeed;
            _homingDecaySequence = DOTween.Sequence()
                .AppendInterval(_precisionDuration)
                .Append(DOVirtual.Float(_rotationSpeed, 0, _imprecisionDuration, r => _currentRotationSpeed = r))
                .Play();
        }

        private void Update()
        {
            Vector2 direction = _target.Value.transform.position - transform.position;
            transform.up = Vector3.Lerp(transform.up, direction.normalized, _currentRotationSpeed);
            _rb.velocity = transform.up * _speed;
        }
        
        protected override void OnRelease()
        {
            base.OnRelease();
            _homingDecaySequence?.Kill();
        }
    }
}