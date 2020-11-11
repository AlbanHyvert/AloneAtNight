using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = ("Scriptable Objects/Inventory System/Inventory"))]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItem> _itemList = new List<InventoryItem>();

    private FP_Controller _fpPlayer = null;

    public void InitInventory(FP_Controller player)
    {
        _fpPlayer = player;

        _itemList.Clear();
    }

    public List<InventoryItem> GetInventories()
        => _itemList;

    public void AddItem(InventoryItem item)
        => _itemList.Add(item);

    public void RemoveItem(InventoryItem item)
        => _itemList.Remove(item);

    public void Clear()
    {
        _itemList.Clear();
        _fpPlayer = null;
    }
}