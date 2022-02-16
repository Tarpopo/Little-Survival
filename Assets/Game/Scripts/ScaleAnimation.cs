using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    [SerializeField] private bool _startOnEnable;
    [SerializeField] private Vector3 _startScale;
    [SerializeField] private Vector3 _endScale;
    [SerializeField] private float _duration;
    
    private void OnEnable()
    {
        if (_startOnEnable == false) return;
        Play();
    }

    public void Play()
    {
        transform.localScale = _startScale;
        transform.DOScale(_endScale,_duration);
    }

    public void ResetAnimation()
    {
        transform.localScale = _endScale;
        transform.DOScale(_startScale,_duration);
    }
}