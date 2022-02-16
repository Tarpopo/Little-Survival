using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAllUIButton : MyButton
{
    private SceneUI _sceneUi;
    private DevPanelWindow _devPanelWindow;
    private Coroutine checkCornerClickLoop;
    
    protected override void Awake()
    {
        base.Awake();

        _sceneUi = FindObjectOfType<SceneUI>();
        _devPanelWindow = GetComponentInParent<DevPanelWindow>();
    }

    protected override void ClickButton()
    {
        base.ClickButton();

        if (_sceneUi != null)
        {
            _sceneUi.SetActiveAllWindow(false);
            _devPanelWindow.Enable(false);
            
            if(checkCornerClickLoop != null)
                StopCoroutine(checkCornerClickLoop);

            checkCornerClickLoop = _sceneUi.StartCoroutine(CheckCornerClickLoop());
        }
    }

    private IEnumerator CheckCornerClickLoop()
    {
        var clickBoxSize = Mathf.Min(Screen.height, Screen.width) / 5f;
        
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                
                if(mousePosition.x > Screen.width - clickBoxSize && mousePosition.y > Screen.height - clickBoxSize)
                {
                    _sceneUi.SetActiveAllWindow(true);
                    yield break;
                }
            }
            
            yield return null;
        }
    }
}
