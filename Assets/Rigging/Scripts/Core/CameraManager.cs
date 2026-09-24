using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        public Camera OutputCamera => CinemachineBrain.GetActiveBrain(CinemachineBrain.ActiveBrainCount - 1).OutputCamera;

        override protected void OnDispose()
        {

        }
    }
}
