using System;
using System.Collections.Generic;
using Rigging.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Rigging.Demo.Reticle
{
    [Serializable]
    public struct CardinalDirection
    {
        public RectTransform Left;
        public RectTransform Right;
        public RectTransform Top;
        public RectTransform Bottom;
    }
    
    public class CardinalDirectionReticleView : MonoBehaviour, IReticleView
    {
        [SerializeField] private RectTransform mainReticleRectTransform;
        [SerializeField] private RectTransform obstacleReticleRectTransform;
        [SerializeField] private CardinalDirection mainReticleDirectionTransforms;
        [SerializeField] private CardinalDirection obstacleReticleDirectionTransforms;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float opaqueSpeed;
        [SerializeField] private float transparentSpeed;
        [SerializeField] private Image center;
        [SerializeField] private List<Image> images;
        [SerializeField] private Color obstacleColor;

        private RectTransform _rootRectTransform;
        private bool _isHitObstacle;
        private float _targetAlpha;
        private bool _isAiming;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(mainReticleRectTransform, this);
            Debugger.ValidateSerializeField(obstacleReticleRectTransform, this);
            Debugger.ValidateSerializeField(canvasGroup, this);
            
            obstacleReticleRectTransform.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            canvasGroup.alpha = 0f;
        }

        private void Update()
        {
            UpdateAlpha();
            ActivateObstacleReticle();
        }

        public void Initialize(RectTransform rootRectTransform)
        {
            _rootRectTransform = rootRectTransform;
        }

        public void SetReticleSize(float size)
        {
            ReticlePositionCalculator.CalcCardinalDirectionPosition(mainReticleRectTransform, mainReticleDirectionTransforms, size);
        }

        public void SetObstacleReticleSize(float size)
        {
            ReticlePositionCalculator.CalcCardinalDirectionPosition(obstacleReticleRectTransform, obstacleReticleDirectionTransforms, size);
        }

        public void SetTargetAlpha(float alpha)
        {
            _targetAlpha = alpha;
        }

        public void EnableCenterPoint(bool isReticleClosed)
        {
            if (center != null) center.enabled = isReticleClosed;
        }

        public void SetAimState(bool isAiming)
        {
            _isAiming = isAiming;
        }
        
        public void SetObstaclePosition(Vector3? obstaclePosition)
        {
            _isHitObstacle = obstaclePosition.HasValue;
            
            if (_isHitObstacle)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rootRectTransform,
                    CameraManager.I.OutputCamera.WorldToScreenPoint(obstaclePosition.Value),
                    null,
                    out var localPosition
                );

                obstacleReticleRectTransform.anchoredPosition = localPosition;
            }
        }

        public void ActivateObstacleReticle()
        {
            if (_isAiming && _isHitObstacle)
            {
                if (obstacleReticleRectTransform.gameObject.activeSelf) return;
                
                obstacleReticleRectTransform.gameObject.SetActive(true);
                foreach (var image in images) image.color = obstacleColor;
            }
            else
            {
                if (!obstacleReticleRectTransform.gameObject.activeSelf) return;
                
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