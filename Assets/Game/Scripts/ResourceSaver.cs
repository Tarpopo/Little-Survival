using UnityEngine;

public class ResourceSaver
{
    public static void SaveResource(ResourceType type, int value) => PlayerPrefs.SetInt(type.ToString(), value);

    public static int GetResource(ResourceType type) => PlayerPrefs.GetInt(type.ToString(), 0);
}