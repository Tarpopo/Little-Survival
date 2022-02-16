using UnityEngine;

public class StringDataValueSavable : DataValueSavable<string> 
{
    public StringDataValueSavable(string saveKey) : base(saveKey){}
    
    protected override void Load()
    {
        base.Load();
        _value = PlayerPrefs.GetString("DataValue " + SaveKey, "");
    }

    public override void Save()
    {
        base.Save();
        PlayerPrefs.SetString("DataValue " + SaveKey, _value);
    }
}