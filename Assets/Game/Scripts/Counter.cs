using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Counter
{
    [SerializeField] private int _maxCount;
    private int _currentCount;
    public bool IsEmpty => _currentCount <= 0;
    [SerializeField] private UnityEvent _onCounterEmpty;
    public event UnityAction OnCounterEmpty
    {
        add => _onCounterEmpty.AddListener(value);
        remove => _onCounterEmpty.RemoveListener(value);
    }

    public void SetMaxCount(int maxCount)
    {
        if (maxCount < 0) return;
        _maxCount = maxCount;
    }

    public void RestartCounter()
    {
        _currentCount = _maxCount;
    }

    public void Reduce()
    {
        if (IsEmpty) return;
        _currentCount--;
        if (IsEmpty) _onCounterEmpty?.Invoke();
    }
}