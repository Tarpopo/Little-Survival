using UnityEngine;

public class LightLevelSetter : MonoBehaviour
{
    [SerializeField] private bool _changeOnEnable = true;
    [SerializeField] private Vector3 _rotation;

    private void OnEnable()
    {
        if (_changeOnEnable == false) return;
        SetLight();
    }

    public void SetLight() => LightSetter.Instance.SetLight(_rotation);
}