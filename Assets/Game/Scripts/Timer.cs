using System;
using UnityEngine;
public class Timer 
{
    private float _time;
    private Action _onTimerEnded;
    public bool IsTick => _time > 0;
    public void StartTimer(float time,Action onEndTimer)
    {
        _time = time;
        _onTimerEnded = onEndTimer;
    }
    public void UpdateTimer()
    {
        if (_time <= 0) return;
        _time -= Time.deltaTime;
        if (_time <= 0) _onTimerEnded?.Invoke();
    }
}
