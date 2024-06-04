using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;  
    public float smoothSpeed = 0.125f;
    public BackgroundElement[] _backgroundElement;

    public Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
            return;

        transform.position = new Vector3(playerTransform.position.x, playerTransform.transform.position.y, transform.position.z);
        Vector3 movmentDirection = transform.position - lastCameraPosition;

        foreach (var element in _backgroundElement)
        {
            element.BackgroundTransformPosition += new Vector3(movmentDirection.x * element.ScrollSpeed, movmentDirection.y * element.ScrollSpeed);
        }
       // Vector3 pos = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);

        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);



  
        lastCameraPosition = transform.position;
    }
}
