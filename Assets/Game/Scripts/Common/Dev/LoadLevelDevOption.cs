using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadLevelDevOption : DevOption
{
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private MyButton confirmationButton;

    private LevelManager _levelManager;
    private int _currentLevelIndex;

    protected override void Init()
    {
        base.Init();

        _levelManager = FindObjectOfType<LevelManager>();
        
        dropDown.onValueChanged.AddListener(UpdateDropdown);
        confirmationButton.OnClick += LoadLevel;
    }

    protected override void UpdateFields()
    {
        base.UpdateFields();
        dropDown.ClearOptions();

        if (_levelManager != null)
        {
            var levelNames = new List<string>();

            for (var i = 0; i < _levelManager.Levels.Count; i++)
            {
                levelNames.Add($"Level {i}");
            }
            
            dropDown.AddOptions(levelNames);
        }
    }

    private void LoadLevel()
    {
        ChangeProperty();

        Statistics.CurrentLevelNumber = _currentLevelIndex + 1;
        _levelManager.LoadLevel(_currentLevelIndex);
    }

    private void UpdateDropdown(int value)
    {
        _currentLevelIndex = value;
    }
}