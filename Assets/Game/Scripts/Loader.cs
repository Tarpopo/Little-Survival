using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}