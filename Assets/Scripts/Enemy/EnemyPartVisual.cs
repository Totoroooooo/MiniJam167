using DG.Tweening;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(EnemyPart))]
    public class EnemyPartVisual : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private EnemyPart _part;
        [SerializeField] private VisualPart[] _featherVisuals;
        
        [Header("Settings")]
        [SerializeField] private Gradient _normalGradient;
        [Space]
        [SerializeField] private ColorMemo _flashColor;
        [SerializeField] private FloatMemo _flashDuration;
        [Space]
        [SerializeField] private ColorMemo _disabledColor;
        [SerializeField] private FloatMemo _enabledDuration;
        [Space]
        [SerializeField] private ColorMemo _corruptedColor;

        private void Reset()
        {
            _part = GetComponent<EnemyPart>();
        }

        private void Awake()
        {
            _part.Initialized += OnInitialized;
            _part.Hidden += OnHidden;
            _part.Enabled += OnEnabled;
            _part.Disabled += OnDisabled;
            _part.Hit += OnHit;
            _part.Corrupted += OnCorrupted;
        }

        private void OnDestroy()
        {
            _part.Initialized -= OnInitialized;
            _part.Hidden -= OnHidden;
            _part.Enabled -= OnEnabled;
            _part.Disabled -= OnDisabled;
            _part.Hit -= OnHit;
            _part.Corrupted -= OnCorrupted;
        }

        private void OnInitialized()
        {
            Color color = _normalGradient.Evaluate(0f);
            foreach (VisualPart bone in _featherVisuals)
                bone.Renderer.sprite = bone.NormalSprite;

            OnEnabled();
        }

        private void OnHidden()
        {
            SetColor(Color.clear);
        }

        private void OnEnabled()
        {
            DOVirtual.Color(_disabledColor.Value, _normalGradient.Evaluate(1), _enabledDuration.Value, SetColor).Play();
        }

        private void OnDisabled(float duration)
        {
            
        }

        private void OnHit(float health, float maxHealth, float damage)
        {
            Color color = health == 0
                ? _disabledColor.Value
                : _normalGradient.Evaluate(health / maxHealth);
            SetColor(_flashColor.Value);
            DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
        }

        private void OnCorrupted()
        {
            Color color = _corruptedColor.Value;
            foreach (VisualPart bone in _featherVisuals)
                bone.Renderer.sprite = bone.CorruptedSprite;
            SetColor(_flashColor.Value);
            DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
        }
        
        private void SetColor(Color color)
        {
            foreach (VisualPart feather in _featherVisuals)
                feather.Renderer.color = color;
        }
    }
}