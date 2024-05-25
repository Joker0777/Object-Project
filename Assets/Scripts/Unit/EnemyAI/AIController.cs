using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private AIState currentState;
    

    void Start()
    {
        ChangeState(new PatrolState(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        { 
            currentState.OnStateRun();
        }     
    }

    public void ChangeState(AIState state)
    {
        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = state;

        currentState.OnStateEnter();
    }


}
