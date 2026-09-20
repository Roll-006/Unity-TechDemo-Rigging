using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class AimArmBackState : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerFacing _playerFacing;
        private ITriggerable _weapon;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement = Debugger.I.ValidateComponent<PlayerMovement>(animator);
            _playerFacing = Debugger.I.ValidateComponent<PlayerFacing>(animator);
            var weaponHolder = Debugger.I.ValidateComponent<WeaponHolder>(animator);
            
            _weapon = weaponHolder.EquippedWeaponInstance.GetComponent<ITriggerable>();
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _playerMovement.StopSprint();
            _playerFacing.SyncCameraForward();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }
    }
}
