using System;
using MiniJam167.Projectile;
using MiniJam167.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(EnemyPart))]
    public class EnemySkillSpawner : ProjectileSpawner
    {
        [Serializable] public struct Spawn
        {
            public Transform[] Points;
            public Vector2[] RotationRange;
        }
        
        [SerializeField] private Spawn[] _spawns;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _corruptFireRate;
        
        [Header("Components")]
        [SerializeField] private EnemyPart _part;
        
        private float _timer;
        private float _delay;

        private void Reset()
        {
            _part = GetComponent<EnemyPart>();
        }

        private void Awake()
        {
            _part.Initialized += OnEnabled;
            _part.Enabled += OnEnabled;
            _part.Disabled += OnDisabled;
            _part.Corrupted += OnCorrupted;
            _part.Hidden += OnHidden;
        }

        private void OnDestroy()
        {
            _part.Initialized -= OnEnabled;
            _part.Enabled -= OnEnabled;
            _part.Disabled -= OnDisabled;
            _part.Corrupted -= OnCorrupted;
            _part.Hidden -= OnHidden;
        }

        private void Update()
        {
            if (_delay == 0)
                return;
            
            _timer += Time.deltaTime;
            if (_timer >= _delay)
            {
                _timer -= _delay;
                Spawn spawn = _spawns.GetRandom();
                Transform point = spawn.Points.GetRandom();
                Vector2 rotationRange = spawn.RotationRange.GetRandom();
                float rotationDegree = Random.Range(rotationRange.x, rotationRange.y);
                Quaternion rotation = Quaternion.Euler(0, 0, rotationDegree);
                SpawnProjectile(point.transform.position, rotation);
            }
        }

        private void OnEnabled()
        {
            _delay = _fireRate == 0 ? 0 : 1f / _fireRate;
            _timer = _delay;
        }

        private void OnDisabled(float duration)
        {
            _delay = 0;
        }

        private void OnCorrupted()
        {
            _delay = _corruptFireRate == 0 ? 0 : 1f / _corruptFireRate;
        }

        private void OnHidden()
        {
            _delay = 0;
        }
    }
}