using UnityEngine;

public class Level : MonoBehaviour
{
    public event System.Action OnLevelLoaded;
    public event System.Action OnLevelCompleted;
    public event System.Action OnLevelLosing;
    public event System.Action<LevelProgress> OnProgressUpdated;
    public Vector3 NextLevelPosition => _nextLevelPoint.localPosition;
    public Quaternion NextLevelRotation => _nextLevelPoint.rotation;
    [SerializeField] private Transform _nextLevelPoint;
    [SerializeField] private SampleLevelObject winSampleObject;
    [SerializeField] private SampleLevelObject loseSampleObject;
    [SerializeField] private GameObject helperText;

    private bool levelStarted;

    private LevelProgress levelProgress;

    public void StartGameProcess()
    {
        // Старт игрового процесса
        levelStarted = true;
        winSampleObject.OnClick += WinSampleObject_OnClick;
        loseSampleObject.OnClick += LoseSampleObject_OnClick;
    }

    private void Start()
    {
        // Сообщает, что уровень загружен
        OnLevelLoaded?.Invoke();

        // Задает начальные значения для прогресса уровня
        levelProgress = new LevelProgress(0, 0, 100);
    }

    private void WinSampleObject_OnClick()
    {
        // Победа
        winSampleObject.OnClick -= WinSampleObject_OnClick;
        loseSampleObject.OnClick -= LoseSampleObject_OnClick;
        helperText.SetActive(false);

        OnLevelCompleted?.Invoke();
    }

    private void LoseSampleObject_OnClick()
    {
        // Проигрыш
        winSampleObject.OnClick -= WinSampleObject_OnClick;
        loseSampleObject.OnClick -= LoseSampleObject_OnClick;

        OnLevelLosing?.Invoke();
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Удачно завершаем уровень
        if (Input.GetKeyDown(KeyCode.Space)) OnLevelCompleted?.Invoke();
        // Неудачно завершаем уровень
        if (Input.GetKeyDown(KeyCode.Backspace)) OnLevelLosing?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Изменяет прогресс прохождения уровня
            levelProgress.CurrentValue = 10f;
            // Сообщает, что прогресс обновился
            OnProgressUpdated?.Invoke(levelProgress);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Изменяет прогресс прохождения уровня
            levelProgress.CurrentValue = 20f;
            // Сообщает, что прогресс обновился
            OnProgressUpdated?.Invoke(levelProgress);
        }
    }
#endif
}

public struct LevelProgress
{
    public float CurrentValue;
    private readonly float _minValue;
    private readonly float _maxValue;

    public LevelProgress(float currentValue, float minValue, float maxValue)
    {
        CurrentValue = currentValue;
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public float Progress => (CurrentValue - _minValue) / (_maxValue - _minValue);
}