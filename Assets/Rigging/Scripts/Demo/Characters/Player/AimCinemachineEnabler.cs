using R3;
using Rigging.Common;
using Rigging.Core;
using UnityEngine;
using Unity.Cinemachine;

namespace Rigging.Demo.Characters.Player
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

        private void Start()
        {
            shooter.IsAiming
                .Subscribe(OnEnableCinemachine)
                .AddTo(this);
        }

        private void OnEnableCinemachine(bool isAiming)
        {
            cinemachine.enabled = isAiming;
        }
    }
}