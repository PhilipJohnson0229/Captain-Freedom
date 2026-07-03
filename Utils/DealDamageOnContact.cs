using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;

    private ulong ownerClientId;

    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody == null) { return; }

        //if (col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        //{
        //    health.TakeDamage(damage);
        //}
    }
}
