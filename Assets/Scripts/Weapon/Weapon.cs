using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public Transform initialPosition, pickPosition;
    public int intialSpareAmmo, fireRate, bulletSpeed, bulletDrop, damage;

    public GameObject weaponObj;

    public Weapon(GameObject weaponObj, GameObject hand, Vector3 position, Vector3 rotation,
                  int intialSpareAmmo, int fireRate, int bulletSpeed, int bulletDrop, int damage)
    {
        this.intialSpareAmmo = intialSpareAmmo;
        this.fireRate = fireRate;
        this.bulletSpeed = bulletSpeed;
        this.bulletDrop = bulletDrop;
        this.damage = damage;
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

    //public 

    //public Weapon(GameObject weaponObj, GameObject hand, Vector3 position, Vector3 rotation,
    //              int intialSpareAmmo, int fireRate, int bulletSpeed, int bulletDrop, int damage)
    //{
    //    this.weaponObj = weaponObj;

    //    initialPosition = new GameObject().transform;
    //    initialPosition.parent = weaponObj.transform.parent;
    //    initialPosition.localPosition = weaponObj.transform.localPosition;
    //    initialPosition.localEulerAngles = weaponObj.transform.localEulerAngles;

    //    pickPosition = new GameObject().transform;
    //    pickPosition.parent = hand.transform;
    //    pickPosition.localPosition = position;
    //    pickPosition.localEulerAngles = rotation;
    //}

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

    public void RotateWeapon(float mouseY)
    {
        Vector3 v;
        if(this.weaponObj.name == "Primary Weapon")
        {
            v = new Vector3(-mouseY, 0, 0);
        }
        else
        {
            v = new Vector3(0, 0, mouseY);

        }
        weaponObj.transform.Rotate(v);
    }
}
