using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Weapons
{
    public class RayPivotTrigger : MonoBehaviour
    {
        [SerializeField] private SphereCollider trigger;
        [SerializeField] private float exitRadius;

        public bool IsHit { get; private set; }

        private float _defaultRadius;

        private void Awake()
        {
            Debugger.I.ValidateSerializeField(trigger, this);

            _defaultRadius = trigger.radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            IsHit = true;
            trigger.radius = exitRadius;
            Debug.Log("Enter");
        }

        private void OnTriggerExit(Collider other)
        {
            IsHit = false;
            trigger.radius = _defaultRadius;
            Debug.Log("Exit");
        }
    }
}
