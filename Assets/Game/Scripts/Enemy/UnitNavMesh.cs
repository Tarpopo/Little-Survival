using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class UnitNavMesh
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public void SetDestination(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
        //_navMeshAgent.acceleration = 500;
        //_navMeshAgent.angularSpeed = 500;
    }

    public void Move(Vector3 position)
    {
        //_navMeshAgent.Move(position);
        _navMeshAgent.Warp(position);
    }

    public void ResetDestination()
    {
        _navMeshAgent.ResetPath();
        //_navMeshAgent.acceleration = 0;
        //_navMeshAgent.angularSpeed = 0;
    }

    public void SetAcceleration(bool active)
    {
        var value = active ? 500 : 0;
        _navMeshAgent.acceleration = value;
        _navMeshAgent.angularSpeed = value;
    }

    public void SetSpeed(float speed) => _navMeshAgent.speed = speed;
}