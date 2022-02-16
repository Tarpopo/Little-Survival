using UnityEngine;
using UnityEngine.Events;

public class LockedElement : MonoBehaviour
{
    [SerializeField] private bool _isLock = true;
    [SerializeField] private UnityEvent _onEnable;
    [SerializeField] private UnityEvent _onDisable;
    public void SetState(bool isLock) => _isLock = isLock;

    public void Enable()
    {
        if (_isLock) return;
        _onEnable?.Invoke();
    }

    public void Disable()
    {
        if (_isLock) return;
        _onDisable?.Invoke();
    }
}