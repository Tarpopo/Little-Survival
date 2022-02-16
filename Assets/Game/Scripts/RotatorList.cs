using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorList : MonoBehaviour
{
    [SerializeField] private int _rotateSpeed;
    [SerializeField] private bool _startOnEnable;
    [SerializeField] private Vector3 _rotateAxis;
    [SerializeField] private Transform[] _rotaries;

    public void SetRotateState(bool isActive)
    {
        if (isActive) StartCoroutine(Rotate());
        else StopAllCoroutines();
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            foreach (var rotateable in _rotaries) rotateable.Rotate(_rotateAxis, _rotateSpeed);
            yield return null;
        }
    }

    private void OnEnable() => SetRotateState(_startOnEnable);
    private void OnDisable() => StopAllCoroutines();
}