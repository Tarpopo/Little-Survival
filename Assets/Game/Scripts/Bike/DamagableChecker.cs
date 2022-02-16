using Game.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Bike
{
    public class DamagableChecker : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGetDamageable;
        [SerializeField] private UnityEvent _onApplyDamage;
        [SerializeField] private int _damageLayerIndex;
        [SerializeField] private bool _autoDamage;

        public event UnityAction OnGetDamageable
        {
            add => _onGetDamageable.AddListener(value);
            remove => _onGetDamageable.RemoveListener(value);
        }

        private IDamageable _current;

        public void TryApplyDamage(int damage)
        {
            _current?.TakeDamage(transform.position, damage);
            _current = null;
            _onApplyDamage?.Invoke();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != _damageLayerIndex) return;
            other.collider.GetComponent<IDamageable>().TakeDamage(transform.position, 2);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _damageLayerIndex || _current != null) return;
            _current = other.GetComponent<IDamageable>();
            if (_current == null) return;
            _onGetDamageable?.Invoke();
            if (_autoDamage)
            {
                _current.TakeDamage(transform.position, 1);
                _current = null;
            }
        }
    }
}