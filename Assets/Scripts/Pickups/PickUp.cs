using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] PickupType type;
    [SerializeField] string unitTag = "Player";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        Unit unit = collision.GetComponent<Unit>();
        if (unit != null && unit.CompareTag(unitTag))
        {
           PickUpManager.instance.CollectObject(unit, type);
           Destroy(gameObject);
        }       
    }
}
