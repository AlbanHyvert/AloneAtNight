using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Inventory System/Items/Object Item")]
public class ObjectItem : InventoryItem
{
    public override void AssignItemToPlayer(FP_Controller player)
    {
       // player.CreateObjectInstance(this);
    }
}
