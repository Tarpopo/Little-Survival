using UnityEngine;
using System.Collections;
using System.Diagnostics;
using TMPro;

public class FpsCounterDevOption : DevOption
{
    [SerializeField] private MyButton switchButton;
    [SerializeField] private TextMeshProUGUI toggleText;
    private FpsCounter _fpsCounter;

    private bool _isVisibleFpsCounter;
    
    protected override void Init()
    {
        base.Init();
        _fpsCounter = FindObjectOfType<FpsCounter>();

        if (_fpsCounter != null)
            _isVisibleFpsCounter = _fpsCounter.IsShow;

        RefreshToggleText();
        switchButton.OnClick += ToggleVisibility;
    }

    private void RefreshToggleText()
    {
        toggleText.text = _isVisibleFpsCounter ? "Hide FPS counter" : "Show FPS counter";
    }

    private void ToggleVisibility()
    {
        if (_fpsCounter != null)
        {
            _isVisibleFpsCounter = !_isVisibleFpsCounter;
            _fpsCounter.SetActiveFpsCounter(_isVisibleFpsCounter);
            RefreshToggleText();
        }
    }
}