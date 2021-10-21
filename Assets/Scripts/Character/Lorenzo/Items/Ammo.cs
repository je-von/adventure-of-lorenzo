using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    public Ammo(MonoBehaviour mono) : base(mono, Resources.Load<Sprite>("AMMO_SPRITE"))
    {
    }

    public override void UseItem()
    {
        Lorenzo.GetInstance().primaryWeapon.spareAmmo += 30;
        Lorenzo.GetInstance().secondaryWeapon.spareAmmo += 30;

    }
}
