using UnityEngine;

namespace Rigging.Common
{
    public static class LayerWeightUpdater
    {
        public static void UpdateLayerWeight(Animator animator, int layerIndex, bool isActiveLayer, float towardSpeed)
        {
            var currentLayerWeight = animator.GetLayerWeight(layerIndex);
            var targetLayerWeight = isActiveLayer ? 1f : 0f;
            var resultLayerWeight = Mathf.MoveTowards(currentLayerWeight, targetLayerWeight, towardSpeed * Time.deltaTime);

            animator.SetLayerWeight(layerIndex, resultLayerWeight);
        }
    }
}
