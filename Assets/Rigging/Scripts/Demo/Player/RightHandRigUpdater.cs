using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Riggin.Demo.Player
{
    public class RightHandRigUpdater : MonoBehaviour
    {
        [SerializeField] private Rig rig;
        [SerializeField] private float towardSpeed;

        private bool _isEnable;

        private void Awake()
        {
            if (rig == null)
                throw new InvalidOperationException($"{nameof(rig)} ({typeof(Rig).Name}) の取得に失敗しました。SerializeField に登録してください。");
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

        public void EnableRightHandRig()
        {
            _isEnable = true;
        }

        public void DisableRightHandRig()
        {
            _isEnable = false;
        }
    }
}
