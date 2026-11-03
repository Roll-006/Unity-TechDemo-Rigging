using Rigging.Core;
using Rigging.Demo.Reticle;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public static class ShootDirectionCalculator
    {
        /// <summary>
        /// 撃つ方向を計算する。
        /// </summary>
        /// <param name="reticleModel">レティクルモデル</param>
        /// <param name="aimTargetPosition">狙っている対象の座標</param>
        /// <param name="rootRectTransform">UI のルート</param>
        /// <param name="weaponRay">武器から出る光線</param>
        /// <returns>撃つ方向</returns>
        public static Vector3 CalcShootDirection(ReticleModel reticleModel, Vector3 aimTargetPosition, RectTransform rootRectTransform, Ray weaponRay)
        {
            var targetPosition = reticleModel.ReticleShapeType switch
            {
                ReticleShapeType.Square => CalcSquareReticleTargetPosition(reticleModel, aimTargetPosition, rootRectTransform),
                ReticleShapeType.Circle => CalcCircleReticleTargetPosition(reticleModel, aimTargetPosition, rootRectTransform),
                _ => aimTargetPosition
            };
            
            return (targetPosition - weaponRay.origin).normalized;
        }
        
        private static Vector3 CalcSquareReticleTargetPosition(ReticleModel reticleModel, Vector3 aimTargetPosition, RectTransform rootRectTransform)
        {
            var halfSize = reticleModel.CurrentSize.CurrentValue * 0.5f;
            var range = new Vector2(
                Random.Range(-halfSize, halfSize),
                Random.Range(-halfSize, halfSize));
            
            return ConvertRangeToWorld(aimTargetPosition, range, rootRectTransform);
        }
        
        private static Vector3 CalcCircleReticleTargetPosition(ReticleModel reticleModel, Vector3 aimTargetPosition, RectTransform rootRectTransform)
        {
            var range = Random.insideUnitCircle * reticleModel.CurrentSize.CurrentValue * 0.5f;
            return ConvertRangeToWorld(aimTargetPosition, range, rootRectTransform);
        }
        
        private static Vector3 ConvertRangeToWorld(Vector3 aimTargetPosition, Vector2 range, RectTransform rootRectTransform)
        {
            var outputCamera = CameraManager.I.OutputCamera;
            if (outputCamera == null) return aimTargetPosition;

            // スクリーン座標に変換
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rootRectTransform, outputCamera.WorldToScreenPoint(aimTargetPosition), null, out var screenCenter);
            var worldRange = rootRectTransform.TransformVector(new(range.x, range.y, 0f));
            var screenPosition = screenCenter + worldRange;

            // ターゲットに生成された平面と光線が衝突した位置に発射する
            var ray = outputCamera.ScreenPointToRay(screenPosition);
            var plane = new Plane(-outputCamera.transform.forward, aimTargetPosition);
            return plane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : aimTargetPosition;
        }
    }
}