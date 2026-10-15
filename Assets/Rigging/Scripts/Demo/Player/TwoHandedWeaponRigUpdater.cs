using Rigging.Core;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Rigging.Demo.Player
{
    public class TwoHandedWeaponRigUpdater : MonoBehaviour
    {
        [SerializeField] private Rig rig;
        [SerializeField] private float towardSpeed;

        private bool _isEnable;

        private void Awake()
        {
            Debugger.ValidateSerializeField(rig, this);
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

        public void OnEnableTwoHandedWeaponRig()
        {
            _isEnable = true;
        }

        public void OnDisableTwoHandedWeaponRig()
        {
            _isEnable = false;
        }
    }
}