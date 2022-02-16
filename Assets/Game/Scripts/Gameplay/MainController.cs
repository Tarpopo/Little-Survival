using SquareDino;
using UnityEngine;
using SquareDino.Scripts.MyAds;
using SquareDino.Scripts.MyAnalytics;
using SquareDino.Scripts.RateUsPopUp;

public class MainController : MonoBehaviour
{
    [SerializeField] private SceneUI sceneUI;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ParticleSystem fireworksParticleSystem;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        sceneUI.GameWindow.TapToPlayButton.OnClick += StartLevel;
        sceneUI.GameWindow.RestartButton.OnClick += RestartGame;
        sceneUI.VictoryWindow.ContinueButton.OnClick += NextLevel;
        sceneUI.LosingWindow.RestartButton.OnClick += RestartGame;

        levelManager.OnLevelLoaded += LevelManager_OnLevelLoaded;
        levelManager.OnLevelCompleted += LevelManager_OnLevelCompleted;
        levelManager.OnLevelNotPassed += LevelManager_OnLevelNotPassed;

        LoadLevel();
    }

    private void LoadLevel()
    {
        levelManager.LoadLevel();
    }

    private void NextLevel()
    {
        MoneyHandler.SaveMoneyData();
        MyAdsManager.InterstitialShow(InterstitialClosed, "Next Level");
    }

    private void RestartGame()
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;
        sceneUI.LosingWindow.Enable(false);

        MyAnalyticsManager.LevelRestart();
        MyAdsManager.InterstitialShow(InterstitialClosed, "Restart");
    }

    private void InterstitialClosed()
    {    
        levelManager.LoadLevel();
    }

    private void LevelManager_OnLevelLoaded()
    {
        fireworksParticleSystem.Stop();
        fireworksParticleSystem.gameObject.SetActive(false);

        levelManager.CurrentLevel.OnProgressUpdated += CurrentLevel_OnProgressUpdated;

        sceneUI.VictoryWindow.Enable(false);
        sceneUI.GameWindow.Enable(true);
        sceneUI.GameWindow.TapToPlayButton.gameObject.SetActive(true);
        MyAnalyticsManager.LevelStart(Statistics.CurrentLevelNumber.ToString());
    }

    private void StartLevel()
    {
        sceneUI.GameWindow.TapToPlayButton.gameObject.SetActive(false);

        levelManager.CurrentLevel.StartGameProcess();
    }

    private void CurrentLevel_OnProgressUpdated(LevelProgress levelProgress)
    {
        sceneUI.GameWindow.UpdateProgressBar(levelProgress);
    }

    private void LevelManager_OnLevelCompleted()
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;

        fireworksParticleSystem.gameObject.SetActive(true);
        if (!fireworksParticleSystem.isPlaying) fireworksParticleSystem.Play();

        sceneUI.GameWindow.Enable(false);
        sceneUI.VictoryWindow.Enable(true);

        RateUsManager.IncreaseNumberOfSessions();
        MyVibration.Haptic(MyHapticTypes.Selection);
        MyAnalyticsManager.LevelWin();
    }

    private void LevelManager_OnLevelNotPassed()
    {
        levelManager.CurrentLevel.OnProgressUpdated -= CurrentLevel_OnProgressUpdated;

        sceneUI.GameWindow.Enable(false);
        sceneUI.LosingWindow.Enable(true);

        MyVibration.Haptic(MyHapticTypes.Selection);
        MyAnalyticsManager.LevelFailed();
    }
}