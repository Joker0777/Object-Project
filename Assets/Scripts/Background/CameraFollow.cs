using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Unit player;  
    public float smoothSpeed = 0.125f;
    public BackgroundElement[] _backgroundElement;
    private EventManager _eventManager;

    private Vector3 lastCameraPosition;


    private void Awake()
    {
       _eventManager = EventManager.Instance;
    }
    private void Start()
    {
        lastCameraPosition = transform.position;

    }

    private void OnEnable()
    {
        _eventManager.OnPlayerRespawn += GetNewPlayerReference;

    }

    private void OnDisable()
    {
        _eventManager.OnPlayerRespawn -= GetNewPlayerReference;
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        transform.position = new Vector3(player.transform.position.x, player.transform.transform.position.y, transform.position.z);
        Vector3 movmentDirection = transform.position - lastCameraPosition;

        foreach (var element in _backgroundElement)
        {
            element.BackgroundTransformPosition += new Vector3(movmentDirection.x * element.ScrollSpeed, movmentDirection.y * element.ScrollSpeed);
        }
  
        lastCameraPosition = transform.position;
    }

    private void GetNewPlayerReference(Unit playerUnit)
    {
        player = playerUnit;
    }
}
