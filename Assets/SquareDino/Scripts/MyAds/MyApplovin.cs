using UnityEngine;
using UnityEngine.Serialization;

#if FLAG_APPLOVIN
using SquareDino.Scripts.MyAnalytics;
#endif

namespace SquareDino.Scripts.MyAds
{
    public class MyApplovin : MonoBehaviour
    {
        private static MyApplovin instance;
        private string _mediationName;
        
        [SerializeField] private bool isDebug;
        [SerializeField] private string maxSdkKey = "6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR";
        [Space(10)]
        [SerializeField] private string iOSInterID;
        [SerializeField] private string iOSRewardID;
        [SerializeField] private string iOSBannerID;
        [FormerlySerializedAs("andorindInterId")]
        [Space(10)]
        [SerializeField] private string androidInterId;
        [SerializeField] private string androidRewardId;
        [SerializeField] private string androidBannerId;
        
        [SerializeField] private bool bannerAutoShow;

        private static string _interstitialAdUnitId;
        private static string _rewardedAdUnitId;
        private static string _bannerAdUnitId;

        private int _bannerLoadCounter;
        
#if FLAG_APPLOVIN
        
        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            _mediationName = GetType().Name;
#if UNITY_ANDROID
        _interstitialAdUnitId = andorindInterId;
        _rewardedAdUnitId = androidRewardId;
        _bannerAdUnitId = androidBannerId;
#elif UNITY_IOS
            _interstitialAdUnitId = iOSInterID;
            _rewardedAdUnitId = iOSRewardID;
            _bannerAdUnitId = iOSBannerID;
#endif

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
                // AppLovin SDK is initialized, start loading ads
                if (isDebug) MaxSdk.ShowMediationDebugger();
                InitializeInterstitialAds();
                InitializeRewardedAds();
                InitializeBanner();
            };
            MaxSdk.SetSdkKey(maxSdkKey);
            MaxSdk.InitializeSdk();      
            MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
        }

        private void OnEnable()
        {
            MyAdsManager.InterstitialShowAction += InterstitialShow;
            MyAdsManager.RewardedShowAction += RewardedShow;
            MyAdsManager.BannerShowAction += BannerShow;
            MyAdsManager.BannerHideAction += BannerHide;
        }

        private void OnDisable()
        {
            MyAdsManager.InterstitialShowAction -= InterstitialShow;
            MyAdsManager.RewardedShowAction -= RewardedShow;
            MyAdsManager.BannerShowAction -= BannerShow;
            MyAdsManager.BannerHideAction -= BannerHide;
        }

        private void BannerHide()
        {
            if (_bannerAdUnitId == "") return;
            MaxSdk.HideBanner(_bannerAdUnitId);
        }

        private void BannerShow()
        {
            if (_bannerAdUnitId == "") return;
            MaxSdk.ShowBanner(_bannerAdUnitId);
        }

        private void InitializeBanner()
        {
            if (_bannerAdUnitId == "") return;
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
            MaxSdk.CreateBanner(_bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerLoadedEvent;
            // Set background or background color for banners to be fully functional
            // MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, <YOUR_BANNER_BACKGROUND_COLOR>);

            if (bannerAutoShow)
            {
                MaxSdk.ShowBanner(_bannerAdUnitId);
                MyAnalyticsManager.BannerShow();
            }
        }
        
        private void OnBannerLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            ++_bannerLoadCounter;
            if (_bannerLoadCounter > 1)
            {
                MyAnalyticsManager.BannerClosed();
                MyAnalyticsManager.BannerShow();
            }
        }

        #region Interstitial

        private void InterstitialShow(string placeName = "default")
        {
            var available = MaxSdk.IsInterstitialReady(_interstitialAdUnitId);
            if (available) MaxSdk.ShowInterstitial(_interstitialAdUnitId);
            MyAdsManager.InterstitialShowFeedback(available, _mediationName);
        }

        int retryAttempt;

        private void InitializeInterstitialAds()
        {
            // Attach callback
            MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        private void LoadInterstitial()
        {
            if (_interstitialAdUnitId != "") MaxSdk.LoadInterstitial(_interstitialAdUnitId);
        }

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
            MyAdsManager.InterstitialLoaded();
            // Reset retry attempt
            retryAttempt = 0;
        }

        private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
        {
            // Interstitial ad failed to load. We recommend retrying with exponentially higher delays.

            retryAttempt++;
            double retryDelay = Mathf.Pow(2, retryAttempt);

            Invoke("LoadInterstitial", (float)retryDelay);
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            LoadInterstitial();
        }

        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            MyAdsManager.Instance.InterstitialClosed();
            LoadInterstitial();
        }
    
        #endregion
        
        #region Rewarded

        private void RewardedShow(string placeName)
        {
            var available = MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);
            if (available) MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
            MyAdsManager.RewardedShowFeedback(available, _mediationName);
        }

        private void InitializeRewardedAds()
        {
            // Attach callback
            MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }

        private void LoadRewardedAd()
        {
            if (_rewardedAdUnitId == "") return;
            MyAdsManager.RewardedLoaded(false);
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }

        private void OnRewardedAdLoadedEvent(string adUnitId)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'

            // Reset retry attempt
            retryAttempt = 0;
            
            MyAdsManager.RewardedLoaded(true);
        }

        private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
        {
            // Rewarded ad failed to load. We recommend retrying with exponentially higher delays.
            MyAdsManager.RewardedLoaded(false);
            retryAttempt++;
            double retryDelay = Mathf.Pow(2, retryAttempt);

            Invoke("LoadRewardedAd", (float)retryDelay);
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            MyAdsManager.Instance.RewardedClosed(false);
            
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId) { }

        private void OnRewardedAdClickedEvent(string adUnitId) {
            MyAdsManager.RewardedClicked();
        }

        private void OnRewardedAdDismissedEvent(string adUnitId)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            MyAdsManager.Instance.RewardedClosed(true);
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
        {
            // Rewarded ad was displayed and user should receive the reward
            MyAdsManager.RewardedReward();
        }
    
        #endregion
#endif
    }
}