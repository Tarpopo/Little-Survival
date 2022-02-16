using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DevPanelWindow : CanvasGroupBased
{
    [SerializeField] private MyButton closeButton;
    private DevOption[] _devOptions;

    protected override void Awake()
    {
        base.Awake();
        _devOptions = GetComponentsInChildren<DevOption>();
        closeButton.OnClick += DisableWindow;
    }

    private void DisableWindow()
    {
        Enable(false);
    }

    private void OnEnable()
    {
        foreach (var devOption in _devOptions)
        {
            devOption.OnPropertyChanged += CheckOption;
        }
    }

    private void OnDisable()
    {
        foreach (var devOption in _devOptions)
        {
            devOption.OnPropertyChanged -= CheckOption;
        }
    }

    private void CheckOption(DevOption devOption)
    {
        if (devOption.NeedToHideDevPanel)
            Enable(false);
    }
}