using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public List<GameObject> weaponVisuals;
    public Dictionary<string, GameObject> weaponVisualsDictianory;
    private ItemData_Equipment currentEquipment;
    [field:SerializeField]public Animator currentWeaponAnim { get; private set; }

    void Start()
    {
        weaponVisuals = new List<GameObject>();
        weaponVisualsDictianory = new Dictionary<string, GameObject>();

        PrepareEquipmentVisuals();
    }

    //Handle subscription and unsubscription to avoid memory leaks
    private void OnEnable()
    {
        Inventory.onWeaponChanged += SwitchEquipmentVisual;
    }

    private void OnDisable()
    {
        Inventory.onWeaponChanged -= SwitchEquipmentVisual;
    }

    private void PrepareEquipmentVisuals()
    {
        //Debug.Log("Building the dictionary");
        foreach (Transform weapon in this.transform)
        {
            weaponVisuals.Add(weapon.gameObject);
            weaponVisualsDictianory.Add(weapon.gameObject.name, weapon.gameObject);
        }
    }

    private void SwitchEquipmentVisual(ItemData_Equipment newEquipment)
    {
        currentEquipment = newEquipment;
    }

    private void PerformVisualSwap(ItemData_Equipment newEquipment)
    {
        //search the dictionary for gameobject by name and if match, activate
        foreach (KeyValuePair<string, GameObject> equipment in weaponVisualsDictianory)
        {
            //weaponVisualName is being compared to the name of the gameobject
            //if (newEquipment.objectVisualName == equipment.Key)
            //{
            //    //turn off all gameobjects in list
            //    foreach (GameObject weapon in weaponVisuals)
            //    {
            //        weapon.SetActive(false);
            //    }

            //    //set the found match to active so we can see the weapon
            //    equipment.Value.gameObject.SetActive(true);
            //    currentWeaponAnim = equipment.Value.gameObject.GetComponent<Animator>();
            //}
        }
    }

    public void ShowHideEquipment(bool show)
    {
        if (show)
        {
            PerformVisualSwap(currentEquipment);
        }
        else
        {
            foreach (GameObject weapon in weaponVisuals)
            {
                weapon.SetActive(false);
            }
        }
       
    }
}
