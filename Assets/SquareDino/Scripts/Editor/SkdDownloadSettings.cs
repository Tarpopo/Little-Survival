using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SquareDino.Scripts
{
    public class SkdDownloadSettings
    {
        private static Action OnInitialized;
        private static readonly string sdkDownloadListUrl = "https://drive.google.com/u/0/uc?id=1TVuEa3UlXNjxLq-9tIr6pRbs2q2l7Thd&export=download";

        private static Dictionary<string, SdkDownloadDataEditor> _urlDependensies = new Dictionary<string, SdkDownloadDataEditor>();
        private static SdkDownloadDatas _sdkDownloadData = new SdkDownloadDatas();
        private static UnityWebRequest _www;
        public static bool isInitialize;
        
        public static void Init()
        {
            _urlDependensies.Clear();

            _www?.Dispose();
            _www = UnityWebRequest.Get(sdkDownloadListUrl);
            _www.SendWebRequest().completed += Download;
        }
        
        private static void Download(AsyncOperation obj)
        {
            if (_www.isNetworkError || _www.isHttpError)
            {
                Debug.Log("ERROR: " + _www.error);
            }
            else
            {
                JsonParse(_www.downloadHandler.text);
                isInitialize = true;
            }
        }

        private static void JsonParse(string jsonFile)
        {
            if(string.IsNullOrEmpty(jsonFile)) return;
            
            _sdkDownloadData = JsonUtility.FromJson<SdkDownloadDatas>(jsonFile);
            
            foreach (var sdkDownloadData in _sdkDownloadData.sdkDownloadDatas)
            {                
                SdkDownloadDataEditor sdkDownloadDataEditor = new SdkDownloadDataEditor();
                sdkDownloadDataEditor.SdkDownloadData = sdkDownloadData;
                sdkDownloadDataEditor.selected = 0;
                
                _urlDependensies.Add(sdkDownloadData.name, sdkDownloadDataEditor);
            }
            
            OnInitialized?.Invoke();
        }
        
        public static bool CheckSdk(string sdkName, out SdkDownloadDataEditor versions)
        {
            if (!isInitialize)
            {
                versions = null;
                return false;
            }

            return _urlDependensies.TryGetValue(sdkName, out versions);
        }
    }
}