using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private RectTransform _outsideCicle;
    [SerializeField] private RectTransform _insideCicle;
    [SerializeField] private float _radiusInsideCicle = 50;
    private Vector3 _normalizedDirection => (GetScreenPoint() - (Vector2) _outsideCicle.position).normalized;
    private bool _isActive;

    private void Start()
    {
        _camera = Camera.main;
        _playerInput = PlayerInput.Instance;
        SetActive(false);
    }

    private void SetActive(bool isActive)
    {
        _isActive = isActive;
        _outsideCicle.gameObject.SetActive(_isActive);
        if (_isActive)
        {
            _outsideCicle.localPosition = GetScreenPoint();
        }
        else
        {
            ResetPositions();
        }
    }

    private Vector2 GetScreenPoint()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, Input.mousePosition, _camera,
            out var position);
        return position;
    }

    private void ResetPositions()
    {
        _insideCicle.localPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _insideCicle.localPosition =
            -_playerInput.MoveDirection.normalized * Mathf.Clamp(_playerInput.Distance, 0, _radiusInsideCicle);
    }
}