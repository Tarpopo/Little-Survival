using UnityEngine;

#if !UNITY_EDITOR && FLAG_SUPERSONIC
    using SupersonicWisdomSDK;
    using System.Collections.Generic;
#endif

namespace SquareDino.Scripts.Ads
{
    public class MySuperSonic : MonoBehaviour
    {
        [SerializeField] private string _apiKeyIOS;
        [SerializeField] private string _apiKeyGooglePlay;
        
#if !UNITY_EDITOR && FLAG_SUPERSONIC
        private void Awake()
        {
            string apiKey;
#if (UNITY_IOS)
            apiKey = _apiKeyIOS;
#elif (UNITY_ANDROID)
            apiKey = _apiKeyGooglePlay;
#endif
            SupersonicWisdom.Init(apiKey);
        }
        
        private void OnEnable() {
            SupersonicWisdom.OnSettingsLoadedEvent += OnSettingsLoaded;
            SupersonicWisdom.OnSettingsErrorEvent += OnSettingsError;
        }

        private void OnSettingsLoaded(Dictionary<string, object> settings)
        {
            Debug.Log("SuperSonic: OnSettingsLoaded " + settings);
        }

        private void OnSettingsError(SupersonicWisdom.SUPERSONICWISDOM_ERRORS nErrCode)
        {
            Debug.Log("SuperSonic: OnSettingsError " + nErrCode);
        }
#endif
    }
}