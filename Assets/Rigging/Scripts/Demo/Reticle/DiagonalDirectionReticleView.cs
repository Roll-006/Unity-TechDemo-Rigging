using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rigging.Demo.Reticle
{
    public class DiagonalDirectionReticleView : MonoBehaviour, IReticleView
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float opaqueSpeed;
        [SerializeField] private float transparentSpeed;
        [SerializeField] private Image center;
        [SerializeField] private RectTransform topLeft;
        [SerializeField] private RectTransform topRight;
        [SerializeField] private RectTransform bottomLeft;
        [SerializeField] private RectTransform bottomRight;

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
            
            if (topLeft != null) topLeft.anchoredPosition = 
                new Vector2(
                    -MathUtility.GetIsoscelesRightTriangleLegLength(topLeft.sizeDelta.x) / 2f, 
                    MathUtility.GetIsoscelesRightTriangleLegLength(topLeft.sizeDelta.x) / 2f);
            
            if (topRight != null) topRight.anchoredPosition = 
                new Vector2(
                    MathUtility.GetIsoscelesRightTriangleLegLength(topRight.sizeDelta.x) / 2f, 
                    MathUtility.GetIsoscelesRightTriangleLegLength(topRight.sizeDelta.x) / 2f);
            
            if (bottomLeft != null) bottomLeft.anchoredPosition = 
                new Vector2(
                    -MathUtility.GetIsoscelesRightTriangleLegLength(bottomLeft.sizeDelta.x) / 2f, 
                    -MathUtility.GetIsoscelesRightTriangleLegLength(bottomLeft.sizeDelta.x) / 2f);
            
            if (bottomRight != null) bottomRight.anchoredPosition = 
                new Vector2(
                    MathUtility.GetIsoscelesRightTriangleLegLength(bottomRight.sizeDelta.x) / 2f, 
                    -MathUtility.GetIsoscelesRightTriangleLegLength(bottomRight.sizeDelta.x) / 2f);
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