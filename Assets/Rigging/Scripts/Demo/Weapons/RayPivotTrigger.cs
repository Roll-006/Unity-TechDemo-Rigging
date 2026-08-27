using UnityEngine;

public class RayPivotTrigger : MonoBehaviour
{
    public bool IsHit { get; private set; }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"TriggerEnter : {other.gameObject.name}");
        IsHit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"TriggerExit : {other.gameObject.name}");
        IsHit = false;
    }
}
