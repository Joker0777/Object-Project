using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer 
{
    private float _timer;
    private float _timerDuration;

    public float TimerDuration 
    {  
        get { return _timerDuration; } 
        set { _timerDuration = value; }
    }

    public void UpdateTimer(float deltaTime)
    {
   
        if (_timer > 0)
        {
            _timer -= deltaTime;
        }
        else
        {
            _timer = 0;
        }
    }

    
    public void StartTimer()
    {
        _timer = _timerDuration;
    }


    public bool IsRunning()
    {
        return _timer > 0;
    }

    public CooldownTimer(float timerDuration)
    {
        _timerDuration = timerDuration;
    }
}
