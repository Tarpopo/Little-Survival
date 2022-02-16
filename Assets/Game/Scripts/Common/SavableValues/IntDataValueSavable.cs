using UnityEngine;

public class IntDataValueSavable : DataValueSavable<int>
{
    public IntDataValueSavable(string saveKey) : base(saveKey){}
    
    protected override void Load()
    {
        base.Load();
        _value = PlayerPrefs.GetInt("DataValue " + SaveKey, 0);
    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetInt("DataValue " + SaveKey, _value);
    }
}