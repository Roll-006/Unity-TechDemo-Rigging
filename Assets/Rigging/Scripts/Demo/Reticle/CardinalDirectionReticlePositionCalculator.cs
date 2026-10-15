using Rigging.Common;
using UnityEngine;

namespace Rigging.Demo.Reticle
{
    public static class ReticlePositionCalculator
    {
        public static void CalcCardinalDirectionPosition(RectTransform transform, CardinalDirection direction, float size)
        {
            transform.sizeDelta = new Vector2(size, size);
            
            if (direction.Left != null) direction.Left.anchoredPosition = new Vector2(-direction.Left.sizeDelta.x / 2f, 0f);
            if (direction.Right != null) direction.Right.anchoredPosition = new Vector2(direction.Right.sizeDelta.x / 2f, 0f);
            if (direction.Top != null) direction.Top.anchoredPosition = new Vector2(0f, direction.Top.sizeDelta.y / 2f);
            if (direction.Bottom != null) direction.Bottom.anchoredPosition = new Vector2(0f, -direction.Bottom.sizeDelta.y / 2f);
        }
        
        public static void CalcDiagonalDirectionPosition(RectTransform transform, DiagonalDirection direction, float size)
        {
            transform.sizeDelta = new Vector2(size, size);
            
            if (direction.TopLeft != null)
                direction.TopLeft.anchoredPosition = new Vector2(-CalcPosition(direction.TopLeft.sizeDelta.x), CalcPosition(direction.TopLeft.sizeDelta.x));
            
            if (direction.TopRight != null)
                direction.TopRight.anchoredPosition = new Vector2(CalcPosition(direction.TopRight.sizeDelta.x), CalcPosition(direction.TopRight.sizeDelta.x));
            
            if (direction.BottomLeft != null)
                direction.BottomLeft.anchoredPosition = new Vector2(-CalcPosition(direction.BottomLeft.sizeDelta.x), -CalcPosition(direction.BottomLeft.sizeDelta.x));
            
            if (direction.BottomRight != null)
                direction.BottomRight.anchoredPosition = new Vector2(CalcPosition(direction.BottomRight.sizeDelta.x), -CalcPosition(direction.BottomRight.sizeDelta.x));
        }
        
        private static float CalcPosition(float width)
        {
            return MathUtility.GetIsoscelesRightTriangleLegLength(width) / 2f;
        }
    }
}