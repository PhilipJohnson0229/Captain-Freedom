using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType
{
    Weapon,
    Ammo,
    Key
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Special Attack Vectors")]
    public int weaponType;

    [Header("Craft requirements")]

    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;
    public int stackSize;
    public override string GetDescription()
    {
        return description;
    }



    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
