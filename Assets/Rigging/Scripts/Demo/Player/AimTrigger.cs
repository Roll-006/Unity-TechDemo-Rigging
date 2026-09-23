using System;
using UnityEngine;

namespace Rigging.Demo.Player
{
    public class AimTrigger : MonoBehaviour
    {
        public bool IsHit { get; private set; }
        
        private void OnTriggerEnter(Collider other)
        {
            IsHit = true;
        }

        private void OnTriggerExit(Collider other)
        {
            IsHit = false;
        }
    }
}