using System.Collections.Generic;
using Rigging.Core;
using Unity.Cinemachine;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class StandCinemachineEnabler : MonoBehaviour
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
                if (!otherCinemachine.enabled) continue;
                
                cinemachine.enabled = false;
                return;
            }

            cinemachine.enabled = true;
        }
    }
}