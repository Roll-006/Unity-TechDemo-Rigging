using UnityEngine;
using UnityEngine.Pool;

namespace Rigging.Core
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject pooledPrefab;
        [SerializeField] private Transform parent;
        [SerializeField, Min(0)] private int initializePoolSize;
        [SerializeField, Min(0)] private int maxPoolSize;
        
        private ObjectPool<GameObject> _pool;
        
        private void Start()
        {
            _pool = new(
                createFunc: OnCreateObject,
                actionOnGet: OnGetObject,
                actionOnRelease: OnReturnedObject,
                actionOnDestroy: OnDestroyObject,
                collectionCheck: false,
                defaultCapacity: initializePoolSize,
                maxSize: maxPoolSize
            );
        }

        private GameObject OnCreateObject()
        {
            return Instantiate(pooledPrefab, parent);
        }
        
        private void OnGetObject(GameObject obj)
        {
            obj.SetActive(true);
        }
        
        void OnReturnedObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        void OnDestroyObject(GameObject obj)
        {
            Destroy(obj);
        }

        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject obj)
        {
            _pool.Release(obj);
        }
    }
}