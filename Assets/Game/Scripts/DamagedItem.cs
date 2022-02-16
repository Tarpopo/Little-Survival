using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamagedItem : MonoBehaviour
{
    private void Start() => GetComponent<Health>().ResetHealth();
}