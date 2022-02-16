using Game.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class Weapon : MonoBehaviour, IUpgradable
    {
        [SerializeField] private UnityEvent _onUpgrade;

        public event UnityAction OnUpgrade
        {
            add => _onUpgrade.AddListener(value);
            remove => _onUpgrade.RemoveListener(value);
        }

        public Vector3 UpgradePoint => _upgradePoint.position;
        [SerializeField] private int _damage = 15;
        [SerializeField] private int _maxDamageAdd = 25;
        [SerializeField] private WeaponSwitcher _switcher;
        [SerializeField] private Transform _upgradePoint;
        private int _startDamage;

        private void Awake() => _startDamage = _damage;

        public int Damage => _damage;

        public void ResetWeapon()
        {
            _damage = _startDamage;
            _switcher?.ResetAllLevels();
        }

        public void SwitchWeapon(WeaponTypes type) => _switcher?.ActiveWeapon(type);

        public bool TryUpgrade()
        {
            _damage += _maxDamageAdd;
            //_switcher.UpgradeWeapon(WeaponTypes.Sword);
            _onUpgrade?.Invoke();
            return true;
        }
    }
}