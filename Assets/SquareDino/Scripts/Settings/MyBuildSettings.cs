using System;
using UnityEngine;

namespace SquareDino.Scripts.Settings
{
    [ExecuteInEditMode]
    public class MyBuildSettings : MonoBehaviour
    {
        private const string FLAG_REMOTECONFIG = "FLAG_REMOTE_CONFIG";
        private const string FLAG_DEVPANEL = "FLAG_DEV_PANEL";
        private const string FLAG_FACEBOOK = "FLAG_FA";
        private const string FLAG_GAME_ANALYTICS = "FLAG_GA";
        private const string FLAG_APPS_FLYER = "FLAG_AF";
        private const string FLAG_FLURRY = "FLAG_FlURRY";
        private const string FLAG_TENJIN = "FLAG_TENJIN";
        private const string FLAG_IRONSOURCE = "FLAG_IRONSOURCE";
        private const string FLAG_YANDEX_METRICA = "FLAG_YANDEX_METRICA";
        private const string FLAG_APPLOVIN = "FLAG_APPLOVIN";
        private const string FLAG_SUPERSONIC = "FLAG_SUPERSONIC";
        private const string FLAG_APPODEAL = "FLAG_APPODEAL";
        private const string FLAG_VOODOO = "FLAG_VOODOO";
        private const string FLAG_HOMA = "FLAG_HOMA";
        private const string FLAG_FIREBASE = "FLAG_FIREBASE";
        private const string FLAG_YSOCORP = "FLAG_YSOCORP";
        private const string FLAG_HOOPSLY = "FLAG_HOOPSLY";
        private const string FLAG_AMPLITUDE = "FLAG_AMPLITUDE";
        private const string FLAG_WANNAPLAY = "FLAG_WANNAPLAY";
        
        public bool remoteConfig;
        public bool devPanel;
        public bool policies;
        public bool rateUs;
        
        public bool iOs;
        public bool iOsFaceboook;
        public bool iOsGameAnalytics;
        public bool iOsAppsFlyer;
        public bool iOsFlurry;
        public bool iOsTenjin;
        public bool iOsIronSource;
        public bool iOsYandexMetrica;
        public bool iOsAppLovin;
        public bool iOsSuperSonic;
        public bool iOsAppodeal;
        public bool iOsVoodoo;
        public bool iOsHoma;
        public bool iOsFirebase;
        public bool iOsYsocorp;
        public bool iOsHoopsly;
        public bool iOsAmplitude;
        public bool iOsWannaPlay;
        
        public bool android;
        public bool androidFaceboook;
        public bool androidGameAnalytics;
        public bool androidAppsFlyer;
        public bool androidFlurry;
        public bool androidTenjin;
        public bool androidIronSource;
        public bool androidYandexMetrica;
        public bool androidAppLovin;
        public bool androidSuperSonic;
        public bool androidAppodeal;
        public bool androidVoodoo;
        public bool androidHoma;
        public bool androidFirebase;
        public bool androidYsocorp;
        public bool androidHoopsly;
        public bool androidAmplitude;
        public bool androidWannaPlay;

        public static bool DevPanel { get; private set; }
        public static bool Policies { get; private set; }
        public static bool RateUs { get; private set; }
        
        private void Awake()
        {
            DevPanel = devPanel;
            Policies = policies;
            RateUs = rateUs;
            Load();
        }

        public string GenerateDefine_iOS(string value)
        {
            value = ClearOldDefines(value);
            if (value.LastIndexOf(';') != value.Length-1) value += ";";
            
            var newDefines = GenerateDefines_iOS();
            return value + newDefines;
        }
        
        public string GenerateDefine_Android(string value)
        {
            value = ClearOldDefines(value);
            if (value.LastIndexOf(';') != value.Length-1) value += ";";
            
            var newDefines = GenerateDefines_Android();
            return value + newDefines;
        }
        
        public string GenerateDefine_Standalone(string value)
        {
            value = ClearOldDefines(value);
            if (value.LastIndexOf(';') != value.Length-1) value += ";";
            
            var newDefines = GenerateDefines_Standalone();
            return value + newDefines;
        }

        private string ClearOldDefines(string value)
        {
            value = RemoveKey(value, FLAG_REMOTECONFIG);
            value = RemoveKey(value, FLAG_DEVPANEL);
            value = RemoveKey(value, FLAG_FACEBOOK);
            value = RemoveKey(value, FLAG_GAME_ANALYTICS);
            value = RemoveKey(value, FLAG_APPS_FLYER);
            value = RemoveKey(value, FLAG_FLURRY);
            value = RemoveKey(value, FLAG_TENJIN);
            value = RemoveKey(value, FLAG_IRONSOURCE);
            value = RemoveKey(value, FLAG_YANDEX_METRICA);
            value = RemoveKey(value, FLAG_APPLOVIN);
            value = RemoveKey(value, FLAG_SUPERSONIC);
            value = RemoveKey(value, FLAG_APPODEAL);
            value = RemoveKey(value, FLAG_VOODOO);
            value = RemoveKey(value, FLAG_HOMA);
            value = RemoveKey(value, FLAG_FIREBASE);
            value = RemoveKey(value, FLAG_YSOCORP);
            value = RemoveKey(value, FLAG_HOOPSLY);
            value = RemoveKey(value, FLAG_AMPLITUDE);
            value = RemoveKey(value, FLAG_WANNAPLAY);
            return RemoveGarbage(value);
        }

        private string RemoveKey(string value, string key)
        {
            while (true)
            {
                var id = value.IndexOf(key, StringComparison.Ordinal);
                if (id <= -1) return value;

                value = value.Remove(id, key.Length);
            }
        }

        private string RemoveGarbage(string value)
        {
            var key = ";;";
            var id = value.IndexOf(key, StringComparison.Ordinal);
            if (id <= -1) return value;
            
            value = value.Remove(id, 1);
            return RemoveKey(value, key);
        }

        private string GenerateDefines_iOS()
        {
            var value = "";
            if (remoteConfig) value += FLAG_REMOTECONFIG + ";";
            if (devPanel) value += FLAG_DEVPANEL + ";";
            if (iOsFaceboook) value += FLAG_FACEBOOK + ";";
            if (iOsGameAnalytics) value += FLAG_GAME_ANALYTICS + ";";
            if (iOsAppsFlyer) value += FLAG_APPS_FLYER + ";";
            if (iOsFlurry) value += FLAG_FLURRY + ";";
            if (iOsTenjin) value += FLAG_TENJIN + ";";
            if (iOsIronSource) value += FLAG_IRONSOURCE + ";";
            if (iOsYandexMetrica) value += FLAG_YANDEX_METRICA + ";";
            if (iOsAppLovin) value += FLAG_APPLOVIN + ";";
            if (iOsSuperSonic) value += FLAG_SUPERSONIC + ";";
            if (iOsAppodeal) value += FLAG_APPODEAL + ";";
            if (iOsVoodoo) value += FLAG_VOODOO + ";";
            if (iOsHoma) value += FLAG_HOMA + ";";
            if (iOsFirebase) value += FLAG_FIREBASE + ";";
            if (iOsYsocorp) value += FLAG_YSOCORP + ";";
            if (iOsHoopsly) value += FLAG_HOOPSLY + ";";
            if (iOsAmplitude) value += FLAG_AMPLITUDE + ";";
            if (iOsWannaPlay) value += FLAG_WANNAPLAY + ";";
                
            return value;
        }
        
        private string GenerateDefines_Android()
        {
            var value = "";
            if (remoteConfig) value += FLAG_REMOTECONFIG + ";";
            if (devPanel) value += FLAG_DEVPANEL + ";";
            if (androidFaceboook) value += FLAG_FACEBOOK + ";";
            if (androidGameAnalytics) value += FLAG_GAME_ANALYTICS + ";";
            if (androidAppsFlyer) value += FLAG_APPS_FLYER + ";";
            if (androidFlurry) value += FLAG_FLURRY + ";";
            if (androidTenjin) value += FLAG_TENJIN + ";";
            if (androidIronSource) value += FLAG_IRONSOURCE + ";";
            if (androidYandexMetrica) value += FLAG_YANDEX_METRICA + ";";
            if (androidAppLovin) value += FLAG_APPLOVIN + ";";
            if (androidSuperSonic) value += FLAG_SUPERSONIC + ";";
            if (androidAppodeal) value += FLAG_APPODEAL + ";";
            if (androidVoodoo) value += FLAG_VOODOO + ";";
            if (androidHoma) value += FLAG_HOMA + ";";
            if (androidFirebase) value += FLAG_FIREBASE + ";";
            if (androidYsocorp) value += FLAG_YSOCORP + ";";
            if (androidHoopsly) value += FLAG_HOOPSLY + ";";
            if (androidAmplitude) value += FLAG_AMPLITUDE + ";";
            if (androidWannaPlay) value += FLAG_WANNAPLAY + ";";
                
            return value;
        }
        
        private string GenerateDefines_Standalone()
        {
            var value = "";
            if (devPanel) value += FLAG_DEVPANEL + ";";
                
            return value;
        }

        public void Save()
        {
            PlayerPrefs.SetInt("BuildSettings: remoteConfig", remoteConfig ? 1 : 0);
            
            PlayerPrefs.SetInt("BuildSettings: iOs", iOs ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsFaceboook", iOsFaceboook ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsGameAnalytics", iOsGameAnalytics ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsAppsFlyer", iOsAppsFlyer ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsFlurry", iOsFlurry ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsTenjin", iOsTenjin ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsIronSource", iOsIronSource ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsYandexMetrica", iOsYandexMetrica ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsAppLovin", iOsAppLovin ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsSuperSonic", iOsSuperSonic ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsAppodeal", iOsAppodeal ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsVoodoo", iOsVoodoo ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsHoma", iOsHoma ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsFirebase", iOsFirebase ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsYsocorp", iOsYsocorp ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsHoopsly", iOsHoopsly ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsAmplitude", iOsAmplitude ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: iOsWannaPlay", iOsWannaPlay ? 1 : 0);
            
            PlayerPrefs.SetInt("BuildSettings: android", android ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidFaceboook", androidFaceboook ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidGameAnalytics", androidGameAnalytics ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidAppsFlyer", androidAppsFlyer ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidFlurry", androidFlurry ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidTenjin", androidTenjin ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidIronSource", androidIronSource ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidYandexMetrica", androidYandexMetrica ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidAppLovin", androidAppLovin ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidSuperSonic", androidSuperSonic ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidAppodeal", androidAppodeal ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidVoodoo", androidVoodoo ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidHoma", androidHoma ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidFirebase", androidFirebase ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidYsocorp", androidYsocorp ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidHoopsly", androidHoopsly ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidAmplitude", androidAmplitude ? 1 : 0);
            PlayerPrefs.SetInt("BuildSettings: androidWannaPlay", androidWannaPlay ? 1 : 0);
        }

        private void Load()
        {
            iOs = PlayerPrefs.GetInt("remoteConfig: remoteConfig") == 1;
            
            iOs = PlayerPrefs.GetInt("BuildSettings: iOs") == 1;
            iOsFaceboook = PlayerPrefs.GetInt("BuildSettings: iOsFaceboook") == 1;
            iOsGameAnalytics = PlayerPrefs.GetInt("BuildSettings: iOsGameAnalytics") == 1;
            iOsAppsFlyer = PlayerPrefs.GetInt("BuildSettings: iOsAppsFlyer") == 1;
            iOsFlurry = PlayerPrefs.GetInt("BuildSettings: iOsFlurry") == 1;
            iOsTenjin = PlayerPrefs.GetInt("BuildSettings: iOsTenjin") == 1;
            iOsIronSource = PlayerPrefs.GetInt("BuildSettings: iOsIronSource") == 1;
            iOsYandexMetrica = PlayerPrefs.GetInt("BuildSettings: iOsYandexMetrica") == 1;
            iOsAppLovin = PlayerPrefs.GetInt("BuildSettings: iOsAppLovin") == 1;
            iOsSuperSonic = PlayerPrefs.GetInt("BuildSettings: iOsSuperSonic") == 1;
            iOsAppodeal = PlayerPrefs.GetInt("BuildSettings: iOsAppodeal") == 1;
            iOsVoodoo = PlayerPrefs.GetInt("BuildSettings: iOsVoodoo") == 1;
            iOsHoma = PlayerPrefs.GetInt("BuildSettings: iOsHoma") == 1;
            iOsFirebase = PlayerPrefs.GetInt("BuildSettings: iOsFirebase") == 1;
            iOsYsocorp = PlayerPrefs.GetInt("BuildSettings: iOsYsocorp") == 1;
            iOsHoopsly = PlayerPrefs.GetInt("BuildSettings: iOsHoopsly") == 1;
            iOsAmplitude = PlayerPrefs.GetInt("BuildSettings: iOsAmplitude") == 1;
            iOsWannaPlay = PlayerPrefs.GetInt("BuildSettings: iOsWannaPlay") == 1;
            
            android = PlayerPrefs.GetInt("BuildSettings: android") == 1;
            androidFaceboook = PlayerPrefs.GetInt("BuildSettings: androidFaceboook") == 1;
            androidGameAnalytics = PlayerPrefs.GetInt("BuildSettings: androidGameAnalytics") == 1;
            androidAppsFlyer = PlayerPrefs.GetInt("BuildSettings: androidAppsFlyer") == 1;
            androidFlurry = PlayerPrefs.GetInt("BuildSettings: androidFlurry") == 1;
            androidTenjin = PlayerPrefs.GetInt("BuildSettings: androidTenjin") == 1;
            androidIronSource = PlayerPrefs.GetInt("BuildSettings: androidIronSource") == 1;
            androidYandexMetrica = PlayerPrefs.GetInt("BuildSettings: androidYandexMetrica") == 1;
            androidAppLovin = PlayerPrefs.GetInt("BuildSettings: androidAppLovin") == 1;
            androidSuperSonic = PlayerPrefs.GetInt("BuildSettings: androidSuperSonic") == 1;
            androidAppodeal = PlayerPrefs.GetInt("BuildSettings: androidAppodeal") == 1;
            androidVoodoo = PlayerPrefs.GetInt("BuildSettings: androidVoodoo") == 1;
            androidHoma = PlayerPrefs.GetInt("BuildSettings: androidHoma") == 1;
            androidFirebase = PlayerPrefs.GetInt("BuildSettings: androidFirebase") == 1;
            androidYsocorp = PlayerPrefs.GetInt("BuildSettings: androidYsocorp") == 1;
            androidHoopsly = PlayerPrefs.GetInt("BuildSettings: androidHoopsly") == 1;
            androidAmplitude = PlayerPrefs.GetInt("BuildSettings: androidAmplitude") == 1;
            androidWannaPlay = PlayerPrefs.GetInt("BuildSettings: androidWannaPlay") == 1;
        }
    }
}