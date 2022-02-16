using UnityEngine;
using System.Collections;

public class DontDestroyOnLoadGameObject : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}