using UnityEngine;

#if !UNITY_EDITOR && FLAG_FLURRY
    using FlurrySDK;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
    public class MyFlurry : MonoBehaviour
    {
        [SerializeField] private string flurryAndroidApiKey;
        [SerializeField] private string flurryIosApiKey;

        private void Start()
        {
#if !UNITY_EDITOR && FLAG_FLURRY
            var apiKey = "";

            #if UNITY_ANDROID
                        apiKey = flurryAndroidApiKey;
            #elif UNITY_IOS
                        apiKey = flurryIosApiKey;
            #endif
                        // Initialize Flurry once.
                        new Flurry.Builder()
                            .WithCrashReporting(true)
                            .WithLogEnabled(true)
                            .WithLogLevel(Flurry.LogLevel.LogVERBOSE)
                            .Build(apiKey);

                        // Example to get Flurry versions.
                        Debug.Log("AgentVersion: " + Flurry.GetAgentVersion());
                        Debug.Log("ReleaseVersion: " + Flurry.GetReleaseVersion());

                        // Log Flurry events.
                        var status = Flurry.LogEvent("Unity Event");
                        Debug.Log("Log Unity Event status: " + status);

            //        // Log Flurry timed events with parameters.
            //        IDictionary<string, string> parameters = new Dictionary<string, string>();
            //        parameters.Add("Author", "Flurry");
            //        parameters.Add("Status", "Registered");
            //        status = Flurry.LogEvent("Unity Event Params Timed", parameters, true);
            //        Debug.Log("Log Unity Event with parameters timed status: " + status);

            //        Flurry.EndTimedEvent("Unity Event Params Timed");
#endif
        }
    }
}