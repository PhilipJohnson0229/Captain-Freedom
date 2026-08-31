using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class SnapZoneActions : GrabbableEvents
{
    [SerializeField] ItemData currentItem;
    [SerializeField] SnapZone snapParent;

    /// <summary>
    public ItemData CurrentItem { get { return currentItem; } }

    public override void OnSnapZoneEnter()
    {
        GameManager.instance.Log($"Trying to add an item to the inventory");

        //AddToInventory();
    }

    public override void OnSnapZoneExit()
    {
        GameManager.instance.Log($"Trying to remove an item to the inventory");

        //RemoveFromInventory();
    }

    public void AddToInventory()
    {
        //this doesnt make sense but we can make use of it when the time comes to be more specific
        //right now this is always true
        if (!Inventory.instance.CanAddItem())
        {
            return;
        }
        
        if(snapParent.HeldItem.TryGetComponent(out EquipmentItem value))
        {
            GameManager.instance.Log($"Trying to add {value.itemData.itemName} to the inventory");
            currentItem = value.itemData;
            //Inventory.instance.AddItem(currentItem);
        }

        //AudioManager.instance.PlaySFX(0, null);
        
        //Extensions.RunActions(giveActions);
    }

    public void RemoveFromInventory()
    {
        GameManager.instance.Log("Trying to remove this item");
        //AudioManager.instance.PlaySFX(0, null);
        //Inventory.instance.RemoveItem(currentItem, false);
        //Extensions.RunActions(takeActions);
    }
}
