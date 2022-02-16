using System;
using UnityEngine;
using UnityEngine.Events;

public class CameraTargetChanger : MonoBehaviour
{
    [SerializeField] private CameraParameters _cameraParameters;
    [SerializeField] private bool _changeOnEnable;
    [SerializeField] private float _changeSpeed = 0.5f;

    [SerializeField] private bool _immediateRotation;

    [SerializeField] private bool _immadiatePosition;

    //[SerializeField] private bool _changeRotation = true;
    [SerializeField] private UnityEvent _onChangeEnd;
    [SerializeField] private bool _isRotateAction;

    private void OnEnable()
    {
        if (_changeOnEnable) ChangeTarget(true, () => _onChangeEnd?.Invoke());
    }

    //public void DisableRotation() => _changeRotation = false;
    public void ChangeTarget(bool changeRotation, Action onChangeEnd) =>
        CameraTargetMover.Instance.ChangeTarget(_cameraParameters, _changeSpeed, changeRotation, _immediateRotation,
            _immadiatePosition, onChangeEnd,_isRotateAction);

    public void ChangeTarget() =>
        CameraTargetMover.Instance.ChangeTarget(_cameraParameters, _changeSpeed, true, false, false,
            () => _onChangeEnd?.Invoke());
}