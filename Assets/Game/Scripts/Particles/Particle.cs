using UnityEngine;
using UnityEngine.Events;
public class Particle : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Transform _transform;
    private Timer _timer;
    private Transform _targetTransform;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _timer = new Timer();
        _transform = transform;
    }
    private void Update()
    {
        _timer.UpdateTimer();
        if (_targetTransform!=null&&_timer.IsTick) _transform.position = _targetTransform.position;
    }
    public void PlayParticle(Vector3 scale,Transform target,float delay,UnityAction action=null)
    {
        _targetTransform = target;
        _transform.localScale = scale;
        _particleSystem.Play();
        _timer.StartTimer(delay, () =>
        {
            _transform.rotation=Quaternion.Euler(Vector3.zero);
            _transform.localScale = Vector3.one;
            _transform.SetParent(null);
            action?.Invoke();
        });
    }
}
