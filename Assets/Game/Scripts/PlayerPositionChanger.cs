using Game.Scripts.Interfaces;
using UnityEngine;

public class PlayerPositionChanger : MonoBehaviour
{
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Vector3 _startPosition;
    private IPositionChanger _positionChanger;
    
    public void ChangePosition()
    {
        _positionChanger = FindObjectOfType<Player>().GetComponent<IPositionChanger>();
        _positionChanger.ChangePosition(_targetPosition.position);
    }

    public void ResetPosition()
    {
        _positionChanger.ResetPosition(_startPosition);
    }
}