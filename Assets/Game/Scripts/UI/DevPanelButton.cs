using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DevPanelButton : MyButton
{
    [SerializeField] private DevPanelWindow devPanelWindowPrefab;

    private DevPanelWindow _devPanelWindow;
    private SceneUI _sceneUi;
    private bool isEnable;
    
    protected override void Awake()
    {
#if FLAG_DEV_PANEL
        isEnable = true;
#endif

        if (!isEnable)
        {
            gameObject.SetActive(false);
            return;
        } 
        
        base.Awake();
        Init();
    }

    private void Init()
    {
        _sceneUi = FindObjectOfType<SceneUI>();
        var canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
            _devPanelWindow = Instantiate(devPanelWindowPrefab, canvas.transform);
    }
    
    protected override void ClickButton()
    {
        base.ClickButton();
        
        if(!isEnable) return;
        ActivateDevPanel();   
    }

    private void ActivateDevPanel()
    {
        if (_devPanelWindow != null)
            _devPanelWindow.Enable(true);
    }
}