using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Demo
{
    public class AimCinemachineEnabler : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachine;
        [SerializeField] private Shooter shooter;
        
        private void Awake()
        {
            Debugger.ValidateSerializeField(cinemachine, this);
            Debugger.ValidateSerializeField(shooter, this);
        }

        private void Update()
        {
            cinemachine.enabled = shooter.IsAiming;
        }
    }
}