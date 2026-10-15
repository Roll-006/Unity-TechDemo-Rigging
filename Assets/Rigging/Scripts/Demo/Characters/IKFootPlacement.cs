using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Characters
{
    [RequireComponent(typeof(Animator))]
    public class IKFootPlacement : UnityEngine.MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private const float KRayOffset = 1f;
        
        private float _distanceToGround;

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

            var ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + new Vector3(0f, KRayOffset, 0f), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, _distanceToGround + KRayOffset))
            {
                
            }
        }
    }
}