using System.Collections;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private int _rotateSpeed;
    [SerializeField] private bool _startOnEnable;
    [SerializeField] private Vector3 _rotateAxis;

    public void SetRotateState(bool isActive)
    {
        if (isActive) StartCoroutine(Rotate());
        else StopAllCoroutines();
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(_rotateAxis, _rotateSpeed);
            yield return null;
        }
    }

    private void OnEnable() => SetRotateState(_startOnEnable);
    private void OnDisable() => StopAllCoroutines();
}
