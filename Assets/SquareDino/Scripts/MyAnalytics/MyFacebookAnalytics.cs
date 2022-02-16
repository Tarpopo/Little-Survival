using UnityEngine;

#if FLAG_FA
    using Facebook.Unity;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
    public class MyFacebookAnalytics : MonoBehaviour
    {
#if FLAG_FA
        private void Awake()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback);
            }
            else
            {
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                Debug.Log("Facebook SDK - Initialized");
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }
#endif
    }
}