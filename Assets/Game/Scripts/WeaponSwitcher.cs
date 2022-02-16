using System;
using UnityEngine;

[Serializable]
public class WeaponSwitcher
{
    [SerializeField] private WeaponSkin[] _skins;
    private WeaponSkin _currentSkin;

    public void ResetAllLevels()
    {
        foreach (var weaponSkin in _skins) weaponSkin.ResetLevel();
        _currentSkin?.ActivateWeapon(false);
        _currentSkin = _skins[(int) WeaponTypes.Arm];
        _currentSkin.ActivateWeapon();
    }

    public void ActiveWeapon(WeaponTypes type)
    {
        if (_currentSkin.WeaponType == _skins[(int) type].WeaponType) return;
        _currentSkin.ActivateWeapon(false);
        _currentSkin = _skins[(int) type];
        _currentSkin.ActivateWeapon();
    }

    public void UpgradeWeapon(WeaponTypes type)
    {
        _currentSkin.ActivateWeapon(false);
        _currentSkin = _skins[(int) type];
        _currentSkin.UpgradeWeapon();
    }
}

[Serializable]
public class WeaponSkin
{
    public WeaponTypes WeaponType => _weaponType;
    [SerializeField] private WeaponTypes _weaponType;
    [SerializeField] private GameObject[] _levelPrefabs;
    private int _level;

    public void ResetLevel() => _level = 0;

    public void UpgradeWeapon()
    {
        if (_level + 1 >= _levelPrefabs.Length) return;
        _level++;
        _levelPrefabs[_level - 1].SetActive(false);
        _levelPrefabs[_level].SetActive(true);
    }

    public void ActivateWeapon(bool isActive = true)
    {
        _levelPrefabs[_level].SetActive(isActive);
    }
}