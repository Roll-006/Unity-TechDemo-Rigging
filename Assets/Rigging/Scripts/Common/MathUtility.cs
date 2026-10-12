using UnityEngine;

namespace Rigging.Common
{
    public static class MathUtility
    {
        private const float KSqrt2Half = 0.70710678118f;
        
        /// <summary>
        /// 直角二等辺三角形の隣辺の1つの長さを取得する。
        /// </summary>
        /// <param name="hypotenuse">斜辺</param>
        /// <returns>隣辺の1つの長さ</returns>
        public static float GetIsoscelesRightTriangleLegLength(float hypotenuse)
        {
            // 直角二等辺三角形の各辺の比率より (1 : 1 : √2)
            return hypotenuse * KSqrt2Half;
        }
        
        /// <summary>
        /// 二次ベジェ曲線
        /// </summary>
        /// <param name="a">制御点1</param>
        /// <param name="b">制御点2</param>
        /// <param name="c">制御点3</param>
        /// <param name="t">補間係数(0～1)</param>
        /// <returns>補間後の値</returns>
        public static float QuadraticBezierCurve(float a, float b, float c, float t)
        {
            // 2次ベジェ曲線の公式より (P = (1 - t)^2 * A + 2(1 - t)t * B + t^2 * C)
            var u = 1f - t;
            return u * u * a + 2f * u * t * b + t * t * c;
        }

        /// <summary>
        /// 二次ベジェ曲線
        /// </summary>
        /// <param name="a">制御点1</param>
        /// <param name="b">制御点2</param>
        /// <param name="c">制御点3</param>
        /// <param name="t">補間係数(0～1)</param>
        /// <returns>補間後の値</returns>
        public static Vector2 QuadraticBezierCurve(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            // 2次ベジェ曲線の公式より (P = (1 - t)^2 * A + 2(1 - t)t * B + t^2 * C)
            var u = 1f - t;
            return u * u * a + 2f * u * t * b + t * t * c;
        }

        /// <summary>
        /// 二次ベジェ曲線
        /// </summary>
        /// <param name="a">制御点1</param>
        /// <param name="b">制御点2</param>
        /// <param name="c">制御点3</param>
        /// <param name="t">補間係数(0～1)</param>
        /// <returns>補間後の値</returns>
        public static Vector3 QuadraticBezierCurve(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            // 2次ベジェ曲線の公式より (P = (1 - t)^2 * A + 2(1 - t)t * B + t^2 * C)
            var u = 1f - t;
            return u * u * a + 2f * u * t * b + t * t * c;
        }
    }
}