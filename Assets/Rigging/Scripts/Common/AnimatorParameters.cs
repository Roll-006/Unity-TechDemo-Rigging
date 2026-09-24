using UnityEngine;

namespace Rigging.Common
{
    public static class AnimatorParameters
    {
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");
        public static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        public static readonly int IsCrouching = Animator.StringToHash("IsCrouching");
        public static readonly int IsAiming = Animator.StringToHash("IsAiming");
        public static readonly int HasHandgun = Animator.StringToHash("HasHandgun");
        public static readonly int HasRifle = Animator.StringToHash("HasRifle");
        public static readonly int BackArm = Animator.StringToHash("BackArm");
        public static readonly int ExtendArm = Animator.StringToHash("ExtendArm");
        public static readonly int EquipWeapon = Animator.StringToHash("EquipWeapon");
        public static readonly int UnequipWeapon = Animator.StringToHash("UnequipWeapon");
        public static readonly int TurnSpeed = Animator.StringToHash("TurnSpeed");
        public static readonly int PlayerMoveDirectionX = Animator.StringToHash("PlayerMoveDirectionX");
        public static readonly int PlayerMoveDirectionZ = Animator.StringToHash("PlayerMoveDirectionZ");
        public static readonly int CameraMoveDirectionX = Animator.StringToHash("CameraMoveDirectionX");
    }
}
