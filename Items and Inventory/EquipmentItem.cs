using System;
using UnityEngine;

//We will use this class to determine the activation status of gameobjects in the scene that can be interacted with
public class EquipmentItem : MonoBehaviour
{
    [field: SerializeField] public bool activated { get; private set; }

    [field: SerializeField] public string eId { get; private set; }

    [field: SerializeField] public ItemData itemData { get; private set; }

    public Actions[] actions;

    public EquipmentType equipmentType;

    [ContextMenu("Generate Grabbable ID")]
    public void GenerateId()
    {
        eId = Guid.NewGuid().ToString();
    }

}