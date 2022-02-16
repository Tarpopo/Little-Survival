using UI;
using UnityEngine;

public class SceneUI : MonoBehaviour
{
    [SerializeField] private GameWindow gameWindow;
    [SerializeField] private VictoryWindow victoryWindow;
    [SerializeField] private LosingWindow losingWindow;

    public GameWindow GameWindow => gameWindow;
    public VictoryWindow VictoryWindow => victoryWindow;
    public LosingWindow LosingWindow => losingWindow;

    private void Start()
    {
        HideAll();
    }

    private void HideAll()
    {
        gameWindow.Enable(false);
        victoryWindow.Enable(false);
        losingWindow.Enable(false);
    }

    public void SetActiveAllWindow(bool value)
    {
        gameWindow.gameObject.SetActive(value);
        victoryWindow.gameObject.SetActive(value);
        losingWindow.gameObject.SetActive(value);
    }
}
