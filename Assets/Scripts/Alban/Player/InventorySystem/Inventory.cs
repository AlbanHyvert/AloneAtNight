using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = ("Scriptable Objects/Inventory System/Inventory"))]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItemWrapper> _itemList = null;
    [SerializeField] private InventoryUI _inventoryUIPrefab = null;

    private FP_Controller _fpPlayer = null;
    private InventoryUI _inventoryUI = null;
    private InventoryUI InventoryUI
    {
        get
        {
            /*if(!_inventoryUI)
            {
                _inventoryUI = Instantiate(_inventoryUIPrefab, _fpPlayer.GetInventoryUI().transform);
            }*/

            return _inventoryUI;
        }
    }

    private List<InventoryItem> _playerItemList = null;
    private Dictionary<InventoryItem, int> _itemToCountMap = new Dictionary<InventoryItem, int>();

    public void InitInventory(FP_Controller player)
    {
        _fpPlayer = player;

        _playerItemList = new List<InventoryItem>();

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
        item.AssignItemToPlayer(_fpPlayer);
    }

    public Dictionary<InventoryItem, int> GetAllItemsMap()
    {
        return _itemToCountMap;
    }

    public List<InventoryItemWrapper> GetItemWrappers()
    {
        return _itemList;
    }

    public List<InventoryItem> GetPlayerItems()
    {
        return _playerItemList;
    }
    
    public void AddItem(InventoryItem item, int count)
    {
        int currentItemCount = 0;

        _playerItemList.Add(item);

        if (_itemToCountMap.TryGetValue(item, out currentItemCount))
        {
            _itemToCountMap[item] = currentItemCount + count;
        }
        else
        {
            _itemToCountMap.Add(item, count);
            //old place Add player item list
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
                    _playerItemList.Remove(item);
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