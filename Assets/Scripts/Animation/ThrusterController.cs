using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] EventManager _eventManager;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _eventManager.IsThrusting += ThrusterAnimation;
    }

    private void OnDisable()
    {
        _eventManager.IsThrusting -= ThrusterAnimation;
    }

    protected void ThrusterAnimation(bool thrustingState)
    {
        _animator.SetBool("IsThrusting", thrustingState);
        if (thrustingState )
        {
            _eventManager.OnPlayParticleEffect.Invoke("ThrusterTail", transform.position, 1f);
        }

    }


}
