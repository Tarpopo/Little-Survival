using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace SquareDino.Scripts
{
    public class MyAtt : MonoBehaviour
    {
#if UNITY_IOS
    private void Awake()
    {
        var attStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        if(attStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
    }
#endif
    }
}
