using UnityEngine;
using TMPro;
using System.Collections;

namespace UI
{
    public class LosingWindow : CanvasGroupBased
    {
        [Space]
        [SerializeField] private MyButton restartButton;
        [SerializeField] private TextMeshProUGUI fadedText;

        public MyButton RestartButton => restartButton;

        sealed public override void Enable(bool value, float delay = -1, float duration = -1)
        {
            base.Enable(value, delay, duration);

            restartButton.Show(value);
        }
    }
}