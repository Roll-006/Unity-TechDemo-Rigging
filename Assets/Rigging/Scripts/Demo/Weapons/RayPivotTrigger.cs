using System;
using UnityEngine;

public class RayPivotTrigger : MonoBehaviour
{
    [SerializeField] private SphereCollider trigger;
    [SerializeField] private float exitRadius;

    public bool IsHit { get; private set; }

    private float _defaultRadius;

    private void Awake()
    {
        if (trigger == null)
            throw new InvalidOperationException($"{nameof(trigger)} ({typeof(SphereCollider).Name}) ÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩÅBSerializeField Ç…ìoò^ÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB");

        _defaultRadius = trigger.radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"TriggerEnter : {other.gameObject.name}");
        IsHit = true;
        trigger.radius = exitRadius;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"TriggerExit : {other.gameObject.name}");
        IsHit = false;
        trigger.radius = _defaultRadius;
    }
}
