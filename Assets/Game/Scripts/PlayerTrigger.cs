using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onTriggerEnter;
    [SerializeField] private UnityEvent _onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _onTriggerEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _onTriggerExit?.Invoke();
    }
}