#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using SquareDino.Scripts.Settings;
using UnityEngine;

namespace SquareDino.Scripts.PolicesPopUp
{
    public class MyPoliciesManager : Singleton<MyPoliciesManager>
    {
        [SerializeField] private MyPoliciesPopUp policiesPopUpPrefab;
        [Space]
        [SerializeField] private string privacyPolicyURL = "http://wannatest.games/privacy-policy.html";
        [SerializeField] private string termsOfServictyURL = "http://wannatest.games/terms-of-use.html";

        public event System.Action OnPoliciesAccepted;

        private MyPoliciesPopUp _currentPopUp;

        private void Start()
        {
            if (!MyBuildSettings.Policies)
            {
                Destroy(gameObject);
                return;
            }

#if UNITY_IOS
            var attStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if(attStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
            if (!IsPoliciesAccepted)
            {
                _currentPopUp = Instantiate(policiesPopUpPrefab, transform);
                _currentPopUp.Init(privacyPolicyURL, termsOfServictyURL, PoliciesAccepted);
            }
            else
            {
                PoliciesAccepted();
            }
        }

        public static bool IsPoliciesAccepted => MyPoliciesHandler.PrivacyPolicyAccepted && MyPoliciesHandler.TermsOfServiceAccepted;


        private void PoliciesAccepted()
        {
            MyPoliciesHandler.PrivacyPolicyAccepted = true;
            MyPoliciesHandler.TermsOfServiceAccepted = true;

            OnPoliciesAccepted?.Invoke();
            if (_currentPopUp != null)
                Destroy(_currentPopUp.gameObject);
        }
    }
}
