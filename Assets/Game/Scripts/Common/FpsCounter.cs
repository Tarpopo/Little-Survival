using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private GameObject graphy;

    private GameObject _fpsCounter;
    private bool _isShow = true;

    private const string ShowFpsCounterKey = "showFpsCounterKey";
    public bool IsShow => _isShow;

#if FLAG_DEV_PANEL
    private void Awake()
    {
        _isShow = PlayerPrefs.GetInt(ShowFpsCounterKey, 1) == 1;
        _fpsCounter = Instantiate(graphy, transform);
        _fpsCounter.SetActive(_isShow);
    }
#endif

    public void SetActiveFpsCounter(bool value)
    {
        PlayerPrefs.SetInt(ShowFpsCounterKey, value ? 1 : 0);
        _isShow = value;
        
        _fpsCounter.SetActive(_isShow);
    }
}