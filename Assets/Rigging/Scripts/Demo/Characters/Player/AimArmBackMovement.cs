using System;
using Rigging.Core;
using Rigging.Common;
using Rigging.Demo.Items;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class AimArmBackMovement : MonoBehaviour
    {
        [SerializeField] private AimTrigger aimTrigger;
        [SerializeField] private AimTrigger aimArmBackTrigger;
        [SerializeField] private AimTrigger aimBackTrigger;
        [SerializeField] private Transform spine2Pivot;
        [SerializeField, Tooltip("AimTrigger の radius より大きくある必要あり。")] private float extendArmTriggerRadius;
        
        public AimTrigger AimTrigger => aimTrigger;
        public AimTrigger AimArmBackTrigger => aimArmBackTrigger;
        public AimTrigger AimBackTrigger => aimBackTrigger;
        
        private readonly Collider[] _hitColliders = new Collider[16];
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(aimTrigger, this);
            Debugger.ValidateSerializeField(aimArmBackTrigger, this);
            Debugger.ValidateSerializeField(aimBackTrigger, this);
            Debugger.ValidateSerializeField(spine2Pivot, this);
            
            var backArmTriggerRadius = Debugger.ValidateComponent<SphereCollider>(aimTrigger).radius;
            if (extendArmTriggerRadius <= backArmTriggerRadius)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(extendArmTriggerRadius), 
                    $"不正な値です。有効範囲は [ {nameof(aimTrigger)} の radius < {nameof(extendArmTriggerRadius)} ] です。\n" +
                    $"{nameof(aimTrigger)} の radius: {backArmTriggerRadius}\n" + 
                    $"{nameof(extendArmTriggerRadius)}: {extendArmTriggerRadius}");
            }
        }

        /// <summary>
        /// 腕を引く
        /// </summary>
        /// <param name="animator">アニメーター</param>
        /// <param name="currentAimTrigger">現在銃が位置しているトリガー</param>
        /// <param name="wasNotifiedTrigger">既に腕を引くアニメーション通知を送ったかどうか</param>
        /// <returns>true: 腕を引く通知送信に成功, false: 腕を引く通知送信に失敗</returns>
        public bool BackArm(Animator animator, AimTrigger currentAimTrigger, bool wasNotifiedTrigger)
        {
            if (wasNotifiedTrigger) return false;
            if (!currentAimTrigger.IsHit) return false;
            
            animator.SetTrigger(AnimatorParameters.BackArm);
            return true;
        }
        
        /// <summary>
        /// 腕を伸ばす
        /// </summary>
        /// <param name="animator">アニメーター</param>
        /// <param name="currentAimTrigger">現在銃が位置しているトリガー</param>
        /// <param name="targetAimTrigger">次銃が位置するトリガー</param>
        /// <param name="wasNotifiedTrigger">既に腕を伸ばすアニメーション通知を送ったかどうか</param>
        /// <returns>true: 腕を伸ばす通知送信に成功, false: 腕を伸ばす通知送信に失敗</returns>
        public bool ExtendArm(Animator animator, AimTrigger currentAimTrigger, AimTrigger targetAimTrigger, bool wasNotifiedTrigger)
        {
            if (wasNotifiedTrigger) return false;

            if (!CheckCapsule(currentAimTrigger, targetAimTrigger)) return false;
            if (!CheckRay(targetAimTrigger)) return false;
            
            animator.SetTrigger(AnimatorParameters.ExtendArm);
            return true;
        }

        private bool CheckCapsule(AimTrigger currentAimTrigger, AimTrigger targetAimTrigger)
        {
            var hitCount = Physics.OverlapCapsuleNonAlloc(
                currentAimTrigger.transform.position,
                targetAimTrigger.transform.position,
                extendArmTriggerRadius,
                _hitColliders,
                ~LayerMasks.Player,
                QueryTriggerInteraction.Ignore);

            for (var i = 0; i < hitCount; i++)
            {
                if (_hitColliders[i].TryGetComponent<IOwnable>(out var ownable) && ownable.Owner == gameObject) continue;

                return false;
            }

            return true;
        }

        private bool CheckRay(AimTrigger targetAimTrigger)
        {
            var spine2ToTarget = targetAimTrigger.transform.position - spine2Pivot.position;
            if (Physics.Raycast(
                    spine2Pivot.position,
                    spine2ToTarget.normalized,
                    out var hitInfo,
                    spine2ToTarget.magnitude,
                    ~LayerMasks.Player,
                    QueryTriggerInteraction.Ignore))
            {
                if (!hitInfo.collider.TryGetComponent<IOwnable>(out var ownable) || ownable.Owner != gameObject) return false;
            }
                
            return true;
        }
    }
}