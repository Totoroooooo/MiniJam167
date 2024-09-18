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
        [SerializeField] private Sprite _normalFaceSpriteEye;
        [SerializeField] private Sprite _normalFaceSpriteDamage;
        [SerializeField] private Sprite _normalFaceSpriteProtect;
        [SerializeField] private Sprite _normalFaceSpriteHeal;
        [SerializeField] private Sprite _normalFaceSpriteLaser;
        [SerializeField] private Sprite _normalFaceSpriteEnd;
        [SerializeField] private Gradient _normalGradient;
        [SerializeField] private float _protectFaceDuration = 1;
        
        [Header("Normal Movement")]
        [SerializeField] private float _normalMovementValue = 1;
        [SerializeField] private float _normalMovementDuration = 1;
        [SerializeField] private AnimationCurve _normalMovementCurve;
        
        [Header("Hurt Movement")]
        [SerializeField] private float _hurtMovementValue = 1;
        [SerializeField] private float _hurtMovementDuration = 1;
        [SerializeField] private int _hurtMovementLoops = 4;
        [SerializeField] private AnimationCurve _hurtMovementCurve;
        
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
        
        private Tween _normalMovementTween;
        private Tween _hurtTween;
        
        public delegate void ColorSetEvent(Color color);
        
        public event ColorSetEvent ColorSet;
        
        private void Reset()
        {
            _body = GetComponent<EnemyBody>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            _body.Shielded += OnShielded;
            _body.HealthChanged += OnHealthChanged;
            _body.PhaseChanged += OnPhaseChanged;
            _body.Died += OnDied;

            _normalMovementTween = SetNormalMovementTween();
            _hurtTween = SetHurtTween().Pause();
        }

        private void Start()
        {
            foreach (EnemyPartVisual part in _parts)
                part.Init(this);
        }

        private void OnDestroy()
        {
            _body.Shielded -= OnShielded;
            _body.HealthChanged -= OnHealthChanged;
            _body.PhaseChanged -= OnPhaseChanged;
            _body.Died -= OnDied;
        }

        private Tween SetNormalMovementTween()
        {
            return transform.DOLocalMoveY(transform.position.y + _normalMovementValue, _normalMovementDuration)
                .SetEase(_normalMovementCurve)
                .SetLoops(-1, LoopType.Yoyo)
                .Pause();
        }
        
        private Tween SetHurtTween()
        {
            return transform.DOLocalMoveX(transform.position.x + _hurtMovementValue, _hurtMovementDuration)
                .SetEase(_hurtMovementCurve)
                .SetLoops(_hurtMovementLoops, LoopType.Yoyo)
                .SetAutoKill(false)
                .Pause();
        }
        
        private void OnShielded()
        {
            _normalFaceRenderer.sprite = _normalFaceSpriteProtect;
            DOVirtual.DelayedCall(_protectFaceDuration, () => _normalFaceRenderer.sprite = _normalFaceSpriteDefault).Play();
        }

        private void OnHealthChanged(float health, float maxHealth, float phaseHealth, float phaseMaxHealth, float damage)
        {
            if (health == 0)
                return;
            
            _hurtTween.Restart();
            Color color;
            if (_corrupted)
            {
                float alpha = phaseHealth / phaseMaxHealth;
                color = _corruptedGradient.Evaluate(alpha);
                _scleraRenderer.color = _scleraGradient.Evaluate(alpha);
            }
            else
            {
                _normalMovementTween.Pause();
                DOVirtual.DelayedCall(_flashDuration.Value, () => _normalMovementTween.Play());
                _normalFaceRenderer.sprite = _normalFaceSpriteDamage;
                DOVirtual.DelayedCall(_flashDuration.Value, () => _normalFaceRenderer.sprite = _normalFaceSpriteDefault).Play();
                color = _normalGradient.Evaluate(health / maxHealth);
            }
            SetColorWithoutNotify(_flashColor.Value);
            ColorSet?.Invoke(color);
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
            _normalMovementTween.Pause();
            _renderer.sprite = _corruptedSprite;
            _normalFace.SetActive(false);
            _corruptedFace.SetActive(true);
            SetColor(_corruptedGradient.Evaluate(1));
        }
        
        private void SetNormal()
        {
            _normalMovementTween.Play();
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