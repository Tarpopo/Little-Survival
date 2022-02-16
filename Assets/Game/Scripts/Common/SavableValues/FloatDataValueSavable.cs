using UnityEngine;

public class FloatDataValueSavable : DataValueSavable<float>
{
    public FloatDataValueSavable(string saveKey) : base(saveKey){}
    
    protected override void Load()
    {
        base.Load();
        _value = PlayerPrefs.GetFloat("DataValue " + SaveKey, 0f);
    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetFloat("DataValue " + SaveKey, _value);
    }
}