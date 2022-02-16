using System;
using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MySingleton<PlayerInput>
{
    [SerializeField] private float _activeRadius;
    [SerializeField] private float _fingerMoveRadius;
    [SerializeField] private float _fingerTimeToStop;
    [SerializeField] private UnityEvent _onMove;
    [SerializeField] private UnityEvent _onFingerMove;
    [SerializeField] private UnityEvent _onFingerStop;
    private readonly Timer _fingerTimer = new Timer();
    private Vector2 _fingerStartPosition;
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    public event UnityAction OnMove
    {
        add => _onMove.AddListener(value);
        remove => _onMove.RemoveListener(value);
    }

    public event UnityAction OnFingerMove
    {
        add => _onFingerMove.AddListener(value);
        remove => _onFingerMove.RemoveListener(value);
    }

    public event UnityAction OnFingerStop
    {
        add => _onFingerStop.AddListener(value);
        remove => _onFingerStop.RemoveListener(value);
    }

    [SerializeField] private UnityEvent _onTouchDown;

    public event UnityAction OnTouchDown
    {
        add => _onTouchDown.AddListener(value);
        remove => _onTouchDown.RemoveListener(value);
    }

    [SerializeField] private UnityEvent _onTouchUp;

    public event UnityAction OnTouchUp
    {
        add => _onTouchUp.AddListener(value);
        remove => _onTouchUp.RemoveListener(value);
    }

    public bool IsMouseDown { get; private set; }
    private Camera _camera;
    public bool IsMove => Input.GetMouseButton(0);
    public bool TimerIsTick => _fingerTimer.IsTick;

    public bool IsActiveRadius => Distance > _activeRadius;
    public Vector2 TouchPosition => _endPosition;
    public int XFingerMoveDirection => Math.Sign((_endPosition - _fingerStartPosition).x);

    //public Vector2 FingerMovedDirection => (Vector2) Input.mousePosition - _movePosition;
    public float Distance => Vector2.Distance(_startPosition, _endPosition);

    public float FingerDistance => Vector2.Distance(_fingerStartPosition, _endPosition);

    public Vector3 GetTouchOnPlane(Vector3 position)
    {
        var playerPlane = new Plane(Vector3.up, position);
        var ray = _camera.ScreenPointToRay(TouchPosition);
        return playerPlane.Raycast(ray, out var distToCam) ? ray.GetPoint(distToCam) : Vector3.zero;
    }

    public Vector3 MoveDirection => _startPosition - _endPosition;
    public float Angle => AngleBetweenTwoPoints(_startPosition, _endPosition);

    private void Start() => _camera = Camera.main;

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) => Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;

    // public Vector3 GetPositionOnScreen(Vector3 position)
    // {
    //     return _camera.WorldToScreenPoint(position);
    // }
    private void SetFingerParameters()
    {
        _fingerTimer.StartTimer(_fingerTimeToStop, () => _onFingerStop?.Invoke());
        _fingerStartPosition = Input.mousePosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //SetFingerParameters();
            _startPosition = Input.mousePosition;
            _endPosition = Input.mousePosition;
            _fingerStartPosition = Input.mousePosition;
            _onTouchDown?.Invoke();
            IsMouseDown = true;
        }

        if (Input.GetMouseButton(0))
        {
            _fingerTimer.UpdateTimer();
            //if (_fingerTimer.IsTick) _onFingerMove?.Invoke();
            if (Distance > _activeRadius) _onMove?.Invoke();
            //if (FingerDistance >= _fingerMoveRadius) SetFingerParameters();
            _endPosition = Input.mousePosition;
            //else if (FingerDistance > _fingerMoveRadius) SetFingerParameters();
            if (Input.GetAxis("Mouse X") != 0)
            {
                _fingerTimer.StartTimer(_fingerTimeToStop, () =>
                {
                    if (Input.GetAxis("Mouse X") != 0) return;
                    _fingerStartPosition = Input.mousePosition;
                    _onFingerStop?.Invoke();
                });
                _onFingerMove?.Invoke();
            }
            //else _onFingerStop?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _onTouchUp?.Invoke();
            _onFingerStop?.Invoke();
            IsMouseDown = false;
            //_startPosition = TouchPosition;
        }
    }
}