using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleLevelObject : MonoBehaviour
{
    public event System.Action OnClick;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }
}