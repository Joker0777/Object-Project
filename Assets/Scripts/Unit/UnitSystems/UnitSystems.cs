using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSystems : MonoBehaviour
{
    protected Unit unit;

    protected virtual void Awake()
    {
        if (!transform.root.TryGetComponent<Unit>(out unit))
        {
            Debug.Log("Unit not assigned to parent.");
        }
    }

}
