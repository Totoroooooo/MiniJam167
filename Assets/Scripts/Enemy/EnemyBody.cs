using System;
using System.Collections.Generic;
using MiniJam167.HitSystem;
using UnityEngine;

namespace MiniJam167.Enemy
{
	public class EnemyBody : MonoBehaviour, IHittable
	{
		public struct Phase
		{
			public float MaxHealth;
			public EnemyPart[] Parts;
		}

		[Header("References")]
		[SerializeField] private Collider _collider;

		[Header("Phases")]
		[SerializeField] private Phase[] _phases;

		[Header("Health")]
		[SerializeField] private float _shield;
		[SerializeField] private float _damageMultiplier = 1;

		public float Shield => _shield;
		public float DamageMultiplier => _damageMultiplier;
		
		private float _maxHealth;
		private float _health;
		private float _phaseHealth;
		
		private int _currentPhase;

		private readonly List<EnemyPart> _enabledParts = new();
		
		public delegate void HealthEvent(float health, float maxHealth, float phaseHealth, float phaseMaxHealth, float damage);
		public delegate void PhaseEvent(int phase, int maxPhase);
		
		public event HealthEvent HealthChanged;
		public event PhaseEvent PhaseChanged;
		public event Action Died;

		private void Start()
		{
			Init();
		}
        
        public void Init()
        {
	        _enabledParts.Clear();
            _maxHealth = 0;
            for (int phase = 0; phase < _phases.Length - 1; phase++)
                _maxHealth += _phases[phase].MaxHealth;
            _health = _maxHealth;
            _currentPhase = -1;
            NextPhase();
        }
        
        private void NextPhase()
        {
            _currentPhase++;
            Action action = _currentPhase switch
            {
	            int i when i < _phases.Length - 1 && i >= 0 => NormalPhase,
	            int i when i == _phases.Length - 1 => LastPhase,
	            _ => Die
            };
            action.Invoke();
        }

        private void NormalPhase()
        {
	        EnablePhaseParts();
	        SetPhaseHealth();
        }

        private void LastPhase()
        {
	        SetPhaseHealth();
        }

		private void Die()
		{
			_collider.enabled = false;
			foreach (EnemyPart part in _enabledParts)
				part.Die();
			Died?.Invoke();
		}

		private void SetPhaseHealth()
		{
			_phaseHealth = _phases[_currentPhase].MaxHealth;
			PhaseChanged?.Invoke(_currentPhase, _phases.Length);
		}

		private void EnablePhaseParts()
		{
			foreach (EnemyPart part in _enabledParts)
				part.Enable();

			foreach (EnemyPart part in _phases[_currentPhase].Parts)
			{
				part.Init();
				_enabledParts.Add(part);
			}
		}
		
		public void OnHit(IHitter hitter)
		{
			float damage = Mathf.Min(_phaseHealth, this.GetHitDamage(hitter));
			_phaseHealth -= damage;
			_health -= damage;
			HealthChanged?.Invoke(_health, _maxHealth, _phaseHealth, _phases[_currentPhase].MaxHealth, damage);
			
			if (_health <= 0)
				NextPhase();
		}
	}
}