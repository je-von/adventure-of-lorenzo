using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoInfoController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI weaponNameText, ammoText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string name;
        Weapon currentWeapon;

        if (Lorenzo.GetInstance().primaryWeapon.isUsed)
        {
            currentWeapon = Lorenzo.GetInstance().primaryWeapon;
            name = "PRIMARY WEAPON";
        }
        else
        {
            currentWeapon = Lorenzo.GetInstance().secondaryWeapon;
            name = "SECONDARY WEAPON";
        }

        weaponNameText.text = name;
        ammoText.text = currentWeapon.currentAmmo + " | " + currentWeapon.maxAmmo;
    }
}
