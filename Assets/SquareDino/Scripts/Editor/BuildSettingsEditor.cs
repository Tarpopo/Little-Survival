using System;
using System.Collections.Generic;
using SquareDino.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace SquareDino.Scripts.Editor
{
    [CustomEditor(typeof(MyBuildSettings))]
    public class BuildSettingsEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            SkdDownloadSettings.Init();
        }

        public override void OnInspectorGUI()
        {
            var settings = target as MyBuildSettings;
            if (settings == null) return;
            
            var somethingChanged = false;

            somethingChanged |= Toggle("Remote Config", ref settings.remoteConfig);
            somethingChanged |= Toggle("Dev Panel", ref settings.devPanel);
            somethingChanged |= Toggle("Policies", ref settings.policies);
            somethingChanged |= Toggle("Rate Us", ref settings.rateUs);

            GUILayout.Space(10);
            
            if (somethingChanged)
            {
                RefreshDefine(BuildTargetGroup.Android, settings.GenerateDefine_Android);
                RefreshDefine(BuildTargetGroup.iOS, settings.GenerateDefine_iOS);
                RefreshDefine(BuildTargetGroup.Standalone, settings.GenerateDefine_Standalone);
            }

            somethingChanged = false;
            
            settings.iOs = EditorGUILayout.BeginToggleGroup("AppStore SDKs", settings.iOs);
            if (settings.iOs)
            {
                CheckToggleChange(SDKNames.Facebook, ref settings.iOsFaceboook, ref somethingChanged);
                CheckToggleChange(SDKNames.GameAnalytics, ref settings.iOsGameAnalytics, ref somethingChanged);
                CheckToggleChange(SDKNames.AppsFlyer, ref settings.iOsAppsFlyer, ref somethingChanged);
                CheckToggleChange(SDKNames.Flurry, ref settings.iOsFlurry, ref somethingChanged);
                CheckToggleChange(SDKNames.Tenjin, ref settings.iOsTenjin, ref somethingChanged);
                CheckToggleChange(SDKNames.IronSource, ref settings.iOsIronSource, ref somethingChanged);
                CheckToggleChange(SDKNames.YandexMetrica, ref settings.iOsYandexMetrica, ref somethingChanged);
                CheckToggleChange(SDKNames.AppLovin, ref settings.iOsAppLovin, ref somethingChanged);
                CheckToggleChange(SDKNames.SuperSonic, ref settings.iOsSuperSonic, ref somethingChanged);
                CheckToggleChange(SDKNames.Appodeal, ref settings.iOsAppodeal, ref somethingChanged);
                CheckToggleChange(SDKNames.Voodoo, ref settings.iOsVoodoo, ref somethingChanged);
                CheckToggleChange(SDKNames.Homa, ref settings.iOsHoma, ref somethingChanged);
                CheckToggleChange(SDKNames.Firebase, ref settings.iOsFirebase, ref somethingChanged);
                CheckToggleChange(SDKNames.Ysocorp, ref settings.iOsYsocorp, ref somethingChanged);
                CheckToggleChange(SDKNames.Hoopsly, ref settings.iOsHoopsly, ref somethingChanged);
                CheckToggleChange(SDKNames.Amplitude, ref settings.iOsAmplitude, ref somethingChanged);
                CheckToggleChange(SDKNames.WannaPlay, ref settings.iOsWannaPlay, ref somethingChanged);
            }

            EditorGUILayout.EndToggleGroup();
            if (somethingChanged)
            {
                RefreshDefine(BuildTargetGroup.iOS, settings.GenerateDefine_iOS);
            }
            
            somethingChanged = false;
            settings.android = EditorGUILayout.BeginToggleGroup("GooglePlay SDKs", settings.android);
            if (settings.android)
            {
                CheckToggleChange(SDKNames.Facebook, ref settings.androidFaceboook, ref somethingChanged);
                CheckToggleChange(SDKNames.GameAnalytics, ref settings.androidGameAnalytics, ref somethingChanged);
                CheckToggleChange(SDKNames.AppsFlyer, ref settings.androidAppsFlyer, ref somethingChanged);
                CheckToggleChange(SDKNames.Flurry, ref settings.androidFlurry, ref somethingChanged);
                CheckToggleChange(SDKNames.Tenjin, ref settings.androidTenjin, ref somethingChanged);
                CheckToggleChange(SDKNames.IronSource, ref settings.androidIronSource, ref somethingChanged);
                CheckToggleChange(SDKNames.YandexMetrica, ref settings.androidYandexMetrica, ref somethingChanged);
                CheckToggleChange(SDKNames.AppLovin, ref settings.androidAppLovin, ref somethingChanged);
                CheckToggleChange(SDKNames.SuperSonic, ref settings.androidSuperSonic, ref somethingChanged);
                CheckToggleChange(SDKNames.Appodeal, ref settings.androidAppodeal, ref somethingChanged);
                CheckToggleChange(SDKNames.Voodoo, ref settings.androidVoodoo, ref somethingChanged);
                CheckToggleChange(SDKNames.Homa, ref settings.androidHoma, ref somethingChanged);
                CheckToggleChange(SDKNames.Firebase, ref settings.androidFirebase, ref somethingChanged);
                CheckToggleChange(SDKNames.Ysocorp, ref settings.androidYsocorp, ref somethingChanged);
                CheckToggleChange(SDKNames.Hoopsly, ref settings.androidHoopsly, ref somethingChanged);
                CheckToggleChange(SDKNames.Amplitude, ref settings.androidAmplitude, ref somethingChanged);
                CheckToggleChange(SDKNames.WannaPlay, ref settings.androidWannaPlay, ref somethingChanged);
            } 
            
            EditorGUILayout.EndToggleGroup();

            if (somethingChanged)
            {
                RefreshDefine(BuildTargetGroup.Android, settings.GenerateDefine_Android);
            }
            
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
            
            settings.Save();
        }
        
        private void RefreshDefine(BuildTargetGroup buildTargetGroup, Func<string, string> defineGenerator)
        {
            var oldDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var define = defineGenerator(oldDefine);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, define);
        }
        
        private void CheckToggleChange(string name, ref bool settings, ref bool changer)
        {
            EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(290));
            changer |= Toggle(name, ref settings);
            EditorGUILayout.EndHorizontal();
        }
        
        private bool Toggle(string text, ref bool value)
        {
            float originalValue = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;   
            
            var prevValue = value;
            value = EditorGUILayout.Toggle(text, value);
            
            EditorGUIUtility.labelWidth = originalValue;
            
            return prevValue != value;
        }
    }
}