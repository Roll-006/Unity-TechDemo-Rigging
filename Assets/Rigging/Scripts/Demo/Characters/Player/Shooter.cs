using R3;
using Rigging.Common;
using Rigging.Core;
using Rigging.Demo.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rigging.Demo.Characters.Player
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionReference aimGunAction;
        [SerializeField] private InputActionReference fireAction;
        [SerializeField] private Animator animator;
        [SerializeField] private WeaponHolder weaponHolder;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private CameraPivotMovement cameraPivotMovement;
        [SerializeField] private Transform gripPivot;
        [SerializeField] private Transform aimTrigger;
        [SerializeField] private Transform spine2Pivot;
        [SerializeField] private float towardSpeed;
        
        public ReadOnlyReactiveProperty<bool> IsAiming => _isAiming;
        
        private readonly ReactiveProperty<bool> _isAiming = new ReactiveProperty<bool>();
        private IReticleProvider _reticleProvider;
        private IShootable _shootableWeapon;
        private int _gunLayerIndex;
        private bool _isAimingInput;

        private void Awake()
        {
            Debugger.ValidateSerializeField(playerInput, this);
            Debugger.ValidateSerializeField(aimGunAction, this);
            Debugger.ValidateSerializeField(fireAction, this);
            Debugger.ValidateSerializeField(animator, this);
            Debugger.ValidateSerializeField(weaponHolder, this);
            Debugger.ValidateSerializeField(playerMovement, this);
            Debugger.ValidateSerializeField(cameraPivotMovement, this);
            Debugger.ValidateSerializeField(gripPivot, this);
            Debugger.ValidateSerializeField(aimTrigger, this);
            Debugger.ValidateSerializeField(spine2Pivot, this);

            _gunLayerIndex = animator.GetLayerIndex(AnimatorLayers.GunLayer);
        }
        
        private void Start()
        {
            weaponHolder.EquippedWeaponInstanceChanged
                .Skip(1)
                .Where(x => x != null)
                .Subscribe(ChangeReticleProvider)
                .AddTo(this);
            
            weaponHolder.EquippedWeaponInstanceChanged
                .Skip(1)
                .Subscribe(ChangeShootableWeapon)
                .AddTo(this);
            
            IsAiming
                .Where(isAiming => isAiming == true)
                .Subscribe(InitializeReticle)
                .AddTo(this);
        }
        
        private void OnEnable()
        {
            playerInput.onActionTriggered += OnActionTriggered;
        }

        private void OnDisable()
        {
            playerInput.onActionTriggered -= OnActionTriggered;
        }

        private void OnDestroy()
        {
            _isAiming.Dispose();
        }

        private void Update()
        {
            _isAiming.Value = _isAimingInput && _shootableWeapon != null;
            UpdateReticleSize();
        }

        private void LateUpdate()
        {
            UpdateAnimatorParameters();
            LayerWeightUpdater.UpdateLayerWeight(animator, _gunLayerIndex, IsActiveLayer(), towardSpeed);
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (OnAimGun(context)) return;
            if (OnFire(context)) return;
        }

        private bool OnAimGun(InputAction.CallbackContext context)
        {
            if (context.action != aimGunAction.action) return false;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    _isAimingInput = true;
                    _isAiming.Value = _isAimingInput && _shootableWeapon != null;
                    break;
                    
                case InputActionPhase.Canceled:
                    _isAimingInput = false;
                    _isAiming.Value = false;
                    break;
            }

            return true;
        }

        private bool OnFire(InputAction.CallbackContext context)
        {
            if (context.action != fireAction.action) return false;

            return true;
        }

        private void UpdateAnimatorParameters()
        {
            animator.SetBool(AnimatorParameters.IsAiming, IsAiming.CurrentValue);
            animator.SetBool(AnimatorParameters.CanAim, CanAim());
            animator.SetBool(AnimatorParameters.HasHandgun, weaponHolder.PendingWeapon != null && weaponHolder.PendingWeapon.WeaponType == WeaponType.Handgun);
            animator.SetBool(AnimatorParameters.HasRifle, weaponHolder.PendingWeapon != null && weaponHolder.PendingWeapon.WeaponType == WeaponType.Rifle);
        }

        private bool IsActiveLayer()
        {
            return weaponHolder.EquippedWeapon != null || weaponHolder.PendingWeapon != null;
        }

        private bool CanAim()
        {
            var spine2ToAimTrigger = aimTrigger.position - spine2Pivot.position;
            if (Physics.Raycast(
                    spine2Pivot.position,
                    spine2ToAimTrigger.normalized,
                    spine2ToAimTrigger.magnitude,
                    ~LayerMasks.Player,
                    QueryTriggerInteraction.Ignore))
                return false;
            
            return true;
        }

        private void UpdateReticleSize()
        {
            if (_reticleProvider == null) return;
            
            if (playerMovement.IsMoving || cameraPivotMovement.InputMagnitude > 1f)
            {
                _reticleProvider.OnEnlargeReticle();
                return;
            }

            if (IsAiming.CurrentValue)
            {
                _reticleProvider.OnReduceReticle();
            }
            else
            {
                _reticleProvider.OnStopReticle();
            }
        }

        private void InitializeReticle(bool isAiming)
        {
            _reticleProvider.ReticleModel.CurrentSize.Value = _reticleProvider.ReticleModel.MaxSize;
        }

        /// <summary>
        /// レティクルを持つオブジェクトを武器着脱時に変更する。
        /// </summary>
        /// <param name="equippedWeaponInstance">Hierarchy 上に実在する装備している武器</param>
        private void ChangeReticleProvider(WeaponBase equippedWeaponInstance)
        {
            _reticleProvider = equippedWeaponInstance.GetComponent<IReticleProvider>();
        }

        /// <summary>
        /// 撃つことが可能な武器を着脱時に変更する。
        /// </summary>
        /// <param name="equippedWeaponInstance">Hierarchy 上に実在する装備している武器</param>
        private void ChangeShootableWeapon(WeaponBase equippedWeaponInstance)
        {
            _shootableWeapon = equippedWeaponInstance != null ? equippedWeaponInstance.GetComponent<IShootable>() : null;
        }
    }
}