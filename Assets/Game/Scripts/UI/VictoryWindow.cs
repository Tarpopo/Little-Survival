using System.Collections;
using DG.Tweening;
using SquareDino.Scripts.MyAds;
using TMPro;
using UnityEngine;

namespace UI
{
    public class VictoryWindow : CanvasGroupBased
    {
        [Space]
        [SerializeField] private MyButton continueButton;
        [SerializeField] private TextMeshProUGUI completedLevelNumberText;

        public MyButton ContinueButton => continueButton;

        sealed public override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);

            continueButton.Show(value);

            if (enabled)
            {
                UpdateLevelText();
            }
        }

        private void OnEnable()
        {
            MyAdsManager.RewardedEnable(RewardedLoaded);
        }

        private void OnDisable()
        {
            MyAdsManager.RewardedDisable(RewardedLoaded);
        }

        private void RewardedLoaded(bool flag)
        {
            // КнопкаРевардед.interactable = flag;
        }

        // Обновляет текст с номером пройденного уровня
        private void UpdateLevelText()
        {
            completedLevelNumberText.text = string.Format("LEVEL {0} COMPLETED", Statistics.PlayerLevel - 1);
        }

        protected override void Start()
        {
            showDelayWPS = new WaitForSeconds(showDelay);
        }
    }
}