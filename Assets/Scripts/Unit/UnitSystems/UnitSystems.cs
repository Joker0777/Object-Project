using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSystems : MonoBehaviour
{
    protected Unit unit;

    protected EventManager _eventManager;

    protected virtual void Awake()
    {
        _eventManager = EventManager.Instance;
        if (!transform.root.TryGetComponent<Unit>(out unit))
        {
            Debug.Log("Unit not assigned to parent.");
        }      
    }

    protected virtual void Start()
    {

        if (_eventManager == null)
        {
            Debug.LogError("EventManager is not assigned.");
        }
    }

}
