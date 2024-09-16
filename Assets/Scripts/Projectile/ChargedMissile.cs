using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MiniJam167.HitSystem;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Projectile
{
    [RequireComponent(typeof(Animator))]
    public class ChargedMissile : ProjectileController, IHitter
    {
        private static readonly string ANIMATION_PARAMETER = "State";
        
        [SerializeField] private Animator _animator;
        [SerializeField] private TransformRadio _target;
        [Space]
        [SerializeField] private float _damage = 50f;
        [SerializeField] private float _introDuration = 3f;
        [SerializeField] private float _chargeDuration = 2f;
        [SerializeField] private float _startUp = .5f;
        [SerializeField] private float _laserDuration = 1f;
        [SerializeField] private float _recovery = .05f;
        
        public float Damage => _damage;
        public float Lethality => 0;
        
        private List<IHittable> _hittables = new ();
        private bool _isFollowing;

        private void Reset()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_isFollowing)
                return;
            
            Vector2 direction = _target.Value.transform.position - transform.position;
            transform.right = -direction.normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHittable hittable))
                return;
            
            _hittables.Add(hittable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHittable hittable))
                return;
            
            _hittables.Remove(hittable);
        }
        
        protected override void OnSpawn(Vector2 position, Quaternion rotation)
        {
            ChargeMissile().Forget();
            return;
            
            async UniTaskVoid ChargeMissile()
            {
                _isFollowing = true;
                
                _animator.SetInteger(ANIMATION_PARAMETER, 0);
                await UniTask.WaitForSeconds(_introDuration);
                _animator.SetInteger(ANIMATION_PARAMETER, 1);
                _isFollowing = false;
                
                await UniTask.WaitForSeconds(_chargeDuration);
                _animator.SetInteger(ANIMATION_PARAMETER, 2);
                
                await UniTask.WaitForSeconds(_startUp);
                foreach (IHittable hittable in _hittables)
                    hittable.OnHit(this);
                
                await UniTask.WaitForSeconds(_laserDuration);
                _animator.SetInteger(ANIMATION_PARAMETER, 3);
                
                await UniTask.WaitForSeconds(_recovery);
                Release();
            }
        }
    }
}