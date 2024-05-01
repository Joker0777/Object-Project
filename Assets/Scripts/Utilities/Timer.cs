using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Timer 
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

    public Timer(float timerDuration)
    {
        _timerDuration = timerDuration;
    }
}

*/

public class Timer
{
    private Coroutine _cooldownCoroutine;
    private MonoBehaviour _monoBehaviour;

    public bool IsRunning => _cooldownCoroutine != null;

    private float _timerDuration;

    public float TimerDuration
    {
        get { return _timerDuration; }
        set { _timerDuration = value; }
    }



    public Timer(MonoBehaviour monoBehaviour, float timerDuration)
    {
        _monoBehaviour = monoBehaviour;
        _timerDuration = timerDuration;
    }

    public void StartTimer()
    {
        if (IsRunning)
        {
            _monoBehaviour.StopCoroutine(_cooldownCoroutine);
        }

        _cooldownCoroutine = _monoBehaviour.StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_timerDuration);
        _cooldownCoroutine = null;
    }

    public void StopTimer()
    {
        if (IsRunning)
        {
            _monoBehaviour.StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }
    }
}