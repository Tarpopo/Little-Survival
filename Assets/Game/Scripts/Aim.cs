using DG.Tweening;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 180;
    [SerializeField] private float _xRotateSpeed;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Transform _camera;
    [SerializeField] private Vector3 _punchPosition;
    [SerializeField] private GameObject _aimCenter;
    [SerializeField] private Transform _aim;
    [SerializeField] private Vector3 _aimDisableScale;
    [SerializeField] private float _aimDuration;
    [SerializeField] private Vector2 _xAngleClamp;
    [SerializeField] private Vector2 _yAngleClamp;
    [SerializeField] private Vector2 _xAimClamp;
    private int _cameraWidth;
    private Vector3 _aimStartScale;
    private Vector2 _startAngle;
    private int _rightSide;
    private int _leftSide;

    private void Start()
    {
        var camera = Camera.main;
        _camera = camera.transform;
        _cameraWidth = camera.pixelWidth / 2;
        _rightSide = camera.pixelWidth - camera.pixelWidth / 12;
        _leftSide = camera.pixelWidth / 12;
        _input = PlayerInput.Instance;
        SetStartAngle();
        _aimStartScale = _aim.localScale;
        _startAngle.Reverse();
    }

    private void SetStartAngle() => _startAngle = _camera.eulerAngles;

    private void OnTouchDown()
    {
        _startAngle = _camera.eulerAngles;
        _startAngle.Reverse();
    }

    private void RotateCamera()
    {
        var position = Vector3.zero;
        position.y = _startAngle.y + _input.MoveDirection.y * _rotateSpeed / Screen.width;
        position.x = _startAngle.x + -_input.MoveDirection.x * _rotateSpeed / Screen.width;
        var angle = GetClampVector(position);
        if (_input.TouchPosition.x >= _rightSide || _input.TouchPosition.x <= _leftSide)
        {
            _camera.Rotate(_camera.up, _xRotateSpeed * (_input.TouchPosition.x > _cameraWidth ? 1 : -1));
            //print(_input.TouchPosition.x > _cameraWidth);
            var eulerAngle = _camera.eulerAngles;
            eulerAngle.y = Mathf.Clamp(eulerAngle.y, _xAimClamp.x, _xAimClamp.y);
            eulerAngle.x = angle.y;
            eulerAngle.z = 0;
            _startAngle.x = eulerAngle.y;
            _camera.eulerAngles = eulerAngle;
            //OnTouchDown();
            //_camera.eulerAngles = new Vector3();
            // var vector = GetClampVector(_camera.eulerAngles);
            // _camera.eulerAngles = new Vector3(CalculateAngle(vector.y), CalculateAngle(vector.x));
        }
        else
        {
            _camera.eulerAngles = new Vector3(CalculateAngle(angle.y), CalculateAngle(angle.x));
        }
        //new Vector3(position.y, position.x);
    }

    private Vector2 GetClampVector(Vector3 vector)
    {
        return new Vector3(Mathf.Clamp(vector.x, _xAngleClamp.x, _xAngleClamp.y),
            Mathf.Clamp(vector.y, _yAngleClamp.x, _yAngleClamp.y), 0);
    }

    private float CalculateAngle(float angle) => angle >= 180 ? angle - 360 : angle;

    public void AnimateAim(bool isEnable)
    {
        if (isEnable == false) _aimCenter.SetActive(false);
        _aim.DOScale(isEnable ? _aimStartScale : _aimDisableScale, _aimDuration).onComplete = () =>
        {
            _aimCenter.SetActive(isEnable);
        };
    }

    public void SetActiveAim(bool isActive)
    {
        _aimCenter.gameObject.SetActive(isActive);
        _aim.gameObject.SetActive(isActive);
        _joystick.SetActive(!isActive);
        if (isActive)
        {
            _input.OnMove += RotateCamera;
            _input.OnTouchDown += OnTouchDown;
            _input.OnTouchUp += SetStartAngle;
        }
        else
        {
            _input.OnMove -= RotateCamera;
            _input.OnTouchUp -= SetStartAngle;
            _input.OnTouchDown -= OnTouchDown;
        }
    }

    public void DoShootAnimate()
    {
        _camera.DOPunchPosition(_punchPosition, 0.2f);
        transform.DOPunchScale(_punchPosition, 0.3f);
    }
}