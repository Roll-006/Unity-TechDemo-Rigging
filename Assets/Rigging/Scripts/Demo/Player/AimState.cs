using Rigging.Core;
using Rigging.Demo.Player;
using UnityEngine;

namespace Riggin.Demo.Player
{
    public class AimState : StateMachineBehaviour
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
