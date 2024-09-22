using System;
using DG.Tweening;
using FMODUnity;
using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyPart : MonoBehaviour, IHittable, ITargetable
	{
		[SerializeField] private StudioEventEmitter _hitEventEmitter;
		[SerializeField] private StudioEventEmitter _wingStateEmitter;
		[Space]
		[SerializeField] private GameObject _container;
		[SerializeField] private GameObject _corruptedContainer;
		[SerializeField] private HitCollision[] _hitCollisions;
		[Space]
		[SerializeField] private Transform _targetPosition;
		[Space]
		[SerializeField] private float _maxHealth = 100f;
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;
		[Space]
		[SerializeField] private float _disableDuration = 5f;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		public bool Targetable => _targetable;
		public Vector3 Position => _targetPosition.position;
		public Transform Transform => _targetPosition.transform;
		
		private bool _targetable;
		private float _health;
		private Tween _tween;
		private bool _isCorrupted;
		
		public Action<bool> TargetableChanged { get => _targetableChanged; set => _targetableChanged = value; }
		private Action<bool> _targetableChanged;
		
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
			foreach (HitCollision collision in _hitCollisions)
				collision.SetHittableParent(this);
		}

		public void OnHit(IHitter hitter)
		{
			_hitEventEmitter.Play();
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
			EnableCollision(true);
			Initialized?.Invoke();
		}

		private void EnableCollision(bool enable)
		{
			_wingStateEmitter.SetParameter("WingState", 2);
			_wingStateEmitter.Play();
			_targetable = enable;
			TargetableChanged?.Invoke(enable);
			foreach (HitCollision collision in _hitCollisions)
				collision.Enable(enable);
		}

		public void Hide()
		{
			_container.SetActive(false);
			_corruptedContainer.SetActive(false);
			EnableCollision(false);
			Hidden?.Invoke();
		}

		public void Enable()
		{
			_health = _maxHealth;
			EnableCollision(true);
			Enabled?.Invoke();
		}

		private void Disable()
		{
			EnableCollision(false);
			Disabled?.Invoke(_disableDuration);
			_tween = DOVirtual.DelayedCall(_disableDuration, Enable).Play();
		}

		public void Corrupt()
		{
			_container.SetActive(false);
			_corruptedContainer.SetActive(true);
			EnableCollision(false);
			_tween?.Kill();
			Corrupted?.Invoke();
		}

		public void Die()
		{
			_wingStateEmitter.SetParameter("WingState", 1);
			_wingStateEmitter.Play();
			EnableCollision(false);
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