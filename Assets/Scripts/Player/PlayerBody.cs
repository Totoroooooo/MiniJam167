using DG.Tweening;
using MiniJam167.HitSystem;
using System;
using System.Collections.Generic;
using MiniJam167.Utility;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniJam167.Player
{
    public class PlayerBody : MonoBehaviour, IHittable
	{
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [Header("Skills")]
        [SerializeField] private List<PlayerSkillMemo> _playerSkill;
        
        [Header("Movement")]
		[SerializeField] private float _moveSpeed = 5;
		[SerializeField][Range(0,1)] private float _rotationSpeed = 0.02f;
		[SerializeField][Range(0,1)] private float _magnitudeThreshold = 0.2f;
        
        [Header("Health")]
		[SerializeField] private int _maxHealth = 100;
        [SerializeField] private Gradient _playerColor;
        [SerializeField] private ColorMemo _hitColor;
        [SerializeField] private float _hitDuration = 1f;
        
        [Header("Movement")]
        [SerializeField] private Transform _bottomLeftCorner;
        [SerializeField] private Transform _topRightCorner;

		public float Shield => 0;
		public float DamageMultiplier => 1;

        private float _currentHealth;
        private Vector2 directionRaw;
        private bool init;

        public event Action Died;

        private void Start()
        {
        }

        public void Init()
        {
            init = true;
            PlayerInput.PlayerMoved += OnPlayerMoved;
            foreach (var skill in _playerSkill)
                skill?.Subscribe(transform.position, transform.rotation);
        }

        private void OnDestroy()
        {
            PlayerInput.PlayerMoved -= OnPlayerMoved;
            foreach (var skill in _playerSkill)
                skill?.Unsubscribe(transform.position, transform.rotation);
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void FixedUpdate()
        {
            if (!init) return;
            var direction = directionRaw.normalized;
            if (directionRaw.magnitude < _magnitudeThreshold)
            {
                _rigidBody.velocity = Vector3.zero;
                return;
            }
            transform.up = Vector3.Lerp(transform.up, direction, _rotationSpeed);
            _rigidBody.velocity = transform.up * _moveSpeed;

            var xValidPosition = Mathf.Clamp(transform.position.x, _bottomLeftCorner.position.x, _topRightCorner.position.x);
            var yValidPosition = Mathf.Clamp(transform.position.y, _bottomLeftCorner.position.y, _topRightCorner.position.y);

            transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
        }

        private void OnPlayerMoved(Vector2 position)
        {
            directionRaw = position;
        }

        private void Die()
        {
            gameObject.SetActive(false);
            Died?.Invoke();
            PlayerInput.PlayerMoved -= OnPlayerMoved;
            foreach (var skill in _playerSkill)
                skill?.Unsubscribe(transform.position, transform.rotation);
        }
        
        public void OnHit(IHitter hitter)
		{
            float damage = this.GetHitDamage(hitter);
            _currentHealth -= damage;
            _spriteRenderer.color = _hitColor.Value;
            _spriteRenderer.DOColor(_playerColor.Evaluate(_currentHealth / _maxHealth), _hitDuration);
            if (_currentHealth <= 0)
                Die();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.TryGetComponent(out IHitter hitter))
                return;
            this.OnHit(hitter);
        }

    }
}
