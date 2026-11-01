using UnityEngine;

namespace Rigging.Demo.Items
{
    /// <summary>
    /// 所有されうるオブジェクト。
    /// </summary>
    public interface IOwnable
    {
        public GameObject Owner { get; }
        
        public void SetOwner(GameObject owner);
    }
}