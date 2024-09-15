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
        
        [Header("Normal")]
        [SerializeField] private SpriteRenderer _normalFaceRenderer;
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
        
        [Header("Body Parts")]
        [SerializeField] private VisualPart[] _boneVisuals;
        
        [Header("Damage")]
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
            DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
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
            _corruptedFace.SetActive(true);
            foreach (VisualPart bone in _boneVisuals)
                bone.Renderer.sprite = bone.CorruptedSprite;
            SetColor(_corruptedGradient.Evaluate(1));
        }
        
        private void SetNormal()
        {
            _renderer.sprite = _normalSprite;
            _corruptedFace.SetActive(false);
            foreach (VisualPart bone in _boneVisuals)
                bone.Renderer.sprite = bone.NormalSprite;
            SetColor(_normalGradient.Evaluate(1));
        }

        private void SetPhaseComponents(bool corrupted)
        {
            _corruptedFace.SetActive(corrupted);
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