using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Rigging.Demo.Characters
{
    /// <summary>
    /// IK の片足情報
    /// </summary>
    public struct IKOneFootData
    {
        public AvatarIKGoal IKGoal;
        public Vector3 CurrentFootPosition;
        public Quaternion CurrentFootRotation;
        public Vector3 FootVelocity;
        public RaycastHit FootFlotationHitInfo;
        public RaycastHit FootPenetrationHitInfo;
        public float LegLength;
        public float DistanceFootToToe;
        public bool IsFootFloating;
        public bool IsFootPenetrating;
        public float Weight;
    }
    
    /// <summary>
    /// 人型モデルの足首を IK により地面に設置させる。
    /// </summary>
    public class IKHumanoidFootPlacement : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private LandingDetector landingDetector;
        [SerializeField, Range(0f, 1f)] private float distanceFootToGround;
        [SerializeField, Min(0f)] private float maxBodyDownDistance;
        [SerializeField, Range(0f, 1f)] private float downBodyWeightThreshold;
        [SerializeField, Min(0f)] private float bodySmoothTime;
        [SerializeField, Min(0f)] private float footSmoothTime;
        [SerializeField, Min(0f)] private float rotateSpeed;
        
        [SerializeField, Min(0f), Tooltip("足首が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float footRayOffset;
        
        [SerializeField, Min(0f), Tooltip("つま先が埋まっても Ray を飛ばすために始点の高さをずらす値")]
        private float toeRayOffset;
        
        public float CurrentBodyDownOffset { get; private set; }
        
        private IKOneFootData _leftOneFootData;
        private IKOneFootData _rightOneFootData;
        private float _currentBodySmoothTime;
        private float _currentFootSmoothTime;
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
            InitializeIKFootData(AvatarIKGoal.LeftFoot, ref _leftOneFootData);
            InitializeIKFootData(AvatarIKGoal.RightFoot, ref _rightOneFootData);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!landingDetector.IsLanding)
            {
                _currentBodySmoothTime = 0f;
                _currentFootSmoothTime = 0f;
                return;
            }
            
            DownBody();
            FootIK(ref _leftOneFootData);
            FootIK(ref _rightOneFootData);

            _currentBodySmoothTime = bodySmoothTime;
            _currentFootSmoothTime = footSmoothTime;
        }

        private void InitializeIKFootData(AvatarIKGoal foot, ref IKOneFootData ikOneFootData)
        {
            ikOneFootData.IKGoal = foot;
            
            var boneOffset = ikOneFootData.IKGoal == AvatarIKGoal.LeftFoot ? 0 : 1;
            
            ikOneFootData.LegLength = GetLegLength(HumanBodyBones.LeftUpperLeg + boneOffset, HumanBodyBones.LeftLowerLeg + boneOffset, HumanBodyBones.LeftFoot + boneOffset);
            ikOneFootData.DistanceFootToToe = GetDistanceFootToToe(HumanBodyBones.LeftFoot + boneOffset, HumanBodyBones.LeftToes + boneOffset);
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
        /// <param name="ikOneFootData">IK の片足情報</param>
        private void FootIK(ref IKOneFootData ikOneFootData)
        {
            animator.SetIKPositionWeight(ikOneFootData.IKGoal, ikOneFootData.Weight);
            animator.SetIKRotationWeight(ikOneFootData.IKGoal, ikOneFootData.Weight);
            
            if (IsFootPenetrating(ikOneFootData.IKGoal, out var penetrationHitInfo))
            {
                PlaceFoot(ref ikOneFootData, penetrationHitInfo);
                RotateFoot(ref ikOneFootData, penetrationHitInfo);
            }
            else if (!ikOneFootData.IsFootFloating)
            {
                PlaceFoot(ref ikOneFootData, ikOneFootData.FootFlotationHitInfo);
                RotateFoot(ref ikOneFootData, ikOneFootData.FootFlotationHitInfo);
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
            
            if (!Physics.Raycast(ray, out hitInfo, distanceFootToGround + footRayOffset, layerMask)) return false;
            if (hitInfo.transform.gameObject.layer != Layers.Walkable) return false;

            return true;
        }
        
        /// <summary>
        /// つま先が地面にめり込んでいるかどうか。
        /// </summary>
        /// <param name="ikOneFootData">IK の片足情報</param>
        /// <param name="hitInfo">足首の光線衝突情報</param>
        /// <returns>true: めり込んでいる, false: めり込んでいない</returns>
        private bool IsToePenetrating(IKOneFootData ikOneFootData, out RaycastHit hitInfo)
        {
            var toePosition = animator.GetIKPosition(ikOneFootData.IKGoal) + animator.GetIKRotation(ikOneFootData.IKGoal) * Vector3.forward * ikOneFootData.DistanceFootToToe;
            var ray = new Ray(toePosition + Vector3.up * toeRayOffset, Vector3.down);
            
            if (!Physics.Raycast(ray, out hitInfo, toeRayOffset, layerMask)) return false;
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
            
            if (Physics.Raycast(ray, out hitInfo, maxBodyDownDistance, layerMask))
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
            _leftOneFootData.IsFootFloating = IsFootFloating(AvatarIKGoal.LeftFoot, out _leftOneFootData.FootFlotationHitInfo);
            _rightOneFootData.IsFootFloating = IsFootFloating(AvatarIKGoal.RightFoot, out _rightOneFootData.FootFlotationHitInfo);
            _leftOneFootData.IsFootPenetrating = IsFootPenetrating(AvatarIKGoal.LeftFoot, out _leftOneFootData.FootPenetrationHitInfo);
            _rightOneFootData.IsFootPenetrating = IsFootPenetrating(AvatarIKGoal.RightFoot, out _rightOneFootData.FootPenetrationHitInfo);

            _leftOneFootData.Weight = animator.GetFloat(AnimatorParameters.IKLeftFootWeight);
            _rightOneFootData.Weight = animator.GetFloat(AnimatorParameters.IKRightFootWeight);

            var leftDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.LeftUpperLeg, _leftOneFootData);
            var rightDownDistance = GetRequiredBodyDownDistance(HumanBodyBones.RightUpperLeg, _rightOneFootData);
            
            // 両足から下げる距離が長い方を選択する (最大距離あり)
            var downDistance = Mathf.Min(Mathf.Max(leftDownDistance, rightDownDistance), maxBodyDownDistance);
            CurrentBodyDownOffset = Mathf.SmoothDamp(CurrentBodyDownOffset, downDistance, ref _bodyDownVelocity, _currentBodySmoothTime);

            var position = animator.bodyPosition;
            position.y -= CurrentBodyDownOffset;
            animator.bodyPosition = position;
        }

        /// <summary>
        /// 胴体を下げるために必要な距離を取得する。
        /// </summary>
        /// <param name="upperLeg">太もものボーン</param>
        /// <param name="ikOneFootData">IK の片足情報</param>
        /// <returns>胴体を下げる距離</returns>
        private float GetRequiredBodyDownDistance(HumanBodyBones upperLeg, IKOneFootData ikOneFootData)
        {
            if (ikOneFootData.Weight < downBodyWeightThreshold) return 0f;
            
            // 足を設置する位置を取得
            Vector3 target;
            if (ikOneFootData.IsFootPenetrating) target = ikOneFootData.FootPenetrationHitInfo.point + _distanceFootToGroundVector;
            else if (!ikOneFootData.IsFootFloating) target = ikOneFootData.FootFlotationHitInfo.point + _distanceFootToGroundVector;
            else return 0f;

            var targetToUpperLeg = animator.GetBoneTransform(upperLeg).position - target;

            // 脚が届く場合は胴体を下げない
            var legSqrLength = ikOneFootData.LegLength * ikOneFootData.LegLength;
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
        /// <param name="ikOneFootData">IK の片足情報</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
        private void PlaceFoot(ref IKOneFootData ikOneFootData, RaycastHit footHitInfo)
        {
            var target = footHitInfo.point + _distanceFootToGroundVector;
            ikOneFootData.CurrentFootPosition = Vector3.SmoothDamp(ikOneFootData.CurrentFootPosition, target, ref ikOneFootData.FootVelocity, _currentFootSmoothTime);
            animator.SetIKPosition(ikOneFootData.IKGoal, ikOneFootData.CurrentFootPosition);
        }

        /// <summary>
        /// 足首を回転させる。
        /// </summary>
        /// <param name="ikOneFootData">IK の片足情報</param>
        /// <param name="footHitInfo">足首の光線衝突情報</param>
        private void RotateFoot(ref IKOneFootData ikOneFootData, RaycastHit footHitInfo)
        {
            // つま先が埋まった場合は Foot -> Toe のベクトルを進行方向とする
            // そうでない場合は Foot の進行方向を維持するために地面に投影する
            var forward = IsToePenetrating(ikOneFootData, out var toeHitInfo) ? 
                (toeHitInfo.point - footHitInfo.point + _distanceFootToGroundVector).normalized : 
                Vector3.ProjectOnPlane(animator.GetIKRotation(ikOneFootData.IKGoal) * Vector3.forward, footHitInfo.normal);

            if (forward.sqrMagnitude <= Mathf.Epsilon) return;
            
            ikOneFootData.CurrentFootRotation = Quaternion.Slerp(ikOneFootData.CurrentFootRotation, Quaternion.LookRotation(forward, footHitInfo.normal), rotateSpeed * Time.deltaTime);
            animator.SetIKRotation(ikOneFootData.IKGoal, ikOneFootData.CurrentFootRotation);
        }
    }
}