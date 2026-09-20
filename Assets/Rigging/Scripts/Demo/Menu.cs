using Rigging.Core;
using Rigging.Common;
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
            Debugger.I.ValidateSerializeField(menuAction, this);
            Debugger.I.ValidateSerializeField(canvasGroup, this);
            Debugger.I.ValidateSerializeField(canvasGroupFade, this);
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
