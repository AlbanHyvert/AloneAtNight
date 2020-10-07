using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage = null;
    [SerializeField] private TextMeshProUGUI _itemNameText = null;
    [SerializeField] private TextMeshProUGUI _itemCountText = null;
    [SerializeField] private Button _slotButton = null;

    public void InitSlotVisualisation(Sprite itemSprite, string itemName, int itemCount)
    {
        _itemImage.sprite = itemSprite;
        _itemNameText.text = itemName;
        UpdateSlotCount(itemCount);
    }

    public void UpdateSlotCount(int itemCount)
    {
        _itemCountText.text = itemCount.ToString();
    }

    public void AssignSlotButtonCallback(System.Action onClickCallback)
    {
        _slotButton.onClick.AddListener(() => onClickCallback());
    }
}
