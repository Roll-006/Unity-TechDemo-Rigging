using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Rigging.Demo.Characters
{
    /// <summary>
    /// 人型モデルの足首をIKにより地面に設置させる。
    /// </summary>
    public class IKHumanoidFootPlacement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private LandingDetector landingDetector;
        [SerializeField, Range(0f, 1f)] private float distanceFootToGround;
        [SerializeField, Min(0f)] private float maxBodyDownDistance;
        [SerializeField, Min(0f)] private float bodySmoothTime;
        [SerializeField, Min(0f)] private float footSmoothTime;
        [SerializeField, Min(0f)] private float rotateSpeed;
        
        [SerializeField, Min(0f), Tooltip("足首が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float footRayOffset;
        
        [SerializeField, Min(0f), Tooltip("つま先が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float toeRayOffset;

        private float _currentBodyDownOffset;
        private float _bodyDownVelocity;
        private Vector3 _currentLeftFootPosition;
        private Vector3 _leftFootVelocity;
        private Vector3 _currentRightFootPosition;
        private Vector3 _rightFootVelocity;
        private Quaternion _currentLeftFootRotation;
        private Quaternion _currentRightFootRotation;
        private RaycastHit _leftFootFlotationHitInfo;
        private RaycastHit _rightFootFlotationHitInfo;
        private float _leftLegLength;
        private float _rightLegLength;
        private float _distanceLeftFootToLeftToe;
        private float _distanceRightFootToRightToe;
        private bool _isLeftFootFloating;
        private bool _isRightFootFloating;

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(landingDetector, this);
        }

        private void Start()
        {
            _leftLegLength = GetLegLength(HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot);
            _rightLegLength = GetLegLength(HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot);
            _distanceLeftFootToLeftToe = GetDistanceFootToToe(HumanBodyBones.LeftFoot, HumanBodyBones.LeftToes);
            _distanceRightFootToRightToe = GetDistanceFootToToe(HumanBodyBones.RightFoot, HumanBodyBones.RightToes);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!landingDetector.IsLanding) return;
            
            DownBody();
            FootIK(AvatarIKGoal.LeftFoot);
            FootIK(AvatarIKGoal.RightFoot);
        }

        /// <summary>
        /// 脚の長さを取得する。
        /// </summary>
        /// <param name="upperLeg">太もものボーン</param>
        /// <param name="lowerLeg">ひざのボーン</param>
        /// <param name="foot">足首のボーン</param>
        /// <returns>脚の長さ</returns>
        private float GetLegLength(HumanBodyBones upperLeg, HumanBodyBones lowerLeg, HumanBodyBones foot)
        {
            var upperLegPosition = animator.GetBoneTransform(upperLeg).position;
            var lowerLegPosition = animator.GetBoneTransform(lowerLeg).position;
            var footPosition = animator.GetBoneTransform(foot).position;
            return Vector3.Distance(upperLegPosition, lowerLegPosition) + Vector3.Distance(lowerLegPosition, footPosition);
        }

        /// <summary>
        /// 足首からつま先の距離を取得する。
        /// </summary>
        /// <param name="foot">足首のボーン</param>
        /// <param name="toes">つま先のボーン</param>
        /// <returns>足首からつま先の距離</returns>
        private float GetDistanceFootToToe(HumanBodyBones foot, HumanBodyBones toes)
        {
            return Vector3.Distance(animator.GetBoneTransform(foot).position, animator.GetBoneTransform(toes).position);
        }
        
        /// <summary>
        /// FootIK を行う。
        /// </summary>
        /// <param name="foot">足首の IKGoal</param>
        private void FootIK(AvatarIKGoal foot)
        {
            if (foot != AvatarIKGoal.LeftFoot && foot != AvatarIKGoal.RightFoot) return;

            var isLeftFoot = foot == AvatarIKGoal.LeftFoot;
            var weight = animator.GetFloat(isLeftFoot ? AnimatorParameters.IKLeftFootWeight : AnimatorParameters.IKRightFootWeight);
            var flotationHitInfo = isLeftFoot ? _leftFootFlotationHitInfo : _rightFootFlotationHitInfo;
            var isFloating = isLeftFoot ? _isLeftFootFloating : _isRightFootFloating;
            
            animator.SetIKPositionWeight(foot, weight);
            animator.SetIKRotationWeight(foot, weight);
            
            if (IsFootPenetrating(foot, out var penetrationHitInfo))
            {
                PlaceFoot(foot, penetrationHitInfo);
                RotateFoot(foot, penetrationHitInfo);
            }
            else if (!isFloating)
            {
                PlaceFoot(foot, flotationHitInfo);
                RotateFoot(foot, flotationHitInfo);
            }
        }

        /// <summary>
        /// 足首が地面にめり込んでいるかどうか。
        /// </summary>
        /// <param name="foot">足首の IKGoal</param>
        /// <param name="hitInfo">足首の光線衝突情報</param>
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
        /// <param name="foot">足首の IKGoal</param>
        /// <param name="hitInfo">足首の光線衝突情報</param>
        /// <returns>true: めり込んでいる, false: めり込んでいない</returns>
        private bool IsToePenetrating(AvatarIKGoal foot, out RaycastHit hitInfo)
        {
            var distanceFootToToe = foot == AvatarIKGoal.LeftFoot ? _distanceLeftFootToLeftToe : _distanceRightFootToRightToe;
            var toePosition = animator.GetIKPosition(foot) + animator.GetIKRotation(foot) * Vector3.forward * distanceFootToToe;
            var ray = new Ray(toePosition + Vector3.up * toeRayOffset, Vector3.down);
            
            if (!Physics.Raycast(ray, out hitInfo, toeRayOffset, ~LayerMasks.Player)) return false;
            if (hitInfo.transform.gameObject.layer != Layers.Walkable) return false;

            return true;
        }

        /// <summary>
        /// 足首が空中に浮いているかどうか。
        /// </summary>
        /// <param name="foot">足首の IKGoal</param>
        /// <param name="hitInfo">足首の光線衝突情報</param>
        /// <returns>true: 浮いている, false: 浮いていない</returns>
        private bool IsFootFloating(AvatarIKGoal foot, out RaycastHit hitInfo)
        {
            var ray = new Ray(animator.GetIKPosition(foot), Vector3.down);
            
            if (Physics.Raycast(ray, out hitInfo, maxBodyDownDistance, ~LayerMasks.Player))
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
            _isLeftFootFloating = IsFootFloating(AvatarIKGoal.LeftFoot, out _leftFootFlotationHitInfo);
            _isRightFootFloating = IsFootFloating(AvatarIKGoal.RightFoot, out _rightFootFlotationHitInfo);
            var isLeftFootPenetrating = IsFootPenetrating(AvatarIKGoal.LeftFoot, out var leftPenetrationHitInfo);
            var isRightFootPenetrating = IsFootPenetrating(AvatarIKGoal.RightFoot, out var rightPenetrationHitInfo);

            var leftDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.LeftUpperLeg, _leftLegLength, isLeftFootPenetrating, leftPenetrationHitInfo, _isLeftFootFloating, _leftFootFlotationHitInfo);
            var rightDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.RightUpperLeg, _rightLegLength, isRightFootPenetrating, rightPenetrationHitInfo, _isRightFootFloating, _rightFootFlotationHitInfo);
            
            // 両足から下げる距離が長い方を選択する (最大距離あり)
            var downDistance = Mathf.Min(Mathf.Max(leftDownDistance, rightDownDistance), maxBodyDownDistance);
            _currentBodyDownOffset = Mathf.SmoothDamp(_currentBodyDownOffset, downDistance, ref _bodyDownVelocity, bodySmoothTime);

            var position = animator.bodyPosition;
            position.y -= _currentBodyDownOffset;
            animator.bodyPosition = position;
        }

        /// <summary>
        /// 胴体を下げるために必要な距離を取得する。
        /// </summary>
        /// <param name="upperLeg">太もものボーン</param>
        /// <param name="legLength">脚の長さ</param>
        /// <param name="isPenetrating">足首が地面にめり込んでいるかどうか</param>
        /// <param name="penetrationHitInfo">足首の光線とめり込んだ地面との衝突情報</param>
        /// <param name="isFloating">足首が空中に浮いているかどうか</param>
        /// <param name="flotationHitInfo">足首の光線と足首下の地面との衝突情報</param>
        /// <returns>胴体を下げる距離</returns>
        private float GetRequiredBodyDownDistance(HumanBodyBones upperLeg, float legLength, bool isPenetrating, in RaycastHit penetrationHitInfo, bool isFloating, in RaycastHit flotationHitInfo)
        {
            // 足を設置する位置を取得
            Vector3 target;
            if (isPenetrating) target = penetrationHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
            else if (!isFloating) target = flotationHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
            else return 0f;

            var targetToUpperLeg = animator.GetBoneTransform(upperLeg).position - target;

            // 脚が届く場合は胴体を下げない
            var legSqrLength = legLength * legLength;
            var excess = targetToUpperLeg.sqrMagnitude - legSqrLength;
            if (excess <= Mathf.Epsilon) return 0f;
            
            // 脚の長さと等しくなるような距離を計算する
            // [ targetToUpperLeg.x² + (targetToUpperLeg.y − downDistance)² + targetToUpperLeg.z² = legLength² ]
            // [ downDistance = targetToUpperLeg.y - √(legLength² − (targetToUpperLeg.x² + targetToUpperLeg.z²)) ]
            
            // 脚が縦に伸ばせる長さの二乗を取得
            var verticalReachSqr = legSqrLength - (targetToUpperLeg.x * targetToUpperLeg.x + targetToUpperLeg.z * targetToUpperLeg.z);
            if (verticalReachSqr <= Mathf.Epsilon) return Mathf.Max(0f, targetToUpperLeg.y);

            return Mathf.Max(0f, targetToUpperLeg.y - Mathf.Sqrt(verticalReachSqr));
        }

        /// <summary>
        /// 足首を設置する。
        /// </summary>
        /// <param name="foot">足首の IKGoal</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
        private void PlaceFoot(AvatarIKGoal foot, RaycastHit footHitInfo)
        {
            if (foot == AvatarIKGoal.LeftFoot)
            {
                var target = footHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
                _currentLeftFootPosition = Vector3.SmoothDamp(_currentLeftFootPosition, target, ref _leftFootVelocity, footSmoothTime);
                animator.SetIKPosition(foot, _currentLeftFootPosition);
            }
            else
            {
                var target = footHitInfo.point + new Vector3(0f, distanceFootToGround, 0f);
                _currentRightFootPosition = Vector3.SmoothDamp(_currentRightFootPosition, target, ref _rightFootVelocity, footSmoothTime);
                animator.SetIKPosition(foot, _currentRightFootPosition);
            }
        }

        /// <summary>
        /// 足首を回転させる。
        /// </summary>
        /// <param name="foot">足首の IKGoal</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
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