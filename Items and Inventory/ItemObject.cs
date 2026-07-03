using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Sprite icon;
    //public UI ui;
    public Transform camPos;
    public Transform camTarg;

    private void Awake()
    {
        SetupItem(itemData, Vector2.zero);
    }

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        //this is a stand in here.  we need to set the 3d model of the object by grabbing it from the itemData
        //GetComponent<MeshFilter>().mesh = itemData.itemPrefab;
        //icon = itemData.itemIcon;
        gameObject.name = "Item object - " + itemData.itemName;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.linearVelocity = _velocity;

        //SetupVisuals();
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem())
        {
            rb.linearVelocity = new Vector2(0, 7);
            return;
        }

        AudioManager.instance.PlaySFX(7, null);
        Inventory.instance.AddItem(itemData);
        //Destroy(gameObject);

        //if (itemData.itemType == ItemType.Note) 
        //{
        //    //ui.ReadNote(itemData);
        //}

    }

    public ItemData GetData() => itemData;
}
