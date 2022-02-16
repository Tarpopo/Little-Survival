using System;
using UnityEngine;

#if FLAG_IRONSOURCE
using System.Collections.Generic;
using SquareDino.Scripts.MyAds;
#endif

namespace SquareDino.Scripts.MyAds
{
    public class MyIronSource : MonoBehaviour
    {
        private static MyIronSource _instance;
        private string _mediationName;
        
        [SerializeField] private string _appKeyIOS;
        [SerializeField] private string _appKeyGooglePlay;
        [SerializeField] private bool isDebug;

        private void Awake()
        {
            if (_instance != null) return;
            _instance = this;
        }

#if FLAG_IRONSOURCE

        public void Start()
        {
        _mediationName = GetType().Name;
    #if UNITY_ANDROID
            var appKey = _appKeyGooglePlay;
    #elif UNITY_IPHONE
            var appKey = _appKeyIOS;
    #else
        string appKey = "unexpected_platform";
    #endif
            
            Debug.Log("unity-script: IronSource.Agent.validateIntegration");
            IronSource.Agent.validateIntegration();
            
            Debug.Log("unity-script: unity version" + IronSource.unityVersion());
            // Debug.Log("GAID" + IronSource.Agent.getAdvertiserId());
            
            IronSource.Agent.setAdaptersDebug(isDebug);
            
            // SDK init
            Debug.Log("unity-script: IronSource.Agent.init");
            IronSource.Agent.init(appKey);
            
            Debug.Log("unity-script: LoadInterstitialButtonClicked");
            IronSource.Agent.loadInterstitial();
            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
            
            
            if (isDebug) IronSource.Agent.validateIntegration();
        }

        private void OnEnable()
        {

            //Add Rewarded Video Events
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;

            //Add Rewarded Video DemandOnly Events
            IronSourceEvents.onRewardedVideoAdOpenedDemandOnlyEvent += RewardedVideoAdOpenedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdClosedDemandOnlyEvent += RewardedVideoAdClosedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdLoadedDemandOnlyEvent += this.RewardedVideoAdLoadedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdRewardedDemandOnlyEvent += RewardedVideoAdRewardedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedDemandOnlyEvent += RewardedVideoAdShowFailedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdClickedDemandOnlyEvent += RewardedVideoAdClickedDemandOnlyEvent;
            IronSourceEvents.onRewardedVideoAdLoadFailedDemandOnlyEvent +=
                this.RewardedVideoAdLoadFailedDemandOnlyEvent;


            // Add Offerwall Events
            IronSourceEvents.onOfferwallClosedEvent += OfferwallClosedEvent;
            IronSourceEvents.onOfferwallOpenedEvent += OfferwallOpenedEvent;
            IronSourceEvents.onOfferwallShowFailedEvent += OfferwallShowFailedEvent;
            IronSourceEvents.onOfferwallAdCreditedEvent += OfferwallAdCreditedEvent;
            IronSourceEvents.onGetOfferwallCreditsFailedEvent += GetOfferwallCreditsFailedEvent;
            IronSourceEvents.onOfferwallAvailableEvent += OfferwallAvailableEvent;


            // Add Interstitial Events
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            // Add Interstitial DemandOnly Events
            IronSourceEvents.onInterstitialAdReadyDemandOnlyEvent += InterstitialAdReadyDemandOnlyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedDemandOnlyEvent += InterstitialAdLoadFailedDemandOnlyEvent;
            IronSourceEvents.onInterstitialAdShowFailedDemandOnlyEvent += InterstitialAdShowFailedDemandOnlyEvent;
            IronSourceEvents.onInterstitialAdClickedDemandOnlyEvent += InterstitialAdClickedDemandOnlyEvent;
            IronSourceEvents.onInterstitialAdOpenedDemandOnlyEvent += InterstitialAdOpenedDemandOnlyEvent;
            IronSourceEvents.onInterstitialAdClosedDemandOnlyEvent += InterstitialAdClosedDemandOnlyEvent;

            // Add Rewarded Interstitial Events
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdRewardedEvent;

            // Add Banner Events
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

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
        
        void OnApplicationPause(bool isPaused)
        {
            Debug.Log("unity-script: OnApplicationPause = " + isPaused);
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void BannerHide()
        {
            IronSource.Agent.hideBanner();
        }

        private void BannerShow()
        {
            IronSource.Agent.displayBanner();
        }

        private void InterstitialShow(string placeName = "default")
        {
            var available = IronSource.Agent.isInterstitialReady();
            if (available) IronSource.Agent.showInterstitial();
            MyAdsManager.InterstitialShowFeedback(available, placeName, _mediationName);
        }
        
        private void RewardedShow(string placeName)
        {
                var available = IronSource.Agent.isRewardedVideoAvailable();
                if (available) IronSource.Agent.showRewardedVideo();
                MyAdsManager.RewardedShowFeedback(available, placeName, _mediationName);
        }
        
        #region RewardedAd

        void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
        {
            Debug.Log("unity-script: I got RewardedVideoAvailabilityChangedEvent, value = " + canShowAd);
            if (canShowAd) MyAdsManager.RewardedLoaded();
        }

        void RewardedVideoAdOpenedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdOpenedEvent");
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
        {
            Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
            MyAdsManager.RewardedReward(_rewardedCurrentName);
        }

        void RewardedVideoAdClosedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdClosedEvent");
            MyAdsManager.instance.RewardedClosed(true);
        }

        void RewardedVideoAdStartedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdStartedEvent");
        }

        void RewardedVideoAdEndedEvent()
        {
            Debug.Log("unity-script: I got RewardedVideoAdEndedEvent");
        }

        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
        {
            Debug.Log("unity-script: I got RewardedVideoAdClickedEvent, name = " + ssp.getRewardName());
        }

        /************* RewardedVideo DemandOnly Delegates *************/

        void RewardedVideoAdLoadedDemandOnlyEvent(string instanceId)
        {
        
            Debug.Log("unity-script: I got RewardedVideoAdLoadedDemandOnlyEvent for instance: " + instanceId);
        }

        void RewardedVideoAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
        {
        
            Debug.Log("unity-script: I got RewardedVideoAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void RewardedVideoAdOpenedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got RewardedVideoAdOpenedDemandOnlyEvent for instance: " + instanceId);
        }

        void RewardedVideoAdRewardedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got RewardedVideoAdRewardedDemandOnlyEvent for instance: " + instanceId);
        }

        void RewardedVideoAdClosedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got RewardedVideoAdClosedDemandOnlyEvent for instance: " + instanceId);
        }

        void RewardedVideoAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
        {
            Debug.Log("unity-script: I got RewardedVideoAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void RewardedVideoAdClickedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got RewardedVideoAdClickedDemandOnlyEvent for instance: " + instanceId);
        }


        #endregion

        #region Interstitial

        void InterstitialAdReadyEvent()
        {
            Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        }

        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        }

        void InterstitialAdShowSucceededEvent()
        {
            Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        }

        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void InterstitialAdClickedEvent()
        {
            Debug.Log("unity-script: I got InterstitialAdClickedEvent");
        }

        void InterstitialAdOpenedEvent()
        {
            Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        }

        void InterstitialAdClosedEvent()
        {
            Debug.Log("InterstitialAdClosedEvent");
            MyAdsManager.instance.InterstitialClosed();
            IronSource.Agent.loadInterstitial();
        }

        void InterstitialAdRewardedEvent()
        {
            Debug.Log("unity-script: I got InterstitialAdRewardedEvent");
        }

        /************* Interstitial DemandOnly Delegates *************/

        void InterstitialAdReadyDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got InterstitialAdReadyDemandOnlyEvent for instance: " + instanceId);
        }

        void InterstitialAdLoadFailedDemandOnlyEvent(string instanceId, IronSourceError error)
        {
            Debug.Log("unity-script: I got InterstitialAdLoadFailedDemandOnlyEvent for instance: " + instanceId + ", error code: " + error.getCode() + ",error description : " + error.getDescription());
        }

        void InterstitialAdShowFailedDemandOnlyEvent(string instanceId, IronSourceError error)
        {
            Debug.Log("unity-script: I got InterstitialAdShowFailedDemandOnlyEvent for instance: " + instanceId + ", error code :  " + error.getCode() + ",error description : " + error.getDescription());
        }

        void InterstitialAdClickedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got InterstitialAdClickedDemandOnlyEvent for instance: " + instanceId);
        }

        void InterstitialAdOpenedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got InterstitialAdOpenedDemandOnlyEvent for instance: " + instanceId);
        }

        void InterstitialAdClosedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got InterstitialAdClosedDemandOnlyEvent for instance: " + instanceId);
        }

        void InterstitialAdRewardedDemandOnlyEvent(string instanceId)
        {
            Debug.Log("unity-script: I got InterstitialAdRewardedDemandOnlyEvent for instance: " + instanceId);
        }


        #endregion

        #region Banner

        void BannerAdLoadedEvent()
        {
            Debug.Log("unity-script: I got BannerAdLoadedEvent");
            IronSource.Agent.displayBanner();
        }

        void BannerAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        }

        void BannerAdClickedEvent()
        {
            Debug.Log("unity-script: I got BannerAdClickedEvent");
        }

        void BannerAdScreenPresentedEvent()
        {
            Debug.Log("unity-script: I got BannerAdScreenPresentedEvent");
        }

        void BannerAdScreenDismissedEvent()
        {
            Debug.Log("unity-script: I got BannerAdScreenDismissedEvent");
        }

        void BannerAdLeftApplicationEvent()
        {
            Debug.Log("unity-script: I got BannerAdLeftApplicationEvent");
        }

        #endregion
        
        #region Offerwall

        void OfferwallOpenedEvent()
        {
            Debug.Log("I got OfferwallOpenedEvent");
        }

        void OfferwallClosedEvent()
        {
            Debug.Log("I got OfferwallClosedEvent");
        }

        void OfferwallShowFailedEvent(IronSourceError error)
        {
            Debug.Log("I got OfferwallShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void OfferwallAdCreditedEvent(Dictionary<string, object> dict)
        {
            Debug.Log("I got OfferwallAdCreditedEvent, current credits = " + dict["credits"] + " totalCredits = " + dict["totalCredits"]);
        
        }

        void GetOfferwallCreditsFailedEvent(IronSourceError error)
        {
            Debug.Log("I got GetOfferwallCreditsFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        }

        void OfferwallAvailableEvent(bool canShowOfferwal)
        {
            Debug.Log("I got OfferwallAvailableEvent, value = " + canShowOfferwal);
        
        }

        #endregion
#endif
    }
}