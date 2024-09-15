using System;
using System.Collections.Generic;
using DG.Tweening;
using MiniJam167.HitSystem;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Player
{
    public class PlayerBody : MonoBehaviour, IHittable
    {
        [SerializeField] private TransformRadio _transformRadio;
        [SerializeField] private PlayerRadio _playerRadio;
        
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _healParticles;
        
        [Header("Skills")]
        [SerializeField] private List<PlayerSkillMemo> _playerSkill;
        
        [Header("Movement")]
		[SerializeField] private float _moveSpeed = 5;
		[SerializeField] [Range(0,1)] private float _rotationSpeed = 0.02f;
        [SerializeField] private float _magnitudeThreshold = 2;
        [SerializeField] private float _magnitudeLimit = 0.1f;
        
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
        private bool _init;

        public event Action Died;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _transformRadio.Value = transform;
            _playerRadio.Value = this;
        }

        private void OnDestroy()
        {
            _transformRadio.Value = null;
            _playerRadio.Value = null;
            PlayerInput.PlayerMoved -= OnPlayerMoved;
            foreach (PlayerSkillMemo skill in _playerSkill)
                skill?.Unsubscribe(transform.position, transform.rotation);
        }

        private void FixedUpdate()
        {
            var xValidPosition = Mathf.Clamp(transform.position.x, _bottomLeftCorner.position.x, _topRightCorner.position.x);
            var yValidPosition = Mathf.Clamp(transform.position.y, _bottomLeftCorner.position.y, _topRightCorner.position.y);

            transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
        }

        public void Init()
        {
            _init = true;
            PlayerInput.PlayerMoved += OnPlayerMoved;
            foreach (PlayerSkillMemo skill in _playerSkill)
                skill?.Subscribe(transform.position, transform.rotation);
        }

        private void OnPlayerMoved(Vector2 directionRaw)
        {
            Vector2 direction = directionRaw.normalized;
            float magnitude = directionRaw.magnitude;
            if (magnitude < _magnitudeLimit || !_init)
            {
                _rigidBody.velocity = Vector3.zero;
                return;
            }

            float moveSpeed = magnitude < _magnitudeThreshold
                ? _moveSpeed * (magnitude - _magnitudeLimit) / (_magnitudeThreshold - _magnitudeLimit)
                : _moveSpeed;
            transform.up = Vector3.Lerp(transform.up, direction, _rotationSpeed);
            _rigidBody.velocity = transform.up * moveSpeed;
        }

        private void Die()
        {
            gameObject.SetActive(false);
            _init = false;
            PlayerInput.PlayerMoved -= OnPlayerMoved;
            foreach (PlayerSkillMemo skill in _playerSkill)
                skill?.Unsubscribe(transform.position, transform.rotation);
            Died?.Invoke();
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

        public void Heal(float value)
        {
            _currentHealth = Mathf.Min(_currentHealth + value, _maxHealth);
            _spriteRenderer.DOColor(_playerColor.Evaluate(_currentHealth / _maxHealth), _hitDuration);
            _healParticles?.Play();
        }
    }
}
