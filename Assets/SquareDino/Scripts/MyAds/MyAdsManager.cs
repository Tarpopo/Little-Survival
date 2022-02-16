using System;
using System.Collections;
using UnityEngine;
using SquareDino.Scripts.MyAnalytics;

namespace SquareDino.Scripts.MyAds
{
    public class MyAdsManager : MonoBehaviour
    {
        public static MyAdsManager Instance;

        private static bool _isInterstitialLoaded;
        private static bool _isRewardedLoaded;

        public static Action BannerShowAction = delegate {};
        public static Action BannerHideAction = delegate {};
        
        public static Action<string> InterstitialShowAction = delegate {};
        private static Action _interstitialClosed;
        
        private static event Action <bool> OnRewardedLoaded = delegate {};
        public static Action <string> RewardedShowAction = delegate {};
        private static Action <bool> _rewardedClosed;

        private static bool _isShowingAds;
        private static bool _interstitialTimerOver;
        
        [SerializeField] private bool enableAds = true;
        [SerializeField] private float interstitialFirstTimer = 60f;
        [SerializeField] private float interstitialLoopTimer = 40f;
        [SerializeField] private bool interstitialAutoShow;
        private static bool _gotReward;
        private static bool _rewardedClicked;
        private static string _rewardedPlaceName;
        private static string _interstitialPlaceName;

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
        }

        private void Start()
        {
            StopAllCoroutines();
            StartCoroutine(InterstitialShowCoroutine(interstitialFirstTimer));
        }

        #region Interstitial
        
        public static void InterstitialLoaded()
        {
            Debug.Log("[MyAds] InterstitialLoaded");
            _isInterstitialLoaded = true;
        }

        private IEnumerator InterstitialShowCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            _interstitialTimerOver = true;
            if (interstitialAutoShow) InterstitialShow(null);
        }

        public static void InterstitialShow(Action interstitialClosed, string placeName = "default")
        {
            Debug.Log("[MyAds] InterstitialShow " + "isShowingAds - " + _isShowingAds + " interstitialTimerOver - " +
                      _interstitialTimerOver);
            if (_isShowingAds || !_isInterstitialLoaded || !_interstitialTimerOver)
            {
                interstitialClosed?.Invoke();
                return;
            }
            
            if (interstitialClosed != null) _interstitialClosed = interstitialClosed;
            
            _isShowingAds = true;
            _isInterstitialLoaded = false;
            _interstitialTimerOver = false;
            InterstitialShowAction(placeName);
            MyAnalyticsManager.InterstitialShow(placeName);
            _interstitialPlaceName = placeName;
        }

        public static void InterstitialShowFeedback(bool result, string mediatorName = "default")
        {
            
            Debug.Log("[" + mediatorName + "] InterstitialShowFeedback - isReady " + result);
            if (result)
            {
                MyAnalyticsManager.AdsEvent("video_ads_available", "interstitial", _interstitialPlaceName,
                    "success",
                    Application.internetReachability != NetworkReachability.NotReachable);
                MyAnalyticsManager.AdsEvent("video_ads_started", "interstitial", _interstitialPlaceName, "start",
                    Application.internetReachability != NetworkReachability.NotReachable);
                MyAnalyticsManager.AdsEvent("video_ads_watch", "interstitial", _interstitialPlaceName, "watched",
                    Application.internetReachability != NetworkReachability.NotReachable);
            }
            else
            {
                Instance.InterstitialClosed();
                
                MyAnalyticsManager.AdsEvent("video_ads_available", "interstitial", _interstitialPlaceName, "not_available",
                    Application.internetReachability != NetworkReachability.NotReachable);
            }
        }
        
        public void InterstitialClosed()
        {
            Debug.Log("[MyAds] InterstitialClosed");
            MyAnalyticsManager.InterstitialClosed(_interstitialPlaceName);

            _isShowingAds = false;
            _interstitialTimerOver = false;
            _interstitialClosed?.Invoke();
            _interstitialClosed = null;
            StopAllCoroutines();
            StartCoroutine(InterstitialShowCoroutine(interstitialLoopTimer));
        }

        public void FailedToShow(string errorCode)
        {
            _isShowingAds = false;
            Debug.Log("[MyAds] " + errorCode);
        }

        #endregion
        
        #region Rewarded

        public static void RewardedLoaded(bool loaded)
        {
            Debug.Log("[MyAds] RewardedLoaded() - " + loaded);
            _isRewardedLoaded = loaded;
            OnRewardedLoaded(loaded);
        }

        public static void RewardedEnable(Action <bool> rewardedAvailable)
        {
            OnRewardedLoaded += rewardedAvailable;
            OnRewardedLoaded(_isRewardedLoaded);
        }
        
        public static void RewardedDisable(Action <bool> rewardedAvailable)
        {
            OnRewardedLoaded -= rewardedAvailable;
        }
        
        public static void RewardedShow(Action <bool> rewardedClosed = null, string placeName = "default")
        {
            Debug.Log("[MyAds] RewardedShow " + "isShowingAds = " + _isShowingAds + " _isRewardedLoaded = " + _isRewardedLoaded);
            if (_isShowingAds || !_isRewardedLoaded)
            {
                rewardedClosed?.Invoke(false);
                return;
            }
            if (rewardedClosed != null) _rewardedClosed = rewardedClosed;

            Debug.Log("[MyAds] RewardedShowAction(" + "placeName = " + placeName + ")");
            _isShowingAds = true;
            _interstitialTimerOver = false;
            RewardedLoaded(false);
            _rewardedPlaceName = placeName;
            RewardedShowAction(_rewardedPlaceName);
            MyAnalyticsManager.RewardedShow(_rewardedPlaceName);
        }
        
        public static void RewardedShowFeedback(bool available, string mediatorName = "default")
        {
            Debug.Log("[" + mediatorName + "] RewardedShowFeedback - isReady " + available);
            if (available)
            {
                MyAnalyticsManager.AdsEvent("video_ads_available", "rewarded", _rewardedPlaceName, "success",
                    Application.internetReachability != NetworkReachability.NotReachable);
                MyAnalyticsManager.AdsEvent("video_ads_started", "rewarded", _rewardedPlaceName, "start",
                    Application.internetReachability != NetworkReachability.NotReachable);
            }
            else
            {
                Instance.RewardedClosed(false);

                MyAnalyticsManager.AdsEvent("video_ads_available", "rewarded", _rewardedPlaceName, "not_available",
                    Application.internetReachability != NetworkReachability.NotReachable);
            }
        }
        
        public static void RewardedReward()
        {
            _gotReward = true;
        }
        
        public static void RewardedClicked()
        {
            _rewardedClicked = true;
        }
        
        public void RewardedClosed(bool available)
        {
            Debug.Log("[MyAds] RewardedClosed");
            
            string result;
            if (_rewardedClicked) result = "clicked";
            else result = _gotReward ? "watched" : "canceled";
            
            if (available)
                MyAnalyticsManager.AdsEvent("video_ads_watch", "rewarded", _rewardedPlaceName, result,
                    Application.internetReachability != NetworkReachability.NotReachable);

            MyAnalyticsManager.RewardedClosed(_rewardedPlaceName, _gotReward);

            _isShowingAds = false;
            _interstitialTimerOver = false;
            
            _rewardedClosed?.Invoke(_gotReward);
            _rewardedClosed = null;
            
                
            _gotReward = false;
            _rewardedClicked = false;

            StopAllCoroutines();
            StartCoroutine(InterstitialShowCoroutine(interstitialLoopTimer)); 
        }

        #endregion
        
        #region Banner

        public static void BannerShow()
        {
            BannerShowAction();
            MyAnalyticsManager.BannerShow();
        }
        
        public static void BannerHide()
        {
            BannerHideAction();
            MyAnalyticsManager.BannerClosed();
        }
        #endregion
    }
}