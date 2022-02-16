using UnityEngine;
public class ForwardMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private void FixedUpdate() => transform.position += transform.forward * _moveSpeed;
}