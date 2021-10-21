using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    public Ammo() : base(Resources.Load<Sprite>("AMMO_SPRITE"))
    {
    }

    public override void UseItem()
    {
        Lorenzo.GetInstance().primaryWeapon.maxAmmo += 30;
        Lorenzo.GetInstance().secondaryWeapon.maxAmmo += 30;

    }
}
