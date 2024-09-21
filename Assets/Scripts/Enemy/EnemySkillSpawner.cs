using System;
using DG.Tweening;
using MiniJam167.Projectile;
using MiniJam167.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(EnemyPart))]
    public class EnemySkillSpawner : ProjectileSpawner
    {
        [Serializable] public struct Pattern
        {
            public float DelayBetweenEachPart;
            public float DelayAfterPattern;
            public PatternPart[] Parts;
        }
        
        [Serializable] public struct PatternPart
        {
            public Transform Point;
            public float Rotation;
            public float Delay;
        }
        
        [Serializable] public struct Spawn
        {
            public Transform[] Points;
            public Vector2[] RotationRange;
        }
        
        [SerializeField] private Pattern[] _patterns;
        [SerializeField] private Pattern[] _corruptPatterns;
        [SerializeField] private Spawn[] _spawns;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _corruptFireRate;
        
        [Header("Components")]
        [SerializeField] private EnemyPart _part;
        
        private float _timer;
        private float _delay;
        private bool _isCorrupted;
        private Sequence _patternTween;

        private void Reset()
        {
            _part = GetComponent<EnemyPart>();
        }

        private void Awake()
        {
            _part.Initialized += OnInitialized;
            _part.Enabled += OnEnabled;
            _part.Disabled += OnDisabled;
            _part.Corrupted += OnCorrupted;
            _part.Hidden += OnHidden;
        }

        private void OnDestroy()
        {
            _part.Initialized -= OnInitialized;
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
                SpawnProjectile(point.transform.position, rotation, point);
            }
        }

        private void OnInitialized()
        {
            _isCorrupted = false;
            OnEnabled();
        }

        private void OnEnabled()
        {
            _delay = _fireRate == 0 ? 0 : 1f / _fireRate;
            _timer = _delay;
            SetNewPattern();
        }

        private void OnDisabled(float duration)
        {
            _patternTween?.Kill();
            _delay = 0;
        }

        private void OnCorrupted()
        {
            _isCorrupted = true;
            SetNewPattern();
            _delay = _corruptFireRate == 0 ? 0 : 1f / _corruptFireRate;
        }

        private void OnHidden()
        {
            _patternTween?.Kill();
            _delay = 0;
        }

        private void SetNewPattern()
        {
            var patterns = _isCorrupted ? _corruptPatterns : _patterns;
            if (patterns.Length == 0)
                return;
            
            Pattern pattern = patterns.GetRandom();
            
            _patternTween?.Kill();
            _patternTween = DOTween.Sequence();

            foreach (PatternPart part in pattern.Parts)
                _patternTween
                    .AppendInterval(part.Delay + pattern.DelayBetweenEachPart)
                    .AppendCallback(() =>
                    {
                        Quaternion rotation = Quaternion.Euler(0, 0, part.Rotation);
                        SpawnProjectile(part.Point.position, rotation, part.Point);
                    });
            
            _patternTween
                .AppendInterval(pattern.DelayAfterPattern)
                .AppendCallback(SetNewPattern)
                .SetAutoKill()
                .Play();
        }
    }
}