using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDropDownList : MonoBehaviour
{
    [SerializeField] private DevLoadLevelButton _devLoadLevelButton;
    [SerializeField] private TMP_Dropdown levelDropdownList;
    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        Init();
        
        levelDropdownList.onValueChanged.AddListener(InitButton);
    }
    
    private void InitButton(int listID)
    {
        _devLoadLevelButton.Init(listID);
    }
    
    private void Init()
    {
        var levelManager = FindObjectOfType<LevelManager>();
        
        if (levelManager != null)
        {
            var levelNames = new List<string>();
            
            for (int i = 0; i < levelManager.Levels.Count; i++)
            {
                levelNames.Add($"Level {i}");
            }
            
            levelDropdownList.ClearOptions();   
            levelDropdownList.AddOptions(levelNames);
        }
    }
}