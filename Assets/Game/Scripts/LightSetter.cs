using DefaultNamespace;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;

public class LightSetter : MySingleton<LightSetter>
{
    [SerializeField] private Transform _light;
    [SerializeField] private LightSet[] _lightSets;
    public void SetLight(int index) => _lightSets.ForEach(item => item.TrySetLight(_light, index));
    public void SetLight(Vector3 rotation) => _light.transform.eulerAngles = rotation;
}

[System.Serializable]
public struct LightSet
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _changeDuration;

    public void TrySetLight(Transform light, int levelIndex)
    {
        if (_levelIndex != levelIndex) return;
        light.transform.position = _position;
        light.transform.eulerAngles = _rotation;
        // light.DOMove(_position, _changeDuration);
        // light.DORotate(_rotation, _changeDuration);
    }
}