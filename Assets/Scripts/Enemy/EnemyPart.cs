using System;
using Cysharp.Threading.Tasks;
using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyPart : MonoBehaviour, IHittable
	{
		[SerializeField] private Collider _collider;
		[Space]
		[SerializeField] private float _maxHealth = 100f;
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;
		[Space]
		[SerializeField] private float _disableDuration = 5f;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		
		private float _currentHealth;
		private EnemyBody _mainBody;
		
		public delegate void TimedEvent(float duration);
		public event TimedEvent Disabled;

		public event Action Enabled;

		public void SetMainBody(EnemyBody mainBody)
		{
			_mainBody = mainBody;
		}

		public void OnHit(IHitter hitter)
		{
			float damage = this.GetHitDamage(hitter);
			_currentHealth -= damage;
			if (_currentHealth <= 0)
				Disable();
		}

		private void Enable()
		{
			_currentHealth = _maxHealth;
			_collider.enabled = true;
			Enabled?.Invoke();
		}

		private void Disable()
		{
			_collider.enabled = false;
			Disabled?.Invoke(_disableDuration);
			Async().Forget();
			return;

			async UniTaskVoid Async()
			{
				await UniTask.WaitForSeconds(_disableDuration);
				Enable();
			}
		}
	}
}