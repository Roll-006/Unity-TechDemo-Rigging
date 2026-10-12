using Rigging.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rigging.Demo.Reticle
{
    public class CardinalDirectionReticleView : MonoBehaviour, IReticleView
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float opaqueSpeed;
        [SerializeField] private float transparentSpeed;
        [SerializeField] private Image center;
        [SerializeField] private RectTransform left;
        [SerializeField] private RectTransform right;
        [SerializeField] private RectTransform top;
        [SerializeField] private RectTransform bottom;

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

        public void SetReticleSize(float size)
        {
            rectTransform.sizeDelta = new Vector2(size, size);
            
            if (left != null) left.anchoredPosition = new Vector2(-left.sizeDelta.x / 2f, 0f);
            if (right != null) right.anchoredPosition = new Vector2(right.sizeDelta.x / 2f, 0f);
            if (top != null) top.anchoredPosition = new Vector2(0f, top.sizeDelta.y / 2f);
            if (bottom != null) bottom.anchoredPosition = new Vector2(0f, -bottom.sizeDelta.y / 2f);
        }

        public void SetTargetAlpha(float alpha)
        {
            _targetAlpha = alpha;
        }

        public void SetMinReticleSize(bool isMinReticleSize)
        {
            if (center != null) center.enabled = isMinReticleSize;
        }

        public void UpdateAlpha()
        {
            if (Mathf.Abs(_targetAlpha - canvasGroup.alpha) <= Mathf.Epsilon) return;
            
            var speed = canvasGroup.alpha < _targetAlpha ? opaqueSpeed : transparentSpeed;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, _targetAlpha, speed * Time.deltaTime);
        }
    }
}