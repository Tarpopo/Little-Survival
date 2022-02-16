using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _oldLevelDelay;
    private void Start() => _levelManager = FindObjectOfType<LevelManager>();
    public void LoadScene(int sceneIndex) => _levelManager.LoadLevel(sceneIndex);
    public void LoadSceneWithOldDelay(int sceneIndex) => _levelManager.LoadLevel(sceneIndex, _oldLevelDelay);
}