using Rigging.Common;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private InputActionReference menuAction;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private CanvasGroupFade canvasGroupFade;

        private bool _isActive;

        private void Awake()
        {
            if (menuAction == null)
                throw new InvalidOperationException($"{nameof(menuAction)} ({typeof(InputActionReference).Name}) ‚ÌŽæ“¾‚ÉŽ¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");

            if (canvasGroup == null)
                throw new InvalidOperationException($"{nameof(canvasGroup)} ({typeof(CanvasGroup).Name}) ‚ÌŽæ“¾‚ÉŽ¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");

            if (canvasGroupFade == null)
                throw new InvalidOperationException($"{nameof(canvasGroupFade)} ({typeof(CanvasGroupFade).Name}) ‚ÌŽæ“¾‚ÉŽ¸”s‚µ‚Ü‚µ‚½BSerializeField ‚É“o˜^‚µ‚Ä‚­‚¾‚³‚¢B");
        }

        private void Start()
        {
            _isActive = canvasGroup.alpha > 0f;
        }

        private void OnEnable()
        {
            menuAction.action.started += OnMenu;
        }

        private void OnDisable()
        {
            menuAction.action.started -= OnMenu;
        }

        private void OnMenu(InputAction.CallbackContext context)
        {
            _isActive = !_isActive;
            if (_isActive)
            {
                canvasGroupFade.OnFadeIn();
            }
            else
            {
                canvasGroupFade.OnFadeOut();
            }
        }
    }
}
