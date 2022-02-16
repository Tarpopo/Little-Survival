using System.Collections;
using UnityEngine;

namespace SquareDino.Scripts.MyAnalytics
{
    public class LevelLengthTimer : MonoBehaviour
    {
        private IEnumerator _levelLengthCoroutine;
        public static float Value;

        private void OnEnable()
        {
            MyAnalyticsManager.OnLevelStart += OnLevelStart;
            MyAnalyticsManager.OnLevelFinish += OnLevelFinish;
        }
        
        private void OnDisable()
        {
            MyAnalyticsManager.OnLevelStart -= OnLevelStart;
            MyAnalyticsManager.OnLevelFinish -= OnLevelFinish; 
        }

        private void OnLevelStart()
        {
            StopAllCoroutines();
            Value = 0f;
            StartCoroutine(LevelLengthCoroutine());
        }

        private void OnLevelFinish()
        {
            StopAllCoroutines();
        }
        
        private IEnumerator LevelLengthCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Value += 1f;
            }
        }
    }
}