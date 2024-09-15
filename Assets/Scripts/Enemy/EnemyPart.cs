using System;
using DG.Tweening;
using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyPart : MonoBehaviour, IHittable
	{
		[SerializeField] private Collider2D _collider;
		[SerializeField] private GameObject _container;
		[SerializeField] private GameObject _corruptedContainer;
		[Space]
		[SerializeField] private float _maxHealth = 100f;
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;
		[Space]
		[SerializeField] private float _disableDuration = 5f;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		
		private float _health;
		private Tween _tween;
		private bool _isCorrupted;
		
		public delegate void TimedEvent(float duration);
		public delegate void HitEvent(float health, float maxHealth, float damage);
		
		public event Action Initialized;
		public event Action Hidden;
		public event Action Enabled;
		public event TimedEvent Disabled;
		public event HitEvent Hit;
		public event Action Corrupted;
		public event Action Protected;
		public event Action Died;

		private void Awake()
		{
			_container.SetActive(false);
			_corruptedContainer.SetActive(false);
		}

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
			_container.SetActive(true);
			_corruptedContainer.SetActive(false);
			_health = _maxHealth;
			_collider.enabled = true;
			Initialized?.Invoke();
		}

		public void Hide()
		{
			_container.SetActive(false);
			_corruptedContainer.SetActive(false);
			_collider.enabled = false;
			Hidden?.Invoke();
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
			_tween = DOVirtual.DelayedCall(_disableDuration, Enable).Play();
		}

		public void Corrupt()
		{
			_container.SetActive(false);
			_corruptedContainer.SetActive(true);
			_collider.enabled = false;
			_tween?.Kill();
			Corrupted?.Invoke();
		}

		public void Die()
		{
			_collider.enabled = false;
			Died?.Invoke();
		}

		public bool Protect(IHitter hitter)
		{
			if (_isCorrupted || _health <= 0) return false;
			
			Protected?.Invoke();
			OnHit(hitter);
			return true;
		}
	}
}