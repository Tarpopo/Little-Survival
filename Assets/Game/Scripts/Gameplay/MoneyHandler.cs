using System;
using UnityEngine;

public static class MoneyHandler
{
    public static event Action<int> OnValueAdded;
    public static event Action<int> OnValueSubtracted;
    public static event Action<int> OnValueChanged;
    
    private static IntDataValueSavable moneyData = new IntDataValueSavable("MoneyDataKey");
    public static IntDataValueSavable MoneyData => moneyData;

    public static void AddMoney(int addedValue)
    {
        if(addedValue < 0)
            throw new ArgumentOutOfRangeException((string) null, "ArgumentOutOfRange_BadAddedValue");
        
        moneyData.Value += addedValue;
        OnValueAdded?.Invoke(addedValue);
        OnValueChanged?.Invoke(moneyData.Value);
    }

    public static void SubtractMoney(int subtractValue)
    {
        if(subtractValue > 0)
            throw new ArgumentOutOfRangeException((string) null, "ArgumentOutOfRange_BadSubtractedValue");

        if((moneyData.Value - subtractValue) < 0)
            
        
        moneyData.Value -= subtractValue;
        OnValueSubtracted?.Invoke(subtractValue);
        OnValueChanged?.Invoke(moneyData.Value);
    }

    public static void SaveMoneyData()
    {
        moneyData.Save();
    }
}