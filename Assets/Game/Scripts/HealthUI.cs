using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthUI
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private RotateToCamera _canvasRotator;

    public bool IsHaveUI => _progressBar != null;

    public void UpdateUI(float value, bool showUI)
    {
        _progressBar.fillAmount = value;
        _canvas.SetActive(showUI); 
        //UpdateCanvas();
    }

    // public void UpdateCanvas()
    // {
    //     if (_canvasRotator == null) return;
    //     _canvasRotator.Rotate();
    // }
}