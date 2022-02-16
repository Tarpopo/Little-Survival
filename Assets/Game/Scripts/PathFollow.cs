using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PathFollow : MonoBehaviour
{
    [SerializeField] private UnityEvent _onFlollowEnd;
    [SerializeField] private float _rotateDuration;
    [SerializeField] private float _moveDuration;
    [SerializeField] private int _moveFrames;
    [SerializeField] private Transform _moveObject;
    [SerializeField] private List<Transform> _points;
    private Transform _currentPoint;

    public void StartMove()
    {
        _currentPoint = _points[0];
        //StartCoroutine(CorroutinesKid.Move(_moveObject, _currentPoint.position, _moveFrames, TryStartPoint));
        _moveObject.DOMove(_currentPoint.position, _moveDuration).onComplete = TryStartPoint;
        _moveObject.DOLocalRotate(_currentPoint.eulerAngles, _rotateDuration);
    }

    private void TryStartPoint()
    {
        _points.Remove(_currentPoint);
        if (_points.Count <= 0)
        {
            _onFlollowEnd?.Invoke();
            return;
        }
        _currentPoint = _points[0];
        //StartCoroutine(CorroutinesKid.Move(_moveObject, _currentPoint.position, _moveFrames, TryStartPoint));
        _moveObject.DOMove(_currentPoint.position, _moveDuration).onComplete = TryStartPoint;
        _moveObject.DORotate(_currentPoint.eulerAngles, _rotateDuration);
    }
}