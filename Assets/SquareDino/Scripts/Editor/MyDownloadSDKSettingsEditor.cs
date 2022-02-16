using System.Collections.Generic;
using SquareDino.Scripts.Settings;
using UnityEditor;
using UnityEngine;

namespace SquareDino.Scripts.Editor
{
    [CustomEditor(typeof(MyDownloadSDKSettings))]
    public class MyDownloadSDKSettingsEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            SkdDownloadSettings.Init();
        }

        public override void OnInspectorGUI()
        {
            TryInitSdk(SDKNames.Facebook);
            TryInitSdk(SDKNames.GameAnalytics);
            TryInitSdk(SDKNames.AppsFlyer);
            TryInitSdk(SDKNames.Flurry);
            TryInitSdk(SDKNames.Tenjin);
            TryInitSdk(SDKNames.IronSource);
            TryInitSdk(SDKNames.YandexMetrica);
            TryInitSdk(SDKNames.AppLovin);
            TryInitSdk(SDKNames.SuperSonic);
            TryInitSdk(SDKNames.Appodeal);
            TryInitSdk(SDKNames.Voodoo);
            
            if (GUILayout.Button("Refresh Download Data", GUILayout.Width(290), GUILayout.Height(30)))
            {
                SkdDownloadSettings.Init();
            }
        }
        
        private void TryInitSdk(string name)
        {
            SdkDownloadDataEditor sdkDownloadData;
            
            if (SkdDownloadSettings.isInitialize && SkdDownloadSettings.CheckSdk(name, out sdkDownloadData))
            {
                EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(290));
                
                var versions = new List<string>();
                var urls = new List<string>();
                
                foreach (var versionTemp in sdkDownloadData.SdkDownloadData.versions)
                {
                    versions.Add(versionTemp.version);
                    urls.Add(versionTemp.url);
                }
                
                GUILayout.Label(name, GUILayout.Width(100));
                
                sdkDownloadData.selected = EditorGUILayout.Popup(sdkDownloadData.selected, versions.ToArray());
                    
                if (GUILayout.Button("Download", GUILayout.Width(110)))
                {
                    Debug.Log($"Download {name} of version {versions[sdkDownloadData.selected]}");
                    ExternalImportPackages.ImportPackage(urls[sdkDownloadData.selected]);
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }
    }   
}