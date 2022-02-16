using UnityEngine;

#if FLAG_YSOCORP
using YsoCorp.GameUtils;
using SquareDino.Scripts.MyAds;
#endif

namespace SquareDino.Scripts.Publishers
{
    public class MyYso : MonoBehaviour
    {
        private static MyYso instance;
        private string _mediationName;

#if FLAG_YSOCORP
        
        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            _mediationName = GetType().Name;
            MyAdsManager.InterstitialLoaded();
            MyAdsManager.RewardedLoaded(true);
        }

        private void OnEnable()
        {
            MyAdsManager.InterstitialShowAction += InterstitialShow;
            MyAdsManager.RewardedShowAction += RewardedShow;
            // MyAdsManager.BannerShowAction += BannerShow;
            // MyAdsManager.BannerHideAction += BannerHide;
        }

        private void OnDisable()
        {
            MyAdsManager.InterstitialShowAction -= InterstitialShow;
            MyAdsManager.RewardedShowAction -= RewardedShow;
            // MyAdsManager.BannerShowAction -= BannerShow;
            // MyAdsManager.BannerHideAction -= BannerHide;
        }

        private void BannerHide()
        {
            // if (_bannerAdUnitId == "") return;
            // MaxSdk.HideBanner(_bannerAdUnitId);
        }

        private void BannerShow()
        {
            // if (_bannerAdUnitId == "") return;
            // MaxSdk.ShowBanner(_bannerAdUnitId);
        }

        #region Interstitial

        private void InterstitialShow(string placeName = "default")
        {
            MyAdsManager.InterstitialShowFeedback(true, placeName, _mediationName);
            YCManager.instance.adsManager.ShowInterstitial(() => {
                MyAdsManager.Instance.InterstitialClosed();
                MyAdsManager.InterstitialLoaded();
            });
        }

        #endregion
    
        #region Rewarded

        private void RewardedShow(string placeName)
        {
            MyAdsManager.RewardedShowFeedback(true, placeName, _mediationName);
            YCManager.instance.adsManager.ShowRewarded((bool ok) => {
                if (ok) {
                    MyAdsManager.RewardedReward();
                }
                MyAdsManager.Instance.RewardedClosed(true, placeName);
                MyAdsManager.RewardedLoaded(true);
            });
        }

        #endregion
#endif
    }
}
