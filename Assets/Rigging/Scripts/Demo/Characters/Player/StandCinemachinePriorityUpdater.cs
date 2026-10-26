using System.Collections.Generic;
using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class StandCinemachinePriorityUpdater : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField] private List<CinemachineCamera> otherCinemachines;

        private void Awake()
        {
            Debugger.ValidateSerializeField(cinemachine, this);
        }

        private void Update()
        {
            foreach (var otherCinemachine in otherCinemachines)
            {
                if (!CinemachineCore.IsLiveInBlend(otherCinemachine)) continue;
                
                cinemachine.Priority = 0;
                return;
            }

            cinemachine.Priority = 1;
        }
    }
}