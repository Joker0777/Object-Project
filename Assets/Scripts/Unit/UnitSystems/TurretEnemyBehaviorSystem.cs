using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyBehaviorSystem : EnemyBehaviourSystem
{
    [SerializeField] TurretController[] _turrets;


    protected override void Update()
    {
        if (_currentState == EnemyState.Attack)
        {
            foreach (TurretController controller in _turrets)
            {
                controller.RotateTurretToTarget(directionToCurrentTarget);
            }
        }
      //  else
      //  {
       //     foreach(TurretController controller in _turrets)
       //     {
        //        controller.RotateTurretToTarget(unit.transform.forward);
        //    }
      //  }
        
        base.Update();
    }
}
