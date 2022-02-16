using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace UI
{
    public class CanvasGroupBased : MonoBehaviour
    {
        public event System.Action<bool> OnEnableAll;
        public event System.Action<bool> OnEnableInteractable;

        protected void DoEnable(bool value)
        {
            if (OnEnableAll != null)
            {
                OnEnableAll(value);
            }
        }

        [SerializeField]
        protected CanvasGroup canvasGroup;
        [Header("Transition values")]
        [SerializeField, Min(0)] protected float showDelay = 0;
        [SerializeField, Min(0)] protected float fadeInDuration = 0;
        [SerializeField, Min(0)] protected float fadeOutDuration = 0;
        [Space]

        protected WaitForSeconds showDelayWPS;
        protected WaitForSeconds fadeInDurationWPS;
        protected WaitForSeconds fadeOutDurationWPS;
        private Coroutine _coroutine;

        public bool IsShown
        {
            get;
            protected set;
        }

        protected virtual void Awake()
        {
            showDelayWPS = new WaitForSeconds(showDelay);
            fadeInDurationWPS = new WaitForSeconds(fadeInDuration);
            fadeOutDurationWPS = fadeInDuration == fadeOutDuration ?
                fadeInDurationWPS :
                new WaitForSeconds(fadeOutDuration);
        }

        /// <summary>
        /// Used for UnityEvents (e.g. buttons in editor)
        /// </summary>
        /// <param name="value"></param>
        public void Enable0(bool value) => Enable(value, -1, -1);

        /// <summary>
        /// </summary>
        /// <param name="value">d</param>
        /// <param name="delay">Delay before window appears, if less than 0 will be used showDelay value, used when value == true</param>
        /// <param name="duration">Duration of fading in or out, if less than 0 will be used fadeDuration value</param>
        public virtual void Enable(bool value, float delay = -1, float duration = -1)
        {

            if (delay < 0) delay = showDelay;
            if (duration < 0) duration = value ? fadeInDuration : fadeOutDuration;

            if (value && (delay) > 0f)
            {
                if (_coroutine != null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(WithDelay());
            }
            else
            {
                Enable();
            }


            IEnumerator WithDelay()
            {
                if (delay == showDelay) yield return showDelayWPS;
                else yield return new WaitForSeconds(delay);

                Enable();
            }

            void Enable()
            {
                IsShown = value;

                if (duration > 0)
                {
                    canvasGroup.DOFade(IsShown ? 1f : 0f, duration);
                }
                else
                {
                    canvasGroup.alpha = value ? 1f : 0f;
                }
                OnEnableAll?.Invoke(enabled);
                EnableInteractable(value, duration);
            }

        }

        public void EnableInteractable(bool enabled, float duration)
        {
            if (duration <= 0)
            {
                canvasGroup.blocksRaycasts = enabled;
                canvasGroup.interactable = enabled;
                OnEnableInteractable?.Invoke(enabled);
                OnEnableEnd(enabled);
            }
            else
            {
                if (_coroutine != null) StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(WithDelay());
            }

            IEnumerator WithDelay()
            {
                if (duration == fadeInDuration) yield return fadeInDurationWPS;
                else if (duration == fadeOutDuration) yield return fadeOutDurationWPS;
                else yield return new WaitForSeconds(duration);
                EnableInteractable(enabled, 0);
            }
        }
        protected virtual void OnEnableEnd(bool value) { }


        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

    }
}
