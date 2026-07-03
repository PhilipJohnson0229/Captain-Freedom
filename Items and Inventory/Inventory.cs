using BNG;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> notes;
    public Dictionary<ItemData, InventoryItem> notesDictionary;



    [Header("Inventory UI")]

    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform notesSlotParent;

    //private UI_ItemSlot[] inventoryItemSlot;
    //private UI_ItemSlot[] notesItemSlot;
    [field: SerializeField] public SnapZone[] equipmentSlot { get; private set; }

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

    //observer pattern
    public static event Action<ItemData_Equipment> onWeaponChanged;
    public static event Action<ItemData_Equipment> onAmmoChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();

        notes = new List<InventoryItem>();
        notesDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        //equpmentSlots = equpmentSlotParent.GetComponentsInChildren<SnapZone>();
        //inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        //notesItemSlot = notesSlotParent.GetComponentsInChildren<UI_ItemSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item, item.stackSize);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }


        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].equipmentType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        ////these two methods clean up the UI to ensure reflection of accuarate inventory
        //for (int i = 0; i < inventoryItemSlot.Length; i++)
        //{
        //    inventoryItemSlot[i].CleanUpSlot();
        //}

        //for (int i = 0; i < notesItemSlot.Length; i++)
        //{
        //    notesItemSlot[i].CleanUpSlot();
        //}


        //for (int i = 0; i < inventory.Count; i++)
        //{
        //    inventoryItemSlot[i].UpdateSlot(inventory[i]);
        //}

        //for (int i = 0; i < notes.Count; i++)
        //{
        //    notesItemSlot[i].UpdateSlot(notes[i]);
        //}

    }


    public void AddItem(ItemData _item, int stackSize = 1)
    {
        Debug.Log($"added {_item.itemName} to inventory");
        if (CanAddItem())
            AddToInventory(_item, stackSize);
        //else if (_item.itemType == ItemType.Note)
        //    AddToNotes(_item);

        UpdateSlotUI();
    }

    private void AddToNotes(ItemData _item)
    {
        if (notesDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            notes.Add(newItem);
            notesDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item, int stackSize)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
            Debug.Log($"This item {_item.itemName} was already in the inventory dictionary");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = stackSize;
            inventory.Add(newItem);
            inventoryDictianory.Add(_item, newItem);
            Debug.Log($"This item {_item.itemName} was just added to the inventory dictionary");
        }
    }

    public void RemoveItem(ItemData _item, bool all)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1 && !all)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else if (value.stackSize > 1 && !all)
            {
                value.RemoveStack();
            }
            else if (value.stackSize > 1 && all)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
                
        }


        if (notesDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                notes.Remove(stashValue);
                notesDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        //if (inventory.Count >= inventoryItemSlot.Length)
        //{
        //    return false;
        //}

        return true;
    }

    public void EquipItem(ItemData _item, int stackSize)
    {
        //this temp veriable is here to help with duplication of equipped items
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            if(oldEquipment.equipmentType == EquipmentType.Ammo)
            {
                UnequipItem(oldEquipment, true);
            }
            else
            {
                UnequipItem(oldEquipment, false);
            }
            
            //return the unequipped item to the inventory
            AddItem(oldEquipment);
        }

        if (equipmentDictionary.TryGetValue(newEquipment, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            newItem.stackSize = stackSize;
            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
        }

        //equipment.Add(newItem);
        //equipmentDictionary.Add(newEquipment, newItem);

        if (newEquipment.equipmentType == EquipmentType.Ammo)
        {
            RemoveItem(_item, true);
        }
        else
        {
            RemoveItem(_item, false);
        }

        

        UpdateSlotUI();

        //this is where we need to trigger this event


        if (newEquipment.equipmentType == EquipmentType.Weapon)
        {
            onWeaponChanged?.Invoke(newEquipment);
        }

        //need to call thisevent if we change the ammo type
        if(newEquipment.equipmentType == EquipmentType.Ammo)
        {
            onAmmoChanged?.Invoke(newEquipment);
        }

    }

    public void UnequipItem(ItemData_Equipment itemToRemove, bool all)
    {
        //TODO here we can turn off the 3d model of the item were unequipping
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            if (value.stackSize <= 1 && !all)
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(itemToRemove);
            }
            else if (value.stackSize > 1 && !all)
            {
                value.RemoveStack();
            }
            else if (value.stackSize > 1 && all)
            {
                equipment.Remove(value);
                equipmentDictionary.Remove(itemToRemove);
            }
        }
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        // Check if all required materials are avalible with the required quantity.

        foreach (var requiredItem in _requiredMaterials)
        {
            if (notesDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requiredItem.stackSize)
                {
                    Debug.Log("Not enough materials: " + requiredItem.data.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("Materials not found in stash: " + requiredItem.data.name);
                return false;
            }
        }

        // If all materials are avalible, remove them from stash.

        foreach (var requiredMaterial in _requiredMaterials)
        {
            for (int i = 0; i < requiredMaterial.stackSize; i++)
            {
                RemoveItem(requiredMaterial.data, false);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("Craft is succsesful: " + _itemToCraft.name);
        return true;
    }


    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => notes;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }

    public ItemData GetItem(ItemData passedItem)
    {
        if (inventoryDictianory.TryGetValue(passedItem, out InventoryItem value))
        {
            if(value.data == passedItem)
            {
                return value.data;
            }
        }

        return null;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (KeyValuePair<string, int> pair in _data.equipment)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    ItemData_Equipment equipment = item as ItemData_Equipment;
                    equipment.stackSize = pair.Value;
                    loadedEquipment.Add(equipment);
                }
            }
        }



        //foreach (KeyValuePair<string, string> pair in _data.snapZoneIds)
        //{
        //    foreach (var item in itemDataBase)
        //    {
        //        if (item != null && item.itemId == pair.Key)
        //        {
        //            InventoryItem itemToLoad = new InventoryItem(item);
        //            itemToLoad.stackSize = pair.Value;
        //            loadedItems.Add(itemToLoad);
        //        }
        //    }
        //}
    }

    public void SaveData(GameData _data)
    {
        _data.inventory.Clear();
        _data.equipment.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictianory)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in notesDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipment.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        _data.snapZoneIds.Clear();

        foreach (SnapZone slot in equipmentSlot)
        {
            _data.snapZoneIds.Add(slot.sId, slot.HeldItem ? slot.HeldItem.GetComponent<EquipmentItem>().itemData.itemId : "");
            if (slot.HeldItem)
            {
                slot.HeldItem.GetComponent<EquipmentItem>().itemData.setSnapZoneId(slot.sId);
            }
        }

    }



#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif


}
