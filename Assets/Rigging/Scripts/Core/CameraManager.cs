using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        /// <summary>
        /// null チェック推奨
        /// </summary>
        public CinemachineBrain ActiveBrain => CinemachineBrain.ActiveBrainCount > 0 ? CinemachineBrain.GetActiveBrain(CinemachineBrain.ActiveBrainCount - 1) : null;
        
        /// <summary>
        /// null チェック推奨
        /// </summary>
        public Camera OutputCamera => ActiveBrain?.OutputCamera;

        /// <summary>
        /// null チェック推奨
        /// </summary>
        public CinemachineCamera ActiveCinemachine => ActiveBrain?.ActiveVirtualCamera as CinemachineCamera;
        
        protected override void OnDispose()
        {

        }
    }
}