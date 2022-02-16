using Game.Scripts;
using UnityEngine;

public class WeaponUpper : MonoBehaviour
{
    private Weapon _weapon;
    private void Start() => _weapon = FindObjectOfType<Weapon>();

    public void UpgradeWeapon()
    {
        if (_weapon == null) _weapon = FindObjectOfType<Weapon>();
        _weapon.TryUpgrade();
    }
}