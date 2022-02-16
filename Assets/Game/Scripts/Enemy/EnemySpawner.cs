using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCounterEnd;
    [SerializeField] private Counter _enemyCounter;
    [SerializeField] private Health[] _enemies;
    [SerializeField] private GameObject[] _enemyiesObjects;
    [SerializeField] private bool _changeLightOnEnable;
    [SerializeField] private int _lightSetIndex = 2;

    private void Start()
    {
        foreach (var enemy in _enemies) enemy.OnDeath += _enemyCounter.Reduce;
        _enemyCounter.OnCounterEmpty += () => Invoke(nameof(OnCounterEnd), 0.5f);
    }

    public void OnCounterEnd() => _onCounterEnd?.Invoke();

    private void OnEnable()
    {
        if (_changeLightOnEnable) LightSetter.Instance.SetLight(_lightSetIndex);
        _enemyCounter.SetMaxCount(_enemies.Length);
        _enemyCounter.RestartCounter();
        foreach (var enemy in _enemyiesObjects) enemy.SetActive(true);
    }

    private void OnDisable() => LightSetter.Instance.SetLight(Statistics.CurrentLevelNumber);
}