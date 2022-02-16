using System.Linq;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        _canvas.worldCamera = FindObjectsOfType<Camera>().FirstOrDefault(element => element.CompareTag("UICamera"));
    }
}