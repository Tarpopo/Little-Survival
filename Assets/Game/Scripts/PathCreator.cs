using System.Linq;
using UnityEngine;

[System.Serializable]
public class PathCreator
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _closeRadius = 0.1f;
    private Vector3 _currentPoint;
    private Transform _currentTransform;
    public bool IsClosePoint(Vector3 target) => Vector3.Distance(_currentPoint, target) <= _closeRadius;

    public Vector3 GetRandomPoint()
    {
        if (_currentTransform != null) _currentTransform.gameObject.SetActive(true);
        _currentTransform = _points.Where(item => item.gameObject.activeSelf).GetRandomElement();
        _currentPoint = _currentTransform.position;
        _currentTransform.gameObject.SetActive(false);
        return _currentPoint;
    }

    public void OnDisable()
    {
        if (_currentTransform == null) return;
        _currentTransform.gameObject.SetActive(true);
    }
}