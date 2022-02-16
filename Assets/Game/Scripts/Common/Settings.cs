using UnityEngine;

public static class Settings
{
    private const string VIBRATION = "Vibration";

    public static bool VibrationEnabled
    {
        get => PlayerPrefs.GetInt(VIBRATION, 1) == 1;
        set => PlayerPrefs.SetInt(VIBRATION, value ? 1 : 0);
    }
}