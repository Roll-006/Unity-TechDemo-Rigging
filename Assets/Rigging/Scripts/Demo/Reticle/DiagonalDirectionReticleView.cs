using System;
using System.Collections.Generic;
using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public struct DiagonalDirection
    {
        public RectTransform TopLeft;
        public RectTransform TopRight;
        public RectTransform BottomLeft;
        public RectTransform BottomRight;
    }
    
    public class DiagonalDirectionReticleView : MonoBehaviour, IReticleView
    {
        [SerializeField] private RectTransform mainReticleRectTransform;
        [SerializeField] private RectTransform obstacleReticleRectTransform;
        [SerializeField] private DiagonalDirection mainReticleDirectionTransforms;
        [SerializeField] private DiagonalDirection obstacleReticleDirectionTransforms;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float opaqueSpeed;
        [SerializeField] private float transparentSpeed;
        [SerializeField] private Image center;
        [SerializeField] private List<Image> images;
        [SerializeField] private Color obstacleColor;

        private RectTransform _rootRectTransform;
        private float _targetAlpha;
        private bool _isAllowToUseObstacleReticle;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(mainReticleRectTransform, this);
            Debugger.ValidateSerializeField(obstacleReticleRectTransform, this);
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
        
        public void Initialize(RectTransform rootRectTransform)
        {
            _rootRectTransform = rootRectTransform;
        }

        public void SetReticleSize(float size)
        {
            ReticlePositionCalculator.CalcDiagonalDirectionPosition(mainReticleRectTransform, mainReticleDirectionTransforms, size);
        }
        
        public void SetObstacleReticleSize(float size)
        {
            ReticlePositionCalculator.CalcDiagonalDirectionPosition(obstacleReticleRectTransform, obstacleReticleDirectionTransforms, size);
        }

        public void SetTargetAlpha(float alpha)
        {
            _targetAlpha = alpha;
        }
        
        public void SetEnableCenterPoint(bool isReticleClosed)
        {
            if (center != null) center.enabled = isReticleClosed;
        }
        
        public void SetAllowToUseObstacleReticle(bool isAiming)
        {
            _isAllowToUseObstacleReticle = isAiming;
        }
        
        public void SetPositionObstacleReticle(Vector3? obstaclePosition)
        {
            if (obstaclePosition.HasValue)
            {
                if (!_isAllowToUseObstacleReticle) return;
                
                obstacleReticleRectTransform.gameObject.SetActive(true);
                
                foreach (var image in images) image.color = obstacleColor;
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rootRectTransform,
                    CameraManager.I.OutputCamera.WorldToScreenPoint(obstaclePosition.Value),
                    null,
                    out var localPosition
                );
                
                obstacleReticleRectTransform.anchoredPosition = localPosition;
            }
            else
            {
                obstacleReticleRectTransform.gameObject.SetActive(false);
                
                foreach (var image in images) image.color = Color.white;
            }
        }

        public void UpdateAlpha()
        {
            if (Mathf.Abs(_targetAlpha - canvasGroup.alpha) <= Mathf.Epsilon) return;
            
            var speed = canvasGroup.alpha < _targetAlpha ? opaqueSpeed : transparentSpeed;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, _targetAlpha, speed * Time.deltaTime);
        }
    }
}
