using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Rigging.Demo.Characters
{
    /// <summary>
    /// IKの片足情報
    /// </summary>
    public struct IKFootData
    {
        public AvatarIKGoal IKGoal;
        public Quaternion CurrentFootRotation;
        public RaycastHit FootFlotationHitInfo;
        public RaycastHit FootPenetrationHitInfo;
        public float LegLength;
        public float DistanceFootToToe;
        public bool IsFootFloating;
        public bool IsFootPenetrating;
        public float Weight;
    }
    
    /// <summary>
    /// 人型モデルの足首をIKにより地面に設置させる。
    /// </summary>
    public class IKHumanoidFootPlacement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private LandingDetector landingDetector;
        [SerializeField, Range(0f, 1f)] private float distanceFootToGround;
        [SerializeField, Min(0f)] private float maxBodyDownDistance;
        [SerializeField, Min(0f)] private float maxFootUpDistance;
        [SerializeField, Min(0f)] private float bodySmoothTime;
        [SerializeField, Min(0f)] private float rotateSpeed;
        
        [SerializeField, Min(0f), Tooltip("足首が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float footRayOffset;
        
        [SerializeField, Min(0f), Tooltip("つま先が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float toeRayOffset;
        
        private IKFootData _leftFootData;
        private IKFootData _rightFootData;
        private float _currentBodySmoothTime;
        private float _currentBodyDownOffset;
        private float _bodyDownVelocity;
        private Vector3 _distanceFootToGroundVector;

        private void Awake()
        {
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(landingDetector, this);

            _distanceFootToGroundVector = new Vector3(0f, distanceFootToGround, 0f);
        }

        private void Start()
        {
            InitializeIKFootData(AvatarIKGoal.LeftFoot, ref _leftFootData);
            InitializeIKFootData(AvatarIKGoal.RightFoot, ref _rightFootData);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!landingDetector.IsLanding)
            {
                _currentBodySmoothTime = 0f;
                return;
            }
            
            DownBody();
            FootIK(ref _leftFootData);
            FootIK(ref _rightFootData);

            _currentBodySmoothTime = bodySmoothTime;
        }

        private void InitializeIKFootData(AvatarIKGoal foot, ref IKFootData ikFootData)
        {
            ikFootData.IKGoal = foot;
            
            var boneOffset = ikFootData.IKGoal == AvatarIKGoal.LeftFoot ? 0 : 1;
            
            ikFootData.LegLength = GetLegLength(HumanBodyBones.LeftUpperLeg + boneOffset, HumanBodyBones.LeftLowerLeg + boneOffset, HumanBodyBones.LeftFoot + boneOffset);
            ikFootData.DistanceFootToToe = GetDistanceFootToToe(HumanBodyBones.LeftFoot + boneOffset, HumanBodyBones.LeftToes + boneOffset);
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
        /// <param name="ikFootData">IK の片足情報</param>
        private void FootIK(ref IKFootData ikFootData)
        {
            animator.SetIKPositionWeight(ikFootData.IKGoal, ikFootData.Weight);
            animator.SetIKRotationWeight(ikFootData.IKGoal, ikFootData.Weight);
            
            if (IsFootPenetrating(ikFootData.IKGoal, out var penetrationHitInfo))
            {
                PlaceFoot(ref ikFootData, penetrationHitInfo);
                RotateFoot(ref ikFootData, penetrationHitInfo);
            }
            else if (!ikFootData.IsFootFloating)
            {
                PlaceFoot(ref ikFootData, ikFootData.FootFlotationHitInfo);
                RotateFoot(ref ikFootData, ikFootData.FootFlotationHitInfo);
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
        /// <param name="ikFootData">IK の片足情報</param>
        /// <param name="hitInfo">足首の光線衝突情報</param>
        /// <returns>true: めり込んでいる, false: めり込んでいない</returns>
        private bool IsToePenetrating(IKFootData ikFootData, out RaycastHit hitInfo)
        {
            var toePosition = animator.GetIKPosition(ikFootData.IKGoal) + animator.GetIKRotation(ikFootData.IKGoal) * Vector3.forward * ikFootData.DistanceFootToToe;
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
            _leftFootData.IsFootFloating = IsFootFloating(AvatarIKGoal.LeftFoot, out _leftFootData.FootFlotationHitInfo);
            _rightFootData.IsFootFloating = IsFootFloating(AvatarIKGoal.RightFoot, out _rightFootData.FootFlotationHitInfo);
            _leftFootData.IsFootPenetrating = IsFootPenetrating(AvatarIKGoal.LeftFoot, out _leftFootData.FootPenetrationHitInfo);
            _rightFootData.IsFootPenetrating = IsFootPenetrating(AvatarIKGoal.RightFoot, out _rightFootData.FootPenetrationHitInfo);

            _leftFootData.Weight = animator.GetFloat(AnimatorParameters.IKLeftFootWeight);
            _rightFootData.Weight = animator.GetFloat(AnimatorParameters.IKRightFootWeight);

            var leftDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.LeftUpperLeg, _leftFootData);
            var rightDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.RightUpperLeg, _rightFootData);
            
            // 両足から下げる距離が長い方を選択する (最大距離あり)
            var downDistance = Mathf.Min(Mathf.Max(leftDownDistance, rightDownDistance), maxBodyDownDistance);
            _currentBodyDownOffset = Mathf.SmoothDamp(_currentBodyDownOffset, downDistance, ref _bodyDownVelocity, _currentBodySmoothTime);

            var position = animator.bodyPosition;
            position.y -= _currentBodyDownOffset;
            animator.bodyPosition = position;
        }

        /// <summary>
        /// 胴体を下げるために必要な距離を取得する。
        /// </summary>
        /// <param name="upperLeg">太もものボーン</param>
        /// <param name="ikFootData">IK の片足情報</param>
        /// <returns>胴体を下げる距離</returns>
        private float GetRequiredBodyDownDistance(HumanBodyBones upperLeg, IKFootData ikFootData)
        {
            if (ikFootData.Weight < 0.9f) return 0f;
            
            // 足を設置する位置を取得
            Vector3 target;
            if (ikFootData.IsFootPenetrating) target = ikFootData.FootPenetrationHitInfo.point + _distanceFootToGroundVector;
            else if (!ikFootData.IsFootFloating) target = ikFootData.FootFlotationHitInfo.point + _distanceFootToGroundVector;
            else return 0f;

            var targetToUpperLeg = animator.GetBoneTransform(upperLeg).position - target;

            // 脚が届く場合は胴体を下げない
            var legSqrLength = ikFootData.LegLength * ikFootData.LegLength;
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
        /// <param name="ikFootData">IK の片足情報</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
        private void PlaceFoot(ref IKFootData ikFootData, RaycastHit footHitInfo)
        {
            var target = footHitInfo.point + _distanceFootToGroundVector;
            var maxHeight = animator.GetIKPosition(ikFootData.IKGoal).y + maxFootUpDistance;
            if (target.y > maxHeight) target.y = maxHeight;

            animator.SetIKPosition(ikFootData.IKGoal, target);
        }

        /// <summary>
        /// 足首を回転させる。
        /// </summary>
        /// <param name="ikFootData">IK の片足情報</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
        private void RotateFoot(ref IKFootData ikFootData, RaycastHit footHitInfo)
        {
            // つま先が埋まった場合は Foot -> Toe のベクトルを進行方向とする
            // そうでない場合は Foot の進行方向を維持するために地面に投影する
            var forward = IsToePenetrating(ikFootData, out var toeHitInfo) ? 
                (toeHitInfo.point - footHitInfo.point + _distanceFootToGroundVector).normalized : 
                Vector3.ProjectOnPlane(animator.GetIKRotation(ikFootData.IKGoal) * Vector3.forward, footHitInfo.normal);

            if (forward.sqrMagnitude <= Mathf.Epsilon) return;
            
            ikFootData.CurrentFootRotation = Quaternion.Slerp(ikFootData.CurrentFootRotation, Quaternion.LookRotation(forward, footHitInfo.normal), rotateSpeed * Time.deltaTime);
            animator.SetIKRotation(ikFootData.IKGoal, ikFootData.CurrentFootRotation);
        }
    }
}