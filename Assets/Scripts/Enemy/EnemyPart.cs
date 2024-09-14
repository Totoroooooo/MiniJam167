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
		
		private float _health;
		
		public delegate void TimedEvent(float duration);
		public delegate void HitEvent(float health, float maxHealth, float damage);
		
		public event Action Initialized;
		public event Action Enabled;
		public event TimedEvent Disabled;
		public event HitEvent Hit;
		public event Action Corrupted;
		public event Action Died;

		public void OnHit(IHitter hitter)
		{
			float damage = Mathf.Min(_health, this.GetHitDamage(hitter));
			_health -= damage;
			Hit?.Invoke(_health, _maxHealth, damage);
			if (_health <= 0)
				Disable();
		}

		public void Init()
		{
			_health = _maxHealth;
			_collider.enabled = true;
			Initialized?.Invoke();
		}

		public void Enable()
		{
			_health = _maxHealth;
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

		public void Corrupt()
		{
			_collider.enabled = false;
			Corrupted?.Invoke();
		}

		public void Die()
		{
			_collider.enabled = false;
			Died?.Invoke();
		}
	}
}