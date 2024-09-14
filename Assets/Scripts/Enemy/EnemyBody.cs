using System;
using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyBody : MonoBehaviour, IHittable
	{
		[Header("References")]
		[SerializeField] private Collider _collider;
		[SerializeField] private EnemyPart[] _enemyParts;
		
		[Header("Health")]
		[SerializeField] private float _maxHealth = 100;
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		
		private float _currentHealth;
		
		public delegate void HealthEvent(float currentHealth, float maxHealth, float damage);
		public event HealthEvent HealthChanged;
		public event Action Died;

		private void Awake()
		{
			foreach (EnemyPart part in _enemyParts)
			{
				
			}
			_currentHealth = _maxHealth;
		}

		private void Die()
		{
			_collider.enabled = false;
			Died?.Invoke();
		}
		
		public void OnHit(IHitter hitter)
		{
			float damage = this.GetHitDamage(hitter);
			_currentHealth -= damage;
			HealthChanged?.Invoke(_currentHealth, _maxHealth, damage);
			
			if (_currentHealth <= 0)
				Die();
		}
	}
}