using Rigging.Core;
using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class AimArmBackMovement : MonoBehaviour
    {
        [SerializeField] private AimTrigger aimTrigger;
        [SerializeField] private AimTrigger aimArmBackTrigger;
        [SerializeField] private AimTrigger aimBackTrigger;
        [SerializeField] private Transform spine2Pivot;
        
        public AimTrigger AimTrigger => aimTrigger;
        public AimTrigger AimArmBackTrigger => aimArmBackTrigger;
        public AimTrigger AimBackTrigger => aimBackTrigger;
        
        private void Awake()
        {
            Debugger.I.ValidateSerializeField(aimTrigger, this);
            Debugger.I.ValidateSerializeField(aimArmBackTrigger, this);
            Debugger.I.ValidateSerializeField(aimBackTrigger, this);
            Debugger.I.ValidateSerializeField(spine2Pivot, this);
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

            if (Physics.CheckCapsule(
                    currentAimTrigger.transform.position,
                    targetAimTrigger.transform.position,
                    0.1f,
                    ~Layers.Player,
                    QueryTriggerInteraction.Ignore))
                return false;
            
            var spine2ToTarget = targetAimTrigger.transform.position - spine2Pivot.position;
            if (Physics.Raycast(
                    spine2Pivot.position,
                    spine2ToTarget.normalized,
                    spine2ToTarget.magnitude,
                    ~Layers.Player,
                    QueryTriggerInteraction.Ignore))
                return false;
            
            animator.SetTrigger(AnimatorParameters.ExtendArm);
            
            return true;
        }
    }
}
