using System.Collections.Generic;
using Rigging.Core;
using UnityEngine;

namespace Rigging.Demo.Items
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private GameObjectPool _bulletPool;
        private GameObjectPool _hitPointPool;
        private Vector3 _initialPosition;
        private float _range;
        private List<Collider> _ignoreColliders = new();

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _initialPosition = transform.position;
        }
        
        private void Update()
        {
            if (Vector3.Distance(transform.position, _initialPosition) > _range)
            {
                Release();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            foreach (var ignoreCollider in _ignoreColliders)
            {
                if (ignoreCollider == collision.collider) return;
            }
            
            var contact = collision.GetContact(0);
            _hitPointPool.Get().transform.SetPositionAndRotation(contact.point, Quaternion.LookRotation(contact.normal));

            Release();
        }

        /// <summary>
        /// 撃たれた。
        /// </summary>
        /// <param name="initialVelocity">初速度</param>
        /// <param name="range">射程</param>
        /// <param name="shotPivot">発射地点の Pivot</param>
        /// <param name="shotDirection">撃った方向</param>
        /// <param name="bulletPool">弾丸のプールマネージャー</param>
        public void OnShot(float initialVelocity, float range, Transform shotPivot, Vector3 shotDirection, GameObjectPool bulletPool)
        {
            _bulletPool = bulletPool;
            _range = range;
            
            transform.position = shotPivot.position;
            transform.rotation = shotPivot.rotation;
            _rigidbody.AddForce(shotDirection * initialVelocity, ForceMode.Impulse);
        }

        private void Release()
        {
            _bulletPool.Release(gameObject);
            _ignoreColliders.Clear();
        }

        public void RecordHitPoint(GameObjectPool hitPointPool)
        {
            _hitPointPool =  hitPointPool;
        }
    }
}