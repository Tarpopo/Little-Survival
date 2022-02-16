using DG.Tweening;
using UnityEngine;

public class TriggerScaler : MonoBehaviour
{
    [SerializeField] private Vector3 _scale;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private int _vibrato;
    [SerializeField] private float _elasticity;
    [SerializeField] private float _duration = 0.5f;
    private bool _isPlaying;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        TryStartAnimation();
    }

    private void TryStartAnimation()
    {
        if (_isPlaying) return;
        _isPlaying = true;
        transform.DOPunchScale(_scale, _duration + 0.1f, _vibrato, _elasticity).onComplete = () => _isPlaying = false;
        transform.DOPunchRotation(_rotation, _duration, _vibrato, _elasticity);
    }
}