using SquareDino.Scripts.Settings;
using UnityEngine;
using DG.Tweening;

namespace SquareDino.Scripts.RateUsPopUp
{
    public class RateUsManager : Singleton<RateUsManager>
    {
        [Header("Settings")]
        [SerializeField] private int numberOfSessionsToShow = 3;
        [Space]
        [SerializeField] private RateUsPopUp rateUsPopUpPrefab;

        private RateUsPopUp currentPopUp;

        public static bool NeverShowPopup => RateUsHandler.NeverShowPopup;

        public static void IncreaseNumberOfSessions()
        {
            if (Instance == null) return;

            RateUsHandler.NumberOfSessions++;
   
            if (RateUsHandler.NumberOfSessions >= Instance.numberOfSessionsToShow)
            {
                RateUsHandler.NumberOfSessions = 0;

                Instance.Display();
            }
        }

        private void Display()
        {
            if (!NeverShowPopup)
            {
                currentPopUp = Instantiate(rateUsPopUpPrefab, transform);

                currentPopUp.ContentTransform.localScale = Vector3.zero;
                currentPopUp.ContentTransform.DOScale(1f, 0.25f);

                currentPopUp.OnClosed += CurrentPopUp_OnClosed;
                currentPopUp.OnRatedHigh += CurrentPopUp_OnRatedHigh;
                currentPopUp.OnRatedNotHigh += CurrentPopUp_OnRatedNotHigh;
            }
        }

        private void CurrentPopUp_OnClosed()
        {
            if (currentPopUp != null)
                currentPopUp.ContentTransform.DOScale(0f, 0.25f).OnComplete(() => Destroy(currentPopUp.gameObject));
        }

        private void CurrentPopUp_OnRatedNotHigh()
        {
            if (currentPopUp != null)
                currentPopUp.ContentTransform.DOScale(0f, 0.25f).OnComplete(() => Destroy(currentPopUp.gameObject));
        }

        private void CurrentPopUp_OnRatedHigh()
        {
            OpenUrl();
            GameIsRated();
        }

        public void OpenUrl()
        {
        #if UNITY_ANDROID
            #if UNITY_EDITOR
                Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
            #else
                Application.OpenURL("market://details?id=" + Application.identifier);
            #endif
        #endif
        #if UNITY_IOS
            Application.OpenURL("https://itunes.apple.com/app/id" + Application.identifier);
        #endif
        }

        private void Start()
        {
            if (!MyBuildSettings.RateUs)
            {
                Destroy(gameObject);
                return;
            }
        }

        private void GameIsRated()
        {
            RateUsHandler.NeverShowPopup = true;

            if (currentPopUp != null)
                currentPopUp.ContentTransform.DOScale(0f, 0.25f).OnComplete(() => Destroy(currentPopUp.gameObject));
        }
    }
}