using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = ("Scriptable Objects/Inventory System/Inventory"))]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItemWrapper> _itemList = null;
    [SerializeField] private InventoryUI _inventoryUIPrefab = null;

    private InventoryUI _inventoryUI = null;
    private InventoryUI InventoryUI
    {
        get
        {
            if(!_inventoryUI)
            {
                _inventoryUI = Instantiate(_inventoryUIPrefab, FindObjectOfType<Canvas>().transform.GetChild(0));
            }

            return _inventoryUI;
        }
    }

    private Dictionary<InventoryItem, int> _itemToCountMap = new Dictionary<InventoryItem, int>();

    public void InitInventory()
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            _itemToCountMap.Add(_itemList[i].GetItem(), _itemList[i].GetItemCount());
        }
    }

    public void OpenInventoryUI()
    {
        _inventoryUI.gameObject.SetActive(true);
        _inventoryUI.InitInventoryUI(this);
    }

    public void AssignItem(InventoryItem item)
    {
        Debug.Log(string.Format("Player assigned {0} item", item.GetName()));
    }

    public Dictionary<InventoryItem, int> GetAllItemsMap()
    {
        return _itemToCountMap;
    }

    public void AddItem(InventoryItem item, int count)
    {
        int currentItemCount = 0;

        if(_itemToCountMap.TryGetValue(item, out currentItemCount))
        {
            _itemToCountMap[item] = currentItemCount + count;
        }
        else
        {
            _itemToCountMap.Add(item, count);
        }

        if(_inventoryUI != null)
        {
            _inventoryUI.CreateOrUpdateSlot(this, item, count);
        }
    }

    public void RemoveItem(InventoryItem item, int count)
    {
        int currentItemCount = 0;

        if (_itemToCountMap.TryGetValue(item, out currentItemCount))
        {
            _itemToCountMap[item] = currentItemCount - count;
            
            if(_inventoryUI != null)
            {
                if(currentItemCount - count <= 0)
                {
                    _inventoryUI.DestroySlot(item);
                }
                else
                {
                    _inventoryUI.UpdateSlot(item, currentItemCount - count);
                }
            }
        
        }
        else
        {
            Debug.Log(string.Format("Can't remove {0} .This item is not in the inventory."));
        }
    }
}