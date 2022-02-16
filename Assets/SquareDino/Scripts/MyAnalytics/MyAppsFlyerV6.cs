using UnityEngine;

#if FLAG_AF
    using AppsFlyerSDK;
#endif

namespace SquareDino.Scripts.MyAnalytics
{
    public class MyAppsFlyerV6 : MonoBehaviour {
#if FLAG_AF
        private void Start()
        {
            AppsFlyer.setCustomerUserId(SystemInfo.deviceUniqueIdentifier);
        }
#endif
    }
}