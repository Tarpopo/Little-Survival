using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ItemsSaver : MySingleton<ItemsSaver>
{
    private ItemsData _itemsData = new ItemsData();

    public void AddItem(int itemID, ItemState state)
    {
        if (_itemsData.Items.Any(item => item.TrySetItemState(itemID, state))) return;
        _itemsData.Items.Add(new ItemParameters(itemID, state));
        SaveData();
    }

    public ItemState GetItemState(int itemID)
    {
        var itemState = ItemState.Empty;
        _itemsData.Items.ForEach(item => item.TryGetItemState(itemID, ref itemState));
        return itemState;
    }

    public void SetTowerResources(ResourceType type, int count)
    {
        _itemsData.TowerResources[(int) type] = count;
        SaveData();
    }

    public int GetTowerResources(ResourceType type) => _itemsData.TowerResources[(int) type];
    public int GetMoto() => _itemsData.MotocycleResources;
    public void SetMoto(int count)
    {
        _itemsData.MotocycleResources = count;
        SaveData();
    }

    private void Awake() => _itemsData = LoadData();

    //private void OnApplicationQuit() => SaveData();

    //private void OnDestroy() => SaveData();

    public void SaveData() =>
        PlayerPrefs.SetString(Constants.PlayerPrefsKeyNames.ITEMS_DICTIONARY, JsonUtility.ToJson(_itemsData));

    private ItemsData LoadData() => PlayerPrefs.HasKey(Constants.PlayerPrefsKeyNames.ITEMS_DICTIONARY)
        ? JsonUtility.FromJson<ItemsData>(PlayerPrefs.GetString(Constants.PlayerPrefsKeyNames.ITEMS_DICTIONARY))
        : new ItemsData();
}

public class ItemsData
{
    public List<ItemParameters> Items = new List<ItemParameters>();
    public List<int> TowerResources = new List<int>() {0, 0};
    public int MotocycleResources;
}

// [Serializable]
// public class UpgradableItemParameters
// {
//     [SerializeField] private int _itemID; 
//     private List<int> _resources = new List<int>() {0, 0, 0};
//
//     public bool TrySetItemState(int itemID, ResourceType type, int count)
//     {
//         if (_itemID == itemID) _resources[(int)type] = count;
//         return _itemID == itemID;
//     }
//
//     public void TryGetResourceCount(int itemID, ref ItemState state)
//     {
//         if (_itemID != itemID) return;
//         state = _itemState;
//     }
// }

[Serializable]
public class ItemParameters
{
    [SerializeField] private int _itemID;
    [SerializeField] private ItemState _itemState;

    public ItemParameters(int itemID, ItemState state)
    {
        _itemState = state;
        _itemID = itemID;
    }

    public bool TrySetItemState(int itemID, ItemState state)
    {
        if (_itemID == itemID) _itemState = state;
        return _itemID == itemID;
    }

    public void TryGetItemState(int itemID, ref ItemState state)
    {
        if (_itemID != itemID) return;
        state = _itemState;
    }
}

public enum ItemState
{
    Enable,
    Disable,
    EnableWithInclude,
    Empty
}