using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Characters
{
    public class CameraPivotHumanoidFootIKBodyOffset : MonoBehaviour
    {
        [SerializeField] private IKHumanoidFootPlacement footPlacement;

        private float _initialHeight;

        private void Awake()
        {
            Debugger.ValidateSerializeField(footPlacement, this);
            
            _initialHeight = transform.localPosition.y;
        }

        private void LateUpdate()
        {
            transform.localPosition = new(transform.localPosition.x, _initialHeight, transform.localPosition.z);
            
            var position = transform.position;
            position.y -= footPlacement.CurrentBodyDownOffset;
            transform.position = position;
        }
    }
}