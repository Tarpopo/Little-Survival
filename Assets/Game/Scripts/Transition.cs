using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _clear;
    [SerializeField] private Color _dark;
    [SerializeField] private float _duration;
    private bool _isPlay;

    public void PlayTransition(bool isDark, Action onTransitionEnd)
    {
        if (_isPlay) return;
        _isPlay = true;
        _image.DOColor(isDark ? _dark : _clear, _duration).onComplete = () =>
        {
            _isPlay = false;
            onTransitionEnd?.Invoke();
        };
    }
}