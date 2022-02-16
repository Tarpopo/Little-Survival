using System;

namespace SquareDino
{
    public static class MyVibration
    {
        private static bool _isCanPlayVibro;

        public static void Haptic(MyHapticTypes hapticType)
        {
            if (!Settings.VibrationEnabled) return;
            VibrationHandler.Instance.AddVibration(hapticType);
        }
    }
}