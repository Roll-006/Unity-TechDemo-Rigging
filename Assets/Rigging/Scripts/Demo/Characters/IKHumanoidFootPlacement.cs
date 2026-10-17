using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Rigging.Demo.Characters
{
    /// <summary>
    /// 人型モデルの足をIKにより地面に設置させる。
    /// </summary>
    public class IKHumanoidFootPlacement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private LandingDetector landingDetector;
        [SerializeField, Range(0f, 1f)] private float distanceFootToGround;
        [SerializeField, Min(0f)] private float maxRootBoneDownDistance;
        [SerializeField, Range(0f, 1f)] private float bodyDownMagnitude;
        [SerializeField, Min(0f)] private float bodyDampTime;
        [SerializeField, Min(0f)] private float footDampTime;
        [SerializeField, Min(0f)] private float rotateSpeed;
        
        [SerializeField, Min(0f), Tooltip("足が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float footRayOffset;
        
        [SerializeField, Min(0f), Tooltip("つま先が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float toeRayOffset;

        private float _currentBodyDownOffset;
        private float _bodyDownOffsetVelocity;
        private Vector3 _currentLeftFootPosition;
        private Vector3 _leftFootVelocity;
        private Vector3 _currentRightFootPosition;
        private Vector3 _rightFootVelocity;
        private Quaternion _currentLeftFootRotation;
        private Quaternion _currentRightFootRotation;
        private RaycastHit _leftFootFlotationHitInfo;
        private RaycastHit _rightFootFlotationHitInfo;
        private float _distanceFootToToe;

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(landingDetector, this);
        }

        private void Start()
        {
            _distanceFootToToe = Vector3.Distance(animator.GetBoneTransform(HumanBodyBones.LeftFoot).position, animator.GetBoneTransform(HumanBodyBones.LeftToes).position);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            DownBody();
            FootIK(AvatarIKGoal.LeftFoot);
            FootIK(AvatarIKGoal.RightFoot);
        }
        
        /// <summary>
        /// FootIK を行う。
        /// </summary>
        /// <param name="foot">足の IKGoal</param>
        private void FootIK(AvatarIKGoal foot)
        {
            if (foot != AvatarIKGoal.LeftFoot && foot != AvatarIKGoal.RightFoot) return;

            var weight = animator.GetFloat(foot == AvatarIKGoal.LeftFoot ? AnimatorParameters.IKLeftFootWeight : AnimatorParameters.IKRightFootWeight);
            var flotationHitInfo = foot == AvatarIKGoal.LeftFoot ? _leftFootFlotationHitInfo : _rightFootFlotationHitInfo;
            
            animator.SetIKPositionWeight(foot, weight);
            animator.SetIKRotationWeight(foot, weight);
            
            if (IsFootPenetrating(foot, out var penetrationHitInfo))
            {
                PlaceFoot(foot, penetrationHitInfo);
                RotateFoot(foot, penetrationHitInfo);
            }
            else
            {
                PlaceFoot(foot, flotationHitInfo);
                RotateFoot(foot, flotationHitInfo);
            }
        }

        /// <summary>
        /// 足が地面にめり込んでいるかどうか。
        /// </summary>
        /// <param name="foot">足の IKGoal</param>
        /// <param name="hitInfo">足の光線衝突情報</param>
        /// <returns>true: めり込んでいる, false: めり込んでいない</returns>
        private bool IsFootPenetrating(AvatarIKGoal foot, out RaycastHit hitInfo)
        {
            var ray = new Ray(animator.GetIKPosition(foot) + Vector3.up * footRayOffset, Vector3.down);
            
            if (!Physics.Raycast(ray, out hitInfo, distanceFootToGround + footRayOffset, ~LayerMasks.Player)) return false;
            if (hitInfo.transform.gameObject.layer != Layers.Walkable) return false;

            return true;
        }
        
        /// <summary>
        /// つま先が地面にめり込んでいるかどうか。
        /// </summary>
        /// <param name="foot">足の IKGoal</param>
        /// <param name="hitInfo">足の光線衝突情報</param>
        /// <returns>true: めり込んでいる, false: めり込んでいない</returns>
        private bool IsToePenetrating(AvatarIKGoal foot, out RaycastHit hitInfo)
        {
            var toePosition = animator.GetIKPosition(foot) + animator.GetIKRotation(foot) * Vector3.forward * _distanceFootToToe;
            var ray = new Ray(toePosition + Vector3.up * toeRayOffset, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            
            if (!Physics.Raycast(ray, out hitInfo, toeRayOffset, ~LayerMasks.Player)) return false;
            if (hitInfo.transform.gameObject.layer != Layers.Walkable) return false;

            return true;
        }

        /// <summary>
        /// 足が空中に浮いているかどうか。
        /// </summary>
        /// <param name="foot">足の IKGoal</param>
        /// <param name="hitInfo">足の光線衝突情報</param>
        /// <returns>true: 浮いている, false: 浮いていない</returns>
        private bool IsFootFloating(AvatarIKGoal foot, out RaycastHit hitInfo)
        {
            var ray = new Ray(animator.GetIKPosition(foot), Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            
            if (Physics.Raycast(ray, out hitInfo, maxRootBoneDownDistance, ~LayerMasks.Player))
            {
                if (hitInfo.transform.gameObject.layer == Layers.Walkable) return false;
            }
            
            return true;
        }

        /// <summary>
        /// 胴体を下げる。
        /// </summary>
        private void DownBody()
        {
            // 両足ともめり込んでいる場合は無視
            var isLeftFootPenetrating = IsFootPenetrating(AvatarIKGoal.LeftFoot, out var leftFootPenetrationHitInfo);
            var isRightFootPenetrating = IsFootPenetrating(AvatarIKGoal.RightFoot, out var rightFootPenetrationHitInfo);
            if (isLeftFootPenetrating && isRightFootPenetrating) return;
            
            // 両足ともレイキャストの範囲外の場合は無視
            var isLeftFootFloating = IsFootFloating(AvatarIKGoal.LeftFoot, out _leftFootFlotationHitInfo);
            var isRightFootFloating = IsFootFloating(AvatarIKGoal.RightFoot, out _rightFootFlotationHitInfo);
            if (isLeftFootFloating && isRightFootFloating) return;
            
            var distanceLeftFootToGround = Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.LeftFoot), _leftFootFlotationHitInfo.point);
            var distanceRightFootToGround = Vector3.Distance(animator.GetIKPosition(AvatarIKGoal.RightFoot), _rightFootFlotationHitInfo.point);
            var rootDownDistance = distanceLeftFootToGround >= distanceRightFootToGround ? distanceLeftFootToGround : distanceRightFootToGround;
            
            _currentBodyDownOffset = Mathf.SmoothDamp(_currentBodyDownOffset, rootDownDistance * bodyDownMagnitude, ref _bodyDownOffsetVelocity, bodyDampTime);
            var position = animator.bodyPosition;
            position.y -= _currentBodyDownOffset;
            animator.bodyPosition = position;
        }

        private void PlaceFoot(AvatarIKGoal foot, RaycastHit footHitInfo)
        {
            if (foot == AvatarIKGoal.LeftFoot)
            {
                var target = footHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
                _currentLeftFootPosition = Vector3.SmoothDamp(_currentLeftFootPosition, target, ref _leftFootVelocity, footDampTime);
                animator.SetIKPosition(foot, _currentLeftFootPosition);
            }
            else
            {
                var target = footHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
                _currentRightFootPosition = Vector3.SmoothDamp(_currentRightFootPosition, target, ref _rightFootVelocity, footDampTime);
                animator.SetIKPosition(foot, _currentRightFootPosition);
            }
        }

        /// <summary>
        /// 足を回転させる。
        /// </summary>
        /// <param name="foot">足の IKGoal</param>
        /// <param name="footHitInfo">足の光線衝突情報</param>
        private void RotateFoot(AvatarIKGoal foot, RaycastHit footHitInfo)
        {
            // つま先が埋まった場合は Foot -> Toe のベクトルを進行方向とする
            // そうでない場合は Foot の進行方向を維持するために地面に投影する
            var forward = IsToePenetrating(foot, out var toeHitInfo) ? 
                (toeHitInfo.point - footHitInfo.point + new Vector3(0f, distanceFootToGround, 0f)).normalized : 
                Vector3.ProjectOnPlane(animator.GetIKRotation(foot) * Vector3.forward, footHitInfo.normal);

            if (forward.sqrMagnitude <= Mathf.Epsilon) return;

            if (foot == AvatarIKGoal.LeftFoot)
            {
                _currentLeftFootRotation = Quaternion.Slerp(_currentLeftFootRotation, Quaternion.LookRotation(forward, footHitInfo.normal), rotateSpeed * Time.deltaTime);
                animator.SetIKRotation(foot, _currentLeftFootRotation);
            }
            else
            {
                _currentRightFootRotation = Quaternion.Slerp(_currentRightFootRotation, Quaternion.LookRotation(forward, footHitInfo.normal), rotateSpeed * Time.deltaTime);
                animator.SetIKRotation(foot, _currentRightFootRotation);
            }
        }
    }
}