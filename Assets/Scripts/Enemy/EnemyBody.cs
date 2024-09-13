using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyBody : MonoBehaviour, IHittable
	{
		[SerializeField] private EnemyPart[] _enemyParts;
		
		[SerializeField] private float _maxHealth = 100;
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		
		private float _currentHealth;

		private void Awake()
		{
			foreach (EnemyPart part in _enemyParts)
				part.SetMainBody(this);
			_currentHealth = _maxHealth;
		}

		private void Die()
		{
			gameObject.SetActive(false);
		}
		
		public void OnHit(IHitter hitter)
		{
			float damage = this.GetHitDamage(hitter);
			_currentHealth -= damage;
			//TODO: add fx
			if (_currentHealth <= 0)
				Die();
		}
	}
}