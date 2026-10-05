using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public class CardinalDirectionReticleView : MonoBehaviour, IReticleView
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float opaqueSpeed;
        [SerializeField] private float transparentSpeed;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;

        private ReticleViewModel _viewModel;
        private float _targetAlpha;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(rectTransform, this);
            Debugger.ValidateSerializeField(canvasGroup, this);
        }

        private void OnEnable()
        {
            canvasGroup.alpha = 0f;
        }

        private void Update()
        {
            UpdateAlpha();
        }

        private void OnDisable()
        {
            _viewModel.ReticleSizeChanged -= OnSetReticleSize;
            _viewModel.ReticleAlphaChanged -= OnSetAlpha;
        }

        public void SetViewModel(ReticleViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.ReticleSizeChanged += OnSetReticleSize;
            _viewModel.ReticleAlphaChanged += OnSetAlpha;
        }

        public void OnSetReticleSize(float size)
        {
            rectTransform.sizeDelta = new Vector2(size, size);
            
            if (left != null) left.anchoredPosition = new Vector2(-left.sizeDelta.x / 2f, 0f);
            if (right != null) right.anchoredPosition = new Vector2(right.sizeDelta.x / 2f, 0f);
            if (top != null) top.anchoredPosition = new Vector2(0f, top.sizeDelta.y / 2f);
            if (bottom != null) bottom.anchoredPosition = new Vector2(0f, -bottom.sizeDelta.y / 2f);
        }

        public void OnSetAlpha(float alpha)
        {
            _targetAlpha = alpha;
        }

        private void UpdateAlpha()
        {
            if (Mathf.Abs(_targetAlpha - canvasGroup.alpha) <= Mathf.Epsilon) return;
            
            var speed = canvasGroup.alpha < _targetAlpha ? opaqueSpeed : transparentSpeed;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, _targetAlpha, speed * Time.deltaTime);
        }
    }
}