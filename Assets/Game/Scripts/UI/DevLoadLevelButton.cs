using TMPro;
using UnityEngine;

public class DevLoadLevelButton : MyButton
{
    private int _levelID;
    private LevelManager _levelManager;

    protected override void Awake()
    {
        base.Awake();

        _levelManager = FindObjectOfType<LevelManager>();
    }

    protected override void ClickButton()
    {
        base.ClickButton();

        Statistics.PlayerLevel = _levelID + 1;
        _levelManager.LoadLevel(_levelID);
    }

    public void Init(int levelID)
    {
        _levelID = levelID;
    }
}