using System.Collections.Generic;
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
        
        private readonly List<CinemachineCamera> _cinemachines = new();
        
        public void Push(CinemachineCamera cinemachine)
        {
            if (cinemachine == null) return;
            _cinemachines.Remove(cinemachine);
            _cinemachines.Add(cinemachine);
            ApplyPriority();
        }

        public void Remove(CinemachineCamera cinemachine)
        {
            if (cinemachine == null) return;
            if (!_cinemachines.Remove(cinemachine)) return;
            cinemachine.Priority = 0;
            ApplyPriority();
        }

        private void ApplyPriority()
        {
            for (var i = 0; i < _cinemachines.Count; i++)
            {
                _cinemachines[i].Priority = i + 1;
            }
        }
        
        protected override void OnDispose()
        {

        }
    }
}