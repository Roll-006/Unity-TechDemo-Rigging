using UnityEngine;

namespace Rigging.Common
{
    public class CameraPivotShareAxis : MonoBehaviour
    {
        public Vector2 Axis { get; private set; }

        public void MovePivot(Vector2 input)
        {
            Axis += input;
        }
        
        public void ApplyLimit(CameraPivotLimitData limitData)
        {
            var axis = Axis;
            
            axis.x = limitData.IsWrapYaw ? MathUtility.Wrap(Axis.x, limitData.MinYaw, limitData.MaxYaw) : 
                Mathf.Clamp(Axis.x, limitData.MinYaw, limitData.MaxYaw);
            
            axis.y = limitData.IsWrapPitch ? MathUtility.Wrap(Axis.y, limitData.MinPitch, limitData.MaxPitch) : 
                Mathf.Clamp(Axis.y, limitData.MinPitch, limitData.MaxPitch);
            
            Axis = axis;
        }
    }
}