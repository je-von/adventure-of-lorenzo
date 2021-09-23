using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public Transform initialPosition, pickPosition;
    public int clipSize, intialSpareAmmo, fireRate, bulletSpeed, bulletDrop, damage;

    public GameObject weaponObj;

    public Weapon(GameObject weaponObj, GameObject hand, Vector3 position, Vector3 rotation)
    {
        this.weaponObj = weaponObj;

        initialPosition = new GameObject().transform;
        initialPosition.parent = weaponObj.transform.parent;
        initialPosition.localPosition = weaponObj.transform.localPosition;
        initialPosition.localEulerAngles = weaponObj.transform.localEulerAngles;

        pickPosition = new GameObject().transform;
        pickPosition.parent = hand.transform;
        pickPosition.localPosition = position;
        pickPosition.localEulerAngles = rotation;
    }

    public void PickWeapon()
    {
        weaponObj.transform.parent = pickPosition.parent;
        weaponObj.transform.localPosition = pickPosition.localPosition;
        weaponObj.transform.localEulerAngles = pickPosition.localEulerAngles;
    }

    public void PutWeapon()
    {
        weaponObj.transform.parent = initialPosition.parent;
        weaponObj.transform.localPosition = initialPosition.localPosition;
        weaponObj.transform.localEulerAngles = initialPosition.localEulerAngles;
    }
}
