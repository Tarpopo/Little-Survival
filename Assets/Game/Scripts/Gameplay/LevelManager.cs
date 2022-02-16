using System.Collections.Generic;
using Sirenix.OdinInspector;
using SquareDino.Scripts.MyAnalytics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event System.Action OnLevelCompleted;
    public event System.Action OnLevelNotPassed;

    public event System.Action OnLevelLoaded;

    [Header("Settings")] [InlineEditor(InlineEditorObjectFieldModes.Foldout)] [SerializeField]
    private LevelContainer levelContainer;

    [SerializeField] private Transform levelParent;
    [SerializeField] private bool levelRandomizerAfterAllLevelsCompleted;

    [SerializeField, Sirenix.OdinInspector.ReadOnly]
    private int currentLevelNumber;

    private readonly RandomNoRepeate randomLevelNumber = new RandomNoRepeate();

    public Level CurrentLevel { get; private set; }
    private Level _oldLevel;

    public List<Level> Levels => levelContainer.Levels;

    public void LoadLevel()
    {
        UnloadLevel();
        currentLevelNumber = Statistics.CurrentLevelNumber;

        if (levelContainer.IsDebug)
        {
            CurrentLevel = Instantiate(levelContainer.DebugLevel, levelParent);
        }
        else
        {
            CurrentLevel = Instantiate(levelContainer.Levels[currentLevelNumber], levelParent);
        }

        CurrentLevel.transform.position = levelParent.position;
        CurrentLevel.OnLevelLoaded += LevelLoaded;
    }

    public void LoadLevel(int id)
    {
        UnloadLevel();
        CurrentLevel = Instantiate(levelContainer.Levels[id], levelParent);
        CurrentLevel.transform.position = levelParent.position;
        CurrentLevel.OnLevelLoaded += LevelLoaded;
    }

    public void LoadLevel(int id, float oldLevelDelay)
    {
        Invoke(nameof(DelayDestroyLevel), oldLevelDelay);
        _oldLevel = CurrentLevel;
        CurrentLevel = Instantiate(levelContainer.Levels[id], levelParent);
        CurrentLevel.transform.position = levelParent.position;
        //CurrentLevel.gameObject.SetActive(false);
        //CurrentLevel.transform.position = levelParent.position;
        //CurrentLevel.transform.rotation = _oldLevel.NextLevelRotation;
        //CurrentLevel.transform.position = _oldLevel.NextLevelPosition;
        //CurrentLevel.gameObject.SetActive(true);
        //print(CurrentLevel.transform.position);
        CurrentLevel.OnLevelLoaded += LevelLoaded;
    }

    private void DelayDestroyLevel()
    {
        if (_oldLevel == null) return;
        Destroy(_oldLevel.gameObject);
    }

    public void UnloadLevel(bool editor = false)
    {
        if (CurrentLevel != null)
        {
            CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
            if (editor)
                DestroyImmediate(CurrentLevel.gameObject);
            else
                Destroy(CurrentLevel.gameObject);
        }
    }

    private void IncreaseLevelNumber()
    {
        Statistics.PlayerLevel++;

        if (Statistics.AllLevelsCompleted && levelRandomizerAfterAllLevelsCompleted)
        {
            currentLevelNumber = randomLevelNumber.GetAvailable();
        }
        else
        {
            if (currentLevelNumber <= levelContainer.Levels.Count - 2)
            {
                currentLevelNumber++;
            }
            else
            {
                currentLevelNumber = levelRandomizerAfterAllLevelsCompleted ? randomLevelNumber.GetAvailable() : 0;
                Statistics.AllLevelsCompleted = true;
            }
        }

        Statistics.CurrentLevelNumber = currentLevelNumber;
    }

    private void LevelLoaded()
    {
        CurrentLevel.OnLevelLoaded -= LevelLoaded;
        CurrentLevel.OnLevelCompleted += CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing += CurrentLevel_OnLevelLosing;
        OnLevelLoaded?.Invoke();
    }

    private void CurrentLevel_OnLevelLosing()
    {
        CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;
        OnLevelNotPassed?.Invoke();
    }

    private void CurrentLevel_OnLevelCompleted()
    {
        CurrentLevel.OnLevelCompleted -= CurrentLevel_OnLevelCompleted;
        CurrentLevel.OnLevelLosing -= CurrentLevel_OnLevelLosing;
        IncreaseLevelNumber();
        OnLevelCompleted?.Invoke();
    }

    private void Start()
    {
        randomLevelNumber.Init(levelContainer.Levels.Count);
        MyAnalyticsManager.SetUniqueLevelsCount(levelContainer.Levels.Count);
        MyAnalyticsManager.SetLevelsRandomizer(levelRandomizerAfterAllLevelsCompleted);
    }
}