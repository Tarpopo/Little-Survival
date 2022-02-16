using UnityEngine;

namespace Game.Scripts.Interfaces
{
    public interface IDamageable
    {
        Transform Target { get; }
        WeaponTypes Item { get; }
        bool IsDeath { get; }
        void TakeDamage(Vector3 position, int damage);
    }
}