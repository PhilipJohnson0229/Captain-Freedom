using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActions : Actions
{
    [SerializeField] bool giveItem; //this will decide wether we are giving or receiving the item
    [SerializeField] Actions[] giveActions, takeActions;
    //[SerializeField] UI ui;
    [SerializeField] ItemData currentItem;

    /// <summary>
    public ItemData CurrentItem { get { return currentItem; } }



    public override void Act()
    {
        PickupItem();
       
    }

    public void PickupItem()
    {
        Debug.Log("Trying to get this item");
        if (!Inventory.instance.CanAddItem())
        {
            return;
        }

        if (giveItem)
        {
            AudioManager.instance.PlaySFX(0, null);
            Inventory.instance.AddItem(currentItem);
            Extensions.RunActions(giveActions);
        }
        else
        {
            AudioManager.instance.PlaySFX(0, null);
            Inventory.instance.RemoveItem(currentItem, false);
            Extensions.RunActions(takeActions);
        }

        //if (currentItem.itemType == ItemType.Note)
        //{
        //    //ui.ReadNote(currentItem);
        //    AudioManager.instance.PlaySFX(7, null);
        //}

    }
}
