using DG.Tweening;
using MiniJam167.HitSystem;
using System;
using System.Collections.Generic;
using MiniJam167.Utility;
using Unity.Burst.CompilerServices;
using UnityEngine;

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

        private Vector2 _playerMovementInput;
        private float _currentHealth;

        private void Start()
        {
            PlayerInput.PlayerMoved += OnPlayerMoved;
            foreach (var skill in _playerSkill)
                skill.Subscribe(transform.position, transform.rotation);
        }

        private void Update()
        {
            var xValidPosition = Mathf.Clamp(transform.position.x, _bottomLeftCorner.position.x, _topRightCorner.position.x);
            var yValidPosition = Mathf.Clamp(transform.position.y, _bottomLeftCorner.position.y, _topRightCorner.position.y);

            transform.position = new Vector3(xValidPosition, yValidPosition, 0f);
        }

        private void OnDestroy()
        {
            PlayerInput.PlayerMoved -= OnPlayerMoved;
            foreach (var skill in _playerSkill)
                skill.Unsubscribe(transform.position, transform.rotation);
        }

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void OnPlayerMoved(Vector2 position)
        {
            _playerMovementInput = position;
        }

        private void FixedUpdate()
		{
            _rigidBody.velocity = _playerMovementInput * _moveSpeed;
        }

        private void Die()
        {
            gameObject.SetActive(false);
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
