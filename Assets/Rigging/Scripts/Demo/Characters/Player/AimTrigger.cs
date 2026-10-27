using UnityEngine;

namespace Rigging.Demo.Characters.Player
{
    public class AimTrigger : MonoBehaviour
    {
        public bool IsHit { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            
            IsHit = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;
            
            IsHit = false;
        }
    }
}