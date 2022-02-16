using UnityEngine;

#if FLAG_HOMA
using HomaGames.HomaBelly;
#endif

namespace SquareDino.Scripts.Publishers
{
    public class MyHoma : MonoBehaviour
    {
        private static MyHoma instance;
        private string _mediationName;
        
        [SerializeField] private bool bannerAutoShow;

        private string _rewardedCurrentName;

#if FLAG_HOMA

        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            _mediationName = GetType().Name;

            BannerCallbacks();
            InterstitialCallbacks();
            RewardedCallbacks();
            
            if (!HomaBelly.Instance.IsInitialized)
            {
                Events.onInitialized += () =>
                {
                    HomaBelly.Instance.LoadBanner(BannerPosition.BOTTOM);
                };
            }
            else
            {
                HomaBelly.Instance.LoadBanner(BannerPosition.BOTTOM);
            }
        }

        private void Start()
        {
            MyAdsManager.InterstitialShowAction += InterstitialShow;
            MyAdsManager.RewardedShowAction += RewardedShow;
            MyAdsManager.BannerShowAction += BannerShow;
            MyAdsManager.BannerHideAction += BannerHide;

            // !!! TEMP !!! //
            // MyAdsManager.InterstitialLoaded();
        }

        #region Banner
        
        private void BannerCallbacks()
        {
            Events.onBannerAdLoadedEvent += OnBannerAdLoadedEvent;
        }
        private void BannerHide()
        {
            HomaBelly.Instance.HideBanner();
        }

        private void BannerShow()
        {
            HomaBelly.Instance.ShowBanner();
        }

        private void OnBannerAdLoadedEvent(string obj)
        {
            if (bannerAutoShow) HomaBelly.Instance.ShowBanner();
            // bannerAutoShow = false;
        }
        #endregion

        #region Interstitial

        private void InterstitialShow(string placeName = "default")
        {
            var available = HomaBelly.Instance.IsInterstitialAvailable();
            if (available) HomaBelly.Instance.ShowInsterstitial();
            MyAdsManager.InterstitialShowFeedback(available, placeName, _mediationName);
        }

        private void InterstitialCallbacks()
        {
            Events.onInterstitialAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            /// Invoked when the Interstitial is Ready to shown after load function is called
            Events.onInterstitialAdReadyEvent += OnInterstitialAdReadyEvent;
            /// Invoked when the initialization process has failed.
            ///  @param description - string - contains information about the failure.
            // Events.onInterstitialAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
            
            /// Invoked right before the Interstitial screen is about to open.
            // HomaGames.HomaBelly.Events.onInterstitialAdShowSucceededEvent += OnInterstitialAdShowSucceededEvent;
            
            /// Invoked when the ad fails to show.
            /// @param description - string - contains information about the failure.
            Events.onInterstitialAdShowFailedEvent += OnInterstitialAdShowFailedEvent;
            
            /// Invoked when end user clicked on the interstitial ad
            // HomaGames.HomaBelly.Events.onInterstitialAdClickedEvent += OnInterstitialAdClickedEvent;
            
            /// Invoked when the Interstitial Ad Unit has opened
            // HomaGames.HomaBelly.Events.onInterstitialAdOpenedEvent += OnInterstitialAdOpenedEvent;
            
            /// Invoked when the interstitial ad closed and the user goes back to the application screen.
            Events.onInterstitialAdClosedEvent += OnInterstitialAdClosedEvent;
        }

        private void OnInterstitialAdShowFailedEvent(string obj)
        {
            Debug.Log("OnInterstitialAdShowFailedEvent " + obj);
            MyAdsManager.Instance.InterstitialClosed();
        }

        private void OnInterstitialLoadFailedEvent()
        {
            Debug.Log("OnInterstitialLoadFailedEvent");
        }

        private void OnInterstitialAdReadyEvent()
        {
            MyAdsManager.InterstitialLoaded();
        }

        private void OnInterstitialAdClosedEvent()
        {
            MyAdsManager.Instance.InterstitialClosed();
        }

        #endregion
        
        #region Rewarded

        private void RewardedShow(string placeName)
        {
            _rewardedCurrentName = placeName;
            var available = HomaBelly.Instance.IsRewardedVideoAdAvailable();
            if (available) HomaBelly.Instance.ShowRewardedVideoAd();
            MyAdsManager.RewardedShowFeedback(available, placeName, _mediationName);
        }

        private void RewardedCallbacks()
        {
            /// Invoked when the RewardedVideo ad view has opened
            // Events.onRewardedVideoAdStartedEvent += onRewardedVideoAdStartedEvent;
            
            /// Invoked when the RewardedVideo ad view is about to be closed.
            Events.onRewardedVideoAdClosedEvent += OnRewardedVideoAdClosedEvent;
            Events.onRewardedVideoAdClickedEvent += EventsOnonRewardedVideoAdClickedEvent;
            /// Invoked when there is a change in the ad availability status.
            /// @param - available - value will change to true when rewarded videos are available
            Events.onRewardedVideoAvailabilityChangedEvent += OnRewardedVideoAvailabilityChangedEvent;
            
            /// Invoked when the video ad starts playing. 
            // Events.onRewardedVideoAdStartedEvent += OnRewardedVideoAdStartedEvent;
            
            /// Invoked when the video ad finishes playing.
            // Events.onRewardedVideoAdEndedEvent += OnRewardedVideoAdEndedEvent;
            
            /// Invoked when the user completed the video and should be rewarded.
            Events.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
            
            /// Invoked when the Rewarded Video failed to show
            Events.onRewardedVideoAdShowFailedEvent += OnRewardedVideoAdShowFailedEvent;
        }

        private void OnRewardedVideoAdShowFailedEvent(string obj)
        {
            Debug.Log("OnRewardedVideoAdShowFailedEven " + obj);
            MyAdsManager.Instance.RewardedClosed(false, _rewardedCurrentName);
        }
        
        private void EventsOnonRewardedVideoAdClickedEvent(string obj)
        {
            MyAdsManager.RewardedClicked();
        }

        private void OnRewardedVideoAdRewardedEvent(VideoAdReward obj)
        {
            MyAdsManager.RewardedReward();
        }

        private void OnRewardedVideoAvailabilityChangedEvent(bool flag, string arg2)
        {
            MyAdsManager.RewardedLoaded(flag);
        }

        private void OnRewardedVideoAdClosedEvent(string obj)
        {
            Debug.Log("OnRewardedVideoAdClosedEvent");
            MyAdsManager.Instance.RewardedClosed(true, _rewardedCurrentName);
        }
    
        #endregion
#endif
    }
}