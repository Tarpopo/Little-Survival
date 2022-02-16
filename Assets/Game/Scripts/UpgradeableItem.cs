using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeableItem : ResourcePointCollector
{
    [SerializeField] private List<GameObject> _states;
    [SerializeField] private UnityEvent _onUpgrade;
    [SerializeField] private GameObject _canvas;
    private GameObject _currentState;
    private int _upgradeStep;

    protected override void Start()
    {
        base.Start();
        OnCountUpdate += TryUpdatePrefab;
        _currentState = _states[0];
        _upgradeStep = GetAllResourceCount() / (_states.Count - 1);
        if (_saveItem == false) return;
        //TryUpdatePrefab();
        _currentState.SetActive(false);
        _currentState = GetAllCurrentResource() >= GetAllResourceCount() / 2 ? _states[1] : _states[0];
        _currentState.SetActive(true);
        if (IsAllCollected == false) return;
        _currentState.SetActive(false);
        _states.GetLastElement().SetActive(true);
        OnAllCollected();
        _canvas.SetActive(false);
    }

    private void TryUpdatePrefab()
    {
        if (GetAllCurrentResource() % _upgradeStep > 0) return;
        _currentState.SetActive(false);
        _states.TryGetNextItem(_currentState, out _currentState);
        _currentState.SetActive(true);
        _onUpgrade?.Invoke();
    }

    public void ResetAll()
    {
        // ResetAllResources();
        // TryUpdatePrefab();
    }
}