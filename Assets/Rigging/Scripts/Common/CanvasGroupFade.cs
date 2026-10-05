using Rigging.Core;
using UnityEngine;

namespace Rigging.Common
{
    public class CanvasGroupFade : MonoBehaviour
    {
        private enum FadeType
        {
            None,
            FadeIn,
            FadeOut,
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInSpeed;
        [SerializeField] private float fadeOutSpeed;

        private FadeType _fadeType;

        private void Awake()
        {
            Debugger.ValidateSerializeField(canvasGroup, this);
        }

        private void Update()
        {
            switch (_fadeType)
            {
                case FadeType.FadeIn:
                    canvasGroup.alpha += fadeInSpeed * Time.unscaledDeltaTime;
                    if (canvasGroup.alpha >= 1f)
                    {
                        _fadeType = FadeType.None;
                    }
                    break;

                case FadeType.FadeOut:
                    canvasGroup.alpha -= fadeOutSpeed * Time.unscaledDeltaTime;
                    if (canvasGroup.alpha <= 0f)
                    {
                        _fadeType = FadeType.None;
                        canvasGroup.interactable = false;
                        canvasGroup.blocksRaycasts = false;
                    }
                    break;
            }
        }

        public void OnFadeIn()
        {
            _fadeType = FadeType.FadeIn;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void OnFadeOut()
        {
            _fadeType = FadeType.FadeOut;
        }
    }
}
