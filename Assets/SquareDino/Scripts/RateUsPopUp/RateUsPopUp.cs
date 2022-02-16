using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquareDino.Scripts.RateUsPopUp
{
    public class RateUsPopUp : MonoBehaviour
    {
        private const string GOOGLE_PLAY_LINK = "http://play.google.com/store/apps/details?id=";

        public event System.Action OnRatedHigh;
        public event System.Action OnRatedNotHigh;
        public event System.Action OnClosed;

        [SerializeField] private GameObject content;
        [SerializeField] private MyButton closeButton;

        [SerializeField] private RateUsStarElement[] starElements;

        public Transform ContentTransform => content.transform;

        private void Reset()
        {
            foreach (var star in starElements)
                star.Enable(false);
        }

        private void Awake()
        {
            closeButton.OnClick += CloseButton_OnClick;

            foreach (var star in starElements)
                star.OnSelected += Star_OnSelected;
        }

        private void Star_OnSelected(RateUsStarElement starElement)
        {
            foreach (var star in starElements)
                star.OnSelected -= Star_OnSelected;

            Reset();

            int elementIndex = 0;

            for (int i = 0; i < starElements.Length; i++)
            {
                starElements[i].Enable(true);

                if (starElement == starElements[i])
                {
                    elementIndex = i;
                    break;
                }
            }

            if (elementIndex > 2) OnRatedHigh?.Invoke();
            else OnRatedNotHigh?.Invoke();
        }

        private void CloseButton_OnClick()
        {
            OnClosed?.Invoke();
        }
    }
}