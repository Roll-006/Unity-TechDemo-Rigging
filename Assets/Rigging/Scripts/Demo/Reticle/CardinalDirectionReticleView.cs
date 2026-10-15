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
        private float _targetAlpha;
        private bool _isAllowToUseObstacleReticle;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(mainReticleRectTransform, this);
            Debugger.ValidateSerializeField(obstacleReticleRectTransform, this);
            Debugger.ValidateSerializeField(canvasGroup, this);
            
            obstacleReticleRectTransform.gameObject.SetActive(false);
            
            Debug.Log("生成されました。");
        }

        private void OnEnable()
        {
            canvasGroup.alpha = 0f;
        }

        private void OnDisable()
        {
            Debug.Log("非アクティブ化されました。");
        }

        private void OnDestroy()
        {
            Debug.Log("削除されました。");
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

        public void AllowToUseObstacleReticle(bool isAiming)
        {
            _isAllowToUseObstacleReticle = isAiming;
        }
        
        public void ActivateObstacleReticle(Vector3? obstaclePosition)
        {
            if (obstaclePosition.HasValue && _isAllowToUseObstacleReticle)
            {
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
                //obstacleReticleRectTransform.gameObject.SetActive(false);
                
                foreach (var image in images) image.color = Color.white;
            }
        }

        public void DeactivateObstacleReticle(bool isAiming)
        {
            Debug.Log($"削除前 obstacleReticleRectTransform: {obstacleReticleRectTransform}");
            obstacleReticleRectTransform.gameObject.SetActive(false);
            Debug.Log($"削除後 obstacleReticleRectTransform: {obstacleReticleRectTransform}");
        }
        
        public void UpdateAlpha()
        {
            if (Mathf.Abs(_targetAlpha - canvasGroup.alpha) <= Mathf.Epsilon) return;
            
            var speed = canvasGroup.alpha < _targetAlpha ? opaqueSpeed : transparentSpeed;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, _targetAlpha, speed * Time.deltaTime);
        }
    }
}