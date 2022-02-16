using System;
using UnityEngine;
using System.Collections.Generic;

#if FLAG_GA
        using GameAnalyticsSDK;
#endif
#if FLAG_HOMA
        using HomaGames.HomaBelly;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
    public class MyAnalyticsManager : MonoBehaviour
    {
        private static MyAnalyticsManager _instance;

        public static event Action OnLevelStart = delegate { };
        public static event Action OnLevelFinish = delegate { };

        private static int _totalLevelStartCounter = 1;
        private static int _totalUniqueLevelsCount = int.MaxValue;
        private static int _levelIncrementalNumber;
        private static string _levelCurrentName;
        private static string _levelDifficulty;
        private static bool _levelRandomizer = true;
        private static string _levelPreviousNumber;
        private static int _levelLoop;
        private static bool _levelRandom;
        private static bool _isRestart;
        private static string _levelType;
        private static int _sessionCounter;
        private bool _isGameStarted;
        private static bool _isLevelStarted;

        private void Awake()
        {
            if (_instance != null) return;
            _instance = this;
            _levelIncrementalNumber = PlayerPrefs.GetInt("Analytics_LevelIncrementalNumber");
            _totalLevelStartCounter = PlayerPrefs.GetInt("Analytics_TotalLevelStartCounter");
            _levelPreviousNumber = PlayerPrefs.GetString("Analytics_LevelPreviousNumber");
            _sessionCounter = PlayerPrefs.GetInt("Analytics_SessionCounter");
            PlayerPrefs.SetInt("Analytics_SessionCounter", ++_sessionCounter);
        }

        private void Start()
        {
            // #if FLAG_HOMA
            //         HomaBelly.DefaultAnalytics.GameplayStarted();
            // #endif

            if (_isGameStarted) return;
            _isGameStarted = true;
        }

        public static void SetUniqueLevelsCount(int value)
        {
            _totalUniqueLevelsCount = value;
        }

        public static void SetLevelsRandomizer(bool value)
        {
            _levelRandomizer = value;
        }

        public static void LevelRestart()
        {
            _isRestart = true;
            #if FLAG_WANNAPLAY
                AmplitudeHelper.Inctance.LevelRestart(_levelIncrementalNumber);                      
            #endif
        }

        public static void LevelStart(string levelRealName, string levelType = "default", string levelDiff = "easy")
        {
            _isLevelStarted = true;
            _levelCurrentName = levelRealName;
            _levelDifficulty = levelDiff;
            _levelType = levelType;

            PlayerPrefs.SetInt("Analytics_TotalLevelStartCounter", ++_totalLevelStartCounter);
            if (_levelPreviousNumber != _levelCurrentName)
            {
                _levelPreviousNumber = _levelCurrentName;
                PlayerPrefs.SetString("Analytics_LevelPreviousNumber", _levelPreviousNumber);
                PlayerPrefs.SetInt("Analytics_LevelIncrementalNumber", ++_levelIncrementalNumber);
            }

            _levelLoop = _levelIncrementalNumber / _totalUniqueLevelsCount + 1;
            _levelRandom = _levelRandomizer & (_levelIncrementalNumber > _totalUniqueLevelsCount);
            Debug.Log("Analytics - LevelStart:"
                      + " currentLevel: " + _levelIncrementalNumber
                      + " levelName: " + _levelCurrentName
                      + " totalLevelsPlayed: " + _totalLevelStartCounter
                      + " levelLoop: " + _levelLoop
                      + " levelDiff: " + _levelDifficulty
                      + " levelRandom: " + _levelRandom);
            OnLevelStart();

#if FLAG_GA
                                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + _levelIncrementalNumber);
#endif
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("level_start",
                                        new Dictionary<string, object> {
                                                {"level_number", _levelIncrementalNumber},
                                                {"level_name", _levelCurrentName},
                                                {"level_count", _totalLevelStartCounter},
                                                {"level_diff", _levelDifficulty},
                                                {"level_loop", _levelLoop},
                                                {"level_random", _levelRandom},
                                                {"level_type", _levelType}
                                                });
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#if FLAG_VOODOO
                                TinySauce.OnGameStarted(_levelIncrementalNumber.ToString());
#endif
            // #if FLAG_HOMA
            //         HomaBelly.DefaultAnalytics.LevelStarted(_levelIncrementalNumber.ToString());
            // #endif
#if FLAG_HOOPSLY
                                HoopslyIntegration.Instance.RaiseLevelStartEvent(_levelIncrementalNumber.ToString(), true);
#endif

#if FLAG_FIREBASE
                                Firebase.Analytics.Parameter[] parameters = {
                                        new Firebase.Analytics.Parameter("Number", _levelIncrementalNumber),
                                        new Firebase.Analytics.Parameter("State", _isRestart?"Restart":"Start")
                                };
                                Firebase.Analytics.FirebaseAnalytics.LogEvent("Level_Start", parameters);
#endif
            
#if FLAG_WANNAPLAY
                                if (!_isRestart) AmplitudeHelper.Inctance.LevelStarted(_levelIncrementalNumber);
#endif

            if (_isRestart)
            {
                _isRestart = false;
                _levelRandom = false;
            }
        }

        public static void LevelFailed(int progress = 0, int score = 0)
        {
            var result = "lose";
            if (_isRestart) result = "restart";
            Debug.Log("Analytics - LevelFailed:"
                      + " currentLevel: " + _levelIncrementalNumber
                      + " levelName: " + _levelCurrentName
                      + " totalLevelsPlayed: " + _totalLevelStartCounter
                      + " levelDiff: " + _levelDifficulty
                      + " levelLoop: " + _levelLoop
                      + " levelRandom: " + _levelRandom
                      + " score: " + score
                      + " time: " + LevelLengthTimer.Value
                      + " progress: " + progress
                      + "Result: " + result);
            OnLevelFinish();
#if FLAG_GA
                                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level" + _levelIncrementalNumber, score);
                                GameAnalytics.NewDesignEvent("Levels:Duration:" + _levelIncrementalNumber, LevelLengthTimer.Value);
#endif
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("level_finish",
                                        new Dictionary<string, object> {
                                                {"level_number", _levelIncrementalNumber},
                                                {"level_name", _levelCurrentName},
                                                {"level_count", _totalLevelStartCounter},
                                                {"level_diff", _levelDifficulty},
                                                {"level_loop", _levelLoop},
                                                {"level_random", _levelRandom},
                                                {"result", "lose"},
                                                {"time", LevelLengthTimer.Value},
                                                {"progress", progress},
                                                {"level_type", _levelType}});
                                                
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#if FLAG_VOODOO
                                TinySauce.OnGameFinished(_levelIncrementalNumber.ToString(), false, progress);
#endif
            // #if FLAG_HOMA
            //         HomaBelly.DefaultAnalytics.LevelFailed();
            // #endif
#if FLAG_HOOPSLY
                                HoopslyIntegration.Instance.RaiseLevelFinishedEvent(_levelIncrementalNumber.ToString(), LevelFinishedResult.lose, (int)LevelLengthTimer.Value);
#endif
#if FLAG_FIREBASE
                            Firebase.Analytics.Parameter[] parameters = {
                                    new Firebase.Analytics.Parameter("Number", _levelIncrementalNumber),
                                    new Firebase.Analytics.Parameter("State", "Lose")
                            };
                            Firebase.Analytics.FirebaseAnalytics.LogEvent("Level_End", parameters);
#endif
#if FLAG_WANNAPLAY
                            if (_isLevelStarted) AmplitudeHelper.Inctance.LevelFailed(_levelIncrementalNumber);
#endif

            _isLevelStarted = false;
        }

        public static void LevelWin(int score = 0)
        {
            if (_isLevelStarted) 
            Debug.Log("Analytics - LevelWin:"
                      + " currentLevel: " + _levelIncrementalNumber
                      + " levelName: " + _levelCurrentName
                      + " totalLevelsPlayed: " + _totalLevelStartCounter
                      + " score: " + score
                      + " time: " + LevelLengthTimer.Value
                      + " progress: 100"
                      + " levelDiff: " + _levelDifficulty);
            OnLevelFinish();
#if FLAG_GA
                                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level" + _levelIncrementalNumber, score);
                        		GameAnalytics.NewDesignEvent("Levels:Duration:" + _levelIncrementalNumber, LevelLengthTimer.Value);
#endif
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("level_finish",
                                        new Dictionary<string, object> {
                                                {"level_number", _levelIncrementalNumber},
                                                {"level_name", _levelCurrentName},
                                                {"level_count", _totalLevelStartCounter},
                                                {"level_diff", _levelDifficulty},
                                                {"level_loop", _levelLoop},
                                                {"level_random", _levelRandom},
                                                {"result", "win"},
                                                {"time", LevelLengthTimer.Value},
                                                {"progress", 100},
                                                {"level_type", _levelType}});
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#if FLAG_VOODOO
                                TinySauce.OnGameFinished(_levelIncrementalNumber.ToString(), true, score);
#endif
            // #if FLAG_HOMA
            //         HomaBelly.DefaultAnalytics.LevelCompleted();
            // #endif
#if FLAG_HOOPSLY
                                HoopslyIntegration.Instance.RaiseLevelFinishedEvent(_levelIncrementalNumber.ToString(), LevelFinishedResult.win, (int)LevelLengthTimer.Value);
#endif
#if FLAG_FIREBASE
                            Firebase.Analytics.Parameter[] parameters = {
                                    new Firebase.Analytics.Parameter("Number", _levelIncrementalNumber),
                                    new Firebase.Analytics.Parameter("State", "Win")
                            };
                            Firebase.Analytics.FirebaseAnalytics.LogEvent("Level_End", parameters);
#endif
#if FLAG_WANNAPLAY
            if (_isLevelStarted) AmplitudeHelper.Inctance.LevelCompleted(_levelIncrementalNumber);
#endif
            _isLevelStarted = false;
        }

        public static void LevelUp(int newLevel)
        {
            Debug.Log("Analytics - LevelUp: " + newLevel);
#if !UNITY_EDITOR
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("level_up",
                                        new Dictionary<string, object> {
                                                {"level", newLevel}});
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#endif
        }

        public static void Tutorial(string currentStepName)
        {
            Debug.Log("Analytics - Tutorial: " + currentStepName);
#if FLAG_GA
                        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "tutorial: step_name - " + currentStepName);
#endif
#if FLAG_YANDEX_METRICA
                        AppMetrica.Instance.ReportEvent("tutorial",
                                new Dictionary<string, object> {
                                        {"step_name", currentStepName}});
                        AppMetrica.Instance.SendEventsBuffer();
#endif
        }
        
        public static void TutorialStarted()
        {
#if FLAG_WANNAPLAY
            AmplitudeHelper.Inctance.TutorialStarted();
#endif
        }
        
        private static int _stepId;
        public static void TutorialNStarted()
        {
#if FLAG_WANNAPLAY
            AmplitudeHelper.Inctance.TutorialNStarted(++_stepId);
#endif
        }
        
        public static void TutorialCompleted()
        {
#if FLAG_WANNAPLAY
            AmplitudeHelper.Inctance.TutorialCompleted();
#endif
        }

        public static void Log(string name, string param1)
        {
            Debug.Log("Analytics - Log: " + name);
#if !UNITY_EDITOR
#if FLAG_GA
                                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, "param1 " + param1);
#endif
#endif

        }

        public static void LogItemUpgrade(string itemName, string newLevel)
        {
            Debug.Log("Analytics - Log: " + "LogItemUpgrade");
#if !UNITY_EDITOR
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("ItemUpgrade",
                                        new Dictionary<string, object> {
                                                {itemName, newLevel}
                                                });
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#endif
        }

        public static void AdsEvent(string name, string adType, string placement, string result, bool connection)
        {
#if !UNITY_EDITOR
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent(name,
                                        new Dictionary<string, object> {
                                                {"ad_type", adType},
                                                {"placement", placement},
                                                {"result", result},
                                                {"connection", connection}});
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#endif
            Debug.Log("Ads Event: " + name + " " + adType + " " + placement + " " + connection);
        }

        public static void SkinUnlockEvent(string skinName, string unlockType)
        {
#if !UNITY_EDITOR
#if FLAG_YANDEX_METRICA
                                AppMetrica.Instance.ReportEvent("skin_unlock",
                                        new Dictionary<string, object> {
                                                {"skin_Name", skinName},
                                                {"unlock_Type", unlockType}});
                                AppMetrica.Instance.SendEventsBuffer();
#endif
#endif
            Debug.Log("SkinUnlockEvent: " + skinName + " " + unlockType);
        }

        public static void InterstitialShow(string placement)
        {
#if FLAG_WANNAPLAY
    AmplitudeHelper.Inctance.InterstitialStarted(placement);
#endif
            MyFirebase.LogEventAds("Interstitial", placement, "Shown");
        }

        public static void InterstitialClosed(string placement)
        {
#if FLAG_WANNAPLAY
        AmplitudeHelper.Inctance.InterstitialComplete(placement);
#endif
            MyFirebase.LogEventAds("Interstitial", placement, "Finished");
        }

        public static void BannerShow()
        {
#if FLAG_WANNAPLAY
    AmplitudeHelper.Inctance.BannerStarted();
#endif
        }

        public static void BannerClosed()
        {
#if FLAG_WANNAPLAY
    AmplitudeHelper.Inctance.BannerCompleted();
#endif
        }

        public static void RewardedShow(string placement)
        {
#if FLAG_WANNAPLAY
AmplitudeHelper.Inctance.RewardedStarted(placement);
#endif
            MyFirebase.LogEventAds("Rewarded", placement, "Shown");
        }

        public static void RewardedClosed(string placement, bool gotReward)
        {
#if FLAG_WANNAPLAY
if (gotReward) AmplitudeHelper.Inctance.RewardedComplete(placement);
#endif
            MyFirebase.LogEventAds("Rewarded", placement, "Finished");
        }
    }
}