using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Characters
{
    public class CameraPivotHumanoidFootIKBodyOffset : MonoBehaviour
    {
        [SerializeField] private IKHumanoidFootPlacement footPlacement;

        private float _initializeHeight;

        private void Awake()
        {
            Debugger.ValidateSerializeField(footPlacement, this);
            
            _initializeHeight = transform.localPosition.y;
        }

        private void LateUpdate()
        {
            transform.localPosition = new(transform.localPosition.x, _initializeHeight, transform.localPosition.z);
            
            var position = transform.position;
            position.y -= footPlacement.CurrentBodyDownOffset;
            transform.position = position;
        }
    }
}