using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace SquareDino
{
    public class VibrationHandler : MonoBehaviour
    {
        #region Singleton
        
        private static VibrationHandler _instance;

        public static VibrationHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<VibrationHandler>();

                    if (_instance == null)
                    {
                        var go = new GameObject("[Vibration Handler]");
                        DontDestroyOnLoad(go);
                        _instance = go.AddComponent<VibrationHandler>();
                    }
                }

                return _instance;
            }
        }
        
        #endregion
        
        private readonly List<MyHapticTypes> _addedVibrationPerFrame = new List<MyHapticTypes>();
        private bool _isCanPlayVibro;

        private readonly Dictionary<int, HapticTypes> _overrideHapticsType = new Dictionary<int, HapticTypes>()
        {
            {(int) MyHapticTypes.LightImpact, HapticTypes.LightImpact},
            {(int) MyHapticTypes.Selection, HapticTypes.Selection},
            {(int) MyHapticTypes.Failure, HapticTypes.Failure}
        };

        private readonly MyHapticTypes[] _orderHaptic =
        {
            MyHapticTypes.LightImpact,
            MyHapticTypes.Selection,
            MyHapticTypes.Failure,
        };

        public void AddVibration(MyHapticTypes hapticTypes)
        {
            _addedVibrationPerFrame.Add(hapticTypes);
            _isCanPlayVibro = true;
        }

        private void TryPlayVibration()
        {
            if (!_isCanPlayVibro) return;
            _isCanPlayVibro = false;

            PlayVibration(CalculateVibrationTypeByOrder());
        }

        private void PlayVibration(MyHapticTypes hapticType)
        {   
            if (hapticType == MyHapticTypes.Selection)
            {
#if UNITY_ANDROID
                MMNVAndroid.AndroidVibrate(30);
#endif
#if UNITY_IOS
                MMVibrationManager.Haptic(HapticTypes.Selection);
#endif
            }
            else
            {
                if (_overrideHapticsType.TryGetValue((int) hapticType, out var value))
                {
                    MMVibrationManager.Haptic(value);
                }
            }

            _addedVibrationPerFrame.Clear();
        }

        private MyHapticTypes CalculateVibrationTypeByOrder()
        {
            var maxOrder = int.MinValue;

            for (int i = 0; i < _addedVibrationPerFrame.Count; i++)
            {
                for (int j = 0; j < _orderHaptic.Length; j++)
                {
                    if (_addedVibrationPerFrame[i] == _orderHaptic[j] && maxOrder < j)
                    {
                        maxOrder = j;
                    }
                }
            }

            return _orderHaptic[maxOrder];
        }

        private void LateUpdate() => TryPlayVibration();
    }
}