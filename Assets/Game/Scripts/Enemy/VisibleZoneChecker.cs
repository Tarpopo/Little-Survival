using System;
using Game.Scripts.Interfaces;
using UnityEngine;

namespace Game.Scripts.Enemy
{
    [Serializable]
    public class VisibleZoneChecker
    {
        [SerializeField] private float _visibleAngle;
        [SerializeField] private float _minTriggerRadius;
        [SerializeField] private float _closeRadius;
        [SerializeField] private Transform _currentPoint;
        [SerializeField] private Transform _target;
        private IDamageable _damageable;
        public IDamageable GetDamageable() => _damageable ??= _target.GetComponent<IDamageable>();

        public bool IsTargetExist => _target != null;
        public Vector3 TargetPosition => _target.position;
        public bool IsCloseDistance => Vector3.Distance(_currentPoint.position, _target.position) <= _closeRadius;
        public Vector3 TargetDirection => (_target.position - _currentPoint.position).normalized;

        public void SetTarget(Transform target)
        {
            if (IsTargetExist) return;
            _target = target;
        }

        public bool CheckTarget()
        {
            // return IsTargetExist && (IsTargetDistance &&
            //                          Vector3.Angle(_currentPoint.forward, _target.position - _currentPoint.position) <=
            //                          _visibleAngle);
            return IsTargetExist && IsTargetDistance;
        }

        public bool IsTargetDistance => Vector3.Distance(_currentPoint.position, _target.position) <= _minTriggerRadius;
    }
}