using UnityEngine;

#if UNITY_IOS
    using UnityEngine.iOS;
#endif

namespace SquareDino.Scripts.Settings
{
    public class MyProjectQuality : MonoBehaviour
    {

        public bool neverSleep;

        private void Start()
        {
            Application.targetFrameRate = 60;
            SetQualitySettings();

            Debug.Log("SystemInfo.deviceModel = " + SystemInfo.deviceModel);
            
            if (neverSleep) Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void SetQualitySettings()
        {
            #if UNITY_IOS
                if (Device.generation <= DeviceGeneration.iPhone6
                    || Device.generation <= DeviceGeneration.iPadMini4Gen
                    || Device.generation <= DeviceGeneration.iPad5Gen
                    || Device.generation <= DeviceGeneration.iPadAir2)
                    QualitySettings.SetQualityLevel(0);
            #endif
        }
    }
}