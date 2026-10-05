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
            Debugger.ValidateSerializeField(menuAction, this);
            Debugger.ValidateSerializeField(canvasGroup, this);
            Debugger.ValidateSerializeField(canvasGroupFade, this);
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
