using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHolder : MonoBehaviour
{
    public List<GameObject> ammo;
    public List<AmmoType> projectiles;
    public Dictionary<string, GameObject> ammoDictianory;
    public Transform muzzle;
    private ItemData_Equipment currentAmmo;
    public bool shootsProjectile;
    public float bulletSpeed = 20f;

    void Start()
    {
        ammo = new List<GameObject>();
        ammoDictianory = new Dictionary<string, GameObject>();

        PrepareAmmoVisuals();
    }

    //Handle subscription and unsubscription to avoid memory leaks
    private void OnEnable()
    {
        Inventory.onAmmoChanged += SwitchAmmoVisuals;
    }

    private void OnDisable()
    {
        Inventory.onAmmoChanged -= SwitchAmmoVisuals;
    }

    private void PrepareAmmoVisuals()
    {
        //Debug.Log("Building the dictionary");
        foreach (Transform weapon in this.transform)
        {
            ammo.Add(weapon.gameObject);
            ammoDictianory.Add(weapon.gameObject.name, weapon.gameObject);
        }
    }

    private void SwitchAmmoVisuals(ItemData_Equipment newEquipment)
    {
        currentAmmo = newEquipment;
    }

    private void PerformVisualSwap(ItemData_Equipment newEquipment)
    {
        //search the dictionary for gameobject by name and if match, activate
        foreach (KeyValuePair<string, GameObject> equipment in ammoDictianory)
        {
            //weaponVisualName is being compared to the name of the gameobject
            //if (newEquipment.objectVisualName == equipment.Key)
            //{
            //    //turn off all gameobjects in list
            //    foreach (GameObject weapon in ammo)
            //    {
            //        weapon.SetActive(false);
            //    }

            //    //set the found match to active so we can see the weapon
            //    equipment.Value.gameObject.SetActive(true);
            //}
        }
    }

    public void ShowHideEquipment(bool show)
    {
        if (show)
        {
            PerformVisualSwap(currentAmmo);
        }
        else
        {
            foreach (GameObject weapon in ammo)
            {
                weapon.SetActive(false);
            }
        }

    }

    public void ShootProjectile(string projectileName)
    {
        if (!shootsProjectile) return;

        foreach(AmmoType ammo in projectiles)
        {
            if(ammo.objectVisualName == projectileName)
            {
                GameObject projectile = Instantiate(ammo.projectile, muzzle.position, muzzle.rotation);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.linearVelocity = rb.transform.forward * bulletSpeed;
            }
        }
    }
}

[System.Serializable]
public class AmmoType
{
    public string objectVisualName;
    public GameObject projectile;
}
