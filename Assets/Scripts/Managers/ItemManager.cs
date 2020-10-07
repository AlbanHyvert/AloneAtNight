using Engine.Singleton;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private Transform _itemWorldPrefab = null;


    public Transform ItemWorldPrefab { get { return _itemWorldPrefab; } set { _itemWorldPrefab = value; } }
}