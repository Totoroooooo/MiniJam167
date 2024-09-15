using System;
using DG.Tweening;
using MiniJam167.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(EnemyBody))]
    public class EnemyVisual : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private EnemyBody _body;
        [SerializeField] private SpriteRenderer _renderer;
        
        [Header("Normal")]
        [SerializeField] private SpriteRenderer _normalFaceRenderer;
        [SerializeField] private GameObject _normalFace;
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _normalFaceSpriteDefault;
        [SerializeField] private Sprite _normalFaceSpriteDamage;
        [SerializeField] private Gradient _normalGradient;
        
        [Header("Corrupted")]
        [SerializeField] private GameObject _corruptedFace;
        [SerializeField] private Sprite _corruptedSprite;
        [SerializeField] private SpriteRenderer _scleraRenderer;
        [SerializeField] private Gradient _corruptedGradient;
        [SerializeField] private Gradient _scleraGradient;
        
        [Header("Damage")]
        [SerializeField] private ColorMemo _flashColor;
        [SerializeField] private FloatMemo _flashDuration;
        
        [Header("Parts")]
        [SerializeField] private EnemyPartVisual[] _parts;

        private bool _corrupted;
        
        public delegate void ColorSetEvent(Color color);
        
        public event ColorSetEvent ColorSet;
        
        private void Reset()
        {
            _body = GetComponent<EnemyBody>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            _body.HealthChanged += OnHealthChanged;
            _body.PhaseChanged += OnPhaseChanged;
            _body.Died += OnDied;
        }

        private void Start()
        {
            foreach (EnemyPartVisual part in _parts)
                part.Init(this);
        }

        private void OnDestroy()
        {
            _body.HealthChanged -= OnHealthChanged;
            _body.PhaseChanged -= OnPhaseChanged;
            _body.Died -= OnDied;
        }

        private void OnHealthChanged(float health, float maxHealth, float phaseHealth, float phaseMaxHealth, float damage)
        {
            if (health == 0)
                return;

            Color color;
            if (_corrupted)
            {
                float alpha = phaseHealth / phaseMaxHealth;
                color = _corruptedGradient.Evaluate(alpha);
                _scleraRenderer.color = _scleraGradient.Evaluate(alpha);
            }
            else
            {
                color = _normalGradient.Evaluate(health / maxHealth);
            }
            SetColor(_flashColor.Value);
            DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColorWithoutNotify).Play();
        }

        private void OnPhaseChanged(int phase, int maxPhase)
        {
            bool wasCorrupted = _corrupted;
            _corrupted = phase == maxPhase - 1;

            if (wasCorrupted == _corrupted && phase > 0)
                return;
            
            if (_corrupted)
                SetCorrupted();
            else
                SetNormal();
        }
        
        private void SetCorrupted()
        {
            _renderer.sprite = _corruptedSprite;
            _normalFace.SetActive(false);
            _corruptedFace.SetActive(true);
            SetColor(_corruptedGradient.Evaluate(1));
        }
        
        private void SetNormal()
        {
            _renderer.sprite = _normalSprite;
            _normalFace.SetActive(true);
            _corruptedFace.SetActive(false);
            SetColor(_normalGradient.Evaluate(1));
        }

        private void OnDied()
        {
            
        }

        private void SetColor(Color color)
        {
            _renderer.color = color;
            ColorSet?.Invoke(color);
        }

        private void SetColorWithoutNotify(Color color)
        {
            _renderer.color = color;
        }
    }
}