using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Quaternion = System.Numerics.Quaternion;

public class CreativeShip : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMovingEnd;
    [SerializeField] private GameObject _bridge;
    [SerializeField] private Transform _ship;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _rotationDuration;

    [SerializeField] private Vector3 _rotation;

    //[SerializeField] private PathFollow _path;
    private CameraTargetMover _cameraTargetMover;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
        _cameraTargetMover = FindObjectOfType<CameraTargetMover>();
    }

    public void MoveShip()
    {
        _bridge.SetActive(false);
        _playerTransform.SetParent(transform);
        //_cameraTargetMover.ChangeSpeed(10);
        transform.DOMove(_endPoint.position, _moveDuration).onComplete = () =>
        {
            transform.DOLocalRotate(_endPoint.eulerAngles, _rotationDuration);
            _onMovingEnd?.Invoke();
        };
        transform.DOLocalRotate(_endPoint.eulerAngles-transform.eulerAngles, _rotationDuration);
    }

    private void ResetShip()
    {
        _playerTransform.SetParent(null);
        //transform.position = _startPosition;
        _bridge.SetActive(true);
        //_cameraTargetMover.ResetSpeed();
    }
}