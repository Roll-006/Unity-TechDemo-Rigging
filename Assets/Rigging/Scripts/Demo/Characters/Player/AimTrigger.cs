using Rigging.Core;
using Rigging.Demo.Items;
using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class AimTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject rootGameObject;
        
        public bool IsHit { get; private set; }

        private void Awake()
        {
            Debugger.ValidateSerializeField(rootGameObject, this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            if (other.TryGetComponent<IOwnable>(out var ownable) && ownable.Owner == rootGameObject) return;
            
            IsHit = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;
            if (other.TryGetComponent<IOwnable>(out var ownable) && ownable.Owner == rootGameObject) return;
            
            IsHit = false;
        }
    }
}