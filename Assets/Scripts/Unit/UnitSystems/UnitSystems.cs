using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSystems : MonoBehaviour
{
    protected Unit unit;

    protected virtual void Awake()
    {
        unit = transform.root.GetComponent<Unit>();
        if (unit == null)
        {
            Debug.Log("Unit not assigned to parent.");
        }
    }  
}
