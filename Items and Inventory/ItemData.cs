using System.Text;
using UnityEngine;
using BNG;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Expendable,
    Immortal
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Grabbable itemPrefab;
    public string itemId;
    public string snapZoneId;
    public int buyerValue;
    public int sellerValue;
    public string description;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        //grab this item and create its itemId based on its position in the directory
        string path = AssetDatabase.GetAssetPath(this);
        //The items absolute path is used to create a unique GUID
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public void setSnapZoneId(string newId)
    {
        snapZoneId = newId;
    }
}
