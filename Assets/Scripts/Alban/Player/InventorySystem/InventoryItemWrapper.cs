using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemWrapper
{
    [SerializeField] private InventoryItem _item = null;
    [SerializeField] private int _count = 0;

    public InventoryItem GetItem()
    {
        return _item;
    }

    public int GetItemCount()
    {
        return _count;
    }
}
