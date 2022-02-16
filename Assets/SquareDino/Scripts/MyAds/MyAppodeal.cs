using UnityEngine;
using SquareDino.Scripts.MyAds;
using SquareDino.Scripts.MyAnalytics;
#if FLAG_APPODEAL
    using AppodealAds.Unity.Api;
    using AppodealAds.Unity.Common;


public class MyAppodeal : MonoBehaviour, IPermissionGrantedListener,
    IInterstitialAdListener, IBannerAdListener, IRewardedVideoAdListener
{
    private static MyAppodeal instance;
    private string _mediationName;
    
    [SerializeField] private string AppKey_GooglePlay = "";
    [SerializeField] private string AppKey_iOS = "4a9995944720b3c909e42e4dced77f5dd3b978f64b7537ac";
    [SerializeField] private bool bannerAutoShow;
    [SerializeField] private bool testingMode;
    [SerializeField] private bool loggingMode;
    
    private string _appKey = "";

    #region Init
    
    private void Awake()
    {
            if (instance != null) return;
            instance = this;
        _mediationName = GetType().Name;
        #if UNITY_ANDROID
           Appodeal.requestAndroidMPermissions(this);
	    #endif
        
        #if UNITY_ANDROID
                _appKey = AppKey_GooglePlay;
        #elif UNITY_IOS
                _appKey = AppKey_iOS;
        #endif
    }
    
    private void Start()
    {   
		Appodeal.setLogLevel(loggingMode ? Appodeal.LogLevel.Verbose : Appodeal.LogLevel.None);
        Appodeal.setTesting(testingMode);
        
        Appodeal.disableLocationPermissionCheck();
        Appodeal.disableWriteExternalStoragePermissionCheck();
        Appodeal.setTriggerOnLoadedOnPrecache(Appodeal.INTERSTITIAL, true);
        Appodeal.setSmartBanners(true);
        Appodeal.setBannerAnimation(false);
        Appodeal.setTabletBanners(false);
        Appodeal.setBannerBackground(false);
        Appodeal.setChildDirectedTreatment(false);
        Appodeal.muteVideosIfCallsMuted(true);
        Appodeal.setAutoCache(Appodeal.INTERSTITIAL, true);
        
        Appodeal.initialize(_appKey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO | Appodeal.BANNER, false);
        
        Appodeal.setInterstitialCallbacks (this);
        Appodeal.setRewardedVideoCallbacks(this);
        Appodeal.setBannerCallbacks(this);
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
    
    #endregion

    #region Interstitial
    
    private void InterstitialShow(string placeName = "default")
    {
        var available = Appodeal.isLoaded(Appodeal.INTERSTITIAL);
        if (available) Appodeal.show(Appodeal.INTERSTITIAL);
        MyAdsManager.InterstitialShowFeedback(available, placeName, _mediationName);
    }

    public void onInterstitialLoaded(bool isPrecache)
    {
        MyAdsManager.InterstitialLoaded();
    }
    public void onInterstitialFailedToLoad() { print("Interstitial failed to load"); }
    
    public void onInterstitialShowFailed()
    {
        print("Appodeal. Interstitial show failed");
    }
    public void onInterstitialShown() { print("Interstitial shown"); }
    public void onInterstitialClicked() { print("Interstitial clicked"); }

    public void onInterstitialClosed()
    {
        MyAdsManager.Instance.InterstitialClosed();
    }
    public void onInterstitialExpired() { print("Appodeal. Interstitial expired"); }

    #endregion
    
    #region Rewarded
    
    private void RewardedShow(string placeName)
    {
        var available = Appodeal.canShow(Appodeal.REWARDED_VIDEO);
        if (available) Appodeal.show(Appodeal.REWARDED_VIDEO);
        MyAdsManager.RewardedShowFeedback(available, placeName, _mediationName);
    }

    public void onRewardedVideoLoaded(bool isPrecache) {
        MyAdsManager.RewardedLoaded(true);
    }
    public void onRewardedVideoFailedToLoad() { print("Rewarded video failed to load"); }
    public void onRewardedVideoShowFailed()
    {
        print("Appodeal. RewardedVideo show failed");
    }
    public void onRewardedVideoShown() { print("Rewarded video shown"); }
    public void onRewardedVideoClosed(bool finished)
    {
        if (finished) MyAdsManager.RewardedReward();
        MyAdsManager.Instance.RewardedClosed(true);
    }
    public void onRewardedVideoFinished(double amount, string name) { print("Appodeal. Reward: " + amount + " " + name); }
    public void onRewardedVideoExpired() { print("Appodeal. Video expired"); }
    public void onRewardedVideoClicked()
    {
        print("Appodeal. Video clicked");
    }
    #endregion
    
    #region Banner
    
    private void BannerShow()
    {
        Appodeal.show(Appodeal.BANNER_BOTTOM);
    }
    
    private void BannerHide()
    {
        Appodeal.hide(Appodeal.BANNER_BOTTOM);
    }

    public void onBannerLoaded(int height, bool precache)
    {
        print("banner loaded");
        if (bannerAutoShow) BannerShow();
    }
    public void onBannerFailedToLoad() { print("banner failed"); }
    public void onBannerShown() { print("banner opened"); }
    public void onBannerClicked() { print("banner clicked"); }
    public void onBannerExpired() { print("banner expired");  }

    #endregion

    #region Permission Grant
    
    public void writeExternalStorageResponse(int result) 
    { 
        if (result == 0) 
        {
            Debug.Log("WRITE_EXTERNAL_STORAGE permission granted"); 
        }
        else
        {
            Debug.Log("WRITE_EXTERNAL_STORAGE permission grant refused"); 
        }
    }
    
    public void accessCoarseLocationResponse(int result) 
    { 
        if(result == 0) 
        {
            Debug.Log("ACCESS_COARSE_LOCATION permission granted"); 
        }
        else
        {
            Debug.Log("ACCESS_COARSE_LOCATION permission grant refused"); 
        }
    }
    
    #endregion
}
#endif