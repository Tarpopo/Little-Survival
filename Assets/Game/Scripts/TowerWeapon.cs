using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private AnimationComponent<WeaponAnimation> _animation;

    [SerializeField] private CameraTargetChanger _reloadPosition;
    [SerializeField] private CameraTargetChanger _shootPosition;
    [SerializeField] private Counter _bulletCounter;
    [SerializeField] private Transform _bullet;
    [SerializeField] private Transform _startBulletPoint;
    [SerializeField] private float _bulletMoveDuration = 0.5f;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _reloadDelay;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _rayDistance;

    [SerializeField] private Vector3 _weaponPosition;
    [SerializeField] private Vector3 _weaponRotation;

    [SerializeField] private float _shootZoom = 25;
    [SerializeField] private float _startZoom = 60;
    [SerializeField] private int _zoomSpeed = 15;
    [SerializeField] private float _afterZoomDelay;

    //[SerializeField] private Transform _towerParent;
    // [SerializeField] private Vector3 _startWeaponPoint;
    // [SerializeField] private Vector3 _endWeaponPoint;
    private Camera _camera;
    private Camera _uiCamera;
    private Timer _timer;
    private Aim _aim;
    private PlayerInput _playerInput;
    private bool _canShoot;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();
        _aim = FindObjectOfType<Aim>();
        _camera = Camera.main;
        _uiCamera = FindObjectsOfType<Camera>().FirstOrDefault(item => item.CompareTag("UICamera"));
        //FindObjectsOfType<Camera>().First(item => item.CompareTag("UICamera")).transform;
        _timer = new Timer();
        _animation.SetParameters();
        _bulletCounter.OnCounterEmpty += () =>
        {
            SetSubscribe(false);
            _timer.StartTimer(_reloadDelay, OnReloadEnd);
            //SetReloadPosition();
            SetReloadZoom();
            _animation.PlayAnimation(WeaponAnimation.Reload);
            _aim.AnimateAim(false);
        };
    }

    private void OnEnable()
    {
        _bullet.gameObject.SetActive(false);
        _bullet.position = _startBulletPoint.position;
        _bulletCounter.RestartCounter();
        _reloadPosition.ChangeTarget(true, () =>
        {
            transform.SetParent(_camera.transform);
            transform.localPosition = _weaponPosition;
            transform.localEulerAngles = _weaponRotation;
            SetSubscribe(true);
        });
        _aim.AnimateAim(false);
        _aim.SetActiveAim(true);
    }

    private void OnDisable()
    {
        //_aim.AnimateAim(false);
        _canShoot = false;
        //SetCamerasZoom(_startZoom, null);
        _camera.fieldOfView = _startZoom;
        _uiCamera.fieldOfView = _startZoom;
        _aim.SetActiveAim(false);
        SetSubscribe(false);
    }

    //private void OnDestroy() => StopAllCoroutines();

    private void Update()
    {
        _timer.UpdateTimer();
        TryShoot();
    }

    private void SetSubscribe(bool subscribe)
    {
        if (subscribe)
        {
            _playerInput.OnTouchUp += SetWeaponPosition;
            _playerInput.OnTouchDown += SetShootPosition;
            return;
        }

        _playerInput.OnTouchUp -= SetWeaponPosition;
        _playerInput.OnTouchDown -= SetShootPosition;
    }

    private void SetCamerasZoom(float zoom, Action onZoomEnd)
    {
        StopAllCoroutines();
        StartCoroutine(_camera.ChangeCameraField(zoom, _zoomSpeed, _afterZoomDelay, onZoomEnd));
        StartCoroutine(_uiCamera.ChangeCameraField(zoom, _zoomSpeed));
    }

    private void SetReloadZoom()
    {
        _canShoot = false;
        SetCamerasZoom(_startZoom, null);
        // StartCoroutine(_camera.ChangeCameraField(_startZoom, _zoomSpeed,
        //     () => _animation.PlayAnimation(WeaponAnimation.Reload)));
        //_aim.AnimateAim(false);
        //_reloadPosition.ChangeTarget(false, () => { _animation.PlayAnimation(WeaponAnimation.Reload); });
    }

    private void SetWeaponPosition()
    {
        _canShoot = false;
        _aim.AnimateAim(false);
        _animation.PlayAnimation(WeaponAnimation.ZoomOut);
        SetCamerasZoom(_startZoom, null);
        //_camera.fieldOfView = _startZoom;
        //StartCoroutine(_camera.ChangeCameraField(_startZoom, _zoomSpeed, null));
        //_reloadPosition.ChangeTarget(false, null);
    }

    private void SetShootPosition()
    {
        _aim.AnimateAim(true);
        _animation.PlayAnimation(WeaponAnimation.ZoomIn);
        SetCamerasZoom(_shootZoom, () => _canShoot = true);
        //StartCoroutine(_camera.ChangeCameraField(_shootZoom, _zoomSpeed, null));
        //_shootPosition.ChangeTarget(false, () => _canShoot = true);
    }

    private void OnReloadEnd()
    {
        SetSubscribe(true);
        if (_playerInput.IsMouseDown) SetShootPosition();
        //_aim.AnimateAim(true);
        _bulletCounter.RestartCounter();
    }

    private void TryShoot()
    {
        if (_canShoot == false || _timer.IsTick || _bulletCounter.IsEmpty) return;
        _timer.StartTimer(_shootDelay, null);
        Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit, _rayDistance,
            _layerMask) == false) return;
        _bullet.position = _startBulletPoint.position;
        hit.collider.GetComponent<Health>().TakeDamage(hit.point, 1);
        _bulletCounter.Reduce();
        _aim.DoShootAnimate();
        _bullet.gameObject.SetActive(true);
        _bullet.DOMove(hit.point, _bulletMoveDuration).onComplete += () => _bullet.gameObject.SetActive(false);
    }
}