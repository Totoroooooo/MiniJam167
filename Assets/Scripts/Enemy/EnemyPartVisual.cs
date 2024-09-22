using DG.Tweening;
using MiniJam167.Utility;
using UnityEngine;

namespace MiniJam167.Enemy
{
    [RequireComponent(typeof(EnemyPart))]
    [RequireComponent(typeof(Animator))]
    public class EnemyPartVisual : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private EnemyPart _part;
        [SerializeField] private Animator _animator;
        [SerializeField] private VisualPart[] _featherVisuals;
	    [SerializeField] private VisualPart[] _boneVisuals;
        
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

        private EnemyVisual _body;
        private Tween _flashTween;
        private bool _isCorrupted;
        
        private void Reset()
        {
            _part = GetComponent<EnemyPart>();
            _animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            _part.Initialized += OnInitialized;
            _part.Hidden += OnHidden;
            _part.Enabled += OnEnabled;
            _part.Disabled += OnDisabled;
            _part.Hit += OnHit;
            _part.Corrupted += OnCorrupted;
            _part.Protected += OnProtected;
        }

        private void OnDestroy()
        {
            _part.Initialized -= OnInitialized;
            _part.Hidden -= OnHidden;
            _part.Enabled -= OnEnabled;
            _part.Disabled -= OnDisabled;
            _part.Hit -= OnHit;
            _part.Corrupted -= OnCorrupted;
            _part.Protected -= OnProtected;
            _body.ColorSet -= SetBoneColor;
        }

        public void Init(EnemyVisual body)
        {
            _body = body;
            _body.ColorSet += SetBoneColor;
        }

        private void OnInitialized()
        {
            _animator.Play("Open");
            OnEnabled();
        }

        private void OnHidden()
        {
            SetColor(Color.clear);
        }

        private void OnEnabled()
        {
            if (_isCorrupted) return;
            _flashTween?.Kill();
            _flashTween = DOVirtual.Color(_disabledColor.Value, _normalGradient.Evaluate(1), _enabledDuration.Value, SetColor).Play();
        }

        private void OnDisabled(float duration)
        {
            
        }

        private void OnHit(float health, float maxHealth, float damage)
        {
            if (_isCorrupted) return;
            Color color = health == 0
                ? _disabledColor.Value
                : _normalGradient.Evaluate(health / maxHealth);
            SetColor(_flashColor.Value);
            _flashTween?.Kill();
            _flashTween = DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
        }

        private void OnCorrupted()
        {
            _animator.Play("Open Corrupted");
            _isCorrupted = true;
            Color color = _corruptedColor.Value;
            SetColor(_flashColor.Value);
            _flashTween?.Kill();
            _flashTween = DOVirtual.Color(_flashColor.Value, color, _flashDuration.Value, SetColor).Play();
        }
        
        private void SetColor(Color color)
        {
            foreach (VisualPart feather in _featherVisuals)
                feather.Renderer.color = color;
        }
        
        private void SetBoneColor(Color color)
        {
            foreach (VisualPart bone in _boneVisuals)
                bone.Renderer.color = color;
        }

        private void OnProtected()
        {
            _animator.Play("Protect");
        }
    }
}