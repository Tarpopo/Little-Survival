using System;
using DG.Tweening;
using UnityEngine;

public class CameraTargetMover : Singleton<CameraTargetMover>
{
    [SerializeField] private CameraParameters _clearParameters;
    [SerializeField] private CameraParameters _startParameters;
    private CameraParameters _currentParameters;
    public void ResetParameters() => _currentParameters = _clearParameters;

    public void ChangeTarget(CameraParameters cameraParameters, float changeSpeed, bool changeRotation,
        bool immediateRotate = false, bool immediatePosition = false, Action onChangeEnd = null,
        bool isRotateAction = false)
    {
        _currentParameters = _clearParameters;
        StopAllCoroutines();
        if (changeRotation)
        {
            transform.DOKill();
            if (immediateRotate) transform.eulerAngles = cameraParameters.Rotation;
            else
                transform.DORotate(cameraParameters.Rotation, 0.7f).onComplete = () =>
                {
                    if (isRotateAction == false) return;
                    onChangeEnd?.Invoke();
                };
        }

        if (immediatePosition)
        {
            transform.position = cameraParameters.GetAllLerpPosition();
            _currentParameters = cameraParameters;
        }
        else
        {
            StartCoroutine(CorroutinesKid.Move(transform, cameraParameters.GetAllLerpPosition(), 6, () =>
            {
                _currentParameters = cameraParameters;
                if (isRotateAction) return;
                onChangeEnd?.Invoke();
            }));
        }
        // transform.DOMove(cameraParameters.GetAllLerpPosition(), changeSpeed).onComplete = () =>
        // {
        //     print("endMOve");
        //     _currentParameters = cameraParameters;
        //     onChangeEnd?.Invoke();
        // };
    }

    private void Awake()
    {
        //_clearParameters.SetDirection(transform.position);
        _currentParameters = _startParameters;
    }

    private void FixedUpdate()
    {
        if (_currentParameters.IsTarget == false) return;
        transform.position = _currentParameters.GetLerpPosition(transform.position);
    }
}

[Serializable]
public class CameraParameters
{
    public bool IsTarget => _target != null;
    public Vector3 Rotation => _rotation;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private bool _onlyXMoving;

    public void SetDirection(Vector3 position) => _direction = (position - _target.position).normalized;
    public Vector3 TargetPosition => _target.position;

    public Vector3 GetLerpPosition(Vector3 position)
    {
        if (_onlyXMoving == false) return Vector3.Lerp(position, _target.position + _direction * _distance, _speed);
        position.x = Vector3.Lerp(position, _target.position + _direction * _distance, _speed).x;
        return position;
    }

    public Vector3 GetAllLerpPosition() => _target.position + _direction * _distance;
}