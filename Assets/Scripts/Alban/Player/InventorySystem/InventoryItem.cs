using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private Sprite _itemSprite = null;
    [SerializeField] private string _itemName = null;

    [SerializeField] private Vector3 _itemLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 _itemLocalRotation = Vector3.zero;

    private ObjectItem _objectItem = null;

    public abstract void AssignItemToPlayer(FP_Controller player);

    public ObjectItem GetObjectItem()
    {
        return _objectItem;
    }

    public void SetObjectItem(ObjectItem objectItem)
    {
        _objectItem = objectItem;
    }

    public GameObject GetPrefab()
    {
        return _itemPrefab;
    }

    public void SetPrefab(GameObject obj)
    {
        _itemPrefab = obj;
    }

    public Sprite GetSprite()
    {
        return _itemSprite;
    }

    public string GetName()
    {
        return _itemName;
    }

    public Vector3 GetLocalPosition()
    {
        return _itemLocalPosition;
    }

    public Quaternion GetLocalRotation()
    {
        return Quaternion.Euler(_itemLocalRotation);
    }
}