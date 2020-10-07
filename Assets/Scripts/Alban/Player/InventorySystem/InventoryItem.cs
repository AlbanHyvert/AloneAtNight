using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName = "Scriptable Objects/Inventory System/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private GameObject _itemPrefab = null;
    [SerializeField] private Sprite _itemSprite = null;
    [SerializeField] private string _itemName = null;

    [SerializeField] private Vector3 _itemLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 _itemLocalRotation = Vector3.zero;

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