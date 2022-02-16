using UnityEngine;

public class MyPoliciesHandler
{
    private const string PrivacyPolicyAcceptedWord = "PrivacyPolicyAccepted";
    private const string TermsOfServiceAcceptedWord = "TermOfServiceAccepted";


    public static bool PrivacyPolicyAccepted
    {
        get => PlayerPrefs.GetInt(PrivacyPolicyAcceptedWord, 0) == 1;
        set => PlayerPrefs.SetInt(PrivacyPolicyAcceptedWord, value ? 1 : 0);
    }
    public static bool TermsOfServiceAccepted
    {
        get => PlayerPrefs.GetInt(TermsOfServiceAcceptedWord, 0) == 1;
        set => PlayerPrefs.SetInt(TermsOfServiceAcceptedWord, value ? 1 : 0);
    }
}
