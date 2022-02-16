using UnityEngine;

public class MyPoliciesPopUp : MonoBehaviour
{
    public event System.Action Accepted;


    [SerializeField] private GameObject _content;
    [SerializeField] private MyButton _acceptButton;
    [SerializeField] private MyButton _privacyPolicyButton;
    [SerializeField] private MyButton _termsOfUseButton;


    private string _privacyPolicyURL;
    private string _termsOfServiceURL;


    private void Awake()
    {
        _acceptButton.OnClick += _acceptButton_OnClick;
        _privacyPolicyButton.OnClick += _privacyPolicyButton_OnClick;
        _termsOfUseButton.OnClick += _termsOfUseButton_OnClick;
    }


    public void Init(string privacyPolicyURL, string termsOfServiceURL, System.Action accepted)
    {
        _privacyPolicyURL = privacyPolicyURL;
        _termsOfServiceURL = termsOfServiceURL;
        Accepted += accepted;
        _content.SetActive(true);
    }


    private void _termsOfUseButton_OnClick()
    {
        Application.OpenURL(_termsOfServiceURL);
    }

    private void _privacyPolicyButton_OnClick()
    {
        Application.OpenURL(_privacyPolicyURL);
    }

    private void _acceptButton_OnClick()
    {
        Accepted?.Invoke();
    }
}
