using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IPositionChanger
    {
        void ChangePosition(Vector3 position);
        void ResetPosition(Vector3 position);
    }
}