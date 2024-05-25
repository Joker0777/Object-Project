using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSystems : MonoBehaviour
{
    protected Unit unit;

    protected EventManager _eventManager;

    protected virtual void Awake()
    {
        if (!transform.root.TryGetComponent<Unit>(out unit))
        {
            Debug.Log("Unit not assigned to parent.");
        }      
    }

    protected virtual void Start()
    {
        _eventManager = unit.EventManager;
        if (_eventManager == null)
        {
            Debug.LogError("EventManager is not assigned.");
        }
    }

}
