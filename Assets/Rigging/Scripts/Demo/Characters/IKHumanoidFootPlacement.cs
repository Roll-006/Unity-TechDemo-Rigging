using Rigging.Common;
using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Characters
{
    public class IKHumanoidFootPlacement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField, Range(0f, 1f)] private float distanceToGround;

        private const float KRayOffset = 1f;

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            FootIK(AvatarIKGoal.LeftFoot);
            FootIK(AvatarIKGoal.RightFoot);
        }

        private void FootIK(AvatarIKGoal foot)
        {
            var weight = foot switch
            {
                AvatarIKGoal.LeftFoot => animator.GetFloat(AnimatorParameters.IKLeftFootWeight),
                AvatarIKGoal.RightFoot => animator.GetFloat(AnimatorParameters.IKRightFootWeight),
                _ => 0f
            };
            
            animator.SetIKPositionWeight(foot, weight);
            animator.SetIKRotationWeight(foot, weight);

            var ray = new Ray(animator.GetIKPosition(foot) + new Vector3(0f, KRayOffset, 0f), Vector3.down);
            if (Physics.Raycast(ray, out var hit, distanceToGround + KRayOffset, ~LayerMasks.Player))
            {
                if (hit.transform.gameObject.layer == Layers.Walkable)
                {
                    animator.SetIKPosition(foot, hit.point + new Vector3(0f, distanceToGround, 0f));
                    animator.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}