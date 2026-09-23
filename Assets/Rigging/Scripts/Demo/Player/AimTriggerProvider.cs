using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class AimTriggerProvider : MonoBehaviour
    {
        [SerializeField] private Animator animator;   
        [SerializeField] private AimTrigger aimTrigger;
        [SerializeField] private AimTrigger aimArmBackTrigger;
        [SerializeField] private AimTrigger aimBackTrigger;
        
        public AimTrigger AimTrigger => aimTrigger;
        public AimTrigger AimArmBackTrigger => aimArmBackTrigger;
        public AimTrigger AimBackTrigger => aimBackTrigger;
        
        private void Awake()
        {
            Debugger.I.ValidateSerializeField(animator, this);
            Debugger.I.ValidateSerializeField(aimTrigger, this);
            Debugger.I.ValidateSerializeField(aimArmBackTrigger, this);
            Debugger.I.ValidateSerializeField(aimBackTrigger, this);
        }
    }
}
