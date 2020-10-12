using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform _slotParent = null;
    [SerializeField] private InventorySlot _slotPrefab = null;

    private Dictionary<InventoryItem, InventorySlot> _itemToSlotMap = new Dictionary<InventoryItem, InventorySlot>();

    public void InitInventoryUI(Inventory inventory)
    {
        Dictionary<InventoryItem,int> itemsMap = inventory.GetAllItemsMap();

        foreach (KeyValuePair<InventoryItem, int> kvp in itemsMap)
        {
            CreateOrUpdateSlot(inventory, kvp.Key, kvp.Value);
        }
    }

    public void CreateOrUpdateSlot(Inventory inventory, InventoryItem item, int itemCount)
    {
        if(!_itemToSlotMap.ContainsKey(item))
        {
            InventorySlot slot = CreateSlot(inventory, item, itemCount);
            _itemToSlotMap.Add(item, slot);
        }
        else
        {
            UpdateSlot(item, itemCount);
        }
    }

    public void UpdateSlot(InventoryItem item, int itemCount)
    {
        _itemToSlotMap[item].UpdateSlotCount(itemCount);
    }
    
    private InventorySlot CreateSlot(Inventory inventory, InventoryItem item, int itemCount)
    {
        InventorySlot slot = Instantiate(_slotPrefab, _slotParent);
        slot.InitSlotVisualisation(item.GetSprite(), item.GetName(), itemCount);
        slot.AssignSlotButtonCallback(() => inventory.AssignItem(item));

        return slot;
    }

    public void DestroySlot(InventoryItem item)
    {
        Destroy(_itemToSlotMap[item].gameObject);
        _itemToSlotMap.Remove(item);
    }
}
