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
        
        private bool _prevCrouching;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(cinemachine, this);
            Debugger.I.ValidateSerializeField(shooter, this);
        }

        private void Update()
        {
            cinemachine.enabled = shooter.IsAiming;
        }
    }
}