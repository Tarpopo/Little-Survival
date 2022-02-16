using UnityEngine;

public static class Statistics
{
    public static int PlayerLevel
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.PlayerPrefsKeyNames.PLAYER_LEVEL, 1);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsKeyNames.PLAYER_LEVEL, value);
            PlayerPrefs.Save();
        }
    }
   
    public static int CurrentLevelNumber
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.PlayerPrefsKeyNames.CURRENT_LEVEL_NUMBER, 0);
        }
        set
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsKeyNames.CURRENT_LEVEL_NUMBER, value);
            PlayerPrefs.Save();
        }
    }

    public static bool AllLevelsCompleted
    {
        get
        {
            return PlayerPrefs.GetInt(Constants.PlayerPrefsKeyNames.ALL_LEVELS_COMPLETED, 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt(Constants.PlayerPrefsKeyNames.ALL_LEVELS_COMPLETED, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}