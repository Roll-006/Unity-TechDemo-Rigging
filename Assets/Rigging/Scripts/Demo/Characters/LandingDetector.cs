using UnityEngine;

namespace Rigging.Demo.Characters
{
    /// <summary>
    /// 地面に着地したかを検出する。
    /// </summary>
    public class LandingDetector : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float rayDistance;
        [SerializeField, Min(0f)] private float rayOffset;
        [SerializeField] private LayerMask effectiveLayers;
        
        /// <summary>
        /// 地面に着地している。
        /// </summary>
        public bool IsLanding { get; private set; }

        private void Start()
        {
            CheckLanding();
        }
        
        private void Update()
        {
            CheckLanding();
        }

        private void CheckLanding()
        {
            IsLanding = Physics.Raycast(transform.position + Vector3.up * rayOffset, Vector3.down, rayDistance, effectiveLayers);
            Debug.DrawLine(transform.position + Vector3.up * rayOffset, transform.position + Vector3.down * rayDistance, Color.yellow);
        }
    }
}