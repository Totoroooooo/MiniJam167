using DG.Tweening;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(EnemyBody))]
    public class EnemyVisual : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private EnemyBody _body;
        [SerializeField] private SpriteRenderer _renderer;
        [Space]
        [SerializeField] private VisualPart[] _boneVisuals;
        
        [Header("Settings")]
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _corruptedSprite;
        [SerializeField] private Gradient _normalGradient;
        [SerializeField] private Gradient _corruptedGradient;
        [SerializeField] private ColorMemo _flashColor;
        [SerializeField] private FloatMemo _flashDuration;

        private bool _corrupted;
        
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
            Color color = _corrupted
                ? _corruptedGradient.Evaluate(phaseHealth / phaseMaxHealth)
                : _normalGradient.Evaluate(health / maxHealth);
            SetColor(_flashColor.Value);
            DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
        }

        private void OnPhaseChanged(int phase, int maxPhase)
        {
            bool wasCorrupted = _corrupted;
            _corrupted = phase == maxPhase - 1;

            if (wasCorrupted == _corrupted && phase > 0)
                return;
            
            if (_corrupted)
            {
                _renderer.sprite = _corruptedSprite;
                foreach (VisualPart bone in _boneVisuals)
                    bone.Renderer.sprite = bone.CorruptedSprite;
                SetColor(_corruptedGradient.Evaluate(1));
                return;
            }
            
            _renderer.sprite = _normalSprite;
            foreach (VisualPart bone in _boneVisuals)
                bone.Renderer.sprite = bone.NormalSprite;
            SetColor(_normalGradient.Evaluate(1));
        }

        private void OnDied()
        {
            
        }

        private void SetColor(Color color)
        {
            _renderer.color = color;
            foreach (VisualPart bone in _boneVisuals)
                bone.Renderer.color = color;
        }
    }
}