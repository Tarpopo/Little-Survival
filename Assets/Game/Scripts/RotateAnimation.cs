using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] private int _saveItemID;
    [SerializeField] private bool _isSave;
    [SerializeField] private Vector3 _endRotate;
    [SerializeField] private float _duration;
    [SerializeField] private UnityEvent _onAnimationEnd;
    [SerializeField] private bool _localRotate;
    [SerializeField] private bool _rotateOnEnable;
    private bool _isPlay;

    private void Start()
    {
        if (_isSave == false) return;
        if (ItemsSaver.Instance.GetItemState(_saveItemID) != ItemState.Enable) return;
        if (_localRotate) transform.localEulerAngles = _endRotate;
        else transform.transform.eulerAngles = _endRotate;
        _onAnimationEnd?.Invoke();
    }

    private void OnEnable()
    {
        if (_rotateOnEnable) PlayAnimation();
    }

    public void PlayAnimation()
    {
        if (_isPlay) return;
        _isPlay = true;
        if (_localRotate) transform.DOLocalRotate(_endRotate, _duration);
        else transform.DORotate(_endRotate, _duration);
        _onAnimationEnd?.Invoke();
        if (_isSave) ItemsSaver.Instance.AddItem(_saveItemID, ItemState.Enable);
    }
}