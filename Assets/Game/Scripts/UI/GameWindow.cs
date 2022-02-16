using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameWindow : CanvasGroupBased
    {
        [Space]
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private Image progressImage;
        [SerializeField] private MyButton restartButton;
        [SerializeField] private MyButton tapToPlayButton;
        [SerializeField] private MyButton settingsButton;

        public MyButton SettingsButton => settingsButton;
        public MyButton RestartButton => restartButton;
        public MyButton TapToPlayButton => tapToPlayButton;

        sealed public override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);

            settingsButton.Show(value);
            restartButton.Show(value);

            if (!enabled) return;
            UpdateLevelText();
            ResetProgressBar(); 
        }

        public void UpdateProgressBar(LevelProgress levelProgress)
        {
            progressImage.fillAmount = levelProgress.Progress;
        }

        public void ResetProgressBar()
        {
            progressImage.fillAmount = 0f;
        }

        private void UpdateLevelText()
        {
            currentLevelText.text = string.Format("LEVEL {0}", Statistics.PlayerLevel);
        }
    }
}