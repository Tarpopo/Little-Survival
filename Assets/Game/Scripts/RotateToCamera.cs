using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    [SerializeField] private bool _xRotate;
    private Transform _camera;
    private Vector3 _startRotation;

    private void Start()
    {
        _camera = Camera.main.transform;
        _startRotation = transform.eulerAngles;
    }

    public void Rotate()
    {
        transform.LookAt(_camera);
        if (_xRotate == false) return;
        _startRotation.x = transform.eulerAngles.x;
        transform.eulerAngles = _startRotation;
    }

    private void Update() => Rotate();
}