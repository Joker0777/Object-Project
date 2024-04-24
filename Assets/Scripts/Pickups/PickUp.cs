using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.attachedRigidbody.GetComponent<Unit>();
        if (unit != null && unit.CompareTag("Player"))
        {
            CollectObject(unit);           
        }
    }

    public virtual void CollectObject(Unit unit)
    {
        Destroy(gameObject);
    }
}
