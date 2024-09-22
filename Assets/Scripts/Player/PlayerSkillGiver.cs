using DG.Tweening;
using MiniJam167.Projectile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniJam167.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerSkillGiver : ProjectileController
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [Space]
        [SerializeField] private PlayerSkillMemo _skill;
        [Space]
        [SerializeField] private float _lifeTime = 30;
        [SerializeField] private float _moveSpeed;
        
        [Header("Scale")]
        [SerializeField] private Vector2 _rotationSpeedRange;
        [SerializeField] private Vector2 _visualRotationSpeedRange;
        
        [Header("Scale")]
        [SerializeField] private float _scaleDuration = 1f;

        private float _rotationSpeed;
        private float _visualRotationSpeed;
        private float _currentRotation;
        private float _currentVisualRotation;
        private Tween _lifeTween;

        private void OnDestroy()
        {
            _skill?.Unsubscribe(Vector2.zero, Quaternion.identity);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerBody body))
                return;
            
            Transform bodyTransform = body.transform;
            _skill.Subscribe(bodyTransform.position, bodyTransform.rotation);
        }  

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out PlayerBody body))
                return;
            Transform bodyTransform = body.transform;
            _skill.Unsubscribe(bodyTransform.position, bodyTransform.rotation);
        }

        private void Update()
        {
            _currentVisualRotation = (_currentVisualRotation + _visualRotationSpeed * Time.deltaTime) % 360;
            transform.rotation = Quaternion.Euler(0, 0, _currentVisualRotation);
            
            _currentRotation = (_currentRotation + _rotationSpeed * Time.deltaTime) % 360;
            _rigidbody.velocity = Quaternion.Euler(0, 0, _currentRotation) * Vector3.one * _moveSpeed;
        }

        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            _currentVisualRotation = _currentRotation = rotation.eulerAngles.z;
            _rotationSpeed = Random.Range(_rotationSpeedRange.x, _rotationSpeedRange.y);
            _visualRotationSpeed = Random.Range(_visualRotationSpeedRange.x, _visualRotationSpeedRange.y);
            _lifeTween = DOVirtual.DelayedCall(_lifeTime, ReleaseAnim).Play();
            Vector3 scale = GetLossyScale();
            transform.localScale = Vector3.zero;
            transform.DOScale(scale, _scaleDuration).SetEase(Ease.OutBounce).Play();
        }

        private void ReleaseAnim()
        {
            transform.DOScale(0, _scaleDuration)
                .OnComplete(Release)
                .Play();
        }

        protected override void OnRelease()
        {
            _lifeTween?.Kill();
        }
    }
}
