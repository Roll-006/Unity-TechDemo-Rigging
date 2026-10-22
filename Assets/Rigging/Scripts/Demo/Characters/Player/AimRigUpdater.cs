using R3;
using Rigging.Core;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Rigging.Demo.Characters.Player
{
    public class AimRigUpdater : MonoBehaviour
    {
        [SerializeField] private Rig rig;
        [SerializeField] private float towardSpeed;
        [SerializeField] private Shooter shooter;
        
        private bool _isEnable;

        private void Awake()
        {
            Debugger.ValidateSerializeField(rig, this);
            Debugger.ValidateSerializeField(shooter, this);
        }

        private void Start()
        {
            shooter.IsAiming
                .Subscribe(OnEnableAimRig)
                .AddTo(this);
        }

        private void Update()
        {
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            var targetRigWeight = _isEnable ? 1f : 0f;
            rig.weight = Mathf.MoveTowards(rig.weight, targetRigWeight, towardSpeed * Time.deltaTime);
        }

        private void OnEnableAimRig(bool isAiming)
        {
            _isEnable = isAiming;
        }
    }
}