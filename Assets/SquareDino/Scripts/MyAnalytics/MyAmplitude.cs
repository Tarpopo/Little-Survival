using UnityEngine;
#if FLAG_AMPLITUDE
        using System;
        using System.Collections.Generic;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
    public class MyAmplitude : MonoBehaviour {
    
        [SerializeField] private string key = "key";
#if FLAG_AMPLITUDE
        private static Amplitude _amplitude;

        private void Start()
        {
            _amplitude = Amplitude.getInstance();
            _amplitude.setServerUrl("https://api2.amplitude.com");
            _amplitude.logging = true;
            _amplitude.trackSessionEvents(true);
            _amplitude.init(key);
        }
#endif
        public static void LogEvent(string text)
        {
#if FLAG_AMPLITUDE
           _amplitude.logEvent(text);
#endif
        }
        
        public static void LogEvent(string text, string paramName, object obj)
        {
#if FLAG_AMPLITUDE
            if (obj == null) return;
            
            var eventProps = new Dictionary<string, object>();
            eventProps.Add(paramName, obj);
            _amplitude.logEvent(text, eventProps);
#endif
        }
        
        public static void LogEvent(string text, string paramName, object obj, string paramName2, object obj2)
        {
#if FLAG_AMPLITUDE
            if (obj == null) return;
            
            var eventProps = new Dictionary<string, object>();
            eventProps.Add(paramName, obj);
            eventProps.Add(paramName2, obj2);
            _amplitude.logEvent(text, eventProps);
#endif
        }
    }
}