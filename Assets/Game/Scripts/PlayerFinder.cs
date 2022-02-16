using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private GameObject _player;
    private void Start() => _player = FindObjectOfType<Player>().gameObject;

    public void SetPlayer(bool isActive) => _player.SetActive(isActive);
}