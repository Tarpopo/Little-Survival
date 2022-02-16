using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingButton : MyButton
{
    [SerializeField] private CanvasGroup childCanvas;
    [SerializeField] private HideSettingsButton _settingsButton;
    private bool isActivated;
    private float _holdTime;
    private bool _isPressed;

    protected override void Awake()
    {
        base.Awake();
        _settingsButton.OnClick += CloseSettings;
    }

    private void CloseSettings()
    {
        isActivated = false;
        childCanvas.DOFade(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => SetEnableClick(false));
    }

    protected override void ClickButton()
    {
        base.ClickButton();
        
        isActivated = !isActivated;

        if (isActivated)
        {
            childCanvas.DOFade(1f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => SetEnableClick(true));
        }
        else
        {
            childCanvas.DOFade(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => SetEnableClick(false));
        }   
    }

    private void SetEnableClick(bool value)
    {
        childCanvas.interactable = value;
        childCanvas.blocksRaycasts = value;
    }
    
    private void Update()
    {
        if (_isPressed)
        {
            _holdTime += Time.deltaTime;
            if (_holdTime >= 5)
            {
#if FLAG_APPLOVIN
                MaxSdk.ShowMediationDebugger();
#endif
                Debug.Log("ShowMediationDebugger");
                //devPanelButton.IsEnable = true;
                _isPressed = false;
                _holdTime = 0;
            }
        }
    }
}