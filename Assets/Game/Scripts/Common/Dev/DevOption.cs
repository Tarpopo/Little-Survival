using System;
using UnityEngine;
using System.Collections;

public class DevOption : MonoBehaviour
{
    public event Action<DevOption> OnPropertyChanged;
    
    [SerializeField] private bool needToHideDevPanel;
    public bool NeedToHideDevPanel => needToHideDevPanel;

    private void Awake()
    {
        Init();
        UpdateFields();
    }

    protected void ChangeProperty()
    {
        OnPropertyChanged?.Invoke(this);
    }

    protected virtual void Init() {}

    protected virtual void UpdateFields(){}
}